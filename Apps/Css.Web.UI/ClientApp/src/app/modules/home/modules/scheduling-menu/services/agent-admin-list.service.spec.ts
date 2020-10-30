import { TestBed } from '@angular/core/testing';

import { AgentAdminListService } from './agent-admin-list.service';

describe('AgentAdminListService', () => {
  let service: AgentAdminListService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AgentAdminListService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
