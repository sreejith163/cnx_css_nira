import { Component, Input, OnInit } from '@angular/core';
import { LoggedUserInfo } from 'src/app/core/models/logged-user-info.model';
import { AuthService } from 'src/app/core/services/auth.service';
import { EmployeeDetails } from '../../../modules/system-admin/models/employee-details.model';
import { PermissionsService } from '../../../modules/system-admin/services/permissions.service';

@Component({
  selector: 'app-sidebar-menu',
  templateUrl: './sidebar-menu.component.html',
  styleUrls: ['./sidebar-menu.component.css']
})
export class SidebarMenuComponent implements OnInit {
  loggedUser: LoggedUserInfo;
  @Input() employee: EmployeeDetails;
  constructor(public permissionService: PermissionsService) { }

  ngOnInit(): void {

  }
}
