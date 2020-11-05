import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AddUpdateClientLobGroupComponent } from './add-update-client-lob-group.component';

describe('AddUpdateClientLobGroupComponent', () => {
  let component: AddUpdateClientLobGroupComponent;
  let fixture: ComponentFixture<AddUpdateClientLobGroupComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AddUpdateClientLobGroupComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AddUpdateClientLobGroupComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
