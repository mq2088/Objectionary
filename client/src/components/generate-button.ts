let loading = false;

export function initGenerateButton(
  container: HTMLElement,
  isEnabled: () => boolean,
  onGenerate: () => Promise<void>,
): void {
  function render() {
    const enabled = isEnabled() && !loading;
    container.innerHTML = `
      <div>
        <button
          id="generate-btn"
          class="w-full py-3.5 rounded-lg text-base font-bold transition-colors
            ${enabled
              ? 'bg-red-600 text-white hover:bg-red-700 cursor-pointer'
              : 'bg-gray-300 text-gray-500 cursor-not-allowed'}"
          ${enabled ? '' : 'disabled'}>
          ${loading ? 'Writing your objection...' : 'Generate My Objection'}
        </button>
        <p class="text-center text-xs text-gray-400 mt-2">Limited to 5 generations per hour.</p>
        <div id="generate-error" class="text-center text-sm text-red-600 mt-2"></div>
      </div>`;

    if (enabled) {
      document.getElementById('generate-btn')!.addEventListener('click', async () => {
        loading = true;
        render();
        await onGenerate();
        loading = false;
        render();
      });
    }
  }

  render();
  (window as any).__refreshGenerateButton = render;
}

export function showError(message: string): void {
  const el = document.getElementById('generate-error');
  if (el) el.textContent = message;
}

export function refreshButton(): void {
  (window as any).__refreshGenerateButton?.();
}
