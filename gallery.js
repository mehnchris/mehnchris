document.querySelectorAll('.photo-gallery').forEach(gallery => {
  const slides = [...gallery.querySelectorAll('.gallery-slide')];
  const controls = gallery.querySelector('.gallery-controls');
  const status = gallery.querySelector('.gallery-status');
  let current = 0;
  function show(index) {
    current = (index + slides.length) % slides.length;
    slides.forEach((slide, i) => { slide.hidden = i !== current; });
    status.textContent = `${String(current + 1).padStart(2, '0')} / ${String(slides.length).padStart(2, '0')} — ${slides[current].querySelector('h3').textContent}`;
  }
  controls.hidden = false;
  gallery.querySelector('[data-gallery-prev]').addEventListener('click', () => show(current - 1));
  gallery.querySelector('[data-gallery-next]').addEventListener('click', () => show(current + 1));
  gallery.addEventListener('keydown', event => {
    if (event.key === 'ArrowLeft' || event.key === 'ArrowRight') {
      event.preventDefault();
      show(current + (event.key === 'ArrowRight' ? 1 : -1));
    }
  });
  let start;
  gallery.addEventListener('touchstart', event => {
    const touch = event.changedTouches[0];
    start = {x: touch.clientX, y: touch.clientY};
  }, {passive: true});
  gallery.addEventListener('touchend', event => {
    if (!start) return;
    const touch = event.changedTouches[0];
    const dx = touch.clientX - start.x, dy = touch.clientY - start.y;
    if (Math.abs(dx) > 50 && Math.abs(dx) > Math.abs(dy) * 1.5) show(current + (dx < 0 ? 1 : -1));
    start = null;
  }, {passive: true});
  gallery.addEventListener('touchcancel', () => { start = null; });
});
