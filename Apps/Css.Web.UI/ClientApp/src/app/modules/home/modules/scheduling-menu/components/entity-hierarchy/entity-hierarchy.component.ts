import { Component, Input, EventEmitter, OnChanges, OnDestroy, OnInit, Output, Injectable } from '@angular/core';

import { NgbModal, NgbModalOptions, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { NgxSpinnerService } from 'ngx-spinner';
import { SubscriptionLike as ISubscription } from 'rxjs';
import { ClientDetails } from '../../../setup-menu/models/client-details.model';
import { ClientService } from '../../../setup-menu/services/client.service';
import { EntityHierarchyModel } from '../../models/entity-hierarchy.model';
import { EntityHierarchyService } from '../../services/entity-hierarchy.service';


@Component({
  selector: 'app-entity-hierarchy',
  templateUrl: './entity-hierarchy.component.html',
  styleUrls: ['./entity-hierarchy.component.scss']
})

export class EntityHierarchyComponent implements OnInit, OnDestroy, OnChanges {

  clientId: number;

  spinnerLeft = 'entityLeft';
  spinnerRight = 'entityRight';


  clientsDetails: ClientDetails[] = [];
  subscriptionList: ISubscription[] = [];
  EntityHierarchyModel: EntityHierarchyModel[] = [];
  EntityHierarchyVar = [];

  constructor(
    private spinnerService: NgxSpinnerService,
    private clientService: ClientService,
    private entityHierarchyService: EntityHierarchyService,
    ) {
  }


  ngOnInit() {
  }

  ngOnDestroy() {
  }

  ngOnChanges() {
  }

  setClient(client: number) {
    this.clientId = client;
    // logic

    this.EntityHierarchyVar = this.getEntityHierarchy();
  }

  getEntityHierarchy(){
    console.log(this.clientId);
    if (this.clientId != null){
      let result;
      result = this.entityHierarchyService.getEntityHierarchyDataById(this.clientId)
      .subscribe((response) => {
        this.spinnerService.hide(this.spinnerLeft);
        if (response.body) {
          this.EntityHierarchyModel = response.body;
        }
      }, (error) => {
        this.spinnerService.hide(this.spinnerLeft);
        console.log(error);
      });
      console.log(this.EntityHierarchyModel);
      return result;
    }

  }
}
