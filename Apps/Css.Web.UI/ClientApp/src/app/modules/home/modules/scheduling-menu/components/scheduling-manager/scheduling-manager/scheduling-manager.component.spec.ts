import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SchedulingManagerMComponent } from './scheduling-manager.component';

describe('SchedulingManagerMComponent', () => {
  let component: SchedulingManagerMComponent;
  let fixture: ComponentFixture<SchedulingManagerMComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SchedulingManagerMComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SchedulingManagerMComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
