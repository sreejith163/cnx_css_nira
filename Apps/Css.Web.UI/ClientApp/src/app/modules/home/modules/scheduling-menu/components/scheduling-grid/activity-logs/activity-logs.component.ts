import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { NgxSpinnerService } from 'ngx-spinner';
import { HeaderPagination } from 'src/app/shared/models/header-pagination.model';
import { AgentScheduleType } from '../../../enums/agent-schedule-type.enum';
import { ActivityLogsQueryParams } from '../../../models/activity-logs-query-params.model';
import { ActivityLogsService } from '../../../services/activity-logs.service';
import { SubscriptionLike as ISubscription } from 'rxjs';
import { SpinnerOptions } from 'src/app/shared/util/spinner-options.util';
import { SchedulingCodeQueryParams } from '../../../../system-admin/models/scheduling-code-query-params.model';
import { SchedulingCodeService } from 'src/app/shared/services/scheduling-code.service';
import { SchedulingCode } from '../../../../system-admin/models/scheduling-code.model';
import { Constants } from 'src/app/shared/util/constants.util';
import { WeekDay } from '@angular/common';

@Component({
  selector: 'app-activity-logs',
  templateUrl: './activity-logs.component.html',
  styleUrls: ['./activity-logs.component.scss']
})
export class ActivityLogsComponent implements OnInit, OnDestroy {

  spinner = 'activity-logs';
  iconCode = '1F383';
  iconDescription = 'Open Time';
  startTimeFilter = '08:00 am';
  endTimeFilter = '10:00 am';

  // tableClassName = 'schedulingActivityTable';
  orderBy = 'createdDate';
  sortBy = 'desc';
  searchKeyword: string;
  totalRevisions = 3;
  timeIntervals = 15;
  characterSplice = 25;
  currentPage = 1;
  pageSize = 5;
  totalRecord: number;

  weekDay = WeekDay;
  paginationSize = Constants.paginationSize;

  columnList = ['Employee Id', 'Day', 'Time Stamp', 'Executed By', 'Origin', 'Status'];
  hiddenColumnList = [];
  openTimes: Array<any>;
  activityLogsData = [];
  schedulingCodes: SchedulingCode[] = [];

  getSchedulingCodesSubscription: ISubscription;
  getActivityLogsSubscription: ISubscription;
  subscriptions: ISubscription[] = [];

  @Input() agentScheduleType: AgentScheduleType;

  constructor(
    public activeModal: NgbActiveModal,
    private spinnerService: NgxSpinnerService,
    private activityLogService: ActivityLogsService,
    private schedulingCodeService: SchedulingCodeService,
  ) { }

  ngOnInit(): void {
    this.openTimes = this.getOpenTimes();
    this.loadSchedulingCodes();
    this.loadActivityLogs();
  }

  ngOnDestroy() {
    this.subscriptions.forEach(subscription => {
      if (subscription) {
        subscription.unsubscribe();
      }
    });
  }

  getDateInStringFormat(timeStamp: any): string {
    if (!timeStamp) {
      return undefined;
    }

    // const date = new Date(timeStamp.year, timeStamp.month - 1, timeStamp.day, 0, 0, 0, 0);
    return timeStamp.toDateString();
  }

  hasColumnHidden(column: string) {
    return this.hiddenColumnList?.findIndex(x => x === column) === -1;
  }

  hasHiddenColumnSelected(column: string) {
    return this.hiddenColumnList?.findIndex(x => x === column) !== -1;
  }

  onCheckColumnsToHide(e) {
    if (e.target.checked) {
      const item = e.target.value;
      this.hiddenColumnList.push(item);
    } else {
      const index = this.hiddenColumnList.findIndex(x => x === e.target.value);
      this.hiddenColumnList.splice(index, 1);
    }
  }

  unifiedToNative(unified: string) {
    if (unified) {
      const codePoints = unified.split('-').map(u => parseInt(`0x${u}`, 16));
      return String.fromCodePoint(...codePoints);
    }
  }

  getGridMaxWidth() {
    return window.innerWidth > 1350 ? (window.innerWidth - 350) + 'px' : '1250px';
  }

  isMainMinute(data: any) {
    return data.split(':')[1] === '00 am' || data.split(':')[1] === '00 pm';
  }

  getIconFromSelectedAgent(id: string, openTime: string) {
    const chart = this.activityLogsData.find(x => x.id === id);

    if (chart?.agentScheduleManagerCharts?.length > 0) {
      for (const item of chart.agentScheduleManagerCharts) {
        const weekTimeData = item?.charts.find(x => this.convertToDateFormat(openTime) >= this.convertToDateFormat(x.startTime) &&
          this.convertToDateFormat(openTime) < this.convertToDateFormat(x.endTime));
        if (weekTimeData) {
          const code = this.schedulingCodes.find(x => x.id === weekTimeData.schedulingCodeId);
          return code ? this.unifiedToNative(code?.icon?.value) : '';
        }
      }
    }

    return '';
  }

  getAgentIconDescription(id: string, openTime: string) {
    const chart = this.activityLogsData.find(x => x.id === id);

    if (chart?.agentScheduleManagerCharts?.length > 0) {
      for (const item of chart.agentScheduleManagerCharts) {
        const weekTimeData = item?.charts.find(x => this.convertToDateFormat(openTime) >= this.convertToDateFormat(x.startTime) &&
          this.convertToDateFormat(openTime) < this.convertToDateFormat(x.endTime));
        if (weekTimeData) {
          const code = this.schedulingCodes.find(x => x.id === weekTimeData.schedulingCodeId);
          return code?.description;
        }
      }
    }

    return '';
  }

  sort(columnName: string, sortBy: string) {
    this.sortBy = sortBy === 'asc' ? 'desc' : 'asc';
    this.orderBy = columnName;

    // this.loadAgentScheduleManger();
  }

  changePage(page: number) {
    this.currentPage = page;
    this.loadActivityLogs();
  }

  changePageSize(pageSize: number) {
    this.pageSize = pageSize;
    this.loadActivityLogs();
  }

  private getQueryParams() {
    const agentSchedulesQueryParams = new ActivityLogsQueryParams();
    agentSchedulesQueryParams.agentScheduleType = this.agentScheduleType;
    agentSchedulesQueryParams.pageNumber = this.currentPage;
    agentSchedulesQueryParams.pageSize = this.pageSize;
    agentSchedulesQueryParams.searchKeyword = this.searchKeyword ?? '';
    agentSchedulesQueryParams.orderBy = `${this.orderBy} ${this.sortBy}`;
    agentSchedulesQueryParams.fields = undefined;

    return agentSchedulesQueryParams;
  }

  private loadActivityLogs() {
    const queryParams = this.getQueryParams();
    this.spinnerService.show(this.spinner, SpinnerOptions);

    this.getActivityLogsSubscription = this.activityLogService.getActivityLogs(queryParams)
      .subscribe((response) => {
        this.activityLogsData = response;
        this.totalRecord = response.length;
        // let headerPaginationValues = new HeaderPagination();
        // headerPaginationValues = JSON.parse(response.headers.get('x-pagination'));
        // this.totalSchedulingRecord = headerPaginationValues.totalCount;

        this.spinnerService.hide(this.spinner);
      }, (error) => {
        this.spinnerService.hide(this.spinner);
        console.log(error);
      });

    this.subscriptions.push(this.getActivityLogsSubscription);

  }

  private loadSchedulingCodes() {
    const queryParams = new SchedulingCodeQueryParams();
    queryParams.skipPageSize = true;
    queryParams.fields = 'id, description, icon';
    this.spinnerService.show(this.spinner, SpinnerOptions);

    this.getSchedulingCodesSubscription = this.schedulingCodeService.getSchedulingCodes(queryParams)
      .subscribe((response) => {
        if (response.body) {
          this.schedulingCodes = response.body;
        }
        this.spinnerService.hide(this.spinner);
      }, (error) => {
        this.spinnerService.hide(this.spinner);
        console.log(error);
      });

    this.subscriptions.push(this.getSchedulingCodesSubscription);
  }

  private convertToDateFormat(time: string) {
    if (time) {
      const count = time.split(' ')[1] === 'pm' ? 12 : undefined;
      if (count) {
        time = (+time.split(':')[0] + 12) + ':' + time.split(':')[1].split(' ')[0];
      } else {
        time = time.split(':')[0] + ':' + time.split(':')[1].split(' ')[0];
      }

      return time;
    }
  }

  private getOpenTimes() {
    const x = this.timeIntervals;
    const times = [];
    let tt = 0;
    const ap = ['am', 'pm'];

    for (let i = 0; tt < 24 * 60; i++) {
      const hh = Math.floor(tt / 60);
      const mm = tt % 60;
      times[i] =
        ('0' + (hh % 12)).slice(-2) +
        ':' +
        ('0' + mm).slice(-2) +
        ' ' +
        ap[Math.floor(hh / 12)];
      tt = tt + x;
    }
    return times;
  }

}
