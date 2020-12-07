import { Component, OnDestroy, OnInit } from '@angular/core';
import { Constants } from 'src/app/shared/util/constants.util';
import { PermissionsService } from '../../../services/permissions.service';
import { PermissionDetails } from '../../../models/permission-details.model';
import { UserRole } from '../../../models/user-role.model';
import { QueryStringParameters } from 'src/app/shared/models/query-string-parameters.model';
import { CssLanguage } from 'src/app/shared/enums/css-language.enum';
import { CssMenu } from 'src/app/shared/enums/css-menu.enum';
import { LanguageTranslationService } from 'src/app/shared/services/language-translation.service';
import { SubscriptionLike as ISubscription } from 'rxjs';
import { TranslationDetails } from 'src/app/shared/models/translation-details.model';

@Component({
  selector: 'app-permissions-list',
  templateUrl: './permissions-list.component.html',
  styleUrls: ['./permissions-list.component.scss']
})
export class PermissionsListComponent implements OnInit, OnDestroy {

  currentPage = 1;
  pageSize = 10;
  totalRecord: number;
  selectedRow: number;
  employeeId: number;
  searchKeyword: string;
  orderBy = 'createdDate';
  sortBy = 'desc';

  roles: UserRole;
  paginationSize = Constants.paginationSize;
  userRoles = Constants.UserRoles;

  translationValues: TranslationDetails[] = [];
  permissions: PermissionDetails[] = [];
  hiddenRolesList: UserRole[] = [];
  hiddenRoles: UserRole[] = [];

  getPermissionsSubscription: any;
  getTranslationValuesSubscription: ISubscription;
  subscriptions: any[] = [];

  constructor(
    private permissionsService: PermissionsService,
    private translationService: LanguageTranslationService
  ) { }

  ngOnInit(): void {
    this.loadTranslationValues();
    this.getPermissions();
    this.getUserRolesToHide();
  }

  ngOnDestroy() {
    this.subscriptions.forEach(subscription => {
      if (subscription) {
        subscription.unsubscribe();
      }
    });
  }

  changePageSize(pageSize: number) {
    this.pageSize = pageSize;
    this.getPermissions();
  }

  changePage(page: number) {
    this.currentPage = page;
    this.getPermissions();
  }

  checkPermission(permission: PermissionDetails, userRoleId: number) {
    return permission.roles.some(x => x.id === userRoleId) ? 'Yes' : 'No';
  }

  onEmployeeSelected(employeeId: number) {
    this.employeeId = employeeId;
    this.selectedRow = undefined;
  }

  onPermissionUpdated() {
    this.resetSelectedData();
  }

  onPermissionAdded() {
    this.resetSelectedData();
    this.getPermissions();
  }

  onPermissionCleared() {
    this.resetSelectedData();
  }

  setSelectedEmployee(employeeId: number, index: number) {
    this.selectedRow = index;
    this.employeeId = employeeId;
  }

  getUserRolesToHide() {
    for (let i = 6; i <= 10; i++) {
      const role = new UserRole();
      role.id = i;
      role.role = this.userRoles[i - 6].role;

      this.hiddenRoles.push(role);
    }
  }

  onCheckRolesToHide(e) {
    if (e.target.checked) {
      const item = new UserRole();
      item.role = e.target.value;
      this.hiddenRolesList.push(item);
    } else {
      const index = this.hiddenRolesList.findIndex(x => x.role === e.target.value);
      this.hiddenRolesList.splice(index, 1);
    }
  }

  hasRoleHidden(role: string) {
    return this.hiddenRolesList.findIndex(x => x.role === role) === -1;
  }

  hasHideRoleSelected(role: string) {
    return this.hiddenRolesList.findIndex(x => x.role === role) !== -1;
  }

  isRowSelected(index: number) {
    return this.selectedRow === index;
  }

  search() {
    this.getPermissions();
  }

  private resetSelectedData() {
    this.employeeId = undefined;
    this.selectedRow = undefined;
  }

  private getQueryParams() {
    const permissionsQueryParams = new QueryStringParameters();
    permissionsQueryParams.pageNumber = this.currentPage;
    permissionsQueryParams.pageSize = this.pageSize;
    permissionsQueryParams.searchKeyword = this.searchKeyword ?? '';
    permissionsQueryParams.orderBy = `${this.orderBy} ${this.sortBy}`;
    permissionsQueryParams.fields = '';

    return permissionsQueryParams;
  }

  private getPermissions() {
    const queryParams = this.getQueryParams();

    this.getPermissionsSubscription = this.permissionsService.getPermissions(queryParams)
      .subscribe((data) => {
        this.permissions = data.sort((a, b) => new Date(b.createdDate).getTime() - new Date(a.createdDate).getTime());
        this.totalRecord = this.permissions.length;
      }, (error) => {
        console.log(error);
      });

    this.subscriptions.push(this.getPermissionsSubscription);
  }

  private loadTranslationValues() {
    const languageId = CssLanguage.English;
    const menuId = CssMenu.Permissions;

    this.getTranslationValuesSubscription = this.translationService.getMenuTranslations(languageId, menuId)
      .subscribe((response) => {
        if (response) {
          this.translationValues = response;
        }
      }, (error) => {
        console.log(error);
      });

    this.subscriptions.push(this.getTranslationValuesSubscription);
  }
}
