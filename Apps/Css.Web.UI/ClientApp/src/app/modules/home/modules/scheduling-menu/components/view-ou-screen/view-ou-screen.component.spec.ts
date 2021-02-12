import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ViewOuScreenComponent } from './view-ou-screen.component';

describe('ViewOuScreenComponent', () => {
  let component: ViewOuScreenComponent;
  let fixture: ComponentFixture<ViewOuScreenComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ViewOuScreenComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ViewOuScreenComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
