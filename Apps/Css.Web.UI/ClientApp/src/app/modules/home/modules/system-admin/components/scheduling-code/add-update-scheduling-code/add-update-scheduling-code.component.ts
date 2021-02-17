import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { NgbActiveModal, NgbModal, NgbModalOptions } from '@ng-bootstrap/ng-bootstrap';
import { NgxSpinnerService } from 'ngx-spinner';
import { SubscriptionLike as ISubscription } from 'rxjs';

import { ErrorWarningPopUpComponent } from 'src/app/shared/popups/error-warning-pop-up/error-warning-pop-up.component';

import { SchedulingCodeType } from '../../../models/scheduling-code-type.model';
import { AddSchedulingCode } from '../../../models/add-scheduling-code.model';
import { UpdateSchedulingCode } from '../../../models/update-scheduling-code.mode';
import { ComponentOperation } from 'src/app/shared/enums/component-operation.enum';

import { SpinnerOptions } from 'src/app/shared/util/spinner-options.util';
import { CustomValidators } from 'src/app/shared/util/validations.util';
import { Constants } from 'src/app/shared/util/constants.util';
import { KeyValue } from 'src/app/shared/models/key-value.model';
import { SchedulingCode } from '../../../models/scheduling-code.model';

import { SchedulingCodeService } from '../../../../../../../shared/services/scheduling-code.service';
import { SchedulingCodeIconsService } from '../../../services/scheduling-code-icons.service';
import { SchedulingCodeTypesService } from '../../../services/scheduling-code-types.service';
import { AuthService } from 'src/app/core/services/auth.service';
import { TranslationDetails } from 'src/app/shared/models/translation-details.model';
import { TranslateService } from '@ngx-translate/core';
import { ContentType } from 'src/app/shared/enums/content-type.enum';

import * as $ from 'jquery';


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
  schedulingIcons: KeyValue[] = [];
  codeList: KeyValue[] = [];

  getSchedulingCodeTypesSubscription: ISubscription;
  getSchedulingIconsSubscription: ISubscription;
  updateSchedulingCodeSubscription: ISubscription;
  addSchedulingCodeSubscription: ISubscription;
  subscriptionList: ISubscription[] = [];

  @Input() operation: ComponentOperation;
  @Input() schedulingCodeData: SchedulingCode;
  @Input() translationValues: TranslationDetails[];

  constructor(
    public translate: TranslateService,
    private formBuilder: FormBuilder,
    public activeModal: NgbActiveModal,
    private modalService: NgbModal,
    private authService: AuthService,
    private schedulingCodeService: SchedulingCodeService,
    private schedulingCodeIconsService: SchedulingCodeIconsService,
    private schedulingCodeTypesService: SchedulingCodeTypesService,
    private spinnerService: NgxSpinnerService,
  ) { }

  ngOnInit(): void {
    this.getSchedulingCodeTypes();
    this.getSchedulingCodeIcons();
    this.createSchedulingCodeForm();
    this.loadSchedulingCodeDetails();
  }

  ngOnDestroy() {
    this.subscriptionList.forEach(subscription => {
      if (subscription) {
        subscription.unsubscribe();
      }
    });
  }

  getTitle() {
    return this.translate.instant(`${ComponentOperation[(this.operation)]}`);
  }

  hasValueSelected(value) {
    const schedulingTypeCode: FormArray = this.schedulingCodeForm.get('schedulingTypeCode') as FormArray;
    return schedulingTypeCode.controls.findIndex(x => x.value === value) !== -1;
  }

  onIconSelect(icon: KeyValue) {
    this.iconId = icon.value;
    this.schedulingCodeForm.controls.iconId.setValue(icon.id);
    const item = $('[data-card-widget=\'collapse\']');
    // Find the box parent........
    const box = item.parents('.card').first();
    // Find the body and the footer
    const bf = box.find('.card-body, .card-footer');
    if (!item.children().hasClass('fa-plus')) {
      item.children('.fa-minus').removeClass('fa-minus').addClass('fa-plus');
      bf.slideUp();
    } else {
      // Convert plus into minus
      item.children('.fa-plus').removeClass('fa-plus').addClass('fa-minus');
      bf.slideDown();
    }
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
    const currentValue = this.schedulingCodeForm.controls.priorityNumber?.value;
    const charCode = (evt.which) ? evt.which : evt.keyCode;
    const isValid = currentValue.length <= 0 ? (charCode < 49 || charCode > 57) : (charCode < 48 || charCode > 57);
    if (isValid) {
      return false;
    }

    return true;
  }

  onCheckboxChange(e) {
    const schedulingTypeCode: FormArray = this.schedulingCodeForm.get('schedulingTypeCode') as FormArray;
    if (e.target.checked) {
      schedulingTypeCode.push(new FormControl(Number(e.target.value)));
    } else {
      let i = 0;
      schedulingTypeCode.controls.forEach((item: FormControl) => {
        if (item.value === Number(e.target.value)) {
          schedulingTypeCode.removeAt(i);
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
    addSchedulingCodeModel.schedulingTypeCode = this.getCodeTypes();
    addSchedulingCodeModel.createdBy = this.authService.getLoggedUserInfo()?.displayName;

    this.spinnerService.show(this.spinner, SpinnerOptions);

    this.addSchedulingCodeSubscription = this.schedulingCodeService.addSchedulingCode(addSchedulingCodeModel)
      .subscribe(() => {
        console.log(addSchedulingCodeModel);
        this.spinnerService.hide(this.spinner);
        this.activeModal.close({ needRefresh: true });
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
      updateSchedulingCodeModel.schedulingTypeCode = this.getCodeTypes();
      updateSchedulingCodeModel.modifiedBy = this.authService.getLoggedUserInfo()?.displayName;

      this.updateSchedulingCodeSubscription = this.schedulingCodeService.updateSchedulingCode
        (this.schedulingCodeData.id, updateSchedulingCodeModel)
        .subscribe(() => {
          console.log(updateSchedulingCodeModel);
          this.spinnerService.hide(this.spinner);
          this.activeModal.close({ needRefresh: true });
        }, (error) => {
          this.spinnerService.hide(this.spinner);
          if (error.status === 409) {
            this.showErrorWarningPopUpMessage(error.error);
          }
        });

      this.subscriptionList.push(this.updateSchedulingCodeSubscription);
    } else {
      this.activeModal.close({ needRefresh: false });
    }
  }

  private loadSchedulingCodeDetails() {
    if (this.operation === ComponentOperation.Edit) {
      this.schedulingCodeForm.patchValue({
        description: this.schedulingCodeData.description,
        priorityNumber: this.schedulingCodeData.priorityNumber,
        iconId: this.schedulingCodeData.icon.id,
        refId: this.schedulingCodeData.refId
      });
      this.setCodeTypes();
      this.iconId = this.schedulingCodeData.icon.value;
    }
  }

  private setCodeTypes() {
    const array = this.schedulingCodeForm.controls.schedulingTypeCode as FormArray;
    this.schedulingCodeData.schedulingTypeCode.forEach(ele => array.push(new FormControl(ele.id)));

    return array;
  }

  private getCodeTypes(): Array<SchedulingCodeType> {
    const codeTypes = new Array<SchedulingCodeType>();
    const schedulingTypeCode = this.schedulingCodeForm.controls.schedulingTypeCode as FormArray;
    schedulingTypeCode.value?.forEach(value => {
      const codeType = new SchedulingCodeType();
      codeType.schedulingCodeTypeId = value;
      codeTypes.push(codeType);
    });

    return codeTypes;
  }

  private hasSchedulingCodeDetailsMismatch() {
    for (const propertyName in this.schedulingCodeForm.value) {
      if (propertyName !== 'schedulingTypeCode' && propertyName !== 'iconId') {
        if (this.schedulingCodeForm.value[propertyName] !== this.schedulingCodeData[propertyName]) {
          return true;
        }
      } else if (propertyName === 'iconId') {
        if (this.schedulingCodeData.icon.id !== this.schedulingCodeForm.value[propertyName]) {
          return true;
        }
      } else {
        for (const index in this.codeList) {
          if (this.schedulingCodeData?.schedulingTypeCode[index]?.id !== this.schedulingCodeForm.controls.schedulingTypeCode.value[index]) {
            return true;
          }
        }
      }
    }
  }

  private showErrorWarningPopUpMessage(contentMessage: string) {
    const options: NgbModalOptions = { backdrop: 'static', centered: true, size: 'sm' };
    const modalRef = this.modalService.open(ErrorWarningPopUpComponent, options);
    modalRef.componentInstance.headingMessage = 'Error';
    modalRef.componentInstance.contentMessage = contentMessage;
    modalRef.componentInstance.messageType = ContentType.String;

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
      schedulingTypeCode: this.formBuilder.array([], Validators.required),
      iconId: new FormControl('', Validators.required),
      refId: new FormControl('', Validators.compose([
        Validators.maxLength(10)])),
    });
  }
}
