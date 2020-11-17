import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AddUpdatePermissionComponent } from './add-update-permission.component';

describe('AddUpdatePermissionComponent', () => {
  let component: AddUpdatePermissionComponent;
  let fixture: ComponentFixture<AddUpdatePermissionComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AddUpdatePermissionComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AddUpdatePermissionComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
