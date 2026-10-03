# Chris Mehn portfolio

Preview with `python -m http.server 4173 --bind 127.0.0.1`, then open http://127.0.0.1:4173.

Edit shared markup and page content in `build_portfolio.py`, then run `python build_portfolio.py`. Styles live in `index.css`; navigation and printing live in `index.js`. Relative paths support GitHub Pages.

Wedding artwork is a stylized presentation, not a screenshot. Enterprise artwork is conceptual and contains no client data. Education uses neutral wording pending degree-title confirmation.

With Playwright available to Node and Microsoft Edge installed, run `node verify-portfolio.cjs`. Checks cover seven pages at four viewport sizes, links, images, overflow, JavaScript errors, and mobile navigation. Screenshots go to ignored `tmp/`.

The resume supports Print / save as PDF. Contact links open email.

`portfolio_extras.py` adds the Benda CPR Services case study and homepage photo gallery during generation. Their styles are in `extras.css`. The gallery uses optimized WebP copies of five original photos, supports buttons, left/right arrow keys and touch swipes, and intentionally does not autoplay. Its behavior is in `gallery.js`.
