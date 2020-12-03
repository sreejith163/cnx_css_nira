import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AddAgentCategoryComponent } from './add-agent-category.component';

describe('AddAgentCategoryComponent', () => {
  let component: AddAgentCategoryComponent;
  let fixture: ComponentFixture<AddAgentCategoryComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AddAgentCategoryComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AddAgentCategoryComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
