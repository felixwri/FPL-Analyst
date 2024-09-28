import { ComponentFixture, TestBed } from '@angular/core/testing';

import { MiniPlayerCardComponent } from './mini-player-card.component';

describe('MiniPlayerCardComponent', () => {
  let component: MiniPlayerCardComponent;
  let fixture: ComponentFixture<MiniPlayerCardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      imports: [MiniPlayerCardComponent]
    })
    .compileComponents();

    fixture = TestBed.createComponent(MiniPlayerCardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
