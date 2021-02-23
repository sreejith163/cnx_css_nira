import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/core/services/auth.service';
import { EmployeeRole } from '../../modules/system-admin/models/employee-role.model';
import { PermissionsService } from '../../modules/system-admin/services/permissions.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {
  employeeRole: EmployeeRole;

  constructor(private authService: AuthService, private permissionsService: PermissionsService) { }

  ngOnInit(): void {
    this.checkPermissions();
  }

  checkPermissions() {
    // check user's role for permissions on the current route
    let roleId = this.permissionsService.userRoleId;

    this.permissionsService.getRoleName(+roleId).subscribe((userRole: EmployeeRole) => {
      this.employeeRole = userRole;
    });
  }

}
