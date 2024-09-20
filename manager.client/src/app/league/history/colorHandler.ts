import { getColors } from '../../../global';
import { LeagueHistory } from '../../../types';

export type ColorMap = { [teamName: string]: string };
type Color = { r: number; g: number; b: number };

export function generateColorMap(league: LeagueHistory[]) {
  let colorMap: ColorMap = {};

  const grey = { r: 50, g: 50, b: 50 } as Color;
  const midGrey = { r: 100, g: 100, b: 100 } as Color;

  let { primary } = getColors();
  let { negative } = getColors();

  let colorA = rgbStringToRgb(primary);
  let colorB = midGrey;
  let colorC = rgbStringToRgb(negative);

  if (!colorA || !colorB || !colorC) {
    throw new Error('Invalid primary or secondary color');
  }

  let half = Math.floor(league.length / 2);

  for (let i = 0; i < league.length; i++) {
    let color = i < half ? lerpColor(colorA, colorB, i / half) : lerpColor(colorB, colorC, (i - half) / half);
    colorMap[league[i].name] = `rgb(${Math.floor(color.r)}, ${Math.floor(color.g)}, ${Math.floor(color.b)})`;
    console.log(colorMap[league[i].name]);
  }

  return colorMap;
}

function lerpColor(a: Color, b: Color, t: number) {
  return {
    r: a.r + (b.r - a.r) * t,
    g: a.g + (b.g - a.g) * t,
    b: a.b + (b.b - a.b) * t,
  } as Color;
}

function rgbStringToRgb(rgb: string): Color | null {
  let result = rgb.match(/\d+/g);
  return result
    ? {
        r: parseInt(result[0], 10),
        g: parseInt(result[1], 10),
        b: parseInt(result[2], 10),
      }
    : null;
}
