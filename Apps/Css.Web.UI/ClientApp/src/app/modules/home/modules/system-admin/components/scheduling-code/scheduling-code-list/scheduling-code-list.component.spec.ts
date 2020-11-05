import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { SchedulingCodeListComponent } from './scheduling-code-list.component';

describe('SchedulingCodeListComponent', () => {
  let component: SchedulingCodeListComponent;
  let fixture: ComponentFixture<SchedulingCodeListComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ SchedulingCodeListComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(SchedulingCodeListComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
