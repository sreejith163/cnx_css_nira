import { TestBed } from '@angular/core/testing';

import { TimeOffsService } from './time-offs.service';

describe('TimeOffsService', () => {
  let service: TimeOffsService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(TimeOffsService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
