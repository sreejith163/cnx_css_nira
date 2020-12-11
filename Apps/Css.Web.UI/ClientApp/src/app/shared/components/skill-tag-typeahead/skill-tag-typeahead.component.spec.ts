import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SkillTagTypeaheadComponent } from './skill-tag-typeahead.component';

describe('SkillTagTypeaheadComponent', () => {
  let component: SkillTagTypeaheadComponent;
  let fixture: ComponentFixture<SkillTagTypeaheadComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SkillTagTypeaheadComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SkillTagTypeaheadComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
