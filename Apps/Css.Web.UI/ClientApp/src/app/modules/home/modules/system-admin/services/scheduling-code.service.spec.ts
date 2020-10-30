import { TestBed } from '@angular/core/testing';

import { SchedulingCodeService } from './scheduling-code.service';

describe('SchedulingCodeService', () => {
  let service: SchedulingCodeService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(SchedulingCodeService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
