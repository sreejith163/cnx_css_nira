import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ClientLobGroupListComponent } from './client-lob-group-list.component';

describe('ClientLobGroupListComponent', () => {
  let component: ClientLobGroupListComponent;
  let fixture: ComponentFixture<ClientLobGroupListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ClientLobGroupListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ClientLobGroupListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
