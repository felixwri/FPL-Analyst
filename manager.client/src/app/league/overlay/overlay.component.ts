import { Component, EventEmitter, Input, Output } from '@angular/core';
import { Picks, Player, Positions } from '../../../types';
import { PlayerCardComponent } from '../../components/player-card/player-card.component';
import { MiniPlayerCardComponent } from '../../components/mini-player-card/mini-player-card.component';

@Component({
  selector: 'team-overlay',
  standalone: true,
  imports: [MiniPlayerCardComponent],
  templateUrl: './overlay.component.html',
  styleUrl: './overlay.component.css',
})
export class OverlayComponent {
  @Input() displayOverlay: boolean = false;
  @Input() overlayData?: Picks | null;

  @Output() closeOverlay = new EventEmitter<boolean>();

  public goalkeepers: Player[] = [];
  public defenders: Player[] = [];
  public midfielders: Player[] = [];
  public forwards: Player[] = [];

  ngOnChanges(): void {
    if (this.overlayData) {
      this.processPlayers();
    }
  }

  processPlayers() {
    if (!this.overlayData) return;
    for (let player of this.overlayData.picks) {
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

  close() {
    this.displayOverlay = false;
    this.overlayData = null;
    this.clearPlayers();
    this.closeOverlay.emit(true);
  }

  clearPlayers() {
    this.goalkeepers = [];
    this.defenders = [];
    this.midfielders = [];
    this.forwards = [];
  }
}
