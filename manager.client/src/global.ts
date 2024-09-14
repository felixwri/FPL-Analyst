export function getColors() {
  let colors: { [key: string]: string } = {};
  colors['primary'] = getComputedStyle(document.documentElement).getPropertyValue('--accent-primary');
  colors['secondary'] = getComputedStyle(document.documentElement).getPropertyValue('--accent-secondary');

  colors['positive'] = getComputedStyle(document.documentElement).getPropertyValue('--positive');
  colors['negative'] = getComputedStyle(document.documentElement).getPropertyValue('--negative');

  colors['text'] = getComputedStyle(document.documentElement).getPropertyValue('--text-color');
  colors['grey'] = getComputedStyle(document.documentElement).getPropertyValue('--grey-text-color');
  return colors;
}
