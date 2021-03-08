import { TestBed } from '@angular/core/testing';

import { AgentScheduleManagersService } from './agent-schedule-managers.service';

describe('AgentScheduleManagersService', () => {
  let service: AgentScheduleManagersService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AgentScheduleManagersService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
