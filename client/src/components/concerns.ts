export interface Concern {
  id: string;
  title: string;
  description: string;
}

export const CONCERNS: Concern[] = [
  { id: 'height_bulk', title: 'Excessive Height & Bulk', description: 'The proposal is for a 10-storey building, which is clearly out of scale with the surrounding low-rise context.' },
  { id: 'structural_risk', title: 'Overdevelopment & Structural Risk', description: 'The development requires excavation of up to 10 metres into sandstone, extending to site boundaries. This creates real risks of ground movement, vibration and damage to neighbouring properties.' },
  { id: 'heritage', title: 'Heritage Impacts', description: 'The site adjoins heritage listed properties at 36 and 38 Redan Street. The proposal would overwhelm these heritage items, disrupt their visual setting and erode the existing character of Redan Street.' },
  { id: 'scenic_protection', title: 'Scenic Protection Area Conflict', description: 'The site is affected by a Scenic Protection Area, where planning objectives include limiting visual intrusion and protecting landscape character. A 10-storey building is difficult to reconcile with those objectives.' },
  { id: 'traffic', title: 'Traffic & Access Concerns', description: 'Safety concerns arise from the use of Redan Lane, which is only a little over 4 metres wide, has no footpaths, and is not designed for increased service and waste vehicle activity.' },
  { id: 'infrastructure', title: 'Lack of Supporting Infrastructure', description: 'Mosman currently operates with a single fire station and limited emergency services capacity. A significant increase in population raises practical questions about emergency response and service capacity.' },
  { id: 'non_compliance', title: 'Non-Compliance & Misleading Claims', description: 'The proposal exceeds height controls and requires a Clause 4.6 variation, yet is described as "compliant". A development that breaches fundamental controls should not be presented as compliant.' },
  { id: 'affordable_housing', title: 'Affordable Housing "Poor Door" Access', description: 'The proposal includes 11 affordable housing units with separate access from the laneway, which raises concerns about a "poor door" style arrangement and whether the design supports inclusive housing.' },
];

const selectedConcerns = new Set<string>();

export function getSelectedConcerns(): string[] {
  return Array.from(selectedConcerns);
}

export function initConcerns(container: HTMLElement, onChange: () => void): void {
  container.innerHTML = `
    <div class="grid grid-cols-1 md:grid-cols-2 gap-3">
      ${CONCERNS.map(c => `
        <label class="concern-card flex items-start gap-3 bg-white p-4 rounded-lg border border-gray-200 cursor-pointer hover:border-gray-400 transition-colors" data-id="${c.id}">
          <input type="checkbox" class="mt-1 accent-red-600" value="${c.id}" />
          <div>
            <strong class="text-sm">${c.title}</strong>
            <p class="text-xs text-gray-500 mt-1">${c.description}</p>
          </div>
        </label>
      `).join('')}
    </div>`;

  container.querySelectorAll('input[type="checkbox"]').forEach(cb => {
    cb.addEventListener('change', (e) => {
      const input = e.target as HTMLInputElement;
      const card = input.closest('.concern-card') as HTMLElement;
      if (input.checked) {
        selectedConcerns.add(input.value);
        card.classList.add('border-red-600', 'bg-red-50');
        card.classList.remove('border-gray-200');
      } else {
        selectedConcerns.delete(input.value);
        card.classList.remove('border-red-600', 'bg-red-50');
        card.classList.add('border-gray-200');
      }
      onChange();
    });
  });
}
