import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { CopyScheduleMComponent } from './copy-schedule.component';

describe('CopyScheduleMComponent', () => {
  let component: CopyScheduleMComponent;
  let fixture: ComponentFixture<CopyScheduleMComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ CopyScheduleMComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(CopyScheduleMComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
