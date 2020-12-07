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
import { AddSkillTag } from '../../../models/add-skill-tag.model';
import { GenericDataService } from '../../../services/generic-data.service';
import { SkillTagService } from '../../../services/skill-tag.service';
import { SubscriptionLike as ISubscription } from 'rxjs';
import { ErrorWarningPopUpComponent } from 'src/app/shared/popups/error-warning-pop-up/error-warning-pop-up.component';
import { MessagePopUpComponent } from 'src/app/shared/popups/message-pop-up/message-pop-up.component';
import { UpdateSkillTag } from '../../../models/update-skill-tag.model';
import { SkillTagResponse } from '../../../models/skill-tag-response.model';
import { TranslationDetails } from 'src/app/shared/models/translation-details.model';

@Component({
  selector: 'app-add-update-skill-tag',
  templateUrl: './add-update-skill-tag.component.html',
  styleUrls: ['./add-update-skill-tag.component.scss']
})
export class AddUpdateSkillTagComponent implements OnInit, OnDestroy {

  spinner = 'skillTag';
  maxLength = Constants.DefaultTextMaxLength;
  openTypes = Constants.OperationHourTypes;

  formSubmitted: boolean;
  clientId: number;
  clientLobGroupId: number;
  skillGroupId: number;

  weekDays: Array<WeekDay>;
  openTime: Array<any>;
  skillTagForm: FormGroup;
  skillTag: SkillTagResponse;

  getSkillTagSubscription: ISubscription;
  addSkillTagSubscription: ISubscription;
  updateSkillTagSubscription: ISubscription;
  subscriptions: ISubscription[] = [];

  @Input() operation: ComponentOperation;
  @Input() skillTagId: number;
  @Input() translationValues: TranslationDetails[];

  constructor(
    private formBuilder: FormBuilder,
    private modalService: NgbModal,
    private skillTagService: SkillTagService,
    private genericDataService: GenericDataService,
    private authService: AuthService,
    private spinnerService: NgxSpinnerService,
    public activeModal: NgbActiveModal
  ) { }

  ngOnInit(): void {
    this.weekDays = Object.keys(WeekDay).filter(key => isNaN(WeekDay[key])).map(x => +x);
    this.openTime = this.genericDataService.openTimes();
    this.intializeSkillTagForm();
    if (this.operation === ComponentOperation.Edit) {
      this.loadSkillTag();
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
    return this.skillTagForm.controls;
  }

  get operationHour() {
    return this.skillTagForm.get('operationHour') as FormArray;
  }

  getTitle() {
    return ComponentOperation[this.operation];
  }

  hasFormControlValidationError(control: string) {
    return (
      this.formSubmitted &&
      this.skillTagForm.controls[control].errors?.required
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

  saveSkillTagDetails() {
    this.formSubmitted = true;
    if (this.skillTagForm.valid && this.clientId && this.clientLobGroupId && this.skillGroupId) {
      this.operation === ComponentOperation.Edit ? this.updateSkillTagDetails() : this.addSkillTagDetails();
    }
  }

  setClient(client: number) {
    this.clientId = client;
    this.clientLobGroupId = undefined;
    this.skillGroupId = undefined;
  }

  setClientLobGroup(clientLobGroupId: number) {
    this.clientLobGroupId = clientLobGroupId;
    this.skillGroupId = undefined;
  }

  setSkillGroup(skillGroupId: number) {
    this.skillGroupId = skillGroupId;
  }

  private addSkillTagDetails() {
    const addSkillTagModel = this.skillTagForm.value as AddSkillTag;
    addSkillTagModel.skillGroupId = this.skillGroupId;
    addSkillTagModel.createdBy = this.authService.getLoggedUserInfo().displayName;

    this.spinnerService.show(this.spinner, SpinnerOptions);
    this.addSkillTagSubscription = this.skillTagService.addSkillTag(addSkillTagModel)
      .subscribe(() => {
        this.spinnerService.hide(this.spinner);
        this.activeModal.close({ needRefresh: true });
        this.showSuccessPopUpMessage('The record has been added!');
      }, (error) => {
        this.spinnerService.hide(this.spinner);
        if (error.status === 409) {
          this.showErrorWarningPopUpMessage(error.error);
        }
      });

    this.subscriptions.push(this.addSkillTagSubscription);
  }

  private updateSkillTagDetails() {
    if (this.hasSkillTagDetailsMismatch()) {
      const updateSkillTagModel = this.skillTagForm.value as UpdateSkillTag;
      updateSkillTagModel.skillGroupId = this.skillGroupId;
      updateSkillTagModel.modifiedBy = this.authService.getLoggedUserInfo().displayName;

      this.spinnerService.show(this.spinner, SpinnerOptions);
      this.updateSkillTagSubscription = this.skillTagService.updateSkillTag(this.skillTagId, updateSkillTagModel)
        .subscribe(() => {
          this.spinnerService.hide(this.spinner);
          this.activeModal.close({ needRefresh: true });
          this.showSuccessPopUpMessage('The record has been updated!');
        }, (error) => {
          this.spinnerService.hide(this.spinner);
          if (error.status === 409) {
            this.showErrorWarningPopUpMessage(error.error);
          }
        });
      this.subscriptions.push(this.updateSkillTagSubscription);
    } else {
      this.activeModal.close({ needRefresh: false });
      this.showSuccessPopUpMessage('No changes has been made!');
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
    const operationHours = this.skillTagForm.get('operationHour') as FormArray;
    operationHours.controls.forEach((operationHourGroup, index) => {
      const openTypeId = this.skillTag.operationHour[index]?.operationHourOpenTypeId;
      operationHourGroup.patchValue({
        operationHourOpenTypeId: openTypeId,
        from: this.skillTag.operationHour[index]?.from,
        to: this.skillTag.operationHour[index]?.to,
      });
      if (openTypeId === 2) {
        operationHourGroup.get('from').enable();
        operationHourGroup.get('to').enable();
      }
    });
    operationHours.updateValueAndValidity();
  }

  private showSuccessPopUpMessage(contentMessage: string) {
    const options: NgbModalOptions = {
      backdrop: false,
      centered: true,
      size: 'sm',
    };
    const modalRef = this.modalService.open(MessagePopUpComponent, options);
    modalRef.componentInstance.headingMessage = 'Success';
    modalRef.componentInstance.contentMessage = contentMessage;

    return modalRef;
  }

  private showErrorWarningPopUpMessage(contentMessage: string) {
    const options: NgbModalOptions = { backdrop: false, centered: true, size: 'sm' };
    const modalRef = this.modalService.open(ErrorWarningPopUpComponent, options);
    modalRef.componentInstance.headingMessage = 'Error';
    modalRef.componentInstance.contentMessage = contentMessage;

    return modalRef;
  }

  private hasSkillTagDetailsMismatch() {
    if (this.skillTagForm.controls.name.value !== this.skillTag.name || this.clientId !== this.skillTag.clientId ||
      this.clientLobGroupId !== this.skillTag.clientLobGroupId || this.skillGroupId !== this.skillTag.skillGroupId) {
      return true;
    } else {
      for (const index in this.weekDays) {
        if (
          this.operationHour.value[index]?.day !== this.skillTag.operationHour[index]?.day ||
          this.operationHour.value[index]?.operationHourOpenTypeId !==
          this.skillTag.operationHour[index]?.operationHourOpenTypeId ||
          this.operationHour.value[index]?.from !== this.skillTag.operationHour[index]?.from ||
          this.operationHour.value[index]?.to !== this.skillTag.operationHour[index]?.to
        ) {
          return true;
        }
      }
    }
  }

  private populateFormDetails() {
    this.skillTagForm.controls.name.setValue(this.skillTag.name);
    this.clientId = this.skillTag.clientId;
    this.clientLobGroupId = this.skillTag.clientLobGroupId;
    this.skillGroupId = this.skillTag.skillGroupId;
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

  private loadSkillTag() {
    this.spinnerService.show(this.spinner, SpinnerOptions);

    this.getSkillTagSubscription = this.skillTagService.getSkillTag(this.skillTagId)
      .subscribe((response) => {
        this.skillTag = response;
        this.populateFormDetails();
        this.spinnerService.hide(this.spinner);
      }, (error) => {
        this.spinnerService.hide(this.spinner);
        if (error.status === 409) {
          this.showErrorWarningPopUpMessage(error.error);
        }
      });
    this.subscriptions.push(this.getSkillTagSubscription);
  }

  private intializeSkillTagForm() {
    this.skillTagForm = this.formBuilder.group({
      name: new FormControl('', Validators.compose([
        Validators.required,
        Validators.maxLength(Constants.DefaultTextMaxLength),
        CustomValidators.cannotContainSpace])),
      operationHour: this.createOperationHoursArray()
    });
  }
}
