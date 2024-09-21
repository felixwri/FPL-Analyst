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

type PositionsMap = { [teamId: string]: Positions };
type Positions = { name: string; points: number; positions: number[] };
type Position = { id: number; points: number; position: number };

export function calcPositions(chart: ChartHandler, league: LeagueHistory[], colorMap: ColorMap) {
  chart.clearData();
  const weeks = league[0].points.length;
  chart.setLabels(generateLabels(weeks));

  // Pad start
  for (let team of league) {
    for (let w = 0; w < weeks - team.points.length; w++) {
      team.points.unshift(0);
    }
    for (let w = 0; w < weeks - team.points_on_bench.length; w++) {
      team.points_on_bench.unshift(0);
    }
    for (let w = 0; w < weeks - team.total_points.length; w++) {
      team.total_points.unshift(0);
    }
  }

  // Calculate positions
  let teamPositions = {} as PositionsMap;
  for (let team of league) {
    teamPositions[team.id] = { name: team.name, points: 0, positions: [] } as Positions;
  }

  // For every week
  let allWeeks = [];
  for (let week = 0; week < weeks; week++) {
    // Generate list of points for each team
    let weekPoints: Position[] = [];
    for (let team of league) {
      weekPoints.push({ id: team.id, points: team.total_points[week], position: 0 } as Position);
    }

    // Sort the points
    let sorted = weekPoints.sort((a, b) => a.points - b.points);

    // 0 to 1 indexing
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
