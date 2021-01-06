import { CdkDragDrop } from '@angular/cdk/drag-drop';
import { WeekDay } from '@angular/common';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { NgbCalendar, NgbDate, NgbDateParserFormatter, NgbDateStruct, NgbModal, NgbModalOptions, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { MessagePopUpComponent } from 'src/app/shared/popups/message-pop-up/message-pop-up.component';
import { Constants } from 'src/app/shared/util/constants.util';
import { SchedulingStatus } from '../../../enums/scheduling-status.enum';
import { trigger, state, style, transition, animate } from '@angular/animations';

import { LanguageTranslationService } from 'src/app/shared/services/language-translation.service';
import { TranslationDetails } from 'src/app/shared/models/translation-details.model';
import { SubscriptionLike as ISubscription } from 'rxjs';
import { GenericStateManagerService } from 'src/app/shared/services/generic-state-manager.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { SpinnerOptions } from 'src/app/shared/util/spinner-options.util';
import { AgentSchedulesService } from '../../../services/agent-schedules.service';
import { AgentSchedulesQueryParams } from '../../../models/agent-schedules-query-params.model';
import { SchedulingCode } from '../../../../system-admin/models/scheduling-code.model';
import { SchedulingCodeService } from 'src/app/shared/services/scheduling-code.service';
import { SchedulingCodeQueryParams } from '../../../../system-admin/models/scheduling-code-query-params.model';
import { UpdateAgentSchedule } from '../../../models/update-agent-schedule.model';
import { AuthService } from 'src/app/core/services/auth.service';
import { AgentSchedulesResponse } from '../../../models/agent-schedules-response.model';
import { AgentScheduleGridResponse } from '../../../models/agent-schedule-grid-response.model';
import { UpdateAgentschedulechart } from '../../../models/update-agent-schedule-chart.model';
import { HeaderPagination } from 'src/app/shared/models/header-pagination.model';
import { ScheduleChart } from '../../../models/schedule-chart.model';
import { AgentScheduleChart } from '../../../models/agent-schedule-chart.model';
import { AgentScheduleType } from '../../../enums/agent-schedule-type.enum';
import { ExcelService } from 'src/app/shared/services/excel.service';

import * as $ from 'jquery';
import { KeyValue } from 'src/app/shared/models/key-value.model';
import { ImportScheduleComponent } from '../import-schedule/import-schedule.component';
import { ExcelData } from '../../../models/excel-data.model';
import { Language } from 'src/app/shared/models/language-value.model';
import { TranslateService } from '@ngx-translate/core';
import { ErrorWarningPopUpComponent } from 'src/app/shared/popups/error-warning-pop-up/error-warning-pop-up.component';
import { ContentType } from 'src/app/shared/enums/content-type.enum';
import { ExcelExportData } from '../../../constants/excel-export-data';
import { CopyScheduleComponent } from '../copy-schedule/copy-schedule.component';

declare function setRowCellIndex(cell: string);
declare function highlightSelectedCells(table: string, cell: string);
declare function removeHighlightedCells(table: string, className: string);
declare function highlightCell(cell: string, className: string);

@Component({
  selector: 'app-scheduling-grid',
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
  templateUrl: './scheduling-grid.component.html',
  styleUrls: ['./scheduling-grid.component.scss']
})
export class SchedulingGridComponent implements OnInit, OnDestroy {
  currentLanguage: Language;

  timeIntervals = 15;
  currentPage = 1;
  pageSize = 10;
  characterSplice = 25;
  maxIconCount = 30;
  iconCount: number;
  startIcon = 0;
  endIcon: number;
  totalSchedulingRecord: number;
  agentSchedulingGroupId?: number;
  agentScheduleId: number;

  selectedIconId: string;
  icon: string;
  searchKeyword: string;
  tableClassName = 'schedulingGridTable';
  selectedCellClassName = 'cell-selected';
  spinner = 'Scheduling';
  scheduleSpinner = 'SchedulingSpinner';
  orderBy = 'createdDate';
  sortBy = 'desc';
  exportFileName = 'Attendance_scheduling';
  isMouseDown: boolean;
  isDelete: boolean;
  startDate: any;

  hoveredDate: NgbDate | null = null;
  toDate: NgbDate | null = null;
  fromDate = this.calendar.getToday();
  today = this.calendar.getToday();

  modalRef: NgbModalRef;
  model: NgbDateStruct;
  weekDay = WeekDay;
  scheduleStatus = SchedulingStatus;
  selectedGrid: AgentScheduleGridResponse;
  schedulingGridData: AgentScheduleGridResponse;
  importedData: ExcelData[] = [];
  paginationSize = Constants.paginationSize;

  openTimes: Array<any>;
  translationValues: TranslationDetails[] = [];
  schedulingIntervals: KeyValue[] = [];
  totalSchedulingGridData: AgentSchedulesResponse[] = [];
  schedulingStatus: any[] = [];
  weekDays: Array<string> = [];
  schedulingCodes: SchedulingCode[] = [];

  updateAgentScheduleChartSubscription: ISubscription;
  getAgentScheduleSubscription: ISubscription;
  updateAgentScheduleSubscription: ISubscription;
  getSchedulingCodesSubscription: ISubscription;
  getAgentSchedulesSubscription: ISubscription;
  getTranslationSubscription: ISubscription;
  importAgentScheduleChartSubscription: ISubscription;
  subscriptions: ISubscription[] = [];

  constructor(
    public translate: TranslateService,
    private calendar: NgbCalendar,
    private modalService: NgbModal,
    public ngbDateParserFormatter: NgbDateParserFormatter,
    private genericStateManagerService: GenericStateManagerService,
    private spinnerService: NgxSpinnerService,
    private agentSchedulesService: AgentSchedulesService,
    private schedulingCodeService: SchedulingCodeService,
    private authService: AuthService,
    private excelService: ExcelService
  ) { }

  ngOnInit(): void {
    this.openTimes = this.getOpenTimes();
    this.weekDays = Object.keys(WeekDay).filter(key => isNaN(WeekDay[key]));
    this.schedulingIntervals = Constants.schedulingIntervals;
    this.schedulingStatus = Object.keys(SchedulingStatus).filter(key => isNaN(SchedulingStatus[key]));
    this.loadTranslations();
    this.subscribeToTranslations();
    this.loadAgentSchedules();
  }

  ngOnDestroy() {
    this.subscriptions.forEach(subscription => {
      if (subscription) {
        subscription.unsubscribe();
      }
    });
  }

  getGridMaxWidth() {
    return window.innerWidth > 1350 ? (window.innerWidth - 350) + 'px' : '1250px';
  }

  onClickDateIcon(index: number) {
    const from = this.totalSchedulingGridData[index]?.dateFrom;
    const to = this.totalSchedulingGridData[index]?.dateTo;
    this.fromDate = this.convertToNgbDate(from) ?? this.today;
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
      this.loadSchedulingCodes();
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
    this.isMouseDown = true;
    const time = event.currentTarget.attributes.time.nodeValue;
    const meridiem = event.currentTarget.attributes.meridiem.nodeValue;
    const week = event.currentTarget.attributes.week.nodeValue;
    const days = this.selectedGrid.agentScheduleCharts.find(x => x.day === +week);
    const fromTime = time + ' ' + meridiem;
    const object = days?.charts.find(x => this.convertToDateFormat(x.startTime) <= this.convertToDateFormat(fromTime) &&
      this.convertToDateFormat(x.endTime) > this.convertToDateFormat(fromTime));
    if (object) {
      const code = this.schedulingCodes.find(x => x.id === object.schedulingCodeId);
      this.icon = code.icon.value ?? undefined;
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
    this.pageSize = pageSize;
    this.loadAgentSchedules();
  }

  changePage(page: number) {
    this.currentPage = page;
    this.loadAgentSchedules();
  }

  setSchedulingStatus(status: string, index: number) {
    this.totalSchedulingGridData[index].status = Number(status);
    this.updateAgentSchedule(index);
  }

  save(gridChart: AgentScheduleGridResponse) {
    this.updateAgentScheduleChart(gridChart.id);
  }

  onSchedulingGroupChange(schedulingGroupId: number) {
    this.agentSchedulingGroupId = schedulingGroupId;
    this.loadAgentSchedules();
  }

  search() {
    this.loadAgentSchedules();
  }

  sort(columnName: string, sortBy: string) {
    this.sortBy = sortBy === 'asc' ? 'desc' : 'asc';
    this.orderBy = columnName;

    this.loadAgentSchedules();
  }

  onSelectStartDate(date: NgbDate) {
    this.startDate = date;
    this.loadAgentSchedules();
  }

  setStartDateAsToday() {
    this.startDate = this.today;
    this.loadAgentSchedules();
  }

  clearStartDate() {
    this.startDate = undefined;
    this.loadAgentSchedules();
  }

  openImportSchedule(agentScheduleId: string) {
    this.getModalPopup(ImportScheduleComponent, 'lg');
    this.modalRef.componentInstance.agentScheduleId = agentScheduleId;
    this.modalRef.componentInstance.translationValues = this.translationValues;
    this.modalRef.componentInstance.agentScheudleType = AgentScheduleType.Scheduling;

    this.modalRef.result.then((result) => {
      const message = result.partialImport ? 'The record has been paritially imported!' : 'The record has been imported!';
      this.getModalPopup(MessagePopUpComponent, 'sm', message);
      this.modalRef.result.then(() => {
        this.loadAgentSchedule(agentScheduleId);
      });
    });
  }

  openCopySchedule(agentScheduleId: string) {
    this.getModalPopup(CopyScheduleComponent, 'lg');
    this.modalRef.componentInstance.agentSchedulingGroupId = this.schedulingGridData?.agentSchedulingGroupId;
    this.modalRef.componentInstance.translationValues = this.translationValues;
    this.modalRef.componentInstance.agentScheduleId = agentScheduleId;
    this.modalRef.componentInstance.agentScheudleType = AgentScheduleType.Scheduling;

    this.modalRef.result.then((result) => {
      if (result.needRefresh) {
        this.getModalPopup(MessagePopUpComponent, 'sm', 'The record has been copied!');
        this.modalRef.result.then(() => {
          this.loadAgentSchedule(agentScheduleId);
          this.loadAgentSchedules();
        });
      } else {
        this.getModalPopup(MessagePopUpComponent, 'sm', 'No changes has been made!');
      }
    });

  }

  exportToExcel(index: number) {
    const today = new Date().getFullYear() + '/' + (new Date().getMonth() + 1) + '/' + new Date().getDay();
    this.excelService.exportAsExcelFile(ExcelExportData, this.exportFileName + today);
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


  private convertToNgbDate(date: Date) {
    if (date) {
      date = new Date(date);
      const newDate: NgbDate = new NgbDate(date.getUTCFullYear(), date.getUTCMonth() + 1, date.getUTCDate() + 1);
      return newDate ?? undefined;
    }
  }

  private matchSchedulingGridDataChanges() {
    if (JSON.stringify(this.schedulingGridData) !== JSON.stringify(this.selectedGrid)) {
      return true;
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

  private saveGridItems() {
    const table = $('#' + this.tableClassName);
    table.find('.' + this.selectedCellClassName).each((index, elem) => {
      this.spinnerService.show(this.scheduleSpinner, SpinnerOptions);
      const week = elem.attributes.week.value;
      let meridiem = elem.attributes.meridiem.value;
      const from = elem.attributes.time.value + ' ' + meridiem;
      let hours = from.split(':')[0];
      const minuteValue = Number(from.split(':')[1].split(' ')[0]) + this.timeIntervals;
      let minute = minuteValue === 5 ? '05' : minuteValue;

      if (minute === 60) {
        meridiem = (hours === '11' && meridiem === 'am') ? 'pm' : meridiem;
        hours = hours !== '11' ? ('0' + (Number(hours) + 1)).slice(-2) : '00';
        minute = '00';
      }

      const to = hours + ':' + minute + ' ' + meridiem;
      const code = this.schedulingCodes.find(x => x.icon.value === this.icon);
      const iconModel = new ScheduleChart(from, to, code?.id);

      const weekDays = this.selectedGrid?.agentScheduleCharts;
      const weekData = weekDays.find(x => x.day === +week);

      if (this.icon && !this.isDelete) {

        if (weekData) {
          this.insertIconToGrid(weekData, iconModel);
        } else {
          const weekDay = new AgentScheduleChart();
          weekDay.day = +week;
          const calendarTime = new ScheduleChart(from, to, code.id);
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
    this.selectedGrid.agentScheduleCharts.forEach((element) => {
      element.charts = this.adjustSchedulingCalendarTimesRange(element.charts);
    });
  }

  private sortSelectedGridCalendarTimes() {
    this.selectedGrid.agentScheduleCharts.forEach((element) => {
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

  private subscribeToTranslations(){
    this.getTranslationSubscription = this.genericStateManagerService.userLanguageChanged.subscribe(
      (language) => {
        if (language) {
          this.loadTranslations();
        }
      }
    );
    this.subscriptions.push(this.getTranslationSubscription);
  }

  private loadTranslations(){
    const browserLang = this.genericStateManagerService.getLanguage();
    this.currentLanguage = browserLang;
    this.translate.use(browserLang ? browserLang : 'en');
  }

  private getQueryParams() {
    const agentSchedulesQueryParams = new AgentSchedulesQueryParams();
    agentSchedulesQueryParams.agentSchedulingGroupId = this.agentSchedulingGroupId;
    agentSchedulesQueryParams.fromDate = this.getDateInStringFormat(this.startDate);
    agentSchedulesQueryParams.pageNumber = this.currentPage;
    agentSchedulesQueryParams.pageSize = this.pageSize;
    agentSchedulesQueryParams.searchKeyword = this.searchKeyword ?? '';
    agentSchedulesQueryParams.orderBy = `${this.orderBy} ${this.sortBy}`;
    agentSchedulesQueryParams.fields = '';
    agentSchedulesQueryParams.employeeIds = undefined;

    return agentSchedulesQueryParams;
  }

  private loadAgentSchedules() {
    const queryParams = this.getQueryParams();
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

  private loadAgentSchedule(agentScheduleId: string) {
    this.spinnerService.show(this.scheduleSpinner, SpinnerOptions);

    this.getAgentScheduleSubscription = this.agentSchedulesService.getAgentSchedule(agentScheduleId)
      .subscribe((response) => {
        if (response) {
          this.selectedGrid = response;
          this.schedulingGridData = JSON.parse(JSON.stringify(this.selectedGrid));
        }
        this.spinnerService.hide(this.scheduleSpinner);
      }, (error) => {
        this.spinnerService.hide(this.scheduleSpinner);
        console.log(error);
      });

    this.subscriptions.push(this.getAgentScheduleSubscription);
  }

  private updateAgentScheduleChart(agentScheduleId: string) {
    if (this.matchSchedulingGridDataChanges()) {
      this.spinnerService.show(this.spinner, SpinnerOptions);
      const chartModel = new UpdateAgentschedulechart();
      chartModel.agentScheduleType = AgentScheduleType.Scheduling;
      chartModel.agentScheduleCharts = this.selectedGrid.agentScheduleCharts;
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

  private getModalPopup(component: any, size: string, contentMessage?: string) {
    const options: NgbModalOptions = { backdrop: 'static', centered: true, size };
    this.modalRef = this.modalService.open(component, options);
    this.modalRef.componentInstance.translationValues = this.translationValues;
    this.modalRef.componentInstance.headingMessage = 'Success';
    this.modalRef.componentInstance.contentMessage = contentMessage;
  }

  private getWarningPopup(contentMessage: string) {
    const options: NgbModalOptions = { backdrop: 'static', centered: true, size: 'sm' };
    this.modalRef = this.modalService.open(ErrorWarningPopUpComponent, options);
    this.modalRef.componentInstance.translationValues = this.translationValues;
    this.modalRef.componentInstance.headingMessage = 'Error';
    this.modalRef.componentInstance.contentMessage = contentMessage;
    this.modalRef.componentInstance.messageType = ContentType.String;
  }

  private getDateInStringFormat(startDate: any): string {
    if (!startDate) {
      return undefined;
    }

    const date = new Date(startDate.year, startDate.month - 1, startDate.day, 0, 0, 0, 0);
    return date.toDateString();
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
