import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { AddUpdateTimeOffsComponent } from './add-update-time-offs.component';

describe('AddUpdateTimeOffsComponent', () => {
  let component: AddUpdateTimeOffsComponent;
  let fixture: ComponentFixture<AddUpdateTimeOffsComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ AddUpdateTimeOffsComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(AddUpdateTimeOffsComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
