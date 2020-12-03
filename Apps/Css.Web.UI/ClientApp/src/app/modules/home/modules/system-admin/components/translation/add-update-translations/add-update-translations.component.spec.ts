import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AddUpdateTranslationsComponent } from './add-update-translations.component';

describe('AddUpdateTranslationsComponent', () => {
  let component: AddUpdateTranslationsComponent;
  let fixture: ComponentFixture<AddUpdateTranslationsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AddUpdateTranslationsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AddUpdateTranslationsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
