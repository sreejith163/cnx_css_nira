import { CdkDragDrop } from '@angular/cdk/drag-drop';
import { Component, Input, OnChanges, OnDestroy, OnInit } from '@angular/core';
import { NgbDate, NgbModal, NgbModalOptions, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { NgxSpinnerService } from 'ngx-spinner';
import { Constants } from 'src/app/shared/util/constants.util';
import { SpinnerOptions } from 'src/app/shared/util/spinner-options.util';
import { SortingType } from '../../../enums/sorting-type.enum';
import { AgentSchedulesQueryParams } from '../../../models/agent-schedules-query-params.model';
import { AgentSchedulesService } from '../../../services/agent-schedules.service';
import { from, SubscriptionLike as ISubscription } from 'rxjs';
import { AgentSchedulesResponse } from '../../../models/agent-schedules-response.model';
import { mergeMap } from 'rxjs/operators';
import { AgentChartResponse } from '../../../models/agent-chart-response.model';
import { SchedulingCodeQueryParams } from '../../../../system-admin/models/scheduling-code-query-params.model';
import { SchedulingCodeService } from 'src/app/shared/services/scheduling-code.service';
import { SchedulingCode } from '../../../../system-admin/models/scheduling-code.model';
import { ScheduleChart } from '../../../models/schedule-chart.model';
import { AgentScheduleChart } from '../../../models/agent-schedule-chart.model';
import { AgentScheduleType } from '../../../enums/agent-schedule-type.enum';
import { AgentScheduleManagerChart } from '../../../models/agent-schedule-manager-chart.model';
import { WeekDay } from '@angular/common';
import { SchedulingStatus } from '../../../enums/scheduling-status.enum';
import { CopyScheduleComponent } from '../copy-schedule/copy-schedule.component';
import { MessagePopUpComponent } from 'src/app/shared/popups/message-pop-up/message-pop-up.component';
import { AgentScheduleGridResponse } from '../../../models/agent-schedule-grid-response.model';

declare function setManagerRowCellIndex(cell, row);
declare function highlightManagerSelectedCells(table: string, cell: string);
declare function highlightCell(cell: string, className: string);
import * as $ from 'jquery';


@Component({
  selector: 'app-scheduling-manager',
  templateUrl: './scheduling-manager.component.html',
  styleUrls: ['./scheduling-manager.component.scss']
})
export class SchedulingManagerComponent implements OnInit, OnDestroy, OnChanges {
  startIcon = 0;
  maxIconCount = 30;
  currentPage = 1;
  pageSize = 3;
  timeIntervals = 15;
  endIcon: number;
  iconCount: number;
  selectedRow: number;
  totalSchedulingRecord: number;
  iconDescription: string;
  icon: string;
  iconCode: string;
  selectedIconId: string;
  startTimeFilter: string;
  endTimeFilter: string;
  orderBy = 'createdDate';
  sortBy = 'desc';
  spinner = 'schedulig-manager';
  scheduleSpinner = 'scheduling-spinner';
  selectedCellClassName = 'cell-selected';
  tableClassName = 'schedulingManagerTable';
  isMouseDown: boolean;
  isDelete: boolean;
  schedulingIntervals = Constants.schedulingIntervals;
  tabIndex = AgentScheduleType.SchedulingManager;
  sortTypeValue = SortingType.Ascending;
  sortType = SortingType;
  openTimes: Array<any>;
  modalRef: NgbModalRef;
  sortingType: any[] = [];
  totalSchedulingGridData: AgentSchedulesResponse[] = [];
  weekDays: Array<string> = [];
  schedulingStatus: any[] = [];
  schedulingCodes: SchedulingCode[] = [];
  managerCharts: AgentChartResponse[] = [];

  getAgentScheduleSubscription: ISubscription;
  getAgentSchedulesSubscription: ISubscription;
  getSchedulingCodesSubscription: ISubscription;
  subscriptions: ISubscription[] = [];

  @Input() currentDate: string;
  @Input() currentLanguage: string;
  @Input() agentSchedulingGroupId: number;
  @Input() startDate: NgbDate;
  @Input() searchKeyword: string;

  

  constructor(
    private agentSchedulesService: AgentSchedulesService,
    private spinnerService: NgxSpinnerService,
    private schedulingCodeService: SchedulingCodeService,
    private modalService: NgbModal,
  ) { }

  ngOnInit(): void {
    this.openTimes = this.getOpenTimes();
    this.weekDays = Object.keys(WeekDay).filter(key => isNaN(WeekDay[key]));
    this.schedulingStatus = Object.keys(SchedulingStatus).filter(key => isNaN(SchedulingStatus[key]));
    this.sortingType = Object.keys(SortingType).filter(key => isNaN(SortingType[key]));
    this.loadSchedulingCodes();
    this.iconCount = (this.schedulingCodes.length <= 30) ? this.schedulingCodes.length : this.maxIconCount;
    this.endIcon = this.iconCount;
    this.loadAgentScheduleManger();
  }

  ngOnDestroy() {
    this.subscriptions.forEach(subscription => {
      if (subscription) {
        subscription.unsubscribe();
      }
    });
  }

  ngOnChanges() {
    if (this.agentSchedulingGroupId) {
      this.loadAgentScheduleManger();
    } else {
      this.totalSchedulingGridData = [];
    }

  }

  previous() {
    if (this.startIcon > 0) {
      this.startIcon = this.startIcon - 1;
      this.endIcon = this.endIcon - 1;
    }
  }

  onIconClick(event) {
    this.selectedIconId = event.target.id;
  }

  sort(columnName: string, sortBy: string) {
    this.sortBy = sortBy === 'asc' ? 'desc' : 'asc';
    this.orderBy = columnName;

    // this.loadAgentSchedules();
  }

  isMainMinute(data: any) {
    return data.split(':')[1] === '00 am' || data.split(':')[1] === '00 pm';
  }

  isRowSelected(index: number) {
    return this.selectedRow === index;
  }

  setAgent(index: number) {
    this.selectedRow = index;
    // const agentScheduleId = this.totalSchedulingGridData[index].id;
    // this.loadAgentInfo();
    // this.loadAgentSchedule(agentScheduleId);
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

  onMouseDown(event, cell, row) {
    let object;
    this.isMouseDown = true;
    const time = event.currentTarget.attributes.time.nodeValue;
    const meridiem = event.currentTarget.attributes.meridiem.nodeValue;
    const fromTime = time + ' ' + meridiem;
    const scheduleId = event.currentTarget.attributes.scheduleId.nodeValue;
    const chart = this.managerCharts.find(x => x.id === scheduleId);
    for (const item of chart.agentScheduleManagerCharts) {
      object = item?.charts.find(x => this.convertToDateFormat(x.startTime) <= this.convertToDateFormat(fromTime) &&
        this.convertToDateFormat(x.endTime) > this.convertToDateFormat(fromTime));
      if (object) {
        break;
      }
    }
    if (object) {
      const code = this.schedulingCodes.find(x => x.id === object.schedulingCodeId);
      this.icon = code.icon.value ?? undefined;
      if (this.isMouseDown && this.icon) {
        setManagerRowCellIndex(cell, row);
      }
    }
    if (this.isMouseDown && !this.icon) {
      this.isDelete = true;
      setManagerRowCellIndex(cell, row);
    }
  }

  onMouseOver(event) {
    this.removeHighlightedCells();
    if (this.isMouseDown && this.icon) {
      highlightManagerSelectedCells(this.tableClassName, event.currentTarget.id);
    } else if (this.isMouseDown && this.isDelete) {
      highlightManagerSelectedCells(this.tableClassName, event.currentTarget.id);
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

  onSortTypeChange(value: number) {
    this.sortTypeValue = value;
    this.openTimes.reverse();
  }

  changeTimeInterval(interval: number) {
    this.timeIntervals = +interval;
    this.openTimes = this.getOpenTimes();
  }

  dragged(event: CdkDragDrop<any>) {
    this.icon = event.item.element.nativeElement.id;
  }

  getAgentIconDescription(scheduleId: string, openTime: string) {
    const chart = this.managerCharts.find(x => x.id === scheduleId);

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

  getIconFromSelectedAgent(scheduleId: string, openTime: string) {
    const chart = this.managerCharts.find(x => x.id === scheduleId);

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

  openCopySchedule(index: number) {
    const agentScheduleId = this.totalSchedulingGridData[index]?.id;
    const employeeId = this.totalSchedulingGridData[index]?.employeeId;
    this.getModalPopup(CopyScheduleComponent, 'lg');
    this.modalRef.componentInstance.agentSchedulingGroupId = this.agentSchedulingGroupId;
    this.modalRef.componentInstance.agentScheduleId = agentScheduleId;
    this.modalRef.componentInstance.employeeId = employeeId;
    this.modalRef.componentInstance.agentScheduleType = this.tabIndex;

    this.modalRef.result.then((result) => {
      if (result.needRefresh) {
        this.getModalPopup(MessagePopUpComponent, 'sm', 'The record has been copied!');
        this.modalRef.result.then(() => {
          this.loadAgentScheduleManger();
        });
      } else {
        this.getModalPopup(MessagePopUpComponent, 'sm', 'No changes has been made!');
      }
    });

  }

  private getQueryParams(fields?: string) {
    const agentSchedulesQueryParams = new AgentSchedulesQueryParams();
    agentSchedulesQueryParams.agentSchedulingGroupId = this.agentSchedulingGroupId;
    agentSchedulesQueryParams.fromDate = this.getDateInStringFormat(this.startDate);
    agentSchedulesQueryParams.pageNumber = this.currentPage;
    agentSchedulesQueryParams.pageSize = this.pageSize;
    agentSchedulesQueryParams.searchKeyword = this.searchKeyword ?? '';
    agentSchedulesQueryParams.orderBy = `${this.orderBy} ${this.sortBy}`;
    agentSchedulesQueryParams.fields = fields ?? undefined;
    agentSchedulesQueryParams.employeeIds = undefined;

    return agentSchedulesQueryParams;
  }

  private loadAgentScheduleManger() {
    const queryParams = this.getQueryParams('id,employeeId,employeeName');
    queryParams.skipPageSize = true;
    this.iconDescription = undefined;
    this.startTimeFilter = undefined;
    this.endTimeFilter = undefined;
    this.iconCode = undefined;
    this.spinnerService.show(this.spinner, SpinnerOptions);

    this.getAgentSchedulesSubscription = this.agentSchedulesService.getAgentSchedules(queryParams)
      .pipe(mergeMap(data => {
        this.totalSchedulingGridData = data.body;
        this.totalSchedulingRecord = this.totalSchedulingGridData.length;
        this.managerCharts = [];
        const ids = Array<string>();
        this.totalSchedulingGridData.forEach(element => {
          ids.push(element.id);
        });
        return this.getAgentChatrs(ids);
      }))
      .subscribe((response) => {
        this.managerCharts.push(response);
        this.spinnerService.hide(this.spinner);
      }, (error) => {
        this.spinnerService.hide(this.spinner);
        console.log(error);
      }, () => {
        const firstAgent = this.managerCharts[0]?.agentScheduleManagerCharts[0]?.charts[0];
        if (firstAgent) {
          const schedulingCode = this.schedulingCodes.find(x => x.id === firstAgent.schedulingCodeId);
          this.iconDescription = schedulingCode?.description;
          this.startTimeFilter = firstAgent?.startTime;
          this.endTimeFilter = firstAgent?.endTime;
          this.iconCode = schedulingCode?.icon.value;
        }
      });

    this.subscriptions.push(this.getAgentSchedulesSubscription);
  }

  private removeHighlightedCells() {
    const table = $('#' + this.tableClassName);
    table.find('.' + this.selectedCellClassName).removeClass(this.selectedCellClassName);
  }

  private getModalPopup(component: any, size: string, contentMessage?: string) {
    const options: NgbModalOptions = { backdrop: 'static', centered: true, size };
    this.modalRef = this.modalService.open(component, options);
    this.modalRef.componentInstance.headingMessage = 'Success';
    this.modalRef.componentInstance.contentMessage = contentMessage;
  }

  private saveGridItems() {
    const table = $('#' + this.tableClassName);
    table.find('.' + this.selectedCellClassName).each((index, elem) => {
      let weekDays;
      let weekData;
      let week;
      let scheduleId;
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

      const to = hours + ':' + minute + ' ' + meridiem;
      const code = this.schedulingCodes.find(x => x.icon.value === this.icon);
      const iconModel = new ScheduleChart(fromTime, to, code?.id);

      const date = new Date(this.currentDate);

      scheduleId = elem?.attributes?.scheduleId?.value;
      const chart = this.managerCharts.find(x => x.id === scheduleId);
      weekDays = chart?.agentScheduleManagerCharts;
      weekData = weekDays.length > 0 ? weekDays[0] : undefined;

      if (this.icon && !this.isDelete) {

        if (weekData) {
          this.insertIconToGrid(weekData, iconModel);
        } else if (this.tabIndex === AgentScheduleType.Scheduling) {
          const weekDay = new AgentScheduleChart();
          weekDay.day = +week;
          const calendarTime = new ScheduleChart(fromTime, to, code.id);
          weekDay.charts.push(calendarTime);
          weekDays.push(weekDay);
        } else if (this.tabIndex === AgentScheduleType.SchedulingManager) {
          const weekDay = new AgentScheduleManagerChart();
          weekDay.date = date;
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
  }

  private formatTimeValuesInSchedulingGrid() {
    for (const item of this.managerCharts) {
      const weekDays = item.agentScheduleManagerCharts;
      weekDays.forEach((element) => {
        element.charts = this.adjustSchedulingCalendarTimesRange(element.charts);
      });
    }
  }

  private sortSelectedGridCalendarTimes() {
    for (const item of this.managerCharts) {
      const weekDays = item.agentScheduleManagerCharts;
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

  private loadSchedulingCodes() {
    const queryParams = new SchedulingCodeQueryParams();
    queryParams.skipPageSize = true;
    queryParams.fields = 'id, description, icon';
    this.spinnerService.show(this.scheduleSpinner, SpinnerOptions);

    this.getSchedulingCodesSubscription = this.schedulingCodeService.getSchedulingCodes(queryParams)
      .subscribe((response) => {
        if (response.body) {
          this.schedulingCodes = response.body;
          this.iconCount = (this.schedulingCodes.length <= 30) ? this.schedulingCodes.length : this.maxIconCount;
          this.endIcon = this.iconCount;
        }
        this.spinnerService.hide(this.scheduleSpinner);
      }, (error) => {
        this.spinnerService.hide(this.scheduleSpinner);
        console.log(error);
      });

    this.subscriptions.push(this.getSchedulingCodesSubscription);
  }

  private getAgentChatrs(ids: string[]) {
    return from(ids).pipe(
      mergeMap(id => this.agentSchedulesService.getCharts(id.toString())
      ));
  }


  private getDateInStringFormat(startDate: any): string {
    if (!startDate) {
      return undefined;
    }

    const date = new Date(startDate.year, startDate.month - 1, startDate.day, 0, 0, 0, 0);
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

}
