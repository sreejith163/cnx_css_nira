import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { BehaviorSubject } from 'rxjs';
import { HttpBaseService } from 'src/app/core/services/http-base.service';
import { ApiResponseModel } from 'src/app/shared/models/api-response.model';
import { environment } from 'src/environments/environment';
import { MoveAgentAdminParameters } from '../models/move-agent-params.model';
import { catchError, tap } from 'rxjs/operators';
import { AgentAdminDetails } from '../models/agent-admin-details.model';
import { AgentAdminQueryParameter } from '../models/agent-admin-query-parameter.model';
import { NgbModal, NgbModalOptions, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { ErrorWarningPopUpComponent } from 'src/app/shared/popups/error-warning-pop-up/error-warning-pop-up.component';
import { ContentType } from 'src/app/shared/enums/content-type.enum';
import { NgxSpinnerService } from 'ngx-spinner';
import { SpinnerOptions } from 'src/app/shared/util/spinner-options.util';
import { AuthService } from 'src/app/core/services/auth.service';
import { MessagePopUpComponent } from 'src/app/shared/popups/message-pop-up/message-pop-up.component';

@Injectable({
  providedIn: 'root'
})
export class MoveAgentsService extends HttpBaseService {
  modalRef: NgbModalRef;

  private baseURL = '';
  agentAdminsUpdatedSubject$ = new BehaviorSubject<number>(undefined);

  selectedAgentAdminIdsSubject$ = new BehaviorSubject<Array<string>>([]); 
  selectedAgentAdminIds$ = this.selectedAgentAdminIdsSubject$.asObservable();
  selectedAgentAdminIds: string[] = [];

  // left side
  agentSchedulingGroupIdLeftSubject$ = new BehaviorSubject<number>(undefined);
  agentSchedulingGroupIdLeft$ = this.agentSchedulingGroupIdLeftSubject$.asObservable();

  agentSchedulingGroupIdLeft?: number;
  agentAdminsLeft: Array<AgentAdminDetails> = [];
  agentAdminsSubjectLeft$ = new BehaviorSubject<Array<AgentAdminDetails>>([]);
  agentAdminsLeft$ = this.agentAdminsSubjectLeft$.asObservable();
  allAgentAdminsLeft: Array<AgentAdminDetails> = [];
  orderByLeft = 'employeeId';
  sortByLeft = 'desc';
  pageNumberLeft = 1;
  totalLeftItems = 0;
  totalAgentAdminsSubjectLeft$ = new BehaviorSubject<number>(0);
  totalAgentAdminsLeft$ = this.totalAgentAdminsSubjectLeft$.asObservable();
  totalLeftPages: number;

  // right side
  agentSchedulingGroupIdRightSubject$ = new BehaviorSubject<number>(undefined);
  agentSchedulingGroupIdRight$ = this.agentSchedulingGroupIdRightSubject$.asObservable();

  agentSchedulingGroupIdRight?: number;
  agentAdminsRight: Array<AgentAdminDetails> = [];
  agentAdminsSubjectRight$ = new BehaviorSubject<Array<AgentAdminDetails>>([]);
  agentAdminsRight$ = this.agentAdminsSubjectRight$.asObservable();
  allAgentAdminsRight: Array<AgentAdminDetails> = [];
  orderByRight = 'employeeId';
  sortByRight = 'desc';
  pageNumberRight = 1;
  totalRightItems = 0;
  totalAgentAdminsSubjectRight$ = new BehaviorSubject<number>(0);
  totalAgentAdminsRight$ = this.totalAgentAdminsSubjectRight$.asObservable();
  totalRightPages: number;

  agentListSpinnerLeft = "agentListSpinnerLeft";
  agentListSpinnerRight = "agentListSpinnerRight";

  constructor(
    private modalService: NgbModal,
    private http: HttpClient,
    private spinnerService: NgxSpinnerService,
    private authService: AuthService
  ) {
    super();
    this.baseURL = environment.services.gatewayService;
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

  // left side agent admins //
  unloadAgentAdminsLeft(){
    this.agentSchedulingGroupIdLeft = 0;
    this.agentAdminsLeft = [];
    this.agentAdminsSubjectLeft$.next(null);
  }

  loadAgentAdminsLeft(schedulingGroupId, searchKeyword?){
    if(this.agentSchedulingGroupIdLeft !== schedulingGroupId){
      this.agentSchedulingGroupIdLeft = schedulingGroupId;
      this.agentSchedulingGroupIdLeftSubject$.next(this.agentSchedulingGroupIdLeft);
      this.agentAdminsLeft = [];
      this.agentAdminsSubjectLeft$.next(null);
    }

    if(this.agentSchedulingGroupIdLeft === this.agentSchedulingGroupIdRight){
      this.agentSchedulingGroupIdLeft = 0;
      this.agentSchedulingGroupIdLeftSubject$.next(undefined);
      this.showErrorWarningPopUpMessage("Scheduling Group destination shouldn't be the same with the source.");
    }else{
      const agentAdminQueryParams = new AgentAdminQueryParameter();
      agentAdminQueryParams.orderBy = `${this.orderByLeft} ${this.sortByLeft}`;
      agentAdminQueryParams.agentSchedulingGroupId = schedulingGroupId
      agentAdminQueryParams.pageNumber = this.pageNumberLeft;
      agentAdminQueryParams.skipPageSize = true;
      agentAdminQueryParams.searchKeyword = searchKeyword ? searchKeyword : '';

      this.spinnerService.show(this.agentListSpinnerLeft, SpinnerOptions);
      
      this.getAgentAdminsLeft(agentAdminQueryParams)
        .subscribe((response) => {
          if (response.body) {

              this.totalAgentAdminsSubjectLeft$.next(response.body.length);

              this.allAgentAdminsLeft = response.body;
              
              this.getNextItemsLeft();
              this.agentAdminsSubjectLeft$.next(this.agentAdminsLeft);
              this.spinnerService.hide(this.agentListSpinnerLeft);
          }
        }, (error) => {
          this.spinnerService.hide(this.agentListSpinnerLeft);
          console.log(error);
        });
    }
  }

  getAgentAdminsLeft(agentAdminsQueryParams: AgentAdminQueryParameter) {
    const url = `${this.baseURL}/agentadmins`;
    return this.http.get<AgentAdminDetails>(url, {
      params: this.convertToHttpParam(agentAdminsQueryParams),
      observe: 'response'
    }).pipe(catchError(this.handleError));
  }

  loadMoreLeft(): void {
    if (this.getNextItemsLeft()) {
      this.agentAdminsSubjectLeft$.next(this.agentAdminsLeft);
    }
  }

  getNextItemsLeft(): boolean {
    if (this.agentAdminsLeft.length >= this.allAgentAdminsLeft.length) {
      return false;
    }
    const remainingLength = Math.min(50, this.allAgentAdminsLeft.length - this.agentAdminsLeft.length);
    this.agentAdminsLeft.push(...this.allAgentAdminsLeft.slice(this.agentAdminsLeft.length, this.agentAdminsLeft.length + remainingLength));
    return true;
  }

  // end of left side agent admins //

  // right side agent admins //
  unloadAgentAdminsRight(){
    this.agentSchedulingGroupIdRight = 0;
    this.agentAdminsRight = [];
    this.agentAdminsSubjectRight$.next(null);
  }

  loadAgentAdminsRight(schedulingGroupId, searchKeyword?){
    if(this.agentSchedulingGroupIdRight !== schedulingGroupId){
      this.agentSchedulingGroupIdRight = schedulingGroupId;
      this.agentSchedulingGroupIdRightSubject$.next(this.agentSchedulingGroupIdRight);
      this.agentAdminsRight = [];
      this.agentAdminsSubjectRight$.next(null);
    }
    if(this.agentSchedulingGroupIdRight === this.agentSchedulingGroupIdLeft){
      this.agentSchedulingGroupIdRight = 0;
      this.agentSchedulingGroupIdRightSubject$.next(undefined);
      this.showErrorWarningPopUpMessage("Scheduling Group destination shouldn't be the same with the source.");
    }else{
      const agentAdminQueryParams = new AgentAdminQueryParameter();
      agentAdminQueryParams.orderBy = `${this.orderByRight} ${this.sortByRight}`;
      agentAdminQueryParams.agentSchedulingGroupId = schedulingGroupId;
      agentAdminQueryParams.skipPageSize = true;
      agentAdminQueryParams.searchKeyword = searchKeyword ? searchKeyword : '';
      
      this.spinnerService.show(this.agentListSpinnerRight, SpinnerOptions);
      
      this.getAgentAdminsRight(agentAdminQueryParams)
        .subscribe((response) => {
          if (response.body) {
            this.totalAgentAdminsSubjectRight$.next(response.body.length);

            this.allAgentAdminsRight = response.body;
            
            this.getNextItemsRight();
            this.agentAdminsSubjectRight$.next(this.agentAdminsRight);            
            this.spinnerService.hide(this.agentListSpinnerRight);
          }
        }, (error) => {
          this.spinnerService.hide(this.agentListSpinnerRight);
          console.log(error);
        });
    }
  }


  getAgentAdminsRight(agentAdminsQueryParams: AgentAdminQueryParameter) {
    const url = `${this.baseURL}/agentadmins`;

    return this.http.get<AgentAdminDetails>(url, {
      params: this.convertToHttpParam(agentAdminsQueryParams),
      observe: 'response'
    }).pipe(catchError(this.handleError));
  }

  loadMoreRight(): void {
    if (this.getNextItemsRight()) {
      this.agentAdminsSubjectRight$.next(this.agentAdminsRight);
    }
  }

  getNextItemsRight(): boolean {
    if (this.agentAdminsRight.length >= this.allAgentAdminsRight.length) {
      return false;
    }
    const remainingLength = Math.min(50, this.allAgentAdminsRight.length - this.agentAdminsRight.length);
    this.agentAdminsRight.push(...this.allAgentAdminsRight.slice(this.agentAdminsRight.length, this.agentAdminsRight.length + remainingLength));
    return true;
  }

  // end of right side agent admins //


  moveAgents(){
    this.selectedAgentAdminIds$.subscribe((agentIds:string[])=>{
      this.selectedAgentAdminIds = agentIds;
    });

    var moveAgentAdminsParams: MoveAgentAdminParameters = {
      agentAdminIds: this.selectedAgentAdminIds,
      sourceSchedulingGroupId: this.agentSchedulingGroupIdLeft,
      destinationSchedulingGroupId: this.agentSchedulingGroupIdRight,
      modifiedBy: this.authService.getLoggedUserInfo().displayName
    }

    this.moveAgentAdmins(moveAgentAdminsParams).subscribe((data)=>{
      this.loadAgentAdminsRight(this.agentSchedulingGroupIdRight);
      this.loadAgentAdminsLeft(this.agentSchedulingGroupIdLeft);
      var msg;
      if(moveAgentAdminsParams.agentAdminIds.length > 1){
          msg = "The agents have been move";       
      }else if(moveAgentAdminsParams.agentAdminIds.length == 1){
          msg = "The agent has been move"; 
      }
      this.showSuccessPopUpMessage(msg);
      this.selectedAgentAdminIds = [];
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


  moveAgentAdmins(moveAgentAdminsParams: MoveAgentAdminParameters){
    const url = `${this.baseURL}/agentadmins/move`;
    return this.http.put<ApiResponseModel>(url, moveAgentAdminsParams)
      .pipe(
        tap(() => {
          this.agentAdminsUpdatedSubject$.next(1);
        }),
        catchError(this.handleError));
  }

}
