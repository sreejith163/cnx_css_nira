import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AddEditTranslationComponent } from './add-edit-translation.component';

describe('AddEditTranslationComponent', () => {
  let component: AddEditTranslationComponent;
  let fixture: ComponentFixture<AddEditTranslationComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AddEditTranslationComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AddEditTranslationComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
