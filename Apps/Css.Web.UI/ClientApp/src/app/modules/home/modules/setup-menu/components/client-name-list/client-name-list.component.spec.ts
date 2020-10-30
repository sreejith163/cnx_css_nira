import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ClientNameListComponent } from './client-name-list.component';

describe('ClientNameListComponent', () => {
  let component: ClientNameListComponent;
  let fixture: ComponentFixture<ClientNameListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ClientNameListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ClientNameListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
