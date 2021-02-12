import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SchedulingMComponent } from './scheduling.component';

describe('SchedulingMComponent', () => {
  let component: SchedulingMComponent;
  let fixture: ComponentFixture<SchedulingMComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SchedulingMComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SchedulingMComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
