import { TestBed } from '@angular/core/testing';

import { SchedulingCodeIconsService } from './scheduling-code-icons.service';

describe('SchedulingCodeIconsService', () => {
  let service: SchedulingCodeIconsService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(SchedulingCodeIconsService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
