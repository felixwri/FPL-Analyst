import { Component, ElementRef, ViewChild } from '@angular/core';
import { ApiService } from '../../services/api.service';
import { getColors } from '../../../global';
import { ServerURL } from '../../../env/environment';
import { UpcomingFixtures } from '../../../types';
import { BaseChartDirective } from 'ng2-charts';
import { ChartConfiguration, ChartData } from 'chart.js';
import { CommonModule } from '@angular/common';

@Component({
  selector: 'fixtures',
  standalone: true,
  imports: [BaseChartDirective, CommonModule],
  templateUrl: './fixtures.component.html',
  styleUrl: './fixtures.component.css',
})
export class FixturesComponent {
  public upcomingFixtures: UpcomingFixtures[] = [];
  public chart: ChartHandler = new ChartHandler();

  @ViewChild('least') least!: ElementRef;
  @ViewChild('most') most!: ElementRef;

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

  ngAfterViewInit() {
    let colors = getColors();
    this.least.nativeElement.style.color = colors['primary'];
    this.most.nativeElement.style.color = colors['grey'];
  }

  /**
   * Generates the data for the chart
   */
  processFixtures() {
    let labels: string[] = [];
    let difficultySums: number[] = [];

    let { primary, grey } = getColors();

    let colors: string[] = [primary, grey];

    let backgroundColors: string[] = [];

    let leastDifficult = Infinity;
    let mostDifficult = -Infinity;
    let max = 0;

    for (let fixtures of this.upcomingFixtures) {
      labels.push(fixtures.Team);
      let sum = 0;
      for (let fixture of fixtures.Fixtures) {
        sum += fixture.RelativeDifficulty;
      }
      if (sum < leastDifficult) {
        leastDifficult = sum;
      }
      if (sum > mostDifficult) {
        mostDifficult = sum;
      }
      difficultySums.push(sum);
    }

    for (let i = 0; i < difficultySums.length; i++) {
      if (difficultySums[i] < 0) {
        backgroundColors.push(colors[0]);
      } else if (difficultySums[i] > 0) {
        backgroundColors.push(colors[1]);
      }

      difficultySums[i] = Math.round(((difficultySums[i] - leastDifficult) / (mostDifficult - leastDifficult)) * 100);
      if (difficultySums[i] == 0) {
        difficultySums[i] = 5;
      }

      // difficultySums[i] = Math.abs(leastDifficult) + difficultySums[i] + 100;
      // if (difficultySums[i] > max) {
      //   max = difficultySums[i]
      // }
    }

    this.chart.setLabels(labels);
    this.chart.addDataset({
      label: 'Difficulty',
      data: difficultySums,
      borderRadius: 10,
      backgroundColor: backgroundColors,
    });
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
          ticks: {
            color: 'white',
            autoSkip: false,
          },
        },
        x: {
          max: 100,
          min: 0,
          ticks: {
            display: false,
          },
        },
      },
      layout: {
        // padding: 10,
        autoPadding: true,
      },
      responsive: true,
      maintainAspectRatio: false,
      indexAxis: 'y',
      plugins: {
        legend: {
          display: false,
        },
      },
    };
  }

  setLabels(labels: string[]) {
    this.data.labels = labels;
  }

  addDataset(dataset: ChartData<'bar'>['datasets'][0]) {
    let newDataset: ChartData<'bar'>['datasets'][0] = dataset;
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
