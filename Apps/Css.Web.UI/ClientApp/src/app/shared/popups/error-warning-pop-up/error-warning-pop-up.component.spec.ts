import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ErrorWarningPopUpComponent } from './error-warning-pop-up.component';

describe('ErrorWarningPopUpComponent', () => {
  let component: ErrorWarningPopUpComponent;
  let fixture: ComponentFixture<ErrorWarningPopUpComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ErrorWarningPopUpComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ErrorWarningPopUpComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
