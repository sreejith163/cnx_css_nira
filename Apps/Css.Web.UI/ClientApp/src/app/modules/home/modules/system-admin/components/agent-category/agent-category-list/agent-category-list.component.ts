import { Component, OnDestroy, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NgbModal, NgbModalOptions, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { TranslateService } from '@ngx-translate/core';
import { NgxSpinnerService } from 'ngx-spinner';
import { SubscriptionLike as ISubscription } from 'rxjs';
import { AuthService } from 'src/app/core/services/auth.service';
import { ComponentOperation } from 'src/app/shared/enums/component-operation.enum';
import { CssMenu } from 'src/app/shared/enums/css-menu.enum';
import { HeaderPagination } from 'src/app/shared/models/header-pagination.model';
import { LanguagePreference } from 'src/app/shared/models/language-preference.model';
import { TranslationDetails } from 'src/app/shared/models/translation-details.model';
import { ConfirmationPopUpComponent } from 'src/app/shared/popups/confirmation-pop-up/confirmation-pop-up.component';
import { ErrorWarningPopUpComponent } from 'src/app/shared/popups/error-warning-pop-up/error-warning-pop-up.component';
import { MessagePopUpComponent } from 'src/app/shared/popups/message-pop-up/message-pop-up.component';
import { LanguagePreferenceService } from 'src/app/shared/services/language-preference.service';
import { Constants } from 'src/app/shared/util/constants.util';
import { SpinnerOptions } from 'src/app/shared/util/spinner-options.util';
import { DataType } from '../../../enum/data-type.enum';
import { AgentCategoryDetails } from '../../../models/agent-category-details.model';
import { AgentCategoryQueryParams } from '../../../models/agent-category-query-params.model';
import { AgentCategoryService } from '../../../services/agent-category.service';
import { AddAgentCategoryComponent } from '../add-agent-category/add-agent-category.component';

@Component({
  selector: 'app-agent-category-list',
  templateUrl: './agent-category-list.component.html',
  styleUrls: ['./agent-category-list.component.scss']
})
export class AgentCategoryListComponent implements OnInit, OnDestroy {
  currentLanguage: string;
  LoggedUser;

  currentPage = 1;
  pageSize = 10;
  characterSplice = 25;
  dataType = DataType;
  totalAgentCategoryRecord: number;
  maxLength = Constants.DefaultTextMaxLength;
  paginationSize = Constants.paginationSize;

  searchKeyword: string;
  orderBy = 'createdDate';
  sortBy = 'desc';
  spinner = 'tableSpinner';

  modalRef: NgbModalRef;
  headerPaginationValues: HeaderPagination;
  translationValues: TranslationDetails[];
  agentCategoryDetails: AgentCategoryDetails[] = [];

  languageSelectionSubscription: ISubscription;
  getTranslationSubscription: ISubscription;
  getAllAgentcategorySubscription: ISubscription;
  deleteAgentCategorySubscription: ISubscription;
  subscriptionList: ISubscription[] = [];

  constructor(
    public translate: TranslateService,
    private modalService: NgbModal,
    private spinnerService: NgxSpinnerService,
    private agentCategoryService: AgentCategoryService,
    private authService: AuthService,
    private route: ActivatedRoute,
    private languagePreferenceService: LanguagePreferenceService
  ) {
    this.LoggedUser = this.authService.getLoggedUserInfo();
  }

  ngOnInit(): void {
    this.preLoadTranslations();
    this.loadTranslations();
    this.loadAgentcategories();
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
    this.loadAgentcategories();
  }

  changePageSize(pageSize: number) {
    this.pageSize = pageSize;
    this.loadAgentcategories();
  }

  changePage(page: number) {
    this.currentPage = page;
    this.loadAgentcategories();
  }

  addAgentCategory() {
    this.getModalPopup(AddAgentCategoryComponent, 'lg');
    this.setComponentValues(ComponentOperation.Add, this.translationValues);

    this.modalRef.result.then(() => {
      this.currentPage = 1;
      this.showSuccessPopUpMessage('The record has been added!');
    });
  }

  editAgentCategory(data: AgentCategoryDetails) {
    this.getModalPopup(AddAgentCategoryComponent, 'lg');
    this.setComponentValues(ComponentOperation.Edit, this.translationValues);
    this.modalRef.componentInstance.agentCategoryId = data.id;

    this.modalRef.result.then((result: any) => {
      if (result.needRefresh) {
        this.showSuccessPopUpMessage('The record has been updated!');
      } else {
        this.showSuccessPopUpMessage('No changes has been made!', false);
      }
    });
  }

  deleteAgentCategory(agentCategoryIndex: number) {
    this.getModalPopup(ConfirmationPopUpComponent, 'sm');
    this.setComponentMessages('Are you sure?', 'You won’t be able to revert this!');
    this.modalRef.componentInstance.deleteRecordIndex = agentCategoryIndex;

    this.modalRef.result.then((result) => {
      if (result && result === agentCategoryIndex) {

        this.spinnerService.show(this.spinner, SpinnerOptions);
        this.deleteAgentCategorySubscription = this.agentCategoryService.deleteAgentCategory(agentCategoryIndex)
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

        this.subscriptionList.push(this.deleteAgentCategorySubscription);
      }
    });
  }

  searchAgentCategories() {
    this.loadAgentcategories();
  }

  sortAgentCategories(columnName: string, sortBy: string) {
    this.sortBy = sortBy === 'asc' ? 'desc' : 'asc';
    this.orderBy = columnName;

    this.loadAgentcategories();
  }

  clearSearchKeyword() {
    this.searchKeyword = undefined;
  }

  private showSuccessPopUpMessage(contentMessage: string, needRefresh = true) {
    this.getModalPopup(MessagePopUpComponent, 'sm');
    this.setComponentMessages('Success', contentMessage);

    if (needRefresh) {
      this.modalRef.result.then(() => {
        this.loadAgentcategories();
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
    const clientQueryParams = new AgentCategoryQueryParams();
    clientQueryParams.pageNumber = this.currentPage;
    clientQueryParams.pageSize = this.pageSize;
    clientQueryParams.searchKeyword = this.searchKeyword ?? '';
    clientQueryParams.orderBy = `${this.orderBy} ${this.sortBy}`;
    clientQueryParams.fields = '';

    return clientQueryParams;
  }

  private loadAgentcategories() {
    const queryParams = this.getQueryParams();
    this.spinnerService.show(this.spinner, SpinnerOptions);

    this.getAllAgentcategorySubscription = this.agentCategoryService.getAgentcategories(queryParams)
      .subscribe((response) => {
        if (response.body) {
          this.agentCategoryDetails = response.body;
          this.headerPaginationValues = JSON.parse(response.headers.get('x-pagination'));
          this.totalAgentCategoryRecord = this.headerPaginationValues.totalCount;
        }

        this.spinnerService.hide(this.spinner);
      }, (error) => {
        this.spinnerService.hide(this.spinner);
        console.log(error);
      });

    this.subscriptionList.push(this.getAllAgentcategorySubscription);
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
