import { Component, OnDestroy, OnInit } from '@angular/core';
import { Constants } from 'src/app/shared/util/constants.util';
import { PermissionsService } from '../../../services/permissions.service';
import { PermissionDetails } from '../../../models/permission-details.model';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { Permission } from '../../../models/permission.model';
import { UserRole } from '../../../models/user-role.model';
import { NgbModal, NgbModalOptions } from '@ng-bootstrap/ng-bootstrap';
import { MessagePopUpComponent } from 'src/app/shared/popups/message-pop-up/message-pop-up.component';
import { ErrorWarningPopUpComponent } from 'src/app/shared/popups/error-warning-pop-up/error-warning-pop-up.component';

@Component({
  selector: 'app-permissions-list',
  templateUrl: './permissions-list.component.html',
  styleUrls: ['./permissions-list.component.scss']
})
export class PermissionsListComponent implements OnInit, OnDestroy {

  currentPage = 1;
  pageSize = 10;
  totalRecord: number;
  selectedRow: number;
  employeeFirstName: string;
  isEdit: boolean;

  permissionForm: FormGroup;
  roles: UserRole;
  translationValues = Constants.permissionsTranslationValues;
  paginationSize = Constants.paginationSize;
  userRoles = Constants.UserRoles;

  permissions: PermissionDetails[] = [];
  updatePermissionData: any[] = [];
  hiddenRolesList: UserRole[] = [];
  hiddenRoles: UserRole[] = [];

  updatePermissionSubscription: any;
  addPermissionSubscription: any;
  getPermissionsSubscription: any;
  subscriptions: any[] = [];

  constructor(
    private permissionsService: PermissionsService,
    private modalService: NgbModal,
    private formBuilder: FormBuilder
  ) { }

  ngOnInit(): void {
    this.getPermissions();
    this.getUserRolesToHide();
    this.intializePermissionForm();
  }

  ngOnDestroy() {
    this.subscriptions.forEach(subscription => {
      if (subscription) {
        subscription.unsubscribe();
      }
    });
  }

  changePageSize(pageSize: number) {
    this.pageSize = pageSize;
    this.getPermissions();
  }

  changePage(page: number) {
    this.currentPage = page;
    this.getPermissions();
  }

  checkPermission(permission: PermissionDetails, userRoleId: number) {
    return permission.roles.some(x => x.id === userRoleId) ? 'Yes' : 'No';
  }

  setEmployee(user: Permission) {
    this.permissionForm.controls.employeeId.setValue(user.employeeId);
    this.employeeFirstName = user.firstName;
  }

  hasValueSelected(value) {
    const roles: FormArray = this.permissionForm.get('roles') as FormArray;
    return roles.controls.findIndex(x => x.value === value) !== -1;
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

  savePermission() {
    if (this.permissionForm.valid) {
      if (this.isEdit) {
        this.updatePermission();
      } else {
        this.addPermission();
      }
    } else {
      this.showErrorWarningPopUpMessage('Please select an employee and its user access role');
    }
  }

  updatePermission() {
    if (this.hasPermissionDetailsMismatch()) {
      let updatePermissionModel = new PermissionDetails();
      updatePermissionModel = this.updatePermissionData[0];
      updatePermissionModel.modifiedDate = String(new Date());
      updatePermissionModel.modifiedBy = 'User';
      updatePermissionModel.roles = new Array<UserRole>();

      this.userRoles.forEach((ele, index) => {
        const role = new UserRole();
        role.id = Number(this.permissionForm.value.roles[index]);
        updatePermissionModel.roles.push(role);
      });

      this.updatePermissionSubscription = this.permissionsService.updatePermission(updatePermissionModel.employeeId, updatePermissionModel)
        .subscribe((success) => {
          if (success) {
            this.isEdit = false;
            this.getPermissions();
            this.intializePermissionForm();
            this.updatePermissionData = [];
            this.showSuccessPopUpMessage('The record has been updated!');
          }
        }, (error) => {
          console.log(error);
        });
      this.subscriptions.push(this.updatePermissionSubscription);
    } else {
      this.isEdit = false;
      this.intializePermissionForm();
      this.updatePermissionData = [];
      this.showSuccessPopUpMessage('No changes has been made!');
    }
  }

  addPermission() {
    const addPermissionModel = new PermissionDetails();
    addPermissionModel.employeeId = this.permissionForm.controls.employeeId.value;
    addPermissionModel.firstName = this.employeeFirstName;
    addPermissionModel.lastName = 'LastName';
    addPermissionModel.createdDate = String(new Date());
    addPermissionModel.createdBy = 'User';
    addPermissionModel.roles = new Array<UserRole>();

    this.userRoles.forEach((ele, index) => {
      const role = new UserRole();
      role.id = Number(this.permissionForm.value.roles[index]);
      addPermissionModel.roles.push(role);
    });

    this.addPermissionSubscription = this.permissionsService.addPermission(addPermissionModel)
      .subscribe((data) => {
        if (data) {
          this.updatePermissionData = JSON.parse(JSON.stringify(data));
          this.getPermissions();
          this.intializePermissionForm();
          this.showSuccessPopUpMessage('The record has been added!');
        }
      }, (error) => {
        this.showErrorWarningPopUpMessage(error.error);
        console.log(error);
      });

    this.subscriptions.push(this.addPermissionSubscription);
  }

  getPermissionRecordToUpdate(record: PermissionDetails, index: number) {
    if (this.updatePermissionData.length === 0) {
      this.isEdit = true;
      this.selectedRow = index;
      this.updatePermissionData.push(record);
      this.setUpdatePermissionDetails(record);
    } else {
      this.isEdit = false;
      this.selectedRow = null;
      this.updatePermissionData = [];
      this.intializePermissionForm();
    }
  }

  getTitle() {
    const buttontTitle = this.isEdit ? 'Update' : 'Add';
    return buttontTitle;
  }

  getUserRolesToHide() {
    for (let i = 6; i <= 10; i++) {
      const role = new UserRole();
      role.id = i;
      role.role = this.userRoles[i - 6].role;

      this.hiddenRoles.push(role);
    }
  }

  onCheckRolesToHide(e) {
    if (e.target.checked) {
      const item = new UserRole();
      item.role = e.target.value;
      this.hiddenRolesList.push(item);
    } else {
      const index = this.hiddenRolesList.findIndex(x => x.role === e.target.value);
      this.hiddenRolesList.splice(index, 1);
    }
  }

  hasRoleHidden(role: string) {
    return this.hiddenRolesList.findIndex(x => x.role === role) === -1;
  }

  hasHideRoleSelected(role: string) {
    return this.hiddenRolesList.findIndex(x => x.role === role) !== -1;
  }

  isRowSelected(index: number) {
    return this.isEdit && this.selectedRow === index;
  }

  private hasPermissionDetailsMismatch() {
    for (const index in this.userRoles) {
      if (this.updatePermissionData[0]?.roles[index]?.id !== this.permissionForm.controls.roles.value[index]) {
        return true;
      }
    }
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

  private setUpdatePermissionDetails(record: PermissionDetails) {
    this.intializePermissionForm();
    this.permissionForm.controls.employeeId.setValue(record.employeeId);
    this.setUserRoles(record);

  }

  private setUserRoles(record: PermissionDetails) {
    const array = this.permissionForm.controls.roles as FormArray;
    record.roles.forEach(ele => array.push(new FormControl(ele.id)));

    return array;
  }

  private getPermissions() {
    this.getPermissionsSubscription = this.permissionsService.getPermissions()
      .subscribe((data) => {
        this.permissions = data.sort((a, b) => new Date(b.createdDate).getTime() - new Date(a.createdDate).getTime());
        this.totalRecord = this.permissions.length;
      }, (error) => {
        console.log(error);
      });

    this.subscriptions.push(this.getPermissionsSubscription);
  }

  private intializePermissionForm() {
    this.permissionForm = this.formBuilder.group({
      employeeId: new FormControl(null, Validators.required),
      roles: this.formBuilder.array([], Validators.required)
    });
  }

}
