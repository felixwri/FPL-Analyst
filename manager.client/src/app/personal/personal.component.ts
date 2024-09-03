import { Component } from '@angular/core';
import { StorageService } from '../services/storage.service';
import { TeamData, UpcomingFixtures } from '../../types';
import { ActivatedRoute, Router } from '@angular/router';
import { ApiService } from '../services/api.service';
import { ServerURL } from '../../global';

@Component({
  selector: 'app-personal',
  templateUrl: './personal.component.html',
  styleUrl: './personal.component.css'
})
export class PersonalComponent {
  personalID: number | null = null;
  teamData: TeamData | null = null;

  constructor(
    private storageService: StorageService,
    private apiService: ApiService,
    private route: ActivatedRoute,
    private router: Router
  ) {}

  ngOnInit() {
    let teamID = this.route.snapshot.paramMap.get('id');
    if (!teamID) {
      this.router.navigate(['/']);
    }
    this.teamData = this.storageService.getTeamData();
    this.personalID = this.teamData?.id || null;
  }
}
