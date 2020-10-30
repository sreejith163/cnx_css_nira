import { TestBed } from '@angular/core/testing';

import { SchedulingGridService } from './scheduling-grid.service';

describe('SchedulingGridService', () => {
  let service: SchedulingGridService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(SchedulingGridService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
