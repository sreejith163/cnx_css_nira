import { animate, state, style, transition, trigger } from '@angular/animations';
import { WeekDay } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { NgbModal, NgbModalOptions } from '@ng-bootstrap/ng-bootstrap';
import { PaginationSize } from 'src/app/shared/models/pagination-size.model';
import { Translation } from 'src/app/shared/models/translation.model';
import { ConfirmationPopUpComponent } from 'src/app/shared/popups/confirmation-pop-up/confirmation-pop-up.component';
import { MessagePopUpComponent } from 'src/app/shared/popups/message-pop-up/message-pop-up.component';
import { Constants } from 'src/app/shared/util/constants.util';
import { SkillGroupDetails} from '../../models/skill-group.model';
import { DropdownListService } from '../../services/dropdown-list.service';
import { SkillGroupsService } from '../../services/skill-groups.service';
import { AddEditSkillGroupComponent } from '../add-edit-skill-group/add-edit-skill-group.component';

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
export class SkillGroupListComponent implements OnInit {

  currentPage = 1;
  pageSize = 10;
  characterSplice = 25;
  totalSkillGroupRecord: number;
  weekDay = WeekDay;
  skillGroupData: SkillGroupDetails;
  expandedDetail: SkillGroupDetails;
  translationValues: Translation[];
  paginationSize: PaginationSize[] = [];
  totalSkillGroup: SkillGroupDetails[] = [];
  skillGroups: SkillGroupDetails[] = [];

  constructor(
    private skillGroupService: SkillGroupsService,
    private modalService: NgbModal
  ) { }

  ngOnInit(): void {
    this.translationValues = Constants.skillGroupsTranslationValues;
    this.totalSkillGroup = this.skillGroupService.getSkillGroups();
    this.totalSkillGroupRecord = this.totalSkillGroup.length;
    this.skillGroups = this.totalSkillGroup.sort((a, b) => new Date(b.createdDate).getTime() - new Date(a.createdDate).getTime()
    );
    this.paginationSize = Constants.paginationSize;
  }

  changePageSize(pageSize: number) {
    this.pageSize = pageSize;
  }

  changePage(page: number) {
    this.currentPage = page;
  }

  addSkillGroup() {
    const modalRef = this.setModalOptionsForAddEdit();
    modalRef.componentInstance.title = 'Add';
    modalRef.componentInstance.translationValues = this.translationValues;
  }

  editSkillGroup(skillGroupData: SkillGroupDetails) {
    const modalRef = this.setModalOptionsForAddEdit();
    modalRef.componentInstance.title = 'Edit';
    modalRef.componentInstance.skillGroupData = skillGroupData;
    modalRef.componentInstance.translationValues = this.translationValues;

    modalRef.result.then((result) => {
      if (this.expandedDetail) {
        this.expandedDetail = result ? result : this.expandedDetail;
      }
    });
  }

  delete(skillGroupId: number) {
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
    modalRef.componentInstance.deleteRecordIndex = skillGroupId;

    modalRef.result.then((result) => {
      if (result && result === skillGroupId) {
        this.skillGroupService.deleteSkillGroup(skillGroupId);
        const modal = this.getModalPopup(MessagePopUpComponent, 'sm');
        modal.componentInstance.headingMessage = 'Success';
        modal.componentInstance.contentMessage = 'The record has been deleted!';
      }
    });
  }

  toggleDetails(el: SkillGroupDetails) {
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
    const modalRef = this.modalService.open(AddEditSkillGroupComponent, options);
    return modalRef;
  }

  private getModalPopup(component: any, size: string) {
    const options: NgbModalOptions = { backdrop: false, centered: true, size };
    return this.modalService.open(component, options);
  }
}
