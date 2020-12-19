import { TestBed } from '@angular/core/testing';

import { AgentSchedulesService } from './agent-schedules.service';

describe('AgentSchedulesService', () => {
  let service: AgentSchedulesService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AgentSchedulesService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
