import { Component, Input } from '@angular/core';
import { Player } from '../../../types';

@Component({
  selector: 'mini-player-card',
  standalone: true,
  imports: [],
  templateUrl: './mini-player-card.component.html',
  styleUrl: './mini-player-card.component.css',
})
export class MiniPlayerCardComponent {
  @Input() player: Player | null = null;
}
