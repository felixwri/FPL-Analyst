import { CommonModule } from '@angular/common';
import { Component, Input } from '@angular/core';
import { League, LeagueData, Picks } from '../../../types';
import { OverlayComponent } from '../overlay/overlay.component';

@Component({
  selector: 'standings',
  standalone: true,
  imports: [CommonModule, OverlayComponent],
  templateUrl: './standings.component.html',
  styleUrl: './standings.component.css',
})
export class StandingsComponent {
  @Input() leagueData?: LeagueData | null;
  @Input() managerPicks?: Picks[] | null;
  liveScores: { [key: number]: number } = {};
  isLive: boolean = false;

  displayOverlay: boolean = false;
  overlayData?: Picks | null;

  ngOnChanges(): void {
    if (!this.managerPicks) return;
    if (!this.leagueData) return;

    if (this.managerPicks[0].picks[0].isLive) {
      this.isLive = true;
    }

    let sums: { [key: number]: number } = {};
    for (let team in this.managerPicks) {
      let picks = this.managerPicks[team].picks;
      let sum = 0;
      for (let pick of picks) {
        sum += pick.weekPoints * pick.multiplier;
      }
      sums[this.managerPicks[team].id] = sum;
    }
    this.liveScores = sums;
  }

  getPickSum(id: number): number {
    if (!this.liveScores) return 0;
    return this.liveScores[id];
  }

  display(id: number): void {
    this.displayOverlay = true;
    let picks = this.managerPicks?.find((p) => p.id === id);
    this.overlayData = picks;
  }

  closeOverlay(): void {
    this.displayOverlay = false;
    this.overlayData = null;
  }
}
