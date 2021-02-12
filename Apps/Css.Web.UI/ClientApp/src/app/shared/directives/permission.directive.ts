import { Directive, Input, ElementRef, TemplateRef, ViewContainerRef } from '@angular/core';
import { AuthService } from 'src/app/core/services/auth.service';
import { EmployeeDetails } from 'src/app/modules/home/modules/system-admin/models/employee-details.model';
import { PermissionsService } from 'src/app/modules/home/modules/system-admin/services/permissions.service';

export class PermissionAttribute{
  rolesPermitted: number[];
}

@Directive({
  selector: '[permission]'
})
export class PermissionDirective {
  @Input() permission: PermissionAttribute;

  constructor(
    private templateRef: TemplateRef<any>,
    private container: ViewContainerRef,
    private authService: AuthService, 
    private permissionsService: PermissionsService) { 
  }

  ngOnInit(): void {
    const employeeId = this.authService.getLoggedUserInfo().employeeId;
      // if role is inside rolesPermitted do not REMOVE element
      if (this.permission.rolesPermitted.indexOf(+this.permissionsService.userRoleId) > -1) {
        this.container.createEmbeddedView(this.templateRef);
      } else {
        this.container.clear();
      }
  }
  

}
