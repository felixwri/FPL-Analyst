import { ComponentFixture, TestBed } from '@angular/core/testing';

import { LeagueComponent } from './league.component';
import { provideRouter } from '@angular/router';
import { provideHttpClient } from '@angular/common/http';

describe('LeagueComponent', () => {
  let component: LeagueComponent;
  let fixture: ComponentFixture<LeagueComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [LeagueComponent],
      providers: [provideRouter([]), provideHttpClient()],
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(LeagueComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
