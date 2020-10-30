import { TestBed } from '@angular/core/testing';

import { ClientNameListService } from './client-name-list.service';

describe('ClientNameListService', () => {
  let service: ClientNameListService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ClientNameListService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
