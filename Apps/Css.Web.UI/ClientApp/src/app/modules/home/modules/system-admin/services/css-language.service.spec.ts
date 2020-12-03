import { TestBed } from '@angular/core/testing';

import { CssLanguageService } from './css-language.service';

describe('CssLanguageService', () => {
  let service: CssLanguageService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(CssLanguageService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
