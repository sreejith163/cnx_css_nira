import { Component, OnInit } from '@angular/core';
import { SkillTag } from '../../models/skill-tag.model';
import { SkillTagsService } from '../../services/skill-tags.service';
import { NgbModal, NgbModalOptions } from '@ng-bootstrap/ng-bootstrap';
import { transition, animate, style, trigger, state } from '@angular/animations';
import { ConfirmationPopUpComponent } from 'src/app/shared/popups/confirmation-pop-up/confirmation-pop-up.component';
import { MessagePopUpComponent } from 'src/app/shared/popups/message-pop-up/message-pop-up.component';
import { UpdateSkillTagsComponent } from '../update-skill-tags/update-skill-tags.component';
import { Constants } from 'src/app/shared/util/constants.util';
import { Translation } from 'src/app/shared/models/translation.model';
import { WeekDay } from '@angular/common';

@Component({
  selector: 'app-skill-tags',
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
  templateUrl: './skill-tags.component.html',
  styleUrls: ['./skill-tags.component.css']
})
export class SkillTagsComponent implements OnInit {

  currentPage = 1;
  pageSize = 5;
  expandedDetail: SkillTag;
  translationValues: Translation[];
  skillTags: SkillTag[] = [];

  constructor(
    private skillTagSevice: SkillTagsService,
    private modalService: NgbModal
  ) { }

  ngOnInit(): void {
    this.translationValues = Constants.skillTagsTranslationValues;
    this.skillTagSevice.getSkillTags()
      .subscribe((data) => {
        this.skillTags = data.sort((a, b) => new Date(b.createdDate).getTime() - new Date(a.createdDate).getTime()
        );
      });
  }

  addSkillTags() {
    const modalRef = this.getModalPopup(UpdateSkillTagsComponent, 'lg');
    modalRef.componentInstance.title = 'Add';
    modalRef.componentInstance.translationValues = this.translationValues;
  }

  edit(data: SkillTag) {
    const modalRef = this.getModalPopup(UpdateSkillTagsComponent, 'lg');
    modalRef.componentInstance.title = 'Edit';
    modalRef.componentInstance.skillTag = data;
    modalRef.componentInstance.translationValues = this.translationValues;

    modalRef.result.then((result) => {
      if (this.expandedDetail) {
        this.expandedDetail = result ? result : this.expandedDetail;
      }
    });
  }

  delete(id: number) {
    const modalRef = this.getModalPopup(ConfirmationPopUpComponent, 'sm');
    modalRef.componentInstance.headingMessage = 'Are you sure?';
    modalRef.componentInstance.contentMessage = 'You won’t be able to revert this!';
    modalRef.componentInstance.deleteRecordIndex = id;

    modalRef.result.then((result) => {
      if (result && result === id) {
        this.skillTagSevice.deleteSkillTag(id);
        const modal = this.getModalPopup(MessagePopUpComponent, 'sm');
        modal.componentInstance.headingMessage = 'Success';
        modal.componentInstance.contentMessage = 'The record has been deleted!';
      }
    });
  }

  changePageSize(pageSize: number) {
    this.pageSize = pageSize;
  }

  changePage(page: number) {
    this.currentPage = page;
  }

  toggleDetails(el: SkillTag) {
    if (this.expandedDetail && this.expandedDetail.id === el.id) {
      this.expandedDetail = null;
    } else {
      this.expandedDetail = el;
    }
  }

  getWeekDay(weekDay: number) {
    return WeekDay[weekDay];
  }

  private getModalPopup(component: any, size: string) {
    const options: NgbModalOptions = { backdrop: false, centered: true, size };
    return this.modalService.open(component, options);
  }
}
