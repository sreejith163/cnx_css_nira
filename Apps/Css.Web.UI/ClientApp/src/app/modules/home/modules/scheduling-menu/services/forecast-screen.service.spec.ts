import { TestBed } from '@angular/core/testing';

import { ForecastScreenService } from './forecast-screen.service';

describe('ForecastScreenService', () => {
  let service: ForecastScreenService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ForecastScreenService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
