import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AgentSchedulingGroupTypeaheadComponent } from './agent-scheduling-group-typeahead.component';

describe('AgentSchedulingGroupTypeaheadComponent', () => {
  let component: AgentSchedulingGroupTypeaheadComponent;
  let fixture: ComponentFixture<AgentSchedulingGroupTypeaheadComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AgentSchedulingGroupTypeaheadComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AgentSchedulingGroupTypeaheadComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
