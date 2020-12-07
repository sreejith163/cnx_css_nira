import { Component, OnDestroy, OnInit } from '@angular/core';
import { NgbModal, NgbModalOptions, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { NgxSpinnerService } from 'ngx-spinner';
import { SubscriptionLike as ISubscription } from 'rxjs';

import { SchedulingCodeService } from '../../../services/scheduling-code.service';
import { LanguageTranslationService } from 'src/app/shared/services/language-translation.service';
import { GenericStateManagerService } from 'src/app/shared/services/generic-state-manager.service';

import { SchedulingCode } from '../../../models/scheduling-code.model';
import { HeaderPagination } from 'src/app/shared/models/header-pagination.model';
import { QueryStringParameters } from 'src/app/shared/models/query-string-parameters.model';

import { ConfirmationPopUpComponent } from 'src/app/shared/popups/confirmation-pop-up/confirmation-pop-up.component';
import { MessagePopUpComponent } from 'src/app/shared/popups/message-pop-up/message-pop-up.component';
import { AddUpdateSchedulingCodeComponent } from '../add-update-scheduling-code/add-update-scheduling-code.component';

import { Constants } from 'src/app/shared/util/constants.util';
import { SpinnerOptions } from 'src/app/shared/util/spinner-options.util';
import { ComponentOperation } from 'src/app/shared/enums/component-operation.enum';
import { CssMenu } from 'src/app/shared/enums/css-menu.enum';
import { TranslationDetails } from 'src/app/shared/models/translation-details.model';

@Component({
  selector: 'app-scheduling-code-list',
  templateUrl: './scheduling-code-list.component.html',
  styleUrls: ['./scheduling-code-list.component.scss']
})
export class SchedulingCodeListComponent implements OnInit, OnDestroy {

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
  modalRef: NgbModalRef;
  headerPaginationValues: HeaderPagination;
  translationValues: TranslationDetails[];
  schedulingCodes: SchedulingCode[] = [];

  languageSelectionSubscription: ISubscription;
  getTranslationValuesSubscription: ISubscription;
  deleteSchedulingCodeSubscription: ISubscription;
  getSchedulingCodesSubscription: ISubscription;
  subscriptionList: ISubscription[] = [];

  constructor(
    private modalService: NgbModal,
    private schedulingCodeService: SchedulingCodeService,
    private spinnerService: NgxSpinnerService,
    private translationService: LanguageTranslationService,
    private genericStateManagerService: GenericStateManagerService
  ) { }

  ngOnInit(): void {
    this.loadTranslationValues();
    this.loadSchedulingCodes();
    this.subscribeToUserLanguage();
  }

  ngOnDestroy() {
    this.subscriptionList.forEach(subscription => {
      if (subscription) {
        subscription.unsubscribe();
      }
    });
  }

  unifiedToNative(unified: string) {
    const codePoints = unified.split('-').map(u => parseInt(`0x${u}`, 16));
    return String.fromCodePoint(...codePoints);
  }

  clearSearchKeyword() {
    this.searchKeyword = undefined;
  }

  addSchedulingCode() {
    this.getModalPopup(AddUpdateSchedulingCodeComponent, 'lg');
    this.setComponentValues(ComponentOperation.Add, this.translationValues);

    this.modalRef.result.then(() => {
      this.loadSchedulingCodes();
    });
  }

  editSchedulingCode(schedulingCodeData: SchedulingCode) {
    this.getModalPopup(AddUpdateSchedulingCodeComponent, 'lg');
    this.setComponentValues(ComponentOperation.Edit, this.translationValues);
    this.modalRef.componentInstance.schedulingCodeData = schedulingCodeData;

    this.modalRef.result.then((result) => {
      if (result.needRefresh) {
        this.loadSchedulingCodes();
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
            this.loadSchedulingCodes();
            this.getModalPopup(MessagePopUpComponent, 'sm');
            this.setComponentMessages('Success', 'The record has been deleted!');
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

  private getModalPopup(component: any, size: string) {
    const options: NgbModalOptions = { backdrop: false, centered: true, size };
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
    const queryParams = new QueryStringParameters();
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

  private loadTranslationValues() {
    const languageId = this.genericStateManagerService.getCurrentLanguage()?.id;
    const menuId = CssMenu.SchedulingCodes;

    this.getTranslationValuesSubscription = this.translationService.getMenuTranslations(languageId, menuId)
      .subscribe((response) => {
        if (response) {
          this.translationValues = response;
        }
      }, (error) => {
        console.log(error);
      });

    this.subscriptionList.push(this.getTranslationValuesSubscription);
  }

  private subscribeToUserLanguage() {
    this.languageSelectionSubscription = this.genericStateManagerService.userLanguageChanged.subscribe(
      (languageId: number) => {
        if (languageId) {
          this.loadTranslationValues();
        }
      }
    );

    this.subscriptionList.push(this.languageSelectionSubscription);
  }
}
