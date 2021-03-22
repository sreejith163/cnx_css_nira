import { WeekDay } from '@angular/common';
import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { NgbActiveModal, NgbCalendar, NgbDateStruct, NgbTimepickerConfig, NgbTimeStruct } from '@ng-bootstrap/ng-bootstrap';
import { ComponentOperation } from 'src/app/shared/enums/component-operation.enum';
import { Constants } from 'src/app/shared/util/constants.util';
import { SchedulingCode } from '../../../../system-admin/models/scheduling-code.model';
import { TimeOffResponse } from '../../../models/time-offs-response.model';
import * as $ from 'jquery';
import { AddTimeOffs } from '../../../models/add-time-offs.model';
import { TimeOffsService } from '../../../services/time-offs.service';
import { SubscriptionLike as ISubscription } from 'rxjs';
import { NgxSpinnerService } from 'ngx-spinner';
import { SpinnerOptions } from 'src/app/shared/util/spinner-options.util';

@Component({
  selector: 'app-add-update-time-offs',
  templateUrl: './add-update-time-offs.component.html',
  providers: [NgbTimepickerConfig],
  styleUrls: ['./add-update-time-offs.component.scss']
})
export class AddUpdateTimeOffsComponent implements OnInit, OnDestroy {

  spinner = 'add-update-time-offs';
  formSubmitted: boolean;
  startDateValue: NgbDateStruct;
  endDateValue: NgbDateStruct;

  timeOffForm: FormGroup;
  today = this.calendar.getToday();
  // timeOffAgentAccess = Constants.TimeOffAgentAccess;
  // deselectedTimeOption = Constants.TimeOffDeselectedTimeOption;

  weekDays: Array<WeekDay>;
  timeOffCodes = [];
  time: NgbTimeStruct = { hour: 0, minute: 0, second: 0 };

  updateTimeOffSubscription: ISubscription;
  addTimeOffSubscription: ISubscription;
  subscriptions: ISubscription[] = [];

  @Input() operation: ComponentOperation;
  @Input() timeOffCodeData: TimeOffResponse;
  @Input() schedulingCodes: SchedulingCode[];

  constructor(
    private formBuilder: FormBuilder,
    public activeModal: NgbActiveModal,
    private calendar: NgbCalendar,
    public config: NgbTimepickerConfig,
    private timeOffsService: TimeOffsService,
    private spinnerService: NgxSpinnerService,
  ) {
    config.seconds = true;
    config.spinners = false;
  }

  ngOnInit(): void {
    this.weekDays = Object.keys(WeekDay).filter(key => isNaN(WeekDay[key])).map(x => +x);
    this.intializeTimeOffForm();
    this.getTimeOffCodes();
    this.onDayLengthChange();
    if (this.operation === ComponentOperation.Edit) {
      this.populateTimeOffFormDetails();
    }
  }

  ngOnDestroy() {
    this.subscriptions.forEach(subscription => {
      if (subscription) {
        subscription.unsubscribe();
      }
    });
  }

  onEndDateChange(date: NgbDateStruct) {
    const startDate = date.day + '/' + date.month + '/' + date.year;
    this.timeOffForm.controls.endDate.patchValue(new Date(startDate));
  }

  onStartDateChange(date: NgbDateStruct) {
    const endDate = date.day + '/' + date.month + '/' + date.year;
    this.timeOffForm.controls.startDate.patchValue(new Date(endDate));
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

  onDayLengthChange() {
    const hour = String(this.time.hour).length === 1 ? '0' + this.time.hour : this.time.hour;
    const minute = String(this.time.minute).length === 1 ? '0' + this.time.minute : this.time.minute;
    const second = String(this.time.second).length === 1 ? '0' + this.time.second : this.time.second;
    const item = hour + ':' + minute + ':' + second;
    this.timeOffForm.controls.FTEDayLength.patchValue(item);
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
    this.formSubmitted = true;
    if (this.timeOffForm.valid) {
      this.operation === ComponentOperation.Edit ? this.updateTimeOff() : this.addTimeOff();
    }
  }

  hasFullWeeksListValueSelected(value) {
    const fullWeekList: FormArray = this.timeOffForm.controls.fullWeeks.get('fullWeekList') as FormArray;
    return fullWeekList.controls.findIndex(x => x.value === value) !== -1;
  }

  onFullWeeksListCheckboxChange(e) {
    const fullWeekList: FormArray = this.timeOffForm.controls.fullWeeks.get('fullWeekList') as FormArray;
    if (e.target.checked) {
      fullWeekList.push(new FormControl(Number(e.target.value)));
    } else {
      let i = 0;
      fullWeekList.controls.forEach((item: FormControl) => {
        if (item.value === Number(e.target.value)) {
          fullWeekList.removeAt(i);
          return;
        }
        i++;
      });
    }
  }

  hasDayRequestOnValueSelected(value) {
    const allowDayRequestOn: FormArray = this.timeOffForm.controls.allowDayRequestOn as FormArray;
    return allowDayRequestOn.controls.findIndex(x => x.value === value) !== -1;
  }

  onDayRequestOnCheckboxChange(e) {
    const allowDayRequestOn: FormArray = this.timeOffForm.controls.allowDayRequestOn as FormArray;
    if (e.target.checked) {
      allowDayRequestOn.push(new FormControl(Number(e.target.value)));
    } else {
      let i = 0;
      allowDayRequestOn.controls.forEach((item: FormControl) => {
        if (item.value === Number(e.target.value)) {
          allowDayRequestOn.removeAt(i);
          return;
        }
        i++;
      });
    }
  }

  // onDeselectedTimeValueChange(e) {
  //   const value = e.target.checked;
  //   const controlName = e.target.name;
  //   const deselectedTime: FormGroup = this.timeOffForm.controls.deselectedTime as FormGroup;
  //   if (controlName === 'released') {
  //     deselectedTime.controls.released.patchValue(value);
  //     deselectedTime.controls.reserve.patchValue(!value);
  //     deselectedTime.controls.waitListExists.patchValue(!value);
  //   } else if (controlName === 'reserve') {
  //     deselectedTime.controls.released.patchValue(!value);
  //     deselectedTime.controls.reserve.patchValue(value);
  //     deselectedTime.controls.waitListExists.patchValue(!value);
  //   } else if (controlName === 'waitListExists') {
  //     deselectedTime.controls.released.patchValue(!value);
  //     deselectedTime.controls.reserve.patchValue(!value);
  //     deselectedTime.controls.waitListExists.patchValue(value);
  //   }
  // }

  // hasDeselectedTimeGroupChecked(controlName: string) {
  //   const x = controlName;
  //   const deselectedTime: FormGroup = this.timeOffForm.controls.deselectedTime as FormGroup;
  //   return deselectedTime.controls[x].value;
  // }

  private populateTimeOffFormDetails() {
    this.timeOffForm.controls.description.setValue(this.timeOffCodeData.description);
    this.timeOffForm.controls.timeOffCode.setValue(this.timeOffCodeData.timeOffCode);
    this.timeOffForm.controls.startDate.setValue(this.timeOffCodeData.startDate);
    this.timeOffForm.controls.endDate.setValue(this.timeOffCodeData.endDate);
    this.timeOffForm.controls.FTEDayLength.setValue(this.timeOffCodeData.FTEDayLength);
    this.timeOffForm.controls.firstDayOfWeek.setValue(this.timeOffCodeData.firstDayOfWeek);
    this.startDateValue = this.setDateInNgbFormat(this.timeOffCodeData.startDate);
    this.endDateValue = this.setDateInNgbFormat(this.timeOffCodeData.endDate);
    this.setAllowDayRequestData();
    this.setFTEDayLength();
    this.setAgentAccessData();
    this.setFullWeekData();
    this.setDeselectedTimeData();
  }

  private setDeselectedTimeData() {
    const deselectedTime: FormGroup = this.timeOffForm.controls.deselectedTime as FormGroup;
    deselectedTime.controls.deselectTime.setValue(this.timeOffCodeData?.deselectedTime?.reserve);
    deselectedTime.controls.deselectSavedDays.setValue(this.timeOffCodeData?.deselectedTime?.deselectSavedDays);
  }

  private setFullWeekData() {
    const fullWeeks: FormGroup = this.timeOffForm.controls.fullWeeks as FormGroup;
    fullWeeks.controls.daysBeforeWeek.setValue(this.timeOffCodeData.fullWeeks.daysBeforeWeek);
    fullWeeks.controls.daysAfterWeek.setValue(this.timeOffCodeData.fullWeeks.daysAfterWeek);
    const array = fullWeeks.controls.fullWeekList as FormArray;
    array.reset();
    this.timeOffCodeData.fullWeeks.fullWeekList.forEach(ele => array.push(new FormControl(ele)));
  }

  private setAgentAccessData() {
    const agentAccess: FormGroup = this.timeOffForm.controls.agentAccess as FormGroup;
    agentAccess.controls.timeOffAllotments.setValue(this.timeOffCodeData.agentAccess.timeOffAllotments);
    agentAccess.controls.waitList.setValue(this.timeOffCodeData.agentAccess.waitList);
    agentAccess.controls.timeOffAnyDay.setValue(this.timeOffCodeData.agentAccess.timeOffAnyDay);
    agentAccess.controls.addNoteAllotments.setValue(this.timeOffCodeData.agentAccess.addNoteAllotments);
    agentAccess.controls.showPastDays.setValue(this.timeOffCodeData.agentAccess.showPastDays);
  }

  private setFTEDayLength() {
    const hour = this.timeOffCodeData.FTEDayLength.slice(0, 2) === '00' ? 0 : this.timeOffCodeData.FTEDayLength.slice(0, 2);
    const minute = this.timeOffCodeData.FTEDayLength.slice(4, 5) === '00' ? 0 : this.timeOffCodeData.FTEDayLength.slice(4, 5);
    const second = this.timeOffCodeData.FTEDayLength.slice(7, 8) === '00' ? 0 : this.timeOffCodeData.FTEDayLength.slice(7, 8);
    this.time = {
      hour: +hour,
      minute: +minute,
      second: +second
    };
  }

  private setAllowDayRequestData() {
    const array = this.timeOffForm.controls.allowDayRequestOn as FormArray;
    array.clear();
    this.timeOffCodeData.allowDayRequestOn.forEach(ele => array.push(new FormControl(ele)));

    return array;
  }

  private setDateInNgbFormat(date: Date) {
    const value: NgbDateStruct = {
      day: date.getDate(),
      month: +date.getMonth() + 1,
      year: date.getFullYear()
    };


    return value;
  }

  private addTimeOff() {
    const model = this.timeOffForm.value as TimeOffResponse;
    this.setDeselectedTimeModel(model);

    this.addTimeOffSubscription = this.timeOffsService.addTimeOff(model)
      .subscribe((response) => {
        this.spinnerService.hide(this.spinner);
        this.activeModal.close();

      }, (error) => {
        console.log(error);
      });

    this.subscriptions.push(this.addTimeOffSubscription);
  }

  private updateTimeOff() {
    if (this.hasTimeOffCodeDetailsMisMatch()) {
      this.spinnerService.show(this.spinner, SpinnerOptions)
      const model = this.timeOffForm.value as TimeOffResponse;
      model.id = this.timeOffCodeData.id;
      this.setDeselectedTimeModel(model);

      this.updateTimeOffSubscription = this.timeOffsService.updateTimeOff(model)
        .subscribe((response) => {
          this.spinnerService.hide(this.spinner);
          this.activeModal.close({ needRefresh: true });
        }, (error) => {
          console.log(error);
        });

      this.subscriptions.push(this.updateTimeOffSubscription);
    } else {
      this.activeModal.close({ needRefesh: false });
    }

  }

  hasTimeOffCodeDetailsMisMatch() {
    for (const propertyName in this.timeOffForm.value) {
      if (this.timeOffForm.value[propertyName] !== this.timeOffCodeData[propertyName]) {
        return true;
      }
    }
  }

  private setDeselectedTimeModel(model: TimeOffResponse) {
    const deselectedTime: FormGroup = this.timeOffForm.controls.deselectedTime as FormGroup;
    if (+deselectedTime.controls.deselectTime.value === 1) {
      model.deselectedTime.released = true;
      model.deselectedTime.reserve = false;
      model.deselectedTime.waitListExists = false;
    } else if (+deselectedTime.controls.deselectTime.value === 2) {
      model.deselectedTime.released = false;
      model.deselectedTime.reserve = true;
      model.deselectedTime.waitListExists = false;
    } else if (+deselectedTime.controls.deselectTime.value === 3) {
      model.deselectedTime.released = false;
      model.deselectedTime.reserve = false;
      model.deselectedTime.waitListExists = true;
    }

  }

  private createAllowDayRequestOnArray() {
    const array = new FormArray([]);
    this.weekDays.forEach((element, index) => {
      array.push(new FormControl(element));
    });

    return array;
  }

  private createFullWeeksArray() {
    const array = new FormArray([]);
    this.weekDays.forEach((element, index) => {
      array.push(new FormControl(element));
    });

    return array;
  }

  private createAgentAccessFormGroup() {
    const arrayItem = this.formBuilder.group(
      {
        timeOffAllotments: new FormControl(true),
        waitList: new FormControl(false),
        timeOffAnyDay: new FormControl(false),
        addNoteAllotments: new FormControl(true),
        showPastDays: new FormControl(true),
      },
    );

    return arrayItem;
  }

  private createFullWeeksFormGroup() {
    const arrayItem = this.formBuilder.group(
      {
        daysBeforeWeek: new FormControl('', Validators.max(999)),
        daysAfterWeek: new FormControl('', Validators.max(999)),
        fullWeekList: this.createFullWeeksArray(),
      },
    );

    return arrayItem;
  }

  private createDeselctedTimeFormGroup() {
    const arrayItem = this.formBuilder.group(
      {
        deselectTime: new FormControl(3),
        deselectSavedDays: new FormControl(4)
      },
    );

    return arrayItem;
  }

  // private createDeselctedTimeFormGroup() {
  //   const arrayItem = this.formBuilder.group(
  //     {
  //       released: new FormControl(true),
  //       reserve: new FormControl(false),
  //       waitListExists: new FormControl(false),
  //       deselectSavedDays: new FormControl(true)
  //     },
  //   );

  //   return arrayItem;
  // }

  private intializeTimeOffForm() {
    this.timeOffForm = this.formBuilder.group({
      description: new FormControl(''),
      timeOffCode: new FormControl(''),
      startDate: new FormControl(''),
      endDate: new FormControl(''),
      allowDayRequestOn: this.createAllowDayRequestOnArray(),
      FTEDayLength: new FormControl(''),
      firstDayOfWeek: new FormControl(''),
      agentAccess: this.createAgentAccessFormGroup(),
      fullWeeks: this.createFullWeeksFormGroup(),
      deselectedTime: this.createDeselctedTimeFormGroup()
    });

  }

}
