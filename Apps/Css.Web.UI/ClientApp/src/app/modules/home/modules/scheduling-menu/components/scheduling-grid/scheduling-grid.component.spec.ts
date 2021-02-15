import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SchedulingGridComponent } from './scheduling-grid.component';

describe('SchedulingGridComponent', () => {
  let component: SchedulingGridComponent;
  let fixture: ComponentFixture<SchedulingGridComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SchedulingGridComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SchedulingGridComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
