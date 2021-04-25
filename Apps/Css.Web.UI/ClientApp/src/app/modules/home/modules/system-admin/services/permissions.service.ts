import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { CookieService } from 'ngx-cookie-service';
import { forkJoin, Observable } from 'rxjs';
import { catchError } from 'rxjs/operators';
import { HttpBaseService } from 'src/app/core/services/http-base.service';
import { QueryStringParameters } from 'src/app/shared/models/query-string-parameters.model';
import { Constants } from 'src/app/shared/util/constants.util';
import { environment } from 'src/environments/environment';
import { AgentInfo } from '../../scheduling-menu/models/agent-info.model';
import { EmployeeDetails } from '../models/employee-details.model';
import { EmployeeRole } from '../models/employee-role.model';
import { Employee } from '../models/employee.model';
import { PermissionDetails } from '../models/permission-details.model';
import { Permission } from '../models/permission.model';
import { UserLoginLog } from '../models/user-logger.model';

@Injectable()
export class PermissionsService extends HttpBaseService {

  roles = Constants.UserRoles;
  employees: Permission[] = [];
  users: Employee[] = [];
  permissions: PermissionDetails[] = [];

  public get userRoleId(){
    return this.cookieService.get('userRoleId') ?? undefined;
  }

  private baseURL = '';

  constructor(
    private cookieService: CookieService,
    private http: HttpClient
  ) {
    super();
    this.baseURL = environment.services.gatewayService;
  }

  getRoleName(roleId){
    const url = `${this.baseURL}/roles/${roleId}`;
    return this.http.get<EmployeeRole>(url)
      .pipe(catchError(this.handleError));
  }

  getEmployees(queryParams: QueryStringParameters) {
    const url = `${this.baseURL}/userpermissions`;

    return this.http.get<EmployeeDetails>(url,
      { params: this.convertToHttpParam(queryParams), observe: 'response' })
      .pipe(catchError(this.handleError));
  }

  getAgentInfo(employeeId: string) {
    const url = `${this.baseURL}/agentAdmins/employees/${employeeId}`;

    return this.http.get<AgentInfo>(url)
      .pipe(catchError(this.handleError));
  }

  getEmployee(employeeId: string) {
    const url = `${this.baseURL}/userpermissions/${employeeId}`;

    return this.http.get<EmployeeDetails>(url)
      .pipe(catchError(this.handleError));
  }

  authLogin(username: string, password : string){
    const url = `${this.baseURL}/Auth/${username}/key/${password}`;

    return this.http.get<EmployeeDetails>(url)
      .pipe(catchError(this.handleError));
  }

  userLogging(employeeId: string, userLoginLog: UserLoginLog): Observable<any[]> {
    const userLogApiUrl = `${this.baseURL}/log`;

    let userLogResponse = this.http.post<any>(userLogApiUrl, userLoginLog)
    .pipe(catchError(this.handleError));

    const getEmployeeUrl = `${this.baseURL}/userpermissions/${employeeId}`;

    let employeeResponse = this.http.get<EmployeeDetails>(getEmployeeUrl)
      .pipe(catchError(this.handleError));
    
    // Observable.forkJoin (RxJS 5) changes to just forkJoin() in RxJS 6
    return forkJoin([userLogResponse, employeeResponse]);
  }

  deleteEmployee(employeeId: string) {
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

  updateUserPermission(employeeId: string, employeeDetails: EmployeeDetails){
    const url = `${this.baseURL}/userpermissions/${employeeId}`;

    return this.http.put<EmployeeDetails>(url, employeeDetails)
      .pipe(catchError(this.handleError));
  }

  // for sidebar menu
  isMenuHidden(rolesPermitted: number[], userRoleId) {
    // if role is inside rolesPermitted do not hide element
    if (rolesPermitted.indexOf(+userRoleId) > -1) {
      return false;
    } else {
      return true;
    }
  }

  // for special elements
  noPermission(rolesPermitted: number[], userRoleId) {
    // if role is inside rolesPermitted do not hide element
    if (rolesPermitted.indexOf(+userRoleId) > -1) {
      return true;
    } else {
      return false;
    }
  }

  storePermission(userRoleId){
    this.cookieService.set('userRoleId', userRoleId, null, environment.settings.cookiePath, null, false, 'Strict');
  }

  addUserLog(userLoginLog: UserLoginLog){
    const url = `https://localhost:44318/api/log`;

    return this.http.post<any>(url, {
      params: this.convertToHttpParam(userLoginLog),
      observe: 'response'
    })
      .pipe(catchError(this.handleError));
  }

}
