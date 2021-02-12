import { TestBed } from '@angular/core/testing';

import { SkillGroupService } from './skill-group.service';

describe('SkillGroupService', () => {
  let service: SkillGroupService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(SkillGroupService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
