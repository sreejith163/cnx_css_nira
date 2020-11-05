import { TestBed } from '@angular/core/testing';

import { ClientLobGroupService } from './client-lob-group.service';

describe('ClientLobGroupService', () => {
  let service: ClientLobGroupService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ClientLobGroupService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
