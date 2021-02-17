import { Component, OnDestroy, OnInit } from '@angular/core';
import { NgbModal, NgbModalOptions, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { NgxSpinnerService } from 'ngx-spinner';
import { SubscriptionLike as ISubscription } from 'rxjs';

import { ConfirmationPopUpComponent } from 'src/app/shared/popups/confirmation-pop-up/confirmation-pop-up.component';
import { AddUpdateClientNameComponent } from '../add-update-client-name/add-update-client-name.component';
import { MessagePopUpComponent } from 'src/app/shared/popups/message-pop-up/message-pop-up.component';
import { ErrorWarningPopUpComponent } from 'src/app/shared/popups/error-warning-pop-up/error-warning-pop-up.component';

import { ClientDetails } from '../../../models/client-details.model';
import { ClientNameQueryParameters } from '../../../models/client-name-query-parameters.model';
import { HeaderPagination } from 'src/app/shared/models/header-pagination.model';
import { PaginationSize } from 'src/app/shared/models/pagination-size.model';
import { ComponentOperation } from 'src/app/shared/enums/component-operation.enum';
import { TranslationDetails } from 'src/app/shared/models/translation-details.model';
import { CssMenu } from 'src/app/shared/enums/css-menu.enum';

import { Constants } from 'src/app/shared/util/constants.util';
import { SpinnerOptions } from 'src/app/shared/util/spinner-options.util';

import { ClientService } from '../../../services/client.service';
import { LanguageTranslationService } from 'src/app/shared/services/language-translation.service';
import { GenericStateManagerService } from 'src/app/shared/services/generic-state-manager.service';
import { TranslateService } from '@ngx-translate/core';
import { Language } from 'src/app/shared/models/language-value.model';
import { LanguagePreference } from 'src/app/shared/models/language-preference.model';
import { LanguagePreferenceService } from 'src/app/shared/services/language-preference.service';
import { ActivatedRoute } from '@angular/router';
import { AuthService } from 'src/app/core/services/auth.service';

@Component({
  selector: 'app-client-name-list',
  templateUrl: './client-name-list.component.html',
  styleUrls: ['./client-name-list.component.css']
})
export class ClientNameListComponent implements OnInit, OnDestroy {
  currentLanguage: string;
  LoggedUser;

  currentPage = 1;
  pageSize = 10;
  characterSplice = 25;
  totalClientRecord: number;
  maxLength = Constants.DefaultTextMaxLength;

  searchKeyword: string;
  orderBy = 'createdDate';
  sortBy = 'desc';
  spinner = 'tableSpinner';

  modalRef: NgbModalRef;
  headerPaginationValues: HeaderPagination;
  paginationSize: PaginationSize[] = [];
  translationValues: TranslationDetails[];
  clientsDetails: ClientDetails[] = [];

  getTranslationSubscription: ISubscription;
  getAllClientDetailsSubscription: ISubscription;
  deleteClientSubscription: ISubscription;
  subscriptionList: ISubscription[] = [];

  constructor(
    public translate: TranslateService,
    private modalService: NgbModal,
    private spinnerService: NgxSpinnerService,
    private clientService: ClientService,
    private authService: AuthService,
    private route: ActivatedRoute,
    private languagePreferenceService: LanguagePreferenceService
  ) {
    this.LoggedUser = this.authService.getLoggedUserInfo();
  }

  ngOnInit(): void {
    this.paginationSize = Constants.paginationSize;
    this.preLoadTranslations();
    this.loadTranslations();
    this.loadClients();
    this.subscribeToTranslations();

  }

  ngOnDestroy() {
    this.subscriptionList.forEach(subscription => {
      if (subscription) {
        subscription.unsubscribe();
      }
    });
  }

  clearSearchText() {
    this.searchKeyword = undefined;
    this.loadClients();
  }

  changePageSize(pageSize: number) {
    this.pageSize = pageSize;
    this.loadClients();
  }

  changePage(page: number) {
    this.currentPage = page;
    this.loadClients();
  }

  addClient() {
    this.getModalPopup(AddUpdateClientNameComponent, 'lg');
    this.setComponentValues(ComponentOperation.Add);

    this.modalRef.result.then(() => {
      this.currentPage = 1;
      this.showSuccessPopUpMessage('The record has been added!');
    });
  }

  editClient(data: ClientDetails) {
    this.getModalPopup(AddUpdateClientNameComponent, 'lg');
    this.setComponentValues(ComponentOperation.Edit);
    this.modalRef.componentInstance.clientDetails = data;

    this.modalRef.result.then((result: any) => {
      if (result.needRefresh) {
        this.showSuccessPopUpMessage('The record has been updated!');
      } else {
        this.showSuccessPopUpMessage('No changes has been made!', false);
      }
    });
  }

  deleteClient(clientIndex: number) {
    this.getModalPopup(ConfirmationPopUpComponent, 'sm');
    this.setComponentMessages('Are you sure?', 'You won’t be able to revert this!');
    this.modalRef.componentInstance.deleteRecordIndex = clientIndex;

    this.modalRef.result.then((result) => {
      if (result && result === clientIndex) {

        this.spinnerService.show(this.spinner, SpinnerOptions);
        this.deleteClientSubscription = this.clientService.deleteClient(clientIndex)
          .subscribe(() => {
            this.spinnerService.hide(this.spinner);
            this.showSuccessPopUpMessage('The record has been deleted!');
          }, (error) => {
            this.spinnerService.hide(this.spinner);
            if (error.status === 424) {
              this.getModalPopup(ErrorWarningPopUpComponent, 'sm');
              this.setComponentMessages('Error', error.error);
            }
          });

        this.subscriptionList.push(this.deleteClientSubscription);
      }
    });
  }

  searchClients() {
    this.loadClients();
  }

  sortClients(columnName: string, sortBy: string) {
    this.sortBy = sortBy === 'asc' ? 'desc' : 'asc';
    this.orderBy = columnName;

    this.loadClients();
  }

  clearSearchKeyword() {
    this.searchKeyword = undefined;
  }

  private showSuccessPopUpMessage(contentMessage: string, needRefresh = true) {
    this.getModalPopup(MessagePopUpComponent, 'sm');
    this.setComponentMessages('Success', contentMessage);

    if (needRefresh) {
      this.modalRef.result.then(() => {
        this.loadClients();
      });
    }
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

  private getQueryParams() {
    const clientQueryParams = new ClientNameQueryParameters();
    clientQueryParams.pageNumber = this.currentPage;
    clientQueryParams.pageSize = this.pageSize;
    clientQueryParams.searchKeyword = this.searchKeyword ?? '';
    clientQueryParams.orderBy = `${this.orderBy} ${this.sortBy}`;
    clientQueryParams.fields = '';

    return clientQueryParams;
  }

  private loadClients() {
    const queryParams = this.getQueryParams();
    this.spinnerService.show(this.spinner, SpinnerOptions);

    this.getAllClientDetailsSubscription = this.clientService.getClients(queryParams)
      .subscribe((response) => {
        if (response.body) {
          this.clientsDetails = response.body;
          this.headerPaginationValues = JSON.parse(response.headers.get('x-pagination'));
          this.totalClientRecord = this.headerPaginationValues.totalCount;
        }

        this.spinnerService.hide(this.spinner);
      }, (error) => {
        this.spinnerService.hide(this.spinner);
        console.log(error);
      });

    this.subscriptionList.push(this.getAllClientDetailsSubscription);
  }

  private subscribeToTranslations() {
    this.getTranslationSubscription = this.languagePreferenceService.userLanguageChanged.subscribe(
      (language) => {
        if (language) {
          this.loadTranslations();
        }
      });

    this.subscriptionList.push(this.getTranslationSubscription);
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
