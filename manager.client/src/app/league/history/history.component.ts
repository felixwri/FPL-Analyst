import { Component, Input } from '@angular/core';
import { BaseChartDirective } from 'ng2-charts';
import { ChartConfiguration, ChartData } from 'chart.js';
import { League, LeagueHistory } from '../../../types';
import { ColorMap, generateColorMap } from './colorHandler';
import { calculatePosition, calculatePositionCumilative } from './chartFunctions';

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
  public colorMap: ColorMap = {};

  ngOnChanges(): void {
    if (!this.league) return;

    console.log(this.league);

    this.colorMap = generateColorMap(this.league);

    calculatePositionCumilative(this.chart, this.league, this.colorMap);
  }

  historyChart(type: 'total' | 'rank' | 'points' | 'pointsBench' | 'pointsTotal') {
    if (this.league !== null)
      if (type === 'rank') {
        calculatePosition(this.chart, this.league, this.colorMap);
      } else if (type === 'total') {
        calculatePositionCumilative(this.chart, this.league, this.colorMap);
      } else if (type === 'points') {
        this.calcaulatePoints();
      }
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
      let col = this.colorMap[team.name];
      let dataset = {
        label: team.name,
        data: team.points,
        borderColor: col,
        backgroundColor: col,
        borderWidth: 2,
        tension: 0.2,
      };

      this.chart.addDataset(dataset);
    }

    this.chart.hasData = true;
  }
}

export class ChartHandler {
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
      scales: {
        y: {
          min: 1,
          offset: true,
          ticks: {
            stepSize: 1,
          },
        },
      },
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
