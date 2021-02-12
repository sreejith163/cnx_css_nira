import { Component, OnInit } from '@angular/core';
import * as $ from 'jquery';
import * as AdminLte from 'admin-lte';
import { ActivatedRoute, Router } from '@angular/router';
import { SubscriptionLike as ISubscription } from 'rxjs';
import { GenericStateManagerService } from 'src/app/shared/services/generic-state-manager.service';
import { CSS_LANGUAGES } from 'src/app/shared/models/language-value.model';
import { TranslateService } from '@ngx-translate/core';
import { LanguagePreferenceService } from 'src/app/shared/services/language-preference.service';
import { LanguagePreference } from 'src/app/shared/models/language-preference.model';
import { AuthService } from 'src/app/core/services/auth.service';
import { PermissionsService } from '../../modules/system-admin/services/permissions.service';
import { EmployeeDetails } from '../../modules/system-admin/models/employee-details.model';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  employeeDetails: EmployeeDetails;
  constructor(
    private router: Router,
    private authService: AuthService,
    private permissionsService: PermissionsService
  ) {
    this.checkPermissions();
  }

  ngOnInit(): void {
    $('[data-widget="treeview"]').each(x => {
      AdminLte.Treeview._jQueryInterface.call($(this), 'init');
    });
  }

  navigateToAgentAdmin() {
    this.router.navigate(['add-agent-profile']);
  }


  checkPermissions() {
    const employeeId = this.authService.getLoggedUserInfo().employeeId;

    // check user's role for permissions on the current route
    this.permissionsService.getEmployee(+employeeId).subscribe((employee: EmployeeDetails) => {
      this.employeeDetails = employee;
    });
  }
}
