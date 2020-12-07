import { Component, OnDestroy, OnInit } from '@angular/core';
import { NgbModal, NgbModalOptions, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { SubscriptionLike as ISubscription } from 'rxjs';
import { NgxSpinnerService } from 'ngx-spinner';

import { AddUpdateTranslationsComponent } from '../add-update-translations/add-update-translations.component';
import { ConfirmationPopUpComponent } from 'src/app/shared/popups/confirmation-pop-up/confirmation-pop-up.component';
import { MessagePopUpComponent } from 'src/app/shared/popups/message-pop-up/message-pop-up.component';
import { ErrorWarningPopUpComponent } from 'src/app/shared/popups/error-warning-pop-up/error-warning-pop-up.component';

import { LanguageTranslationService } from 'src/app/shared/services/language-translation.service';

import { Constants } from 'src/app/shared/util/constants.util';
import { SpinnerOptions } from 'src/app/shared/util/spinner-options.util';
import { HeaderPagination } from 'src/app/shared/models/header-pagination.model';
import { ComponentOperation } from 'src/app/shared/enums/component-operation.enum';
import { TranslationDetails } from 'src/app/shared/models/translation-details.model';
import { TranslationQueryParams } from 'src/app/shared/models/translation-query-params.model';
import { CssLanguage } from 'src/app/shared/enums/css-language.enum';
import { CssMenu } from 'src/app/shared/enums/css-menu.enum';




@Component({
  selector: 'app-translation-list',
  templateUrl: './translation-list.component.html',
  styleUrls: ['./translation-list.component.scss']
})
export class TranslationListComponent implements OnInit, OnDestroy {

  currentPage = 1;
  pageSize = 10;
  characterSplice = 25;
  totalRecord: number;
  searchKeyword: string;
  orderBy = 'createdDate';
  sortBy = 'desc';
  spinner = 'translations';

  modalRef: NgbModalRef;
  maxLength = Constants.DefaultTextMaxLength;
  headerPaginationValues: HeaderPagination;
  paginationSize = Constants.paginationSize;
  translations: TranslationDetails[] = [];
  translationValues: TranslationDetails[] = [];

  getTranslationValuesSubscription: ISubscription;
  getTranslatiosSubscription: ISubscription;
  deleteTranslationSubscription: ISubscription;
  subscriptions: ISubscription[] = [];

  constructor(
    private modalService: NgbModal,
    private translationService: LanguageTranslationService,
    private spinnerService: NgxSpinnerService,
  ) { }

  ngOnInit(): void {
    this.loadTranslationValues();
    this.loadTranslations();
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
    this.loadTranslations();
  }

  changePage(page: number) {
    this.currentPage = page;
    this.loadTranslations();
  }

  search() {
    this.loadTranslations();
  }

  sort(columnName: string, sortBy: string) {
    this.sortBy = sortBy === 'asc' ? 'desc' : 'asc';
    this.orderBy = columnName;

    this.loadTranslations();
  }

  addTranslation() {
    this.getModalPopup(AddUpdateTranslationsComponent, 'lg');
    this.setComponentValues(ComponentOperation.Add, this.translationValues);

    this.modalRef.result.then(() => {
      this.currentPage = 1;
      this.loadTranslations();
    });
  }

  editTranslation(translationId: number) {
    this.getModalPopup(AddUpdateTranslationsComponent, 'lg');
    this.setComponentValues(ComponentOperation.Edit, this.translationValues);
    this.modalRef.componentInstance.translationId = translationId;

    this.modalRef.result.then((result: any) => {
      if (result.needRefresh) {
        this.loadTranslations();
      }
    });
  }

  deleteTranslation(translationIndex: number) {
    this.getModalPopup(ConfirmationPopUpComponent, 'sm');
    this.setComponentMessages('Are you sure?', 'You won’t be able to revert this!');
    this.modalRef.componentInstance.deleteRecordIndex = translationIndex;

    this.modalRef.result.then((result) => {
      if (result && result === translationIndex) {

        this.spinnerService.show(this.spinner, SpinnerOptions);
        this.deleteTranslationSubscription = this.translationService.deleteLanguageTranslation(translationIndex)
          .subscribe(() => {
            this.spinnerService.hide(this.spinner);
            this.loadTranslations();
            this.getModalPopup(MessagePopUpComponent, 'sm');
            this.setComponentMessages('Success', 'The record has been deleted!');
          }, (error) => {
            this.spinnerService.hide(this.spinner);
            if (error.status === 424) {
              this.getModalPopup(ErrorWarningPopUpComponent, 'sm');
              this.setComponentMessages('Error', error.error);
            }
          });

        this.subscriptions.push(this.deleteTranslationSubscription);
      }
    });
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
    const queryParams = new TranslationQueryParams();
    queryParams.languageId = undefined;
    queryParams.menuId = undefined;
    queryParams.variableId = undefined;
    queryParams.pageNumber = this.currentPage;
    queryParams.pageSize = this.pageSize;
    queryParams.searchKeyword = this.searchKeyword ?? '';
    queryParams.orderBy = `${this.orderBy} ${this.sortBy}`;
    queryParams.fields = '';

    return queryParams;
  }

  private loadTranslations() {
    const queryParams = this.getQueryParams();
    this.spinnerService.show(this.spinner, SpinnerOptions);

    this.getTranslatiosSubscription = this.translationService.getLanguageTranslations(queryParams)
      .subscribe((response) => {
        if (response.body) {
          this.translations = response.body;
          this.headerPaginationValues = JSON.parse(response.headers.get('x-pagination'));
          this.totalRecord = this.headerPaginationValues.totalCount;
        }
        this.spinnerService.hide(this.spinner);
      }, (error) => {
        this.spinnerService.hide(this.spinner);
        console.log(error);
      });

    this.subscriptions.push(this.getTranslatiosSubscription);
  }

  private loadTranslationValues() {
    const languageId = CssLanguage.English;
    const menuId = CssMenu.Translation;

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
