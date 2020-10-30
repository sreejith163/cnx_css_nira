import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AgentAdminListComponent } from './agent-admin-list.component';

describe('AgentAdminListComponent', () => {
  let component: AgentAdminListComponent;
  let fixture: ComponentFixture<AgentAdminListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AgentAdminListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AgentAdminListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
