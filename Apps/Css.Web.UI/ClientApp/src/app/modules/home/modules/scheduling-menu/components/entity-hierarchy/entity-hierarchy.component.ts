import { Component, Input, EventEmitter, OnChanges, OnDestroy, OnInit, Output, Injectable } from '@angular/core';

import { NgbModal, NgbModalOptions, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { NgxSpinnerService } from 'ngx-spinner';
import { SubscriptionLike as ISubscription } from 'rxjs';
import { ClientDetails } from '../../../setup-menu/models/client-details.model';
import { ClientService } from '../../../setup-menu/services/client.service';
import { EntityAgentSchedulingGroupDetails, EntityClientDetails, EntityClientLOBDetails, EntityHierarchyModel } from '../../models/entity-hierarchy.model'
import { EntityHierarchyService } from '../../services/entity-hierarchy.service';


@Component({
  selector: 'app-entity-hierarchy',
  templateUrl: './entity-hierarchy.component.html',
  styleUrls: ['./entity-hierarchy.component.scss']
})

export class EntityHierarchyComponent implements OnInit {
  clientId: number;
  
  spinnerLeft = 'entityLeft';
  spinnerRight = 'entityRight';
  isHidden: boolean[] = [];
  
  subscriptionList: ISubscription[] = [];
  entityHierarchy: EntityHierarchyModel;
  entityClient: EntityClientDetails;
  entityAgentSchedulingGroups: EntityAgentSchedulingGroupDetails[] = [];
  entityClientLOBs: EntityClientLOBDetails[] = [];

  constructor(
    private spinnerService: NgxSpinnerService,
    private clientService: ClientService,
    private entityHierarchyService: EntityHierarchyService,
    ) {
  }


  ngOnInit() {
  }

  setClient(client: number) {
    this.clientId = client;
    // logic

    this.getEntityHierarchy();
  }

  getEntityHierarchy(){
    if (this.clientId != null){
      let result;
      result = this.entityHierarchyService.getEntityHierarchyDataById(this.clientId)
      .subscribe((response) => {
        console.log(response)
        this.spinnerService.hide(this.spinnerLeft);
        if (response) {
          this.entityHierarchy = response;
          this.entityClient = this.entityHierarchy.client;
          this.entityAgentSchedulingGroups = this.entityHierarchy.agentSchedulingGroups;
          this.entityClientLOBs = this.entityClient.clientLOBs;
        }
      }, (error) => {
        this.spinnerService.hide(this.spinnerLeft);
        console.log(error);
      });
    }
  }
}
