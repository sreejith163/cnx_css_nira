import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { UpdateClientListComponent } from './update-client-list.component';

describe('UpdateClientListComponent', () => {
  let component: UpdateClientListComponent;
  let fixture: ComponentFixture<UpdateClientListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ UpdateClientListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(UpdateClientListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
