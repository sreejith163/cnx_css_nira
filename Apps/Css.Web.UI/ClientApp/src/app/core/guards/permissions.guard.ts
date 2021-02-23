import { Injectable } from '@angular/core';
import { CanActivate, ActivatedRouteSnapshot, RouterStateSnapshot, UrlTree, Router } from '@angular/router';
import { Observable } from 'rxjs';
import { EmployeeDetails } from 'src/app/modules/home/modules/system-admin/models/employee-details.model';
import { EmployeeRole } from 'src/app/modules/home/modules/system-admin/models/employee-role.model';
import { PermissionsService } from 'src/app/modules/home/modules/system-admin/services/permissions.service';
import { AuthService } from '../services/auth.service';

@Injectable()
export class PermissionsGuard implements CanActivate {

  constructor(
    private permissionsService: PermissionsService,
    private authService: AuthService,
    private router: Router
  ) { }
  canActivate(
    next: ActivatedRouteSnapshot,
    state: RouterStateSnapshot): Observable<boolean | UrlTree> | Promise<boolean | UrlTree> | boolean | UrlTree {
    if (this.authService.isLoggedIn()) {
      // const employeeId = this.authService.getLoggedUserInfo().employeeId;
          // check user's role for permissions on the current route
      let roleId = this.permissionsService.userRoleId;
      
      this.permissionsService.getRoleName(+roleId).subscribe((userRole: EmployeeRole) => {
        if (!next.data.permissions.includes(userRole.roleId)) {
          if (state.url !== '/home/dashboard') {
            // redirect to home if not permitted
            this.router.navigate(['/home/dashboard']);
          }
        }
      });
      // // check user's role for permissions on the current route
      // this.permissionsService.getEmployee(+employeeId).subscribe((employee: EmployeeDetails) => {
      //   if (!next.data.permissions.includes(employee.userRoleId)) {
      //     if (state.url !== '/home/dashboard') {
      //       // redirect to home if not permitted
      //       this.router.navigate(['/home/dashboard']);
      //     }
      //   }
      // }, error => {

      // });
      return true;
    } else {
      this.router.navigate(['/login']);
      return false;
    }
  }
}
