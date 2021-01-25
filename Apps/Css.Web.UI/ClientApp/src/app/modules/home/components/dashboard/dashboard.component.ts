import { Component, OnInit } from '@angular/core';
import { AuthService } from 'src/app/core/services/auth.service';
import { EmployeeDetails } from '../../modules/system-admin/models/employee-details.model';
import { PermissionsService } from '../../modules/system-admin/services/permissions.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {
  employee: EmployeeDetails;

  constructor(private authService: AuthService, private permissionsService: PermissionsService) { }

  ngOnInit(): void {
    this.checkPermissions();
  }

  checkPermissions() {
    const employeeId = this.authService.getLoggedUserInfo().employeeId;
    // check user's role for permissions on the current route
    this.permissionsService.getEmployee(+employeeId).subscribe((employee: EmployeeDetails) => {
      this.employee = employee;
    });
  }

}
