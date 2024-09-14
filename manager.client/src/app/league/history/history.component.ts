import { Component, Input } from '@angular/core';
import { BaseChartDirective } from 'ng2-charts';
import { ChartConfiguration, ChartData } from 'chart.js';
import { League, LeagueHistory } from '../../../types';

@Component({
  selector: 'league-history',
  standalone: true,
  imports: [BaseChartDirective],
  templateUrl: './history.component.html',
  styleUrl: './history.component.css',
})
export class HistoryComponent {
  @Input() league: LeagueHistory[] | null = null;
  public chart: ChartHandler = new ChartHandler();

  ngOnChanges(): void {
    if (!this.league) return;

    console.log(this.league);

    this.calculatePosition();
  }

  historyChart(type: 'rank' | 'points' | 'pointsBench' | 'pointsTotal') {
    if (type === 'rank') {
      this.calculatePosition();
    } else if (type === 'points') {
      this.calcaulatePoints();
    }
    // } else if (type === "pointsBench") {
    //   this.calculatePointsOnBench();
    // } else if (type === "pointsTotal") {
    //   this.calculateTotalPoints();
    // }
  }

  calcaulatePoints() {
    this.chart.clearData();
    if (this.league === null) return;
    const weeks = this.league[0].points.length;

    let labels = [];
    for (let i = 1; i <= weeks; i++) {
      labels.push('W ' + i.toString());
    }

    this.chart.setLabels(labels);

    for (let team of this.league) {
      let col = `rgba(255, ${100 + Math.random() * 155}, ${100 + Math.random() * 155}, 1)`;
      let dataset = {
        label: team.name,
        data: team.points,
        borderColor: col,
        backgroundColor: col,
        borderWidth: 1,
        tension: 0.2,
      };

      this.chart.addDataset(dataset);
    }

    this.chart.hasData = true;
  }

  calculatePosition() {
    this.chart.clearData();
    if (this.league === null) return;
    const weeks = this.league[0].points.length;

    let labels = [];
    for (let i = 1; i <= weeks; i++) {
      labels.push('W ' + i.toString());
    }

    let teamPositions = {} as any;
    for (let team of this.league) {
      teamPositions[team.id] = { name: team.name, positions: [] };
    }

    let allWeeks = [];
    for (let week = 0; week < weeks; week++) {
      let weekPoints = [];
      for (let team of this.league) {
        weekPoints.push({ id: team.id, points: team.points[week], position: 0 });
      }

      let sorted = weekPoints.sort((a, b) => a.points - b.points);

      for (let i = 0; i < sorted.length; i++) {
        sorted[i].position = i + 1;
      }
      allWeeks.push(sorted);
    }

    for (let i = 0; i < this.league.length; i++) {
      for (let week = 0; week < weeks; week++) {
        let p = allWeeks[week]?.find((x) => x.id === this.league?.[i]?.id);
        if (p) teamPositions[this.league[i].id].positions.push(p.position);
      }
    }

    this.chart.setLabels(labels);

    for (let team in teamPositions) {
      let col = `rgba(255, ${100 + Math.random() * 155}, ${100 + Math.random() * 155}, 1)`;
      let dataset = {
        label: teamPositions[team].name,
        data: teamPositions[team].positions,
        borderColor: col,
        backgroundColor: col,
        borderWidth: 1,
        tension: 0.2,
      };

      this.chart.addDataset(dataset);
    }
    this.chart.hasData = true;
  }
}

class ChartHandler {
  options: ChartConfiguration<'line'>['options'];
  data: ChartData<'line'>;
  public hasData: boolean = false;

  constructor() {
    this.data = {
      labels: [],
      datasets: [],
    };
    let delayed = false;
    this.options = {
      animation: {
        onComplete: () => {
          delayed = true;
        },
        delay: (context) => {
          let delay = 0;
          if (context.type === 'data' && context.mode === 'default' && !delayed) {
            delay = context.dataIndex * 100;
          }
          return delay;
        },
      },
      scales: {},
      layout: {
        // padding: 10,
        autoPadding: true,
      },
      responsive: true,
      maintainAspectRatio: false,
      plugins: {
        legend: {
          display: false,
        },
      },
    };
  }

  clearData() {
    this.data = {
      labels: [],
      datasets: [],
    };
  }

  setLabels(labels: string[]) {
    this.data.labels = labels;
  }

  addDataset(dataset: ChartData<'line'>['datasets'][0]) {
    let newDataset: ChartData<'line'>['datasets'][0] = dataset;
    this.data.datasets.push(newDataset);
  }

  setData(data: ChartData<'line'>) {
    this.data = data;
  }

  setOptions(options: ChartConfiguration<'line'>['options']) {
    this.options = options;
  }
}
