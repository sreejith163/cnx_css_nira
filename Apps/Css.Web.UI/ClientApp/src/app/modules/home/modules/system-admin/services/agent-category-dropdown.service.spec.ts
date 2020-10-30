import { TestBed } from '@angular/core/testing';

import { AgentCategoryDropdownService } from './agent-category-dropdown.service';

describe('AgentCategoryDropdownService', () => {
  let service: AgentCategoryDropdownService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AgentCategoryDropdownService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
