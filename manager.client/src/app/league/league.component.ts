import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ApiService } from '../services/api.service';
import { ServerURL } from '../../env/environment';
import { StorageService } from '../services/storage.service';
import { LeagueHistory, TeamData } from '../../types';

@Component({
  selector: 'app-league',
  templateUrl: './league.component.html',
  styleUrl: './league.component.css',
})
export class LeagueComponent {
  leagueId: string | number | null = null;
  leagueData: any[any] | null = null;
  leagueHistory: LeagueHistory[] | null = null;
  teamData: TeamData | null = null;
  displayOptions: boolean = false;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private apiservice: ApiService,
    private storageService: StorageService,
  ) {}

  ngOnInit(): void {
    this.leagueId = this.route.snapshot.paramMap.get('id');
    this.teamData = this.storageService.getTeamData();

    if (!this.leagueId && !this.teamData) {
      this.router.navigate(['/']);
      return;
    }

    if (!this.leagueId && this.teamData) {
      // this.getLeagueData();

      if (this.teamData.leagues.length === 1) {
        this.leagueId = this.teamData.leagues[0].id;
        this.leagueSelected(this.leagueId);
        return;
      }

      this.displayOptions = true;
      console.log(this.displayOptions);
      console.log(this.teamData);
      return;
    }

    if (this.leagueId !== null) {
      this.getLeagueData();
      this.getLeagueHistory(this.leagueId);
    }
  }

  leagueSelected(event: any) {
    console.log(event);
    this.leagueId = event;
    if (this.leagueId != null) {
      this.getLeagueData();
      this.getLeagueHistory(event.toString());
    }
  }

  getLeagueData(): void {
    this.apiservice.get(`${ServerURL}/league/${this.leagueId}`).subscribe({
      next: (res) => {
        console.log(res);
        this.leagueData = res;
      },
      error: (err) => {
        console.log(err);
      },
    });
  }

  getLeagueHistory(leagueId: string) {
    this.apiservice.getLeagueHistory(leagueId).subscribe({
      next: (res) => {
        console.log(res);
        this.leagueHistory = res;
      },
      error: (err) => {
        console.log(err);
      },
    });
  }

  selectLeague(leagueId: string) {
    this.router.navigate([`/league/${leagueId}`]);
    return;
  }
}
