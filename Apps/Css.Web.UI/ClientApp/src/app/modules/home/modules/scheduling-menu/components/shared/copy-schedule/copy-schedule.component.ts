import { DatePipe } from '@angular/common';
import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { TranslateService } from '@ngx-translate/core';
import { NgxSpinnerService } from 'ngx-spinner';
import { SubscriptionLike as ISubscription } from 'rxjs';
import { AuthService } from 'src/app/core/services/auth.service';
import { HeaderPagination } from 'src/app/shared/models/header-pagination.model';
import { Constants } from 'src/app/shared/util/constants.util';
import { SpinnerOptions } from 'src/app/shared/util/spinner-options.util';
import { ActivityOrigin } from '../../../enums/activity-origin.enum';
import { AgentScheduleType } from '../../../enums/agent-schedule-type.enum';
import { AgentScheduleManagersQueryParams } from '../../../models/agent-schedule-mangers-query-params.model';
import { AgentSchedulesQueryParams } from '../../../models/agent-schedules-query-params.model';
import { CopyAgentScheduleChart } from '../../../models/copy-agent-schedule-chart.model';
import { CopyAgentScheduleManagerChart } from '../../../models/copy-agent-schedule-manager-chart.model';
import { SchedulingAgents } from '../../../models/scheduling-agents.model';
import { AgentScheduleManagersService } from '../../../services/agent-schedule-managers.service';
import { AgentSchedulesService } from '../../../services/agent-schedules.service';

@Component({
  selector: 'app-copy-schedule',
  templateUrl: './copy-schedule.component.html',
  styleUrls: ['./copy-schedule.component.scss'],
  providers: [DatePipe]
})
export class CopyScheduleComponent implements OnInit, OnDestroy {

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
  @Input() employeeId: number;
  @Input() agentScheduleId: string;
  @Input() agentScheduleType: AgentScheduleType;
  @Input() fromDate: Date;
  @Input() dateFrom: Date;
  @Input() dateTo: Date;

  constructor(
    public translate: TranslateService,
    public activeModal: NgbActiveModal,
    private spinnerService: NgxSpinnerService,
    private agentSchedulesService: AgentSchedulesService,
    private agentScheduleManagerService: AgentScheduleManagersService,
    private authService: AuthService,
    private datepipe: DatePipe,
  ) { }

  ngOnInit(): void {
    this.agentScheduleType === AgentScheduleType.Scheduling ? this.loadSchedulingAgents() : this.loadManagerAgents();
    this.employeeAgentSchedulingGroupId = this.agentSchedulingGroupId;
  }

  ngOnDestroy() {
    this.subscriptions.forEach(subscription => {
      if (subscription) {
        subscription.unsubscribe();
      }
    });
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
      const index = this.checkedAgents.findIndex(x => x.employeeId === +e.target.value);
      if (index !== -1) {
        this.checkedAgents[index].isChecked = true;
      } else {
        this.checkedAgents.push(this.agents.find(x => x.employeeId === +e.target.value));
      }
    } else {
      const index = this.checkedAgents.findIndex(x => x.employeeId === +e.target.value);
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

  copySchedule() {
    this.formSubmitted = true;
    const copiedAgents = [];
    if (this.checkedAgents.length > 0 || this.masterSelected && this.agentSchedulingGroupId) {
      if (this.checkedAgents.length > 0 && this.checkedAgents.filter(x => x.isChecked === true).length !== this.totalAgents) {
        for (const item of this.checkedAgents.filter(x => x.isChecked === true)) {
          copiedAgents.push(item.employeeId);
        }
      }
      this.agentScheduleType === AgentScheduleType.Scheduling ?
        this.copyAgentSchedule(copiedAgents) : this.copyAgentScheduleManager(copiedAgents);
    }

  }

  private getSchedulingQueryParams() {
    const agentSchedulesQueryParams = new AgentSchedulesQueryParams();
    agentSchedulesQueryParams.agentSchedulingGroupId = this.agentSchedulingGroupId;
    agentSchedulesQueryParams.excludeConflictSchedule = true;
    agentSchedulesQueryParams.fromDate = this.getDateInStringFormat(this.fromDate);
    agentSchedulesQueryParams.pageNumber = this.currentPage;
    agentSchedulesQueryParams.pageSize = this.pageSize;
    agentSchedulesQueryParams.orderBy = `${this.orderBy} ${this.sortBy}`;
    agentSchedulesQueryParams.fields = 'employeeId,firstName,lastName';

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
        const index = this.agents.findIndex(x => x.employeeId === this.employeeId);
        if (index > -1) {
          this.agents.splice(index, 1);
        }
        this.employeeAgentSchedulingGroupId === this.agentSchedulingGroupId ?
          this.totalAgents = +headerPaginationValues.totalCount - 1 : this.totalAgents = +headerPaginationValues.totalCount;
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
        const index = this.agents.findIndex(x => x.employeeId === this.employeeId);
        if (index > -1) {
          this.agents.splice(index, 1);
        }
        this.employeeAgentSchedulingGroupId === this.agentSchedulingGroupId ?
          this.totalAgents = +headerPaginationValues.totalCount - 1 : this.totalAgents = +headerPaginationValues.totalCount;
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

  private copyAgentSchedule(copiedAgents: Array<number>) {
    this.spinnerService.show(this.spinner, SpinnerOptions);
    const copyData = new CopyAgentScheduleChart();
    copyData.agentSchedulingGroupId = this.agentSchedulingGroupId;
    copyData.activityOrigin = ActivityOrigin.CSS;
    copyData.modifiedUser = +this.authService.getLoggedUserInfo()?.employeeId;
    copyData.modifiedBy = this.authService.getLoggedUserInfo()?.displayName;
    copyData.employeeIds = this.masterSelected ? [] : copiedAgents;
    copyData.dateFrom = this.getFormattedDate(this.dateFrom);
    copyData.dateTo = this.getFormattedDate(this.dateTo);

    this.copyAgentScheduleChartSubscription = this.agentSchedulesService.copyAgentScheduleChart(this.agentScheduleId, copyData)
      .subscribe(() => {
        this.spinnerService.hide(this.spinner);
        this.activeModal.close({ needRefresh: true });
      }, (error) => {
        this.spinnerService.hide(this.spinner);
        console.log(error);
      });

    this.subscriptions.push(this.copyAgentScheduleChartSubscription);
  }

  private copyAgentScheduleManager(copiedAgents: Array<number>) {
    this.spinnerService.show(this.spinner, SpinnerOptions);
    const copyData = new CopyAgentScheduleManagerChart();
    copyData.agentSchedulingGroupId = this.agentSchedulingGroupId;
    copyData.date = this.getFormattedDate(this.fromDate);
    copyData.activityOrigin = ActivityOrigin.CSS;
    copyData.modifiedUser = +this.authService.getLoggedUserInfo()?.employeeId;
    copyData.modifiedBy = this.authService.getLoggedUserInfo()?.displayName;
    copyData.employeeIds = this.masterSelected ? [] : copiedAgents;

    this.copyAgentScheduleManagerChartSubscription = this.agentScheduleManagerService
      .copyAgentScheduleManagerChart(this.employeeId, copyData)
      .subscribe(() => {
        this.spinnerService.hide(this.spinner);
        this.activeModal.close({ needRefresh: true });
      }, (error) => {
        this.spinnerService.hide(this.spinner);
        console.log(error);
      });

    this.subscriptions.push(this.copyAgentScheduleManagerChartSubscription);
  }

  private getFormattedDate(date: Date) {
    const transformedDate = this.datepipe.transform(date, 'yyyy-MM-dd');
    return new Date(transformedDate);
  }
}


