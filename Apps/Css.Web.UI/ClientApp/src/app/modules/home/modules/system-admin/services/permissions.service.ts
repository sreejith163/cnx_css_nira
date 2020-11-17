import { Injectable } from '@angular/core';
import { of } from 'rxjs';
import { QueryStringParameters } from 'src/app/shared/models/query-string-parameters.model';
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
    for (let i = 1; i <= 20; i++) {
      const employee = new Permission();
      employee.employeeId = i;
      employee.firstName = 'FirstName ' + i;
      employee.lastName = 'LastName ' + i;

      this.employees.push(employee);
    }
  }

  getEmployees(queryParams: QueryStringParameters) {
    const end = queryParams.pageNumber * queryParams.pageSize;
    const start = (queryParams.pageNumber * queryParams.pageSize) - (queryParams.pageSize);
    if (queryParams.searchKeyword) {
      const employee = new Array<Permission>();
      this.employees.forEach(ele => {
        if (ele.firstName.toLowerCase().includes(queryParams.searchKeyword.toLowerCase())) {
          employee.push(ele);
        }
      });
      return of(employee);
    } else {
      return of(this.employees.slice(start, end));
    }
  }

  getEmployee(employeeId: number) {
    return of(this.employees.find(x => x.employeeId === employeeId));
  }

  createUserPermissionDetails() {
    for (let i = 1; i <= 9; i++) {
      const permission = new PermissionDetails();
      permission.employeeId = i;
      permission.firstName = 'FirstName ' + i;
      permission.lastName = 'LastName ' + i;
      permission.roles = new Array<UserRole>();
      const n = i % 2 === 0 ? 0 : 3;
      for (let j = n; j < n + 2; j++) {
        permission.roles.push(this.roles[j]);
      }
      permission.createdBy = 'User ' + i;
      permission.createdDate = new Date('2020-09-1' + i);
      permission.modifiedBy = 'User ' + i;
      permission.modifiedDate = new Date('2020-09-1' + i);

      this.permissions.push(permission);
    }
  }

  getPermissions(queryParams?: QueryStringParameters) {
    if (queryParams.searchKeyword) {
      const permissions = this.searchKeyword(queryParams.searchKeyword);
      return of(permissions);
    } else {
      return of(this.permissions);
    }
  }

  getPermission(employeeId: number) {
    return of(this.permissions.find(x => x.employeeId === employeeId));
  }

  addPermission(permission: PermissionDetails) {
    this.permissions.push(permission);
    const success = true;
    return of(success);
  }

  updatePermission(employeeId: number, permission: PermissionDetails) {
    const index = this.permissions.findIndex(x => x.employeeId === employeeId);
    if (index !== -1) {
      this.permissions[index] = permission;
    }
    const success = true;
    return of(success);
  }

  private searchKeyword(keyword) {
    const permissions = new Array<PermissionDetails>();
    this.permissions.forEach(ele => {
      if (ele.firstName.toLowerCase().includes(keyword.toLowerCase())) {
        permissions.push(ele);
      } else if (ele.lastName.toLowerCase().includes(keyword.toLowerCase())) {
        permissions.push(ele);
      } else if (ele.employeeId === keyword) {
        permissions.push(ele);
      }
    });

    return permissions;
  }

}
