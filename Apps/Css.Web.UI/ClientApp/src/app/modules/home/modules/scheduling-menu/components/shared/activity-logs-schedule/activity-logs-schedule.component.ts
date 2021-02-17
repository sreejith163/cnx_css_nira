import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { NgxSpinnerService } from 'ngx-spinner';
import { HeaderPagination } from 'src/app/shared/models/header-pagination.model';
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


@Component({
  selector: 'app-activity-logs-schedule',
  templateUrl: './activity-logs-schedule.component.html',
  styleUrls: ['./activity-logs-schedule.component.scss']
})
export class ActivityLogsScheuldeComponent implements OnInit, OnDestroy {

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
    public translate: TranslateService,
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
        const code = item?.agentScheduleChart?.charts.find(x => x.startTime <= time && x.endTime > time)?.schedulingCodeId;
        if (code) {
          model[time] = this.schedulingCodes.find(x => x.id === code)?.description;
        } else {
          model[time] = '';
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

    return queryParams;
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
    this.setAgentFilters();
    this.totalRecord = this.activityLogsChart?.length;
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
    let id = 0;
    for (const item of this.activityLogsData) {
        item.schedulingFieldDetails.agentScheduleCharts.forEach((icon) => {
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
            x.endTime = x?.endTime.trim().toLowerCase();
            x.startTime = x?.startTime.trim().toLowerCase();
            if (x.endTime.trim().toLowerCase() === '00:00 am') {
              x.endTime = '11:60 pm';
            }
          });
          chart.agentScheduleChart = new AgentScheduleChart();
          chart.day = icon?.day;
          chart.agentScheduleChart.charts = icon?.charts;
          this.activityLogsChart.push(chart);
        });
    }
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
        item?.schedulingFieldDetails?.agentScheduleManagerCharts[0]?.charts.map(x => {
          x.endTime = x?.endTime.trim().toLowerCase();
          x.startTime = x?.startTime.trim().toLowerCase();
          if (x.endTime.trim().toLowerCase() === '00:00 am') {
            x.endTime = '11:60 pm';
          }
        });
        chart.agentScheduleChart = new AgentScheduleChart();
        chart.agentScheduleChart.charts = item?.schedulingFieldDetails?.agentScheduleManagerCharts[0]?.charts;
        this.activityLogsChart.push(chart);
      });
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
