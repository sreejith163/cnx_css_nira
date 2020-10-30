import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AddEditClientLobGroupComponent } from './add-edit-client-lob-group.component';

describe('AddEditClientLobGroupComponent', () => {
  let component: AddEditClientLobGroupComponent;
  let fixture: ComponentFixture<AddEditClientLobGroupComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AddEditClientLobGroupComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AddEditClientLobGroupComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
