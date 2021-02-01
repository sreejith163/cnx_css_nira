import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { TranslateService } from '@ngx-translate/core';
import { NgxSpinnerService } from 'ngx-spinner';
import { SubscriptionLike as ISubscription } from 'rxjs';
import { AuthService } from 'src/app/core/services/auth.service';
import { HeaderPagination } from 'src/app/shared/models/header-pagination.model';
import { GenericStateManagerService } from 'src/app/shared/services/generic-state-manager.service';
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
  checkAll: boolean;

  agents: SchedulingAgents[] = [];
  checkedAgents: SchedulingAgents[] = [];

  paginationSize = Constants.paginationSize;

  copyAgentScheduleChartSubscription: ISubscription;
  getAgentsSubscription: ISubscription;
  subscriptions: ISubscription[] = [];

  @Input() agentSchedulingGroupId: number;
  @Input() employeeId: number;
  @Input() agentScheduleId: string;
  @Input() agentScheduleType: AgentScheduleType;

  constructor(
    public translate: TranslateService,
    public activeModal: NgbActiveModal,
    private spinnerService: NgxSpinnerService,
    private agentSchedulesService: AgentSchedulesService,
    private authService: AuthService
  ) { }

  ngOnInit(): void {
    this.loadAgents();
  }

  ngOnDestroy() {
    this.subscriptions.forEach(subscription => {
      if (subscription) {
        subscription.unsubscribe();
      }
    });
  }

  onSchedulingGroupChange(schedulingGroupId: number) {
    this.masterSelected = false;
    this.checkAll = false;
    if (schedulingGroupId) {
      this.checkedAgents = [];
      this.agentSchedulingGroupId = +schedulingGroupId;
      this.loadAgents();
    } else {
      this.agentSchedulingGroupId = undefined;
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
    if (this.checkedAgents.filter(x => x.isChecked === true).length === this.totalAgents - 1) {
      this.masterSelected = true;
    } else {
      this.masterSelected = false;
    }

  }

  copySchedule() {
    this.formSubmitted = true;
    const copiedAgents = [];
    if (this.checkedAgents.length > 0 || this.masterSelected) {
      if (this.checkedAgents.length > 0 && this.checkedAgents.filter(x => x.isChecked === true).length !== this.totalAgents) {
        for (const item of this.checkedAgents.filter(x => x.isChecked === true)) {
          copiedAgents.push(item.employeeId);
        }
      }
      this.copyAgentSchedule(copiedAgents);
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

  private loadAgents() {
    const queryParams = this.getQueryParams();
    this.spinnerService.show(this.spinner, SpinnerOptions);

    this.getAgentsSubscription = this.agentSchedulesService.getAgentSchedules(queryParams)
      .subscribe((response) => {
        this.agents = response.body;
        const index = this.agents.findIndex(x => x.employeeId === this.employeeId);
        this.agents.splice(index, 1);
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
    const copyData = new CopyAgentSchedulechart();
    copyData.agentSchedulingGroupId = this.agentSchedulingGroupId;
    copyData.agentScheduleType = this.agentScheduleType;
    copyData.modifiedBy = this.authService.getLoggedUserInfo()?.displayName;
    copyData.employeeIds = this.masterSelected ? [] : copiedAgents;

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
