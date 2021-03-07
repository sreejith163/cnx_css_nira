import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MoveAgentsSchedulingGroupTypeaheadComponent } from './move-agents-scheduling-group-typeahead.component';

describe('MoveAgentsSchedulingGroupTypeaheadComponent', () => {
  let component: MoveAgentsSchedulingGroupTypeaheadComponent;
  let fixture: ComponentFixture<MoveAgentsSchedulingGroupTypeaheadComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MoveAgentsSchedulingGroupTypeaheadComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MoveAgentsSchedulingGroupTypeaheadComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
