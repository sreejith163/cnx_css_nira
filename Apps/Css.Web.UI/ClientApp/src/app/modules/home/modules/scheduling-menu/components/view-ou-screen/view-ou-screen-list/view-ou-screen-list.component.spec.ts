import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ViewOuScreenListComponent } from './view-ou-screen-list.component';

describe('ViewOuScreenListComponent', () => {
  let component: ViewOuScreenListComponent;
  let fixture: ComponentFixture<ViewOuScreenListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ViewOuScreenListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ViewOuScreenListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
