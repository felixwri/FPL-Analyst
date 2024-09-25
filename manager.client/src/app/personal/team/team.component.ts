import { Component } from '@angular/core';
import { ApiService } from '../../services/api.service';

@Component({
  selector: 'app-team',
  standalone: true,
  imports: [],
  templateUrl: './team.component.html',
  styleUrl: './team.component.css',
})
export class TeamComponent {
  constructor(private apiService: ApiService) {
    this.getFixtures();
  }

  getFixtures() {
    this.apiService.getFixtures().subscribe((data) => {
      if (data) {
        console.log(data);
      }
    });
  }
}
