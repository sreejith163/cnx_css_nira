import { Component, OnInit } from '@angular/core';
import { NgbModal, NgbModalOptions } from '@ng-bootstrap/ng-bootstrap';
import { ConfirmationPopUpComponent } from 'src/app/shared/popups/confirmation-pop-up/confirmation-pop-up.component';
import { ClientDetails } from '../../models/client-details.model';
import { ClientNameListService } from '../../services/client-name-list.service';
import { UpdateClientListComponent } from '../update-client-list/update-client-list.component';
import { MessagePopUpComponent } from 'src/app/shared/popups/message-pop-up/message-pop-up.component';
import { Translation } from 'src/app/shared/models/translation.model';
import { Constants } from 'src/app/shared/util/constants.util';

@Component({
  selector: 'app-client-name-list',
  templateUrl: './client-name-list.component.html',
  styleUrls: ['./client-name-list.component.css']
})
export class ClientNameListComponent implements OnInit {

  currentPage = 1;
  pageSize = 5;
  translationValues: Translation[];
  clients: ClientDetails[] = [];

  constructor(
    private clienNameListService: ClientNameListService,
    private modalService: NgbModal
  ) { }

  ngOnInit(): void {
    this.translationValues = Constants.clientNameTranslationValues;
    const clients = this.clienNameListService.getClientDetails();
    this.clients = clients.sort((a, b) => new Date(b.createdDate).getTime() - new Date(a.createdDate).getTime());
  }

  changePageSize(pageSize: number) {
    this.pageSize = pageSize;
  }

  changePage(page: number) {
    this.currentPage = page;
  }

  addClient() {
    const modalRef = this.getModalPopup(UpdateClientListComponent, 'lg');
    modalRef.componentInstance.title = 'Add';
    modalRef.componentInstance.translationValues = this.translationValues;
  }

  edit(data: ClientDetails) {
    const modalRef = this.getModalPopup(UpdateClientListComponent, 'lg');
    modalRef.componentInstance.title = 'Edit';
    modalRef.componentInstance.clientDetails = data;
    modalRef.componentInstance.translationValues = this.translationValues;
  }

  delete(clientIndex: number) {
    const modalRef = this.getModalPopup(ConfirmationPopUpComponent, 'sm');
    modalRef.componentInstance.headingMessage = 'Are you sure?';
    modalRef.componentInstance.contentMessage = 'You won’t be able to revert this!';
    modalRef.componentInstance.deleteRecordIndex = clientIndex;

    modalRef.result.then((result) => {
      if (result && result === clientIndex) {
        this.clienNameListService.deleteClient(clientIndex);
        const modal = this.getModalPopup(MessagePopUpComponent, 'sm');
        modal.componentInstance.headingMessage = 'Success';
        modal.componentInstance.contentMessage = 'The record has been deleted!';
      }
    });
  }

  private getModalPopup(component: any, size: string) {
    const options: NgbModalOptions = { backdrop: false, centered: true, size };
    return this.modalService.open(component, options);
  }
}
