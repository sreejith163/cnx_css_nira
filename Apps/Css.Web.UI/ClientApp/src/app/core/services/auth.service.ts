import { Injectable } from '@angular/core';
import { CookieService } from 'ngx-cookie-service';
import { environment } from 'src/environments/environment';
import { LoggedUserInfo } from '../models/logged-user-info.model';
import jwt_decode from 'jwt-decode';
import { EmployeeDetails } from 'src/app/modules/home/modules/system-admin/models/employee-details.model';
import { PermissionsService } from 'src/app/modules/home/modules/system-admin/services/permissions.service';
import { Router } from '@angular/router';
import { BehaviorSubject } from 'rxjs';
import { NgbModal, NgbModalOptions, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { ErrorPopUpComponent } from 'src/app/shared/popups/error-pop-up/error-pop-up.component';

// temporary model for UAT
export class UAT {
  uid: string;
  employeeId: string;
  displayName: string;
}


@Injectable()
export class AuthService {
  private currentUserUATSubject: BehaviorSubject<UAT>;
  modalRef: NgbModalRef;

  constructor(
    private cookieService: CookieService,
    private router: Router,
    private permissionsService: PermissionsService,
    private modalService: NgbModal,
  ) {

    // fetch the userUAT details from cookie storage
    const userUAT: UAT = {
      uid: this.cookieService.get('uid'),
      employeeId: this.cookieService.get('employeeId'),
      displayName: this.cookieService.get('displayName'),
    };

    // store the object on a subject
    this.currentUserUATSubject = new BehaviorSubject<UAT>(userUAT);

  }

  public get currentUserUATValue(): UAT {
    return this.currentUserUATSubject.value;
  }

  isLoggedIn() {
    const isLoggedIn = this.cookieService.get(environment.settings.sessionName);
    if (isLoggedIn){
      return isLoggedIn ?? false;
    }else{
      return this.currentUserUATValue ?? false;
    }
  }

  getLoggedUserInfo(): LoggedUserInfo {
    if (this.isLoggedIn()) {
      const token = this.cookieService.get(environment.settings.sessionName);

      if (token){
          const decodedToken = jwt_decode(token);
          const user = new LoggedUserInfo();
          user.uid = decodedToken.uid;
          user.employeeId = decodedToken.employeeid;
          user.displayName = decodedToken.displayname;
          return user;
      }else{
          const user = new LoggedUserInfo();
          user.uid = this.cookieService.get('uid');
          user.employeeId = this.cookieService.get('employeeId');
          user.displayName = this.cookieService.get('displayName');
          return user;
      }

    }

  }

  login() {
    window.location.href = environment.sso.authBaseUrl + environment.sso.authAppToken;
  }

  private getModalPopup(component: any, size: string) {
    const options: NgbModalOptions = { backdrop: 'static', centered: true, size };
    this.modalRef = this.modalService.open(component, options);
  }

  loginUAT(userUAT: UAT) {
    // store the UAT details inside a cookie to persist uat session
    this.cookieService.set('employeeId', userUAT.employeeId, null, environment.settings.cookiePath, null, false, 'Strict');
    this.cookieService.set('uid', userUAT.uid, null, environment.settings.cookiePath, null, false, 'Strict');
    this.cookieService.set('displayName', userUAT.displayName, null, environment.settings.cookiePath, null, false, 'Strict');
    this.permissionsService.getEmployee(+userUAT.employeeId).subscribe((user:EmployeeDetails)=>{
      this.permissionsService.storePermission(user.userRoleId);
      this.router.navigate(['home']);
    },error=>{

      this.getModalPopup(ErrorPopUpComponent, 'sm');
      this.modalRef.componentInstance.headingMessage = 'Invalid Credentials';
      this.modalRef.componentInstance.contentMessage = "You don't have the permissions needed to access this.";

      this.router.navigate(['login']);
    })
  }
}
