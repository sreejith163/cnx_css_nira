import { Component, OnDestroy, OnInit } from '@angular/core';
import { NgbModal, NgbModalOptions, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { NgxSpinnerService } from 'ngx-spinner';
import { SubscriptionLike as ISubscription } from 'rxjs';

import { ConfirmationPopUpComponent } from 'src/app/shared/popups/confirmation-pop-up/confirmation-pop-up.component';
import { AddUpdateClientNameComponent } from '../add-update-client-name/add-update-client-name.component';
import { MessagePopUpComponent } from 'src/app/shared/popups/message-pop-up/message-pop-up.component';

import { ClientDetails } from '../../../models/client-details.model';
import { Translation } from 'src/app/shared/models/translation.model';
import { ClientNameQueryParameters } from '../../../models/client-name-query-parameters.model';
import { HeaderPagination } from 'src/app/shared/models/header-pagination.model';
import { PaginationSize } from 'src/app/shared/models/pagination-size.model';
import { ComponentOperation } from 'src/app/shared/enums/component-operation.enum';

import { Constants } from 'src/app/shared/util/constants.util';
import { SpinnerOptions } from 'src/app/shared/util/spinner-options.util';

import { ClientService } from '../../../services/client.service';

@Component({
  selector: 'app-client-name-list',
  templateUrl: './client-name-list.component.html',
  styleUrls: ['./client-name-list.component.css']
})
export class ClientNameListComponent implements OnInit, OnDestroy {

  currentPage = 1;
  pageSize = 5;
  totalClientRecord: number;
  clientId: number;

  searchKeyword: string;
  orderBy = 'createdDate';
  sortBy = 'desc';
  spinner = 'tableSpinner';

  modalRef: NgbModalRef;
  headerPaginationValues: HeaderPagination;
  paginationSize: PaginationSize[] = [];
  translationValues: Translation[];
  clientsDetails: ClientDetails[] = [];

  getAllClientDetailsSubscription: ISubscription;
  getClientTranslationSubscription: ISubscription;
  deleteClientSubscription: ISubscription;
  subscriptionList: ISubscription[] = [];

  constructor(
    private modalService: NgbModal,
    private spinnerService: NgxSpinnerService,
    private clientService: ClientService
  ) { }

  ngOnInit(): void {
    this.translationValues = Constants.clientNameTranslationValues;
    this.paginationSize = Constants.paginationSize;
    this.loadClients();
  }

  ngOnDestroy() {
    this.subscriptionList.forEach(subscription => {
      if (subscription) {
        subscription.unsubscribe();
      }
    });
  }

  changePageSize(pageSize: number) {
    this.pageSize = pageSize;
    this.loadClients();
  }

  changePage(page: number) {
    this.currentPage = page;
    this.loadClients();
  }

  addClient() {
    this.getModalPopup(AddUpdateClientNameComponent, 'lg');
    this.setComponentValues(ComponentOperation.Add, this.translationValues);

    this.modalRef.result.then(() => {
      this.currentPage = 1;
      this.loadClients();
    });
  }

  editClient(data: ClientDetails) {
    this.getModalPopup(AddUpdateClientNameComponent, 'lg');
    this.setComponentValues(ComponentOperation.Edit, this.translationValues);
    this.modalRef.componentInstance.clientDetails = data;

    this.modalRef.result.then((result: any) => {
      if (result.needRefresh) {
        this.loadClients();
      }
    });
  }

  deleteClient(clientIndex: number) {
    this.getModalPopup(ConfirmationPopUpComponent, 'sm');
    this.setComponentMessages('Are you sure?', 'You won’t be able to revert this!');
    this.modalRef.componentInstance.deleteRecordIndex = clientIndex;

    this.modalRef.result.then((result) => {
      if (result && result === clientIndex) {

        this.spinnerService.show(this.spinner, SpinnerOptions);
        this.deleteClientSubscription = this.clientService.deleteClient(clientIndex)
          .subscribe(() => {
            this.spinnerService.hide(this.spinner);
            this.loadClients();
            this.getModalPopup(MessagePopUpComponent, 'sm');
            this.setComponentMessages('Success', 'The record has been deleted!');
          }, (error) => {
            this.spinnerService.hide(this.spinner);
            console.log(error);
          });

        this.subscriptionList.push(this.deleteClientSubscription);
      }
    });
  }

  setClient(client: number) {
    this.clientId = client;
  }

  searchClients() {
    this.loadClients();
  }

  sortClients(columnName: string, sortBy: string) {
    this.sortBy = sortBy === 'asc' ? 'desc' : 'asc';
    this.orderBy = columnName;

    this.loadClients();
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
    const clientQueryParams = new ClientNameQueryParameters();
    clientQueryParams.clientId = this.clientId ?? 0;
    clientQueryParams.pageNumber = this.currentPage;
    clientQueryParams.pageSize = this.pageSize;
    clientQueryParams.searchKeyword = this.searchKeyword ?? '';
    clientQueryParams.orderBy = `${this.orderBy} ${this.sortBy}`;
    clientQueryParams.fields = '';

    return clientQueryParams;
  }

  private loadClients() {
    const queryParams = this.getQueryParams();
    this.spinnerService.show(this.spinner, SpinnerOptions);

    this.getAllClientDetailsSubscription = this.clientService.getClients(queryParams)
      .subscribe((response) => {
        if (response.body) {
          this.clientsDetails = response.body;
          this.headerPaginationValues = JSON.parse(response.headers.get('x-pagination'));
          this.totalClientRecord = this.headerPaginationValues.totalCount;
        }

        this.spinnerService.hide(this.spinner);
      }, (error) => {
        this.spinnerService.hide(this.spinner);
        console.log(error);
      });

    this.subscriptionList.push(this.getAllClientDetailsSubscription);
  }
}