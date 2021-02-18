import { WeekDay } from '@angular/common';
import { Component, Input, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { NgbActiveModal, NgbCalendar } from '@ng-bootstrap/ng-bootstrap';
import { ComponentOperation } from 'src/app/shared/enums/component-operation.enum';
import { Constants } from 'src/app/shared/util/constants.util';
import { SchedulingCode } from '../../../../system-admin/models/scheduling-code.model';
import { TimeOffResponse } from '../../../models/time-offs-response.model';
import * as $ from 'jquery';

@Component({
  selector: 'app-add-update-time-offs',
  templateUrl: './add-update-time-offs.component.html',
  styleUrls: ['./add-update-time-offs.component.scss']
})
export class AddUpdateTimeOffsComponent implements OnInit {

  spinner = 'add-update-time-offs';
  formSubmitted: boolean;


  timeOffForm: FormGroup;
  today = this.calendar.getToday();
  timeOffAgentAccess = Constants.TimeOffAgentAccess;
  deselectedTimeOption = Constants.TimeOffDeselectedTimeOption;

  weekDays: Array<WeekDay>;
  timeOffCodes = [];

  @Input() operation: ComponentOperation;
  @Input() timeOffCodeData: TimeOffResponse;
  @Input() schedulingCodes: SchedulingCode[];

  constructor(
    private formBuilder: FormBuilder,
    public activeModal: NgbActiveModal,
    private calendar: NgbCalendar,
  ) { }

  ngOnInit(): void {
    this.weekDays = Object.keys(WeekDay).filter(key => isNaN(WeekDay[key])).map(x => +x);
    this.intializeTimeOffForm();
    this.getTimeOffCodes();
  }

  set() {
    const item = $('.clockpicker');
    item.clockpicker();
  }

  getTitle() {
    return ComponentOperation[this.operation];
  }

  hasFormControlValidationError(control: string) {
    return (
      this.formSubmitted &&
      this.timeOffForm.controls[control].errors?.required
    );
  }

  getWeekDay(weekDay: number) {
    return WeekDay[weekDay];
  }

  setStartDateAsToday() {
    const date = this.today.day + '/' + this.today.month + '/' + this.today.year;
    this.timeOffForm.controls.startDate.patchValue(this.today);

  }

  setEndDateAsToday() {
    const date = this.today.day + '/' + this.today.month + '/' + this.today.year;
    this.timeOffForm.controls.endDate.patchValue(this.today);
  }

  getTimeOffCodes() {
    for (const item of this.schedulingCodes) {
      const codePoints = item.icon.value.split('-').map(u => parseInt(`0x${u}`, 16));
      const icon = String.fromCodePoint(...codePoints);
      const tempData = {
        data: icon + ' ' + item.description,
        value: item.id
      };
      this.timeOffCodes.push(tempData);
    }
  }

  save() {

  }

  private allowDayRequestOnArray() {
    const array = new FormArray([]);
    this.weekDays.forEach((element, index) => {
      const arrayItem = this.formBuilder.group(
        {
          day: new FormControl(element),
          value: new FormControl(''),
        },
      );
      array.push(arrayItem);
    });

    return array;
  }

  private intializeTimeOffForm() {
    this.timeOffForm = this.formBuilder.group({
      description: new FormControl(''),
      timeOffCode: new FormControl(''),
      startDate: new FormControl(''),
      endDate: new FormControl(''),
      allowDayRequestOn: new FormControl(''),
      FTEDayLength: new FormControl(''),
      firstDayOfWeek: new FormControl(''),
      agentAccess: new FormControl(''),
    });
  }

}
