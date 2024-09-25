import { ComponentFixture, TestBed } from '@angular/core/testing';

import { PersonalComponent } from './personal.component';
import { ActivatedRoute, provideRouter } from '@angular/router';
import { FixturesComponent } from './fixtures/fixtures.component';
import { provideHttpClient } from '@angular/common/http';
import { SearchIdComponent } from '../components/search-id/search-id.component';
import { TeamComponent } from './team/team.component';

describe('PersonalComponent', () => {
  let component: PersonalComponent;
  let fixture: ComponentFixture<PersonalComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [PersonalComponent],
      imports: [FixturesComponent, SearchIdComponent, TeamComponent],
      providers: [provideRouter([]), provideHttpClient()],
    }).compileComponents();

    fixture = TestBed.createComponent(PersonalComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
