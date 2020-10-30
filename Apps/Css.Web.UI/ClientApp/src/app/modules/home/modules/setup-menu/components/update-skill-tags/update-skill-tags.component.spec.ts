import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { UpdateSkillTagsComponent } from './update-skill-tags.component';

describe('UpdateSkillTagsComponent', () => {
  let component: UpdateSkillTagsComponent;
  let fixture: ComponentFixture<UpdateSkillTagsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ UpdateSkillTagsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(UpdateSkillTagsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
