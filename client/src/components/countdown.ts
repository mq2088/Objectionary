const DEADLINE = new Date('2026-03-30T06:00:00Z').getTime();

export function initCountdown(container: HTMLElement): void {
  function update() {
    const now = Date.now();
    const diff = DEADLINE - now;

    if (diff <= 0) {
      const elapsed = now - DEADLINE;
      const days = Math.floor(elapsed / (1000 * 60 * 60 * 24));
      const hours = Math.floor((elapsed % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
      const minutes = Math.floor((elapsed % (1000 * 60 * 60)) / (1000 * 60));
      const seconds = Math.floor((elapsed % (1000 * 60)) / 1000);

      container.innerHTML = `
        <div class="flex justify-center gap-6">
          <div class="text-center">
            <div class="text-4xl font-bold text-gray-400">${days}</div>
            <div class="text-xs text-gray-500 uppercase tracking-wider">Days</div>
          </div>
          <div class="text-center">
            <div class="text-4xl font-bold text-gray-400">${hours}</div>
            <div class="text-xs text-gray-500 uppercase tracking-wider">Hours</div>
          </div>
          <div class="text-center">
            <div class="text-4xl font-bold text-gray-400">${minutes}</div>
            <div class="text-xs text-gray-500 uppercase tracking-wider">Minutes</div>
          </div>
          <div class="text-center">
            <div class="text-4xl font-bold text-gray-400">${seconds}</div>
            <div class="text-xs text-gray-500 uppercase tracking-wider">Seconds</div>
          </div>
        </div>
        <p class="text-xs text-gray-500 mt-2">elapsed since deadline</p>`;

      showLateNotice();
      requestAnimationFrame(() => setTimeout(update, 1000));
      return;
    }

    const days = Math.floor(diff / (1000 * 60 * 60 * 24));
    const hours = Math.floor((diff % (1000 * 60 * 60 * 24)) / (1000 * 60 * 60));
    const minutes = Math.floor((diff % (1000 * 60 * 60)) / (1000 * 60));
    const seconds = Math.floor((diff % (1000 * 60)) / 1000);

    const isUrgent = diff < 48 * 60 * 60 * 1000;
    const numClass = isUrgent ? 'text-red-500' : 'text-red-600';

    container.innerHTML = `
      <div class="flex justify-center gap-6">
        <div class="text-center">
          <div class="text-4xl font-bold ${numClass}">${days}</div>
          <div class="text-xs text-gray-400 uppercase tracking-wider">Days</div>
        </div>
        <div class="text-center">
          <div class="text-4xl font-bold ${numClass}">${hours}</div>
          <div class="text-xs text-gray-400 uppercase tracking-wider">Hours</div>
        </div>
        <div class="text-center">
          <div class="text-4xl font-bold ${numClass}">${minutes}</div>
          <div class="text-xs text-gray-400 uppercase tracking-wider">Minutes</div>
        </div>
        <div class="text-center">
          <div class="text-4xl font-bold ${numClass}">${seconds}</div>
          <div class="text-xs text-gray-400 uppercase tracking-wider">Seconds</div>
        </div>
      </div>`;

    requestAnimationFrame(() => setTimeout(update, 1000));
  }

  update();
}

function showLateNotice(): void {
  if (document.getElementById('late-notice')) return;

  const heroText = document.querySelector('p.text-gray-300.max-w-xl');
  if (!heroText) return;

  const notice = document.createElement('p');
  notice.id = 'late-notice';
  notice.className = 'text-red-400 text-sm mt-3 max-w-xl mx-auto';
  notice.textContent =
    'The official submission period has already concluded on 30 March 2026 at 5pm, ' +
    'but your objection may still be accepted if you submit by 2 April.';
  heroText.insertAdjacentElement('afterend', notice);
}
