import { LeagueHistory } from '../../../types';
import { ColorMap } from './colorHandler';
import { ChartHandler } from './history.component';

function generateLabels(weeks: number) {
  let labels = [];
  for (let i = 1; i <= weeks; i++) {
    labels.push('W ' + i.toString());
  }
  return labels;
}

export function calculatePosition(chart: ChartHandler, league: LeagueHistory[], colorMap: ColorMap) {
  chart.clearData();
  const weeks = league[0].points.length;

  let labels = generateLabels(weeks);

  let teamPositions = {} as any;
  for (let team of league) {
    teamPositions[team.id] = { name: team.name, positions: [] };
  }

  let allWeeks = [];
  for (let week = 0; week < weeks; week++) {
    let weekPoints = [];
    for (let team of league) {
      weekPoints.push({ id: team.id, points: team.points[week], position: 0 });
    }

    let sorted = weekPoints.sort((a, b) => a.points - b.points);

    for (let i = 0; i < sorted.length; i++) {
      sorted[i].position = i + 1;
    }
    allWeeks.push(sorted);
  }

  for (let i = 0; i < league.length; i++) {
    for (let week = 0; week < weeks; week++) {
      let p = allWeeks[week]?.find((x) => x.id === league?.[i]?.id);
      if (p) teamPositions[league[i].id].positions.push(p.position);
    }
  }

  chart.setLabels(labels);

  for (let team in teamPositions) {
    let col = colorMap[teamPositions[team].name];
    let dataset = {
      label: teamPositions[team].name,
      data: teamPositions[team].positions,
      borderColor: col,
      backgroundColor: col,
      borderWidth: 2,
      tension: 0.2,
    };

    chart.addDataset(dataset);
  }
  chart.hasData = true;
}

export function calculatePositionCumilative(chart: ChartHandler, league: LeagueHistory[], colorMap: ColorMap) {
  chart.clearData();
  const weeks = league[0].points.length;

  let labels = generateLabels(weeks);

  let teamPositions = {} as any;
  for (let team of league) {
    teamPositions[team.id] = { name: team.name, positions: [] };
  }

  let allWeeks = [];
  for (let week = 0; week < weeks; week++) {
    let weekPoints = [];
    for (let team of league) {
      weekPoints.push({ id: team.id, points: team.total_points[week], position: 0 });
    }

    let sorted = weekPoints.sort((a, b) => a.points - b.points);

    for (let i = 0; i < sorted.length; i++) {
      sorted[i].position = i + 1;
    }
    allWeeks.push(sorted);
  }

  for (let i = 0; i < league.length; i++) {
    for (let week = 0; week < weeks; week++) {
      let p = allWeeks[week]?.find((x) => x.id === league?.[i]?.id);
      if (p) teamPositions[league[i].id].positions.push(p.position);
    }
  }

  chart.setLabels(labels);

  for (let team in teamPositions) {
    let col = colorMap[teamPositions[team].name];
    let dataset = {
      label: teamPositions[team].name,
      data: teamPositions[team].positions,
      borderColor: col,
      backgroundColor: col,
      borderWidth: 4,
      tension: 0.2,
    };

    chart.addDataset(dataset);
  }
  chart.hasData = true;
}
