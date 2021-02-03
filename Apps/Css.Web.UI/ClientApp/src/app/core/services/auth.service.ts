import { Injectable } from '@angular/core';
import { CookieService } from 'ngx-cookie-service';
import { environment } from 'src/environments/environment';
import { LoggedUserInfo } from '../models/logged-user-info.model';
import { uatenvironment } from 'src/environments/environment';
import jwt_decode from 'jwt-decode';
import { EmployeeDetails } from 'src/app/modules/home/modules/system-admin/models/employee-details.model';
import { PermissionsService } from 'src/app/modules/home/modules/system-admin/services/permissions.service';

@Injectable()
export class AuthService {

  constructor(
    private cookieService: CookieService
  ) { }

  isLoggedIn() {
    console.log('islog?');
    uatenvironment.UAT = Boolean (this.cookieService.get('UAT'));
    if (uatenvironment.UAT){
      console.log('islog');
      return true;
    }else{
      return this.cookieService.get(environment.settings.sessionName) ?? false;
    }
  }

  getLoggedUserInfo(): LoggedUserInfo {
    console.log('try');
    uatenvironment.UAT = Boolean (this.cookieService.get('UAT'));
    console.log(uatenvironment.UAT);
    if (uatenvironment.UAT){
      uatenvironment.uatUsername = this.cookieService.get('uatUsername');
      uatenvironment.uatEmployeeId = this.cookieService.get('uatEmployeeId');
      const user = new LoggedUserInfo();
      user.uid = uatenvironment.uatEmployeeId;
      user.employeeId = uatenvironment.uatEmployeeId;
      user.displayName = uatenvironment.uatUsername;
      console.log('dumaan');
      return user;
    }else{
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
}
