export type Tone = 'formal' | 'moderate' | 'personal';

const TONE_TOOLTIPS: Record<Tone, string> = {
  formal: 'Structured submission with numbered points and references to planning instruments (LEP, SEPP, Clause 4.6)',
  moderate: 'Firm and clear objection with some planning references, but more conversational language',
  personal: "Written as a concerned resident's letter — accessible tone, focuses on lived impact",
};

let currentTone: Tone = 'formal';

export function getSelectedTone(): Tone {
  return currentTone;
}

export function initToneSelector(container: HTMLElement): void {
  function render() {
    container.innerHTML = `
      <div>
        <label class="text-sm font-semibold">Tone</label>
        <div class="flex gap-2 mt-2">
          ${(['formal', 'moderate', 'personal'] as Tone[]).map(tone => `
            <button
              class="tone-btn px-5 py-2 rounded text-sm transition-colors relative group
                ${currentTone === tone
                  ? 'bg-gray-900 text-white'
                  : 'bg-white text-gray-700 border border-gray-300 hover:border-gray-500'}"
              data-tone="${tone}">
              ${tone.charAt(0).toUpperCase() + tone.slice(1)}
              <span class="invisible group-hover:visible absolute bottom-full left-1/2 -translate-x-1/2 mb-2 w-64 p-2 bg-gray-900 text-white text-xs rounded shadow-lg z-10">
                ${TONE_TOOLTIPS[tone]}
              </span>
            </button>
          `).join('')}
        </div>
      </div>`;

    container.querySelectorAll('.tone-btn').forEach(btn => {
      btn.addEventListener('click', () => {
        currentTone = (btn as HTMLElement).dataset.tone as Tone;
        render();
      });
    });
  }

  render();
}
