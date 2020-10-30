import { TestBed } from '@angular/core/testing';

import { AgentCategoryListService } from './agent-category-list.service';

describe('AgentCategoryListService', () => {
  let service: AgentCategoryListService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(AgentCategoryListService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
