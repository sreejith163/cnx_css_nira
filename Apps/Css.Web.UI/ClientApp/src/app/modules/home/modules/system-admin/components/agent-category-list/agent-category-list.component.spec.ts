import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AgentCategoryListComponent } from './agent-category-list.component';

describe('AgentCategoryListComponent', () => {
  let component: AgentCategoryListComponent;
  let fixture: ComponentFixture<AgentCategoryListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AgentCategoryListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AgentCategoryListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
