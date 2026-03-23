import './styles/main.css';
import { initCountdown } from './components/countdown';
import { initConcerns, getSelectedConcerns } from './components/concerns';
import { initToneSelector, getSelectedTone } from './components/tone-selector';
import { initPersonalNote, getPersonalNote } from './components/personal-note';
import { initGenerateButton, showError, refreshButton } from './components/generate-button';
import { initOutputPanel, showOutput } from './components/output-panel';
import { generateObjection } from './api';

initCountdown(document.getElementById('countdown')!);

const outputContainer = document.getElementById('output')!;

initConcerns(document.getElementById('concerns')!, () => {
  refreshButton();
});

initToneSelector(document.getElementById('tone-selector')!);
initPersonalNote(document.getElementById('personal-note')!);
initOutputPanel(outputContainer);

async function handleGenerate() {
  const concerns = getSelectedConcerns();
  const tone = getSelectedTone();
  const personalNote = getPersonalNote();

  const result = await generateObjection({ concerns, tone, personalNote });

  if (result.ok) {
    showOutput(outputContainer, result.text, handleGenerate);
  } else {
    showError(result.error);
  }
}

initGenerateButton(
  document.getElementById('generate-button')!,
  () => getSelectedConcerns().length > 0,
  handleGenerate,
);
