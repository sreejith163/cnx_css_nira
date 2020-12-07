import { TestBed } from '@angular/core/testing';

import { CssLanguageervice } from './css-language.service';

describe('CssLanguageervice', () => {
  let service: CssLanguageervice;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(CssLanguageervice);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
