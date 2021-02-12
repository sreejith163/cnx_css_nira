import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ViewOuScreenFilterComponent } from './view-ou-screen-filter.component';

describe('ViewOuScreenFilterComponent', () => {
  let component: ViewOuScreenFilterComponent;
  let fixture: ComponentFixture<ViewOuScreenFilterComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ViewOuScreenFilterComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ViewOuScreenFilterComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
