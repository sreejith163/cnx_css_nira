import { Component, OnInit } from '@angular/core';
import { NgbModal, NgbModalOptions } from '@ng-bootstrap/ng-bootstrap';
import { ConfirmationPopUpComponent } from 'src/app/shared/popups/confirmation-pop-up/confirmation-pop-up.component';
import { MessagePopUpComponent } from 'src/app/shared/popups/message-pop-up/message-pop-up.component';
import { Constants } from 'src/app/shared/util/constants.util';
import { ClientLOBGroupDetails } from '../../models/client-lob-group-details.model';
import { PaginationSize } from 'src/app/shared/models/pagination-size.model';
import { ClientLobGroupDropdownService } from '../../services/client-lob-group-dropdown.service';
import { ClientLobGroupListService } from '../../services/client-lob-group-list.service';
import { AddEditClientLobGroupComponent } from '../add-edit-client-lob-group/add-edit-client-lob-group.component';
import { Translation } from 'src/app/shared/models/translation.model';
import { WeekDay } from '@angular/common';

@Component({
  selector: 'app-client-lob-group-list',
  templateUrl: './client-lob-group-list.component.html',
  styleUrls: ['./client-lob-group-list.component.scss']
})
export class ClientLobGroupListComponent implements OnInit {

  currentPage = 1;
  pageSize = 5;
  weekDay = WeekDay;
  translationValues: Translation[];
  paginationSize: PaginationSize[] = [];
  totalLobGroupRecord: number;

  totalLobGroup: ClientLOBGroupDetails[] = [];

  constructor(
    private clientLOBGroupService: ClientLobGroupListService,
    private clinetLobGroupDropdownService: ClientLobGroupDropdownService,
    private modalService: NgbModal
  ) {}

  ngOnInit(): void {
    this.translationValues = Constants.clientLOBGroupTranslationValues;
    this.totalLobGroup = this.clientLOBGroupService.getClientLOBGroups();
    this.totalLobGroupRecord = this.totalLobGroup.length;
    this.totalLobGroup = this.totalLobGroup.sort((a, b) => new Date(b.createdDate).getTime() - new Date(a.createdDate).getTime()
    );
    this.paginationSize = this.clinetLobGroupDropdownService.getTablePageSizeList();
  }

  changePageSize(pageSize: number) {
    this.pageSize = pageSize;
  }

  changePage(page: number) {
    this.currentPage = page;
  }

  openModal() {
    const modalRef = this.setModalOptionsForAddEdit();
    modalRef.componentInstance.title =  'Add';
    modalRef.componentInstance.translationValues = this.translationValues;
  }

  edit(clientLobGroup: ClientLOBGroupDetails) {
    const modalRef = this.setModalOptionsForAddEdit();
    modalRef.componentInstance.title = 'Edit';
    modalRef.componentInstance.clientLobGroupData = clientLobGroup;
    modalRef.componentInstance.translationValues = this.translationValues;
  }

  delete(clientLobGroupId: number) {
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
    modalRef.componentInstance.deleteRecordIndex = clientLobGroupId;

    modalRef.result.then((result) => {
      if (result && result === clientLobGroupId) {
        this.clientLOBGroupService.deleteClientLOBGroup(clientLobGroupId);
        const modal = this.getModalPopup(MessagePopUpComponent, 'sm');
        modal.componentInstance.headingMessage = 'Success';
        modal.componentInstance.contentMessage = 'The record has been deleted!';
      }
    });
  }

  private setModalOptionsForAddEdit() {
    const options: NgbModalOptions = {
      backdrop: false,
      centered: true,
      size: 'lg',
    };
    const modalRef = this.modalService.open(AddEditClientLobGroupComponent, options);
    return modalRef;
  }

  private getModalPopup(component: any, size: string) {
    const options: NgbModalOptions = { backdrop: false, centered: true, size };
    return this.modalService.open(component, options);
  }

}

