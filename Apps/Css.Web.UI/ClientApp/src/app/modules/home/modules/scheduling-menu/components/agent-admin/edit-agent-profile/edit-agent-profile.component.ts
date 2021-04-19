import { DatePipe } from '@angular/common';
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
  NgbModalOptions,
  NgbModalRef
} from '@ng-bootstrap/ng-bootstrap';
import { NgxSpinnerService } from 'ngx-spinner';
import { SubscriptionLike as ISubscription } from 'rxjs';
import { AuthService } from 'src/app/core/services/auth.service';
import { ActivityType } from 'src/app/shared/enums/activity-type.enum';
import { ComponentOperation } from 'src/app/shared/enums/component-operation.enum';
import { ContentType } from 'src/app/shared/enums/content-type.enum';
import { TranslationDetails } from 'src/app/shared/models/translation-details.model';
import { ErrorWarningPopUpComponent } from 'src/app/shared/popups/error-warning-pop-up/error-warning-pop-up.component';
import { Constants } from 'src/app/shared/util/constants.util';
import { SpinnerOptions } from 'src/app/shared/util/spinner-options.util';
import { CustomValidators } from 'src/app/shared/util/validations.util';
import { GenericDataService } from '../../../../setup-menu/services/generic-data.service';
import { AgentAdminBase } from '../../../models/agent-admin-base.model';
import { AgentAdminResponse } from '../../../models/agent-admin-response.model';
import { UpdateAgentAdmin } from '../../../models/update-agent-admin.model';
import { AgentAdminService } from '../../../services/agent-admin.service';
import { ActivityLogsComponent } from '../activity-logs/activity-logs.component';

@Component({
  selector: 'app-edit-agent-profile',
  templateUrl: './edit-agent-profile.component.html',
  styleUrls: ['./edit-agent-profile.component.css'],
  providers: [DatePipe]
})
export class EditAgentProfileComponent implements OnInit, OnDestroy {
  // modal ref
  modalRef: NgbModalRef;

  hasMismatch: boolean;
  formSubmitted: boolean;
  today = this.calendar.getToday();
  agentProfileModel: AgentAdminBase;
  agentProfileForm: FormGroup;
  model: NgbDateStruct;
  agentAdminDetails: AgentAdminResponse;

  spinner = 'agentAdmin';
  maxLength = Constants.DefaultEmpTextMaxLength;

  clientId: number;
  clientLobGroupId: number;
  skillGroupId: number;
  skillTagId: number;
  agentSchedulingGroupId: number;

  getAgentAdminSubscription: ISubscription;
  editAgentAdminSubscription: ISubscription;
  updateAgentAdminSubscription: ISubscription;

  subscriptions: ISubscription[] = [];

  @Input() operation: ComponentOperation;
  @Input() agentAdminId: string;
  @Input() translationValues: TranslationDetails[];

  constructor(
    private formBuilder: FormBuilder,
    private calendar: NgbCalendar,
    public activeModal: NgbActiveModal,
    private modalService: NgbModal,
    private ngbDateParserFormatter: NgbDateParserFormatter,
    private agentService: AgentAdminService,
    private datepipe: DatePipe,
    private genericDataService: GenericDataService,
    private authService: AuthService,
    private spinnerService: NgxSpinnerService,
  ) { }

  get agentSchForm() { return this.agentProfileForm.controls; }

  ngOnInit(): void {
    this.agentFormIntialization();

    if (this.operation === ComponentOperation.Edit) {
      this.loadAgentAdminGroup();
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
    return this.agentProfileForm.controls;
  }

  getTitle() {
    return ComponentOperation[this.operation];
  }

  validateHireDateFormat() {
    const hireDate = this.agentProfileForm.controls.hireDate.value;
    if (!this.calendar.isValid(hireDate) || String(hireDate.year).length !== 4 || !this.validateHireDate()) {
      return false;
    } else {
      return true;
    }
  }

  validateHireDate() {
    const hireDate = new Date(this.ngbDateParserFormatter.format(this.agentProfileForm.controls.hireDate.value));
    if (hireDate > new Date()) {
      return false;
    } else {
      return true;
    }
  }

  validateEmployeeId() {
    this.agentProfileForm.controls.employeeId.setValue(this.agentProfileForm.controls.employeeId.value.toString().replace(/\B[a-zA-Z]/gi,""));
    this.agentProfileForm.controls.employeeId.setValue(this.agentProfileForm.controls.employeeId.value.toString().replace(/[^A-Za-z0-9]/gi,""));
    if (this.agentProfileForm.controls.employeeId.value) {
      if (!isNaN(+this.agentProfileForm.controls.employeeId.value) && +this.agentProfileForm.controls.employeeId.value <= 0) {
        return false;
      }
      return true;
    }
    return false;
  }

  validateSupervisorId() {
    this.agentProfileForm.controls.supervisorId.setValue(this.agentProfileForm.controls.supervisorId.value.toString().replace(/\B[a-zA-Z]/gi,""));
    this.agentProfileForm.controls.supervisorId.setValue(this.agentProfileForm.controls.supervisorId.value.toString().replace(/[^A-Za-z0-9]/gi,""));
    if (this.agentProfileForm.controls.supervisorId.value) {
      if (!isNaN(+this.agentProfileForm.controls.supervisorId.value) && +this.agentProfileForm.controls.supervisorId.value <= 0) {
        return false;
      }
      return true;
    }
    return false;
  }

  saveAgentAdminDetails() {
    this.formSubmitted = true;
    if (this.agentProfileForm.valid && this.clientId && this.clientLobGroupId && this.skillGroupId &&
       this.skillTagId && this.agentSchedulingGroupId && this.validateHireDateFormat() && this.validateEmployeeId() && this.validateSupervisorId()) {
      this.saveAgentProfileDetailsOnModel();
      this.updateAgentAdminProfileDetails();
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

  setClient(client: number) {
    this.clientId = client;
    this.clientLobGroupId = undefined;
    this.skillGroupId = undefined;
    this.skillTagId = undefined;
    this.agentSchedulingGroupId = undefined;
  }

  setClientLobGroup(clientLobGroupId: number) {
    this.clientLobGroupId = clientLobGroupId;
    this.skillGroupId = undefined;
    this.skillTagId = undefined;
    this.agentSchedulingGroupId = undefined;
  }

  setSkillGroup(skillGroupId: number) {
    this.skillGroupId = skillGroupId;
    this.skillTagId = undefined;
    this.agentSchedulingGroupId = undefined;
  }

  setSkillTag(skillTagId: number) {
    this.skillTagId = skillTagId;
    this.agentSchedulingGroupId = undefined;
  }

  setAgentSchedulingGroupId(agentSchedulinggroupId: number) {
    this.agentSchedulingGroupId = agentSchedulinggroupId;
  }

  private saveAgentProfileDetailsOnModel() {
    this.agentProfileModel = new AgentAdminBase();
    this.agentProfileModel = this.agentProfileForm.value;
    const myDate = this.ngbDateParserFormatter.format(this.agentProfileForm.controls.hireDate.value);
    this.agentProfileModel.skillGroupId = this.skillGroupId;
    this.agentProfileModel.clientId = this.clientId;
    this.agentProfileModel.clientLobGroupId = this.clientLobGroupId;
    this.agentProfileModel.skillTagId = this.skillTagId;
    this.agentProfileModel.agentSchedulingGroupId = this.agentSchedulingGroupId;
    const hireDate = this.agentProfileForm.controls.hireDate.value;
    this.agentProfileModel.hireDate = this.getFormattedDate(new Date(hireDate.year, hireDate.month - 1, hireDate.day));

    this.agentProfileModel.pto.earned = this.agentProfileForm.controls.pto.get('earned').value;
    this.agentProfileModel.pto.credited = this.agentProfileForm.controls.pto.get('credited').value;
    this.agentProfileModel.pto.cofromlastyear = this.agentProfileForm.controls.pto.get('cofromlastyear').value;
    this.agentProfileModel.pto.planned = this.agentProfileForm.controls.pto.get('planned').value;
    this.agentProfileModel.pto.taken = this.agentProfileForm.controls.pto.get('taken').value;
    this.agentProfileModel.pto.debited = this.agentProfileForm.controls.pto.get('debited').value;
    this.agentProfileModel.pto.cofornextyear = this.agentProfileForm.controls.pto.get('cofornextyear').value;
    this.agentProfileModel.pto.remaining = this.agentProfileForm.controls.pto.get('remaining').value;
  }


  private showErrorWarningPopUpMessage(contentMessage: string) {
    const options: NgbModalOptions = { backdrop: false, centered: true, size: 'sm' };
    const modalRef = this.modalService.open(ErrorWarningPopUpComponent, options);
    modalRef.componentInstance.headingMessage = 'Error';
    modalRef.componentInstance.contentMessage = contentMessage;
    modalRef.componentInstance.messageType = ContentType.String;

    return modalRef;
  }

  private updateAgentAdminProfileDetails() {
    if (this.hasAgentAdminDetailsMismatch()) {
      const updateAgentAdminModel = this.agentProfileModel as UpdateAgentAdmin;
      updateAgentAdminModel.modifiedBy = this.authService.getLoggedUserInfo().displayName;
      this.spinnerService.show(this.spinner, SpinnerOptions);

      this.updateAgentAdminSubscription = this.agentService.updateAgentAdmin(
        this.agentAdminId, updateAgentAdminModel)
        .subscribe((resp) => {
          this.spinnerService.hide(this.spinner);
          this.activeModal.close({ needRefresh: true });
        }, (error) => {
          this.spinnerService.hide(this.spinner);
          if (error.status === 409 || error.status === 404) {
            this.showErrorWarningPopUpMessage(error.error);
          }
        });
      this.subscriptions.push(this.updateAgentAdminSubscription);
    } else {
      this.activeModal.close({ needRefresh: false });
    }
  }

  private hasAgentAdminDetailsMismatch() {
    if (this.agentProfileModel.employeeId !== this.agentAdminDetails.employeeId ||
      this.agentProfileModel.sso !== this.agentAdminDetails.sso ||
      this.agentProfileModel.skillTagId !== this.agentAdminDetails.skillTagId ||
      this.agentProfileModel.agentSchedulingGroupId !== this.agentAdminDetails.agentSchedulingGroupId ||
      this.agentProfileModel.pto !== this.agentAdminDetails.pto ||
      this.agentProfileModel.firstName !== this.agentAdminDetails.firstName ||
      this.agentProfileModel.lastName !== this.agentAdminDetails.lastName ||
      this.agentProfileModel.supervisorId !== this.agentAdminDetails.supervisorId ||
      this.agentProfileModel.supervisorName !== this.agentAdminDetails.supervisorName ||
      this.agentProfileModel.supervisorSso !== this.agentAdminDetails.supervisorSso ||
      this.agentProfileModel.hireDate !== this.agentAdminDetails.hireDate
    ) {
      return true;
    }
  }


  private loadAgentAdminGroup() {
    this.spinnerService.show(this.spinner, SpinnerOptions);

    this.getAgentAdminSubscription = this.agentService.getAgentAdmin(this.agentAdminId)
      .subscribe((response) => {
        this.agentAdminDetails = response;
        this.populateAgentDetailsOnAgentForm();
        this.spinnerService.hide(this.spinner);
      }, (error) => {
        this.spinnerService.hide(this.spinner);
        if (error.status === 409) {
          this.showErrorWarningPopUpMessage(error.error);
        }
      });
    this.subscriptions.push(this.getAgentAdminSubscription);
  }


  private populateAgentDetailsOnAgentForm() {
    this.clientId = this.agentAdminDetails.clientId;
    this.clientLobGroupId = this.agentAdminDetails.clientLobGroupId;
    this.skillGroupId = this.agentAdminDetails.skillGroupId;
    this.skillTagId = this.agentAdminDetails.skillTagId;
    this.agentSchedulingGroupId = this.agentAdminDetails.agentSchedulingGroupId;
    const tempPTO = this.agentAdminDetails.pto != null ? this.agentAdminDetails.pto : '';

    this.agentProfileForm.patchValue({
      employeeId: this.agentAdminDetails.employeeId,
      sso: this.agentAdminDetails.sso,
      firstName: this.agentAdminDetails.firstName,
      lastName: this.agentAdminDetails.lastName,
      supervisorId: this.agentAdminDetails.supervisorId,
      supervisorName: this.agentAdminDetails.supervisorName,
      supervisorSso: this.agentAdminDetails.supervisorSso,
      pto: tempPTO,
    });
    if (this.agentAdminDetails.hireDate) {
      const date = new Date(this.agentAdminDetails.hireDate);
      const ngbDateStruct: NgbDateStruct = {
        day: date.getUTCDate(),
        month: date.getUTCMonth() + 1,
        year: date.getUTCFullYear(),
      };
      this.agentProfileForm.controls.hireDate.setValue(ngbDateStruct);
    }
  }

  private agentFormIntialization() {
    this.agentProfileForm = this.formBuilder.group({
      employeeId: new FormControl('', Validators.compose([Validators.required, Validators.maxLength(45)])),
      sso: new FormControl('', Validators.compose([Validators.required, CustomValidators.isValidEmail])),
      firstName: new FormControl('', Validators.compose([Validators.required, CustomValidators.cannotContainSpace])),
      lastName: new FormControl('', Validators.compose([Validators.required, CustomValidators.cannotContainSpace])),
      hireDate: new FormControl('', Validators.required),
      supervisorId: new FormControl('', Validators.compose([Validators.required, CustomValidators.cannotContainSpace, Validators.maxLength(45)])),
      supervisorName: new FormControl('', Validators.required),
      supervisorSso: new FormControl('', Validators.compose([Validators.required, CustomValidators.isValidEmail])),
      pto: this.formBuilder.group({
        earned: new FormControl(),
        credited: new FormControl(),
        cofromlastyear: new FormControl(),
        planned: new FormControl(),
        taken: new FormControl(),
        debited: new FormControl(),
        cofornextyear: new FormControl(),
        remaining: new FormControl(),
      }),
    }, { validators: [CustomValidators.sameSSO('sso', 'supervisorSso'), CustomValidators.sameEmployeeId('employeeId', 'supervisorId')] });
  }

  showActivityLogs() {
    const options: NgbModalOptions = { backdrop: 'static', centered: true, size: 'xl' };
    this.modalRef = this.modalService.open(ActivityLogsComponent, options);
    this.modalRef.componentInstance.activityType = ActivityType.AgentAdmin;
    this.modalRef.componentInstance.employeeId = this.agentAdminDetails.employeeId;
    this.modalRef.componentInstance.employeeName = this.agentAdminDetails.firstName + ' ' + this.agentAdminDetails.firstName;
    this.modalRef.result.then((confirmed) => {
      if (confirmed === true) {
      }
    });
  }

  private getFormattedDate(date: Date) {
    const transformedDate = this.datepipe.transform(date, 'yyyy-MM-dd');
    return new Date(transformedDate);
  }
}
