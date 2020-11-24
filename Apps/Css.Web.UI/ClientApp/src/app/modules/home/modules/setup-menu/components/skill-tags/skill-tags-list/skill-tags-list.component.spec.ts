import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SkillTagsListComponent } from './skill-tags-list.component';

describe('SkillTagsListComponent', () => {
  let component: SkillTagsListComponent;
  let fixture: ComponentFixture<SkillTagsListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SkillTagsListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SkillTagsListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
