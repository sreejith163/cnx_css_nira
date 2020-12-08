import { WeekDay } from '@angular/common';
import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import {
  FormArray,
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
} from '@angular/forms';
import {
  NgbActiveModal,
  NgbModal,
  NgbModalOptions,
} from '@ng-bootstrap/ng-bootstrap';
import { ComponentOperation } from 'src/app/shared/enums/component-operation.enum';
import { TimeZone } from 'src/app/shared/models/time-zone.model';
import { CustomValidators } from 'src/app/shared/util/validations.util';
import { SubscriptionLike as ISubscription } from 'rxjs';
import { Constants } from 'src/app/shared/util/constants.util';
import { AuthService } from 'src/app/core/services/auth.service';
import { SkillGroupService } from '../../../services/skill-group.service';
import { TimezoneService } from 'src/app/shared/services/timezone.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { SpinnerOptions } from 'src/app/shared/util/spinner-options.util';
import { AddSkillGroup } from '../../../models/add-skill-group.model';
import { ErrorWarningPopUpComponent } from 'src/app/shared/popups/error-warning-pop-up/error-warning-pop-up.component';
import { UpdateSkillGroup } from '../../../models/update-skill-group.model';
import { GenericDataService } from '../../../services/generic-data.service';
import { SkillGroupResponse } from '../../../models/skill-group-response.model';
import { TranslationDetails } from 'src/app/shared/models/translation-details.model';

@Component({
  selector: 'app-add-edit-skill-group',
  templateUrl: './add-edit-skill-group.component.html',
  styleUrls: ['./add-edit-skill-group.component.scss']
})
export class AddEditSkillGroupComponent implements OnInit, OnDestroy {

  spinner = 'skillGroup';
  maxLength = Constants.DefaultTextMaxLength;
  openTypes = Constants.OperationHourTypes;

  formSubmitted: boolean;
  clientId: number;
  clientLobGroupId: number;

  weekDays: Array<WeekDay>;
  openTime: Array<any>;

  skillGroup: SkillGroupResponse;
  skillGroupForm: FormGroup;
  timeZoneList: TimeZone[] = [];

  getSkillGroupSubscription: ISubscription;
  getTimeZonesSubscription: ISubscription;
  addSkillGroupSubscription: ISubscription;
  updateSkillGroupSubscription: ISubscription;
  subscriptions: ISubscription[] = [];

  @Input() operation: ComponentOperation;
  @Input() skillGroupId: number;
  @Input() translationValues: TranslationDetails[];

  constructor(
    private formBuilder: FormBuilder,
    private authService: AuthService,
    private skillGroupService: SkillGroupService,
    private genericDataService: GenericDataService,
    private modalService: NgbModal,
    private timezoneService: TimezoneService,
    private spinnerService: NgxSpinnerService,
    public activeModal: NgbActiveModal
  ) { }

  ngOnInit(): void {

    this.weekDays = Object.keys(WeekDay).filter(key => isNaN(WeekDay[key])).map(x => +x);
    this.openTime = this.genericDataService.openTimes();
    this.intializeSkillGroupForm();
    this.getTimeZones();

    if (this.operation === ComponentOperation.Edit) {
      this.loadSkillGroup();
    }
  }

  ngOnDestroy() {
    this.subscriptions.forEach(subscription => {
      if (subscription) {
        subscription.unsubscribe();
      }
    });
  }

  get form() {
    return this.skillGroupForm.controls;
  }

  get operationHour() {
    return this.skillGroupForm.get('operationHour') as FormArray;
  }

  getTitle() {
    return ComponentOperation[this.operation];
  }

  hasFormControlValidationError(control: string) {
    return (
      this.formSubmitted &&
      this.skillGroupForm.controls[control].errors?.required
    );
  }

  hasOpenHoursValidationError(controlName: string, index: number) {
    return (
      this.formSubmitted &&
      this.operationHour.controls[index].get(controlName).errors?.required
    );
  }

  getWeekDay(weekDay: number) {
    return WeekDay[weekDay];
  }

  getTranslationIdForWeek(weekDay: number) {
    return 'radio_add_edit_first_day_of_week_' + this.getWeekDay(weekDay)?.toLowerCase();
  }

  changeOperatingHoursDays(startDay: WeekDay) {
    this.sortOperatingHoursArray(startDay);
  }

  onOptionTypeChange(index: number) {
    const value = this.operationHour.controls[index].get('operationHourOpenTypeId').value;

    if (value === String(this.openTypes.find(x => x.id === 2).id)) {
      this.operationHour.controls[index].get('from').setValidators([Validators.required]);
      this.operationHour.controls[index].get('to').setValidators([Validators.required]);
      this.operationHour.controls[index].get('from').enable();
      this.operationHour.controls[index].get('to').enable();
    } else {
      this.operationHour.controls[index].patchValue({ from: '', to: '' });
      this.operationHour.controls[index].get('from').clearValidators();
      this.operationHour.controls[index].get('to').clearValidators();
      this.operationHour.controls[index].get('from').disable();
      this.operationHour.controls[index].get('to').disable();
    }

    this.operationHour.controls[index].get('from').updateValueAndValidity();
    this.operationHour.controls[index].get('to').updateValueAndValidity();
  }

  saveSkillGroupDetails() {
    this.formSubmitted = true;
    if (this.skillGroupForm.valid && this.clientId && this.clientLobGroupId) {
      this.operation === ComponentOperation.Edit ? this.updateSkillGroupDetails() : this.addSkillGroupDetails();
    }
  }

  setClient(client: number) {
    this.clientId = client;
    this.clientLobGroupId = undefined;
  }

  setClientLobGroup(clientLobGroupId: number) {
    this.clientLobGroupId = clientLobGroupId;
  }

  private addSkillGroupDetails() {
    const addSkillGroupModel = this.skillGroupForm.value as AddSkillGroup;
    addSkillGroupModel.createdBy = this.authService.getLoggedUserInfo()?.displayName;
    addSkillGroupModel.clientLobGroupId = this.clientLobGroupId;

    this.spinnerService.show(this.spinner, SpinnerOptions);
    this.addSkillGroupSubscription = this.skillGroupService.addSkillGroup(addSkillGroupModel)
      .subscribe(() => {
        this.spinnerService.hide(this.spinner);
        this.activeModal.close({ needRefresh: true });
      }, (error) => {
        this.spinnerService.hide(this.spinner);
        if (error.status === 409) {
          this.showErrorWarningPopUpMessage(error.error);
        }
      });

    this.subscriptions.push(this.addSkillGroupSubscription);
  }

  private updateSkillGroupDetails() {
    if (this.hasSkillGroupDetailsMismatch()) {

      const updateSkillGroupModel = this.skillGroupForm.value as UpdateSkillGroup;
      updateSkillGroupModel.modifiedBy = this.authService.getLoggedUserInfo()?.displayName;
      updateSkillGroupModel.clientLobGroupId = this.clientLobGroupId;

      this.spinnerService.show(this.spinner, SpinnerOptions);
      this.updateSkillGroupSubscription = this.skillGroupService.updateSkillGroup(this.skillGroupId, updateSkillGroupModel)
        .subscribe(() => {
          this.spinnerService.hide(this.spinner);
          this.activeModal.close({ needRefresh: true });
        }, (error) => {
          this.spinnerService.hide(this.spinner);
          if (error.status === 409) {
            this.showErrorWarningPopUpMessage(error.error);
          }
        });

      this.subscriptions.push(this.updateSkillGroupSubscription);
    } else {
      this.activeModal.close({ needRefresh: false });
    }
  }

  private createOperationHoursArray() {
    const operationHours = new FormArray([]);
    this.weekDays.forEach((element, index) => {
      const operationHourGroup = this.formBuilder.group(
        {
          day: new FormControl(element, Validators.required),
          operationHourOpenTypeId: new FormControl('', Validators.required),
          from: new FormControl({ value: '', disabled: true }),
          to: new FormControl({ value: '', disabled: true })
        },
        { validators: this.rangeValidator.bind(this) }
      );
      operationHours.push(operationHourGroup);
    });

    return operationHours;
  }

  private populateOperationHoursValue() {
    const operationHours = this.skillGroupForm.get('operationHour') as FormArray;
    operationHours.controls.forEach((operationHourGroup, index) => {
      const openTypeId = this.skillGroup.operationHour[index]?.operationHourOpenTypeId;
      operationHourGroup.patchValue({
        operationHourOpenTypeId: openTypeId,
        from: this.skillGroup.operationHour[index]?.from,
        to: this.skillGroup.operationHour[index]?.to,
      });
      if (openTypeId === 2) {
        operationHourGroup.get('from').enable();
        operationHourGroup.get('to').enable();
      }
    });
    operationHours.updateValueAndValidity();
  }

  private showErrorWarningPopUpMessage(contentMessage: string) {
    const options: NgbModalOptions = { backdrop: false, centered: true, size: 'sm' };
    const modalRef = this.modalService.open(ErrorWarningPopUpComponent, options);
    modalRef.componentInstance.headingMessage = 'Error';
    modalRef.componentInstance.contentMessage = contentMessage;
    return modalRef;
  }

  private hasSkillGroupDetailsMismatch() {
    if (this.skillGroupForm.controls.name.value !== this.skillGroup.name || this.clientId !== this.skillGroup.clientId ||
      this.clientLobGroupId !== this.skillGroup.clientLobGroupId ||
      this.skillGroupForm.controls.firstDayOfWeek.value !== this.skillGroup.firstDayOfWeek ||
      this.skillGroupForm.controls.timezoneId.value !== this.skillGroup.timezoneId) {
      return true;
    } else {
      for (const index in this.weekDays) {
        if (
          this.operationHour.value[index]?.day !== this.skillGroup.operationHour[index]?.day ||
          this.operationHour.value[index]?.operationHourOpenTypeId !==
          this.skillGroup.operationHour[index]?.operationHourOpenTypeId ||
          this.operationHour.value[index]?.from !== this.skillGroup.operationHour[index]?.from ||
          this.operationHour.value[index]?.to !== this.skillGroup.operationHour[index]?.to
        ) {
          return true;
        }
      }
    }
  }

  private getSortedWeekDays(day: number) {
    const days = [];
    const previousDays = [];
    let found = false;

    this.weekDays.forEach((e) => {
      if (e === day && !found) {
        found = true;
        days.push(e);
      } else if (!found) {
        previousDays.push(e);
      } else {
        days.push(e);
      }
    });

    return (days.concat(previousDays));
  }

  private sortOperatingHoursArray(startDay) {
    if (this.skillGroupForm) {
      const sortedDays = this.getSortedWeekDays(startDay);
      const currentFormArrayValues = this.skillGroupForm.controls.operationHour.value;
      const sortedFormArray = this.getSortedOperatingHoursArray(sortedDays, currentFormArrayValues);
      this.operationHour.clear();
      this.skillGroupForm.setControl('operationHour', sortedFormArray);
      this.operationHour.controls.forEach(element => {
        if (+element.value.operationHourOpenTypeId !== 2) {
          element.get('from').disable();
          element.get('to').disable();
        }
      });
      this.operationHour.updateValueAndValidity();
    }
  }

  private getSortedOperatingHoursArray(sortedDays: any[], currentFormArrayValues: any[]) {
    const operationHour = new FormArray([]);
    sortedDays.forEach((element) => {
      const operatingHoursGroup = this.formBuilder.group(
        {
          day: [element, Validators.required],
          operationHourOpenTypeId: [currentFormArrayValues.find(x => x.day === element)?.operationHourOpenTypeId, Validators.required],
          from: [currentFormArrayValues.find(x => x.day === element)?.from],
          to: [currentFormArrayValues.find(x => x.day === element)?.to],
        },
        { validators: this.rangeValidator.bind(this) }
      );
      operationHour.push(operatingHoursGroup);
    });

    return operationHour;
  }

  private rangeValidator(operationHour: FormGroup) {
    const start = operationHour.get('from')?.value ?? '';
    const end = operationHour.get('to')?.value ?? '';

    if (start) {
      const operationHourOpenTypeId = operationHour.get('operationHourOpenTypeId')?.value ?? '';
      let startTime;
      let endTime;

      if (+operationHourOpenTypeId !== 2) {
        return null;
      }

      if (start) {
        startTime = new Date(null, null, null, this.genericDataService.getHours(start), this.genericDataService.getMinutes(start));
      }
      if (end) {
        endTime = new Date(null, null, null, this.genericDataService.getHours(end), this.genericDataService.getMinutes(end));
      }

      return (startTime < endTime) ? null : { rangeError: true };
    }
  }

  private getTimeZones() {
    this.spinnerService.show(this.spinner, SpinnerOptions);
    this.getTimeZonesSubscription = this.timezoneService.getTimeZones()
      .subscribe((response) => {
        this.timeZoneList = response;
        this.spinnerService.hide(this.spinner);
      }, (error) => {
        this.spinnerService.hide(this.spinner);
        console.log(error);
      });

    this.subscriptions.push(this.getTimeZonesSubscription);
  }

  private loadSkillGroup() {
    this.spinnerService.show(this.spinner, SpinnerOptions);

    this.getSkillGroupSubscription = this.skillGroupService.getSkillGroup(this.skillGroupId)
      .subscribe((response) => {
        this.skillGroup = response;
        this.populateFormDetails();
        this.spinnerService.hide(this.spinner);
      }, (error) => {
        this.spinnerService.hide(this.spinner);
        if (error.status === 409) {
          this.showErrorWarningPopUpMessage(error.error);
        }
      });

    this.subscriptions.push(this.getSkillGroupSubscription);
  }

  private populateFormDetails() {
    this.skillGroupForm.controls.name.setValue(this.skillGroup.name);
    this.skillGroupForm.controls.firstDayOfWeek.setValue(this.skillGroup.firstDayOfWeek);
    this.skillGroupForm.controls.timezoneId.setValue(this.skillGroup.timezoneId);
    this.clientId = this.skillGroup.clientId;
    this.clientLobGroupId = this.skillGroup.clientLobGroupId;
    this.sortOperatingHoursArray(this.skillGroup.firstDayOfWeek);
    this.populateOperationHoursValue();
  }

  private intializeSkillGroupForm() {
    this.skillGroupForm = this.formBuilder.group({
      name: new FormControl('', Validators.compose([
        Validators.required,
        Validators.maxLength(Constants.DefaultTextMaxLength),
        CustomValidators.cannotContainSpace])),
      firstDayOfWeek: new FormControl('', Validators.required),
      timezoneId: new FormControl('', Validators.required),
      operationHour: this.createOperationHoursArray()
    });
  }
}
