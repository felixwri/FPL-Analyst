import { Component } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { ApiService } from '../services/api.service';
import { ServerURL } from '../../global';
import { StorageService } from '../services/storage.service';
import { TeamData } from '../../types';

@Component({
  selector: 'app-league',
  templateUrl: './league.component.html',
  styleUrl: './league.component.css'
})
export class LeagueComponent {
  leagueId: string | null = null;
  leagueData: any[any] | null = null;
  teamData: TeamData | null = null;
  displayOptions: boolean = false;

  constructor(
    private route: ActivatedRoute,
    private router: Router,
    private apiservice: ApiService,
    private storageService: StorageService
  ) { }

  ngOnInit(): void {
    this.leagueId = this.route.snapshot.paramMap.get('id');
    this.teamData = this.storageService.getTeamData();

    if (!this.leagueId && !this.teamData) { 
      this.router.navigate(['/']);
      return;
    }

    if (!this.leagueId && this.teamData) {
      // this.getLeagueData(); 
      this.displayOptions = true;
      console.log(this.teamData);
      return;
    } 

    this.getLeagueData();

  }

  getLeagueData(): void {
    
    this.apiservice.get(`${ServerURL}/league/${this.leagueId}`).subscribe((res) => {
      console.log(res);
      this.leagueData = res;
    });
  }

  selectLeague(leagueId: string) {
    this.router.navigate([`/league/${leagueId}`]);
    return;
  }
}
