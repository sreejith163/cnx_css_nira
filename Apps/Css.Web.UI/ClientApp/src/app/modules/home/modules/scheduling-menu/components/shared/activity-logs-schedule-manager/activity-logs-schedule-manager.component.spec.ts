import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ActivityLogsScheduleManagerComponent } from './activity-logs-schedule-manager.component';

describe('ActivityLogsScheduleManagerComponent', () => {
  let component: ActivityLogsScheduleManagerComponent;
  let fixture: ComponentFixture<ActivityLogsScheduleManagerComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ActivityLogsScheduleManagerComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ActivityLogsScheduleManagerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
