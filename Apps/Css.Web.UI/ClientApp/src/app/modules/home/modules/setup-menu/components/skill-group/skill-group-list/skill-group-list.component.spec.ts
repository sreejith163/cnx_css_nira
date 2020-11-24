import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SkillGroupListComponent } from './skill-group-list.component';

describe('SkillGroupListComponent', () => {
  let component: SkillGroupListComponent;
  let fixture: ComponentFixture<SkillGroupListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SkillGroupListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SkillGroupListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
