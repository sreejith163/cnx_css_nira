import { Component, Input, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
} from '@angular/forms';
import {
  NgbActiveModal,
  NgbCalendar,
  NgbDateParserFormatter,
  NgbDateStruct,
  NgbModal,
  NgbModalOptions,
} from '@ng-bootstrap/ng-bootstrap';
import { Translation } from 'src/app/shared/models/translation.model';
import { MessagePopUpComponent } from 'src/app/shared/popups/message-pop-up/message-pop-up.component';
import { DataType } from '../../enum/data-type.enum';
import { AgentCategory } from '../../models/agent-category.model';
import { AgentCategoryDropdownService } from '../../services/agent-category-dropdown.service';
import { AgentCategoryListService } from '../../services/agent-category-list.service';
import { CustomValidators } from 'src/app/shared/util/validations.util';

@Component({
  selector: 'app-add-agent-category',
  templateUrl: './add-agent-category.component.html',
  styleUrls: ['./add-agent-category.component.scss'],
})
export class AddAgentCategoryComponent implements OnInit {
  hasMismatch: boolean;
  formSubmitted: boolean;
  isEdit: boolean;
  today = this.calendar.getToday();
  dataTypes = [];
  dataType = DataType;
  agentCategoryForm: FormGroup;
  agentCategoryModel: AgentCategory;
  model: NgbDateStruct;
  rangeForm: FormGroup;
  dateRangeForm: FormGroup;

  @Input() title: string;
  @Input() agentCategoryData: AgentCategory;
  @Input() translationValues: Translation[];

  constructor(
    private agentCategoryListService: AgentCategoryListService,
    private formBuilder: FormBuilder,
    private calendar: NgbCalendar,
    public activeModal: NgbActiveModal,
    private modalService: NgbModal,
    private ngbDateParserFormatter: NgbDateParserFormatter,
    private agentCategoryDropdownService: AgentCategoryDropdownService
  ) {}

  ngOnInit(): void {
    this.agentFormIntialization();
    this.getDropdownValues();
    if (this.title === 'Edit') {
      this.isEdit = true;
      this.populateAgentDetailsOnAgentForm();
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

  addDateRangeControl(range?){
    if (this.agentCategoryForm.get('range')) {
      this.agentCategoryForm.removeControl('range');
    }
    this.agentCategoryForm.addControl('dateRange', this.dateRangeForm);
    if (range) {
      this.agentCategoryForm.get('dateRange').patchValue({
        dateMinRange: range.start,
        dateMaxRange: range.end,
      });
    }
  }

  addAlphaNumericControl(range?) {
    if (this.agentCategoryForm.get('dateRange')) {
      this.agentCategoryForm.removeControl('dateRange');
    }
    this.rangeForm.reset();
    this.agentCategoryForm.addControl('range', this.rangeForm);
    if (+this.agentCategoryForm.get('dataType').value === 3 ) {
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
      return true;
    }
  }

  canPasteValue() {
    return (this.agentCategoryForm.controls.dataType.value === '3') ? false : true;
  }

  setEndDateAsToday() {
    this.dateRangeForm.controls.dateMaxRange.patchValue(this.today);
  }

  setStartDateAsToday() {
    this.dateRangeForm.controls.dateMinRange.patchValue(this.today);
  }

  save() {
    this.formSubmitted = true;
    if (this.agentCategoryForm.valid) {
      this.saveAgentCategoryDetailsOnModel();
      if (this.isEdit) {
        this.updateAgentcategoryDetails();
      } else {
        this.addAgentCategoryDetails();
      }
    }
  }

  private addAgentCategoryDetails() {
    this.agentCategoryListService.addAgentCategory(this.agentCategoryModel);
    this.activeModal.close();
    this.showSuccessPopUpMessage('The record has been added!');
  }

  private updateAgentcategoryDetails() {
    this.matchAgentCategoryDataChanges();

    if (this.hasMismatch) {
      this.agentCategoryModel.employeeId = this.agentCategoryData.employeeId;
      this.agentCategoryModel.createdDate = this.agentCategoryData.createdDate;
      this.agentCategoryModel.createdBy = this.agentCategoryData.createdBy;
      this.agentCategoryListService.updateAgentCategrory(this.agentCategoryModel);
      this.activeModal.close();
      this.showSuccessPopUpMessage('The record has been updated!');
    } else {
      this.activeModal.close();
      this.showSuccessPopUpMessage('No changes has been made!');
    }
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

  private matchAgentCategoryDataChanges() {
    if (this.agentCategoryModel.descriptionOrName !== this.agentCategoryData.descriptionOrName ||
      this.agentCategoryModel.dataType !== this.agentCategoryData.dataType ||
      this.agentCategoryModel.range.end !== this.agentCategoryData.range.end ||
      this.agentCategoryModel.range.start !== this.agentCategoryData.range.start) {
      this.hasMismatch = true;
    }
  }

  private saveAgentCategoryDetailsOnModel() {
    this.agentCategoryModel = new AgentCategory();
    if (this.agentCategoryForm.get('range')) {
      this.agentCategoryModel.range = {
        start: this.agentCategoryForm.get('range')
        .get('minRange').value,
        end: this.agentCategoryForm.get('range')
        .get('maxRange').value
      };
    } else {
      this.agentCategoryModel.range = {
        start: this.ngbDateParserFormatter
        .format(this.agentCategoryForm.get('dateRange')
        .get('dateMinRange').value),
        end: this.ngbDateParserFormatter
        .format(this.agentCategoryForm.get('dateRange')
        .get('dateMaxRange').value)
      };
    }
    this.agentCategoryModel.dataType = +this.agentCategoryForm.controls.dataType.value;
    this.agentCategoryModel.descriptionOrName = this.agentCategoryForm.controls.descriptionOrName.value;
  }

  rangeRequiredError(parentControl, control) {
    return this.formSubmitted &&
    this.agentCategoryForm.get(parentControl).get(control)?.errors?.required;
  }

  private populateAgentDetailsOnAgentForm() {
    let range;
    this.agentCategoryForm.controls.dataType.setValue(
      this.agentCategoryData.dataType
    );
    if (this.agentCategoryData.dataType === 2) {
      range = {
        start: this.getDateStruct(this.agentCategoryData.range.start),
        end: this.getDateStruct(this.agentCategoryData.range.end),
      };
      this.addDateRangeControl(range);
    } else {
      range = {
        start: this.agentCategoryData.range.start,
        end: this.agentCategoryData.range.end,
      };
      this.addAlphaNumericControl(range);
    }
    this.agentCategoryForm.controls.descriptionOrName.setValue(
      this.agentCategoryData.descriptionOrName
    );
  }

  getDateStruct(value) {
    const date = new Date(value);
    const ngbDateStruct: NgbDateStruct = {
      day: date.getUTCDate(),
      month: date.getUTCMonth() + 1,
      year: date.getUTCFullYear(),
    };
    return ngbDateStruct;
  }

  private getDropdownValues() {
    this.dataTypes = this.agentCategoryDropdownService.getAgentCategoryDataTypes();
  }

  private agentFormIntialization() {
    this.agentCategoryForm = this.formBuilder.group({
      descriptionOrName: new FormControl('', Validators.compose([Validators.required, CustomValidators.cannotContainSpace])),
      dataType: new FormControl('', Validators.required),
    });

    this.dateRangeForm = this.formBuilder.group({
      dateMinRange: new FormControl('', Validators.required),
      dateMaxRange: new FormControl('', Validators.required)
    }, { validator: [
      CustomValidators.fromToDate('dateMinRange', 'dateMaxRange')
    ]});

    this.rangeForm = this.formBuilder.group({
      minRange: new FormControl('', Validators.compose([Validators.required, Validators.maxLength(40)])),
      maxRange: new FormControl('', Validators.compose([Validators.required, Validators.maxLength(40)])),
    });
  }
}
