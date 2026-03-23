const DEADLINE = new Date('2026-03-29T13:00:00Z').getTime();

export function initCountdown(container: HTMLElement): void {
  function update() {
    const now = Date.now();
    const diff = DEADLINE - now;

    if (diff <= 0) {
      container.innerHTML = `
        <div class="text-center py-8">
          <p class="text-2xl font-bold text-red-600">Submissions Have Closed</p>
        </div>`;
      disableForm();
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

function disableForm(): void {
  const form = document.getElementById('concern-form');
  if (form) {
    form.innerHTML = `
      <div class="text-center py-12 text-gray-500">
        <p class="text-xl font-semibold">Submissions have closed.</p>
        <p class="mt-2">The exhibition period ended on 30 March 2026.</p>
      </div>`;
  }
}
