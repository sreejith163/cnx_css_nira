import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ClientLobGroupTypeaheadComponent } from './client-lob-group-typeahead.component';

describe('ClientLobGroupTypeaheadComponent', () => {
  let component: ClientLobGroupTypeaheadComponent;
  let fixture: ComponentFixture<ClientLobGroupTypeaheadComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ClientLobGroupTypeaheadComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ClientLobGroupTypeaheadComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
