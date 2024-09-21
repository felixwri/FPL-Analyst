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
}
