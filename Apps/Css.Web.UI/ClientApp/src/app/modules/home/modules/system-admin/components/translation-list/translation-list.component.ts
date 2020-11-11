import { Component, OnInit } from '@angular/core';
import { NgbModal, NgbModalOptions } from '@ng-bootstrap/ng-bootstrap';
import { PaginationSize } from 'src/app/shared/models/pagination-size.model';
import { Translation } from 'src/app/shared/models/translation.model';
import { ConfirmationPopUpComponent } from 'src/app/shared/popups/confirmation-pop-up/confirmation-pop-up.component';
import { MessagePopUpComponent } from 'src/app/shared/popups/message-pop-up/message-pop-up.component';
import { Constants } from 'src/app/shared/util/constants.util';
import { TranslationService } from '../../services/translation.service';
import { AddEditTranslationComponent } from '../add-edit-translation/add-edit-translation.component';

@Component({
  selector: 'app-translation-list',
  templateUrl: './translation-list.component.html',
  styleUrls: ['./translation-list.component.scss']
})
export class TranslationListComponent implements OnInit {

  currentPage = 1;

  pageSize = 10;
  characterSplice = 25;
  totalRecord: number;
  translationValues: Translation[];
  paginationSize: PaginationSize[] = [];
  totaltranslationList: Translation[] = [];

  constructor(
    private translationService: TranslationService,
    private modalService: NgbModal
  ) { }

  ngOnInit(): void {
    this.translationValues = Constants.translationTranslationValues;
    this.totaltranslationList = this.translationService.getTranslationList();
    this.totalRecord = this.totaltranslationList.length;
    this.totaltranslationList = this.totaltranslationList.sort((a, b) =>
      new Date(b.createdDate).getTime() - new Date(a.createdDate).getTime());
    this.paginationSize = Constants.paginationSize;
  }

  openModal() {
    const modalRef = this.getModalPopup(AddEditTranslationComponent, 'lg');
    modalRef.componentInstance.title = 'Add';
    modalRef.componentInstance.translationValues = this.translationValues;
    modalRef.result.then((result) => {
      if (result) {
        this.translationService.addTranslation(result);
      }
    });
  }

  edit(translationData) {
    const modalRef = this.getModalPopup(AddEditTranslationComponent, 'lg');
    modalRef.componentInstance.title = 'Edit';
    modalRef.componentInstance.language = translationData.language;
    modalRef.componentInstance.menu = translationData.menu;
    modalRef.componentInstance.variable = translationData.variableId;
    modalRef.componentInstance.description = translationData.description;
    modalRef.componentInstance.translation = translationData.translation;
    modalRef.componentInstance.translationData = translationData;
    modalRef.componentInstance.translationValues = this.translationValues;
    modalRef.result.then((result) => {
      if (result) {
        this.translationService.updateTranslation(result);
      }
    });
  }

  delete(translationData) {
    const modalRef = this.getModalPopup(ConfirmationPopUpComponent, 'sm');
    modalRef.componentInstance.headingMessage = 'Are you sure?';
    modalRef.componentInstance.contentMessage = 'You won’t be able to revert this!';
    modalRef.componentInstance.deleteRecordIndex = translationData.variableId;

    modalRef.result.then((result) => {
      if (result && result === translationData.variableId) {
        this.translationService.deleteTranslation(translationData);
        const modal = this.getModalPopup(MessagePopUpComponent, 'sm');
        modal.componentInstance.headingMessage = 'Success';
        modal.componentInstance.contentMessage = 'The record has been deleted!';
      }
    });
  }

  changePageSize(pageSize) {
    this.pageSize = Number(pageSize);
  }

  changePage(page) {
    this.currentPage = page;
  }

  private getModalPopup(component: any, size: string) {
    const options: NgbModalOptions = { backdrop: false, centered: true, size };
    return this.modalService.open(component, options);
  }
}
