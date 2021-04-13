import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { NgbModal, NgbModalOptions, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  Validators
} from '@angular/forms';

import { AuthService, UAT } from 'src/app/core/services/auth.service';
import { CookieService } from 'ngx-cookie-service';
import { NgxSpinnerService } from 'ngx-spinner';
import { SpinnerOptions } from 'src/app/shared/util/spinner-options.util';
import { environment } from 'src/environments/environment';
import { EmployeeDetails } from 'src/app/modules/home/modules/system-admin/models/employee-details.model';
import { PermissionsService } from 'src/app/modules/home/modules/system-admin/services/permissions.service';
import { ErrorPopUpComponent } from 'src/app/shared/popups/error-pop-up/error-pop-up.component';
import { AuthLogin } from 'src/app/modules/home/modules/system-admin/models/auth-login.model';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {
  modalRef: NgbModalRef;
  public uatUsername: any;
  public uatPassword: any;
  public username: any;
  public password: any;
  uatLoginForm: FormGroup;
  authLogin: AuthLogin;
  spinner = "spinner";
  errorMessage: any;

  constructor(
    private toast: ToastrService,
    private spinnerService: NgxSpinnerService,
    private cookieService: CookieService,
    private router: Router,
    private modalService: NgbModal,
    private formBuilder: FormBuilder,
    private authService: AuthService,
    private route: ActivatedRoute,
    private permissionService: PermissionsService,
  ) { }

  ngOnInit(): void {
    this.uatLoginIntialization();
  }

  login() {
    this.authService.login();
  }

  // this is a temporary module and is to be deleted after UAT Testing

  openVerticallyCentered(content) {
    this.modalService.open(content, { centered: true });
  }

  // checkUATCredentials(uatUsername, uatPassword) {
  //   if (uatUsername === 'CSS_test_agent' && uatPassword === 'Neutron47coleslaw') { return true; }
  //   if (uatUsername === 'CSS_test_mgr' && uatPassword === 'Thread25shortness') { return true; }
  //   if (uatUsername === 'CSS_test_reports' && uatPassword === 'Statute13scariness') { return true; }
  //   if (uatUsername === 'CSS_test_wfm' && uatPassword === 'Neutron47coleslaw') { return true; }
  //   if (uatUsername === 'CSS_test_admin' && uatPassword === 'Pastor40overripe136') { return true; }
  //   return false;
  // }

  login_uat_test() {

    this.permissionService.authLogin(this.uatUsername, this.uatPassword)
      .subscribe(
        (response) => {
          this.spinnerService.show(this.spinner);
          this.authLogin = response;
          const userUAT: UAT = {
            uid: response.employeeId,
            employeeId: response.employeeId,
            displayName: `${this.firstUpper(response.firstname)} ${this.firstUpper(response.lastname)}`

          };
          setTimeout(() => {
         
            this.spinnerService.hide();
            this.modalService.dismissAll();
            this.loginUAT(userUAT);
          }, 2500);


        },
        (error) => {
          if (error.status == 404) {
            console.error('username and password not found')
            this.errorMessage = error.error;
            this.toast.error(error.error);

            this.spinnerService.hide;

          } else if (error.status == 400) {
            console.error('username or password is empty')

            this.errorMessage = error.error;
            this.toast.error(error.error);
            this.spinnerService.hide;

          }
        }

      )

   
  }
  firstUpper(ourString) {
    return ourString.substring(0, 1).toUpperCase() + ourString.substring(1).toLowerCase();
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
    this.permissionService.getEmployee(+userUAT.employeeId).subscribe((user: EmployeeDetails) => {
      this.permissionService.storePermission(user.userRoleId);

      this.router.navigate(['home']);
      this.spinnerService.hide(this.spinner);

    }, error => {

      this.router.navigate(['login']);
      this.spinnerService.hide(this.spinner);

      this.getModalPopup(ErrorPopUpComponent, 'sm');
      this.modalRef.componentInstance.headingMessage = 'Invalid Credentials';
      this.modalRef.componentInstance.contentMessage = "You don't have the permissions needed to access this.";

    })
  }


  convertUATUsername(uatUsername) {
    switch (uatUsername) {
      case 'CSS_test_agent': {
        return 5;
      }
      case 'CSS_test_mgr': {
        return 4;
      }
      case 'CSS_test_reports': {
        return 3;
      }
      case 'CSS_test_wfm': {
        return 2;
      }
      case 'CSS_test_admin': {
        return 1;
      }
      default: {
        break;
      }
    }
  }

  private uatLoginIntialization() {
    this.uatLoginForm = this.formBuilder.group({
      uatUsername: new FormControl(),
      uatPassword: new FormControl()
    });
  }
}
