import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AgentScheduleMonthlyViewComponent } from './agent-schedule-monthly-view.component';

describe('AgentScheduleMonthlyViewComponent', () => {
  let component: AgentScheduleMonthlyViewComponent;
  let fixture: ComponentFixture<AgentScheduleMonthlyViewComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AgentScheduleMonthlyViewComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AgentScheduleMonthlyViewComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
