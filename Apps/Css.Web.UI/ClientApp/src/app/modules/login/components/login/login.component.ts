import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { environment } from 'src/environments/environment';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
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

@Component({
  selector: 'app-login',
  templateUrl: './login.component.html',
  styleUrls: ['./login.component.css']
})
export class LoginComponent implements OnInit {

  public uatUsername: any;
  public uatPassword: any;
  uatLoginForm: FormGroup;
  spinner = 'uatLoginSpinner';

  constructor(
    private spinnerService: NgxSpinnerService,
    private cookieService: CookieService,
    private router: Router,
    private modalService: NgbModal,
    private formBuilder: FormBuilder,
    private authService: AuthService,
    private route: ActivatedRoute

  ) { }

  ngOnInit(): void {
    this.uatLoginIntialization();
   }

  login() {
    window.location.href = environment.sso.authBaseUrl + environment.sso.authAppToken;
  }

  // this is a temporary module and is to be deleted after UAT Testing

  openVerticallyCentered(content) {
    this.modalService.open(content, { centered: true });
  }

  checkUATCredentials(uatUsername, uatPassword){
    if (uatUsername === 'CSS_test_agent' && uatPassword === 'Neutron47coleslaw') {return true; }
    if (uatUsername === 'CSS_test_sup' && uatPassword === 'Dramatize63surgical') {return true; }
    if (uatUsername === 'CSS_test_mgr' && uatPassword === 'Thread25shortness') {return true; }
    if (uatUsername === 'CSS_test_reports' && uatPassword === 'Statute13scariness') {return true; }
    if (uatUsername === 'CSS_test_wfm' && uatPassword === 'Neutron47coleslaw') {return true; }
    if (uatUsername === 'CSS_test_admin' && uatPassword === 'Pastor40overripe136') {return true; }
    return false;
  }

  login_uat_test(){
    // redirect to home if permission exists
    if (this.checkUATCredentials(this.uatUsername, this.uatPassword)){
      this.spinnerService.show(this.spinner, SpinnerOptions);

      const userUAT: UAT = {
        uid: this.uatUsername,
        employeeId: this.convertUATUsername(this.uatUsername).toString(),
        displayName: this.uatUsername
      };

      // pass the UAT Object to the authService for handling
      setTimeout(() => {
        /** spinner ends after 2.5 seconds */
        this.spinnerService.hide(this.spinner);
        this.modalService.dismissAll();
        this.authService.loginUAT(userUAT);
      }, 2500);

    }else{
      console.log('invalid credentials');
    }

  }


  convertUATUsername(uatUsername){
    switch (uatUsername) {
      case 'CSS_test_agent': {
        return 6;
      }
      case 'CSS_test_sup': {
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
