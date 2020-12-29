import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CopyScheduleComponent } from './copy-schedule.component';

describe('CopyScheduleComponent', () => {
  let component: CopyScheduleComponent;
  let fixture: ComponentFixture<CopyScheduleComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CopyScheduleComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CopyScheduleComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
