
import { Component, OnDestroy, OnInit } from '@angular/core';
import { WeekDay } from '@angular/common';
import { NgbModal, NgbModalOptions, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { NgxSpinnerService } from 'ngx-spinner';
import { SubscriptionLike as ISubscription } from 'rxjs';

import { ConfirmationPopUpComponent } from 'src/app/shared/popups/confirmation-pop-up/confirmation-pop-up.component';
import { MessagePopUpComponent } from 'src/app/shared/popups/message-pop-up/message-pop-up.component';
import { AddUpdateClientLobGroupComponent } from '../add-update-client-lob-group/add-update-client-lob-group.component';
import { ErrorWarningPopUpComponent } from 'src/app/shared/popups/error-warning-pop-up/error-warning-pop-up.component';

import { HeaderPagination } from 'src/app/shared/models/header-pagination.model';
import { ComponentOperation } from 'src/app/shared/enums/component-operation.enum';
import { Constants } from 'src/app/shared/util/constants.util';
import { ClientLOBGroupDetails } from '../../../models/client-lob-group-details.model';
import { ClientLobGroupQueryParameters } from '../../../models/client-lob-group-query-parameters.model';
import { PaginationSize } from 'src/app/shared/models/pagination-size.model';
import { SpinnerOptions } from 'src/app/shared/util/spinner-options.util';
import { CssMenu } from 'src/app/shared/enums/css-menu.enum';
import { TranslationDetails } from 'src/app/shared/models/translation-details.model';

import { ClientLobGroupService } from '../../../services/client-lob-group.service';
import { LanguageTranslationService } from 'src/app/shared/services/language-translation.service';
import { GenericStateManagerService } from 'src/app/shared/services/generic-state-manager.service';
import { Language } from 'src/app/shared/models/language-value.model';
import { TranslateService } from '@ngx-translate/core';
import { LanguagePreferenceService } from 'src/app/shared/services/language-preference.service';
import { ActivatedRoute } from '@angular/router';
import { AuthService } from 'src/app/core/services/auth.service';
import { LanguagePreference } from 'src/app/shared/models/language-preference.model';

@Component({
  selector: 'app-client-lob-group-list',
  templateUrl: './client-lob-group-list.component.html',
  styleUrls: ['./client-lob-group-list.component.scss']
})

export class ClientLobGroupListComponent implements OnInit, OnDestroy {
  currentLanguage: string;
  LoggedUser;

  currentPage = 1;
  pageSize = 10;
  characterSplice = 25;
  totalClientLOBGroupRecord: number;
  clientLOBGroupId: number;
  clientId?: number;
  spinner = 'tableSpinner';

  searchKeyword: string;
  orderBy = 'CreatedDate';
  sortBy = 'desc';
  sortKeyword: 'asc' | 'desc';
  weekDay = WeekDay;

  modalRef: NgbModalRef;
  headerPaginationValues: HeaderPagination;
  maxLength = Constants.DefaultTextMaxLength;
  paginationSize: PaginationSize[] = [];
  translationValues: TranslationDetails[];
  clientLOBGroupDetails: ClientLOBGroupDetails[] = [];

  getTranslationSubscription: ISubscription;
  getAllClientLOBGroupDetailsSubscription: ISubscription;
  getClientLOBGroupTranslationSubscription: ISubscription;
  deleteClientLOBGroupSubscription: ISubscription;
  subscriptionList: ISubscription[] = [];

  constructor(
    public translate: TranslateService,
    private modalService: NgbModal,
    private spinnerService: NgxSpinnerService,
    private clientLOBGroupService: ClientLobGroupService,
    private authService: AuthService,
    private route: ActivatedRoute,
    private languagePreferenceService: LanguagePreferenceService
  ) {
    this.LoggedUser = this.authService.getLoggedUserInfo();
  }

  ngOnInit(): void {
    this.sortKeyword = 'asc';
    this.paginationSize = Constants.paginationSize;
    this.preLoadTranslations();
    this.loadTranslations();
    this.loadClientLOBGroups();
    this.subscribeToTranslations();
  }

  ngOnDestroy() {
    this.subscriptionList.forEach(subscription => {
      if (subscription) {
        subscription.unsubscribe();
      }
    });
  }

  changePageSize(pageSize: number) {
    this.pageSize = pageSize;
    this.loadClientLOBGroups();
  }

  changePage(page: number) {
    this.currentPage = page;
    this.loadClientLOBGroups();
  }

  clearSearchText() {
    this.searchKeyword = undefined;
    this.loadClientLOBGroups();
  }

  setClient(client: number) {
    this.clientId = client;
    this.loadClientLOBGroups();
  }

  addClientLOBGroup() {
    this.getModalPopup(AddUpdateClientLobGroupComponent, 'lg');
    this.setComponentValues(ComponentOperation.Add, this.translationValues);

    this.modalRef.result.then(() => {
      this.currentPage = 1;
      this.showSuccessPopUpMessage('The record has been added!');
    });
  }

  editClientLOBGroup(data: ClientLOBGroupDetails) {
    this.getModalPopup(AddUpdateClientLobGroupComponent, 'lg');
    this.setComponentValues(ComponentOperation.Edit, this.translationValues);
    this.modalRef.componentInstance.clientLOBGroupDetails = data;

    this.modalRef.result.then((result: any) => {
      if (result.needRefresh) {
        this.showSuccessPopUpMessage('The record has been updated!');
      } else {
        this.showSuccessPopUpMessage('No changes has been made!', false);
      }
    });
  }

  deleteClientLOBGroup(clientIndex: number) {
    this.getModalPopup(ConfirmationPopUpComponent, 'sm');
    this.setComponentMessages('Are you sure?', 'You won’t be able to revert this!');
    this.modalRef.componentInstance.deleteRecordIndex = clientIndex;

    this.modalRef.result.then((result) => {
      if (result && result === clientIndex) {
        this.spinnerService.show(this.spinner, SpinnerOptions);
        this.deleteClientLOBGroupSubscription = this.clientLOBGroupService.deleteClientLOBGroup(clientIndex)
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

        this.subscriptionList.push(this.deleteClientLOBGroupSubscription);
      }
    });
  }

  sort(columnName: string, sortBy: string) {
    this.sortBy = sortBy === 'asc' ? 'desc' : 'asc';
    this.orderBy = columnName;

    this.loadClientLOBGroups();
  }

  setClientLOBGroup(clientLOBGroup: number) {
    this.clientLOBGroupId = clientLOBGroup;
  }

  searchClientLOBGroups() {
    this.loadClientLOBGroups();
  }

  private showSuccessPopUpMessage(contentMessage: string, needRefresh = true) {
    this.getModalPopup(MessagePopUpComponent, 'sm');
    this.setComponentMessages('Success', contentMessage);

    if (needRefresh) {
      this.modalRef.result.then(() => {
        this.loadClientLOBGroups();
      });
    }
  }

  private getModalPopup(component: any, size: string) {
    const options: NgbModalOptions = { backdrop: 'static', centered: true, size };
    this.modalRef = this.modalService.open(component, options);
  }

  private setComponentValues(operation: ComponentOperation, translationValues: Array<TranslationDetails>) {
    this.modalRef.componentInstance.operation = operation;
    this.modalRef.componentInstance.translationValues = translationValues;
  }

  private setComponentMessages(headingMessage: string, contentMessage: string) {
    this.modalRef.componentInstance.headingMessage = headingMessage;
    this.modalRef.componentInstance.contentMessage = contentMessage;
  }

  private getQueryParams() {
    const clientLOBGroupQueryParams = new ClientLobGroupQueryParameters();
    clientLOBGroupQueryParams.clientId = this.clientId;
    clientLOBGroupQueryParams.pageNumber = this.currentPage;
    clientLOBGroupQueryParams.pageSize = this.pageSize;
    clientLOBGroupQueryParams.searchKeyword = this.searchKeyword ?? '';
    clientLOBGroupQueryParams.orderBy = `${this.orderBy} ${this.sortBy}`;
    clientLOBGroupQueryParams.fields = '';

    return clientLOBGroupQueryParams;
  }

  private loadClientLOBGroups() {
    const queryParams = this.getQueryParams();
    this.spinnerService.show(this.spinner, SpinnerOptions);
    this.getAllClientLOBGroupDetailsSubscription = this.clientLOBGroupService.getClientLOBGroups(queryParams)
      .subscribe((response) => {
        this.spinnerService.hide(this.spinner);
        if (response.body) {
          this.clientLOBGroupDetails = response.body;
          this.headerPaginationValues = JSON.parse(response.headers.get('x-pagination'));
          this.totalClientLOBGroupRecord = this.headerPaginationValues.totalCount;
        }
      }, (error) => {
        this.spinnerService.hide(this.spinner);
        console.log(error);
      });

    this.subscriptionList.push(this.getAllClientLOBGroupDetailsSubscription);
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
