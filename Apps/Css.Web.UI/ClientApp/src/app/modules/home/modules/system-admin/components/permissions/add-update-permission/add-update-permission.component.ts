import { Component, EventEmitter, Input, OnChanges, OnDestroy, OnInit, Output, SimpleChanges } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { NgbModal, NgbModalOptions } from '@ng-bootstrap/ng-bootstrap';
import { Translation } from 'src/app/shared/models/translation.model';
import { ErrorWarningPopUpComponent } from 'src/app/shared/popups/error-warning-pop-up/error-warning-pop-up.component';
import { MessagePopUpComponent } from 'src/app/shared/popups/message-pop-up/message-pop-up.component';
import { Constants } from 'src/app/shared/util/constants.util';
import { PermissionDetails } from '../../../models/permission-details.model';
import { Permission } from '../../../models/permission.model';
import { UserRole } from '../../../models/user-role.model';
import { PermissionsService } from '../../../services/permissions.service';

@Component({
  selector: 'app-add-update-permission',
  templateUrl: './add-update-permission.component.html',
  styleUrls: ['./add-update-permission.component.scss']
})
export class AddUpdatePermissionComponent implements OnInit, OnChanges, OnDestroy {

  currentPage = 1;
  pageSize = 10;
  isEdit: boolean;
  totalRecord: number;
  searchKeyword: string;
  orderBy = 'createdDate';
  sortBy = 'desc';

  userRoles = Constants.UserRoles;
  employee: Permission;
  permission: PermissionDetails;
  permissionForm: FormGroup;

  updatePermissionSubscription: any;
  addPermissionSubscription: any;
  getPermissionSubscription: any;
  subscriptions: any[] = [];

  @Input() employeeId: number;
  @Input() translationValues: Translation[];
  @Output() employeeSelected: EventEmitter<number> = new EventEmitter<number>();
  @Output() permissionAdded = new EventEmitter();
  @Output() permissionUpdated = new EventEmitter();
  @Output() permissionCleared = new EventEmitter();

  constructor(
    private formBuilder: FormBuilder,
    private permissionsService: PermissionsService,
    private modalService: NgbModal,
  ) { }

  ngOnInit(): void {
    this.loadPermissions();
  }

  ngOnChanges(changes: SimpleChanges) {
    if (changes?.employeeId?.currentValue !== changes?.employeeId?.previousValue) {
      this.loadPermissions();
    }
  }

  ngOnDestroy() {
    this.subscriptions.forEach(subscription => {
      if (subscription) {
        subscription.unsubscribe();
      }
    });
  }

  getButtonTitle() {
    return this.isEdit ? 'Update' : 'Add';
  }

  get userRolesFormArray() {
    return this.permissionForm.get('roles') as FormArray;
  }

  onCheckboxChange(e) {
    const roles: FormArray = this.permissionForm.get('roles') as FormArray;
    if (e.target.checked) {
      roles.push(new FormControl(Number(e.target.value)));
    } else {
      let i = 0;
      roles.controls.forEach((item: FormControl) => {
        if (item.value === Number(e.target.value)) {
          roles.removeAt(i);
          return;
        }
        i++;
      });
    }
  }

  onEmployeeSelected(permission: Permission) {
    this.employeeId = permission.employeeId;
    this.employee = permission;
    this.loadPermissions();
    this.employeeSelected.emit(permission.employeeId);
  }

  onEmployeeCleared() {
    this.permissionCleared.emit();
    this.intializePermissionForm();
    this.employee = undefined;
    this.isEdit = false;
  }

  hasValueSelected(value) {
    const roles: FormArray = this.permissionForm.get('roles') as FormArray;
    return roles.controls.findIndex(x => x.value === value) !== -1;
  }

  savePermission() {
    if (this.checkFormIsValid()) {
      if (this.isEdit) {
        this.updatePermission();
      } else {
        this.addPermission();
      }
    }
  }

  private checkFormIsValid() {
    if (this.permissionForm.invalid && !this.employeeId) {
      this.showErrorWarningPopUpMessage('Please select an employee and its user access role');
      return false;
    } else if (this.permissionForm.invalid) {
      this.showErrorWarningPopUpMessage('Please select user access role');
      return false;
    } else if (!this.employeeId) {
      this.showErrorWarningPopUpMessage('Please select an employee');
      return false;
    } else {
      return true;
    }
  }

  private addPermission() {
    const addPermissionModel = this.employee as PermissionDetails;
    addPermissionModel.createdDate = new Date();
    addPermissionModel.createdBy = 'User';
    addPermissionModel.roles = new Array<UserRole>();

    this.userRolesFormArray.controls.forEach((ele, index) => {
      const role = new UserRole();
      role.id = Number(this.permissionForm.value.roles[index]);
      addPermissionModel.roles.push(role);
    });

    this.addPermissionSubscription = this.permissionsService.addPermission(addPermissionModel)
      .subscribe((data) => {
        if (data) {
          this.resetForm();
          this.permissionAdded.emit();
          this.showSuccessPopUpMessage('The record has been added!');
        }
      }, (error) => {
        this.showErrorWarningPopUpMessage(error.error);
        console.log(error);
      });

    this.subscriptions.push(this.addPermissionSubscription);
  }

  private updatePermission() {
    if (this.hasPermissionDetailsMismatch()) {
      this.permission.modifiedDate = new Date();
      this.permission.modifiedBy = 'User';
      this.permission.roles = new Array<UserRole>();

      this.userRolesFormArray.controls.forEach((ele, index) => {
        const role = new UserRole();
        role.id = Number(this.permissionForm.value.roles[index]);
        this.permission.roles.push(role);
      });

      this.updatePermissionSubscription = this.permissionsService.updatePermission(this.permission.employeeId, this.permission)
        .subscribe((success) => {
          if (success) {
            this.resetForm();
            this.permissionUpdated.emit();
            this.showSuccessPopUpMessage('The record has been updated!');
          }
        }, (error) => {
          console.log(error);
        });
      this.subscriptions.push(this.updatePermissionSubscription);
    } else {
      this.resetForm();
      this.permissionUpdated.emit();
      this.showSuccessPopUpMessage('No changes has been made!');
    }
  }

  private resetForm() {
    this.isEdit = false;
    this.permission = undefined;
    this.employeeId = undefined;
    this.intializePermissionForm();
  }

  private hasPermissionDetailsMismatch() {
    for (const index in this.userRoles) {
      if (this.permission.roles[index]?.id !== this.permissionForm.controls.roles.value[index]) {
        return true;
      }
    }
  }

  private getPermission() {
    if (this.employeeId) {
      this.getPermissionSubscription = this.permissionsService.getPermission(this.employeeId)
        .subscribe((data: PermissionDetails) => {
          this.permission = data;
          if (data) {
            this.setPermissionDetails(data);
          }
        }, (error) => {
          console.log(error);
        });

      this.subscriptions.push(this.getPermissionSubscription);
    }
  }

  private setPermissionDetails(record: PermissionDetails) {
    this.setUserRoles(record);
    this.isEdit = true;
  }

  private setUserRoles(record: PermissionDetails) {
    const array = this.permissionForm.controls.roles as FormArray;
    record.roles.forEach(ele => array.push(new FormControl(ele.id)));

    return array;
  }

  private loadPermissions() {
    this.isEdit = false;
    this.intializePermissionForm();
    this.getPermission();
  }

  private showSuccessPopUpMessage(contentMessage: string) {
    const options: NgbModalOptions = { backdrop: false, centered: true, size: 'sm' };
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

  private intializePermissionForm() {
    this.permissionForm = this.formBuilder.group({
      roles: this.formBuilder.array([], Validators.required)
    });
  }
}
