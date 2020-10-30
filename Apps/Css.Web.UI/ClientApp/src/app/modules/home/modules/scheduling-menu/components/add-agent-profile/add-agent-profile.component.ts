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
import { CustomValidators } from 'src/app/shared/util/validations.util';
import { AgentAdmin } from '../../models/agent-admin.model';
import { AgentSchedulingGroupName } from '../../models/agent-scheduling-group-name.model';
import { EmployeeId } from '../../models/employee-id.model';
import { AgentAdminDropdownsService } from '../../services/agent-admin-dropdowns.service';
import { AgentAdminListService } from '../../services/agent-admin-list.service';

@Component({
  selector: 'app-add-agent-profile',
  templateUrl: './add-agent-profile.component.html',
  styleUrls: ['./add-agent-profile.component.css'],
})
export class AddAgentProfileComponent implements OnInit {

  hasMismatch: boolean;
  isEdit: boolean;
  formSubmitted: boolean;
  today = this.calendar.getToday();
  agentProfileModel: AgentAdmin;
  agentProfileForm: FormGroup;
  model: NgbDateStruct;
  agentSchedulingGroupNames: AgentSchedulingGroupName[] = [];
  employeeIdList: EmployeeId[] = [];

  @Input() employeeId: number;
  @Input() title: string;
  @Input() agentProfileData: AgentAdmin;
  @Input() translationValues: Translation[];

  constructor(
    private formBuilder: FormBuilder,
    private calendar: NgbCalendar,
    public activeModal: NgbActiveModal,
    private modalService: NgbModal,
    private ngbDateParserFormatter: NgbDateParserFormatter,
    private agentDropdownService: AgentAdminDropdownsService,
    private agentAdminService: AgentAdminListService
  ) { }

  ngOnInit(): void {
    this.agentFormIntialization();
    this.getDropdownValues();
    if (this.title === 'Edit') {
      this.isEdit = true;
      this.populateAgentDetailsOnAgentForm();
    }
  }

  get form() {
    return this.agentProfileForm.controls;
  }

  save() {
    this.formSubmitted = true;
    if (this.agentProfileForm.valid) {
      this.saveAgentProfileDetailsOnModel();
      if (this.isEdit) {
        this.updateAgentAdminProfileDetails();
      } else {
        this.addAgentAdminProfileDetails();
      }
    }
  }

  hasFormControlValidationError(control: string) {
    return (
      this.formSubmitted &&
      this.agentProfileForm.controls[control].errors?.required
    );
  }

  setHireDateAsToday() {
    this.agentProfileForm.controls.hireDate.patchValue(this.today);
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

  private saveAgentProfileDetailsOnModel() {
    this.agentProfileModel = new AgentAdmin();
    this.agentProfileModel = this.agentProfileForm.value;
    const myDate = this.ngbDateParserFormatter.format(this.agentProfileForm.controls.hireDate.value);
    this.agentProfileModel.hireDate = myDate;
  }

  private addAgentAdminProfileDetails() {
    this.agentAdminService.addAgentAdmin(this.agentProfileModel);
    this.activeModal.close();
    this.showSuccessPopUpMessage('The record has been added!');
  }

  private updateAgentAdminProfileDetails() {
    this.matchAgentProfileDataChanges();

    if (this.hasMismatch) {
      this.agentProfileModel.id = this.agentProfileData.id;
      this.agentProfileModel.createdDate = this.agentProfileData.createdDate;
      this.agentProfileModel.createdBy = this.agentProfileData.createdBy;
      this.agentAdminService.updateAgentAdmin(this.agentProfileModel);
      this.activeModal.close();
      this.showSuccessPopUpMessage('The record has been updated!');
    } else {
      this.activeModal.close();
      this.showSuccessPopUpMessage('No changes has been made!');
    }
  }

  private matchAgentProfileDataChanges() {
    for (const propertyName in this.agentProfileForm.value) {
      if (
        this.agentProfileData[propertyName] !==
        this.agentProfileModel[propertyName]
      ) {
        this.hasMismatch = true;
        break;
      }
    }
  }

  private populateAgentDetailsOnAgentForm() {
    this.agentProfileForm.patchValue({
      employeeId: this.agentProfileData.employeeId,
      sso: this.agentProfileData.sso,
      agentSchedulingGroupName: this.agentProfileData.agentSchedulingGroupName,
      firstName: this.agentProfileData.firstName,
      lastName: this.agentProfileData.lastName,
    });
    const date = new Date(this.agentProfileData.hireDate);
    const ngbDateStruct: NgbDateStruct = {
      day: date.getUTCDate(),
      month: date.getUTCMonth() + 1,
      year: date.getUTCFullYear(),
    };
    this.agentProfileForm.controls.hireDate.setValue(ngbDateStruct);
  }

  private getDropdownValues() {
    this.agentSchedulingGroupNames = this.agentDropdownService.getAgentSchedulingGroupNames();
    this.employeeIdList = this.agentDropdownService.getEmployeeIdList();
  }

  private agentFormIntialization() {
    this.agentProfileForm = this.formBuilder.group({
      employeeId: new FormControl('', Validators.required),
      sso: new FormControl('', Validators.compose([Validators.required, CustomValidators.isValidEmail])),
      agentSchedulingGroupName: new FormControl('', Validators.required),
      firstName: new FormControl('', Validators.compose([Validators.required, CustomValidators.cannotContainSpace])),
      lastName: new FormControl('', Validators.compose([Validators.required, CustomValidators.cannotContainSpace])),
      hireDate: new FormControl('', Validators.required),
    });
  }
}
