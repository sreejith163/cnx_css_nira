import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AddUpdateSchedulingCodeComponent } from './add-update-scheduling-code.component';

describe('AddUpdateSchedulingCodeComponent', () => {
  let component: AddUpdateSchedulingCodeComponent;
  let fixture: ComponentFixture<AddUpdateSchedulingCodeComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AddUpdateSchedulingCodeComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AddUpdateSchedulingCodeComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
