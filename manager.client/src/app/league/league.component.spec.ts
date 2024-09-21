import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LeagueComponent } from './league.component';
import { provideRouter } from '@angular/router';
import { provideHttpClient } from '@angular/common/http';
import { ChooseLeagueComponent } from './choose-league/choose-league.component';
import { StandingsComponent } from './standings/standings.component';
import { HistoryComponent } from './history/history.component';
import { StatsComponent } from './stats/stats.component';

describe('LeagueComponent', () => {
  let component: LeagueComponent;
  let fixture: ComponentFixture<LeagueComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [LeagueComponent],
      imports: [ChooseLeagueComponent, StandingsComponent, HistoryComponent, StatsComponent],
      providers: [provideRouter([]), provideHttpClient()],
    }).compileComponents();

    fixture = TestBed.createComponent(LeagueComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
