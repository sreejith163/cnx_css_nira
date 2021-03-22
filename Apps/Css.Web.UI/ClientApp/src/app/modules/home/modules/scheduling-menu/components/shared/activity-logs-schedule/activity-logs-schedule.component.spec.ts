import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ActivityLogsScheduleComponent } from './activity-logs-schedule.component';

describe('ActivityLogsScheduleComponent', () => {
  let component: ActivityLogsScheduleComponent;
  let fixture: ComponentFixture<ActivityLogsScheduleComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ActivityLogsScheduleComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ActivityLogsScheduleComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
