import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ActivityLogsScheuldeComponent } from './activity-logs-schedule.component';

describe('ActivityLogsScheuldeComponent', () => {
  let component: ActivityLogsScheuldeComponent;
  let fixture: ComponentFixture<ActivityLogsScheuldeComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ActivityLogsScheuldeComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ActivityLogsScheuldeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
