import { Component, OnDestroy, OnInit } from '@angular/core';
import { Constants } from 'src/app/shared/util/constants.util';
import { PermissionsService } from '../../../services/permissions.service';
import { PermissionDetails } from '../../../models/permission-details.model';
import { UserRole } from '../../../models/user-role.model';
import { QueryStringParameters } from 'src/app/shared/models/query-string-parameters.model';
import { CssMenu } from 'src/app/shared/enums/css-menu.enum';
import { LanguageTranslationService } from 'src/app/shared/services/language-translation.service';
import { SubscriptionLike as ISubscription } from 'rxjs';
import { TranslationDetails } from 'src/app/shared/models/translation-details.model';
import { GenericStateManagerService } from 'src/app/shared/services/generic-state-manager.service';
import { TranslateService } from '@ngx-translate/core';
import { Language } from 'src/app/shared/models/language-value.model';
import { LanguagePreference } from 'src/app/shared/models/language-preference.model';
import { LanguagePreferenceService } from 'src/app/shared/services/language-preference.service';
import { ActivatedRoute } from '@angular/router';
import { AuthService } from 'src/app/core/services/auth.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { HeaderPagination } from 'src/app/shared/models/header-pagination.model';
import { SpinnerOptions } from 'src/app/shared/util/spinner-options.util';
import { Employee } from '../../../models/employee.model';
import { NgbModal, NgbModalOptions, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { ComponentOperation } from 'src/app/shared/enums/component-operation.enum';
import { AddUpdatePermissionComponent } from '../add-update-permission/add-update-permission.component';
import { MessagePopUpComponent } from 'src/app/shared/popups/message-pop-up/message-pop-up.component';
import { EmployeeDetails } from '../../../models/employee-details.model';
import { GenericPopUpComponent } from 'src/app/shared/popups/generic-pop-up/generic-pop-up.component';
import { ErrorWarningPopUpComponent } from 'src/app/shared/popups/error-warning-pop-up/error-warning-pop-up.component';
import { ContentType } from 'src/app/shared/enums/content-type.enum';
import { ConfirmationPopUpComponent } from 'src/app/shared/popups/confirmation-pop-up/confirmation-pop-up.component';

@Component({
  selector: 'app-permissions-list',
  templateUrl: './permissions-list.component.html',
  styleUrls: ['./permissions-list.component.scss']
})
export class PermissionsListComponent implements OnInit, OnDestroy {
  currentLanguage: string;
  LoggedUser;
  modalRef: NgbModalRef;

  currentPage = 1;
  pageSize = 10;
  totalRecord: number;
  selectedRow: number;
  employeeId: number;
  searchKeyword: string;
  orderBy = 'createdDate';
  sortBy = 'desc';
  spinner = 'permissionSpinner';

  spinnerOptions = SpinnerOptions;
  headerPaginationValues: HeaderPagination;
  paginationSize = Constants.paginationSize;
  maxLength = Constants.DefaultTextMaxLength;
  employees: Employee[] = [];

  roles: UserRole;
  userRoles = Constants.UserRoles;

  permissions: PermissionDetails[] = [];
  hiddenRolesList: UserRole[] = [];
  hiddenRoles: UserRole[] = [];

  getPermissionsSubscription: ISubscription;
  getTranslationSubscription: ISubscription;
  subscriptions: any[] = [];

  constructor(
    private modalService: NgbModal,
    private spinnerService: NgxSpinnerService,
    public translate: TranslateService,
    public permissionsService: PermissionsService,
    private authService: AuthService,
    private route: ActivatedRoute,
    private languagePreferenceService: LanguagePreferenceService
  ) {
    this.LoggedUser = this.authService.getLoggedUserInfo();
  }

  ngOnInit(): void {
    this.preLoadTranslations();
    this.loadTranslations();
    this.subscribeToTranslations();
    this.loadEmployees();
  }

  ngOnDestroy() {
    this.subscriptions.forEach(subscription => {
      if (subscription) {
        subscription.unsubscribe();
      }
    });
  }

  clearSearchText() {
    this.searchKeyword = undefined;
    this.loadEmployees();
  }

  changePageSize(pageSize: number) {
    this.pageSize = pageSize;
    this.loadEmployees();
  }

  changePage(page: number) {
    this.currentPage = page;
    this.loadEmployees();
  }


  private getModalPopup(component: any, size: string) {
    const options: NgbModalOptions = { backdrop: 'static', centered: true, size };
    this.modalRef = this.modalService.open(component, options);
  }

  private setComponentValues(operation: ComponentOperation) {
    this.modalRef.componentInstance.operation = operation;
  }

  private setComponentMessages(headingMessage: string, contentMessage: string) {
    this.modalRef.componentInstance.headingMessage = headingMessage;
    this.modalRef.componentInstance.contentMessage = contentMessage;
  }

  private showSuccessPopUpMessage(contentMessage: string, needRefresh = true) {
    this.getModalPopup(MessagePopUpComponent, 'sm');
    this.setComponentMessages('Success', contentMessage);
  }

  addPermission() {
    this.getModalPopup(AddUpdatePermissionComponent, 'lg');
    this.setComponentValues(ComponentOperation.Add);

    this.modalRef.result.then((confirmed) => {
      if (confirmed === true) {
        this.currentPage = 1;
        this.loadEmployees();
        this.showSuccessPopUpMessage('The record has been added!');
      }
    });
  }

  deleteUserPermission(employee: EmployeeDetails) {
    this.spinnerService.show(this.spinner, SpinnerOptions);
    this.permissionsService.deleteEmployee(employee.employeeId)
      .subscribe(() => {
        this.spinnerService.hide(this.spinner);
        this.loadEmployees();
        this.showSuccessPopUpMessage('User Permission has been deleted!');
      }, (error) => {
        this.spinnerService.hide(this.spinner);
        if (error.status === 409) {
          this.showErrorWarningPopUpMessage(error.error);
        }
      });
  }

  // Pop up messages
  private showErrorWarningPopUpMessage(contentMessage: string) {
    const options: NgbModalOptions = { backdrop: 'static', centered: true, size: 'sm' };
    const modalRef = this.modalService.open(ErrorWarningPopUpComponent, options);
    modalRef.componentInstance.headingMessage = 'Error';
    modalRef.componentInstance.contentMessage = contentMessage;
    modalRef.componentInstance.messageType = ContentType.String;

    return modalRef;
  }

  confirmDeleteUserPermission(employee: EmployeeDetails) {
    // this.getModalPopup(GenericPopUpComponent, 'md');
    // this.setConfirmDialogMessages(`Are you sure you? You won't be able to revert this!`, ``, `Yes`, `No`);
    this.getModalPopup(ConfirmationPopUpComponent, 'sm');
    this.setComponentMessages('Are you sure?', 'You won’t be able to revert this!');
    this.modalRef.componentInstance.deleteRecordIndex = employee.employeeId;
    this.modalRef.result.then((result) => {
      if (result && result === employee.employeeId) {
        this.deleteUserPermission(employee);
      }
    });
  }

  editUserPermission(employee: EmployeeDetails) {
    if(+this.permissionsService.userRoleId == 1){
      this.getModalPopup(AddUpdatePermissionComponent, 'lg');
      this.setComponentValues(ComponentOperation.Edit);
      this.modalRef.componentInstance.employee = employee;
  
      this.modalRef.result.then((confirmed) => {
        if (confirmed === true) {
          this.currentPage = 1;
          this.loadEmployees();
          this.showSuccessPopUpMessage('The record has been updated!');
        }
      });
    }
  }

  search() {
    this.loadEmployees();
  }

  sort(columnName: string, sortBy: string) {
    this.sortBy = sortBy === 'asc' ? 'desc' : 'asc';
    this.orderBy = columnName;

    this.loadEmployees();
  }


  private getQueryParams() {
    const queryParams = new QueryStringParameters();
    queryParams.pageNumber = this.currentPage;
    queryParams.pageSize = this.pageSize;
    queryParams.searchKeyword = this.searchKeyword ?? '';
    queryParams.orderBy = `${this.orderBy} ${this.sortBy}`;
    queryParams.fields = '';

    return queryParams;
  }

  private loadEmployees() {
    const queryParams = this.getQueryParams();
    this.spinnerService.show(this.spinner, SpinnerOptions);

    this.getPermissionsSubscription = this.permissionsService.getEmployees(queryParams)
      .subscribe((response) => {
        if (response.body) {
          this.employees = response.body;
          this.headerPaginationValues = JSON.parse(response.headers.get('x-pagination'));
          this.totalRecord = this.headerPaginationValues.totalCount;
        }
        this.spinnerService.hide(this.spinner);
      }, (error) => {
        this.spinnerService.hide(this.spinner);
        console.log(error);
      });

    this.subscriptions.push(this.getPermissionsSubscription);
  }

  private subscribeToTranslations() {
    this.getTranslationSubscription = this.languagePreferenceService.userLanguageChanged.subscribe(
      (language) => {
        if (language) {
          this.loadTranslations();
        }
      });

    this.subscriptions.push(this.getTranslationSubscription);
  }

  private preLoadTranslations() {
    // Preload the user language //
    const browserLang = this.route.snapshot.data.languagePreference.languagePreference;
    this.currentLanguage = browserLang ? browserLang : 'en';
    this.translate.use(this.currentLanguage);
  }

  private loadTranslations() {
    // load the user language from api //
    this.languagePreferenceService.getLanguagePreference(this.LoggedUser.employeeId).subscribe((langPref: LanguagePreference) => {
      this.currentLanguage = langPref.languagePreference ? langPref.languagePreference : 'en';
      this.translate.use(this.currentLanguage);
    });
  }
}
