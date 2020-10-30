import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AddEditSchedulingCodeComponent } from './add-edit-scheduling-code.component';

describe('AddEditSchedulingCodeComponent', () => {
  let component: AddEditSchedulingCodeComponent;
  let fixture: ComponentFixture<AddEditSchedulingCodeComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AddEditSchedulingCodeComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AddEditSchedulingCodeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
