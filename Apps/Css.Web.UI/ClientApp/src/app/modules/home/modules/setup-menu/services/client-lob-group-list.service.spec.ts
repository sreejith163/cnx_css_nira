import { TestBed } from '@angular/core/testing';

import { ClientLobGroupListService } from './client-lob-group-list.service';

describe('ClientLobGroupListService', () => {
  let service: ClientLobGroupListService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ClientLobGroupListService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
