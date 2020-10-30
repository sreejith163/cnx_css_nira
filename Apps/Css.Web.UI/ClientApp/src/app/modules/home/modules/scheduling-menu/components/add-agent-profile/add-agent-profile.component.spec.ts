import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AddAgentProfileComponent } from './add-agent-profile.component';

describe('AddAgentProfileComponent', () => {
  let component: AddAgentProfileComponent;
  let fixture: ComponentFixture<AddAgentProfileComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AddAgentProfileComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AddAgentProfileComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
