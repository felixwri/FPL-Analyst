import { Component } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { ApiService } from '../services/api.service';
import { ServerURL } from '../../global';

@Component({
  selector: 'app-league',
  templateUrl: './league.component.html',
  styleUrl: './league.component.css'
})
export class LeagueComponent {
  leagueId: string | null = null;

  constructor(private route: ActivatedRoute, private apiservice: ApiService) {}

  ngOnInit(): void {
    this.leagueId = this.route.snapshot.paramMap.get('id');

    console.log(this.leagueId);

    if (this.leagueId) {
      this.apiservice.get(`${ServerURL}/league/${this.leagueId}`).subscribe((res) => {
        console.log(res);
      });
    }
  }
}
