import { Component } from '@angular/core';
import { ApiService } from '../../services/api.service';
import { ServerURL } from '../../../global';
import { UpcomingFixtures } from '../../../types';
import { BaseChartDirective } from 'ng2-charts';
import { ChartConfiguration, ChartData } from 'chart.js';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'fixtures',
  standalone: true,
  imports: [BaseChartDirective, CommonModule],
  templateUrl: './fixtures.component.html',
  styleUrl: './fixtures.component.css'
})
export class FixturesComponent {
  public upcomingFixtures: UpcomingFixtures[] = [];
  public chart: ChartHandler = new ChartHandler();


  constructor(private apiService: ApiService) {
    this.getFixtures();
  }

  getFixtures() {
    this.apiService.get(`${ServerURL}/fixture`).subscribe((data: any) => {
      console.log(data);
      if (data) {
        this.upcomingFixtures = data;
        this.processFixtures();
      }
    });
  }


  processFixtures() {
    

    let labels: string[] = [];
    let difficultySums: number[] = [];

    let mostDifficult = Infinity;
    let max = 0;

    for (let fixtures of this.upcomingFixtures) {
      labels.push(fixtures.Team);
      let sum = 0;
      for (let fixture of fixtures.Fixtures) {
        sum += fixture.RelativeDifficulty;
      }
      if (sum < mostDifficult) {
        mostDifficult = sum;
      }
      difficultySums.push(sum);
    }

    for (let i = 0; i < difficultySums.length; i++) {
      difficultySums[i] = Math.abs(mostDifficult) + difficultySums[i] + 100;
      if (difficultySums[i] > max) {
        max = difficultySums[i]
      }
    }

    this.chart.setLabels(labels);
    this.chart.addDataset('Difficulty', difficultySums);
    console.log(max);
    this.chart.hasData = true;
  }
}

class ChartHandler {
  options: ChartConfiguration<'bar'>['options'];
  data: ChartData<'bar'>;
  public hasData: boolean = false;

  constructor() {
    this.data = {
      labels: [],
      datasets: [],
    }
    let delayed = false;
    this.options = {
      animation: {
        onComplete: () => {
          delayed = true;
        },
        delay: (context) => {
          let delay = 0;
          if (context.type === 'data' && context.mode === 'default' && !delayed) {
            delay = context.dataIndex * 100
          }
          return delay;
        },
      },
      scales: {
        y: {
            ticks: {
                autoSkip: false
            }
        },
        x: {
          ticks: {
            display: false
          }
        }
      },
      layout: {
        padding: 10
      },
      responsive: true,
      maintainAspectRatio: false,
      indexAxis: 'y',
      plugins: {
        legend: {
          display: false,
        },
      },
    }
  }

  setLabels(labels: string[]) { 
    this.data.labels = labels;
  }

  addDataset(label: string, dataset: number[]) { 
    let newDataset: ChartData<'bar'>['datasets'][0] = { label, data: dataset };
    this.data.datasets.push(newDataset);

    console.log(this.data);
  }

  setData(data: ChartData<'bar'>) { 
    this.data = data;
  }

  setOptions(options: ChartConfiguration<'bar'>['options']) {
    this.options = options;
  }
}