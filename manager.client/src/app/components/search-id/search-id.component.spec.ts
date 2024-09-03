import { ComponentFixture, TestBed } from '@angular/core/testing';

import { SearchIdComponent } from './search-id.component';
import { provideHttpClient } from '@angular/common/http';

describe('SearchIdComponent', () => {
  let component: SearchIdComponent;
  let fixture: ComponentFixture<SearchIdComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [],
      providers: [provideHttpClient()]
    })
    .compileComponents();
    
    fixture = TestBed.createComponent(SearchIdComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
