import { Injectable } from '@angular/core';
import { CookieService } from 'ngx-cookie-service';
import { environment } from 'src/environments/environment';
import { LoggedUserInfo } from '../models/logged-user-info.model';
import jwt_decode from 'jwt-decode';
import { EmployeeDetails } from 'src/app/modules/home/modules/system-admin/models/employee-details.model';
import { PermissionsService } from 'src/app/modules/home/modules/system-admin/services/permissions.service';
import { Router } from '@angular/router';
import { BehaviorSubject } from 'rxjs';

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
    private router: Router,
  ) {

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
    if (this.currentUserUATValue) {
      return true;
    } else {
      return this.cookieService.get(environment.settings.sessionName) ?? false;
    }
  }

  getLoggedUserInfo(): LoggedUserInfo {
    if (this.currentUserUATValue) {
      const user = new LoggedUserInfo();
      user.uid = this.cookieService.get('uid');
      user.employeeId = this.cookieService.get('employeeId');
      user.displayName = this.cookieService.get('displayName');
      return user;
    } else {
      if (this.isLoggedIn()) {
        const token = this.cookieService.get(environment.settings.sessionName);
        const decodedToken = jwt_decode(token);
        const user = new LoggedUserInfo();
        user.uid = decodedToken.uid;
        user.employeeId = decodedToken.employeeid;
        user.displayName = decodedToken.displayname;
        return user;
      }
    }

  }

  loginUAT(userUAT: UAT) {
    // store the UAT details inside a cookie to persist uat session
    this.cookieService.set('employeeId', userUAT.employeeId);
    this.cookieService.set('uid', userUAT.uid);
    this.cookieService.set('displayName', userUAT.displayName);

    this.router.navigate(['home']);
  }
}
