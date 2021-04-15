import { DatePipe } from '@angular/common';
import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { NgbActiveModal, NgbCalendar, NgbModal, NgbModalOptions, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { TranslateService } from '@ngx-translate/core';
import * as moment from 'moment';
import { NgxSpinnerService } from 'ngx-spinner';
import { SubscriptionLike as ISubscription } from 'rxjs';
import { AuthService } from 'src/app/core/services/auth.service';
import { ComponentOperation } from 'src/app/shared/enums/component-operation.enum';
import { HeaderPagination } from 'src/app/shared/models/header-pagination.model';
import { Constants } from 'src/app/shared/util/constants.util';
import { SpinnerOptions } from 'src/app/shared/util/spinner-options.util';
import { ActivityOrigin } from '../../../enums/activity-origin.enum';
import { AgentScheduleType } from '../../../enums/agent-schedule-type.enum';
import { SchedulingStatus } from '../../../enums/scheduling-status.enum';
import { AgentScheduleManagersQueryParams } from '../../../models/agent-schedule-mangers-query-params.model';
import { AgentScheduleRange } from '../../../models/agent-schedule-range.model';
import { AgentSchedulesQueryParams } from '../../../models/agent-schedules-query-params.model';
import { AgentSchedulesResponse } from '../../../models/agent-schedules-response.model';
import { CopyMultipleAgentScheduleChart } from '../../../models/copy-multiple-agent-schedule-chart.model';
import { CopyAgentScheduleManagerChart } from '../../../models/copy-agent-schedule-manager-chart.model';
import { ScheduleDateRangeBase } from '../../../models/schedule-date-range-base.model';
import { SchedulingAgents } from '../../../models/scheduling-agents.model';
import { AgentScheduleManagersService } from '../../../services/agent-schedule-managers.service';
import { AgentSchedulesService } from '../../../services/agent-schedules.service';
import { CopyScheduleDateRangeComponent } from '../copy-schedule-date-range/copy-schedule-date-range.component';
import { DateRangePopUpComponent } from '../date-range-pop-up/date-range-pop-up.component';
import { CopyScheduleOperation } from 'src/app/shared/enums/copy-schedule-operation.enum';
import { ContentType } from 'src/app/shared/enums/content-type.enum';
import { ErrorWarningPopUpComponent } from 'src/app/shared/popups/error-warning-pop-up/error-warning-pop-up.component';

@Component({
  selector: 'app-copy-schedule',
  templateUrl: './copy-schedule.component.html',
  styleUrls: ['./copy-schedule.component.scss'],
  providers: [DatePipe]
})
export class CopyScheduleComponent implements OnInit, OnDestroy {
  activeTab = 1;
  copyOperation = CopyScheduleOperation;
  modalRef: NgbModalRef;
  targetDateRangesOfOtherAgents: ScheduleDateRangeBase[] = [];
  targetDateRangesIndividual: ScheduleDateRangeBase[] = [];

  totalAgents: number;
  employeeAgentSchedulingGroupId: number;
  currentPage = 1;
  pageSize = 10;
  characterSplice = 25;
  spinner = 'Copy-Schedule';
  orderBy = 'employeeId';
  sortBy = 'asc';
  formSubmitted: boolean;
  masterSelected: boolean;
  checkAll: boolean;

  agents: SchedulingAgents[] = [];
  checkedAgents: SchedulingAgents[] = [];

  paginationSize = Constants.paginationSize;

  copyAgentScheduleManagerChartSubscription: ISubscription;
  getAgentScheduleManagersSubscription: ISubscription;
  copyAgentScheduleChartSubscription: ISubscription;
  getAgentsSubscription: ISubscription;
  subscriptions: ISubscription[] = [];

  @Input() agentSchedulingGroupId: number;
  @Input() employeeId: string;
  @Input() agentScheduleId: string;
  @Input() agentScheduleType: AgentScheduleType;
  @Input() fromDate: Date;
  @Input() dateFrom: Date;
  @Input() dateTo: Date;

  constructor(
    private calendar: NgbCalendar,
    private modalService: NgbModal,
    public translate: TranslateService,
    public activeModal: NgbActiveModal,
    private spinnerService: NgxSpinnerService,
    private agentSchedulesService: AgentSchedulesService,
    private agentScheduleManagerService: AgentScheduleManagersService,
    private authService: AuthService,
    private datepipe: DatePipe,
  ) { }

  ngOnInit(): void {
    this.loadSchedulingAgents();
    this.employeeAgentSchedulingGroupId = this.agentSchedulingGroupId;
  }

  ngOnDestroy() {
    this.subscriptions.forEach(subscription => {
      if (subscription) {
        subscription.unsubscribe();
      }
    });

    this.targetDateRangesIndividual = [];
    this.targetDateRangesOfOtherAgents = [];
    this.checkedAgents = [];
  }

  sort(columnName: string, sortBy: string) {
    if (this.agentSchedulingGroupId) {
      this.sortBy = sortBy === 'asc' ? 'desc' : 'asc';
      this.orderBy = columnName;

      this.loadSchedulingAgents();
    }
  }

  hasEmployeeSelected() {
    return this.checkedAgents.length > 0 || this.masterSelected;
  }

  onSchedulingGroupChange(schedulingGroupId: number) {
   
    this.masterSelected = false;
    this.checkAll = false;
    if (schedulingGroupId) {
      this.checkedAgents = [];
      this.agentSchedulingGroupId = +schedulingGroupId;
      this.agentScheduleType === AgentScheduleType.Scheduling ? this.loadSchedulingAgents() : this.loadManagerAgents();
     
    } else {
      this.agentSchedulingGroupId = undefined;
      this.agents = [];
    }
 
  }

  changePageSize(pageSize: number) {
    this.pageSize = pageSize;
    this.agentScheduleType === AgentScheduleType.Scheduling ? this.loadSchedulingAgents() : this.loadManagerAgents();
  }

  changePage(page: number) {
    this.currentPage = page;
    this.agentScheduleType === AgentScheduleType.Scheduling ? this.loadSchedulingAgents() : this.loadManagerAgents();
  }

  checkUncheckAll(e) {
    for (const agent of this.agents) {
      agent.isChecked = this.masterSelected;
    }
    this.checkedAgents = this.agents.filter(x => x.isChecked === true);
    this.checkAll = e.target.checked ? true : false;
  }

  isAllSelected(e) {
    if (e.target.checked) {
      const index = this.checkedAgents.findIndex(x => x.employeeId === e.target.value);
      if (index !== -1) {
        this.checkedAgents[index].isChecked = true;
      } else {
        this.checkedAgents.push(this.agents.find(x => x.employeeId === e.target.value));
      }
    } else {
      const index = this.checkedAgents.findIndex(x => x.employeeId === e.target.value);
      if (index !== -1) {
        this.checkedAgents[index].isChecked = false;
      }
    }
    if (this.checkedAgents.filter(x => x.isChecked === true).length === this.totalAgents) {
      this.masterSelected = true;
    } else {
      this.masterSelected = false;
    }

  }

  copySchedule(operation:CopyScheduleOperation) {
    this.formSubmitted = true;
    if(operation == CopyScheduleOperation.Other){
      const copiedAgents = [];
      if(this.targetDateRangesOfOtherAgents.length > 0){
        if (this.checkedAgents.length > 0 || this.masterSelected && this.agentSchedulingGroupId) {
          if (this.checkedAgents.length > 0 && this.checkedAgents.filter(x => x.isChecked === true).length !== this.totalAgents) {
            // if not all are checked/selected, push the selected ids to an array and process
            for (const item of this.checkedAgents.filter(x => x.isChecked === true)) {
              copiedAgents.push(item.employeeId);
            }            
          }
          this.copyAgentSchedule(copiedAgents, this.targetDateRangesOfOtherAgents);
        }
      }else{
        this.getModalPopup(ErrorWarningPopUpComponent, 'sm', "Please add Date Range(s) first.");
      }
    }else if(operation == CopyScheduleOperation.Individual){
      const employeeId = [];
      if(this.targetDateRangesIndividual.length > 0){
        employeeId.push(this.employeeId);
        this.copyAgentSchedule(employeeId, this.targetDateRangesIndividual);
      }else{
        this.getModalPopup(ErrorWarningPopUpComponent, 'sm', "Please add Date Range(s) first.");
      }
    }

  }

  private getSchedulingQueryParams() {
    const agentSchedulesQueryParams = new AgentSchedulesQueryParams();
    agentSchedulesQueryParams.agentSchedulingGroupId = this.agentSchedulingGroupId;
    agentSchedulesQueryParams.excludeConflictSchedule = true;
    // agentSchedulesQueryParams.dateFrom = this.getDateInStringFormat(this.dateFrom);
    // agentSchedulesQueryParams.dateTo = this.getDateInStringFormat(this.dateTo);
    agentSchedulesQueryParams.pageNumber = this.currentPage;
    agentSchedulesQueryParams.pageSize = this.pageSize;
    agentSchedulesQueryParams.orderBy = `${this.orderBy} ${this.sortBy}`;
    agentSchedulesQueryParams.fields = 'employeeId,firstName,lastName';
    agentSchedulesQueryParams.excludedEmployeeId = this.employeeId ? this.employeeId : undefined;
    return agentSchedulesQueryParams;
  }

  private getManagerQueryParams() {
    const agentScheduleManagerQueryParams = new AgentScheduleManagersQueryParams();
    agentScheduleManagerQueryParams.agentSchedulingGroupId = this.agentSchedulingGroupId;
    agentScheduleManagerQueryParams.excludeConflictSchedule = true;
    agentScheduleManagerQueryParams.date = this.getDateInStringFormat(this.fromDate);
    agentScheduleManagerQueryParams.pageNumber = this.currentPage;
    agentScheduleManagerQueryParams.pageSize = this.pageSize;
    agentScheduleManagerQueryParams.orderBy = `${this.orderBy} ${this.sortBy}`;
    agentScheduleManagerQueryParams.fields = 'employeeId,firstName,lastName';

    return agentScheduleManagerQueryParams;
  }

  private getDateInStringFormat(fromDate: any): string {
    if (!fromDate) {
      return undefined;
    }

    const date = new Date(fromDate);
    return date.toDateString();
  }

  private loadSchedulingAgents() {
    const queryParams = this.getSchedulingQueryParams();
    this.spinnerService.show(this.spinner, SpinnerOptions);

    this.getAgentsSubscription = this.agentSchedulesService.getAgentSchedules(queryParams)
      .subscribe((response) => {
        this.agents = response.body;
        
        let headerPaginationValues = new HeaderPagination();
        headerPaginationValues = JSON.parse(response.headers.get('x-pagination'));
        this.totalAgents = headerPaginationValues.totalCount;
        this.showSelectedEmployees();
        this.spinnerService.hide(this.spinner);
      }, (error) => {
        this.spinnerService.hide(this.spinner);
      });

    this.subscriptions.push(this.getAgentsSubscription);
  }

  private loadManagerAgents() {
    const queryParams = this.getManagerQueryParams();
    this.spinnerService.show(this.spinner, SpinnerOptions);

    this.getAgentScheduleManagersSubscription = this.agentScheduleManagerService.getAgentScheduleManagers(queryParams)
      .subscribe((response) => {
        this.agents = response.body;
        let headerPaginationValues = new HeaderPagination();
        headerPaginationValues = JSON.parse(response.headers.get('x-pagination'));
        this.totalAgents = +headerPaginationValues.totalCount;
        this.showSelectedEmployees();
        this.spinnerService.hide(this.spinner);
      }, (error) => {
        this.spinnerService.hide(this.spinner);
      });

    this.subscriptions.push(this.getAgentScheduleManagersSubscription);
  }

  private showSelectedEmployees() {
    if (this.masterSelected || this.checkAll && this.checkedAgents.length === 0 ||
      this.checkedAgents.filter(x => x.isChecked === true).length === this.totalAgents) {
      this.agents.forEach(ele => {
        ele.isChecked = true;
      });
    }
    else if (!this.masterSelected && this.checkAll) {
      const distinct = this.checkedAgents;
      this.agents.forEach(ele => {
        if (distinct.findIndex(x => x.employeeId === ele.employeeId) === -1) {
          ele.isChecked = true;
          this.checkedAgents.push(ele);
        }
      });
      if (this.checkedAgents.length > 0) {
        this.checkedAgents.forEach(ele => {
          if (ele.isChecked === true) {
            const index = this.agents.findIndex(x => x.employeeId === ele.employeeId);
            if (index !== -1) {
              this.agents[index].isChecked = true;
            }
          }
        });
      }
    } else {
      if (this.checkedAgents.length > 0) {
        this.checkedAgents.forEach(ele => {
          if (ele.isChecked === true) {
            const index = this.agents.findIndex(x => x.employeeId === ele.employeeId);
            if (index !== -1) {
              this.agents[index].isChecked = true;
            }
          }
        });
      }
    }
  }

  private copyAgentSchedule(copiedAgents: Array<number>, targetDateRangesOfOtherAgents: ScheduleDateRangeBase[]) {
    console.log(copiedAgents)
    this.spinnerService.show(this.spinner, SpinnerOptions);
    const copyData = new CopyMultipleAgentScheduleChart();
    copyData.agentSchedulingGroupId = this.agentSchedulingGroupId;
    copyData.activityOrigin = ActivityOrigin.CSS;
    copyData.modifiedUser = +this.authService.getLoggedUserInfo()?.employeeId;
    copyData.modifiedBy = this.authService.getLoggedUserInfo()?.displayName;
    // if all agents are selected, empty the employee ids array to process it on the backend
    copyData.employeeIds = this.masterSelected ? [] : copiedAgents;
    copyData.dateFrom = this.getFormattedDate(this.dateFrom);
    copyData.dateTo = this.getFormattedDate(this.dateTo);
    copyData.selectedDateRanges = targetDateRangesOfOtherAgents;

    this.copyAgentScheduleChartSubscription = this.agentSchedulesService.copyMultipleAgentScheduleChart(this.agentScheduleId, copyData)
      .subscribe(() => {
        this.spinnerService.hide(this.spinner);
        this.activeModal.close({ needRefresh: true });
      }, (error) => {
        this.spinnerService.hide(this.spinner);
        console.log(error);
      });

    this.subscriptions.push(this.copyAgentScheduleChartSubscription);
  }

  private getCalendarPopup(component: any, size: string) {
    const options: NgbModalOptions = { backdrop: 'static', centered: true, size };
    this.modalRef = this.modalService.open(component, options);
  }


  addDateRange(operation:CopyScheduleOperation) {
    if(operation == CopyScheduleOperation.Other){
      this.getCalendarPopup(CopyScheduleDateRangeComponent, 'sm');
      this.modalRef.componentInstance.operation = ComponentOperation.Add;
      this.modalRef.componentInstance.isEditNewDateRange = false;
      this.modalRef.result.then((result: ScheduleDateRangeBase) => {
        if(result){
          // check if date range already exists in the list
          var isExisting = this.targetDateRangesOfOtherAgents.find(x => this.getTimeStamp(x.dateFrom) >= this.getTimeStamp(result.dateFrom) && this.getTimeStamp(x.dateTo) <= this.getTimeStamp(result.dateTo));
          if(isExisting !== undefined){
            this.getModalPopup(ErrorWarningPopUpComponent, 'sm', "Date Range is already in list.");
          }else{
            result.dateFrom = this.getFormattedDate(result.dateFrom);
            result.dateTo = this.getFormattedDate(result.dateTo);
            this.targetDateRangesOfOtherAgents.push(result);
          }          
        }
      });
    }else if(operation == CopyScheduleOperation.Individual){
      this.getCalendarPopup(CopyScheduleDateRangeComponent, 'sm');
      this.modalRef.componentInstance.operation = ComponentOperation.Add;
      this.modalRef.componentInstance.isEditNewDateRange = false;
      this.modalRef.result.then((result: ScheduleDateRangeBase) => {
        if(result){
          // check if date range already exists in the list
          var isExisting = this.targetDateRangesIndividual.find(x => this.getTimeStamp(x.dateFrom) >= this.getTimeStamp(result.dateFrom) && this.getTimeStamp(x.dateTo) <= this.getTimeStamp(result.dateTo));
          if(isExisting !== undefined){
            this.getModalPopup(ErrorWarningPopUpComponent, 'sm', "Date Range is already in list.");
          }else{
            result.dateFrom = this.getFormattedDate(result.dateFrom);
            result.dateTo = this.getFormattedDate(result.dateTo);
            this.targetDateRangesIndividual.push(result);
          }          
        }
      });
    }

  }

  private changeToUTCDate(date){
    return new Date(new Date(date).toString().replace(/\sGMT.*$/, " GMT+0000"));
  }

  getTimeStamp(date: any){
    if(date){
      date = this.changeToUTCDate(date)?.getTime()
    }
    return date;
  }

  deleteTargetRangeOfOtherAgents(targetRange){
    const indexId = this.targetDateRangesOfOtherAgents.indexOf(targetRange,0);
    // remove the ids from the array
    if (indexId > -1 ) {
      this.targetDateRangesOfOtherAgents.splice(indexId, 1);
    }
  }

  deleteTargetRangeIndividual(targetRange){
    const indexId = this.targetDateRangesIndividual.indexOf(targetRange,0);
    // remove the ids from the array
    if (indexId > -1 ) {
      this.targetDateRangesIndividual.splice(indexId, 1);
    }
  }

  // private addDateRangeToList(el: AgentSchedulesResponse, result: ScheduleDateRangeBase) {
  //   const range = new AgentScheduleRange();
  //   range.dateFrom = this.getFormattedDate(result?.dateFrom);
  //   range.dateTo = this.getFormattedDate(result?.dateTo);
  //   console.log(range)
  //   range.status = SchedulingStatus['Pending Schedule'];
  //   range.agentSchedulingGroupId = el.activeAgentSchedulingGroupId;
  //   range.scheduleCharts = [];
  //   el.ranges.push(range);
  //   el.rangeIndex = el.ranges.length - 1;
  //   el.modifiedBy = this.authService.getLoggedUserInfo().displayName;
  //   el.modifiedDate = new Date();
  // }

  // private copyAgentScheduleManager(copiedAgents: Array<number>) {
  //   this.spinnerService.show(this.spinner, SpinnerOptions);
  //   const copyData = new CopyAgentScheduleManagerChart();
  //   copyData.agentSchedulingGroupId = this.agentSchedulingGroupId;
  //   copyData.date = this.getFormattedDate(this.fromDate);
  //   copyData.activityOrigin = ActivityOrigin.CSS;
  //   copyData.modifiedUser = +this.authService.getLoggedUserInfo()?.employeeId;
  //   copyData.modifiedBy = this.authService.getLoggedUserInfo()?.displayName;
  //   copyData.employeeIds = this.masterSelected ? [] : copiedAgents;

  //   this.copyAgentScheduleManagerChartSubscription = this.agentScheduleManagerService
  //     .copyAgentScheduleManagerChart(this.employeeId, copyData)
  //     .subscribe(() => {
  //       this.spinnerService.hide(this.spinner);
  //       this.activeModal.close({ needRefresh: true });
  //     }, (error) => {
  //       this.spinnerService.hide(this.spinner);
  //       console.log(error);
  //     });

  //   this.subscriptions.push(this.copyAgentScheduleManagerChartSubscription);
  // }

  loadTabContent(){
    // call the methods for loading data based on what the active tab is
    switch(this.activeTab) { 
      case 1: { 
        this.targetDateRangesOfOtherAgents = [];
        break; 
      } 
      case 2: { 
        this.targetDateRangesIndividual = [];
        break; 
      }
      
      default: { 
        break; 
      } 
    } 
  }

  private getFormattedDate(date: Date) {
    const transformedDate = this.datepipe.transform(date, 'yyyy-MM-dd');
    return new Date(transformedDate);
  }

  private getModalPopup(component: any, size: string, contentMessage?: string) {
    const options: NgbModalOptions = { backdrop: 'static', centered: true, size };
    this.modalRef = this.modalService.open(component, options);
    this.modalRef.componentInstance.headingMessage = 'Error';
    this.modalRef.componentInstance.contentMessage = contentMessage;
    this.modalRef.componentInstance.messageType = ContentType.String;
  }

}


