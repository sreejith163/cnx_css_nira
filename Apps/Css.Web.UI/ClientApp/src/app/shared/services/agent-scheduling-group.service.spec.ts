import { TestBed } from '@angular/core/testing';
import { AgentSchedulingGroupService } from './agent-scheduling-group.service';


describe('AgentSchedulingGroupService', () => {
  let service: AgentSchedulingGroupService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AgentSchedulingGroupService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
