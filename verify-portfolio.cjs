const { chromium } = require('playwright');
const fs = require('fs');
const path = require('path');
const {pathToFileURL} = require('url');
(async()=>{
 const browser = await chromium.launch({channel:'msedge',headless:true});
 const page = await browser.newPage();
 const errors=[]; page.on('pageerror',e=>errors.push(e.message));
 const files=['index.html','chrisWebDev.html','resume.html','brandon-hope.html','enterprise.html','benda-cpr.html','chrisFootball.html','chrisSoccer.html'];
 for(const width of [1440,768,390,320]){
  await page.setViewportSize({width,height:1000});
  for(const file of files){
   await page.goto(pathToFileURL(path.resolve(file)).href);
   await page.evaluate(()=>document.fonts.ready);
   const result=await page.evaluate(()=>({overflow:document.documentElement.scrollWidth>innerWidth,images:[...document.images].every(i=>i.complete&&i.naturalWidth>0),links:[...document.querySelectorAll('a[href]')].map(a=>a.getAttribute('href'))}));
   if(result.overflow||!result.images)throw Error(`${file} at ${width}: ${JSON.stringify(result)}`);
   for(const link of result.links){if(!/^(https?:|mailto:)/.test(link)){const [target,hash]=link.split('#');if(target&&!fs.existsSync(target))throw Error('Missing '+target);if(!target&&hash&&!(await page.locator('[id="'+hash+'"]').count()))throw Error('Missing anchor '+link)}}
   if(file==='index.html'&&[1440,390].includes(width)){fs.mkdirSync('tmp',{recursive:true});await page.screenshot({path:`tmp/portfolio-${width}.png`,fullPage:true});}
  }
 }
 await page.goto(pathToFileURL(path.resolve('index.html')).href);
 await page.locator('.menu-toggle').click();
 if(await page.locator('.menu-toggle').getAttribute('aria-expanded')!=='true')throw Error('Menu did not open');
 await page.keyboard.press('Escape');
 if(await page.locator('.menu-toggle').getAttribute('aria-expanded')!=='false')throw Error('Menu did not close');
 const visibleSlide=()=>page.locator('.gallery-slide:not([hidden])');
 await page.locator('[data-gallery-next]').click();
 if(await visibleSlide().getAttribute('aria-label')!=='2 of 5')throw Error('Next slide failed');
 await page.locator('.photo-gallery').focus();
 await page.keyboard.press('ArrowLeft');
 if(await visibleSlide().getAttribute('aria-label')!=='1 of 5')throw Error('Keyboard slider failed');
 await page.locator('[data-gallery-prev]').click();
 if(await visibleSlide().getAttribute('aria-label')!=='5 of 5')throw Error('Slider wrap failed');
 for(let i=0;i<5;i++){
  await page.locator('[data-gallery-next]').click();
  if(!await visibleSlide().locator('img').evaluate(im=>im.complete&&im.naturalWidth>0))throw Error('Slider image failed');
 }
 await page.emulateMedia({reducedMotion:'reduce'});
 await page.locator('[data-gallery-next]').click();
 if(await visibleSlide().count()!==1)throw Error('Reduced motion slider failed');
 if(errors.length)throw Error(errors.join('\n'));
 console.log('PASS: 8 pages at 4 viewport sizes; local links, images, menu, slider buttons, keyboard, wraparound and reduced motion; no JavaScript errors.');
 await browser.close();
})().catch(e=>{console.error(e);process.exit(1)});
