import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SkillGroupTypeaheadComponent } from './skill-group-typeahead.component';

describe('SkillGroupTypeaheadComponent', () => {
  let component: SkillGroupTypeaheadComponent;
  let fixture: ComponentFixture<SkillGroupTypeaheadComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SkillGroupTypeaheadComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SkillGroupTypeaheadComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
