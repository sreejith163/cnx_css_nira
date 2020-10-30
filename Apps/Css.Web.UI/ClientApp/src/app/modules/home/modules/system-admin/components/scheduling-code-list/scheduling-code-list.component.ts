import { Component, OnInit } from '@angular/core';
import { NgbModal, NgbModalOptions } from '@ng-bootstrap/ng-bootstrap';
import { PaginationSize } from 'src/app/shared/models/pagination-size.model';
import { Translation } from 'src/app/shared/models/translation.model';
import { ConfirmationPopUpComponent } from 'src/app/shared/popups/confirmation-pop-up/confirmation-pop-up.component';
import { MessagePopUpComponent } from 'src/app/shared/popups/message-pop-up/message-pop-up.component';
import { Constants } from 'src/app/shared/util/constants.util';
import { SchedulingCode } from '../../models/scheduling-code.model';
import { SchedulingCodeService } from '../../services/scheduling-code.service';
import { AddEditSchedulingCodeComponent } from '../add-edit-scheduling-code/add-edit-scheduling-code.component';

@Component({
  selector: 'app-scheduling-code-list',
  templateUrl: './scheduling-code-list.component.html',
  styleUrls: ['./scheduling-code-list.component.scss']
})
export class SchedulingCodeListComponent implements OnInit {

  currentPage = 1;
  pageSize = 5;
  translationValues: Translation[];
  paginationSize: PaginationSize[] = [];
  totalSchedulingCodesRecord: number;

  totalSchedulingCodes: SchedulingCode[] = [];
  schedulingCodes: SchedulingCode[] = [];

  constructor(
    private schedulingCodeService: SchedulingCodeService,
    private modalService: NgbModal
  ) { }

  ngOnInit(): void {
    this.translationValues = Constants.schedulingCodeTranslationValues;
    this.totalSchedulingCodes = this.schedulingCodeService.getSchedulingCodes();
    this.totalSchedulingCodesRecord = this.totalSchedulingCodes.length;
    this.totalSchedulingCodes = this.totalSchedulingCodes.sort((a, b) => a.id - b.id);
    this.paginationSize = this.schedulingCodeService.getTablePageSizeList();
  }

  unifiedToNative(unified: string) {
    const codePoints = unified.split('-').map(u => parseInt(`0x${u}`, 16));
    return String.fromCodePoint(...codePoints);
  }

  openModal() {
    const modalRef = this.getModalPopup(AddEditSchedulingCodeComponent, 'lg');
    modalRef.componentInstance.title = 'Add';
    modalRef.componentInstance.translationValues = this.translationValues;
  }

  edit(schedulingCodeData: SchedulingCode) {
    const modalRef = this.getModalPopup(AddEditSchedulingCodeComponent, 'lg');
    modalRef.componentInstance.title = 'Edit';
    modalRef.componentInstance.schedulingCodeData = schedulingCodeData;
    modalRef.componentInstance.translationValues = this.translationValues;
  }

  delete(id: number) {
    const modalRef = this.getModalPopup(ConfirmationPopUpComponent, 'sm');
    modalRef.componentInstance.headingMessage = 'Are you sure?';
    modalRef.componentInstance.contentMessage = 'You won’t be able to revert this!';
    modalRef.componentInstance.deleteRecordIndex = id;

    modalRef.result.then((result) => {
      if (result && result === id) {
        this.schedulingCodeService.deleteSchedulingCode(id);
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

  private getModalPopup(component: any, size: string) {
    const options: NgbModalOptions = { backdrop: false, centered: true, size };
    return this.modalService.open(component, options);
  }

}
