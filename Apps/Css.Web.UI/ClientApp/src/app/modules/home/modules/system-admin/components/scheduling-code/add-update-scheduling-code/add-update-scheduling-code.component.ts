import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { NgbActiveModal, NgbModal, NgbModalOptions } from '@ng-bootstrap/ng-bootstrap';
import { NgxSpinnerService } from 'ngx-spinner';
import { SubscriptionLike as ISubscription } from 'rxjs';

import { MessagePopUpComponent } from 'src/app/shared/popups/message-pop-up/message-pop-up.component';

import { Translation } from 'src/app/shared/models/translation.model';
import { SchedulingCodeType } from '../../../models/scheduling-code-type.model';
import { AddSchedulingCode } from '../../../models/add-scheduling-code.model';
import { UpdateSchedulingCode } from '../../../models/update-scheduling-code.mode';
import { SchedulingIcon } from '../../../models/scheduling-icon.model';
import { SchedulingCodeDetails } from '../../../models/scheduling-code-details.model';
import { ComponentOperation } from 'src/app/shared/enums/component-operation.enum';

import { SpinnerOptions } from 'src/app/shared/util/spinner-options.util';
import { CustomValidators } from 'src/app/shared/util/validations.util';
import { Constants } from 'src/app/shared/util/constants.util';

import { SchedulingCodeService } from '../../../services/scheduling-code.service';
import { SchedulingCodeIconsService } from '../../../services/scheduling-code-icons.service';
import { SchedulingCodeTypesService } from '../../../services/scheduling-code-types.service';
import { ErrorWarningPopUpComponent } from 'src/app/shared/popups/error-warning-pop-up/error-warning-pop-up.component';


@Component({
  selector: 'app-add-update-scheduling-code',
  templateUrl: './add-update-scheduling-code.component.html',
  styleUrls: ['./add-update-scheduling-code.component.scss']
})
export class AddUpdateSchedulingCodeComponent implements OnInit, OnDestroy {

  maxLength = Constants.DefaultTextMaxLength;
  maxPriority = 10;
  formSubmitted: boolean;
  iconId: string;
  spinner = 'modalSpinner';

  schedulingCodeForm: FormGroup;
  schedulingIcons: SchedulingIcon[] = [];
  codeList: SchedulingCodeType[] = [];

  getSchedulingCodeTypesSubscription: ISubscription;
  getSchedulingIconsSubscription: ISubscription;
  updateSchedulingCodeSubscription: ISubscription;
  addSchedulingCodeSubscription: ISubscription;
  subscriptionList: ISubscription[] = [];

  @Input() operation: ComponentOperation;
  @Input() schedulingCodeData: SchedulingCodeDetails;
  @Input() translationValues: Translation[];

  constructor(
    private formBuilder: FormBuilder,
    public activeModal: NgbActiveModal,
    private modalService: NgbModal,
    private schedulingCodeService: SchedulingCodeService,
    private schedulingCodeIconsService: SchedulingCodeIconsService,
    private schedulingCodeTypesService: SchedulingCodeTypesService,
    private spinnerService: NgxSpinnerService,
  ) { }

  ngOnInit(): void {
    this.getSchedulingCodeTypes();
    this.getSchedulingCodeIcons();
    this.createSchedulingCodeForm();
    this.loadExistingSchedulingCodeDetails();
  }

  ngOnDestroy() {
    this.subscriptionList.forEach(subscription => {
      if (subscription) {
        subscription.unsubscribe();
      }
    });
  }

  getTitle() {
    return ComponentOperation[this.operation];
  }

  hasValueSelected(value) {
    const codeTypes: FormArray = this.schedulingCodeForm.get('codeTypes') as FormArray;
    return codeTypes.controls.findIndex(x => x.value === value) !== -1;
  }

  onIconSelect(icon: SchedulingIcon) {
    this.iconId = icon.value;
    this.schedulingCodeForm.controls.iconId.setValue(icon.id);
  }

  unifiedToNative(unified: string) {
    const codePoints = unified.split('-').map(u => parseInt(`0x${u}`, 16));
    return String.fromCodePoint(...codePoints);
  }

  hasFormControlValidationError(control: string) {
    return (
      this.formSubmitted &&
      this.schedulingCodeForm.controls[control].errors?.required
    );
  }

  isNumberKey(evt) {
    const charCode = (evt.which) ? evt.which : evt.keyCode;
    if ((charCode < 48 || charCode > 57)) {
      return false;
    }

    return true;
  }

  onCheckboxChange(e) {
    const codeTypes: FormArray = this.schedulingCodeForm.get('codeTypes') as FormArray;
    if (e.target.checked) {
      codeTypes.push(new FormControl(Number(e.target.value)));
    } else {
      let i = 0;
      codeTypes.controls.forEach((item: FormControl) => {
        if (item.value === Number(e.target.value)) {
          codeTypes.removeAt(i);
          return;
        }
        i++;
      });
    }
  }

  saveSchedulingCodeDetails() {
    this.formSubmitted = true;
    if (this.schedulingCodeForm.valid) {
      this.operation === ComponentOperation.Edit ? this.updateSchedulingCodeDetails() : this.addSchedulingCodeDetails();
    }
  }

  private addSchedulingCodeDetails() {
    const addSchedulingCodeModel = this.schedulingCodeForm.value as AddSchedulingCode;
    addSchedulingCodeModel.createdBy = 'User';

    this.spinnerService.show(this.spinner, SpinnerOptions);

    this.addSchedulingCodeSubscription = this.schedulingCodeService.addSchedulingCode(addSchedulingCodeModel)
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

    this.subscriptionList.push(this.addSchedulingCodeSubscription);
  }

  private updateSchedulingCodeDetails() {
    if (this.hasSchedulingCodeDetailsMismatch()) {
      this.spinnerService.show(this.spinner, SpinnerOptions);

      const updateSchedulingCodeModel = this.schedulingCodeForm.value as UpdateSchedulingCode;
      updateSchedulingCodeModel.refId = 1;
      updateSchedulingCodeModel.modifiedBy = 'User';

      this.updateSchedulingCodeSubscription = this.schedulingCodeService.updateSchedulingCode
        (this.schedulingCodeData.id, updateSchedulingCodeModel)
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

      this.subscriptionList.push(this.updateSchedulingCodeSubscription);
    } else {
      this.activeModal.close({ needRefresh: false });
      this.showSuccessPopUpMessage('No changes has been made!');
    }
  }

  private loadExistingSchedulingCodeDetails() {
    if (this.operation === ComponentOperation.Edit) {
      this.schedulingCodeForm.patchValue({
        description: this.schedulingCodeData.description,
        priorityNumber: this.schedulingCodeData.priorityNumber,
        iconId: this.schedulingCodeData.icon.id
      });
      this.setCodeValue();
      this.iconId = this.schedulingCodeData.icon.value;
    }
  }

  private setCodeValue() {
    const array = this.schedulingCodeForm.controls.codeTypes as FormArray;
    this.schedulingCodeData.schedulingTypeCode.forEach(ele => array.push(new FormControl(ele.id)));

    return array;
  }

  private hasSchedulingCodeDetailsMismatch() {
    for (const propertyName in this.schedulingCodeForm.value) {
      if (propertyName !== 'codeTypes' && propertyName !== 'iconId') {
        if (this.schedulingCodeForm.value[propertyName] !== this.schedulingCodeData[propertyName]) {
          return true;
        }
      } else if (propertyName === 'iconId') {
        if (this.schedulingCodeData.icon.id !== this.schedulingCodeForm.value[propertyName]) {
          return true;
        }
      } else {
        for (const index in this.codeList) {
          if (this.schedulingCodeData?.schedulingTypeCode[index]?.id !== this.schedulingCodeForm.controls.codeTypes.value[index]) {
            return true;
          }
        }
      }
    }
  }

  private showErrorWarningPopUpMessage(contentMessage: string) {
    const options: NgbModalOptions = { backdrop: false, centered: true, size: 'sm' };
    const modalRef = this.modalService.open(ErrorWarningPopUpComponent, options);
    modalRef.componentInstance.headingMessage = 'Error';
    modalRef.componentInstance.contentMessage = contentMessage;

    return modalRef;
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

  private getSchedulingCodeIcons() {
    this.getSchedulingIconsSubscription = this.schedulingCodeIconsService.getSchedulingIcons()
      .subscribe((response) => {
        this.schedulingIcons = response;
      }, (error) => {
        console.log(error);
      });

    this.subscriptionList.push(this.getSchedulingIconsSubscription);
  }

  private getSchedulingCodeTypes() {
    this.getSchedulingCodeTypesSubscription = this.schedulingCodeTypesService.getSchedulingCodeTypes()
      .subscribe((response) => {
        this.codeList = response;
      }, (error) => {
        console.log(error);
      });

    this.subscriptionList.push(this.getSchedulingCodeTypesSubscription);
  }

  private createSchedulingCodeForm() {
    this.schedulingCodeForm = this.formBuilder.group({
      description: new FormControl('', Validators.compose([
        Validators.required,
        Validators.maxLength(50),
        CustomValidators.cannotContainSpace])),
      priorityNumber: new FormControl('', Validators.compose([
        Validators.required,
        Validators.maxLength(10)])),
      codeTypes: this.formBuilder.array([], Validators.required),
      iconId: new FormControl('', Validators.required),
    });
  }
}
