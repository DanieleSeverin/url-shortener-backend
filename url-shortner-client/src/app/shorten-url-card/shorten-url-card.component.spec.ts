import { ComponentFixture, TestBed } from '@angular/core/testing';

import { ShortenUrlCardComponent } from './shorten-url-card.component';

describe('ShortenUrlCardComponent', () => {
  let component: ShortenUrlCardComponent;
  let fixture: ComponentFixture<ShortenUrlCardComponent>;

  beforeEach(async () => {
    await TestBed.configureTestingModule({
      declarations: [ ShortenUrlCardComponent ]
    })
    .compileComponents();
  });

  beforeEach(() => {
    fixture = TestBed.createComponent(ShortenUrlCardComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
