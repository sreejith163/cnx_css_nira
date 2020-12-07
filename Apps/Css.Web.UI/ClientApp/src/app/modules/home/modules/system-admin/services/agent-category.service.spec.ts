import { TestBed } from '@angular/core/testing';

import { AgentCategoryService } from './agent-category.service';

describe('AgentCategoryService', () => {
  let service: AgentCategoryService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AgentCategoryService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
