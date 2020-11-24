import { TestBed } from '@angular/core/testing';

import { SkillTagService } from './skill-tag.service';

describe('SkillTagService', () => {
  let service: SkillTagService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(SkillTagService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
