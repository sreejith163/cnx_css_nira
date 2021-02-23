import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { TimeOffsListComponent } from './time-offs-list.component';

describe('TimeOffsListComponent', () => {
  let component: TimeOffsListComponent;
  let fixture: ComponentFixture<TimeOffsListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ TimeOffsListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(TimeOffsListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
