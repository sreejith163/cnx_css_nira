import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ForecastScreenListComponent } from './forecast-screen-list.component';

describe('ForecastScreenListComponent', () => {
  let component: ForecastScreenListComponent;
  let fixture: ComponentFixture<ForecastScreenListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ForecastScreenListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ForecastScreenListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
