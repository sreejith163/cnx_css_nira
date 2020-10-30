import { TestBed } from '@angular/core/testing';

import { ClientLobGroupDropdownService } from './client-lob-group-dropdown.service';

describe('ClientLobGroupDropdownService', () => {
  let service: ClientLobGroupDropdownService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(ClientLobGroupDropdownService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
