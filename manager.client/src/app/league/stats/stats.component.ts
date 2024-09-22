import { Component, Input } from '@angular/core';
import { Picks } from '../../../types';

@Component({
  selector: 'league-stats',
  standalone: true,
  imports: [],
  templateUrl: './stats.component.html',
  styleUrl: './stats.component.css',
})
export class StatsComponent {
  @Input() managerPicks?: Picks[] | null;

  private playerFrequency: { [key: string]: { name: string; frequency: number; points: number } } = {};
  public frequencyArray: { name: string; frequency: number; points: number }[] = [];

  ngOnChanges() {
    this.calculatePlayerFrequency();
  }

  calculatePlayerFrequency() {
    if (!this.managerPicks) {
      return;
    }

    for (const managersTeam of this.managerPicks) {
      for (const playerList of managersTeam.picks) {
        let fullName = playerList.firstName + ' ' + playerList.secondName;
        if (this.playerFrequency[playerList.id]) {
          this.playerFrequency[playerList.id].frequency += 1;
        } else {
          let freq = {
            name: fullName,
            frequency: 1,
            points: playerList.totalPoints,
          };
          this.playerFrequency[playerList.id] = freq;
        }
      }
    }

    let freq: { name: string; frequency: number; points: number }[] = [];
    for (let key in this.playerFrequency) {
      freq.push(this.playerFrequency[key]);
    }

    freq.sort((a, b) => {
      return b.frequency - a.frequency;
    });

    const total = this.managerPicks.length;

    freq = freq.slice(0, 15);

    freq = freq.map((x) => {
      return {
        name: x.name,
        frequency: Math.round((x.frequency / total) * 100),
        points: x.points,
      };
    });

    this.frequencyArray = freq;
  }
}
