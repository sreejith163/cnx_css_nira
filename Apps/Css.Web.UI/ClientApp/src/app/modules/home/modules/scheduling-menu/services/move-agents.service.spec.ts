import { TestBed } from '@angular/core/testing';

import { MoveAgentsService } from './move-agents.service';

describe('MoveAgentsService', () => {
  let service: MoveAgentsService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(MoveAgentsService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
