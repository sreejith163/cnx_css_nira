import { animate, state, style, transition, trigger } from '@angular/animations';
import { WeekDay } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { NgbModal, NgbModalOptions } from '@ng-bootstrap/ng-bootstrap';
import { PaginationSize } from 'src/app/shared/models/pagination-size.model';
import { Translation } from 'src/app/shared/models/translation.model';
import { ConfirmationPopUpComponent } from 'src/app/shared/popups/confirmation-pop-up/confirmation-pop-up.component';
import { MessagePopUpComponent } from 'src/app/shared/popups/message-pop-up/message-pop-up.component';
import { Constants } from 'src/app/shared/util/constants.util';
import { SchedulingGroupDetails } from '../../models/scheduling-group-details.model';
import { AgentSchedulingGroupService } from '../../services/agent-scheduling-group.service';
import { DropdownListService } from '../../services/dropdown-list.service';
import { AddEditAgentSchedulingGroupComponent } from '../add-edit-agent-scheduling-group/add-edit-agent-scheduling-group.component';

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
export class AgentSchedulingGroupListComponent implements OnInit {

  currentPage = 1;
  pageSize = 5;
  totalScheduledGroupRecord: number;
  weekDay = WeekDay;
  expandedDetail: SchedulingGroupDetails;
  translationValues: Translation[];
  paginationSize: PaginationSize[] = [];
  totalScheduledGroup: SchedulingGroupDetails[] = [];

  constructor(
    private schedulingGroupService: AgentSchedulingGroupService,
    private dropdownService: DropdownListService,
    private modalService: NgbModal
  ) { }

  ngOnInit(): void {
    this.translationValues = Constants.agentSchedulingGroupTranslationValues;
    this.totalScheduledGroup = this.schedulingGroupService.getSchedulingGroups();
    this.totalScheduledGroupRecord = this.totalScheduledGroup.length;
    this.totalScheduledGroup = this.totalScheduledGroup.sort((a, b) => new Date(b.createdDate).getTime() - new Date(a.createdDate).getTime()
    );
    this.paginationSize = this.dropdownService.getTablePageSizeList();
  }

  changePageSize(pageSize: number) {
    this.pageSize = pageSize;
  }

  changePage(page: number) {
    this.currentPage = page;
  }

  addSchedulingGroup() {
    const modalRef = this.setModalOptionsForAddEdit();
    modalRef.componentInstance.title = 'Add';
    modalRef.componentInstance.translationValues = this.translationValues;
  }

  editSchedulingGroup(schedulingGroupData: SchedulingGroupDetails) {
    const modalRef = this.setModalOptionsForAddEdit();
    modalRef.componentInstance.title = 'Edit';
    modalRef.componentInstance.schedulingGroupData = schedulingGroupData;
    modalRef.componentInstance.translationValues = this.translationValues;

    modalRef.result.then((result) => {
      if (this.expandedDetail) {
        this.expandedDetail = result ? result : this.expandedDetail;
      }
    });
  }

  delete(schedulingGroupId: number) {
    const options: NgbModalOptions = {
      backdrop: false,
      centered: true,
      size: 'sm',
    };
    const modalRef = this.modalService.open(
      ConfirmationPopUpComponent,
      options
    );
    modalRef.componentInstance.headingMessage = 'Are you sure?';
    modalRef.componentInstance.contentMessage =
      'You won’t be able to revert this!';
    modalRef.componentInstance.deleteRecordIndex = schedulingGroupId;

    modalRef.result.then((result) => {
      if (result && result === schedulingGroupId) {
        this.schedulingGroupService.deleteSchedulingGroup(schedulingGroupId);
        const modal = this.getModalPopup(MessagePopUpComponent, 'sm');
        modal.componentInstance.headingMessage = 'Success';
        modal.componentInstance.contentMessage = 'The record has been deleted!';
      }
    });
  }

  toggleDetails(el: SchedulingGroupDetails) {
    if (this.expandedDetail && this.expandedDetail.id === el.id) {
      this.expandedDetail = null;
    } else {
      this.expandedDetail = el;
    }
  }

  getWeekDay(weekDay: number) {
    return WeekDay[weekDay];
  }

  private setModalOptionsForAddEdit() {
    const options: NgbModalOptions = {
      backdrop: false,
      centered: true,
      size: 'lg',
    };
    const modalRef = this.modalService.open(AddEditAgentSchedulingGroupComponent, options);
    return modalRef;
  }

  private getModalPopup(component: any, size: string) {
    const options: NgbModalOptions = { backdrop: false, centered: true, size };
    return this.modalService.open(component, options);
  }

}
