import { Component, Input } from '@angular/core';
import { Player } from '../../../types';

@Component({
  selector: 'player-card',
  standalone: true,
  imports: [],
  templateUrl: './player-card.component.html',
  styleUrl: './player-card.component.css',
})
export class PlayerCardComponent {
  @Input() player: Player | null = null;
}
