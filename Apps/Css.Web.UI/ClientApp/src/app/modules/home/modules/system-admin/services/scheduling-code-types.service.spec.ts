import { TestBed } from '@angular/core/testing';

import { SchedulingCodeTypesService } from './scheduling-code-types.service';

describe('SchedulingCodeTypesService', () => {
  let service: SchedulingCodeTypesService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(SchedulingCodeTypesService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
