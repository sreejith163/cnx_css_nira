import { Injectable } from '@angular/core';
import { CookieService } from 'ngx-cookie-service';
import { environment } from 'src/environments/environment';
import { LoggedUserInfo } from '../models/logged-user-info.model';

import jwt_decode from 'jwt-decode';

@Injectable()
export class AuthService {

  constructor(
    private cookieService: CookieService
  ) { }

  isLoggedIn() {
    return this.cookieService.get(environment.settings.sessionName) ?? false;
  }

  getLoggedUserInfo(): LoggedUserInfo {
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
