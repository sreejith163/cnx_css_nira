import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NgbModal, NgbModalOptions, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { CookieService } from 'ngx-cookie-service';
import { NgxSpinnerService } from 'ngx-spinner';
import { environment } from 'src/environments/environment';
import { AuthService } from './core/services/auth.service';
import { EmployeeDetails } from './modules/home/modules/system-admin/models/employee-details.model';
import { PermissionsService } from './modules/home/modules/system-admin/services/permissions.service';
import { ErrorPopUpComponent } from './shared/popups/error-pop-up/error-pop-up.component';
import { UserLoginLog } from './core/models/user-logger.model';

@Component({
  selector: 'app-callback',
  templateUrl: './callback.component.html',
  styleUrls: ['./callback.component.scss']
})
export class CallbackComponent implements OnInit {

  modalRef: NgbModalRef;
  spinner = 'spinner';

  constructor(
    private router: Router,
    private cookieService: CookieService,
    private route: ActivatedRoute,
    private authService: AuthService,
    private permissionsService: PermissionsService,
    private modalService: NgbModal,
    private spinnerService: NgxSpinnerService,
  ) { }

  private getModalPopup(component: any, size: string) {
    const options: NgbModalOptions = { backdrop: 'static', centered: true, size };
    this.modalRef = this.modalService.open(component, options);
  }
  browserCheck(){
   var browser = (function(){
      var ua= navigator.userAgent, tem, 
      M= ua.match(/(opera|chrome|safari|firefox|msie|trident(?=\/))\/?\s*(\d+)/i) || [];
      if(/trident/i.test(M[1])){
          tem=  /\brv[ :]+(\d+)/g.exec(ua) || [];
          return 'IE '+(tem[1] || '');
      }
      if(M[1]=== 'Chrome'){
          tem= ua.match(/\b(OPR|Edge)\/(\d+)/);
          if(tem!= null) return tem.slice(1).join(' ').replace('OPR', 'Opera');
      }
      M= M[2]? [M[1], M[2]]: [navigator.appName, navigator.appVersion, '-?'];
      if((tem= ua.match(/version\/(\d+)/i))!= null) M.splice(1, 1, tem[1]);
      return M.join(' ');
  })();
  return browser
  }
  ngOnInit(): void {
    this.spinnerService.show(this.spinner);
    const authorizationToken = this.route.snapshot.queryParamMap.get('token');
    if (authorizationToken) {
      this.cookieService.set(environment.settings.sessionName, authorizationToken, null, environment.settings.cookiePath, null, false, 'Strict');
      if (this.authService.isLoggedIn()){
            const employeeId = this.authService.getLoggedUserInfo().employeeId;
            const userLogged = new UserLoginLog();
            userLogged.sso = this.authService.getLoggedUserInfo().uid;
            userLogged.timeStamp = new Date().toISOString();
            userLogged.userAgent =  this.browserCheck();
            //userLogged.timeStamp = new Date().toda();

            // check user's permissions

            this.permissionsService.userLogging(+employeeId, userLogged).subscribe(responseList => {
              
              this.permissionsService.storePermission(responseList[1].userRoleId);

              // redirect to home if permission exists
              this.router.navigate(['home']);
              this.spinnerService.hide(this.spinner);

            }, error => {
              this.router.navigate(['login']);
              this.spinnerService.hide(this.spinner);

              this.getModalPopup(ErrorPopUpComponent, 'sm');
              this.modalRef.componentInstance.headingMessage = 'Invalid Credentials';
              this.modalRef.componentInstance.contentMessage = 'You don\'t have the permissions needed to access this.';
            });

            // this.permissionsService.getEmployee(+employeeId).subscribe((employee: EmployeeDetails) =>
            // {
            //   this.permissionsService.storePermission(employee.userRoleId);
            //   this.permissionsService.addUserLog(userLogged);

            //   // redirect to home if permission exists
            //   this.router.navigate(['home']);
            //   this.spinnerService.hide(this.spinner);

            //   //log 

            // }, error => {

            //   this.router.navigate(['login']);
            //   this.spinnerService.hide(this.spinner);

            //   this.getModalPopup(ErrorPopUpComponent, 'sm');
            //   this.modalRef.componentInstance.headingMessage = 'Invalid Credentials';
            //   this.modalRef.componentInstance.contentMessage = 'You don\'t have the permissions needed to access this.';

            // });
      }else{
        this.spinnerService.hide(this.spinner);
        this.router.navigate(['login']);
      }
    }
  }
}
