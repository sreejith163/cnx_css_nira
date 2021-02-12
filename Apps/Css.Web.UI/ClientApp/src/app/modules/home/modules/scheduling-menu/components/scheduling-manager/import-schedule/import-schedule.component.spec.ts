import { async, ComponentFixture, TestBed } from '@angular/core/testing';

import { ImportScheduleMComponent } from './import-schedule.component';

describe('ImportScheduleMComponent', () => {
  let component: ImportScheduleMComponent;
  let fixture: ComponentFixture<ImportScheduleMComponent>;

  beforeEach(async(() => {
    TestBed.configureTestingModule({
      declarations: [ ImportScheduleMComponent ]
    })
    .compileComponents();
  }));

  beforeEach(() => {
    fixture = TestBed.createComponent(ImportScheduleMComponent);
    component = fixture.componentInstance;
    fixture.detectChanges();
  });

  it('should create', () => {
    expect(component).toBeTruthy();
  });
});
