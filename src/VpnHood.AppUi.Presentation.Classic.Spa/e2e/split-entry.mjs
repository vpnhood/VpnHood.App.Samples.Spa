// split-entry.mjs — a split tunneling list's refused entry, said in the person's language. The app
// refuses `*.example.com` in a domain list with a SplitEntryException whose data carries the entry and
// why (Entry, Reason); the SPA words it itself (ErrorHandler's SplitEntry case), keeps the page open
// and saves nothing, as the app saves all its lists or none.
//
// The app's own message is replaced on the way back, its data kept, so the dialog shows the SPA's
// words only if the SplitEntry case reads the data: in English the two say the same.
//
// The one mutation: split tunneling and Split Domains are switched on for the page to take input,
// and the user settings are put back as they were in finally.
//
// Prereqs: the app is running and `npm run dev` serves the SPA, as for smoke.mjs.
//   VH_E2E_APP   app + API origin      (default: VITE_API_BASE_URL from the environment or .env.development(.local))
//   VH_E2E_SPA   SPA origin            (default: http://localhost:8080)
// Run: node e2e/split-entry.mjs
import { chromium } from 'playwright';
import { readFileSync, existsSync } from 'node:fs';
import { fileURLToPath } from 'node:url';
import path from 'node:path';

const root = path.dirname(path.dirname(fileURLToPath(import.meta.url)));
const en = JSON.parse(readFileSync(path.join(root, 'src/locales/en.json'), 'utf8'));

// the SPA in the browser talks to VITE_API_BASE_URL; drive the same app from here
const envApi = [process.env.VITE_API_BASE_URL?.trim(), ...['.env.development.local', '.env.development']
  .map(f => path.join(root, f))
  .filter(existsSync)
  .map(f => readFileSync(f, 'utf8').match(/^VITE_API_BASE_URL=(.+)$/m)?.[1]?.trim())]
  .find(Boolean);
const APP = process.env.VH_E2E_APP ?? envApi;
const SPA = process.env.VH_E2E_SPA ?? 'http://localhost:8080';
if (!APP)
  throw new Error('VITE_API_BASE_URL is not set in the environment or .env.development(.local): the test needs the app API origin.');

const entry = '*.example.com';
const expected = en.SPLIT_DOMAIN_ASTERISK_ERROR.replace('{entry}', entry);

async function api(method, route, body) {
  const response = await fetch(`${APP}/api/app/${route}`, {
    method,
    headers: body === undefined ? undefined : { 'Content-Type': 'application/json' },
    body: body === undefined ? undefined : JSON.stringify(body)
  });
  if (!response.ok)
    throw new Error(`${method} ${route}: ${response.status} ${await response.text()}`);
  const text = await response.text();
  return text ? JSON.parse(text) : undefined;
}

const failures = [];
function check(condition, what) {
  console.log(`${condition ? 'ok  ' : 'FAIL'} ${what}`);
  if (!condition) failures.push(what);
}

const savedSettings = (await api('GET', 'info')).userSettings;
const savedDomains = await api('GET', 'split-by-domains');
const browser = await chromium.launch();
try {
  await api('PUT', 'user-settings', {
    ...savedSettings,
    splitTunneling: { ...savedSettings.splitTunneling, enabled: true, useDomain: true }
  });

  const page = await browser.newPage({ locale: 'en-US', viewport: { width: 420, height: 900 } });
  await page.route('**/api/app/split-by-domains', async route => {
    if (route.request().method() !== 'PUT')
      return route.continue();
    const response = await route.fetch();
    const body = await response.json();
    body.message = 'The app own message';
    await route.fulfill({ response, body: JSON.stringify(body) });
  });

  // in through the app, so the back button stays in it and runs the page's leave guard, which saves
  await page.goto(`${SPA}/split-tunneling`);
  await page.waitForLoadState('networkidle');
  await page.getByText(en.SPLIT_DOMAINS, { exact: true }).first().click();
  await page.waitForURL('**/split-tunneling/split-domains');
  await page.waitForLoadState('networkidle');

  await page.locator('textarea:visible').first().fill(entry);
  await page.getByRole('button').first().click();

  const dialog = page.getByText(expected, { exact: true });
  await dialog.waitFor({ timeout: 10000 }).catch(() => undefined);
  check(await dialog.isVisible(), `the dialog says: ${expected}`);
  check(new URL(page.url()).pathname === '/split-tunneling/split-domains', 'the page stays open');

  const domains = await api('GET', 'split-by-domains');
  check(JSON.stringify(domains) === JSON.stringify(savedDomains), 'no list was saved');
} finally {
  await browser.close();
  await api('PUT', 'user-settings', savedSettings);
}

console.log(failures.length ? `\n${failures.length} failure(s).` : '\nAll passed.');
process.exit(failures.length ? 1 : 0);
