import { TestBed } from '@angular/core/testing';

import { AgentAdminDropdownsService } from './agent-admin-dropdowns.service';

describe('AgentAdminDropdownsService', () => {
  let service: AgentAdminDropdownsService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AgentAdminDropdownsService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
