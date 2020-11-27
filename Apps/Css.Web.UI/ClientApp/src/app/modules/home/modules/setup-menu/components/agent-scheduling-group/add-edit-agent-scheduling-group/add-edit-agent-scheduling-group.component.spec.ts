import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AddEditAgentSchedulingGroupComponent } from './add-edit-agent-scheduling-group.component';

describe('AddEditAgentSchedulingGroupComponent', () => {
  let component: AddEditAgentSchedulingGroupComponent;
  let fixture: ComponentFixture<AddEditAgentSchedulingGroupComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AddEditAgentSchedulingGroupComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AddEditAgentSchedulingGroupComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
