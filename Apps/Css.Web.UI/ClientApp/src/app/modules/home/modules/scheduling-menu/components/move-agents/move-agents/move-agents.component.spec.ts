import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MoveAgentsComponent } from './move-agents.component';

describe('MoveAgentsComponent', () => {
  let component: MoveAgentsComponent;
  let fixture: ComponentFixture<MoveAgentsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MoveAgentsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MoveAgentsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
