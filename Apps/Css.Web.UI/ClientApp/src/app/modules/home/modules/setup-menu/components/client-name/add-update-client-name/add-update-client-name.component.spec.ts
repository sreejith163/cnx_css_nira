import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AddUpdateClientNameComponent } from './add-update-client-name.component';

describe('AddUpdateClientNameComponent', () => {
  let component: AddUpdateClientNameComponent;
  let fixture: ComponentFixture<AddUpdateClientNameComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AddUpdateClientNameComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AddUpdateClientNameComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
