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
import { DatePipe, WeekDay } from '@angular/common';
import { ActivityType } from 'src/app/shared/enums/activity-type.enum';
import { ActivityLogsChart } from '../../../models/activity-logs-chart.model';
import { ActivityLogsResponse } from '../../../models/activity-logs-response.model';
import { ActivityOrigin } from '../../../enums/activity-origin.enum';
import { ActivityStatus } from '../../../enums/activity-status.enum';
import { AgentScheduleChart } from '../../../models/agent-schedule-chart.model';
import { ExcelService } from 'src/app/shared/services/excel.service';
import { TranslateService } from '@ngx-translate/core';
import { ScheduleChart } from '../../../models/schedule-chart.model';
import { ActivityLogsManagerChart } from '../../../models/activity-logs-manager-chart.model';
import { AgentScheduleManagerChart } from '../../../models/agent-schedule-manager-chart.model';
import { ManagerScheduleChart } from '../../../models/manager-schedule-chart.model';


@Component({
  selector: 'app-activity-logs-schedule',
  templateUrl: './activity-logs-schedule.component.html',
  styleUrls: ['./activity-logs-schedule.component.scss'],
  providers: [DatePipe]
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
  tommorrowDate: Date;

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
  @Input() dateFrom: Date;
  @Input() dateTo: Date;

  constructor(
    public translate: TranslateService,
    public activeModal: NgbActiveModal,
    private spinnerService: NgxSpinnerService,
    private activityLogService: ActivityLogsService,
    private schedulingCodeService: SchedulingCodeService,
    private excelService: ExcelService,
    private datepipe: DatePipe,
  ) { }

  ngOnInit(): void {
    if (this.activityType === ActivityType.SchedulingGrid) {
      this.columnList = ['Employee Id', 'Day', 'Time Stamp', 'Executed By', 'Origin', 'Status'];
    } else {
      this.tommorrowDate = new Date(this.startDate);
      this.tommorrowDate.setDate(this.startDate.getDate() + 1);
      this.timeIntervals = 30;
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

  getIconFromManagerAgent(id: number, openTime: string, date?: Date) {
    if (this.activityLogsChart.length > 0) {
      const item = this.activityLogsChart.find(x => x.id === id);
      const chart = item.managerChart.find(x => this.getDateInStringFormat(x.date) === this.getDateInStringFormat(date));
      const weekTimeData = chart?.charts?.find(x => this.convertToDateFormat(openTime) >=
        this.convertToDateFormat(x?.startTime) &&
        this.convertToDateFormat(openTime) < this.convertToDateFormat(x?.endTime));
      if (weekTimeData) {
        const code = this.schedulingCodes.find(x => x.id === weekTimeData?.schedulingCodeId);
        return code ? this.unifiedToNative(code?.icon?.value) : '';
      }
    }

    return '';

  }

  getAgentManagerIconDescription(id: number, openTime: string, date?: Date) {
    if (this.activityLogsChart.length > 0) {
      const item = this.activityLogsChart.find(x => x.id === id);
      const chart = item.managerChart.find(x => this.getDateInStringFormat(x.date) === this.getDateInStringFormat(date));
      const weekTimeData = chart?.charts?.find(x => this.convertToDateFormat(openTime) >=
        this.convertToDateFormat(x?.startTime) &&
        this.convertToDateFormat(openTime) < this.convertToDateFormat(x?.endTime));
      if (weekTimeData) {
        const code = this.schedulingCodes.find(x => x.id === weekTimeData?.schedulingCodeId);
        return code?.description;
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
    queryParams.dateFrom = this.activityType === ActivityType.SchedulingGrid ? this.getDateInStringFormat(this.dateFrom) : undefined;
    queryParams.dateto = this.activityType === ActivityType.SchedulingGrid ? this.getDateInStringFormat(this.dateTo) : undefined;
    queryParams.date = this.activityType === ActivityType.SchedulingManagerGrid ? this.getDateInStringFormat(this.startDate) : undefined;

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

  private setManagerAgentFilters() {
    const openTime = this.schedulingCodes.find(x => x?.description?.trim().toLowerCase() === 'open time');
    const openTimeIndexToday = this.activityLogsChart[0]?.managerChart[0]?.charts.findIndex(x => x?.schedulingCodeId === +openTime?.id);
    const openTimeIndexTommorrow = this.activityLogsChart[0]?.managerChart[1]?.charts.
      findIndex(x => x?.schedulingCodeId === +openTime?.id);
    if (openTimeIndexToday > -1 || openTimeIndexTommorrow > -1) {
      this.setManagerFilterData(openTimeIndexToday, openTimeIndexTommorrow, openTime);
    } else {
      const codeId = this.activityLogsChart[0]?.managerChart[0]?.charts[0]?.schedulingCodeId;
      const code = this.schedulingCodes.find(x => x.id === codeId);
      const codeIndexToday = this.activityLogsChart[0]?.managerChart[0]?.charts.findIndex(x => x?.schedulingCodeId === +codeId);
      const codeIndexTommorrow = this.activityLogsChart[0]?.managerChart[1]?.charts.
        findIndex(x => x?.schedulingCodeId === +codeId);
      this.setManagerFilterData(codeIndexToday, codeIndexTommorrow, code);
    }
  }

  private setManagerFilterData(todayIndex: number, tommorrowIndex: number, iconData: SchedulingCode) {
    const filterArray = new Array<ScheduleChart>();
    if (todayIndex > -1) {
      filterArray.push(this.activityLogsChart[0]?.managerChart[0]?.charts[todayIndex]);
    }
    if (tommorrowIndex > -1) {
      filterArray.push(this.activityLogsChart[0]?.managerChart[1]?.charts[tommorrowIndex]);
    }
    const formattedArray = JSON.parse(JSON.stringify(filterArray));
    formattedArray.map(x => {
      if (x.endTime === '11:60 pm') {
        x.endTime = '00:00 am';
      }
    });
    this.formatTimeValuesInSchedulingGrid(formattedArray);
    if (formattedArray.length > 0) {
      this.iconCode = iconData?.icon?.value;
      this.iconDescription = iconData?.description;
      this.startTimeFilter = formattedArray[0].startTime;
      this.endTimeFilter = formattedArray[0].endTime;
    }
  }

  private formatTimeValuesInSchedulingGrid(charts: ScheduleChart[]) {
    let agentScheduleCharts = charts;
    if (agentScheduleCharts.length > 0) {
      agentScheduleCharts = this.adjustSchedulingCalendarTimesRange(agentScheduleCharts);
    }
  }

  private adjustSchedulingCalendarTimesRange(times: Array<ScheduleChart>) {
    const newTimesarray = new Array<ScheduleChart>();
    let calendarTimes = new ScheduleChart(null, null, null);

    for (const index in times) {
      if (+index === 0) {
        calendarTimes = times[index];
        if (+index === times.length - 1) {
          break;
        }
      } else if (calendarTimes.endTime === times[index].startTime && calendarTimes.schedulingCodeId === times[index].schedulingCodeId) {
        calendarTimes.endTime = times[index].endTime;
        if (+index === times.length - 1) {
          break;
        }
      } else {
        const model = new ScheduleChart(calendarTimes.startTime, calendarTimes.endTime, calendarTimes.schedulingCodeId);
        newTimesarray.push(model);
        calendarTimes = times[index];
        if (+index === times.length - 1) {
          break;
        }
      }
    }

    const modelvalue = new ScheduleChart(calendarTimes.startTime, calendarTimes.endTime, calendarTimes.schedulingCodeId);
    newTimesarray.push(modelvalue);

    return newTimesarray;
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
      const todaysChart = new AgentScheduleManagerChart();
      todaysChart.date = this.startDate;
      const tommorrowsChart = new AgentScheduleManagerChart();
      tommorrowsChart.date = this.tommorrowDate;
      if (!item?.schedulingFieldDetails?.activityLogManager?.charts ||
        item?.schedulingFieldDetails?.activityLogManager?.charts?.length === 0) {
        item.schedulingFieldDetails.activityLogManager.charts = [];
      } else {
        item?.schedulingFieldDetails?.activityLogManager?.charts.map(x => {
          let endTime = this.convertDateTimeToLocalMeridiemTime(x?.endDateTime);
          let startTime = this.convertDateTimeToLocalMeridiemTime(x?.startDateTime);
          if (endTime?.trim().toLowerCase() === '00:00 am' || endTime?.trim().toLowerCase() === '12:00 am') {
            endTime = '11:60 pm';
          }
          if (endTime?.trim().toLowerCase().slice(0, 2) === '12') {
            endTime = '00' + endTime?.trim().toLowerCase().slice(2, 8);
          }
          if (startTime.trim().toLowerCase().slice(0, 2) === '12') {
            startTime = '00' + startTime?.trim().toLowerCase().slice(2, 8);
          }
          const startDate = this.startDate.getUTCDate() + '-' + this.startDate.getUTCMonth() + '-' + this.startDate.getUTCFullYear();
          const dataDate = new Date(x.startDateTime);
          const date = dataDate.getUTCDate() + '-' + dataDate.getUTCMonth() + '-' + dataDate.getUTCFullYear();
          if (startDate === date) {
            const schedulechart = new ScheduleChart(startTime, endTime, x.schedulingCodeId);
            todaysChart.charts.push(schedulechart);
          } else {
            const schedulechart = new ScheduleChart(startTime, endTime, x.schedulingCodeId);
            tommorrowsChart.charts.push(schedulechart);
          }
        });
        this.sortChartData(tommorrowsChart.charts);
        this.sortChartData(todaysChart.charts);
        chart.managerChart.push(todaysChart);
        chart.managerChart.push(tommorrowsChart);
      }
      this.activityLogsChart.push(chart);
    });
    this.setManagerAgentFilters();
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

  private convertDateTimeToMeridiemTime(time: Date) {
    let convertedTime = '';
    if (time) {
      const hour = new Date(time).getUTCHours();
      const minutes = new Date(time).getUTCMinutes();
      const hours = hour < 10 ? '0' + hour : hour;
      const minute = minutes < 10 ? '0' + minutes : minutes;
      if (hour > 12) {
        convertedTime = hours + ':' + minute + ' ' + 'pm';
      } else {
        convertedTime = hours + ':' + minute + ' ' + 'am';
      }
    }

    return convertedTime;
  }

  private convertDateTimeToLocalMeridiemTime(time: Date) {
    let convertedTime = '';
    if (time) {
      let hour = String(time).split('T')[1].slice(0, 2);
      const minutes = String(time).split('T')[1].slice(3, 5);
      if (+hour > 12) {
        hour = String(+hour - 12);
        convertedTime = hour + ':' + minutes + ' ' + 'pm';
      } else {
        convertedTime = hour + ':' + minutes + ' ' + 'am';
      }
    }

    return convertedTime;
  }

  private getFormattedDate(date: Date) {
    const transformedDate = this.datepipe.transform(date, 'yyyy-MM-dd');
    return new Date(transformedDate);
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
