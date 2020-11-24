
import { Component, OnDestroy, OnInit } from '@angular/core';
import { NgbModal, NgbModalOptions, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { NgxSpinnerService } from 'ngx-spinner';
import { SubscriptionLike as ISubscription } from 'rxjs';

import { ConfirmationPopUpComponent } from 'src/app/shared/popups/confirmation-pop-up/confirmation-pop-up.component';
import { MessagePopUpComponent } from 'src/app/shared/popups/message-pop-up/message-pop-up.component';

import { Translation } from 'src/app/shared/models/translation.model';
import { HeaderPagination } from 'src/app/shared/models/header-pagination.model';
import { ComponentOperation } from 'src/app/shared/enums/component-operation.enum';

import { Constants } from 'src/app/shared/util/constants.util';
import { SpinnerOptions } from 'src/app/shared/util/spinner-options.util';
import { WeekDay } from '@angular/common';
import { animate, state, style, transition, trigger } from '@angular/animations';
import { SkillGroupDetails } from '../../../models/skill-group-details.model';
import { SkillGroupService } from '../../../services/skill-group.service';
import { SkillGroupQueryParameters } from '../../../models/skill-group-query-parameters.model';
import { AddEditSkillGroupComponent } from '../add-edit-skill-group/add-edit-skill-group.component';
import { ErrorWarningPopUpComponent } from 'src/app/shared/popups/error-warning-pop-up/error-warning-pop-up.component';

@Component({
  selector: 'app-skill-group-list',
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
  templateUrl: './skill-group-list.component.html',
  styleUrls: ['./skill-group-list.component.scss']
})

export class SkillGroupListComponent implements OnInit, OnDestroy {

  currentPage = 1;
  pageSize = 10;
  characterSplice = 25;
  paginationSize = Constants.paginationSize;
  orderBy = 'CreatedDate';
  sortBy = 'desc';
  spinner = 'skillGroups';

  totalSkillGroupRecord: number;
  clientId: number;
  clientLobGroupId?: number;
  skillGroupId: number;
  searchKeyword: string;
  weekDay = WeekDay;

  modalRef: NgbModalRef;
  headerPaginationValues: HeaderPagination;
  translationValues = Constants.skillGroupsTranslationValues;
  skillGroupDetails: SkillGroupDetails[] = [];

  getAllSkillGroupDetailsSubscription: ISubscription;
  getSkillGroupSubscription: ISubscription;
  deleteSkillGroupSubscription: ISubscription;
  subscriptions: ISubscription[] = [];

  constructor(
    private modalService: NgbModal,
    private spinnerService: NgxSpinnerService,
    private skillGroupService: SkillGroupService
  ) { }

  ngOnInit(): void {
    this.loadSkillGroups();
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
    this.loadSkillGroups();
  }

  changePage(page: number) {
    this.currentPage = page;
    this.loadSkillGroups();
  }

  addSkillGroup() {
    this.getModalPopup(AddEditSkillGroupComponent, 'lg');
    this.setComponentValues(ComponentOperation.Add, this.translationValues);

    this.modalRef.result.then(() => {
      this.loadSkillGroups();
    });
  }

  editSkillGroup(skillGroupId: number) {
    this.getModalPopup(AddEditSkillGroupComponent, 'lg');
    this.setComponentValues(ComponentOperation.Edit, this.translationValues);
    this.modalRef.componentInstance.skillGroupId = skillGroupId;

    this.modalRef.result.then((result: any) => {
      if (result.needRefresh) {
        this.loadSkillGroups();
      }
    });
  }

  deleteSkillGroup(skillGroupIndex: number) {
    this.getModalPopup(ConfirmationPopUpComponent, 'sm');
    this.setComponentMessages('Are you sure?', 'You won’t be able to revert this!');
    this.modalRef.componentInstance.deleteRecordIndex = skillGroupIndex;

    this.modalRef.result.then((result) => {
      if (result && result === skillGroupIndex) {

        this.spinnerService.show(this.spinner, SpinnerOptions);
        this.deleteSkillGroupSubscription = this.skillGroupService.deleteSkillGroup(skillGroupIndex)
          .subscribe(() => {
            this.spinnerService.hide(this.spinner);
            this.loadSkillGroups();
            this.getModalPopup(MessagePopUpComponent, 'sm');
            this.setComponentMessages('Success', 'The record has been deleted!');
          }, (error) => {
            this.spinnerService.hide(this.spinner);
            if (error.status === 424) {
              this.getModalPopup(ErrorWarningPopUpComponent, 'sm');
              this.setComponentMessages('Error', error.error);
            }
          });

        this.subscriptions.push(this.deleteSkillGroupSubscription);
      }
    });
  }

  search() {
    this.loadSkillGroups();
  }

  sort(columnName: string, sortBy: string) {
    this.sortBy = sortBy === 'asc' ? 'desc' : 'asc';
    this.orderBy = columnName;

    this.loadSkillGroups();
  }

  setSkillGroup(skillGroup: number) {
    this.skillGroupId = skillGroup;
  }

  searchSkillGroups() {
    this.loadSkillGroups();
  }

  getWeekDay(weekDay: number) {
    return WeekDay[weekDay];
  }

  getOpenType(openTypeId) {
    const skillTag = Constants.operationHours.find(x => x.id === openTypeId);
    return skillTag.open;
  }

  setClient(client: number) {
    this.clientId = client;
    this.loadSkillGroups();
  }

  setClientLob(clientLob: number) {
    this.clientLobGroupId = clientLob;
    this.loadSkillGroups();
  }

  private getModalPopup(component: any, size: string) {
    const options: NgbModalOptions = { backdrop: false, centered: true, size };
    this.modalRef = this.modalService.open(component, options);
  }

  private setComponentValues(operation: ComponentOperation, translationValues: Array<Translation>) {
    this.modalRef.componentInstance.operation = operation;
    this.modalRef.componentInstance.translationValues = translationValues;
  }

  private setComponentMessages(headingMessage: string, contentMessage: string) {
    this.modalRef.componentInstance.headingMessage = headingMessage;
    this.modalRef.componentInstance.contentMessage = contentMessage;
  }

  private getQueryParams() {
    const skillGroupQueryParams = new SkillGroupQueryParameters();
    skillGroupQueryParams.clientId = this.clientId;
    skillGroupQueryParams.clientLobGroupId = this.clientLobGroupId;
    skillGroupQueryParams.pageNumber = this.currentPage;
    skillGroupQueryParams.pageSize = this.pageSize;
    skillGroupQueryParams.searchKeyword = this.searchKeyword ?? '';
    skillGroupQueryParams.orderBy = `${this.orderBy} ${this.sortBy}`;
    skillGroupQueryParams.fields = '';

    return skillGroupQueryParams;
  }

  private loadSkillGroups() {
    const queryParams = this.getQueryParams();
    this.spinnerService.show(this.spinner, SpinnerOptions);
    this.getAllSkillGroupDetailsSubscription = this.skillGroupService.getSkillGroups(queryParams)
      .subscribe((response) => {
        if (response.body) {
          this.skillGroupDetails = response.body;
          this.headerPaginationValues = JSON.parse(response.headers.get('x-pagination'));
          this.totalSkillGroupRecord = this.headerPaginationValues.totalCount;
        }
        this.spinnerService.hide(this.spinner);
      }, (error) => {
        this.spinnerService.hide(this.spinner);
        console.log(error);
      });

    this.subscriptions.push(this.getAllSkillGroupDetailsSubscription);
  }
}
