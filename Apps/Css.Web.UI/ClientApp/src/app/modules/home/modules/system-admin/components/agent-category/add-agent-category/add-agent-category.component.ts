import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  Validators
} from '@angular/forms';
import {
  NgbActiveModal,
  NgbCalendar,
  NgbDateParserFormatter,
  NgbDateStruct,
  NgbModal,
  NgbModalOptions
} from '@ng-bootstrap/ng-bootstrap';
import { NgxSpinnerService } from 'ngx-spinner';
import { SubscriptionLike as ISubscription } from 'rxjs';
import { AuthService } from 'src/app/core/services/auth.service';
import { ComponentOperation } from 'src/app/shared/enums/component-operation.enum';
import { ContentType } from 'src/app/shared/enums/content-type.enum';
import { TranslationDetails } from 'src/app/shared/models/translation-details.model';
import { ErrorWarningPopUpComponent } from 'src/app/shared/popups/error-warning-pop-up/error-warning-pop-up.component';
import { Constants } from 'src/app/shared/util/constants.util';
import { SpinnerOptions } from 'src/app/shared/util/spinner-options.util';
import { CustomValidators } from 'src/app/shared/util/validations.util';
import { DataType } from '../../../enum/data-type.enum';
import { AddAgentCategory } from '../../../models/add-agent-category.model';
import { AgentCategoryBase } from '../../../models/agent-category-base.model';
import { AgentCategoryDetails } from '../../../models/agent-category-details.model';
import { UpdateAgentCategory } from '../../../models/update-agent-category.model';
import { AgentCategoryService } from '../../../services/agent-category.service';

@Component({
  selector: 'app-add-agent-category',
  templateUrl: './add-agent-category.component.html',
  styleUrls: ['./add-agent-category.component.scss'],
})
export class AddAgentCategoryComponent implements OnInit, OnDestroy {
  hasMismatch: boolean;
  formSubmitted: boolean;
  spinner = 'spinner';
  today = this.calendar.getToday();
  dataTypes = [];
  dataType = DataType;
  agentCategoryForm: FormGroup;
  agentCategoryModel: AgentCategoryBase;
  model: NgbDateStruct;
  rangeForm: FormGroup;
  dateRangeForm: FormGroup;
  agentCategory: AgentCategoryDetails;
  maxLength = Constants.DefaultTextMaxLength;

  getAgentCategorySubscription: ISubscription;
  addAgentCategorySubscription: ISubscription;
  updateAgentCategorySubscription: ISubscription;
  subscriptionList: ISubscription[] = [];

  @Input() agentCategoryId: number;
  @Input() operation: ComponentOperation;
  @Input() translationValues: TranslationDetails[];

  constructor(
    private agentCategoryService: AgentCategoryService,
    private authService: AuthService,
    private spinnerService: NgxSpinnerService,
    private formBuilder: FormBuilder,
    private calendar: NgbCalendar,
    public activeModal: NgbActiveModal,
    private modalService: NgbModal,
    private ngbDateParserFormatter: NgbDateParserFormatter
  ) { }

  ngOnInit(): void {
    this.agentFormIntialization();
    this.getDropdownValues();
    if (this.operation === ComponentOperation.Edit) {
      this.loadAgentCategory();
    }

    this.agentCategoryForm.get('dataType').valueChanges.subscribe((data) => {
      if (data) {
        if (parseInt(data, 10) === 2) {
          this.addDateRangeControl();
        } else {
          this.addAlphaNumericControl();
        }
      }
    });
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

  rangeRequiredError(parentControl, control) {
    return this.formSubmitted &&
      this.agentCategoryForm.get(parentControl).get(control)?.errors?.required;
  }

  save() {
    this.formSubmitted = true;
    if (this.agentCategoryForm.valid && this.numericRangeValidation() && this.aplhaNumericRangeValidation() && this.aplhaNumericMinRangeValidation() && this.aplhaNumericMaxRangeValidation()) {
      this.saveAgentCategoryDetailsOnModel();
      this.operation === ComponentOperation.Edit ? this.updateAgentcategoryDetails() : this.addAgentCategoryDetails();
    }
  }

  private addAgentCategoryDetails() {
    const addAgentCategoryModel = this.agentCategoryModel as AddAgentCategory;
    addAgentCategoryModel.createdBy = this.authService.getLoggedUserInfo()?.displayName;

    this.spinnerService.show(this.spinner, SpinnerOptions);
    this.addAgentCategorySubscription = this.agentCategoryService.addAgentcategory(addAgentCategoryModel)
      .subscribe(() => {
        this.spinnerService.hide(this.spinner);
        this.activeModal.close();
      }, (error) => {
        this.spinnerService.hide(this.spinner);
        if (error.status === 409) {
          this.showErrorWarningPopUpMessage(error.error);
        }
        else {
          this.showErrorWarningPopUpMessage('Please fill the fields with correct details');
        }
      });

    this.subscriptionList.push(this.addAgentCategorySubscription);
  }

  private updateAgentcategoryDetails() {
    if (this.hasAgentCategoryDetailsMismatch()) {
      const updateAgentCategoryModel = this.agentCategoryModel as UpdateAgentCategory;
      updateAgentCategoryModel.modifiedBy = this.authService.getLoggedUserInfo().displayName;

      this.spinnerService.show(this.spinner, SpinnerOptions);
      this.updateAgentCategorySubscription = this.agentCategoryService.updateAgentCategory(
        this.agentCategoryId, updateAgentCategoryModel)
        .subscribe(() => {
          this.spinnerService.hide(this.spinner);
          this.activeModal.close({ needRefresh: true });
        }, (error) => {
          this.spinnerService.hide(this.spinner);
          if (error.status === 409) {
            this.showErrorWarningPopUpMessage(error.error);
          }
        });
      this.subscriptionList.push(this.updateAgentCategorySubscription);
    } else {
      this.activeModal.close({ needRefresh: false });
    }
  }

  numericRangeValidation() {
    if (+this.agentCategoryForm.get('dataType').value === 3 &&
      this.rangeForm.controls.minRange.value && this.rangeForm.controls.maxRange.value) {
      if (+this.rangeForm.controls.minRange.value >= +this.rangeForm.controls.maxRange.value) {
        return false;
      }
    }

    return true;
  }

  aplhaNumericRangeValidation() {
    if (+this.agentCategoryForm.get('dataType').value === 1 &&
      this.rangeForm.controls.minRange.value && this.rangeForm.controls.maxRange.value) {
      if (+this.rangeForm.controls.minRange.value === +this.rangeForm.controls.maxRange.value) {
        return false;
      }
    }

    return true;
  }

  aplhaNumericMinRangeValidation() {
    if (+this.agentCategoryForm.get('dataType').value === 1 &&
      this.rangeForm.controls.minRange.value && this.rangeForm.controls.maxRange.value) {
      if (!isNaN(+this.rangeForm.controls.minRange.value) && +this.rangeForm.controls.minRange.value <= 0) {
        return false;
      }
    } else if (+this.agentCategoryForm.get('dataType').value === 2) {
      return true;
    } else if (this.rangeForm.controls.minRange.value === "") {
      return false;
    }

    return true;
  }

  aplhaNumericMaxRangeValidation() {
    if (+this.agentCategoryForm.get('dataType').value === 1 &&
      this.rangeForm.controls.minRange.value && this.rangeForm.controls.maxRange.value) {
      if (!isNaN(+this.rangeForm.controls.maxRange.value) && +this.rangeForm.controls.maxRange.value <= 0) {
        return false;
      }
    } else if (+this.agentCategoryForm.get('dataType').value === 2) {
      return true;
    } else if (this.rangeForm.controls.maxRange.value === "") {
      return false;
    }

    return true;
  }

  addAlphaNumericControl(range?) {
    if (this.agentCategoryForm.get('dateRange')) {
      this.agentCategoryForm.removeControl('dateRange');
    }
    this.rangeForm.reset();
    this.agentCategoryForm.addControl('range', this.rangeForm);
    if (+this.agentCategoryForm.get('dataType').value === 3 &&
      this.rangeForm.controls.minRange.value && this.rangeForm.controls.maxRange.value) {
      this.agentCategoryForm.get('range').setValidators([
        CustomValidators.rangeValidator('minRange', 'maxRange')
      ]);
      this.agentCategoryForm.get('range').updateValueAndValidity();
    } else {
      this.agentCategoryForm.get('range').clearValidators();
      this.agentCategoryForm.get('range').updateValueAndValidity();
    }
    if (range) {
      this.agentCategoryForm.get('range').patchValue({
        minRange: range.start,
        maxRange: range.end,
      });
    }
  }

  get form() {
    return this.agentCategoryForm.controls;
  }

  hasFormControlValidationError(control: string) {
    return (
      this.formSubmitted &&
      this.agentCategoryForm.controls[control].errors?.required
    );
  }

  checkValidity(event) {
    if (+this.agentCategoryForm.get('dataType').value === 3) {
      const charCode = (event.which) ? event.which : event.keyCode;
      if ((charCode < 48 || charCode > 57)) {
        return false;
      }
    } else {
      if (+this.agentCategoryForm.get('dataType').value === 1) {
        const charCode = (event.which) ? event.which : event.keyCode;
        if ((!(charCode > 47 && charCode < 58) &&
          !(charCode > 64 && charCode < 91) &&
          !(charCode > 96 && charCode < 123))) {
          return false;
        }
      }
      return true;
    }
  }

  canPasteValue() {
    return (this.agentCategoryForm.controls.dataType.value === '3') ? false : true;
  }

  // setEndDateAsToday() {
  //   this.dateRangeForm.controls.dateMaxRange.patchValue(this.today);
  // }

  // setStartDateAsToday() {
  //   this.dateRangeForm.controls.dateMinRange.patchValue(this.today);
  // }

  private showErrorWarningPopUpMessage(contentMessage: string) {
    const options: NgbModalOptions = { backdrop: 'static', centered: true, size: 'sm' };
    const modalRef = this.modalService.open(ErrorWarningPopUpComponent, options);
    modalRef.componentInstance.headingMessage = 'Error';
    modalRef.componentInstance.contentMessage = contentMessage;
    modalRef.componentInstance.messageType = ContentType.String;

    return modalRef;
  }

  private addDateRangeControl(range?: any) {
    if (this.agentCategoryForm.get('range')) {
      this.agentCategoryForm.removeControl('range');
    }
    // this.agentCategoryForm.addControl('dateRange', this.dateRangeForm);
    // if (range) {
    //   this.agentCategoryForm.get('dateRange').patchValue({
    //     dateMinRange: range.start,
    //     dateMaxRange: range.end,
    //   });
    // }
  }

  private hasAgentCategoryDetailsMismatch() {
    if (this.agentCategoryModel.name !== this.agentCategory.name ||
      this.agentCategoryModel.dataTypeId !== this.agentCategory.dataTypeId ||
      this.agentCategoryModel.dataTypeMaxValue !== this.agentCategory.dataTypeMaxValue ||
      this.agentCategoryModel.dataTypeMinValue !== this.agentCategory.dataTypeMinValue) {
      return true;
    }
  }

  private saveAgentCategoryDetailsOnModel() {
    this.agentCategoryModel = new AgentCategoryBase();
    if (this.agentCategoryForm.get('range')) {
      this.agentCategoryModel.dataTypeMinValue = this.agentCategoryForm.get('range')
        .get('minRange').value;
      this.agentCategoryModel.dataTypeMaxValue = this.agentCategoryForm.get('range')
        .get('maxRange').value;
    }
    // else {
    //   this.agentCategoryModel.dataTypeMinValue = this.ngbDateParserFormatter
    //     .format(this.agentCategoryForm.get('dateRange')
    //       .get('dateMinRange').value);
    //   this.agentCategoryModel.dataTypeMaxValue = this.ngbDateParserFormatter
    //     .format(this.agentCategoryForm.get('dateRange')
    //       .get('dateMinRange').value);
    // }

    this.agentCategoryModel.dataTypeId = +this.agentCategoryForm.controls.dataType.value;
    this.agentCategoryModel.name = this.agentCategoryForm.controls.descriptionOrName.value;
  }

  private populateAgentDetailsOnAgentForm() {
    let range;
    this.agentCategoryForm.controls.dataType.setValue(
      this.agentCategory.dataTypeId
    );
    if (this.agentCategory.dataTypeId === 2) {
      range = {
        start: this.getDateStruct(this.agentCategory.dataTypeMinValue),
        end: this.getDateStruct(this.agentCategory.dataTypeMaxValue),
      };
      this.addDateRangeControl(range);
    } else {
      range = {
        start: this.agentCategory.dataTypeMinValue,
        end: this.agentCategory.dataTypeMaxValue,
      };
      this.addAlphaNumericControl(range);
    }
    this.agentCategoryForm.controls.descriptionOrName.setValue(
      this.agentCategory.name
    );
  }

  private getDateStruct(value) {
    const date = new Date(value);
    const ngbDateStruct: NgbDateStruct = {
      day: date.getUTCDate(),
      month: date.getUTCMonth() + 1,
      year: date.getUTCFullYear(),
    };
    return ngbDateStruct;
  }

  private getDropdownValues() {
    this.dataTypes = Object.keys(DataType).filter(f => !isNaN(Number(f)));
  }

  private loadAgentCategory() {
    this.spinnerService.show(this.spinner, SpinnerOptions);

    this.getAgentCategorySubscription = this.agentCategoryService.getAgentCategory(this.agentCategoryId.toString())
      .subscribe((response) => {
        this.agentCategory = response;
        this.populateAgentDetailsOnAgentForm();
        this.spinnerService.hide(this.spinner);
      }, (error) => {
        this.spinnerService.hide(this.spinner);
        if (error.status === 409) {
          this.showErrorWarningPopUpMessage(error.error);
        }
      });
    this.subscriptionList.push(this.getAgentCategorySubscription);
  }

  private agentFormIntialization() {
    this.agentCategoryForm = this.formBuilder.group({
      descriptionOrName: new FormControl('', Validators.compose([
        Validators.required,
        Validators.maxLength(50),
        CustomValidators.cannotContainSpace])),
      dataType: new FormControl('', Validators.required),
    });

    // this.dateRangeForm = this.formBuilder.group({
    //   dateMinRange: new FormControl('', Validators.required)
    // }
    // );

    this.rangeForm = this.formBuilder.group({
      minRange: new FormControl('', Validators.maxLength(40)),
      maxRange: new FormControl('', Validators.maxLength(40)),
    });
  }
}
