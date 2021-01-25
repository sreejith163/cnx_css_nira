import { Component, Input, OnInit } from '@angular/core';
import { LoggedUserInfo } from 'src/app/core/models/logged-user-info.model';
import { AuthService } from 'src/app/core/services/auth.service';
import { EmployeeDetails } from '../../../modules/system-admin/models/employee-details.model';

@Component({
  selector: 'app-sidebar-menu',
  templateUrl: './sidebar-menu.component.html',
  styleUrls: ['./sidebar-menu.component.css']
})
export class SidebarMenuComponent implements OnInit {
  loggedUser: LoggedUserInfo;
  @Input() employee: EmployeeDetails;
  permissions: Permissions[] = [];
  constructor(private authService: AuthService) { }

  ngOnInit(): void {
    this.loggedUser = this.authService.getLoggedUserInfo();
  }

  public hideMenu(rolesPermitted: number[], employeeId) {
    // if role is inside rolesPermitted do not hide menu
    if (rolesPermitted.indexOf(employeeId) > -1) {
      return false;
    } else {
      return true;
    }
  }
}
