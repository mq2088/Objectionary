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
              : loading
                ? 'bg-red-400 text-white cursor-not-allowed'
                : 'bg-gray-300 text-gray-500 cursor-not-allowed'}"
          ${enabled ? '' : 'disabled'}>
          ${loading
            ? '<span class="inline-flex items-center gap-2"><svg class="animate-spin h-5 w-5" viewBox="0 0 24 24"><circle class="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" stroke-width="4" fill="none"></circle><path class="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4z"></path></svg>Writing your objection...</span>'
            : 'Generate My Objection'}
        </button>
        ${loading
          ? '<div class="w-full bg-gray-200 rounded-full h-1.5 mt-3 overflow-hidden"><div class="bg-red-600 h-1.5 rounded-full animate-pulse" style="width: 100%"></div></div><p class="text-center text-xs text-gray-500 mt-2">This may take up to 30 seconds...</p>'
          : '<p class="text-center text-xs text-gray-400 mt-2">Limited to 5 generations per hour.</p>'}
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
