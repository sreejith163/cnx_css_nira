import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EmployeeTypeAheadComponent } from './employee-typeahead.component';

describe('EmployeeTypeaheadComponent', () => {
  let component: EmployeeTypeAheadComponent;
  let fixture: ComponentFixture<EmployeeTypeAheadComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EmployeeTypeAheadComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EmployeeTypeAheadComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
