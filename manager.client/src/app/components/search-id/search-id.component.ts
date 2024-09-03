import { Component } from '@angular/core';
import { ApiService } from '../../services/api.service';
import { TeamData } from '../../../types';
import { ServerURL } from '../../../global';
import { CommonModule } from '@angular/common';
import { Observable } from 'rxjs';
import { DataLoaderService } from '../../services/data-loader.service';
import { StorageService } from '../../services/storage.service';
import { Route, Router } from '@angular/router';

@Component({
  selector: 'team-search',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './search-id.component.html',
  styleUrl: './search-id.component.css'
})
export class SearchIdComponent {
  
  teamID: string = "";
  teamData: TeamData = {
    id: 0,
    name: "",
    managerName: "",
    points: 0,
    dateJoined: new Date(),
    leagues: [],
  };

  constructor(
    private apiService: ApiService,
    private dataLoader: DataLoaderService,
    private storageService: StorageService,
    private router: Router
  ) { }


  onSubmit(value: string) {
    this.onSearch(value);
  }

  onSearch(value: string) {
    this.teamID = value;
    console.log(this.teamID);

    this.apiService.post(`${ServerURL}/team`, {teamID: this.teamID}).subscribe((res) => { 
      console.log(res);
      if (res) {
        this.teamData.id = res.id;
        this.teamData.name = res.name;
        this.teamData.managerName = res.player_first_name;
        this.teamData.points = res.summary_overall_points;
        this.teamData.dateJoined = new Date(res.joined_time);
        this.teamData.leagues = res.leagues.classic.filter((league: any) => league.league_type == "x");
        this.dataLoader.setTeamData(this.teamData);

        this.storageService.setTeamData(this.teamData);
      }
      console.log(this.teamData)
    });
  }

  goToPersonal() {
    this.router.navigate([`/personal/${this.teamID}`]);
  }
}
