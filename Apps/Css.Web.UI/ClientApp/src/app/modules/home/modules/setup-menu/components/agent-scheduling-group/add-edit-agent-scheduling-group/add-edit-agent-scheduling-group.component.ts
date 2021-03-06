import { WeekDay } from '@angular/common';
import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { NgbActiveModal, NgbModal, NgbModalOptions } from '@ng-bootstrap/ng-bootstrap';
import { NgxSpinnerService } from 'ngx-spinner';
import { AuthService } from 'src/app/core/services/auth.service';
import { ComponentOperation } from 'src/app/shared/enums/component-operation.enum';
import { Constants } from 'src/app/shared/util/constants.util';
import { SpinnerOptions } from 'src/app/shared/util/spinner-options.util';
import { CustomValidators } from 'src/app/shared/util/validations.util';
import { GenericDataService } from '../../../services/generic-data.service';
import { SubscriptionLike as ISubscription } from 'rxjs';
import { ErrorWarningPopUpComponent } from 'src/app/shared/popups/error-warning-pop-up/error-warning-pop-up.component';
import { AgentSchedulingGroupResponse } from '../../../models/agent-scheduling-group-response.model';
import { AddAgentSchedulingGroup } from '../../../models/add-agent-scheduling-group.model';
import { UpdateAgentSchedulingGroup } from '../../../models/update-agent-scheduling-group.model';
import { TimezoneService } from 'src/app/shared/services/timezone.service';
import { TimeZone } from 'src/app/shared/models/time-zone.model';
import { TranslationDetails } from 'src/app/shared/models/translation-details.model';
import { AgentSchedulingGroupService } from 'src/app/shared/services/agent-scheduling-group.service';
import { ContentType } from 'src/app/shared/enums/content-type.enum';
import { GenericPopUpComponent } from 'src/app/shared/popups/generic-pop-up/generic-pop-up.component';

@Component({
  selector: 'app-add-edit-agent-scheduling-group',
  templateUrl: './add-edit-agent-scheduling-group.component.html',
  styleUrls: ['./add-edit-agent-scheduling-group.component.scss']
})
export class AddEditAgentSchedulingGroupComponent implements OnInit, OnDestroy {

  spinner = 'agentSchedulingGroup';
  maxLength = Constants.DefaultTextMaxLength;
  openTypes = Constants.OperationHourTypes;
  theCheckbox = false;
  formSubmitted: boolean;
  clientId: number;
  clientLobGroupId: number;
  skillGroupId: number;
  skillTagId: number;
  estartProvisions: boolean;
  weekDays: Array<WeekDay>;
  openTime: Array<any>;
  agentSchedulingGroupForm: FormGroup;
  agentSchedulingGroup: AgentSchedulingGroupResponse;

  getAgentSchedulingGroupSubscription: ISubscription;
  addAgentSchedulingGroupSubscription: ISubscription;
  updateAgentSchedulingGroupSubscription: ISubscription;
  getTimeZonesSubscription: ISubscription;
  timeZoneList: TimeZone[] = [];
  subscriptions: ISubscription[] = [];
  marked = false;

  @Input() operation: ComponentOperation;
  @Input() agentSchedulingGroupId: number;
  @Input() translationValues: TranslationDetails[];

  constructor(
    private formBuilder: FormBuilder,
    private modalService: NgbModal,
    private agentSchedulingGroupService: AgentSchedulingGroupService,
    private genericDataService: GenericDataService,
    private authService: AuthService,
    private timezoneService: TimezoneService,
    private spinnerService: NgxSpinnerService,
    public activeModal: NgbActiveModal
  ) { }
  
  public onChangeProvision(event){
    const options: NgbModalOptions = { backdrop: 'static', centered: true, size: 'm'};
    const modalRef = this.modalService.open(GenericPopUpComponent, options);

    var msg;
    if(!this.estartProvisions){
      msg = "Enabling this provision will include this Agent Scheduling Group to the next eStart export. eStart will send the edited schedules to CSS.";
    }else {
      msg = "Disabling this provision will reject incoming edited schedules from eStart for this Agent Scheduling Group.";
    }

    modalRef.componentInstance.headingMessage = 'Please confirm.';
    modalRef.componentInstance.contentMessage = msg;
    modalRef.result.then((result) => {
      if (result && result === true) {
        this.estartProvisions = event.target.checked;
      }
      else {
        this.estartProvisions = !event.target.checked;
      }
    });
  }

  ngOnInit(): void {
    this.weekDays = Object.keys(WeekDay).filter(key => isNaN(WeekDay[key])).map(x => +x);
    this.openTime = this.genericDataService.openTimes();
    this.intializeAgentSchedulingGroupForm();
    this.getTimeZones();

    if (this.operation === ComponentOperation.Edit) {
      this.loadAgentSchedulingGroup();
    }
    else {
      this.agentSchedulingGroupForm.controls.firstDayOfWeek.setValue(WeekDay.Sunday);
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
    return this.agentSchedulingGroupForm.controls;
  }

  get operationHour() {
    return this.agentSchedulingGroupForm.get('operationHour') as FormArray;
  }

  getTitle() {
    return ComponentOperation[this.operation];
  }

  hasFormControlValidationError(control: string) {
    return (
      this.formSubmitted &&
      this.agentSchedulingGroupForm.controls[control].errors?.required
    );
  }

  hasOpenHoursValidationError(controlName: string, index: number) {
    return (
      this.formSubmitted &&
      this.operationHour.controls[index].get(controlName).errors?.required
    );
  }

  isSpecialChar(event)
  {   
    var k;  
    k = event.charCode;
    return((k > 64 && k < 91) || (k > 96 && k < 123) || k == 8 || k == 32 || (k >= 48 && k <= 57)); 
  }

  getWeekDay(weekDay: number) {
    return WeekDay[weekDay];
  }

  changeOperatingHoursDays(startDay: WeekDay) {
    this.sortOperatingHoursArray(startDay);
  }

  private sortOperatingHoursArray(startDay) {
    if (this.agentSchedulingGroupForm) {
      const sortedDays = this.getSortedWeekDays(startDay);
      const currentFormArrayValues = this.agentSchedulingGroupForm.controls.operationHour.value;
      const sortedFormArray = this.getSortedOperatingHoursArray(sortedDays, currentFormArrayValues);
      this.operationHour.clear();
      this.agentSchedulingGroupForm.setControl('operationHour', sortedFormArray);
      this.operationHour.controls.forEach(element => {
        if (+element.value.operationHourOpenTypeId !== 2) {
          element.get('from').disable();
          element.get('to').disable();
        }
      });
      this.operationHour.updateValueAndValidity();
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

  getTranslationIdForWeek(weekDay: number) {
    return 'radio_add_edit_first_day_of_week_' + this.getWeekDay(weekDay)?.toLowerCase();
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

  saveAgentSchedulingGroupDetails() {
    this.formSubmitted = true;
    if (this.agentSchedulingGroupForm.valid && this.clientId && this.clientLobGroupId && this.skillGroupId && this.skillTagId) {
      this.operation === ComponentOperation.Edit ? this.updateAgentSchedulingGroupDetails() : this.addAgentSchedulingGroupDetails();
    }
  }

  setClient(client: number) {
    this.clientId = client;
    this.clientLobGroupId = undefined;
    this.skillGroupId = undefined;
    this.skillTagId = undefined;
  }

  setClientLobGroup(clientLobGroupId: number) {
    this.clientLobGroupId = clientLobGroupId;
    this.skillGroupId = undefined;
    this.skillTagId = undefined;
  }

  setSkillGroup(skillGroupId: number) {
    this.skillGroupId = skillGroupId;
    this.skillTagId = undefined;
  }

  setSkillTag(skillTagId: number) {
    this.skillTagId = skillTagId;
  }

  private addAgentSchedulingGroupDetails() {
    this.agentSchedulingGroupForm.controls.refId.setValue(this.agentSchedulingGroupForm.controls.refId.value.toString().replace(/[^0-9]/gi,""));
    const addAgentSchedulingGroup = this.agentSchedulingGroupForm.value as AddAgentSchedulingGroup;
    addAgentSchedulingGroup.skillTagId = this.skillTagId;
    addAgentSchedulingGroup.createdBy = this.authService.getLoggedUserInfo().displayName;
    this.spinnerService.show(this.spinner, SpinnerOptions);
    this.addAgentSchedulingGroupSubscription = this.agentSchedulingGroupService.addAgentSchedulingGroup(addAgentSchedulingGroup)
      .subscribe(() => {
        this.spinnerService.hide(this.spinner);
        this.activeModal.close({ needRefresh: true });
      }, (error) => {
        this.spinnerService.hide(this.spinner);
        if (error.status === 409) {
          this.showErrorWarningPopUpMessage(error.error);
        }
      });

    this.subscriptions.push(this.addAgentSchedulingGroupSubscription);
  }

  private updateAgentSchedulingGroupDetails() {
    if (this.hasAgentSchedulingGroupDetailsMismatch()) {
      this.agentSchedulingGroupForm.controls.refId.setValue(this.agentSchedulingGroupForm.controls.refId.value.toString().replace(/[^0-9]/gi,""));
      const updateAgentSchedulingGroupModel = this.agentSchedulingGroupForm.value as UpdateAgentSchedulingGroup;
      updateAgentSchedulingGroupModel.skillTagId = this.skillTagId;
      updateAgentSchedulingGroupModel.modifiedBy = this.authService.getLoggedUserInfo().displayName;

      this.spinnerService.show(this.spinner, SpinnerOptions);
      this.updateAgentSchedulingGroupSubscription = this.agentSchedulingGroupService.updateAgentSchedulingGroup(
        this.agentSchedulingGroupId, updateAgentSchedulingGroupModel)
        .subscribe(() => {
          this.spinnerService.hide(this.spinner);
          this.activeModal.close({ needRefresh: true });
        }, (error) => {
          this.spinnerService.hide(this.spinner);
          if (error.status === 409) {
            this.showErrorWarningPopUpMessage(error.error);
          }
        });
      this.subscriptions.push(this.updateAgentSchedulingGroupSubscription);
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
          operationHourOpenTypeId: new FormControl('1', Validators.required),
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
    const operationHours = this.agentSchedulingGroupForm.get('operationHour') as FormArray;
    operationHours.controls.forEach((operationHourGroup, index) => {
      const openTypeId = this.agentSchedulingGroup.operationHour[index]?.operationHourOpenTypeId;
      operationHourGroup.patchValue({
        operationHourOpenTypeId: openTypeId,
        from: this.agentSchedulingGroup.operationHour[index]?.from,
        to: this.agentSchedulingGroup.operationHour[index]?.to,
      });
      if (openTypeId === 2) {
        operationHourGroup.get('from').enable();
        operationHourGroup.get('to').enable();
      }
    });
    operationHours.updateValueAndValidity();
  }

  private showErrorWarningPopUpMessage(contentMessage: string) {
    const options: NgbModalOptions = { backdrop: 'static', centered: true, size: 'sm' };
    const modalRef = this.modalService.open(ErrorWarningPopUpComponent, options);
    modalRef.componentInstance.headingMessage = 'Error';
    modalRef.componentInstance.contentMessage = contentMessage;
    modalRef.componentInstance.messageType = ContentType.String;

    return modalRef;
  }

  private hasAgentSchedulingGroupDetailsMismatch() {
    if (this.agentSchedulingGroupForm.controls.name.value !== this.agentSchedulingGroup.name ||
      this.agentSchedulingGroupForm.controls.refId.value !== this.agentSchedulingGroup.refId ||
      this.clientId !== this.agentSchedulingGroup.clientId ||
      this.clientLobGroupId !== this.agentSchedulingGroup.clientLobGroupId || this.skillGroupId !== this.agentSchedulingGroup.skillGroupId
      || this.skillTagId !== this.agentSchedulingGroup.skillTagId
      || this.agentSchedulingGroupForm.controls.firstDayOfWeek.value !== this.agentSchedulingGroup.firstDayOfWeek || 
      this.estartProvisions !==  this.agentSchedulingGroup.estartProvision ||
      this.agentSchedulingGroupForm.controls.timezoneId.value !== this.agentSchedulingGroup.timezoneId) {
      return true;
    } else {
      for (const index in this.weekDays) {
        if (
          this.operationHour.value[index]?.day !== this.agentSchedulingGroup.operationHour[index]?.day ||
          this.operationHour.value[index]?.operationHourOpenTypeId !==
          this.agentSchedulingGroup.operationHour[index]?.operationHourOpenTypeId ||
          this.operationHour.value[index]?.from !== this.agentSchedulingGroup.operationHour[index]?.from ||
          this.operationHour.value[index]?.to !== this.agentSchedulingGroup.operationHour[index]?.to
        ) {
          return true;
        }
      }
    }
  }

  private populateFormDetails() {
    this.agentSchedulingGroupForm.controls.name.setValue(this.agentSchedulingGroup.name);
    this.agentSchedulingGroupForm.controls.es
    this.agentSchedulingGroupForm.controls.refId.setValue(this.agentSchedulingGroup.refId);
    this.clientId = this.agentSchedulingGroup.clientId;
    this.clientLobGroupId = this.agentSchedulingGroup.clientLobGroupId;
    this.skillGroupId = this.agentSchedulingGroup.skillGroupId;
    this.skillTagId = this.agentSchedulingGroup.skillTagId;
    this.estartProvisions = this.agentSchedulingGroup.estartProvision;

    this.agentSchedulingGroupForm.controls.firstDayOfWeek.setValue(this.agentSchedulingGroup.firstDayOfWeek);
    this.agentSchedulingGroupForm.controls.timezoneId.setValue(this.agentSchedulingGroup.timezoneId);
    this.sortOperatingHoursArray(this.agentSchedulingGroup.firstDayOfWeek);
    this.populateOperationHoursValue();
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

  private loadAgentSchedulingGroup() {
    this.spinnerService.show(this.spinner, SpinnerOptions);

    this.getAgentSchedulingGroupSubscription = this.agentSchedulingGroupService.getAgentSchedulingGroup(this.agentSchedulingGroupId)
      .subscribe((response) => {
        this.agentSchedulingGroup = response;
       
        this.populateFormDetails();
        this.spinnerService.hide(this.spinner);
   
      }, (error) => {
        this.spinnerService.hide(this.spinner);
        if (error.status === 409) {
          this.showErrorWarningPopUpMessage(error.error);
        }
      });
    this.subscriptions.push(this.getAgentSchedulingGroupSubscription);
  }

  private intializeAgentSchedulingGroupForm() {
    this.agentSchedulingGroupForm = this.formBuilder.group({
      name: new FormControl('', Validators.compose([
        Validators.required,
        Validators.maxLength(Constants.DefaultTextMaxLength),
        CustomValidators.cannotContainSpace])),
      refId: new FormControl(''),
      firstDayOfWeek: new FormControl('', Validators.required),
      timezoneId: new FormControl('', Validators.required),
      estartProvision: new FormControl(),
      operationHour: this.createOperationHoursArray()
    });
  }
}
