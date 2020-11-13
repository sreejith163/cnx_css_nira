import { Injectable } from '@angular/core';
import { of } from 'rxjs';
import { Constants } from 'src/app/shared/util/constants.util';
import { PermissionDetails } from '../models/permission-details.model';
import { Permission } from '../models/permission.model';
import { UserRole } from '../models/user-role.model';

@Injectable()
export class PermissionsService {

  roles = Constants.UserRoles;
  employees: Permission[] = [];
  permissions: PermissionDetails[] = [];

  constructor() {
    this.createUserPermissionDetails();
    this.createEmployeeIds();
  }

  createEmployeeIds() {
    for (let i = 6; i <= 10; i++) {
      const employee = new Permission();
      employee.employeeId = i;
      employee.firstName = 'Employee ' + i;

      this.employees.push(employee);
    }
  }

  getEmployees(searchKeyword?: string) {
    if (searchKeyword) {
      const employee = new Array<Permission>();
      this.employees.forEach(ele => {
        if (ele.firstName.includes(searchKeyword)) {
          employee.push(ele);
        }
      });
      return of(employee);
    } else {
      return of(this.employees);
    }
  }

  createUserPermissionDetails() {
    for (let i = 1; i <= 5; i++) {
      const permission = new PermissionDetails();
      permission.employeeId = i;
      permission.firstName = 'FirstName ' + i;
      permission.lastName = 'LastName ' + i;
      permission.roles = new Array<UserRole>();
      permission.roles.push(this.roles[i - 1]);
      permission.createdBy = 'User ' + i;
      permission.createdDate = '2020-09-1' + i;
      permission.modifiedBy = 'User ' + i;
      permission.modifiedDate = '2020-09-1' + i;

      this.permissions.push(permission);
    }
  }

  getPermissions() {
    return of(this.permissions);
  }

  addPermission(permission: PermissionDetails) {
    this.permissions.push(permission);
    const index = this.employees.findIndex(x => x.employeeId === permission.employeeId);
    this.employees.splice(index, 1);
    const employees = this.employees;
    return of(employees);
  }

  updatePermission(employeeId: number, permission: PermissionDetails) {
    const index = this.permissions.findIndex(x => x.employeeId === employeeId);
    if (index !== -1) {
      this.permissions[index] = permission;
    }
    const success = true;
    return of(success);
  }

}
