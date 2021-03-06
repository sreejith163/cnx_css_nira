import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { NgxSpinnerService } from 'ngx-spinner';
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
import { TranslateService } from '@ngx-translate/core';
import { ScheduleChart } from '../../../models/schedule-chart.model';
import * as moment from 'moment';

@Component({
  selector: 'app-activity-logs-schedule',
  templateUrl: './activity-logs-schedule.component.html',
  styleUrls: ['./activity-logs-schedule.component.scss']
})
export class ActivityLogsScheduleComponent implements OnInit, OnDestroy {

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
  activityLogsChart: ActivityLogsChart[] = [];

  getActivityLogsSubscription: ISubscription;
  subscriptions: ISubscription[] = [];

  @Input() activityType: ActivityType;
  @Input() employeeId: string;
  @Input() employeeName: string;
  @Input() startDate: string;
  @Input() dateFrom: string;
  @Input() dateTo: string;
  @Input() schedulingCodes: SchedulingCode[] = [];

  constructor(
    public translate: TranslateService,
    public activeModal: NgbActiveModal,
    private spinnerService: NgxSpinnerService,
    private activityLogService: ActivityLogsService,
    private excelService: ExcelService,
  ) { }

  ngOnInit(): void {
    if (this.activityType === ActivityType.SchedulingGrid) {
      this.columnList = ['Employee Id', 'Day', 'Time Stamp', 'Executed By', 'Origin', 'Status'];
    } else {
      this.columnList = ['Employee Id', 'Time Stamp', 'Executed By', 'Origin', 'Status'];
    }
    this.openTimes = this.getOpenTimes();
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

  getIconFromSelectedAgent(id: number, openTime: string) {
    if (this.activityLogsChart.length > 0) {
      const item = this.activityLogsChart.find(x => x.id === id);
      const weekTimeData = item?.agentScheduleChart?.charts?.find(x => this.convertToDateFormat(openTime) >=
        this.convertToDateFormat(x?.startTime) &&
        this.convertToDateFormat(openTime) < this.convertToDateFormat(x?.endTime));
      if (weekTimeData) {
        const code = this.schedulingCodes.find(x => x.id === weekTimeData?.schedulingCodeId);
        return code ? this.unifiedToNative(code?.icon?.value) : '';
      }
    }

    return '';
  }

  getAgentIconDescription(id: number, openTime: string) {
    if (this.activityLogsChart.length > 0) {
      const item = this.activityLogsChart.find(x => x.id === id);
      const weekTimeData = item?.agentScheduleChart?.charts?.find(x => this.convertToDateFormat(openTime) >=
        this.convertToDateFormat(x?.startTime) &&
        this.convertToDateFormat(openTime) < this.convertToDateFormat(x?.endTime));
      if (weekTimeData) {
        const code = this.schedulingCodes?.find(x => x.id === weekTimeData?.schedulingCodeId);
        return code?.description;
      }
    }

    return '';
  }

  sort(columnName: string, sortBy: string) {
    this.sortBy = sortBy === 'asc' ? 'desc' : 'asc';
    this.orderBy = columnName;
    if (sortBy === 'asc') {
      this.activityLogsChart.sort((a, b): number => {
        if (a[columnName] < b[columnName]) {
          return -1;
        } else if (a[columnName] > b[columnName]) {
          return 1;
        }
        else {
          return 0;
        }
      });

    } else {
      this.activityLogsChart.sort((a, b): number => {
        if (a[columnName] > b[columnName]) {
          return -1;
        } else if (a[columnName] < b[columnName]) {
          return 1;
        }
        else {
          return 0;
        }
      });
    }

  }

  changePage(page: number) {
    this.currentPage = page;
  }

  changePageSize(pageSize: number) {
    this.pageSize = pageSize;
  }

  search() {
    this.formatActivityLogsData();
    if (this.searchKeyword) {
      this.activityLogsChart = this.activityLogsChart.filter(x => x.executedUser.toLowerCase().includes(this.searchKeyword.toLowerCase()) ||
        x.executedBy.toLowerCase().includes(this.searchKeyword.toLowerCase()));
      this.totalRecord = this.activityLogsChart.length;
    }
  }

  exportToExcel() {
    const exportSchedule = new Array<any>();
    for (const item of this.activityLogsChart) {
      const model: any = {};
      model.EmployeeId = +item?.executedUser;
      if (this.activityType === ActivityType.SchedulingGrid) {
        model.Day = WeekDay[item?.day];
      }
      model.ExecutedBy = item?.executedBy;
      model.Origin = ActivityOrigin[item?.activityOrigin];
      model.Status = ActivityStatus[item?.activityStatus];
      model.TimeStamp = this.getDateInStringFormat(item?.timeStamp);
      for (const time of this.openTimes) {
        const code = item?.agentScheduleChart?.charts
          .find(x => x?.startTime?.trim().toLowerCase() <= time?.trim().toLowerCase() &&
            x.endTime?.trim().toLowerCase() > time?.trim().toLowerCase())?.schedulingCodeId;
        const formattedTime = this.formatTimeFormat(time);
        if (code) {
          model[formattedTime] = this.schedulingCodes.find(x => x?.id === code)?.description;
        } else {
          model[formattedTime] = '';
        }
      }

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
    queryParams.searchKeyword = this.searchKeyword ?? '';
    queryParams.orderBy = `${this.orderBy} ${this.sortBy}`;
    queryParams.fields = undefined;
    queryParams.employeeId = this.employeeId;
    queryParams.skipPageSize = true;
    queryParams.dateFrom = this.activityType === ActivityType.SchedulingGrid ? this.getFormattedDate(this.dateFrom) : undefined;
    queryParams.dateto = this.activityType === ActivityType.SchedulingGrid ? this.getFormattedDate(this.dateTo) : undefined;
    queryParams.date = this.activityType === ActivityType.SchedulingManagerGrid ? this.getFormattedDate(this.startDate) : undefined;

    return queryParams;
  }

  private getFormattedDate(date) {
    let dt = new Date(date).toUTCString();
    const transformedDate = moment(dt).format('YYYY-MM-DD');
    console.log(transformedDate)
    return transformedDate;
  }

  private loadActivityLogs() {
    const queryParams = this.getQueryParams();
    this.spinnerService.show(this.spinner, SpinnerOptions);

    this.getActivityLogsSubscription = this.activityLogService.getActivityLogs(queryParams)
      .subscribe((response) => {
        this.activityLogsData = response.body;
        this.formatActivityLogsData();
        this.spinnerService.hide(this.spinner);
      }, (error) => {
        this.spinnerService.hide(this.spinner);
        console.log(error);
      });

    this.subscriptions.push(this.getActivityLogsSubscription);

  }

  private formatActivityLogsData() {
    this.activityLogsChart = [];
    this.activityType === ActivityType.SchedulingManagerGrid ? this.setManagerChartData() : this.setScheduleChart();
    this.totalRecord = this.activityLogsChart?.length;
  }

  private setAgentFilters() {
    const openTime = this.schedulingCodes.find(x => x?.description?.trim().toLowerCase() === 'open time');
    if (openTime) {
      const openTimeIndex = this.activityLogsChart[0]?.agentScheduleChart?.charts.findIndex(x => x?.schedulingCodeId === +openTime?.id);
      if (openTimeIndex > -1) {
        this.iconCode = openTime?.icon?.value;
        this.iconDescription = openTime?.description;
        this.startTimeFilter = this.formatTimeFormat(this.activityLogsChart[0]?.agentScheduleChart?.charts[openTimeIndex]?.startTime);
        this.endTimeFilter = this.formatTimeFormat(this.activityLogsChart[0]?.agentScheduleChart?.charts[openTimeIndex]?.endTime);
      } else {
        const codeId = this.activityLogsChart[0]?.agentScheduleChart?.charts[0]?.schedulingCodeId;
        const code = this.schedulingCodes.find(x => x.id === codeId);
        this.iconCode = code?.icon?.value;
        this.iconDescription = code?.description;
        this.startTimeFilter = this.formatTimeFormat(this.activityLogsChart[0]?.agentScheduleChart?.charts[0]?.startTime);
        this.endTimeFilter = this.formatTimeFormat(this.activityLogsChart[0]?.agentScheduleChart?.charts[0]?.endTime);
      }
    }
  }

  private formatTimeFormat(timeData: string) {
    if (timeData?.trim().toLowerCase().slice(0, 2) === '00') {
      timeData = '12' + timeData?.trim().toLowerCase().slice(2, 8);
    }
    if (timeData?.trim().toLowerCase() === '11:60 pm') {
      timeData = '12:00 am';
    }

    return timeData;
  }

  private setScheduleChart() {
    let id = 0;
    for (const item of this.activityLogsData) {
      item?.schedulingFieldDetails?.activityLogRange?.scheduleCharts?.forEach((icon) => {
        id = id + 1;
        const chart = new ActivityLogsChart();
        chart.id = id;
        chart.employeeId = item?.employeeId;
        chart.executedUser = item?.executedUser;
        chart.executedBy = item?.executedBy;
        chart.activityStatus = item?.activityStatus;
        chart.activityOrigin = item?.activityOrigin;
        chart.timeStamp = item?.timeStamp;
        icon?.charts?.map(x => {
          x.endTime = x?.endTime?.trim().toLowerCase();
          x.startTime = x?.startTime?.trim().toLowerCase();
          if (x?.endTime?.trim().toLowerCase() === '00:00 am' || x?.endTime?.trim().toLowerCase() === '12:00 am') {
            x.endTime = '11:60 pm';
          }
          if (x?.endTime?.trim().toLowerCase().slice(0, 2) === '12') {
            x.endTime = '00' + x?.endTime?.trim().toLowerCase().slice(2, 8);
          }
          if (x?.startTime?.trim().toLowerCase().slice(0, 2) === '12') {
            x.startTime = '00' + x?.startTime?.trim().toLowerCase().slice(2, 8);
          }
        });
        this.sortChartData(icon?.charts);
        chart.agentScheduleChart = new AgentScheduleChart();
        chart.day = icon?.day;
        chart.agentScheduleChart.charts = icon?.charts;
        this.activityLogsChart.push(chart);
      });
    }

    this.setAgentFilters();
  }

  private setManagerChartData() {
    this.activityLogsData.forEach((item, index) => {
      const chart = new ActivityLogsChart();
      chart.id = index;
      chart.employeeId = item?.employeeId;
      chart.executedUser = item?.executedUser;
      chart.executedBy = item?.executedBy;
      chart.activityStatus = item?.activityStatus;
      chart.activityOrigin = item?.activityOrigin;
      chart.timeStamp = item?.timeStamp;
      if (!item?.schedulingFieldDetails?.activityLogManager?.charts ||
        item?.schedulingFieldDetails?.activityLogManager?.charts?.length === 0) {
        item.schedulingFieldDetails.activityLogManager.charts = [];
      } else {
        item?.schedulingFieldDetails?.activityLogManager?.charts.map(x => {
          x.endTime = x?.endTime.trim().toLowerCase();
          x.startTime = x?.startTime.trim().toLowerCase();
          if (x.endTime.trim().toLowerCase() === '00:00 am' || x.endTime.trim().toLowerCase() === '12:00 am') {
            x.endTime = '11:60 pm';
          }
          if (x?.endTime?.trim().toLowerCase().slice(0, 2) === '12') {
            x.endTime = '00' + x?.endTime?.trim().toLowerCase().slice(2, 8);
          }
          if (x?.startTime?.trim().toLowerCase().slice(0, 2) === '12') {
            x.startTime = '00' + x?.startTime?.trim().toLowerCase().slice(2, 8);
          }
        });
      }
      this.sortChartData(item?.schedulingFieldDetails?.activityLogManager?.charts);
      chart.agentScheduleChart = new AgentScheduleChart();
      chart.agentScheduleChart.charts = item?.schedulingFieldDetails?.activityLogManager?.charts;
      this.activityLogsChart.push(chart);
    });
    this.setAgentFilters();
  }

  private sortChartData(chart: ScheduleChart[]) {
    if (chart.length > 0) {
      chart.sort((a, b): number => {
        if (this.convertToDateFormat(a.startTime) < this.convertToDateFormat(b.startTime)) {
          return -1;
        } else if (this.convertToDateFormat(a.startTime) > this.convertToDateFormat(b.startTime)) {
          return 1;
        }
        else {
          return 0;
        }
      });
    }
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
