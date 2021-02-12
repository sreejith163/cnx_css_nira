import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { EntityHierarchyComponent } from './entity-hierarchy.component';

describe('EntityHierarchyComponent', () => {
  let component: EntityHierarchyComponent;
  let fixture: ComponentFixture<EntityHierarchyComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ EntityHierarchyComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(EntityHierarchyComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
