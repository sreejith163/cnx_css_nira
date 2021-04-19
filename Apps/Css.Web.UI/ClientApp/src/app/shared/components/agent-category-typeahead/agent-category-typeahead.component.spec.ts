import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AgentCategoryTypeaheadComponent } from './agent-category-typeahead.component';

describe('AgentCategoryTypeaheadComponent', () => {
  let component: AgentCategoryTypeaheadComponent;
  let fixture: ComponentFixture<AgentCategoryTypeaheadComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AgentCategoryTypeaheadComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AgentCategoryTypeaheadComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
