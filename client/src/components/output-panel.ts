export function initOutputPanel(container: HTMLElement): void {
  container.innerHTML = '';
}

export function showOutput(
  container: HTMLElement,
  text: string,
  onRegenerate: () => Promise<void>,
): void {
  container.innerHTML = `
    <div class="bg-white border border-gray-200 rounded-xl p-6 mt-8">
      <div class="flex justify-between items-center mb-4 flex-wrap gap-2">
        <h3 class="text-lg font-semibold">Your Objection</h3>
        <div class="flex gap-2">
          <button id="regenerate-btn" class="px-4 py-2 bg-white text-gray-700 border border-gray-300 rounded text-sm hover:border-gray-500 transition-colors cursor-pointer">
            Regenerate
          </button>
          <button id="copy-btn" class="px-5 py-2 bg-gray-900 text-white rounded text-sm font-semibold cursor-pointer hover:bg-gray-800 transition-colors">
            Copy to Clipboard
          </button>
        </div>
      </div>
      <div class="bg-gray-50 border border-gray-200 rounded-lg p-6 text-sm leading-relaxed whitespace-pre-wrap" id="objection-text">${escapeHtml(text)}</div>
      <div class="mt-4 p-3.5 bg-blue-50 rounded-lg text-sm">
        <strong>Next step:</strong> Copy the text above and paste it into your submission on the
        <a href="https://www.planningportal.nsw.gov.au/major-projects/projects/residential-development-fill-affordable-housing-40-48-redan-street-mosman" target="_blank" rel="noopener noreferrer" class="text-blue-600 underline hover:text-blue-800">
          NSW Planning Portal
        </a>
      </div>
    </div>`;

  document.getElementById('copy-btn')!.addEventListener('click', async () => {
    await navigator.clipboard.writeText(text);
    const btn = document.getElementById('copy-btn')!;
    btn.textContent = 'Copied!';
    setTimeout(() => { btn.textContent = 'Copy to Clipboard'; }, 2000);
  });

  document.getElementById('regenerate-btn')!.addEventListener('click', async () => {
    const btn = document.getElementById('regenerate-btn')! as HTMLButtonElement;
    btn.disabled = true;
    btn.classList.add('opacity-50', 'cursor-not-allowed');
    btn.innerHTML = '<span class="inline-flex items-center gap-2"><svg class="animate-spin h-4 w-4" viewBox="0 0 24 24"><circle class="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" stroke-width="4" fill="none"></circle><path class="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4z"></path></svg>Regenerating...</span>';
    const textEl = document.getElementById('objection-text')!;
    textEl.innerHTML = '<div class="flex flex-col items-center justify-center py-8 text-gray-400"><svg class="animate-spin h-8 w-8 mb-3" viewBox="0 0 24 24"><circle class="opacity-25" cx="12" cy="12" r="10" stroke="currentColor" stroke-width="4" fill="none"></circle><path class="opacity-75" fill="currentColor" d="M4 12a8 8 0 018-8V0C5.373 0 0 5.373 0 12h4z"></path></svg><p>Regenerating your objection...</p></div>';
    await onRegenerate();
  });
}

function escapeHtml(text: string): string {
  const div = document.createElement('div');
  div.textContent = text;
  return div.innerHTML;
}
