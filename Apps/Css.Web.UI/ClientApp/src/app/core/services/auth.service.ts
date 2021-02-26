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
import { NgxSpinnerService } from 'ngx-spinner';

// temporary model for UAT
export class UAT {
  uid: string;
  employeeId: string;
  displayName: string;
}


@Injectable()
export class AuthService {
  private currentUserUATSubject: BehaviorSubject<UAT>;

  constructor(
    private cookieService: CookieService,
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

}
