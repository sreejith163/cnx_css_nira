import { Component, Input, OnChanges, OnDestroy, OnInit } from '@angular/core';
import { NgbCalendar, NgbDate, NgbModal, NgbModalOptions, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { NgxSpinnerService } from 'ngx-spinner';
import { AuthService } from 'src/app/core/services/auth.service';
import { SpinnerOptions } from 'src/app/shared/util/spinner-options.util';
import { AgentSchedulesResponse } from '../../../models/agent-schedules-response.model';
import { UpdateAgentSchedule } from '../../../models/update-agent-schedule.model';
import { AgentSchedulesService } from '../../../services/agent-schedules.service';
import { SubscriptionLike as ISubscription } from 'rxjs';
import { AgentScheduleGridResponse } from '../../../models/agent-schedule-grid-response.model';
import { SchedulingCode } from '../../../../system-admin/models/scheduling-code.model';
import { ScheduleChart } from '../../../models/schedule-chart.model';
import { AgentScheduleChart } from '../../../models/agent-schedule-chart.model';
import { AgentScheduleType } from '../../../enums/agent-schedule-type.enum';
import { CdkDragDrop } from '@angular/cdk/drag-drop';
import { HeaderPagination } from 'src/app/shared/models/header-pagination.model';
import * as $ from 'jquery';
import { MessagePopUpComponent } from 'src/app/shared/popups/message-pop-up/message-pop-up.component';
import { UpdateAgentschedulechart } from '../../../models/update-agent-schedule-chart.model';
import { AgentSchedulesQueryParams } from '../../../models/agent-schedules-query-params.model';
import { WeekDay } from '@angular/common';
import { CopyScheduleComponent } from '../copy-schedule/copy-schedule.component';
import { SchedulingStatus } from '../../../enums/scheduling-status.enum';
import { Constants } from 'src/app/shared/util/constants.util';
import { animate, state, style, transition, trigger } from '@angular/animations';
import { ActivityLogsComponent } from '../activity-logs/activity-logs.component';
import { ActivityOrigin } from '../../../enums/activity-origin.enum';
import { ActivityType } from 'src/app/shared/enums/activity-type.enum';

declare function setRowCellIndex(cell: string);
declare function highlightSelectedCells(table: string, cell: string);
declare function removeHighlightedCells(table: string, className: string);
declare function highlightCell(cell: string, className: string);

@Component({
  selector: 'app-scheduling',
  animations: [
    trigger(
      'enterAnimation', [
      state('true', style({ opacity: 1, height: 'auto' })),
      state('void', style({ opacity: 0, height: 0 })),
      transition(':enter', animate('400ms ease-in-out')),
      transition(':leave', animate('400ms ease-in-out'))
    ]
    )
  ],
  templateUrl: './scheduling.component.html',
  styleUrls: ['./scheduling.component.scss']
})
export class SchedulingComponent implements OnInit, OnDestroy, OnChanges {
  timeIntervals = 15;
  currentPage = 1;
  pageSize = 3;
  characterSplice = 25;
  maxIconCount = 30;
  iconCount: number;
  startIcon = 0;
  endIcon: number;
  totalSchedulingRecord: number;

  selectedIconId: string;
  icon: string;
  spinner = 'scheduling-tab';
  scheduleSpinner = 'scheduling-spinner';
  selectedCellClassName = 'cell-selected';
  tableClassName = 'schedulingGridTable';
  orderBy = 'createdDate';
  sortBy = 'desc';

  isMouseDown: boolean;
  isDelete: boolean;

  selectedGrid: AgentScheduleGridResponse;
  schedulingGridData: AgentScheduleGridResponse;
  modalRef: NgbModalRef;
  scheduleStatus = SchedulingStatus;
  paginationSize = Constants.schedulingPaginationSize;
  schedulingIntervals = Constants.schedulingIntervals;
  weekDay = WeekDay;

  hoveredDate: NgbDate | null = null;
  toDate: NgbDate | null = null;
  fromDate = this.calendar.getToday();
  today = this.calendar.getToday();

  totalSchedulingGridData: AgentSchedulesResponse[] = [];
  openTimes: Array<any>;
  weekDays: Array<string> = [];
  schedulingStatus: any[] = [];

  updateAgentScheduleChartSubscription: ISubscription;
  getAgentSchedulesSubscription: ISubscription;
  getAgentScheduleSubscription: ISubscription;
  updateAgentScheduleSubscription: ISubscription;
  subscriptions: ISubscription[] = [];

  @Input() currentLanguage: string;
  @Input() searchText: string;
  @Input() startDate: string;
  @Input() agentSchedulingGroupId: number;
  @Input() tabIndex: number;
  @Input() refreshSchedulingTab: boolean;
  @Input() schedulingCodes: SchedulingCode[] = [];

  constructor(
    private calendar: NgbCalendar,
    private spinnerService: NgxSpinnerService,
    private authService: AuthService,
    private agentSchedulesService: AgentSchedulesService,
    private modalService: NgbModal,
  ) { }

  ngOnInit(): void {
    this.schedulingStatus = Object.keys(SchedulingStatus).filter(key => isNaN(SchedulingStatus[key]));
    this.openTimes = this.getOpenTimes();
    this.weekDays = Object.keys(WeekDay).filter(key => isNaN(WeekDay[key]));
  }

  ngOnDestroy() {
    this.subscriptions.forEach(subscription => {
      if (subscription) {
        subscription.unsubscribe();
      }
    });
  }

  ngOnChanges() {
    if (this.tabIndex === AgentScheduleType.Scheduling) {
      this.iconCount = (this.schedulingCodes.length <= 30) ? this.schedulingCodes.length : this.maxIconCount;
      this.endIcon = this.iconCount;
      if (this.agentSchedulingGroupId) {
        this.loadAgentSchedules();
      } else {
        this.totalSchedulingGridData = [];
      }
      if (this.refreshSchedulingTab) {
        this.selectedGrid = null;
      }
    }

  }

  getGridMaxWidth() {
    return window.innerWidth > 1350 ? (window.innerWidth - 350) + 'px' : '1250px';
  }

  cancel() {
    if (this.matchSchedulingGridDataChanges()) {
      this.selectedGrid = JSON.parse(JSON.stringify(this.schedulingGridData));
    } else {
      this.getModalPopup(MessagePopUpComponent, 'sm', 'No changes has been made!');
    }
  }

  onClickDateIcon(index: number) {
    const fromDate = this.totalSchedulingGridData[index]?.dateFrom;
    const to = this.totalSchedulingGridData[index]?.dateTo;
    this.fromDate = this.convertToNgbDate(fromDate) ?? this.today;
    this.toDate = this.convertToNgbDate(to) ?? this.today;
  }

  onDateSelection(date: NgbDate, index: number) {
    if (!this.fromDate && !this.toDate) {
      this.fromDate = date;
      const newDate = new Date(this.fromDate.year, this.fromDate.month - 1, this.fromDate.day, 0, 0, 0, 0);
      this.totalSchedulingGridData[index].dateFrom = newDate;
    } else if (this.fromDate && !this.toDate && date && !date.before(this.fromDate)) {
      this.toDate = date;
      const newDate = new Date(this.toDate.year, this.toDate.month - 1, this.toDate.day, 0, 0, 0, 0);
      this.totalSchedulingGridData[index].dateTo = newDate;
    } else {
      this.toDate = null;
      this.totalSchedulingGridData[index].dateTo = null;
      this.fromDate = date;
      const newDate = new Date(this.fromDate.year, this.fromDate.month - 1, this.fromDate.day, 0, 0, 0, 0);
      this.totalSchedulingGridData[index].dateFrom = newDate;
    }
    this.updateAgentSchedule(index);
  }

  isHovered(date: NgbDate) {
    return this.fromDate && !this.toDate && this.hoveredDate && date.after(this.fromDate) && date.before(this.hoveredDate);
  }

  isInside(date: NgbDate) {
    return this.toDate && date.after(this.fromDate) && date.before(this.toDate);
  }

  isRange(date: NgbDate) {
    return date.equals(this.fromDate) || (this.toDate && date.equals(this.toDate)) || this.isInside(date) || this.isHovered(date);
  }

  unifiedToNative(unified: string) {
    if (unified) {
      const codePoints = unified.split('-').map(u => parseInt(`0x${u}`, 16));
      return String.fromCodePoint(...codePoints);
    }
  }

  toggleDetails(el: AgentSchedulesResponse) {
    if (this.selectedGrid && this.selectedGrid.employeeId === el.employeeId) {
      this.selectedGrid = null;
    } else {
      this.loadAgentSchedule(el.id);
    }
  }

  getIconFromSelectedGrid(week: number, openTime: any) {
    const weekData = this.selectedGrid.agentScheduleCharts.find(x => x.day === +week);

    if (weekData) {
      const weekTimeData = weekData.charts.find(x => this.convertToDateFormat(openTime) >= this.convertToDateFormat(x.startTime) &&
        this.convertToDateFormat(openTime) < this.convertToDateFormat(x.endTime));
      if (weekTimeData) {
        const code = this.schedulingCodes.find(x => x.id === weekTimeData.schedulingCodeId);
        return code ? this.unifiedToNative(code?.icon?.value) : '';
      }
    }

    return '';
  }

  previous() {
    if (this.startIcon > 0) {
      this.startIcon = this.startIcon - 1;
      this.endIcon = this.endIcon - 1;
    }
  }

  next() {
    if (this.endIcon < this.schedulingCodes.length) {
      this.startIcon = this.startIcon + 1;
      this.endIcon = this.endIcon + 1;
    }
  }

  toBeginning() {
    this.startIcon = 0;
    this.endIcon = this.iconCount;
  }

  toEnd() {
    this.startIcon = this.schedulingCodes.length - this.iconCount;
    this.endIcon = this.schedulingCodes.length;
  }

  onIconClick(event) {
    this.selectedIconId = event.target.id;
  }

  isMainMinute(data: any) {
    return data.split(':')[1] === '00 am' || data.split(':')[1] === '00 pm';
  }

  onMouseUp(event) {
    this.isMouseDown = false;
    if (this.isDelete) {
      this.saveGridItems();
    } else {
      highlightCell(event.currentTarget.id, this.selectedCellClassName);
      this.saveGridItems();
    }
  }

  onMouseDown(event) {
    let days;
    this.isMouseDown = true;
    const time = event.currentTarget.attributes.time.nodeValue;
    const meridiem = event.currentTarget.attributes.meridiem.nodeValue;
    if (this.tabIndex === AgentScheduleType.Scheduling) {
      const week = event.currentTarget.attributes.week.nodeValue;
      days = this.selectedGrid.agentScheduleCharts.find(x => x.day === +week);
    }
    const fromTime = time + ' ' + meridiem;
    const object = days?.charts.find(x => this.convertToDateFormat(x.startTime) <= this.convertToDateFormat(fromTime) &&
      this.convertToDateFormat(x.endTime) > this.convertToDateFormat(fromTime));
    if (object) {
      const code = this.schedulingCodes.find(x => x.id === object.schedulingCodeId);
      this.icon = code?.icon?.value ?? undefined;
      if (this.isMouseDown && this.icon) {
        setRowCellIndex(event.currentTarget.id);
      }
    }
    if (this.isMouseDown && !this.icon) {
      this.isDelete = true;
      setRowCellIndex(event.currentTarget.id);
    }
  }

  onMouseOver(event) {
    this.removeHighlightedCells();
    if (this.isMouseDown && this.icon) {
      highlightSelectedCells(this.tableClassName, event.currentTarget.id);
    } else if (this.isMouseDown && this.isDelete) {
      highlightSelectedCells(this.tableClassName, event.currentTarget.id);
    }
  }

  dragged(event: CdkDragDrop<any>) {
    this.icon = event.item.element.nativeElement.id;
  }

  changeTimeInterval(interval: number) {
    this.timeIntervals = +interval;
    this.openTimes = this.getOpenTimes();
  }

  changePageSize(pageSize: number) {
    if (this.agentSchedulingGroupId) {
      this.pageSize = pageSize;
      this.loadAgentSchedules();
    }
  }

  changePage(page: number) {
    if (this.agentSchedulingGroupId) {
      this.currentPage = page;
      this.loadAgentSchedules();
    }
  }

  setSchedulingStatus(status: string, index: number) {
    this.totalSchedulingGridData[index].status = Number(status);
    this.updateAgentSchedule(index);
  }

  sort(columnName: string, sortBy: string) {
    if (this.agentSchedulingGroupId) {
      this.sortBy = sortBy === 'asc' ? 'desc' : 'asc';
      this.orderBy = columnName;

      this.loadAgentSchedules();
    }
  }

  save(gridChart: AgentScheduleGridResponse) {
    this.updateAgentScheduleChart(gridChart.id);
  }

  openActivityLogs(index: number) {
    this.getModalPopup(ActivityLogsComponent, 'xl');
    this.modalRef.componentInstance.activityType = ActivityType.SchedulingGrid;
    this.modalRef.componentInstance.employeeId = this.totalSchedulingGridData[index].employeeId;
    this.modalRef.componentInstance.employeeName = this.selectedGrid.lastName + ' ' + this.selectedGrid.firstName;
    this.modalRef.componentInstance.startDate = new Date(this.startDate);
  }

  openCopySchedule(index: number) {
    const agentScheduleId = this.totalSchedulingGridData[index]?.id;
    const employeeId = this.totalSchedulingGridData[index]?.employeeId;
    this.getModalPopup(CopyScheduleComponent, 'lg');
    this.modalRef.componentInstance.agentSchedulingGroupId = this.schedulingGridData?.agentSchedulingGroupId;
    this.modalRef.componentInstance.agentScheduleId = agentScheduleId;
    this.modalRef.componentInstance.employeeId = employeeId;
    this.modalRef.componentInstance.agentScheduleType = this.tabIndex;

    this.modalRef.result.then((result) => {
      if (result.needRefresh) {
        this.getModalPopup(MessagePopUpComponent, 'sm', 'The record has been copied!');
        this.modalRef.result.then(() => {
          if (this.tabIndex === AgentScheduleType.Scheduling) {
            this.loadAgentSchedule(agentScheduleId);
          }
          this.loadAgentSchedules();
        });
      } else {
        this.getModalPopup(MessagePopUpComponent, 'sm', 'No changes has been made!');
      }
    });

  }

  getIconDescription(week: number, openTime: any) {
    const weekData = this.selectedGrid.agentScheduleCharts.find(x => x.day === +week);

    if (weekData) {
      const weekTimeData = weekData.charts.find(x => this.convertToDateFormat(openTime) >= this.convertToDateFormat(x.startTime) &&
        this.convertToDateFormat(openTime) < this.convertToDateFormat(x.endTime));
      if (weekTimeData) {
        const code = this.schedulingCodes.find(x => x.id === weekTimeData.schedulingCodeId);
        return code?.description;
      }
    }

    return '';

  }

  private removeHighlightedCells() {
    const table = $('#' + this.tableClassName);
    table.find('.' + this.selectedCellClassName).removeClass(this.selectedCellClassName);
  }

  private saveGridItems() {
    const table = $('#' + this.tableClassName);
    table.find('.' + this.selectedCellClassName).each((index, elem) => {
      let weekDays;
      let weekData;
      let week;
      let to;
      this.spinnerService.show(this.scheduleSpinner, SpinnerOptions);
      let meridiem = elem.attributes.meridiem.value;
      const fromTime = elem.attributes.time.value + ' ' + meridiem;
      let hours = fromTime.split(':')[0];
      const minuteValue = Number(fromTime.split(':')[1].split(' ')[0]) + this.timeIntervals;
      let minute = minuteValue === 5 ? '05' : minuteValue;

      if (minute === 60) {
        meridiem = (hours === '11' && meridiem === 'am') ? 'pm' : meridiem;
        hours = hours !== '11' ? ('0' + (Number(hours) + 1)).slice(-2) : '00';
        minute = '00';
      }

      if (hours === '00' && minute === '00' && meridiem === 'pm' && fromTime.split(' ')[1] === 'pm') {
        to = '11:60 pm';
      } else {
        to = hours + ':' + minute + ' ' + meridiem;
      }
      const code = this.schedulingCodes.find(x => x?.icon?.value?.trim()?.toLowerCase() === this.icon?.trim()?.toLowerCase());
      const iconModel = new ScheduleChart(fromTime, to, code?.id);


      if (this.tabIndex === AgentScheduleType.Scheduling) {
        week = elem.attributes.week.value;
        weekDays = this.selectedGrid?.agentScheduleCharts;
        weekData = weekDays.find(x => x.day === +week);
      }

      if (this.icon && !this.isDelete) {

        if (weekData) {
          this.insertIconToGrid(weekData, iconModel);
        } else if (this.tabIndex === AgentScheduleType.Scheduling) {
          const weekDay = new AgentScheduleChart();
          weekDay.day = +week;
          const calendarTime = new ScheduleChart(fromTime, to, code.id);
          weekDay.charts.push(calendarTime);
          weekDays.push(weekDay);
        }
      } else if (this.isDelete) {
        if (weekData) {
          this.clearIconFromGrid(weekData, iconModel);
        }
      }
    });
    this.spinnerService.hide(this.scheduleSpinner);
    this.isDelete = false;
    this.icon = undefined;
    this.sortSelectedGridCalendarTimes();
    this.formatTimeValuesInSchedulingGrid();

    table.find('.' + this.selectedCellClassName).removeClass(this.selectedCellClassName);
  }

  private insertIconToGrid(weekData: AgentScheduleChart, insertIcon: ScheduleChart) {
    if (weekData.charts.find(x => x.startTime === insertIcon.startTime && x.endTime === insertIcon.endTime)) {
      const item = weekData.charts.find(x => x.startTime === insertIcon.startTime && x.endTime === insertIcon.endTime);
      item.schedulingCodeId = insertIcon.schedulingCodeId;
    } else if (weekData.charts.filter(x => this.convertToDateFormat(x.startTime) >= this.convertToDateFormat(insertIcon.startTime) &&
      this.convertToDateFormat(x.endTime) <= this.convertToDateFormat(insertIcon.endTime)).length > 0) {
      const timeDataArray = weekData.charts.filter(x => this.convertToDateFormat(x.startTime) >=
        this.convertToDateFormat(insertIcon.startTime) &&
        this.convertToDateFormat(x.endTime) <= this.convertToDateFormat(insertIcon.endTime));
      timeDataArray.forEach(ele => {
        ele.schedulingCodeId = insertIcon.schedulingCodeId;
      });
      this.sortSelectedGridCalendarTimes();
      this.formatTimeValuesInSchedulingGrid();
    }
    if (!weekData.charts.find(x => x.startTime === insertIcon.startTime && x.endTime ===
      insertIcon.endTime && x.schedulingCodeId === insertIcon.schedulingCodeId)) {
      weekData.charts.forEach(ele => {
        if (this.convertToDateFormat(ele.startTime) < this.convertToDateFormat(insertIcon.startTime) &&
          this.convertToDateFormat(ele.endTime) === this.convertToDateFormat(insertIcon.startTime)) {
          ele.endTime = insertIcon.startTime;
        } else if (this.convertToDateFormat(ele.startTime) > this.convertToDateFormat(insertIcon.startTime) &&
          this.convertToDateFormat(ele.endTime) <= this.convertToDateFormat(insertIcon.endTime)) {
          ele.startTime = insertIcon.endTime;
        } else if (this.convertToDateFormat(ele.startTime) === this.convertToDateFormat(insertIcon.startTime) &&
          this.convertToDateFormat(ele.endTime) > this.convertToDateFormat(insertIcon.endTime)) {
          ele.startTime = insertIcon.endTime;
        } else if (this.convertToDateFormat(ele.startTime) < this.convertToDateFormat(insertIcon.startTime) &&
          this.convertToDateFormat(insertIcon.endTime) < this.convertToDateFormat(ele.endTime)) {
          const calendarTime = new ScheduleChart(insertIcon.endTime, ele.endTime, ele.schedulingCodeId);
          weekData.charts.push(calendarTime);
          ele.endTime = insertIcon.startTime;
        } else if (this.convertToDateFormat(ele.endTime) === this.convertToDateFormat(insertIcon.endTime) &&
          this.convertToDateFormat(ele.startTime) < this.convertToDateFormat(insertIcon.startTime)) {
          ele.endTime = insertIcon.startTime;
        }
      });
      const timeDataArray = weekData.charts.filter(x => this.convertToDateFormat(x.startTime) >=
        this.convertToDateFormat(insertIcon.startTime) &&
        this.convertToDateFormat(x.endTime) <= this.convertToDateFormat(insertIcon.endTime));
      if (timeDataArray.length > 0) {
        timeDataArray.forEach(ele => {
          ele.schedulingCodeId = insertIcon.schedulingCodeId;
        });
      } else {
        const calendarTime = new ScheduleChart(insertIcon.startTime, insertIcon.endTime, insertIcon.schedulingCodeId);
        weekData.charts.push(calendarTime);
      }
    }
  }

  private clearIconFromGrid(weekData: AgentScheduleChart, icon: ScheduleChart) {
    if (weekData.charts.findIndex(x => x.startTime === icon.startTime && x.endTime === icon.endTime) > -1) {
      const startIndex = weekData.charts.findIndex(x => x.startTime === icon.startTime && x.endTime === icon.endTime);
      weekData.charts.splice(startIndex, 1);
    } else if (weekData.charts.filter(x => this.convertToDateFormat(x.startTime) >= this.convertToDateFormat(icon.startTime) &&
      this.convertToDateFormat(x.endTime) <= this.convertToDateFormat(icon.endTime)).length > 0) {
      const timeDataArray = weekData.charts.filter(x => this.convertToDateFormat(x.startTime) >= this.convertToDateFormat(icon.startTime) &&
        this.convertToDateFormat(x.endTime) <= this.convertToDateFormat(icon.endTime));
      timeDataArray.forEach(ele => {
        const startIndex = weekData.charts.findIndex(x =>
          this.convertToDateFormat(x.startTime) === this.convertToDateFormat(ele.startTime));
        if (startIndex > -1) {
          weekData.charts.splice(startIndex, 1);
        }
      });
    }
    if (weekData.charts.findIndex(x => x.startTime === icon.startTime && x.endTime === icon.endTime) < 0) {
      weekData.charts.forEach(ele => {
        if (this.convertToDateFormat(ele.startTime) < this.convertToDateFormat(icon.startTime) &&
          this.convertToDateFormat(ele.endTime) === this.convertToDateFormat(icon.startTime)) {
          ele.endTime = icon.startTime;
        } else if (this.convertToDateFormat(ele.startTime) > this.convertToDateFormat(icon.startTime) &&
          this.convertToDateFormat(ele.endTime) <= this.convertToDateFormat(icon.endTime)) {
          ele.startTime = icon.endTime;
        } else if (ele.startTime === icon.startTime && this.convertToDateFormat(ele.endTime) > this.convertToDateFormat(icon.endTime)) {
          ele.startTime = icon.endTime;
        } else if (this.convertToDateFormat(ele.startTime) < this.convertToDateFormat(icon.startTime) &&
          this.convertToDateFormat(icon.endTime) < this.convertToDateFormat(ele.endTime)) {
          const calendarTime = new ScheduleChart(icon.endTime, ele.endTime, ele.schedulingCodeId);
          weekData.charts.push(calendarTime);
          ele.endTime = icon.startTime;
        } else if (this.convertToDateFormat(ele.endTime) === this.convertToDateFormat(icon.endTime) &&
          this.convertToDateFormat(ele.startTime) < this.convertToDateFormat(icon.startTime)) {
          ele.endTime = icon.startTime;
        }
      });
      const timeDataArray = weekData.charts.filter(x => this.convertToDateFormat(x.startTime) >= this.convertToDateFormat(icon.startTime) &&
        this.convertToDateFormat(x.endTime) <= this.convertToDateFormat(icon.endTime));
      if (timeDataArray.length > 0) {
        timeDataArray.forEach(ele => {
          const startIndex = weekData.charts.findIndex(x =>
            this.convertToDateFormat(x.startTime) === this.convertToDateFormat(ele.startTime));
          if (startIndex > -1) {
            weekData.charts.splice(startIndex, 1);
          }
        });
      }
    }
    if (weekData.charts.length === 0) {
      const index = this.selectedGrid?.agentScheduleCharts.findIndex(x => x.day === weekData.day);
      this.selectedGrid.agentScheduleCharts.splice(index, 1);
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

  private formatTimeValuesInSchedulingGrid() {
    const weekDays = this.selectedGrid.agentScheduleCharts;
    weekDays.forEach((element) => {
      element.charts = this.adjustSchedulingCalendarTimesRange(element.charts);
    });
  }

  private sortSelectedGridCalendarTimes() {
    const weekDays = this.selectedGrid.agentScheduleCharts;
    weekDays.forEach((element) => {
      if (element.charts.length > 0) {
        element.charts.sort((a, b): number => {
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
    });
  }

  private getQueryParams(fields?: string) {
    const agentSchedulesQueryParams = new AgentSchedulesQueryParams();
    agentSchedulesQueryParams.agentSchedulingGroupId = this.agentSchedulingGroupId;
    agentSchedulesQueryParams.fromDate = this.getDateInStringFormat(this.startDate);
    agentSchedulesQueryParams.pageNumber = this.currentPage;
    agentSchedulesQueryParams.pageSize = this.pageSize;
    agentSchedulesQueryParams.searchKeyword = this.searchText ?? '';
    agentSchedulesQueryParams.orderBy = `${this.orderBy} ${this.sortBy}`;
    agentSchedulesQueryParams.fields = fields ?? undefined;
    agentSchedulesQueryParams.employeeIds = undefined;

    return agentSchedulesQueryParams;
  }

  private loadAgentSchedules(fields?: string) {
    const queryParams = this.getQueryParams(fields);
    this.spinnerService.show(this.spinner, SpinnerOptions);

    this.getAgentSchedulesSubscription = this.agentSchedulesService.getAgentSchedules(queryParams)
      .subscribe((response) => {
        this.totalSchedulingGridData = response.body;
        let headerPaginationValues = new HeaderPagination();
        headerPaginationValues = JSON.parse(response.headers.get('x-pagination'));
        this.totalSchedulingRecord = headerPaginationValues.totalCount;

        this.spinnerService.hide(this.spinner);
      }, (error) => {
        this.spinnerService.hide(this.spinner);
        console.log(error);
      });

    this.subscriptions.push(this.getAgentSchedulesSubscription);
  }

  private loadAgentSchedule(agentScheduleId: string) {
    this.spinnerService.show(this.scheduleSpinner, SpinnerOptions);

    this.getAgentScheduleSubscription = this.agentSchedulesService.getAgentSchedule(agentScheduleId)
      .subscribe((response) => {
        if (response) {
          this.selectedGrid = this.formatEndTime(response);
          this.selectedGrid.agentScheduleCharts.map(x => x?.charts.map(y => {
            y.startTime = y?.startTime.trim().toLowerCase();
            y.endTime = y?.endTime.trim().toLowerCase();
          }));
          this.schedulingGridData = JSON.parse(JSON.stringify(this.selectedGrid));
        }
        this.spinnerService.hide(this.scheduleSpinner);
      }, (error) => {
        this.spinnerService.hide(this.scheduleSpinner);
        console.log(error);
      });

    this.subscriptions.push(this.getAgentScheduleSubscription);
  }

  private updateAgentSchedule(index: number) {
    this.spinnerService.show(this.spinner, SpinnerOptions);
    const updateModel = new UpdateAgentSchedule();
    updateModel.dateFrom = this.totalSchedulingGridData[index].dateFrom;
    updateModel.dateTo = this.totalSchedulingGridData[index].dateTo;
    updateModel.status = this.totalSchedulingGridData[index].status;
    updateModel.modifiedBy = this.authService.getLoggedUserInfo()?.displayName;

    this.updateAgentScheduleSubscription = this.agentSchedulesService.
      updateAgentSchedule(this.totalSchedulingGridData[index].id, updateModel)
      .subscribe(() => {
        this.spinnerService.hide(this.spinner);
        if (this.selectedGrid) {
          this.selectedGrid.dateFrom = updateModel.dateFrom;
          this.selectedGrid.dateTo = updateModel.dateTo;
        }
        this.totalSchedulingGridData[index].modifiedBy = updateModel.modifiedBy;
        this.totalSchedulingGridData[index].modifiedDate = new Date();
      }, (error) => {
        this.spinnerService.hide(this.spinner);
        console.log(error);
      });

    this.subscriptions.push(this.updateAgentScheduleSubscription);
  }

  private matchSchedulingGridDataChanges() {
    if (JSON.stringify(this.schedulingGridData) !== JSON.stringify(this.selectedGrid)) {
      return true;
    }
  }

  private updateAgentScheduleChart(agentScheduleId: string) {
    if (this.matchSchedulingGridDataChanges()) {
      this.spinnerService.show(this.spinner, SpinnerOptions);
      const chartModel = new UpdateAgentschedulechart();
      const gridData = this.formatEndTime(this.selectedGrid);
      chartModel.agentScheduleCharts = gridData.agentScheduleCharts;
      chartModel.activityOrigin = ActivityOrigin.CSS;
      chartModel.modifiedUser = +this.authService.getLoggedUserInfo()?.employeeId;
      chartModel.modifiedBy = this.authService.getLoggedUserInfo()?.displayName;

      this.updateAgentScheduleChartSubscription = this.agentSchedulesService
        .updateAgentScheduleChart(agentScheduleId, chartModel)
        .subscribe(() => {
          this.spinnerService.hide(this.spinner);
          this.schedulingGridData = JSON.parse(JSON.stringify(this.selectedGrid));
          this.getModalPopup(MessagePopUpComponent, 'sm', 'The record has been updated!');
          this.modalRef.result.then(() => {
            this.loadAgentSchedules();
          });
        }, (error) => {
          this.spinnerService.hide(this.spinner);
          console.log(error);
        });

      this.subscriptions.push(this.updateAgentScheduleChartSubscription);
    } else {
      this.getModalPopup(MessagePopUpComponent, 'sm', 'No changes has been made!');
    }
  }

  private getModalPopup(component: any, size: string, contentMessage?: string) {
    const options: NgbModalOptions = { backdrop: 'static', centered: true, size };
    this.modalRef = this.modalService.open(component, options);
    this.modalRef.componentInstance.headingMessage = 'Success';
    this.modalRef.componentInstance.contentMessage = contentMessage;
  }


  private formatEndTime(scheduleResponse: AgentScheduleGridResponse) {
    for (const weekData of scheduleResponse.agentScheduleCharts) {
      const responseIndex = weekData?.charts.findIndex(x => x.endTime.trim().toLowerCase() === '00:00 am');
      if (responseIndex > -1) {
        weekData.charts[responseIndex].endTime = '11:60 pm';
      } else {
        const requestIndex = weekData?.charts.findIndex(x => x.endTime.trim().toLowerCase() === '11:60 pm');
        if (requestIndex > -1) {
          weekData.charts[requestIndex].endTime = '00:00 am';
        }
      }
    }

    return scheduleResponse;

  }

  private convertToNgbDate(date: Date) {
    if (date) {
      date = new Date(date);
      const newDate: NgbDate = new NgbDate(date.getUTCFullYear(), date.getUTCMonth() + 1, date.getUTCDate() + 1);
      return newDate ?? undefined;
    }
  }

  private convertToDateFormat(time: string) {
    if (time) {
      const count = time.split(' ')[1].toLowerCase() === 'pm' ? 12 : undefined;
      if (count) {
        time = (+time.split(':')[0] + 12) + ':' + time.split(':')[1].split(' ')[0];
      } else {
        time = time.split(':')[0] + ':' + time.split(':')[1].split(' ')[0];
      }

      return time;
    }
  }

  private getDateInStringFormat(startDate: any): string {
    if (!startDate) {
      return undefined;
    }

    const date = new Date(startDate);
    return date.toDateString();
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
