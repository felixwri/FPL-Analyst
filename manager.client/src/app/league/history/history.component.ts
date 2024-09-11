import { Component, Input } from '@angular/core';
import { BaseChartDirective } from 'ng2-charts';
import { ChartConfiguration, ChartData } from 'chart.js';

@Component({
  selector: 'league-history',
  standalone: true,
  imports: [BaseChartDirective],
  templateUrl: './history.component.html',
  styleUrl: './history.component.css',
})
export class HistoryComponent {
  @Input() league: any;
  public chart: ChartHandler = new ChartHandler();

  ngOnChanges(): void {
    if (!this.league) return;
    console.log(this.league);

    let labels = ['1', '2', '3'];
    this.chart.setLabels(labels);

    for (let team of this.league) {
      let col = `rgba(255, ${100 + Math.random() * 155}, ${100 + Math.random() * 155}, 1)`;
      let dataset = {
        label: team.Name,
        data: team.Points_on_bench,
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
          display: true,
          position: 'bottom',
        },
      },
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
