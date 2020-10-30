import { TestBed } from '@angular/core/testing';

import { SkillTagsService } from './skill-tags.service';

describe('SkillTagsService', () => {
  let service: SkillTagsService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(SkillTagsService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
