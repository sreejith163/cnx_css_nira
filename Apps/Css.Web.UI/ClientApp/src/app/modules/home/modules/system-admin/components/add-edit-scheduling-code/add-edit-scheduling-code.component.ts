import { Component, Input, OnInit } from '@angular/core';
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
import { Translation } from 'src/app/shared/models/translation.model';
import { MessagePopUpComponent } from 'src/app/shared/popups/message-pop-up/message-pop-up.component';
import { Constants } from 'src/app/shared/util/constants.util';
import { CustomValidators } from 'src/app/shared/util/validations.util';
import { SchedulingCodeType } from '../../models/scheduling-code-type.model';
import { SchedulingCode } from '../../models/scheduling-code.model';
import { SchedulingCodeService } from '../../services/scheduling-code.service';
import { ICON_DB } from 'src/app/shared/util/icon.data';

@Component({
  selector: 'app-add-edit-scheduling-code',
  templateUrl: './add-edit-scheduling-code.component.html',
  styleUrls: ['./add-edit-scheduling-code.component.scss'],
})
export class AddEditSchedulingCodeComponent implements OnInit {

  formSubmitted: boolean;
  isEdit: boolean;
  hasMismatch: boolean;
  scheduleIcon: string;
  iconData = ICON_DB;
  schedulingCodeForm: FormGroup;
  schedulingCodeModel: SchedulingCode;
  codeList: SchedulingCodeType[] = [];

  @Input() title: string;
  @Input() schedulingCodeData: SchedulingCode;
  @Input() translationValues: Translation[];

  constructor(
    private formBuilder: FormBuilder,
    public activeModal: NgbActiveModal,
    private modalService: NgbModal,
    private schedulingCodeService: SchedulingCodeService
  ) { }

  ngOnInit(): void {
    this.codeList = Constants.SchedulingCodes;
    this.schedulingCodeFormIntialization();
    this.checkAddOrEditSchedulingCodeDetails();
  }

  hasValueSelected(value) {
    const typesOfCode: FormArray = this.schedulingCodeForm.get('typesOfCode') as FormArray;
    return typesOfCode.controls.findIndex(x => x.value === value) !== -1;
  }

  onIconSelect(icon: any) {
    this.scheduleIcon = icon;
    this.schedulingCodeForm.controls.scheduleIcon.setValue(icon);
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
    if ((charCode < 49 || charCode > 57)) {
      return false;
    }

    return true;
  }

  onCheckboxChange(e) {
    const typesOfCode: FormArray = this.schedulingCodeForm.get('typesOfCode') as FormArray;
    if (e.target.checked) {
      typesOfCode.push(new FormControl(e.target.value));
    } else {
      let i = 0;
      typesOfCode.controls.forEach((item: FormControl) => {
        if (item.value === e.target.value) {
          typesOfCode.removeAt(i);
          return;
        }
        i++;
      });
    }
  }

  save() {
    this.formSubmitted = true;
    if (this.schedulingCodeForm.valid) {
      this.saveSchedulingCodeDetailsOnModel();
      if (this.isEdit) {
        this.updateSchedulingCodeDetails();
      } else {
        this.addSchedulingCodeDetails();
      }
    }
  }

  private addSchedulingCodeDetails() {
    this.schedulingCodeService.addSchedulingCode(this.schedulingCodeModel);
    this.activeModal.close();
    this.showSuccessPopUpMessage('The record has been added!');
  }

  private updateSchedulingCodeDetails() {
    this.matchSchedulingCodeDataChanges();
    this.matchTypesOfCode();

    if (this.hasMismatch) {
      this.schedulingCodeModel.id = this.schedulingCodeData.id;
      this.schedulingCodeModel.refId = this.schedulingCodeData.refId;
      this.schedulingCodeService.updateSchedulingCode(this.schedulingCodeModel);
      this.activeModal.close();
      this.showSuccessPopUpMessage('The record has been updated!');
    } else {
      this.activeModal.close();
      this.showSuccessPopUpMessage('No changes has been made!');
    }
  }

  private checkAddOrEditSchedulingCodeDetails() {
    if (this.title === 'Edit') {
      this.isEdit = true;
      this.populateSchedulingGroupDetailsOnForm();
    }
  }

  private populateSchedulingGroupDetailsOnForm() {
    this.schedulingCodeForm.patchValue({
      description: this.schedulingCodeData.description,
      priorityNo: this.schedulingCodeData.priorityNo,
      scheduleIcon: this.schedulingCodeData.scheduleIcon
    });
    this.setCodeValue();
    this.scheduleIcon = this.schedulingCodeData.scheduleIcon;
  }

  private setCodeValue() {
    const array = this.schedulingCodeForm.controls.typesOfCode as FormArray;
    this.schedulingCodeData.types.forEach(ele => array.push(new FormControl(ele.value)));

    return array;
  }

  private matchSchedulingCodeDataChanges() {
    for (const propertyName in this.schedulingCodeForm.value) {
      if (
        this.schedulingCodeForm.value[propertyName] !==
        this.schedulingCodeData[propertyName] &&
        propertyName !== 'typesOfCode'
      ) {
        this.hasMismatch = true;
        break;
      }
    }
  }

  private matchTypesOfCode() {
    for (const i in this.codeList) {
      if (this.schedulingCodeData?.types[i]?.value !== this.schedulingCodeModel?.types[i]?.value) {
        this.hasMismatch = true;
      }
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

  private saveSchedulingCodeDetailsOnModel() {
    this.schedulingCodeModel = new SchedulingCode();
    this.schedulingCodeModel.description = this.schedulingCodeForm.controls.description.value;
    this.schedulingCodeModel.priorityNo = this.schedulingCodeForm.controls.priorityNo.value;
    this.schedulingCodeModel.types = this.addCodes(
      this.schedulingCodeForm.value.typesOfCode
    );
    this.schedulingCodeModel.scheduleIcon = this.schedulingCodeForm.controls.scheduleIcon.value;
  }

  private addCodes(formValue) {
    const checkArray: FormArray = this.schedulingCodeForm.get('typesOfCode') as FormArray;
    const codeDetails: SchedulingCodeType[] = [];
    for (let i = 0; i < checkArray.length; i++) {
      if (this.isEdit) {
        if (formValue[i] !== this.schedulingCodeData.types[i]?.value) {
          this.hasMismatch = true;
        }
      }

      const schedulingGroupOperatingHours = new SchedulingCodeType();
      schedulingGroupOperatingHours.value = formValue[i];
      codeDetails.push(schedulingGroupOperatingHours);
    }

    return codeDetails;
  }

  private schedulingCodeFormIntialization() {
    this.schedulingCodeForm = this.formBuilder.group({
      description: new FormControl('', Validators.compose([Validators.required, CustomValidators.cannotContainSpace])),
      priorityNo: new FormControl('', Validators.required),
      typesOfCode: this.formBuilder.array([]),
      scheduleIcon: new FormControl(''),
    });
  }
}
