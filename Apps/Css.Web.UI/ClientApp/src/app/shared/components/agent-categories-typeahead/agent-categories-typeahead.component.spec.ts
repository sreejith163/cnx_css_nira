import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AgentCategoriesTypeaheadComponent } from './agent-categories-typeahead.component';

describe('AgentCategoriesTypeaheadComponent', () => {
  let component: AgentCategoriesTypeaheadComponent;
  let fixture: ComponentFixture<AgentCategoriesTypeaheadComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AgentCategoriesTypeaheadComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AgentCategoriesTypeaheadComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
