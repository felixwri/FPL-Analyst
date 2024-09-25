import { Component } from '@angular/core';
import { ApiService } from '../../services/api.service';
import { ActivatedRoute, Router } from '@angular/router';
import { Picks, Player, Positions } from '../../../types';
import { PlayerCardComponent } from '../../components/player-card/player-card.component';

@Component({
  selector: 'team-picks',
  standalone: true,
  imports: [PlayerCardComponent],
  templateUrl: './team.component.html',
  styleUrl: './team.component.css',
})
export class TeamComponent {
  public players: Picks;

  public goalkeepers: Player[] = [];
  public defenders: Player[] = [];
  public midfielders: Player[] = [];
  public forwards: Player[] = [];

  constructor(private apiService: ApiService, private route: ActivatedRoute) {
    this.players = { id: 0, team: '', picks: [] };
  }

  ngOnInit() {
    let teamId = this.route.snapshot.paramMap.get('id');
    if (teamId !== null) {
      this.getPicks(teamId);
    }
  }

  getPicks(teamId: string) {
    this.apiService.getPicks(teamId).subscribe((data) => {
      if (data) {
        this.players = data;
        this.processPlayers();
        console.log(this.players);
      }
    });
  }

  processPlayers() {
    for (let player of this.players.picks) {
      if (player.position === Positions.Goalkeeper) {
        this.goalkeepers.push(player);
      } else if (player.position === Positions.Defender) {
        this.defenders.push(player);
      } else if (player.position === Positions.Midfielder) {
        this.midfielders.push(player);
      } else if (player.position === Positions.Forward) {
        this.forwards.push(player);
      }
    }
  }
}
