import { Component, OnDestroy, OnInit } from '@angular/core';
import { NgbModal, NgbModalOptions, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { NgxSpinnerService } from 'ngx-spinner';
import { Translation } from 'src/app/shared/models/translation.model';
import { Constants } from 'src/app/shared/util/constants.util';
import { SpinnerOptions } from 'src/app/shared/util/spinner-options.util';
import { SkillTagDetails } from '../../../models/skill-tag-details.model';
import { SkillTagService } from '../../../services/skill-tag.service';
import { SubscriptionLike as ISubscription } from 'rxjs';
import { HeaderPagination } from 'src/app/shared/models/header-pagination.model';
import { WeekDay } from '@angular/common';
import { AddUpdateSkillTagComponent } from '../add-update-skill-tag/add-update-skill-tag.component';
import { ComponentOperation } from 'src/app/shared/enums/component-operation.enum';
import { ConfirmationPopUpComponent } from 'src/app/shared/popups/confirmation-pop-up/confirmation-pop-up.component';
import { MessagePopUpComponent } from 'src/app/shared/popups/message-pop-up/message-pop-up.component';
import { ErrorWarningPopUpComponent } from 'src/app/shared/popups/error-warning-pop-up/error-warning-pop-up.component';
import { SkillTagQueryParams } from '../../../models/skill-tag-query-params.model';
import { SkillTagResponse } from '../../../models/skill-tag-response.model';
import { animate, state, style, transition, trigger } from '@angular/animations';

@Component({
  selector: 'app-skill-tags-list',
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
  templateUrl: './skill-tags-list.component.html',
  styleUrls: ['./skill-tags-list.component.scss']
})
export class SkillTagsListComponent implements OnInit, OnDestroy {

  currentPage = 1;
  pageSize = 10;
  characterSplice = 25;
  paginationSize = Constants.paginationSize;
  translationValues = Constants.skillTagsTranslationValues;
  maxLength = Constants.DefaultTextMaxLength;
  orderBy = 'createdDate';
  sortBy = 'desc';
  spinner = 'skillTags';

  clientId: number;
  clientLobGroupId: number;
  skillGroupId: number;
  totalSkillTagsRecord: number;
  searchKeyword: string;

  modalRef: NgbModalRef;
  headerPaginationValues: HeaderPagination;
  skillTag: SkillTagResponse;
  skillTags: SkillTagDetails[] = [];

  getSkillTagsSubscription: ISubscription;
  getSkillTagSubscription: ISubscription;
  deleteSkillTagSubscription: ISubscription;
  subscriptions: ISubscription[] = [];

  constructor(
    private skillTagSevice: SkillTagService,
    private modalService: NgbModal,
    private spinnerService: NgxSpinnerService,
  ) { }

  ngOnInit() {
    this.loadSkillTags();
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
    this.loadSkillTags();
  }

  changePage(page: number) {
    this.currentPage = page;
    this.loadSkillTags();
  }

  addSkillTag() {
    this.getModalPopup(AddUpdateSkillTagComponent, 'lg');
    this.setComponentValues(ComponentOperation.Add, this.translationValues);

    this.modalRef.result.then(() => {
      this.currentPage = 1;
      this.loadSkillTags();
    });
  }

  editSkillTag(skillTagId: number) {
    this.getModalPopup(AddUpdateSkillTagComponent, 'lg');
    this.setComponentValues(ComponentOperation.Edit, this.translationValues);
    this.modalRef.componentInstance.skillTagId = skillTagId;

    this.modalRef.result.then((result: any) => {
      if (result.needRefresh) {
        this.skillTag = undefined;
        this.loadSkillTags();
      }
    });
  }

  deleteSkillTag(skillTagIndex: number) {
    this.getModalPopup(ConfirmationPopUpComponent, 'sm');
    this.setComponentMessages('Are you sure?', 'You won’t be able to revert this!');
    this.modalRef.componentInstance.deleteRecordIndex = skillTagIndex;

    this.modalRef.result.then((result) => {
      if (result && result === skillTagIndex) {

        this.spinnerService.show(this.spinner, SpinnerOptions);
        this.deleteSkillTagSubscription = this.skillTagSevice.deleteSkillTag(skillTagIndex)
          .subscribe(() => {
            this.spinnerService.hide(this.spinner);
            this.loadSkillTags();
            this.getModalPopup(MessagePopUpComponent, 'sm');
            this.setComponentMessages('Success', 'The record has been deleted!');
          }, (error) => {
            this.spinnerService.hide(this.spinner);
            if (error.status === 424) {
              this.getModalPopup(ErrorWarningPopUpComponent, 'sm');
              this.setComponentMessages('Error', error.error);
            }
          });

        this.subscriptions.push(this.deleteSkillTagSubscription);
      }
    });
  }

  search() {
    this.loadSkillTags();
  }

  sort(columnName: string, sortBy: string) {
    this.sortBy = sortBy === 'asc' ? 'desc' : 'asc';
    this.orderBy = columnName;

    this.loadSkillTags();
  }

  toggleDetails(skillTagId: number) {
    if (this.skillTag?.id === skillTagId) {
      this.skillTag = undefined;
    } else {
      this.getExpandedDetails(skillTagId);
    }
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
    this.loadSkillTags();
  }

  setClientLobgroup(clientLobGroupId: number) {
    this.clientLobGroupId = clientLobGroupId;
    this.loadSkillTags();
  }

  setSkillGroup(skillGroupId: number) {
    this.skillGroupId = skillGroupId;
    this.loadSkillTags();
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
    const skillTagQueryParams = new SkillTagQueryParams();
    skillTagQueryParams.clientId = this.clientId;
    skillTagQueryParams.clientLobGroupId = this.clientLobGroupId;
    skillTagQueryParams.skillGroupId = this.skillGroupId;
    skillTagQueryParams.pageNumber = this.currentPage;
    skillTagQueryParams.pageSize = this.pageSize;
    skillTagQueryParams.searchKeyword = this.searchKeyword ?? '';
    skillTagQueryParams.orderBy = `${this.orderBy} ${this.sortBy}`;
    skillTagQueryParams.fields = '';

    return skillTagQueryParams;
  }

  private loadSkillTags() {
    const queryParams = this.getQueryParams();
    this.spinnerService.show(this.spinner, SpinnerOptions);

    this.getSkillTagsSubscription = this.skillTagSevice.getSkillTags(queryParams)
      .subscribe((response) => {
        if (response.body) {
          this.skillTags = response.body;
          this.headerPaginationValues = JSON.parse(response.headers.get('x-pagination'));
          this.totalSkillTagsRecord = this.headerPaginationValues.totalCount;
        }
        this.spinnerService.hide(this.spinner);
      }, (error) => {
        this.spinnerService.hide(this.spinner);
        console.log(error);
      });

    this.subscriptions.push(this.getSkillTagsSubscription);
  }

  private getExpandedDetails(skillTagId: number) {
    this.spinnerService.show(this.spinner, SpinnerOptions);

    this.getSkillTagSubscription = this.skillTagSevice.getSkillTag(skillTagId)
      .subscribe((response) => {
        if (response) {
          this.skillTag = response;
        }
        this.spinnerService.hide(this.spinner);
      }, (error) => {
        this.spinnerService.hide(this.spinner);
        console.log(error);
      });

    this.subscriptions.push(this.getSkillTagSubscription);
  }

}