import { Injectable } from '@angular/core';
import { Resolve, ActivatedRouteSnapshot, RouterStateSnapshot } from '@angular/router';
import { CookieService } from 'ngx-cookie-service';
import { Observable } from 'rxjs';
import { environment } from 'src/environments/environment';
import { LanguagePreferenceService } from '../services/language-preference.service';
import jwt_decode from 'jwt-decode';
import { LoggedUserInfo } from 'src/app/core/models/logged-user-info.model';

@Injectable()
export class LanguagePreferenceResolver implements Resolve<any> {

    user: any;

    constructor(
        private languagePreferenceService: LanguagePreferenceService,
        private cookieService: CookieService) { }

    isLoggedIn() {
        return this.cookieService.get(environment.settings.sessionName) ?? false;
    }

    resolve(route: ActivatedRouteSnapshot, state: RouterStateSnapshot):
        any | Observable<any> | Promise<any> {
        if (this.isLoggedIn()) {
            const token = this.cookieService.get(environment.settings.sessionName);
            const decodedToken = jwt_decode(token);
            const user = new LoggedUserInfo();
            user.uid = decodedToken.uid;
            user.employeeId = decodedToken.employeeid;
            user.displayName = decodedToken.displayname;

            const languagePref = this.languagePreferenceService.getLanguagePreference(user.employeeId);

            return languagePref;
        }
    }
}

