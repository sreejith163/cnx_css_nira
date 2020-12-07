import { TestBed } from '@angular/core/testing';

import { CssMenuService } from './css-menu.service';

describe('CssMenuService', () => {
  let service: CssMenuService;

  beforeEach(() => {
    TestBed.configureTestingModule({});
    service = TestBed.inject(CssMenuService);
  });

  it('should be created', () => {
    expect(service).toBeTruthy();
  });
});
