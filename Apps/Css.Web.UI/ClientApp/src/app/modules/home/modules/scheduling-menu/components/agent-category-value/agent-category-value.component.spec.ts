import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AgentCategoryValueComponent } from './agent-category-value.component';

describe('AgentCategoryValueComponent', () => {
  let component: AgentCategoryValueComponent;
  let fixture: ComponentFixture<AgentCategoryValueComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AgentCategoryValueComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AgentCategoryValueComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
