import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AddUpdateSkillTagComponent } from './add-update-skill-tag.component';

describe('AddUpdateSkillTagComponent', () => {
  let component: AddUpdateSkillTagComponent;
  let fixture: ComponentFixture<AddUpdateSkillTagComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AddUpdateSkillTagComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AddUpdateSkillTagComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
