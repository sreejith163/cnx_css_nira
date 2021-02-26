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

@Component({
  selector: 'app-callback',
  templateUrl: './callback.component.html',
  styleUrls: ['./callback.component.scss']
})
export class CallbackComponent implements OnInit {

  modalRef: NgbModalRef;
  spinner = "spinner";

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

  ngOnInit(): void {
    this.spinnerService.show(this.spinner);
    const authorizationToken = this.route.snapshot.queryParamMap.get('token');
    if (authorizationToken) {
      this.cookieService.set(environment.settings.sessionName, authorizationToken, null, environment.settings.cookiePath, null, false, 'Strict');
      if (this.authService.isLoggedIn()){
            const employeeId = this.authService.getLoggedUserInfo().employeeId;
            // check user's permissions
            this.permissionsService.getEmployee(+employeeId).subscribe((employee: EmployeeDetails) =>
            {
              this.permissionsService.storePermission(employee.userRoleId);

              // redirect to home if permission exists
              this.router.navigate(['home']);                
              this.spinnerService.hide(this.spinner);

            }, error => {

              this.router.navigate(['login']);
              this.spinnerService.hide(this.spinner);

              this.getModalPopup(ErrorPopUpComponent, 'sm');
              this.modalRef.componentInstance.headingMessage = 'Invalid Credentials';
              this.modalRef.componentInstance.contentMessage = "You don't have the permissions needed to access this.";

            });
      }else{
        this.spinnerService.hide(this.spinner);
        this.router.navigate(['login']);
      }
    }
  }
}
