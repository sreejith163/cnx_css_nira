import { animate, state, style, transition, trigger } from '@angular/animations';
import { WeekDay } from '@angular/common';
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
import { AgentAdminDetails } from '../../../models/agent-admin-details.model';
import { AgentAdminQueryParameter } from '../../../models/agent-admin-query-parameter.model';
import { AgentAdminResponse } from '../../../models/agent-admin-response.model';
import { AgentAdminService } from '../../../services/agent-admin.service';
import { AddAgentProfileComponent } from '../add-agent-profile/add-agent-profile.component';

@Component({
  selector: 'app-agent-admin-list',
  animations: [
    trigger(
      'enterAnimation', [
      state('true', style({ opacity: 1, height: 'auto' })),
      state('void', style({ opacity: 0, height: 0 })),
      transition(':enter', animate('400ms ease-in-out')),
      transition(':leave', animate('400ms ease-in-out'))
    ]
    )
  ],
  templateUrl: './agent-admin-list.component.html',
  styleUrls: ['./agent-admin-list.component.scss'],

})
export class AgentAdminListComponent implements OnInit, OnDestroy {

  currentPage = 1;
  pageSize = 10;
  characterSplice = 25;
  paginationSize = Constants.paginationSize;
  maxLength = Constants.DefaultTextMaxLength;
  orderBy = 'createdDate';
  sortBy = 'desc';
  spinner = 'agentAdmins';

  clientId: number;
  clientLobGroupId: number;
  skillGroupId: number;
  skillTagId: number;
  totalAgentAdminsRecord: number;
  searchKeyword: string;
  weekDay = WeekDay;

  modalRef: NgbModalRef;
  headerPaginationValues: HeaderPagination;
  agentAdmin: AgentAdminResponse;
  agentAdmins: AgentAdminDetails[] = [];
  translationValues: TranslationDetails[] = [];

  languageSelectionSubscription: ISubscription;
  getTranslationValuesSubscription: ISubscription;
  getAgentAdminsSubscription: ISubscription;
  getAgentAdminSubscription: ISubscription;
  deleteAgentAdminSubscription: ISubscription;
  subscriptions: ISubscription[] = [];

  constructor(
    private agentAdminService: AgentAdminService,
    private modalService: NgbModal,
    private spinnerService: NgxSpinnerService,
    private translationService: LanguageTranslationService,
    private genericStateManagerService: GenericStateManagerService
  ) { }

  ngOnInit() {
    this.loadTranslationValues();
    this.loadAgentAdmins();
    this.subscribeToUserLanguage();
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
    this.loadAgentAdmins();
  }

  changePage(page: number) {
    this.currentPage = page;
    this.loadAgentAdmins();
  }

  addAgentAdmin() {
    this.getModalPopup(AddAgentProfileComponent, 'lg');
    this.setComponentValues(ComponentOperation.Add, this.translationValues);

    this.modalRef.result.then(() => {
      this.currentPage = 1;
      this.loadAgentAdmins();
    });
  }

  editAgentAdmin(agentAdminId: number) {
    this.getModalPopup(AddAgentProfileComponent, 'lg');
    this.setComponentValues(ComponentOperation.Edit, this.translationValues);
    this.modalRef.componentInstance.agentAdminId = agentAdminId;

    this.modalRef.result.then((result: any) => {
      if (result.needRefresh) {
        this.agentAdmin = undefined;
        this.loadAgentAdmins();
      }
    });
  }

  deleteAgentAdmin(agentAdminIndex: number) {
    this.getModalPopup(ConfirmationPopUpComponent, 'sm');
    this.setComponentMessages('Are you sure?', 'You won’t be able to revert this!');
    this.modalRef.componentInstance.deleteRecordIndex = agentAdminIndex;

    this.modalRef.result.then((result) => {
      if (result && result === agentAdminIndex) {

        this.spinnerService.show(this.spinner, SpinnerOptions);
        this.deleteAgentAdminSubscription = this.agentAdminService.deleteAgentAdmin(agentAdminIndex)
          .subscribe(() => {
            this.spinnerService.hide(this.spinner);
            this.loadAgentAdmins();
            this.getModalPopup(MessagePopUpComponent, 'sm');
            this.setComponentMessages('Success', 'The record has been deleted!');
          }, (error) => {
            this.spinnerService.hide(this.spinner);
            if (error.status === 424) {
              this.getModalPopup(ErrorWarningPopUpComponent, 'sm');
              this.setComponentMessages('Error', error.error);
            }
          });

        this.subscriptions.push(this.deleteAgentAdminSubscription);
      }
    });
  }

  search() {
    this.loadAgentAdmins();
  }

  sort(columnName: string, sortBy: string) {
    this.sortBy = sortBy === 'asc' ? 'desc' : 'asc';
    this.orderBy = columnName;

    this.loadAgentAdmins();
  }

  // toggleDetails(agentAdminId: number) {
  //   if (this.agentAdmin?.id === agentAdminId) {
  //     this.agentAdmin = undefined;
  //   } else {
  //     this.getExpandedDetails(agentAdminId);
  //   }
  // }

  getWeekDay(weekDay: number) {
    return WeekDay[weekDay];
  }

  getOpenType(openTypeId) {
    const agentAdmin = Constants.OperationHourTypes.find(x => x.id === openTypeId);
    return agentAdmin.open;
  }

  setClient(client: number) {
    this.clientId = client;
    this.loadAgentAdmins();
  }

  setClientLobgroup(clientLobGroupId: number) {
    this.clientLobGroupId = clientLobGroupId;
    this.loadAgentAdmins();
  }

  setSkillGroup(skillGroupId: number) {
    this.skillGroupId = skillGroupId;
    this.loadAgentAdmins();
  }

  setSkillTag(skillTagId: number) {
    this.skillTagId = skillTagId;
    this.loadAgentAdmins();
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
    const agentAdminQueryParams = new AgentAdminQueryParameter();
    agentAdminQueryParams.pageNumber = this.currentPage;
    agentAdminQueryParams.pageSize = this.pageSize;
    agentAdminQueryParams.searchKeyword = this.searchKeyword ?? '';
    agentAdminQueryParams.orderBy = `${this.orderBy} ${this.sortBy}`;
    agentAdminQueryParams.fields = '';

    return agentAdminQueryParams;
  }

  private loadAgentAdmins() {
    const queryParams = this.getQueryParams();
    this.spinnerService.show(this.spinner, SpinnerOptions);

    this.getAgentAdminsSubscription = this.agentAdminService.getAgentAdmins(queryParams)
      .subscribe((response) => {
        if (response.body) {
          this.agentAdmins = response.body;
          this.headerPaginationValues = JSON.parse(response.headers.get('x-pagination'));
          this.totalAgentAdminsRecord = this.headerPaginationValues.totalCount;
        }
        this.spinnerService.hide(this.spinner);
      }, (error) => {
        this.spinnerService.hide(this.spinner);
        console.log(error);
      });

    this.subscriptions.push(this.getAgentAdminsSubscription);
  }

  private getExpandedDetails(agentAdminId: number) {
    this.spinnerService.show(this.spinner, SpinnerOptions);

    this.getAgentAdminSubscription = this.agentAdminService.getAgentAdmin(agentAdminId)
      .subscribe((response) => {
        if (response) {
          this.agentAdmin = response;
        }
        this.spinnerService.hide(this.spinner);
      }, (error) => {
        this.spinnerService.hide(this.spinner);
        console.log(error);
      });

    this.subscriptions.push(this.getAgentAdminSubscription);
  }

  private loadTranslationValues() {
    const languageId = this.genericStateManagerService.getCurrentLanguage()?.id;
    const menuId = CssMenu.AgentAdmin;

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

  private subscribeToUserLanguage() {
    this.languageSelectionSubscription = this.genericStateManagerService.userLanguageChanged.subscribe(
      (languageId: number) => {
        if (languageId) {
          this.loadTranslationValues();
        }
      }
    );

    this.subscriptions.push(this.languageSelectionSubscription);
  }
}
