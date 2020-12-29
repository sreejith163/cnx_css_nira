import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { NgxSpinnerService } from 'ngx-spinner';
import { SubscriptionLike as ISubscription } from 'rxjs';
import { AuthService } from 'src/app/core/services/auth.service';
import { HeaderPagination } from 'src/app/shared/models/header-pagination.model';
import { TranslationDetails } from 'src/app/shared/models/translation-details.model';
import { Constants } from 'src/app/shared/util/constants.util';
import { SpinnerOptions } from 'src/app/shared/util/spinner-options.util';
import { AgentScheduleType } from '../../../enums/agent-schedule-type.enum';
import { AgentSchedulesQueryParams } from '../../../models/agent-schedules-query-params.model';
import { CopyAgentSchedulechart } from '../../../models/copy-agent-schedule-chart.model';
import { SchedulingAgents } from '../../../models/scheduling-agents.model';
import { AgentSchedulesService } from '../../../services/agent-schedules.service';

@Component({
  selector: 'app-copy-schedule',
  templateUrl: './copy-schedule.component.html',
  styleUrls: ['./copy-schedule.component.scss']
})
export class CopyScheduleComponent implements OnInit, OnDestroy {

  totalAgents: number;
  currentPage = 1;
  pageSize = 10;
  spinner = 'Copy-Schedule';
  orderBy = 'employeeId';
  sortBy = 'asc';
  formSubmitted: boolean;
  masterSelected: boolean;

  agents: SchedulingAgents[] = [];
  copiedAgents: number[] = [];

  paginationSize = Constants.paginationSize;

  copyAgentScheduleChartSubscription: ISubscription;
  getAgentsSubscription: ISubscription;
  subscriptions: ISubscription[] = [];

  @Input() agentSchedulingGroupId: number;
  @Input() agentScheduleId: string;
  @Input() agentScheudleType: AgentScheduleType;
  @Input() translationValues: TranslationDetails[];

  constructor(
    public activeModal: NgbActiveModal,
    private spinnerService: NgxSpinnerService,
    private agentSchedulesService: AgentSchedulesService,
    private authService: AuthService
  ) { }

  ngOnInit(): void {
    this.loadAgents(true);
  }

  ngOnDestroy() {
    this.subscriptions.forEach(subscription => {
      if (subscription) {
        subscription.unsubscribe();
      }
    });
  }

  onSchedulingGroupChange(schedulingGroupId: number) {
    if (schedulingGroupId) {
      this.agentSchedulingGroupId = +schedulingGroupId;
      this.loadAgents();
    } else {
      this.agents = [];
    }
  }

  changePageSize(pageSize: number) {
    this.pageSize = pageSize;
    this.loadAgents();
  }

  changePage(page: number) {
    this.currentPage = page;
    this.loadAgents();
  }

  checkUncheckAll() {
    for (const agent of this.agents) {
      agent.isChecked = this.masterSelected;
    }
  }

  isAllSelected() {
    this.masterSelected = this.agents.every(x => x.isChecked === true);
  }

  copySchedule() {
    this.formSubmitted = true;
    for (const index in this.agents) {
      if (this.agents[index].isChecked === true) {
        this.copiedAgents.push(this.agents[index].employeeId);
      }
    }
    if (this.agents.some(x => x.isChecked === true)) {
      this.copyAgentSchedule();
    } else {
      this.activeModal.close({ needRefresh: false });
    }

  }

  private getQueryParams() {
    const agentSchedulesQueryParams = new AgentSchedulesQueryParams();
    agentSchedulesQueryParams.agentSchedulingGroupId = this.agentSchedulingGroupId;
    agentSchedulesQueryParams.pageNumber = this.currentPage;
    agentSchedulesQueryParams.pageSize = this.pageSize;
    agentSchedulesQueryParams.orderBy = `${this.orderBy} ${this.sortBy}`;
    agentSchedulesQueryParams.fields = 'employeeId,employeeName';

    return agentSchedulesQueryParams;
  }

  private loadAgents(unCheckAll?: boolean) {
    const queryParams = this.getQueryParams();
    this.spinnerService.show(this.spinner, SpinnerOptions);

    this.getAgentsSubscription = this.agentSchedulesService.getAgentSchedules(queryParams)
      .subscribe((response) => {
        this.agents = response.body;
        let headerPaginationValues = new HeaderPagination();
        headerPaginationValues = JSON.parse(response.headers.get('x-pagination'));
        this.totalAgents = headerPaginationValues.totalCount;
        if (unCheckAll) {
          this.agents.forEach(ele => {
            ele.isChecked = false;
          });
        }
        this.spinnerService.hide(this.spinner);
      }, (error) => {
        this.spinnerService.hide(this.spinner);
      });

    this.subscriptions.push(this.getAgentsSubscription);
  }

  private copyAgentSchedule() {
    this.spinnerService.show(this.spinner, SpinnerOptions);
    const copyData = new CopyAgentSchedulechart();
    copyData.agentSchedulingGroupId = this.agentSchedulingGroupId;
    copyData.agentScheduleType = this.agentScheudleType;
    copyData.modifiedBy = this.authService.getLoggedUserInfo()?.displayName;
    copyData.employeeIds = this.masterSelected ? [] : this.copiedAgents;

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

}
