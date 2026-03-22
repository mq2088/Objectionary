const MAX_LENGTH = 300;

export function getPersonalNote(): string | undefined {
  const textarea = document.getElementById('personal-note') as HTMLTextAreaElement | null;
  const value = textarea?.value?.trim();
  return value || undefined;
}

export function initPersonalNote(container: HTMLElement): void {
  container.innerHTML = `
    <div>
      <label class="text-sm font-semibold" for="personal-note">
        Add a personal note <span class="text-gray-400 font-normal">(optional)</span>
      </label>
      <textarea
        id="personal-note"
        class="w-full mt-2 p-3 border border-gray-300 rounded-lg text-sm resize-y min-h-[60px] focus:outline-none focus:border-gray-500"
        maxlength="${MAX_LENGTH}"
        placeholder="e.g. I live two doors down and my children walk past this site to school every day..."></textarea>
      <div class="text-right text-xs text-gray-400 mt-1">
        <span id="char-count">0</span>/${MAX_LENGTH}
      </div>
    </div>`;

  const textarea = document.getElementById('personal-note') as HTMLTextAreaElement;
  const counter = document.getElementById('char-count')!;

  textarea.addEventListener('input', () => {
    counter.textContent = String(textarea.value.length);
  });
}
