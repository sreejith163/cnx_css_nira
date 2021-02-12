import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SchedulingGridMComponent } from './scheduling-grid.component';

describe('SchedulingGridMComponent', () => {
  let component: SchedulingGridMComponent;
  let fixture: ComponentFixture<SchedulingGridMComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SchedulingGridMComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SchedulingGridMComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
