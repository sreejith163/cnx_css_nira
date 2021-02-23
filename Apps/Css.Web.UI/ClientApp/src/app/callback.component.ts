import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CookieService } from 'ngx-cookie-service';
import { environment } from 'src/environments/environment';
import { AuthService } from './core/services/auth.service';
import { EmployeeDetails } from './modules/home/modules/system-admin/models/employee-details.model';
import { PermissionsService } from './modules/home/modules/system-admin/services/permissions.service';

@Component({
  selector: 'app-callback',
  templateUrl: './callback.component.html',
  styleUrls: ['./callback.component.scss']
})
export class CallbackComponent implements OnInit {

  constructor(
    private router: Router,
    private cookieService: CookieService,
    private route: ActivatedRoute,
    private authService: AuthService,
    private permissionsService: PermissionsService,
  ) { }

  ngOnInit(): void {
    const authorizationToken = this.route.snapshot.queryParamMap.get('token');
    if (authorizationToken) {
      this.cookieService.set(environment.settings.sessionName, authorizationToken, null, environment.settings.cookiePath, null, false, 'Strict');
      if (this.authService.isLoggedIn()){
            const employeeId = this.authService.getLoggedUserInfo().employeeId;
            // check user's permissions
            this.permissionsService.getEmployee(+employeeId).subscribe((employee: EmployeeDetails) =>
            {
              if(+employee.userRoleId > 0){
                this.permissionsService.storePermission(employee.userRoleId);
                // redirect to home if permission exists
                this.router.navigate(['home']);
              }else{
                //check if the user exists on agent admin
                this.permissionsService.getAgentInfo(+employeeId).subscribe(resp => {
                  // permission is default as agent
                  this.permissionsService.storePermission(5);
                  // redirect to home if permission exists
                  this.router.navigate(['home']);
                }, error => {
                  this.router.navigate(['login']);
                })
              }
            }, error => {
              var userRoleId = 5;
              // default to agent
              this.permissionsService.storePermission(userRoleId);
              this.router.navigate(['home']);
              console.log(this.permissionsService.userRoleId)
            });
      }else{
        this.router.navigate(['login']);
      }
    }
  }
}
