import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SchedulingFilterMComponent } from './scheduling-filter.component';

describe('SchedulingFilterMComponent', () => {
  let component: SchedulingFilterMComponent;
  let fixture: ComponentFixture<SchedulingFilterMComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SchedulingFilterMComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SchedulingFilterMComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
