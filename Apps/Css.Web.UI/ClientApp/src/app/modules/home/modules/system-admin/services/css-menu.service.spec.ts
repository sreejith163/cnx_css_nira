import { TestBed } from '@angular/core/testing';

import { CssMenuervice } from './css-menu.service';

describe('CssMenuervice', () => {
  let service: CssMenuervice;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(CssMenuervice);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
