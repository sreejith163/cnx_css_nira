import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AgentSchedulingGroupListComponent } from './agent-scheduling-group-list.component';

describe('AgentSchedulingGroupListComponent', () => {
  let component: AgentSchedulingGroupListComponent;
  let fixture: ComponentFixture<AgentSchedulingGroupListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AgentSchedulingGroupListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AgentSchedulingGroupListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
