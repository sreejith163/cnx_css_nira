import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SchedulingManagerComponent } from './scheduling-manager.component';

describe('SchedulingManagerComponent', () => {
  let component: SchedulingManagerComponent;
  let fixture: ComponentFixture<SchedulingManagerComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SchedulingManagerComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SchedulingManagerComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
