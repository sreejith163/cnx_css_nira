import { Component, OnDestroy, OnInit } from '@angular/core';
import { NgbModal, NgbModalOptions, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { ConfirmationPopUpComponent } from 'src/app/shared/popups/confirmation-pop-up/confirmation-pop-up.component';
import { Constants } from 'src/app/shared/util/constants.util';
import { AgentAdmin } from '../../models/agent-admin.model';
import { AgentAdminListService } from '../../services/agent-admin-list.service';
import { AddAgentProfileComponent } from '../add-agent-profile/add-agent-profile.component';
import { MessagePopUpComponent } from 'src/app/shared/popups/message-pop-up/message-pop-up.component';
import { PaginationSize } from 'src/app/shared/models/pagination-size.model';
import { LanguageTranslationService } from 'src/app/shared/services/language-translation.service';
import { CssMenu } from 'src/app/shared/enums/css-menu.enum';
import { TranslationDetails } from 'src/app/shared/models/translation-details.model';
import { SubscriptionLike as ISubscription } from 'rxjs';
import { GenericStateManagerService } from 'src/app/shared/services/generic-state-manager.service';

@Component({
  selector: 'app-agent-admin-list',
  templateUrl: './agent-admin-list.component.html',
  styleUrls: ['./agent-admin-list.component.scss'],
})
export class AgentAdminListComponent implements OnInit, OnDestroy {
  currentPage = 1;
  pageSize = 10;
  characterSplice = 25;
  totalAgentRecord: number;
  modalRef: NgbModalRef;
  translationValues: TranslationDetails[];
  paginationSize: PaginationSize[] = [];
  totalAgents: AgentAdmin[] = [];

  languageSelectionSubscription: ISubscription;
  getTranslationValuesSubscription: ISubscription;
  subscriptions: ISubscription[] = [];

  constructor(
    private agentAdminListService: AgentAdminListService,
    private modalService: NgbModal,
    private translationService: LanguageTranslationService,
    private genericStateManagerService: GenericStateManagerService
  ) { }

  ngOnInit(): void {
    this.loadTranslationValues();
    this.subscribeToUserLanguage();
    this.totalAgents = this.agentAdminListService.getAgentAdmins().sort(
      (a, b) =>
        new Date(b.createdDate).getTime() - new Date(a.createdDate).getTime()
    );
    this.totalAgentRecord = this.totalAgents.length;
    this.paginationSize = Constants.paginationSize;
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
  }

  changePage(page: number) {
    this.currentPage = page;
  }

  openModal() {
    this.getModalPopup(AddAgentProfileComponent, 'lg');
    this.passPopUpTextValues('Add', this.translationValues);
  }

  edit(agentProfileData: AgentAdmin) {
    this.getModalPopup(AddAgentProfileComponent, 'lg');
    this.passPopUpTextValues('Edit', this.translationValues);
    this.modalRef.componentInstance.agentProfileData = agentProfileData;
  }

  delete(employeeId: number) {
    this.getModalPopup(ConfirmationPopUpComponent, 'sm');
    this.passPopUpMessages('Are you sure?', 'You won’t be able to revert this!');
    this.modalRef.componentInstance.deleteRecordIndex = employeeId;

    this.modalRef.result.then((result) => {
      if (result && result === employeeId) {
        this.agentAdminListService.deleteAgentAdmin(employeeId);
        this.getModalPopup(MessagePopUpComponent, 'sm');
        this.passPopUpMessages('Success', 'The record has been deleted!');
      }
    });
  }

  private getModalPopup(component: any, size: string) {
    const options: NgbModalOptions = { backdrop: 'static', centered: true, size };
    this.modalRef = this.modalService.open(component, options);
  }

  private passPopUpTextValues(title: string, translationValues: Array<TranslationDetails>) {
    this.modalRef.componentInstance.title = title;
    this.modalRef.componentInstance.translationValues = translationValues;
  }

  private passPopUpMessages(headingMessage: string, contentMessage: string) {
    this.modalRef.componentInstance.headingMessage = headingMessage;
    this.modalRef.componentInstance.contentMessage = contentMessage;
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
