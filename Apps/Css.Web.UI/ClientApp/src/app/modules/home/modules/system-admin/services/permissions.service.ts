import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { of } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { HttpBaseService } from 'src/app/core/services/http-base.service';
import { QueryStringParameters } from 'src/app/shared/models/query-string-parameters.model';
import { Constants } from 'src/app/shared/util/constants.util';
import { environment } from 'src/environments/environment';
import { EmployeeDetails } from '../models/employee-details.model';
import { EmployeeRole } from '../models/employee-role.model';
import { Employee } from '../models/employee.model';
import { PermissionDetails } from '../models/permission-details.model';
import { Permission } from '../models/permission.model';

@Injectable()
export class PermissionsService extends HttpBaseService {

  roles = Constants.UserRoles;
  employees: Permission[] = [];
  users: Employee[] = [];
  permissions: PermissionDetails[] = [];

  private baseURL = '';

  constructor(
    private http: HttpClient
  ) {
    super();
    this.baseURL = environment.services.gatewayService;
  }


  getEmployees(queryParams: QueryStringParameters) {
    const url = `${this.baseURL}/userpermissions`;

    return this.http.get<EmployeeDetails>(url,
      { params: this.convertToHttpParam(queryParams), observe: 'response' })
      .pipe(catchError(this.handleError));
  }

  getEmployee(employeeId: number) {
    const url = `${this.baseURL}/userpermissions/${employeeId}`;

    return this.http.get<EmployeeDetails>(url)
      .pipe(catchError(this.handleError));
  }

  deleteEmployee(employeeId: number) {
    const url = `${this.baseURL}/userpermissions/${employeeId}`;

    return this.http.delete<EmployeeDetails>(url)
      .pipe(catchError(this.handleError));
  }

  getPermissions(queryParams: QueryStringParameters) {
    const url = `${this.baseURL}/roles`;

    return this.http.get<EmployeeRole>(url,
      { params: this.convertToHttpParam(queryParams), observe: 'response' })
      .pipe(catchError(this.handleError));
  }

  addUserPermission(employeeDetails: EmployeeDetails){
    const url = `${this.baseURL}/userpermissions`;

    return this.http.post<EmployeeDetails>(url, employeeDetails)
      .pipe(catchError(this.handleError));
  }

  updateUserPermission(employeeId: number, employeeDetails: EmployeeDetails){
    const url = `${this.baseURL}/userpermissions/${employeeId}`;

    return this.http.put<EmployeeDetails>(url, employeeDetails)
      .pipe(catchError(this.handleError));
  }


}
