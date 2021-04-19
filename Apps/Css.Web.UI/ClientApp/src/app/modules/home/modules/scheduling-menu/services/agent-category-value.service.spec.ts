import { TestBed } from '@angular/core/testing';

import { AgentCategoryValueService } from './agent-category-value.service';

describe('AgentCategoryValueService', () => {
  let service: AgentCategoryValueService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AgentCategoryValueService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
