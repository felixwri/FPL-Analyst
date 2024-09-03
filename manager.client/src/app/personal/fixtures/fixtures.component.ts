import { Component } from '@angular/core';
import { ApiService } from '../../services/api.service';
import { ServerURL } from '../../../global';
import { UpcomingFixtures } from '../../../types';

@Component({
  selector: 'fixtures',
  templateUrl: './fixtures.component.html',
  styleUrl: './fixtures.component.css'
})
export class FixturesComponent {
  public upcomingFixtures: UpcomingFixtures[] = [];

  constructor(private apiService: ApiService) {
    this.getFixtures();
  }

  getFixtures() {
    this.apiService.get(`${ServerURL}/fixture`).subscribe((data: any) => {
      console.log(data);
      if (data) {
        this.upcomingFixtures = data;
      }
    });
  }
}
