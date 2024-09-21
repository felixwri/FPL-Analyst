import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';
import { Picks } from '../../../types';

@Component({
  selector: 'standings',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './standings.component.html',
  styleUrl: './standings.component.css',
})
export class StandingsComponent {
  @Input() leagueData: any;
  @Input() managerPicks?: Picks[] | null;
  liveScores?: { [key: number]: number } | null;

  ngOnChanges(): void {
    if (!this.managerPicks) return;

    let sums: { [key: number]: number } = {};
    for (let team in this.managerPicks) {
      let picks = this.managerPicks[team].picks;
      let sum = 0;
      for (let pick of picks) {
        sum += pick.totalPoints * pick.multiplier;
      }
      sums[this.managerPicks[team].id] = sum;
    }
    this.liveScores = sums;
  }

  getPickSum(id: number): number {
    if (!this.liveScores) return 0;
    return this.liveScores[id];
  }
}
