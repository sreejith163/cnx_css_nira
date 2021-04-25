import { Component, EventEmitter, Input, OnChanges, OnDestroy, OnInit, Output, SimpleChanges } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { NgbModal, NgbActiveModal, NgbModalOptions } from '@ng-bootstrap/ng-bootstrap';
import { ComponentOperation } from 'src/app/shared/enums/component-operation.enum';
import { NgxSpinnerService } from 'ngx-spinner';
import { CustomValidators } from 'src/app/shared/util/validations.util';
import { Constants } from 'src/app/shared/util/constants.util';
import { QueryStringParameters } from 'src/app/shared/models/query-string-parameters.model';
import { PermissionsService } from '../../../services/permissions.service';
import { EmployeeRole } from '../../../models/employee-role.model';
import { SpinnerOptions } from 'src/app/shared/util/spinner-options.util';
import { ErrorWarningPopUpComponent } from 'src/app/shared/popups/error-warning-pop-up/error-warning-pop-up.component';
import { ContentType } from 'src/app/shared/enums/content-type.enum';
import { EmployeeDetails } from '../../../models/employee-details.model';
import { AuthService } from 'src/app/core/services/auth.service';

@Component({
  selector: 'app-add-update-permission',
  templateUrl: './add-update-permission.component.html',
  styleUrls: ['./add-update-permission.component.scss']
})
export class AddUpdatePermissionComponent implements OnInit {
  LoggedUser;
  @Input() operation: ComponentOperation;
  @Input() employee: EmployeeDetails;
  spinner = 'addUpdatePermissionsSpinner';
  userPermissionForm: FormGroup;
  formSubmitted: boolean;
  maxLength = Constants.DefaultTextMaxLength;

  // Query Parameters
  currentPage = 1;
  pageSize = 10;
  searchKeyword: string;
  totalRecord: number;
  orderBy = 'createdDate';
  sortBy = 'desc';

  // Roles
  permissions: EmployeeRole[] = [];

  constructor(
    private modalService: NgbModal,
    private spinnerService: NgxSpinnerService,
    public activeModal: NgbActiveModal,
    private formBuilder: FormBuilder,
    private permissionsService: PermissionsService,
    private authService: AuthService,
  ) {
    this.LoggedUser = this.authService.getLoggedUserInfo();
    this.createUserPermissionForm();
  }

  ngOnInit(): void {
    this.loadPermissions();
    this.loadEmployeeDetails();
  }

  public closeModal() {
    this.activeModal.close(false);
  }

  getTitle() {
    return ComponentOperation[this.operation];
  }

  // validators
  hasFormControlValidationError(control: string) {
    return (
      this.formSubmitted &&
      this.userPermissionForm.controls[control].errors?.required
    );
  }

  submitUserPermissionForm() {
    this.formSubmitted = true;
    if (this.userPermissionForm.valid && this.validateEmployeeId()) {
      this.operation === ComponentOperation.Edit ? this.updateUserPermission() : this.addUserPermission();
    }
  }

  private getQueryParams() {
    const queryParams = new QueryStringParameters();
    queryParams.pageNumber = this.currentPage;
    queryParams.pageSize = this.pageSize;
    queryParams.searchKeyword = this.searchKeyword ?? '';
    queryParams.orderBy = `${this.orderBy} ${this.sortBy}`;
    queryParams.fields = '';

    return queryParams;
  }

  private loadPermissions() {
    const queryParams = this.getQueryParams();
    this.permissionsService.getPermissions(queryParams)
      .subscribe((response) => {
        if (response.body) {
          this.permissions = response.body;
        }

      }, (error) => {
        console.log(error);
      });

  }

  private loadEmployeeDetails() {
    if (this.operation === ComponentOperation.Edit) {
      // console.log(this.employee)
      this.userPermissionForm.patchValue({
        firstname: this.employee.firstname,
        lastname: this.employee.lastname,
        sso: this.employee.sso,
        employeeId: this.employee.employeeId
      });
      this.userPermissionForm.controls.userRoleId.setValue(this.employee.roleIndex);
    }
  }


  private addUserPermission() {
    const addUserPermissionModel = this.userPermissionForm.value as EmployeeDetails;
    // append the model with createdby
    addUserPermissionModel.createdBy = this.LoggedUser.displayName;
    this.spinnerService.show(this.spinner, SpinnerOptions);
    this.permissionsService.addUserPermission(addUserPermissionModel)
      .subscribe(() => {
        this.spinnerService.hide(this.spinner);
        this.activeModal.close(true);
      }, (error) => {
        this.spinnerService.hide(this.spinner);
        if (error.status === 409) {
          this.showErrorWarningPopUpMessage(error.error);
        }
      });
  }

  private updateUserPermission() {
    const updatePermissionModel = this.userPermissionForm.value as EmployeeDetails;
    updatePermissionModel.modifiedBy = this.LoggedUser.displayName;
    this.spinnerService.show(this.spinner, SpinnerOptions);
    // console.log(updatePermissionModel);
    this.permissionsService.updateUserPermission(this.employee.employeeId, updatePermissionModel)
      .subscribe(() => {
        this.spinnerService.hide(this.spinner);
        this.activeModal.close(true);
      }, (error) => {
        this.spinnerService.hide(this.spinner);
        if (error.status === 409) {
          this.showErrorWarningPopUpMessage(error.error);
        }
      });
  }

  validateEmployeeId() {
    this.userPermissionForm.controls.employeeId.setValue(this.userPermissionForm.controls.employeeId.value.toString().replace(/\B[a-zA-Z]/gi,""));
    this.userPermissionForm.controls.employeeId.setValue(this.userPermissionForm.controls.employeeId.value.toString().replace(/[^A-Za-z0-9]/gi,""));
    if (this.userPermissionForm.controls.employeeId.value) {
      if (!isNaN(+this.userPermissionForm.controls.employeeId.value) && +this.userPermissionForm.controls.employeeId.value <= 0) {
        return false;
      }
      return true;
    }
    return false;
  }

  // Pop up messages
  private showErrorWarningPopUpMessage(contentMessage: string) {
    const options: NgbModalOptions = { backdrop: 'static', centered: true, size: 'sm' };
    const modalRef = this.modalService.open(ErrorWarningPopUpComponent, options);
    modalRef.componentInstance.headingMessage = 'Error';
    modalRef.componentInstance.contentMessage = contentMessage;
    modalRef.componentInstance.messageType = ContentType.String;

    return modalRef;
  }

  private createUserPermissionForm() {
    this.userPermissionForm = this.formBuilder.group({
      firstname: new FormControl('',
        Validators.compose([
          Validators.required,
          Validators.maxLength(50)]
        )),
      lastname: new FormControl('',
        Validators.compose([
          Validators.required,
          Validators.maxLength(50)]
        )),
      sso: new FormControl('',
        Validators.compose([
          Validators.pattern('^(?=[^@]*[A-Za-z])([a-zA-Z0-9])(([a-zA-Z0-9])*([\._-])?([a-zA-Z0-9]))*@(([a-zA-Z0-9\-])+(\.))+([a-zA-Z]{2,4})+$'),
          Validators.email,
          Validators.required,
          Validators.maxLength(50)]
        )),
      employeeId: new FormControl('',
        Validators.compose([
          Validators.required,
          Validators.maxLength(50)]
        )),
      // permission
      userRoleId: new FormControl('',
        Validators.compose([
          Validators.required,
          Validators.maxLength(50)]
        )),
    });
  }
}

