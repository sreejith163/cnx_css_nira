import { Component, OnDestroy, OnInit } from '@angular/core';
import { NgbModal, NgbModalOptions, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { NgxSpinnerService } from 'ngx-spinner';
import { Constants } from 'src/app/shared/util/constants.util';
import { SpinnerOptions } from 'src/app/shared/util/spinner-options.util';
import { SubscriptionLike as ISubscription } from 'rxjs';
import { HeaderPagination } from 'src/app/shared/models/header-pagination.model';
import { WeekDay } from '@angular/common';
import { ComponentOperation } from 'src/app/shared/enums/component-operation.enum';
import { ConfirmationPopUpComponent } from 'src/app/shared/popups/confirmation-pop-up/confirmation-pop-up.component';
import { MessagePopUpComponent } from 'src/app/shared/popups/message-pop-up/message-pop-up.component';
import { ErrorWarningPopUpComponent } from 'src/app/shared/popups/error-warning-pop-up/error-warning-pop-up.component';
import { animate, state, style, transition, trigger } from '@angular/animations';
import { AgentSchedulingGroupResponse } from '../../../models/agent-scheduling-group-response.model';
import { AgentSchedulingGroupDetails } from '../../../models/agent-scheduling-group-details.model';
import { AddEditAgentSchedulingGroupComponent } from '../add-edit-agent-scheduling-group/add-edit-agent-scheduling-group.component';
import { AgentSchedulingGroupQueryParams } from '../../../models/agent-scheduling-group-query-params.model';
import { CssMenu } from 'src/app/shared/enums/css-menu.enum';
import { LanguageTranslationService } from 'src/app/shared/services/language-translation.service';
import { TranslationDetails } from 'src/app/shared/models/translation-details.model';
import { GenericStateManagerService } from 'src/app/shared/services/generic-state-manager.service';
import { AgentSchedulingGroupService } from 'src/app/shared/services/agent-scheduling-group.service';
import { Language } from 'src/app/shared/models/language-value.model';
import { TranslateService } from '@ngx-translate/core';
import { LanguagePreference } from 'src/app/shared/models/language-preference.model';
import { AuthService } from 'src/app/core/services/auth.service';
import { ActivatedRoute } from '@angular/router';
import { LanguagePreferenceService } from 'src/app/shared/services/language-preference.service';

@Component({
  selector: 'app-agent-scheduling-group-list',
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
  templateUrl: './agent-scheduling-group-list.component.html',
  styleUrls: ['./agent-scheduling-group-list.component.scss']

})
export class AgentSchedulingGroupListComponent implements OnInit, OnDestroy {
  currentLanguage: string;
  LoggedUser;

  currentPage = 1;
  pageSize = 10;
  characterSplice = 25;
  paginationSize = Constants.paginationSize;
  maxLength = Constants.DefaultTextMaxLength;
  orderBy = 'createdDate';
  sortBy = 'desc';
  spinner = 'agentSchedulingGroups';

  clientId: number;
  clientLobGroupId: number;
  skillGroupId: number;
  skillTagId: number;
  totalAgentSchedulingGroupsRecord: number;
  searchKeyword: '';
  dropdownSearchKeyWord = '';
  weekDay = WeekDay;

  modalRef: NgbModalRef;
  headerPaginationValues: HeaderPagination;
  agentSchedulingGroup: AgentSchedulingGroupResponse;
  agentSchedulingGroups: AgentSchedulingGroupDetails[] = [];
  translationValues: TranslationDetails[] = [];

  getTranslationSubscription: ISubscription;
  getAgentSchedulingGroupsSubscription: ISubscription;
  getAgentSchedulingGroupSubscription: ISubscription;
  deleteAgentSchedulingGroupSubscription: ISubscription;
  subscriptions: ISubscription[] = [];

  constructor(
    public translate: TranslateService,
    private agentSchedulingGroupSevice: AgentSchedulingGroupService,
    private modalService: NgbModal,
    private spinnerService: NgxSpinnerService,
    private authService: AuthService,
    private route: ActivatedRoute,
    private languagePreferenceService: LanguagePreferenceService
  ) {
    this.LoggedUser = this.authService.getLoggedUserInfo();
  }

  ngOnInit() {
    this.preLoadTranslations();
    this.loadTranslations();
    this.subscribeToTranslations();
    this.loadAgentSchedulingGroups();
  }

  ngOnDestroy() {
    this.subscriptions.forEach(subscription => {
      if (subscription) {
        subscription.unsubscribe();
      }
    });
  }

  clearSearchText() {
    this.searchKeyword = '';
    this.loadAgentSchedulingGroups();
  }

  changePageSize(pageSize: number) {
    this.pageSize = pageSize;
    this.loadAgentSchedulingGroups();
  }

  changePage(page: number) {
    this.currentPage = page;
    this.loadAgentSchedulingGroups();
  }

  addAgentSchedulingGroup() {
    this.getModalPopup(AddEditAgentSchedulingGroupComponent, 'lg');
    this.setComponentValues(ComponentOperation.Add, this.translationValues);

    this.modalRef.result.then(() => {
      this.currentPage = 1;
      this.showSuccessPopUpMessage('The record has been added!');
    });
  }

  editAgentSchedulingGroup(agentSchedulingGroupId: number) {
    this.getModalPopup(AddEditAgentSchedulingGroupComponent, 'lg');
    this.setComponentValues(ComponentOperation.Edit, this.translationValues);
    this.modalRef.componentInstance.agentSchedulingGroupId = agentSchedulingGroupId;

    this.modalRef.result.then((result: any) => {
      if (result.needRefresh) {
        this.agentSchedulingGroup = undefined;
        this.showSuccessPopUpMessage('The record has been updated!');
      } else {
        this.showSuccessPopUpMessage('No changes has been made!', false);
      }
    });
  }

  deleteAgentSchedulingGroup(agentSchedulingGroupIndex: number) {
    this.getModalPopup(ConfirmationPopUpComponent, 'sm');
    this.setComponentMessages('Are you sure?', 'You won’t be able to revert this!');
    this.modalRef.componentInstance.deleteRecordIndex = agentSchedulingGroupIndex;

    this.modalRef.result.then((result) => {
      if (result && result === agentSchedulingGroupIndex) {

        this.spinnerService.show(this.spinner, SpinnerOptions);
        this.deleteAgentSchedulingGroupSubscription = this.agentSchedulingGroupSevice.deleteAgentSchedulingGroup(agentSchedulingGroupIndex)
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

        this.subscriptions.push(this.deleteAgentSchedulingGroupSubscription);
      }
    });
  }

  search() {
    this.loadAgentSchedulingGroups();
  }

  sort(columnName: string, sortBy: string) {
    this.sortBy = sortBy === 'asc' ? 'desc' : 'asc';
    this.orderBy = columnName;

    this.loadAgentSchedulingGroups();
  }

  toggleDetails(agentSchedulingGroupId: number) {
    if (this.agentSchedulingGroup?.id === agentSchedulingGroupId) {
      this.agentSchedulingGroup = undefined;
    } else {
      this.getExpandedDetails(agentSchedulingGroupId);
    }
  }

  getWeekDay(weekDay: number) {
    return WeekDay[weekDay];
  }

  getOpenType(openTypeId) {
    const agentSchedulingGroup = Constants.OperationHourTypes.find(x => x.id === openTypeId);
    return agentSchedulingGroup.open;
  }

  setClient(client: number) {
    this.clientId = client;
    this.clientLobGroupId = undefined;
    this.skillGroupId = undefined;
    this.skillTagId = undefined;
    this.loadAgentSchedulingGroups();
  }

  setClientLobgroup(clientLobGroupId: number) {
    this.clientLobGroupId = clientLobGroupId;
    this.skillGroupId = undefined;
    this.skillTagId = undefined;
    this.loadAgentSchedulingGroups();
  }

  setSkillGroup(skillGroupId: number) {
    this.skillGroupId = skillGroupId;
    this.skillTagId = undefined;
    this.loadAgentSchedulingGroups();
  }

  setSkillTag(skillTagId: number) {
    this.skillTagId = skillTagId;
    this.loadAgentSchedulingGroups();
  }

  private showSuccessPopUpMessage(contentMessage: string, needRefresh = true) {
    this.getModalPopup(MessagePopUpComponent, 'sm');
    this.setComponentMessages('Success', contentMessage);

    if (needRefresh) {
      this.modalRef.result.then(() => {
        this.loadAgentSchedulingGroups();
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
    const agentSchedulingGroupQueryParams = new AgentSchedulingGroupQueryParams();
    agentSchedulingGroupQueryParams.clientId = this.clientId;
    agentSchedulingGroupQueryParams.clientLobGroupId = this.clientLobGroupId;
    agentSchedulingGroupQueryParams.skillGroupId = this.skillGroupId;
    agentSchedulingGroupQueryParams.skillTagId = this.skillTagId;
    agentSchedulingGroupQueryParams.pageNumber = this.currentPage;
    agentSchedulingGroupQueryParams.pageSize = this.pageSize;
    agentSchedulingGroupQueryParams.searchKeyword = this.searchKeyword ?? this.searchKeyword;
    agentSchedulingGroupQueryParams.orderBy = `${this.orderBy} ${this.sortBy}`;
    agentSchedulingGroupQueryParams.fields = '';

    return agentSchedulingGroupQueryParams;
  }

  private loadAgentSchedulingGroups() {
    const queryParams = this.getQueryParams();
    console.log('queryParams', queryParams);
    this.spinnerService.show(this.spinner, SpinnerOptions);

    this.getAgentSchedulingGroupsSubscription = this.agentSchedulingGroupSevice.getAgentSchedulingGroups(queryParams)
      .subscribe((response) => {
        if (response.body) {
          this.agentSchedulingGroups = response.body;
          this.headerPaginationValues = JSON.parse(response.headers.get('x-pagination'));
          this.totalAgentSchedulingGroupsRecord = this.headerPaginationValues.totalCount;
        }
        this.spinnerService.hide(this.spinner);
      }, (error) => {
        this.spinnerService.hide(this.spinner);
        console.log(error);
      });

    this.subscriptions.push(this.getAgentSchedulingGroupsSubscription);
  }

  private getExpandedDetails(agentSchedulingGroupId: number) {
    this.spinnerService.show(this.spinner, SpinnerOptions);

    this.getAgentSchedulingGroupSubscription = this.agentSchedulingGroupSevice.getAgentSchedulingGroup(agentSchedulingGroupId)
      .subscribe((response) => {
        if (response) {
          this.agentSchedulingGroup = response;
        }
        this.spinnerService.hide(this.spinner);
      }, (error) => {
        this.spinnerService.hide(this.spinner);
        console.log(error);
      });

    this.subscriptions.push(this.getAgentSchedulingGroupSubscription);
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

  private preLoadTranslations(){
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
