import { Component, OnDestroy, OnInit } from '@angular/core';
import { NgbModal, NgbModalOptions, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { NgxSpinnerService } from 'ngx-spinner';
import { SubscriptionLike as ISubscription } from 'rxjs';

import { SchedulingCodeService } from '../../../../../../../shared/services/scheduling-code.service';
import { LanguageTranslationService } from 'src/app/shared/services/language-translation.service';
import { GenericStateManagerService } from 'src/app/shared/services/generic-state-manager.service';

import { SchedulingCode } from '../../../models/scheduling-code.model';
import { HeaderPagination } from 'src/app/shared/models/header-pagination.model';
import { SchedulingCodeQueryParams } from '../../../models/scheduling-code-query-params.model';

import { ConfirmationPopUpComponent } from 'src/app/shared/popups/confirmation-pop-up/confirmation-pop-up.component';
import { MessagePopUpComponent } from 'src/app/shared/popups/message-pop-up/message-pop-up.component';
import { AddUpdateSchedulingCodeComponent } from '../add-update-scheduling-code/add-update-scheduling-code.component';

import { Constants } from 'src/app/shared/util/constants.util';
import { SpinnerOptions } from 'src/app/shared/util/spinner-options.util';
import { ComponentOperation } from 'src/app/shared/enums/component-operation.enum';
import { CssMenu } from 'src/app/shared/enums/css-menu.enum';
import { TranslationDetails } from 'src/app/shared/models/translation-details.model';
import { TranslateService } from '@ngx-translate/core';
import { Language } from 'src/app/shared/models/language-value.model';
import { AuthService } from 'src/app/core/services/auth.service';
import { ActivatedRoute } from '@angular/router';
import { LanguagePreferenceService } from 'src/app/shared/services/language-preference.service';
import { LanguagePreference } from 'src/app/shared/models/language-preference.model';

@Component({
  selector: 'app-scheduling-code-list',
  templateUrl: './scheduling-code-list.component.html',
  styleUrls: ['./scheduling-code-list.component.scss']
})
export class SchedulingCodeListComponent implements OnInit, OnDestroy {
  currentLanguage: string;
  LoggedUser;

  currentPage = 1;
  pageSize = 10;
  characterSplice = 25;
  totalSchedulingCodesRecord: number;

  searchKeyword: string;
  orderBy = 'createdDate';
  sortBy = 'desc';
  spinner = 'schedulingSpinner';

  spinnerOptions = SpinnerOptions;
  paginationSize = Constants.paginationSize;
  maxLength = Constants.DefaultTextMaxLength;
  modalRef: NgbModalRef;
  headerPaginationValues: HeaderPagination;
  schedulingCodes: SchedulingCode[] = [];

  getTranslationSubscription: ISubscription;
  deleteSchedulingCodeSubscription: ISubscription;
  getSchedulingCodesSubscription: ISubscription;
  subscriptionList: ISubscription[] = [];

  constructor(
    public translate: TranslateService,
    private modalService: NgbModal,
    private schedulingCodeService: SchedulingCodeService,
    private spinnerService: NgxSpinnerService,
    private authService: AuthService,
    private route: ActivatedRoute,
    private languagePreferenceService: LanguagePreferenceService
  ) {
    this.LoggedUser = this.authService.getLoggedUserInfo();
  }

  ngOnInit(): void {
    this.loadSchedulingCodes();
    this.subscribeToTranslations();
    this.preLoadTranslations();
    this.loadTranslations();
  }

  ngOnDestroy() {
    this.subscriptionList.forEach(subscription => {
      if (subscription) {
        subscription.unsubscribe();
      }
    });
  }

  getTimeOffCodeValue(timeOffCode: boolean) {
    return timeOffCode ? 'Yes' : 'No';
  }

  unifiedToNative(unified: string) {
    const codePoints = unified.split('-').map(u => parseInt(`0x${u}`, 16));
    return String.fromCodePoint(...codePoints);
  }

  clearSearchText() {
    this.searchKeyword = undefined;
    this.loadSchedulingCodes();
  }

  addSchedulingCode() {
    this.getModalPopup(AddUpdateSchedulingCodeComponent, 'lg');
    this.setComponentValues(ComponentOperation.Add);

    this.modalRef.result.then(() => {
      this.showSuccessPopUpMessage('The record has been added!');
    });
  }

  editSchedulingCode(schedulingCodeData: SchedulingCode) {
    this.getModalPopup(AddUpdateSchedulingCodeComponent, 'lg');
    this.setComponentValues(ComponentOperation.Edit);
    this.modalRef.componentInstance.schedulingCodeData = schedulingCodeData;

    this.modalRef.result.then((result: any) => {
      if (result.needRefresh) {
        this.showSuccessPopUpMessage('The record has been updated!');
      } else {
        this.showSuccessPopUpMessage('No changes has been made!', false);
      }
    });
  }

  deleteSchedulingCode(id: number) {
    this.getModalPopup(ConfirmationPopUpComponent, 'sm');
    this.setComponentMessages('Are you sure?', 'You won’t be able to revert this!');
    this.modalRef.componentInstance.deleteRecordIndex = id;

    this.modalRef.result.then((result) => {
      if (result && result === id) {
        this.spinnerService.show(this.spinner, this.spinnerOptions);

        this.deleteSchedulingCodeSubscription = this.schedulingCodeService.deleteSchedulingCode(id)
          .subscribe(() => {
            this.spinnerService.hide(this.spinner);
            this.showSuccessPopUpMessage('The record has been deleted!');
          }, (error) => {
            this.spinnerService.hide(this.spinner);
            console.log(error);
          });

        this.subscriptionList.push(this.deleteSchedulingCodeSubscription);
      }
    });
  }

  changePageSize(pageSize: number) {
    this.pageSize = pageSize;
    this.loadSchedulingCodes();
  }

  changePage(page: number) {
    this.currentPage = page;
    this.loadSchedulingCodes();
  }

  searchSchedulingCode() {
    this.loadSchedulingCodes();
  }

  sort(columnName: string, sortBy: string) {
    this.sortBy = sortBy === 'asc' ? 'desc' : 'asc';
    this.orderBy = columnName;

    this.loadSchedulingCodes();
  }

  private showSuccessPopUpMessage(contentMessage: string, needRefresh = true) {
    this.getModalPopup(MessagePopUpComponent, 'sm');
    this.setComponentMessages('Success', contentMessage);

    if (needRefresh) {
      this.modalRef.result.then(() => {
        this.loadSchedulingCodes();
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
    const queryParams = new SchedulingCodeQueryParams();
    queryParams.skipPageSize = false;
    queryParams.pageNumber = this.currentPage;
    queryParams.pageSize = this.pageSize;
    queryParams.searchKeyword = this.searchKeyword ?? '';
    queryParams.orderBy = `${this.orderBy} ${this.sortBy}`;
    queryParams.fields = '';

    return queryParams;
  }

  private loadSchedulingCodes() {
    const queryParams = this.getQueryParams();
    this.spinnerService.show(this.spinner, this.spinnerOptions);

    this.getSchedulingCodesSubscription = this.schedulingCodeService.getSchedulingCodes(queryParams)
      .subscribe((response) => {
        if (response.body) {
          this.schedulingCodes = response.body;
          this.headerPaginationValues = JSON.parse(response.headers.get('x-pagination'));
          this.totalSchedulingCodesRecord = this.headerPaginationValues.totalCount;
        }

        this.spinnerService.hide(this.spinner);
      }, (error) => {
        this.spinnerService.hide(this.spinner);
        console.log(error);
      });

    this.subscriptionList.push(this.getSchedulingCodesSubscription);
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
