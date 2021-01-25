import { TestBed } from '@angular/core/testing';

import { LanguagePreferenceService } from './language-preference.service';

describe('LanguagePreferenceService', () => {
  let service: LanguagePreferenceService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(LanguagePreferenceService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
