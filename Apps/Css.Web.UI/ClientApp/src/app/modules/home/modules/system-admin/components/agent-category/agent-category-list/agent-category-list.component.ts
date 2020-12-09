import { Component, OnDestroy, OnInit } from '@angular/core';
import { NgbModal, NgbModalOptions, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { NgxSpinnerService } from 'ngx-spinner';
import { SubscriptionLike as ISubscription } from 'rxjs';
import { ComponentOperation } from 'src/app/shared/enums/component-operation.enum';
import { CssMenu } from 'src/app/shared/enums/css-menu.enum';
import { HeaderPagination } from 'src/app/shared/models/header-pagination.model';
import { TranslationDetails } from 'src/app/shared/models/translation-details.model';
import { ConfirmationPopUpComponent } from 'src/app/shared/popups/confirmation-pop-up/confirmation-pop-up.component';
import { ErrorWarningPopUpComponent } from 'src/app/shared/popups/error-warning-pop-up/error-warning-pop-up.component';
import { MessagePopUpComponent } from 'src/app/shared/popups/message-pop-up/message-pop-up.component';
import { GenericStateManagerService } from 'src/app/shared/services/generic-state-manager.service';
import { LanguageTranslationService } from 'src/app/shared/services/language-translation.service';
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
  getTranslationValuesSubscription: ISubscription;
  getAllAgentcategorySubscription: ISubscription;
  deleteAgentCategorySubscription: ISubscription;
  subscriptionList: ISubscription[] = [];

  constructor(
    private modalService: NgbModal,
    private spinnerService: NgxSpinnerService,
    private agentCategoryService: AgentCategoryService,
    private translationService: LanguageTranslationService,
    private genericStateManagerService: GenericStateManagerService
  ) { }

  ngOnInit(): void {
    this.loadTranslationValues();
    this.loadAgentcategories();
    this.subscribeToUserLanguage();
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

  private loadTranslationValues() {
    const languageId = this.genericStateManagerService.getCurrentLanguage()?.id;
    const menuId = CssMenu.AgentCategories;

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
