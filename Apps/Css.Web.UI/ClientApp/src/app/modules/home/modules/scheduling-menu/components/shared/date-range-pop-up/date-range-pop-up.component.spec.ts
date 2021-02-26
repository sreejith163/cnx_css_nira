import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { DateRangePopUpComponent } from './date-range-pop-up.component';

describe('DateRangePopUpComponent', () => {
  let component: DateRangePopUpComponent;
  let fixture: ComponentFixture<DateRangePopUpComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ DateRangePopUpComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(DateRangePopUpComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
