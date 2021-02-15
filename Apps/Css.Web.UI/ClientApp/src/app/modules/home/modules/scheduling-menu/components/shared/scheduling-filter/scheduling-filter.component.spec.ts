import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SchedulingFilterComponent } from './scheduling-filter.component';

describe('SchedulingFilterComponent', () => {
  let component: SchedulingFilterComponent;
  let fixture: ComponentFixture<SchedulingFilterComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SchedulingFilterComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SchedulingFilterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
