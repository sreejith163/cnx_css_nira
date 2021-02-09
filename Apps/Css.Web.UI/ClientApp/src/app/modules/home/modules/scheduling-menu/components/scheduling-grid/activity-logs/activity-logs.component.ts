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
import { ActivityType } from 'src/app/shared/enums/activity-type.enum';
import { ActivityLogsChart } from '../../../models/activity-logs-chart.model';
import { ActivityLogsResponse } from '../../../models/activity-logs-response.model';
import { ActivityOrigin } from '../../../enums/activity-origin.enum';
import { ActivityStatus } from '../../../enums/activity-status.enum';
import { AgentScheduleChart } from '../../../models/agent-schedule-chart.model';
import { ExcelService } from 'src/app/shared/services/excel.service';
import { ExcelData } from '../../../models/excel-data.model';
import { ActivityLogExcelData } from '../../../models/activity-log-excel-data.model';

@Component({
  selector: 'app-activity-logs',
  templateUrl: './activity-logs.component.html',
  styleUrls: ['./activity-logs.component.scss']
})
export class ActivityLogsComponent implements OnInit, OnDestroy {

  spinner = 'activity-logs';
  orderBy = 'TimeStamp';
  sortBy = 'desc';
  exportFileName = 'Attendance_scheduling';
  searchKeyword: string;
  iconCode: string;
  iconDescription: string;
  startTimeFilter: string;
  endTimeFilter: string;
  totalRevisions = 3;
  timeIntervals = 15;
  characterSplice = 25;
  currentPage = 1;
  pageSize = 10;
  totalRecord: number;

  weekDay = WeekDay;
  activityTypeEnum = ActivityType;
  activityOrigin = ActivityOrigin;
  activityStatus = ActivityStatus;
  paginationSize = Constants.paginationSize;

  columnList: Array<string> = [];
  hiddenColumnList: Array<string> = [];
  openTimes: Array<any>;
  activityLogsData: ActivityLogsResponse[] = [];
  schedulingCodes: SchedulingCode[] = [];
  activityLogsChart: ActivityLogsChart[] = [];

  getSchedulingCodesSubscription: ISubscription;
  getActivityLogsSubscription: ISubscription;
  subscriptions: ISubscription[] = [];

  @Input() activityType: ActivityType;
  @Input() employeeId: number;
  @Input() employeeName: string;
  @Input() startDate: Date;

  constructor(
    public activeModal: NgbActiveModal,
    private spinnerService: NgxSpinnerService,
    private activityLogService: ActivityLogsService,
    private schedulingCodeService: SchedulingCodeService,
    private excelService: ExcelService,
  ) { }

  ngOnInit(): void {
    if (this.activityType === ActivityType.SchedulingGrid) {
      this.columnList = ['Employee Id', 'Day', 'Time Stamp', 'Executed By', 'Origin', 'Status'];
    } else {
      this.columnList = ['Employee Id', 'Time Stamp', 'Executed By', 'Origin', 'Status'];
    }
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

  getDateInStringFormat(timeStamp: Date): string {
    if (!timeStamp) {
      return undefined;
    }
    // const date = timeStamp.getTime();
    const date = new Date(timeStamp);
    return new Date(date).toDateString();
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

  getIconFromSelectedAgent(index: number, openTime: string) {
    if (this.activityLogsChart.length > 0) {
      const item = this.activityLogsChart[index];
      const weekTimeData = item?.agentScheduleChart?.charts.find(x => this.convertToDateFormat(openTime) >=
        this.convertToDateFormat(x.startTime) &&
        this.convertToDateFormat(openTime) < this.convertToDateFormat(x.endTime));
      if (weekTimeData) {
        const code = this.schedulingCodes.find(x => x.id === weekTimeData.schedulingCodeId);
        return code ? this.unifiedToNative(code?.icon?.value) : '';
      }
    }

    return '';
  }

  getAgentIconDescription(index: number, openTime: string) {
    if (this.activityLogsChart.length > 0) {
      const item = this.activityLogsChart[index];
      const weekTimeData = item?.agentScheduleChart?.charts.find(x => this.convertToDateFormat(openTime) >=
        this.convertToDateFormat(x.startTime) &&
        this.convertToDateFormat(openTime) < this.convertToDateFormat(x.endTime));
      if (weekTimeData) {
        const code = this.schedulingCodes.find(x => x.id === weekTimeData.schedulingCodeId);
        return code?.description;
      }
    }

    return '';
  }

  sort(columnName: string, sortBy: string) {
    this.sortBy = sortBy === 'asc' ? 'desc' : 'asc';
    this.orderBy = columnName;

    this.loadActivityLogs();
  }

  changePage(page: number) {
    this.currentPage = page;
    // this.loadActivityLogs();
  }

  changePageSize(pageSize: number) {
    this.pageSize = pageSize;
    // this.loadActivityLogs();
  }

  search() {
    this.loadActivityLogs();
  }

  exportToExcel() {
    const exportSchedule = new Array<ActivityLogExcelData>();
    for (const item of this.activityLogsChart) {
      const model = new ActivityLogExcelData();
      model.EmployeeId = +item?.executedUser;
      if(this.activityType === ActivityType.SchedulingGrid) {
        model.Day = WeekDay[item?.day];
      }
      model.ExecutedBy = item?.executedBy;
      model.Origin = ActivityOrigin[item?.activityOrigin];
      model.Status = ActivityStatus[item?.activityStatus];
      model.TimeStamp = this.getDateInStringFormat(item?.timeStamp);
      exportSchedule.push(model);
    }

    const today = new Date();
    const year = String(today.getFullYear());
    const month = String((today.getMonth() + 1)).length === 1 ?
      ('0' + String((today.getMonth() + 1))) : String((today.getMonth() + 1));
    const day = String(today.getDate()).length === 1 ?
      ('0' + String(today.getDate())) : String(today.getDate());

    const date = year + month + day;
    this.excelService.exportAsExcelFile(exportSchedule, this.exportFileName + date);
  }

  private getQueryParams() {
    const queryParams = new ActivityLogsQueryParams();
    queryParams.activityType = this.activityType;
    // queryParams.pageNumber = this.currentPage;
    // queryParams.pageSize = this.pageSize;
    queryParams.searchKeyword = this.searchKeyword ?? '';
    queryParams.orderBy = `${this.orderBy} ${this.sortBy}`;
    queryParams.fields = undefined;
    queryParams.employeeId = this.employeeId;
    queryParams.skipPageSize = true;


    return queryParams;
  }

  private loadActivityLogs() {
    const queryParams = this.getQueryParams();
    this.spinnerService.show(this.spinner, SpinnerOptions);

    this.getActivityLogsSubscription = this.activityLogService.getActivityLogs(queryParams)
      .subscribe((response) => {
        this.activityLogsChart = [];
        this.activityLogsData = response.body;
        this.activityType === ActivityType.SchedulingManagerGrid ? this.setManagerChartData() : this.setScheduleChart();
        this.setAgentFilters();
        let headerPaginationValues = new HeaderPagination();
        headerPaginationValues = JSON.parse(response.headers.get('x-pagination'));
        this.totalRecord = this.activityLogsChart.length;
        this.spinnerService.hide(this.spinner);
      }, (error) => {
        this.spinnerService.hide(this.spinner);
        console.log(error);
      });

    this.subscriptions.push(this.getActivityLogsSubscription);

  }

  private setAgentFilters() {
    const codeId = this.activityLogsChart[0]?.agentScheduleChart?.charts[0]?.schedulingCodeId;
    const code = this.schedulingCodes.find(x => x.id === codeId);
    this.iconCode = code?.icon?.value;
    this.iconDescription = code?.description;
    this.startTimeFilter = this.activityLogsChart[0]?.agentScheduleChart?.charts[0]?.startTime;
    this.endTimeFilter = this.activityLogsChart[0]?.agentScheduleChart.charts[0]?.endTime;
  }

  private setScheduleChart() {
    for (const item of this.activityLogsData) {
      for (const icon of item.schedulingFieldDetails.agentScheduleCharts) {
        const chart = new ActivityLogsChart();
        chart.employeeId = item.employeeId;
        chart.executedUser = item.executedUser;
        chart.executedBy = item.executedBy;
        chart.activityStatus = item.activityStatus;
        chart.activityOrigin = item.activityOrigin;
        chart.timeStamp = item.timeStamp;
        icon.charts.map(x => {
          x.endTime = x.endTime.trim().toLowerCase();
          x.startTime = x.startTime.trim().toLowerCase();
        });
        chart.agentScheduleChart = new AgentScheduleChart();
        chart.day = icon.day;
        chart.agentScheduleChart.charts = icon.charts;
        this.activityLogsChart.push(chart);
      }
    }
  }

  private setManagerChartData() {
    for (const item of this.activityLogsData) {
      const chart = new ActivityLogsChart();
      chart.employeeId = item.employeeId;
      chart.executedUser = item.executedUser;
      chart.executedBy = item.executedBy;
      chart.activityStatus = item.activityStatus;
      chart.activityOrigin = item.activityOrigin;
      chart.timeStamp = item.timeStamp;
      item.schedulingFieldDetails.agentScheduleManagerCharts[0].charts.map(x => {
        x.endTime = x.endTime.trim().toLowerCase();
        x.startTime = x.startTime.trim().toLowerCase();
      });
      chart.agentScheduleChart = new AgentScheduleChart();
      chart.agentScheduleChart.charts = item?.schedulingFieldDetails?.agentScheduleManagerCharts[0]?.charts;
      this.activityLogsChart.push(chart);
    }
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
