import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CopyScheduleDateRangeComponent } from './copy-schedule-date-range.component';

describe('CopyScheduleDateRangeComponent', () => {
  let component: CopyScheduleDateRangeComponent;
  let fixture: ComponentFixture<CopyScheduleDateRangeComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CopyScheduleDateRangeComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CopyScheduleDateRangeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
