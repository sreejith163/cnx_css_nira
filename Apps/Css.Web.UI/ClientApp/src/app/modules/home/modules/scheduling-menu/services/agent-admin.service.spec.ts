import { TestBed } from '@angular/core/testing';

import { AgentAdminService } from './agent-admin.service';

describe('AgentAdminService', () => {
  let service: AgentAdminService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AgentAdminService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
