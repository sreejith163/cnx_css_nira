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
import { OpenTimeData, ScheduleManagerChart } from '../../../models/schedule-manager-chart.model';
import { ActivityLogsManagerChart, AgentScheduleManagerChart } from '../../../models/activity-log-schedule-manager.model';
import { ActivityLogsScheduleManagerResponse } from '../../../models/activity-logs-schedule-manager-response.model';


@Component({
  selector: 'app-activity-logs-schedule-manager',
  templateUrl: './activity-logs-schedule-manager.component.html',
  styleUrls: ['./activity-logs-schedule-manager.component.scss'],
  providers: [DatePipe]
})
export class ActivityLogsScheduleManagerComponent implements OnInit, OnDestroy {

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
  openTimes: Array<OpenTimeData>;
  activityLogsData: ActivityLogsScheduleManagerResponse[] = [];
  activityLogsChart: ActivityLogsManagerChart[] = [];

  getActivityLogsSubscription: ISubscription;
  subscriptions: ISubscription[] = [];

  @Input() activityType: ActivityType;
  @Input() employeeId: string;
  @Input() employeeName: string;
  @Input() startDate: string;
  // @Input() dateFrom: Date;
  // @Input() dateTo: Date;
  @Input() schedulingCodes: SchedulingCode[] = [];

  timeStampUpdate: number;

  constructor(
    private datepipe: DatePipe,
    public translate: TranslateService,
    public activeModal: NgbActiveModal,
    private spinnerService: NgxSpinnerService,
    private activityLogService: ActivityLogsService,
    private excelService: ExcelService,
  ) { }

  ngOnInit(): void {

    this.columnList = ['Employee Id', 'Time Stamp', 'Executed By', 'Origin', 'Status'];
    
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

  // getIconFromSelectedAgent(id: number, openTime: string) {
  //   if (this.activityLogsChart.length > 0) {
  //     const item = this.activityLogsChart.find(x => x.id === id);
  //     const weekTimeData = item?.agentScheduleChart?.charts?.find(x => this.convertToDateFormat(openTime) >=
  //       this.convertToDateFormat(x?.startTime) &&
  //       this.convertToDateFormat(openTime) < this.convertToDateFormat(x?.endTime));
  //     if (weekTimeData) {
  //       const code = this.schedulingCodes.find(x => x.id === weekTimeData?.schedulingCodeId);
  //       return code ? this.unifiedToNative(code?.icon?.value) : '';
  //     }
  //   }

  //   return '';
  // }

  // getAgentIconDescription(id: number, openTime: string) {
  //   if (this.activityLogsChart.length > 0) {
  //     const item = this.activityLogsChart.find(x => x.id === id);
  //     const weekTimeData = item?.agentScheduleChart?.charts?.find(x => this.convertToDateFormat(openTime) >=
  //       this.convertToDateFormat(x?.startTime) &&
  //       this.convertToDateFormat(openTime) < this.convertToDateFormat(x?.endTime));
  //     if (weekTimeData) {
  //       const code = this.schedulingCodes?.find(x => x.id === weekTimeData?.schedulingCodeId);
  //       return code?.description;
  //     }
  //   }

  //   return '';
  // }

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

    this.spinnerService.show(this.spinner, SpinnerOptions);
    let columnNames = ["EmployeeId", "Executed By", "Origin", "Status",	"Timestamp"];
    let dateTimeHeaders: string[] = [" "," "," "," "," "];

    this.openTimes.map(t =>{
      columnNames.push(t.time)
      dateTimeHeaders.push(`${this.datepipe.transform(new Date(t.dateHeader), "MMM d")}`);
    });

    let csv = dateTimeHeaders.join(',');
    csv += '\r\n';

    csv += columnNames.join(',');
    csv += '\r\n';
  
    let exportData = this.activityLogsChart;
    
    let date = this.startDate.toString();
    let fileName = `${this.exportFileName + date}.csv`;

    exportData.map(x=>{
        let data:string[] = [];
        data = [x.employeeId.toString(), x.executedBy, ActivityOrigin[x.activityOrigin], ActivityStatus[x.activityStatus], this.getDateInStringFormat(x.timeStamp)];

        this.openTimes.map(t =>{
          let code = x.agentScheduleChart.charts.find(c => this.getTimeStamp(c?.startDateTime) <= this.getTimeStamp(t.date) &&
          this.getTimeStamp(c?.endDateTime) > this.getTimeStamp(t.date))?.schedulingCodeId;
          
          let icon = this.schedulingCodes.find(x => x?.id === code)?.description.toString();
          data.push(icon);
        });
        csv += data.join(',')
        csv += '\r\n';
    });

    var blob = new Blob([csv], { type: "text/csv" });

    var link = document.createElement("a");
    if (link.download !== undefined) {
      var url = URL.createObjectURL(blob);
      link.setAttribute("href", url);
      link.setAttribute("download", fileName);
      document.body.appendChild(link);
      link.click();
      document.body.removeChild(link);
      this.spinnerService.hide(this.spinner);
    }
  }

  exportCSV() {
    const exportSchedule = new Array<any>();
    for (const item of this.activityLogsChart) {
      const model: any = {};
      model.EmployeeId = +item?.employeeId;
      if (this.activityType === ActivityType.SchedulingGrid) {
        model.Day = WeekDay[item?.day];
      }
      model.ExecutedBy = item?.executedBy;
      model.Origin = ActivityOrigin[item?.activityOrigin];
      model.Status = ActivityStatus[item?.activityStatus];
      model.TimeStamp = this.getDateInStringFormat(item?.timeStamp);

      for (const time of this.openTimes) {
        const code = item?.agentScheduleChart?.charts
          .find(x => this.getTimeStamp(x?.startDateTime) <= this.getTimeStamp(time.date) &&
          this.getTimeStamp(x?.endDateTime) > this.getTimeStamp(time.date))?.schedulingCodeId;
        const formattedTime = this.formatTimeFormat(time.time);
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
    queryParams.dateFrom = undefined;
    queryParams.dateto = undefined;
    queryParams.date = this.activityType === ActivityType.SchedulingManagerGrid ? this.startDate : undefined;

    return queryParams;
  }

  private loadActivityLogs() {
    const queryParams = this.getQueryParams();
    this.spinnerService.show(this.spinner, SpinnerOptions);

    this.getActivityLogsSubscription = this.activityLogService.getActivityLogs(queryParams)
      .subscribe((response) => {
        console.log(queryParams.date, response)
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
    this.setManagerChartData();
    this.totalRecord = this.activityLogsChart?.length;
  }

  private setAgentFilters() {
    const openTime = this.schedulingCodes.find(x => x?.description?.trim().toLowerCase() === 'open time');
    if (openTime) {
      const openTimeIndex = this.activityLogsChart[0]?.agentScheduleChart?.charts.findIndex(x => x?.schedulingCodeId === +openTime?.id);
      if (openTimeIndex > -1) {
        this.iconCode = openTime?.icon?.value;
        this.iconDescription = openTime?.description;
        this.startTimeFilter = this.formatTimeFormat(this.activityLogsChart[0]?.agentScheduleChart?.charts[openTimeIndex]?.startDateTime);
        this.endTimeFilter = this.formatTimeFormat(this.activityLogsChart[0]?.agentScheduleChart?.charts[openTimeIndex]?.endDateTime);
      } else {
        const codeId = this.activityLogsChart[0]?.agentScheduleChart?.charts[0]?.schedulingCodeId;
        const code = this.schedulingCodes.find(x => x.id === codeId);
        this.iconCode = code?.icon?.value;
        this.iconDescription = code?.description;
        this.startTimeFilter = this.formatTimeFormat(this.activityLogsChart[0]?.agentScheduleChart?.charts[0]?.startDateTime);
        this.endTimeFilter = this.formatTimeFormat(this.activityLogsChart[0]?.agentScheduleChart?.charts[0]?.endDateTime);
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



  private setManagerChartData() {
    this.activityLogsData.forEach((item, index) => {
      const chart = new ActivityLogsManagerChart();
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
      } 
      this.sortChartData(item?.schedulingFieldDetails?.activityLogManager?.charts);
      chart.agentScheduleChart = new AgentScheduleManagerChart();
      chart.agentScheduleChart.charts = item?.schedulingFieldDetails?.activityLogManager?.charts;
      this.activityLogsChart.push(chart);
    });
    this.setAgentFilters();
  }

  private sortChartData(chart: ScheduleManagerChart[]) {
    if (chart.length > 0) {
      chart.sort((a, b): number => {
        if (this.getTimeStamp(a.startDateTime) < this.getTimeStamp(b.startDateTime)) {
          return -1;
        } else if (this.getTimeStamp(a.startDateTime) > this.getTimeStamp(b.startDateTime)) {
          return 1;
        }
        else {
          return 0;
        }
      });
    }
  }

  getTimeStamp(date: any){
    if(date){
      date = new Date(date)?.getTime()
    }

    return date;
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
    let x = this.timeIntervals;
    var times: OpenTimeData[] = [];
    let tt = 0;

    var now;

    if (this.startDate) {
      // now = this.changeToUTCDate(this.startDate);
      now = new Date(`${this.startDate} 00:00`);
    } else {
      now = new Date(new Date().setHours(0, 0, 0, 0));
    }

    for (let i = 0; tt < 48 * 60; i++) {
      let time = this.datepipe.transform(now, 'hh:mm a').toLowerCase();

      times[i] = new OpenTimeData;
      times[i].time = time;

      times[i].date = this.changeToUTCDate(now).toISOString().slice(0, -5) + "Z";
      times[i].dateHeader = this.datepipe.transform(now, 'yyyy-MM-dd');

      times[i].startDateTime = this.changeToUTCDate(now).toISOString().slice(0, -5) + "Z";
      let et = this.addIntervalDate(now, 'minute', x);
      times[i].endDateTime = this.changeToUTCDate(et).toISOString().slice(0, -5) + "Z";

      now = this.addIntervalDate(now, 'minute', x);
      tt = tt + x;
    }

    return times;

  }


  private changeToUTCDate(date){
    return new Date(new Date(date).toString().replace(/\sGMT.*$/, " GMT+0000"));
  }

  private addIntervalDate(date, interval, units) {
    if(!(date instanceof Date))
      return undefined;
    var ret = new Date(date); //don't change original date
    var checkRollover = function() { if(ret.getDate() != date.getDate()) ret.setDate(0);};
    switch(String(interval).toLowerCase()) {
      case 'year'   :  ret.setFullYear(ret.getFullYear() + units); checkRollover();  break;
      case 'quarter':  ret.setMonth(ret.getMonth() + 3*units); checkRollover();  break;
      case 'month'  :  ret.setMonth(ret.getMonth() + units); checkRollover();  break;
      case 'week'   :  ret.setDate(ret.getDate() + 7*units);  break;
      case 'day'    :  ret.setDate(ret.getDate() + units);  break;
      case 'hour'   :  ret.setTime(ret.getTime() + units*3600000);  break;
      case 'minute' :  ret.setTime(ret.getTime() + units*60000);  break;
      case 'second' :  ret.setTime(ret.getTime() + units*1000);  break;
      default       :  ret = undefined;  break;
    }
    return ret;
  }

}
