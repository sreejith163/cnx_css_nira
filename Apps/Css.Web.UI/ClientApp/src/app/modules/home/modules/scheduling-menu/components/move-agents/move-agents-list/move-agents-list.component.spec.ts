import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { MoveAgentsListComponent } from './move-agents-list.component';

describe('MoveAgentsListComponent', () => {
  let component: MoveAgentsListComponent;
  let fixture: ComponentFixture<MoveAgentsListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ MoveAgentsListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(MoveAgentsListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
