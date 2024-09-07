import { CommonModule } from '@angular/common';
import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'choose-league',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './choose-league.component.html',
  styleUrl: './choose-league.component.css'
})
export class ChooseLeagueComponent {
  @Input() display: boolean = false;
  @Input() leagues: any[any] = [];

  @Output() selectedLeagueId = new EventEmitter<number>();
  
  onSelectLeague(leagueId: number) { 
    this.selectedLeagueId.emit(leagueId);
    this.display = false;
  }
}
