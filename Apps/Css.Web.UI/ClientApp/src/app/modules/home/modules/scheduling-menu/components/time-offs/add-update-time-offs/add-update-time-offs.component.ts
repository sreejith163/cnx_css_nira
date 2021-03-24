import { DatePipe, WeekDay } from '@angular/common';
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
import { UpdateTimeOffs } from '../../../models/update-time-offs.model';
import { AuthService } from 'src/app/core/services/auth.service';
import { CustomValidators } from 'src/app/shared/util/validations.util';

@Component({
  selector: 'app-add-update-time-offs',
  templateUrl: './add-update-time-offs.component.html',
  providers: [NgbTimepickerConfig, DatePipe],
  styleUrls: ['./add-update-time-offs.component.scss']
})
export class AddUpdateTimeOffsComponent implements OnInit, OnDestroy {

  spinner = 'add-update-time-offs';
  formSubmitted: boolean;
  startDateValue: NgbDateStruct;
  endDateValue: NgbDateStruct;

  timeOffForm: FormGroup;
  today = this.calendar.getToday();
  deselectedTimeOption = Constants.TimeOffDeselectedTimeOption;

  weekDays: Array<WeekDay>;
  timeOffCodes = [];
  fullWeekArray: number[] = [];
  time: NgbTimeStruct = { hour: 0, minute: 0, second: 0 };

  updateTimeOffSubscription: ISubscription;
  addTimeOffSubscription: ISubscription;
  subscriptions: ISubscription[] = [];

  @Input() operation: ComponentOperation;
  @Input() timeOffCodeData: TimeOffResponse;
  @Input() schedulingCodes: SchedulingCode[];

  get form() {
    return this.timeOffForm.controls;
  }

  constructor(
    private formBuilder: FormBuilder,
    public activeModal: NgbActiveModal,
    private calendar: NgbCalendar,
    public config: NgbTimepickerConfig,
    private timeOffsService: TimeOffsService,
    private spinnerService: NgxSpinnerService,
    private authService: AuthService,
    private datepipe: DatePipe,
  ) {
    config.seconds = true;
    config.spinners = false;
  }

  ngOnInit(): void {
    this.fullWeekArray = [0, 1, 2, 3, 4, 5, 6];
    this.weekDays = Object.keys(WeekDay).filter(key => isNaN(WeekDay[key])).map(x => +x);
    this.intializeTimeOffForm();
    this.getTimeOffCodes();
    // this.onDayLengthChange();
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
    const day = (date.day < 10) ? ('0' + date.day) : date.day;
    const month = (date.month < 10) ? ('0' + date.month) : date.month;
    const endDate =  month + '/' + day + '/'   + date.year;
    this.timeOffForm.controls.endDate.patchValue(this.getFormattedDate(new Date(endDate)));
  }

  onStartDateChange(date: NgbDateStruct) {
    const day = (date.day < 10) ? ('0' + date.day) : date.day;
    const month = (date.month < 10) ? ('0' + date.month) : date.month;
    const startDate =  month + '/' + day + '/'   + date.year;
    this.timeOffForm.controls.startDate.patchValue(this.getFormattedDate(new Date(startDate)));
  }

  getTitle() {
    return ComponentOperation[this.operation];
  }

  hasAgentAccessValidationError() {
    if (!this.timeOffForm.controls.viewAllotments?.value &&
      !this.timeOffForm.controls.viewWaitLists?.value &&
      !this.timeOffForm.controls.timeOffs?.value &&
      !this.timeOffForm.controls.addNotes?.value &&
      !this.timeOffForm.controls.showPastDays?.value) {
      return true;
    }
  }

  hasAllowDayRequestValidationError() {
    const allowDayRequest: FormArray = this.timeOffForm.controls.allowDayRequest as FormArray;
    return allowDayRequest.length === 0;
  }

  hasFTEDayLengthValidationError() {
    return this.time?.hour === 0 && this.time?.minute === 0 && this.time?.second === 0;
  }

  hasFormControlValidationError(control: string) {
    return (
      this.formSubmitted &&
      this.timeOffForm.controls[control]?.errors?.required
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
    if (this.time) {
      const hour = String(this.time.hour).length === 1 ? '0' + this.time.hour : this.time.hour;
      const minute = String(this.time.minute).length === 1 ? '0' + this.time.minute : this.time.minute;
      const second = String(this.time.second).length === 1 ? '0' + this.time.second : this.time.second;
      const item = hour + ':' + minute + ':' + second;
      this.timeOffForm.controls.fTEDayLength.patchValue(item);
    }
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
    if (this.timeOffForm.valid && this.time && !this.hasFTEDayLengthValidationError()) {
      this.operation === ComponentOperation.Edit ? this.updateTimeOff() : this.addTimeOff();
    }
  }

  hasFullWeeksListValueSelected(value) {
    // const fullWeekList: FormArray = this.timeOffForm.get('allowFullWeekRequest') as FormArray;
    // return fullWeekList.controls.findIndex(x => x.value === value) !== -1;
    return this.fullWeekArray.findIndex(x => x === +value) > -1;
  }

  onFullWeeksListCheckboxChange(e) {
    // const fullWeekList: FormArray = this.timeOffForm.get('allowFullWeekRequest') as FormArray;
    if (e.target.checked) {
      this.fullWeekArray.push(+e.target.value);
    } else {
      const index = this.fullWeekArray.findIndex(x => x === +e.target.value);
      if (index > -1) {
        this.fullWeekArray.splice(index, 1);
      }
      // let i = 0;
      // fullWeekList.controls.forEach((item: FormControl) => {
      //   if (item.value === Number(e.target.value)) {
      //     fullWeekList.removeAt(i);
      //     return;
      //   }
      //   i++;
      // });
    }

    this.fullWeekArray.length === 7 ? this.timeOffForm.controls.allowFullWeekRequest.setValue(true) :
     this.timeOffForm.controls.allowFullWeekRequest.setValue(false);
  }

  hasDayRequestOnValueSelected(value) {
    const allowDayRequest: FormArray = this.timeOffForm.controls.allowDayRequest as FormArray;
    return allowDayRequest.controls.findIndex(x => x.value === value) !== -1;
  }

  onDayRequestOnCheckboxChange(e) {
    const allowDayRequest: FormArray = this.timeOffForm.controls.allowDayRequest as FormArray;
    if (e.target.checked) {
      allowDayRequest.push(new FormControl(Number(e.target.value)));
    } else {
      let i = 0;
      allowDayRequest.controls.forEach((item: FormControl) => {
        if (item.value === Number(e.target.value)) {
          allowDayRequest.removeAt(i);
          return;
        }
        i++;
      });
    }
  }

  private populateTimeOffFormDetails() {
    this.timeOffForm.controls.description.setValue(this.timeOffCodeData?.description);
    this.timeOffForm.controls.schedulingCodeId.setValue(this.timeOffCodeData?.schedulingCodeId);
    this.timeOffForm.controls.startDate.setValue(this.timeOffCodeData?.startDate);
    this.timeOffForm.controls.endDate.setValue(this.timeOffCodeData?.endDate);
    this.timeOffForm.controls.fTEDayLength.setValue(this.timeOffCodeData?.fTEDayLength);
    this.setFTEDayLength(this.timeOffCodeData?.fTEDayLength);
    this.timeOffForm.controls.firstDayOfWeek.setValue(this.timeOffCodeData?.firstDayOfWeek);
    this.startDateValue = this.setDateInNgbFormat(this.timeOffCodeData?.startDate);
    this.endDateValue = this.setDateInNgbFormat(this.timeOffCodeData?.endDate);
    this.timeOffForm.controls.viewAllotments.setValue(this.timeOffCodeData?.viewAllotments);
    this.timeOffForm.controls.viewWaitLists.setValue(this.timeOffCodeData?.viewWaitLists);
    this.timeOffForm.controls.timeOffs.setValue(this.timeOffCodeData?.timeOffs);
    this.timeOffForm.controls.addNotes.setValue(this.timeOffCodeData?.addNotes);
    this.timeOffForm.controls.showPastDays.setValue(this.timeOffCodeData?.showPastDays);
    this.timeOffForm.controls.forceOffDaysBeforeWeek.setValue(this.timeOffCodeData?.forceOffDaysBeforeWeek);
    this.timeOffForm.controls.forceOffDaysAfterWeek.setValue(this.timeOffCodeData?.forceOffDaysAfterWeek);
    this.timeOffForm.controls.allowFullWeekRequest.setValue(this.timeOffCodeData?.allowFullWeekRequest);
    this.timeOffCodeData?.allowFullWeekRequest ? this.fullWeekArray = [0, 1, 2, 3, 4, 5, 6] : this.fullWeekArray = [];
    this.timeOffForm.controls.deSelectedTime.setValue(this.timeOffCodeData?.deSelectedTime);
    this.timeOffForm.controls.deselectSavedDays.setValue(this.timeOffCodeData?.deselectSavedDays);
  }

  private setFTEDayLength(dayLength: string) {
    const data = dayLength.trim();
    this.time.hour = +data.slice(0, 2);
    this.time.minute = +data.slice(3, 5);
    this.time.second = +data.slice(6, 8);
  }

  private setDateInNgbFormat(date: Date) {
    const dateValue = new Date(date);
    const value: NgbDateStruct = {
      day: dateValue.getDate(),
      month: +dateValue.getMonth() + 1,
      year: dateValue.getFullYear()
    };

    return value;
  }

  private getFormattedDate(date: Date) {
    const transformedDate = this.datepipe.transform(date, 'yyyy-MM-dd');
    return new Date(transformedDate);
  }

  private addTimeOff() {
    const model = this.timeOffForm.value as AddTimeOffs;
    model.createdby = this.authService.getLoggedUserInfo().displayName;

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
      this.spinnerService.show(this.spinner, SpinnerOptions);
      const model = this.timeOffForm.value as UpdateTimeOffs;
      model.modifiedBy = this.authService.getLoggedUserInfo().displayName;

      this.updateTimeOffSubscription = this.timeOffsService.updateTimeOff(this.timeOffCodeData.id, model)
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

  private hasTimeOffCodeDetailsMisMatch() {
    for (const propertyName in this.timeOffForm.value) {
      if (this.timeOffForm.value[propertyName] !== this.timeOffCodeData[propertyName] && propertyName !== 'allowDayRequest') {
        return true;
      } else if (propertyName === 'allowDayRequest') {
        if (JSON.stringify(this.timeOffForm.value[propertyName]) !== JSON.stringify(this.timeOffCodeData[propertyName])) {
          return true;
        }
      }
    }
  }

  private createAllowDayRequestOnArray() {
    const array = new FormArray([]);
    if (this.operation === ComponentOperation.Add) {
      this.weekDays.forEach((element, index) => {
        array.push(new FormControl(element));
      });
    } else {
      this.timeOffCodeData.allowDayRequest.forEach((element, index) => {
        array.push(new FormControl(element));
      });
    }

    return array;
  }

  private intializeTimeOffForm() {
    this.timeOffForm = this.formBuilder.group({
      description: new FormControl('', Validators.compose([
        Validators.required,
        Validators.maxLength(50),
        CustomValidators.cannotContainSpace
      ])),
      schedulingCodeId: new FormControl('', Validators.required),
      startDate: new FormControl('', Validators.required),
      endDate: new FormControl('', Validators.required),
      allowDayRequest: this.createAllowDayRequestOnArray(),
      fTEDayLength: new FormControl('', Validators.required),
      firstDayOfWeek: new FormControl('', Validators.required),
      viewAllotments: new FormControl(true, Validators.required),
      viewWaitLists: new FormControl(false, Validators.required),
      timeOffs: new FormControl(false, Validators.required),
      addNotes: new FormControl(true, Validators.required),
      showPastDays: new FormControl(true, Validators.required),
      forceOffDaysBeforeWeek: new FormControl('', Validators.required),
      forceOffDaysAfterWeek: new FormControl('', Validators.required),
      allowFullWeekRequest: new FormControl(true, Validators.required),
      deSelectedTime: new FormControl(2, Validators.required),
      deselectSavedDays: new FormControl(true, Validators.required),
    });
  }

}
