import { Component, OnInit } from '@angular/core';
import { AgentAdminDetails } from '../../../models/agent-admin-details.model';
import { AgentAdminResponse } from '../../../models/agent-admin-response.model';
import { AgentSchedulesResponse } from '../../../models/agent-schedules-response.model';
import { MoveAgentAdminParameters } from '../../../models/move-agent-params.model';
import { AgentAdminService } from '../../../services/agent-admin.service';
import { SubscriptionLike as ISubscription } from 'rxjs';
import { ContentType } from 'src/app/shared/enums/content-type.enum';
import { ErrorWarningPopUpComponent } from 'src/app/shared/popups/error-warning-pop-up/error-warning-pop-up.component';
import { NgbModal, NgbModalOptions, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { AgentAdminQueryParameter } from '../../../models/agent-admin-query-parameter.model';
import { Constants } from 'src/app/shared/util/constants.util';
import { NgxSpinnerService } from 'ngx-spinner';
import { HeaderPagination } from 'src/app/shared/models/header-pagination.model';
import { SpinnerOptions } from 'src/app/shared/util/spinner-options.util';
import { AuthService } from 'src/app/core/services/auth.service';
import { MessagePopUpComponent } from 'src/app/shared/popups/message-pop-up/message-pop-up.component';
import { ConfirmationPopUpComponent } from 'src/app/shared/popups/confirmation-pop-up/confirmation-pop-up.component';
import { GenericPopUpComponent } from 'src/app/shared/popups/generic-pop-up/generic-pop-up.component';

@Component({
  selector: 'app-move-agents',
  templateUrl: './move-agents.component.html',
  styleUrls: ['./move-agents.component.scss']
})
export class MoveAgentsComponent implements OnInit {
  LoggedUser;
  modalRef: NgbModalRef;

  currentPageLeft = 1;
  pageSizeLeft = 10;
  characterSpliceLeft = 25;
  paginationSizeLeft = Constants.paginationSize;
  maxLengthLeft = Constants.DefaultTextMaxLength;
  orderByLeft = 'createdDate';
  sortByLeft = 'desc';
  spinnerLeft = 'agentAdminsLeft';
  headerPaginationValuesLeft: HeaderPagination;

  currentPageRight = 1;
  pageSizeRight = 10;
  characterSpliceRight = 25;
  paginationSizeRight = Constants.paginationSize;
  maxLengthRight = Constants.DefaultTextMaxLength;
  orderByRight = 'createdDate';
  sortByRight = 'desc';
  spinnerRight = 'agentAdminsRight';
  headerPaginationValuesRight: HeaderPagination;

  totalAgentAdminsRecordLeft: number;
  totalAgentAdminsRecordRight: number;
  searchKeywordLeft: string;
  searchKeywordRight: string;

  agentSchedulingGroupIdLeft?: number;
  agentSchedulingGroupIdRight?: number;
  agentAdmin: AgentAdminResponse;
  agentAdminsLeftSide: AgentAdminDetails[] = [];
  agentAdminsRightSide: AgentAdminDetails[] = [];

  isSelected: boolean[] = [];
  selected: AgentAdminDetails[] = [];
  selectedAgentAdminIds: string[] = [];

  getAgentAdminsSubscriptionLeft: ISubscription;
  getAgentAdminsSubscriptionRight: ISubscription;
  subscriptions: any[] = [];

  constructor(
    private spinnerService: NgxSpinnerService,
    private modalService: NgbModal,
    private agentAdminService: AgentAdminService,
    private authService: AuthService,
  ) {
    this.LoggedUser = this.authService.getLoggedUserInfo();
  }

  ngOnInit() {

  }


    // Pop up messages
    private showErrorWarningPopUpMessage(contentMessage: string) {
      const options: NgbModalOptions = { backdrop: 'static', centered: true, size: 'sm' };
      const modalRef = this.modalService.open(ErrorWarningPopUpComponent, options);
      modalRef.componentInstance.headingMessage = 'Error';
      modalRef.componentInstance.contentMessage = contentMessage;
      modalRef.componentInstance.messageType = ContentType.String;
  
      return modalRef;
    }
  


  private getQueryParamsLeft(schedulingGroupId) {
    const agentAdminQueryParams = new AgentAdminQueryParameter();
    agentAdminQueryParams.pageNumber = this.currentPageLeft;
    agentAdminQueryParams.pageSize = this.pageSizeLeft;
    agentAdminQueryParams.searchKeyword = this.searchKeywordLeft ?? '';
    agentAdminQueryParams.orderBy = `${this.orderByLeft} ${this.sortByLeft}`;
    agentAdminQueryParams.fields = '';
    agentAdminQueryParams.agentSchedulingGroupId = schedulingGroupId
    return agentAdminQueryParams;
  }

  private getQueryParamsRight(schedulingGroupId) {
    const agentAdminQueryParams = new AgentAdminQueryParameter();
    agentAdminQueryParams.pageNumber = this.currentPageRight;
    agentAdminQueryParams.pageSize = this.pageSizeRight;
    agentAdminQueryParams.searchKeyword = this.searchKeywordRight ?? '';
    agentAdminQueryParams.orderBy = `${this.orderByRight} ${this.sortByRight}`;
    agentAdminQueryParams.fields = '';
    agentAdminQueryParams.agentSchedulingGroupId = schedulingGroupId
    return agentAdminQueryParams;
  }

  // LEFT BOX
  changePageSizeLeft(pageSize: number) {
    this.pageSizeLeft = pageSize;
    this.loadAgentAdminsLeft(this.agentSchedulingGroupIdLeft);
  }

  changePageLeft(page: number) {
    this.currentPageLeft = page;
    this.loadAgentAdminsLeft(this.agentSchedulingGroupIdLeft);
  }

  onSchedulingGroupChangeLeft(schedulingGroupId: number) {
    this.agentSchedulingGroupIdLeft = schedulingGroupId;
    if (this.agentSchedulingGroupIdLeft) {
      this.loadAgentAdminsLeft(schedulingGroupId);
    } else {
      this.agentAdminsLeftSide = [];
    }
  }

  searchLeft() {
    this.loadAgentAdminsLeft(this.agentSchedulingGroupIdLeft);
  }

  private loadAgentAdminsLeft(schedulingGroupId) { 
    this.agentSchedulingGroupIdLeft = schedulingGroupId;

    if(this.agentSchedulingGroupIdRight === this.agentSchedulingGroupIdLeft){
      this.showErrorWarningPopUpMessage("Scheduling Group destination should't be the same with the source.");
    }else{
      const queryParams = this.getQueryParamsLeft(schedulingGroupId);
      this.spinnerService.show(this.spinnerLeft, SpinnerOptions);

      this.getAgentAdminsSubscriptionLeft = this.agentAdminService.getAgentAdmins(queryParams)
        .subscribe((response) => {
          if (response.body) {
            this.agentAdminsLeftSide = response.body;
            this.headerPaginationValuesLeft = JSON.parse(response.headers.get('x-pagination'));
            this.totalAgentAdminsRecordLeft = this.headerPaginationValuesLeft.totalCount;
          }
          this.spinnerService.hide(this.spinnerLeft);
        }, (error) => {
          this.spinnerService.hide(this.spinnerLeft);
          console.log(error);
        });

      this.subscriptions.push(this.getAgentAdminsSubscriptionLeft);      
    }
  }

  // END LEFT BOX

    // RIGHT BOX
    changePageSizeRight(pageSize: number) {
      this.pageSizeRight = pageSize;
      this.loadAgentAdminsRight(this.agentSchedulingGroupIdRight);
    }
  
    changePageRight(page: number) {
      this.currentPageRight = page;
      this.loadAgentAdminsRight(this.agentSchedulingGroupIdRight);
    }
  
    onSchedulingGroupChangeRight(schedulingGroupId: number) {
      this.agentSchedulingGroupIdRight = schedulingGroupId;
      if (this.agentSchedulingGroupIdRight) {
        this.loadAgentAdminsRight(schedulingGroupId);
      } else {
        this.agentAdminsRightSide = [];
      }
    }
  
    searchRight() {
      this.loadAgentAdminsRight(this.agentSchedulingGroupIdRight);
    }
  
    private loadAgentAdminsRight(schedulingGroupId) {
      this.agentSchedulingGroupIdRight = schedulingGroupId;

      if(this.agentSchedulingGroupIdRight === this.agentSchedulingGroupIdLeft){
        this.showErrorWarningPopUpMessage("Scheduling Group destination should't be the same with the source.");
      }else{
        const queryParams = this.getQueryParamsRight(schedulingGroupId);
        this.spinnerService.show(this.spinnerRight, SpinnerOptions);
    
        this.getAgentAdminsSubscriptionRight = this.agentAdminService.getAgentAdmins(queryParams)
          .subscribe((response) => {
            if (response.body) {
              this.agentAdminsRightSide = response.body;
              this.headerPaginationValuesRight = JSON.parse(response.headers.get('x-pagination'));
              this.totalAgentAdminsRecordRight = this.headerPaginationValuesRight.totalCount;
            }
            this.spinnerService.hide(this.spinnerRight);
          }, (error) => {
            this.spinnerService.hide(this.spinnerRight);
            console.log(error);
          });
    
        this.subscriptions.push(this.getAgentAdminsSubscriptionRight);
      }
    }
  
    // END RIGHT BOX
  

  toggleSelected(agentAdmin: AgentAdminDetails){
    if(!this.selected.find(x => x.employeeId === agentAdmin.employeeId)){
      this.selected.push(agentAdmin)

      // push the ids to an array
      this.selectedAgentAdminIds.push(agentAdmin.id);
      console.log(this.selectedAgentAdminIds);
    }else{
      const index = this.selected.indexOf(agentAdmin, 0);
      const indexId = this.selectedAgentAdminIds.indexOf(agentAdmin.id,0);
      // remove the ids from the array
      if (indexId > -1 && index > -1) {
        this.selected.splice(index, 1);
        this.selectedAgentAdminIds.splice(indexId, 1);
      }
      console.log(agentAdmin.employeeId)

    }
  }
  

  moveAgents(){
    var moveAgentAdminsParams: MoveAgentAdminParameters = {
      agentAdminIds: this.selectedAgentAdminIds,
      sourceSchedulingGroupId: this.agentSchedulingGroupIdLeft,
      destinationSchedulingGroupId: this.agentSchedulingGroupIdRight,
      modifiedBy: this.LoggedUser.displayName
    }

    this.agentAdminService.moveAgentAdmins(moveAgentAdminsParams).subscribe((data)=>{
      this.isSelected = [];
      this.searchKeywordLeft = '';
      this.searchKeywordRight = '';
      this.loadAgentAdminsRight(this.agentSchedulingGroupIdRight);
      this.loadAgentAdminsLeft(this.agentSchedulingGroupIdLeft);
      this.selected = [];
      this.selectedAgentAdminIds = [];
      this.showSuccessPopUpMessage("The agent(s) has been move");
    },error=>{
      console.log(error)
    });
  }


  private getModalPopup(component: any, size: string) {
    const options: NgbModalOptions = { backdrop: 'static', centered: true, size };
    this.modalRef = this.modalService.open(component, options);
  }

  private setComponentMessages(headingMessage: string, contentMessage: string) {
    this.modalRef.componentInstance.headingMessage = headingMessage;
    this.modalRef.componentInstance.contentMessage = contentMessage;
  }

  private showSuccessPopUpMessage(contentMessage: string, needRefresh = true) {
    this.getModalPopup(MessagePopUpComponent, 'sm');
    this.setComponentMessages('Success', contentMessage);
  }

  confirmDeleteUserPermission() {
    // this.getModalPopup(GenericPopUpComponent, 'md');
    // this.setConfirmDialogMessages(`Are you sure you? You won't be able to revert this!`, ``, `Yes`, `No`);
    this.getModalPopup(GenericPopUpComponent, 'sm');
    this.setComponentMessages('', 'Are you sure you want to move the agent(s)?');
    this.modalRef.componentInstance.warning = true;
    this.modalRef.result.then((result) => {
      if (result && result === true) {
        this.moveAgents();
      }
    });
  }
  
}
