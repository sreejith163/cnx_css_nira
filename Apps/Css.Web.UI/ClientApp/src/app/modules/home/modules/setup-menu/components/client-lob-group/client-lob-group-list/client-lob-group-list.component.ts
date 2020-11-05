
import { Component, OnDestroy, OnInit } from '@angular/core';
import { NgbModal, NgbModalOptions, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { NgxSpinnerService } from 'ngx-spinner';
import { SubscriptionLike as ISubscription } from 'rxjs';

import { ConfirmationPopUpComponent } from 'src/app/shared/popups/confirmation-pop-up/confirmation-pop-up.component';
import { MessagePopUpComponent } from 'src/app/shared/popups/message-pop-up/message-pop-up.component';

import { ClientDetails } from '../../../models/client-details.model';
import { Translation } from 'src/app/shared/models/translation.model';
import { ClientNameQueryParameters } from '../../../models/client-name-query-parameters.model';
import { HeaderPagination } from 'src/app/shared/models/header-pagination.model';
import { ComponentOperation } from 'src/app/shared/enums/component-operation.enum';

import { Constants } from 'src/app/shared/util/constants.util';
import { ClientService } from '../../../services/client.service';
import { ClientLOBGroupDetails } from '../../../models/client-lob-group-details.model';
import { ClientLobGroupService } from '../../../services/client-lob-group.service';
import { AddUpdateClientLobGroupComponent } from '../add-update-client-lob-group/add-update-client-lob-group.component';
import { ClientLobGroupQueryParameters } from '../../../models/client-lob-group-query-parameters.model';
import { PaginationSize } from 'src/app/shared/models/pagination-size.model';
import { WeekDay } from '@angular/common';

@Component({
  selector: 'app-client-lob-group-list',
  templateUrl: './client-lob-group-list.component.html',
  styleUrls: ['./client-lob-group-list.component.scss']
})

export class ClientLobGroupListComponent implements OnInit, OnDestroy {

  currentPage = 1;
  pageSize = 5;
  totalClientLOBGroupRecord: number;
  clientLOBGroupId: number;
  clientId: number;


  searchClientLOBGroupName: string;
  orderBy = 'createdDate';
  sortKeyword: 'asc' | 'desc';
  weekDay = WeekDay;


  modalRef: NgbModalRef;
  headerPaginationValues: HeaderPagination;
  paginationSize: PaginationSize[] = [];
  translationValues: Translation[];
  clientLOBGroupDetails: ClientLOBGroupDetails[] = [];

  getAllClientLOBGroupDetailsSubscription: ISubscription;
  getClientLOBGroupTranslationSubscription: ISubscription;
  deleteClientLOBGroupSubscription: ISubscription;
  subscriptionList: ISubscription[] = [];

  constructor(
    private modalService: NgbModal,
    private spinner: NgxSpinnerService,
    private clientLOBGroupService: ClientLobGroupService
  ) { }

  ngOnInit(): void {
    this.sortKeyword = 'asc';
    this.translationValues = Constants.clientLOBGroupTranslationValues;
    this.paginationSize = Constants.paginationSize;
    this.loadClientLOBGroups();
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
    this.loadClientLOBGroups();
  }

  changePage(page: number) {
    this.currentPage = page;
    this.loadClientLOBGroups();
  }

  setClient(client: number) {
    this.clientId = client;
    this.loadClientLOBGroups();
  }

  addClientLOBGroup() {
    this.getModalPopup(AddUpdateClientLobGroupComponent, 'lg');
    this.setComponentValues(ComponentOperation.Add, this.translationValues);

    this.modalRef.result.then(() => {
      this.loadClientLOBGroups();
    });
  }

  editClientLOBGroup(data: ClientLOBGroupDetails) {
    this.getModalPopup(AddUpdateClientLobGroupComponent, 'lg');
    this.setComponentValues(ComponentOperation.Edit, this.translationValues);
    this.modalRef.componentInstance.clientDetails = data;

    this.modalRef.result.then(() => {
      this.loadClientLOBGroups();
    });
  }

  deleteClientLOBGroup(clientIndex: number) {
    this.getModalPopup(ConfirmationPopUpComponent, 'sm');
    this.setComponentMessages('Are you sure?', 'You won’t be able to revert this!');
    this.modalRef.componentInstance.deleteRecordIndex = clientIndex;

    this.modalRef.result.then((result) => {
      if (result && result === clientIndex) {
        this.deleteClientLOBGroupSubscription = this.clientLOBGroupService.deleteClient(clientIndex)
          .subscribe((response) => {
            if (response) {
              this.loadClientLOBGroups();
              this.getModalPopup(MessagePopUpComponent, 'sm');
              this.setComponentMessages('Success', 'The record has been deleted!');
            }
          }, (error) => {
            console.log(error);
          });

        this.subscriptionList.push(this.deleteClientLOBGroupSubscription);
      }
    });
  }

  setClientLOBGroup(clientLOBGroup: number) {
    this.clientLOBGroupId = clientLOBGroup;
  }

  searchClientLOBGroups() {
    this.loadClientLOBGroups();
  }

  sortClientLOBGroups(orderBy: string) {
    this.sortKeyword = this.sortKeyword === 'asc' ? 'desc' : 'asc';
    this.orderBy = `${orderBy} ${this.sortKeyword}`;
    //this.loadClientLOBGroups();
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
    const clientLOBGroupQueryParams = new ClientLobGroupQueryParameters();
    clientLOBGroupQueryParams.clientLOBGroupId = this.clientLOBGroupId ?? 0;
    clientLOBGroupQueryParams.clientId = this.clientId ?? 0;
    clientLOBGroupQueryParams.pageNumber = this.currentPage;
    clientLOBGroupQueryParams.pageSize = this.pageSize;
    clientLOBGroupQueryParams.searchKeyword = this.searchClientLOBGroupName ?? '';
    clientLOBGroupQueryParams.orderBy = this.orderBy;
    clientLOBGroupQueryParams.sortBy = this.sortKeyword;
    clientLOBGroupQueryParams.fields = '';

    return clientLOBGroupQueryParams;
  }

  private loadClientLOBGroups() {
    this.spinner.show();
    const queryParams = this.getQueryParams();
    console.log("loadClientLOBGroups");
    this.getAllClientLOBGroupDetailsSubscription = this.clientLOBGroupService.getClientLOBGroups(queryParams)
      .subscribe((response) => {
        if (response.body) {
          console.log("response.bory");
          this.clientLOBGroupDetails = response.body;
          this.headerPaginationValues = JSON.parse(response.headers.get('x-pagination'));
          this.totalClientLOBGroupRecord = this.headerPaginationValues.totalCount;
        }

        this.spinner.hide();
      }, (error) => {
        this.spinner.hide();
        console.log(error);
      });

    this.subscriptionList.push(this.getAllClientLOBGroupDetailsSubscription);
  }
}