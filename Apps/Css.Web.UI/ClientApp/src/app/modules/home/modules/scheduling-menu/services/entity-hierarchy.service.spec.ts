import { TestBed } from '@angular/core/testing';

import { EntityHierarchyService } from './entity-hierarchy.service';

describe('EntityHierarchyService', () => {
  let service: EntityHierarchyService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(EntityHierarchyService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
