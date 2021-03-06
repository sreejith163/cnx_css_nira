import { Component, OnDestroy, OnInit } from '@angular/core';
import { NgbCalendar, NgbDate, NgbDateParserFormatter, NgbModal, NgbModalOptions, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { MessagePopUpComponent } from 'src/app/shared/popups/message-pop-up/message-pop-up.component';

import { Subject, SubscriptionLike as ISubscription } from 'rxjs';
import { SchedulingCode } from '../../../system-admin/models/scheduling-code.model';
import { SchedulingCodeService } from 'src/app/shared/services/scheduling-code.service';
import { PermissionsService } from '../../../system-admin/services/permissions.service';
import { SchedulingCodeQueryParams } from '../../../system-admin/models/scheduling-code-query-params.model';
import { AuthService } from 'src/app/core/services/auth.service';
import { AgentScheduleType } from '../../enums/agent-schedule-type.enum';
import { ExcelService } from 'src/app/shared/services/excel.service';

import { ImportScheduleComponent } from '../shared/import-schedule/import-schedule.component';
import { ExcelData } from '../../models/excel-data.model';
import { SchedulingExcelExportData } from '../../constants/scheduling-excel-export-data';
import { LanguagePreference } from 'src/app/shared/models/language-preference.model';
import { ActivatedRoute } from '@angular/router';
import { LanguagePreferenceService } from 'src/app/shared/services/language-preference.service';
import { TranslateService } from '@ngx-translate/core';
import { AgentScheduleGridResponse } from '../../models/agent-schedule-grid-response.model';
import { SchedulingStatus } from '../../enums/scheduling-status.enum';
import { Constants } from 'src/app/shared/util/constants.util';
import { DatePipe, WeekDay } from '@angular/common';
import { AgentSchedulesResponse } from '../../models/agent-schedules-response.model';
import { NgxSpinnerService } from 'ngx-spinner';
import { CdkDragDrop } from '@angular/cdk/drag-drop';
import { ActivityType } from 'src/app/shared/enums/activity-type.enum';
import { CopyScheduleComponent } from '../shared/copy-schedule/copy-schedule.component';
import { SpinnerOptions } from 'src/app/shared/util/spinner-options.util';
import { ScheduleChart } from '../../models/schedule-chart.model';
import { AgentScheduleChart } from '../../models/agent-schedule-chart.model';
import { AgentSchedulesQueryParams } from '../../models/agent-schedules-query-params.model';
import { AgentSchedulesService } from '../../services/agent-schedules.service';
import { HeaderPagination } from 'src/app/shared/models/header-pagination.model';
import { UpdateAgentSchedule } from '../../models/update-agent-schedule.model';
import { UpdateAgentschedulechart } from '../../models/update-agent-schedule-chart.model';
import { ActivityOrigin } from '../../enums/activity-origin.enum';
import { animate, state, style, transition, trigger } from '@angular/animations';
import { ActivityLogsScheduleComponent } from '../shared/activity-logs-schedule/activity-logs-schedule.component';
import * as $ from 'jquery';
import { DateRangePopUpComponent } from 'src/app/modules/home/modules/scheduling-menu/components/shared/date-range-pop-up/date-range-pop-up.component';
import { ComponentOperation } from 'src/app/shared/enums/component-operation.enum';
import { ConfirmationPopUpComponent } from 'src/app/shared/popups/confirmation-pop-up/confirmation-pop-up.component';
import { AgentScheduleRange } from '../../models/agent-schedule-range.model';
import { ScheduleDateRangeBase } from '../../models/schedule-date-range-base.model';
import { ErrorWarningPopUpComponent } from 'src/app/shared/popups/error-warning-pop-up/error-warning-pop-up.component';
import { ContentType } from 'src/app/shared/enums/content-type.enum';
import { DateRangeQueryParms } from '../../models/date-range-query-params.model';
import * as moment from 'moment';
import { AgentSchedulingGridExport } from '../../models/agent-scheduling-grid-export.model';
import { IDropdownSettings } from 'ng-multiselect-dropdown';
import { AgentSchedulingGroupService } from 'src/app/shared/services/agent-scheduling-group.service';
import { AgentSchedulingGroupBase } from '../../../setup-menu/models/agent-scheduling-group-base.model';
import { AgentSchedulingGroupQueryParams } from '../../../setup-menu/models/agent-scheduling-group-query-params.model';
import { debounceTime, distinctUntilChanged, switchMap } from 'rxjs/operators';
import { BatchReleaseModel } from '../../models/batch-release.model';
import { GenericPopUpComponent } from 'src/app/shared/popups/generic-pop-up/generic-pop-up.component';
import { ToastrService } from 'ngx-toastr';
declare function setRowCellIndex(cell: string);
declare function highlightSelectedCells(table: string, cell: string);
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
  styleUrls: ['./scheduling-grid.component.scss'],
  providers: [DatePipe]
})
export class SchedulingGridComponent implements OnInit, OnDestroy {

  agentSchedulingGroupId: number;
  timeIntervals = 15;
  currentPage = 1;
  pageSize = 3;
  characterSplice = 25;
  maxIconCount = 30;
  iconCount: number;
  startIcon = 0;
  endIcon: number;
  totalSchedulingRecord: number;
  rangeIndex: number;
  status = SchedulingStatus;

  selectedIconId: string;
  icon: string;
  spinner = 'scheduling-tab';
  scheduleSpinner = 'scheduling-spinner';
  selectedCellClassName = 'cell-selected';
  tableClassName = 'schedulingGridTable';
  orderBy = 'createdDate';
  sortBy = 'desc';
  searchText: string;
  exportFileName = 'Attendance_scheduling';
  startDate: string;
  currentLanguage: string;

  isMouseDown: boolean;
  isDelete: boolean;
  refreshSchedulingTab: boolean;
  hasNewDateRangeSelected: boolean;
  LoggedUser;
  exportData: AgentSchedulingGridExport[] = [];
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
  schedulingCodes: SchedulingCode[] = [];
  importedData: ExcelData[] = [];

  deleteScheduleDateRangeSubscription: ISubscription;
  updateAgentScheduleChartSubscription: ISubscription;
  getAgentSchedulesSubscription: ISubscription;
  getAgentScheduleSubscription: ISubscription;
  updateAgentScheduleSubscription: ISubscription;
  getSchedulingCodesSubscription: ISubscription;
  getTranslationSubscription: ISubscription;
  subscriptions: ISubscription[] = [];
  agentScheduleGridExportSubsciption: ISubscription;

  timeStampUpdate: number;

  dropdownList: Array<any>;
  selectedItems = [];
  dropdownSettings = {};
  dropdown_setting: IDropdownSettings;


  pageNumber = 1;
  agentSchedulingGroupItemsBufferSize = 100;
  numberOfItemsFromEndBeforeFetchingMore = 10;

  totalItems = 0;
  totalPages: number;
  searchKeyWord = '';
  dropdownSearchKeyWord = '';
  loading = false;

  itemList = [];

  date_range_model = [];
  date_ranges = [];
  date_range_setting = {};
  settings = {};
  agentSchedId = [];
  indices: any;
  readonly bufferSize: number = 10;

  agentSchedulingGroupItemsBuffer: AgentSchedulingGroupBase[] = [];
  typeAheadInput$ = new Subject<string>();

  typeAheadValueSubscription: ISubscription;
  getAgentSchedulingGroupSubscription: ISubscription;


  asg: any[] = [];
  selectedAsg = [];
  dateRange = [];
  selectedRange = []
  constructor(
    private calendar: NgbCalendar,
    private modalService: NgbModal,
    private spinnerService: NgxSpinnerService,
    public ngbDateParserFormatter: NgbDateParserFormatter,
    private schedulingCodeService: SchedulingCodeService,
    private excelService: ExcelService,
    private authService: AuthService,
    private permissionsService: PermissionsService,
    private route: ActivatedRoute,
    private languagePreferenceService: LanguagePreferenceService,
    private agentSchedulesService: AgentSchedulesService,
    public translate: TranslateService,
    private datepipe: DatePipe,
    private agentSchedulingGroupService: AgentSchedulingGroupService,
    private toast: ToastrService

  ) {
    this.LoggedUser = this.authService.getLoggedUserInfo();
  }

  ngOnInit(): void {
    this.schedulingStatus = Object.keys(SchedulingStatus).filter(key => isNaN(SchedulingStatus[key]));
    this.openTimes = this.getOpenTimes();
    this.weekDays = Object.keys(WeekDay).filter(key => isNaN(WeekDay[key]));
    this.loadSchedulingCodes();
    this.preLoadTranslations();
    this.loadTranslations();
    this.subscribeToAgentSchedulingGroups();
    this.subscribeToTranslations();
    this.batchReleaseLoad();
  }


  ngOnDestroy() {
    this.subscriptions.forEach(subscription => {
      if (subscription) {
        subscription.unsubscribe();
      }
    });
  }

  hasSchedulingIconsDisabled(el: AgentSchedulesResponse) {
    if (el?.ranges[el?.rangeIndex]?.status === SchedulingStatus.Released) {
      return true;
    } else {
      return false;
    }
  }

  hasStatusDisabled(el: AgentSchedulesResponse) {
    if (el?.ranges.length === 0) {
      return true;
    }
    if (el?.ranges.length > 0 && el.ranges[el.rangeIndex]) {
      if (el.ranges[el.rangeIndex].status === SchedulingStatus['Pending Schedule'] &&
        el?.ranges[el?.rangeIndex].scheduleCharts.length === 0) {
        return true;
      } else if (el.ranges[el.rangeIndex].status === SchedulingStatus['Pending Schedule'] &&
        el?.ranges[el?.rangeIndex].scheduleCharts.length > 0) {
        return false;
      } else if (el.ranges[el.rangeIndex].status === SchedulingStatus.Released) {
        return true;
      }
    }

    return true;
  }

  getRangeIndex(el: AgentSchedulesResponse) {
    return el?.rangeIndex;
  }

  addDateRange(el: AgentSchedulesResponse) {
    this.hasNewDateRangeSelected = true;
    this.schedulingGridData = undefined;
    this.getCalendarPopup(DateRangePopUpComponent, 'sm', el);
    this.modalRef.componentInstance.operation = ComponentOperation.Add;
    this.modalRef.componentInstance.agentScheduleId = el.id;
    this.modalRef.componentInstance.isEditNewDateRange = false;
    this.modalRef.result.then((result: ScheduleDateRangeBase) => {
      if (result) {
        this.setNewDateRangeInMemory(el, result);
        this.setComponentMessages('Success', 'The record has been added!');
      } else {
        this.getModalPopup(MessagePopUpComponent, 'sm');
        this.setComponentMessages('Success', 'No changes has been made!');
      }
    });
  }

  editDateRange(el: AgentSchedulesResponse) {
    this.getModalPopup(DateRangePopUpComponent, 'sm');
    this.modalRef.componentInstance.dateFrom = this.formatDateMoment(el.ranges[el.rangeIndex].dateFrom);
    this.modalRef.componentInstance.dateTo = this.formatDateMoment(el.ranges[el.rangeIndex].dateTo);
    this.modalRef.componentInstance.agentScheduleId = el.id;
    this.modalRef.componentInstance.operation = ComponentOperation.Edit;
    this.modalRef.componentInstance.isEditNewDateRange = this.hasNewDateRangeSelected;

    this.modalRef.result.then((result: ScheduleDateRangeBase) => {
      if (result?.dateFrom && result?.dateTo) {
        el.ranges[el.rangeIndex].dateFrom = result.dateFrom;
        el.ranges[el.rangeIndex].dateTo = result.dateTo;
        if (this.selectedGrid) {
          this.selectedGrid.dateFrom = result.dateFrom;
          this.selectedGrid.dateTo = result.dateTo;
          this.schedulingGridData.dateFrom = result.dateFrom;
          this.schedulingGridData.dateTo = result.dateTo;
        }
        this.getModalPopup(MessagePopUpComponent, 'sm');
        this.setComponentMessages('Success', 'The record has been updated!');
      } else {
        this.getModalPopup(MessagePopUpComponent, 'sm');
        this.setComponentMessages('Success', 'No changes has been made!');
      }
    });
  }

  deleteDateRange(el: AgentSchedulesResponse) {
    const options: NgbModalOptions = { backdrop: 'static', centered: true, size: 'sm' };
    this.modalRef = this.modalService.open(ConfirmationPopUpComponent, options);
    this.modalRef.componentInstance.headingMessage = 'Are you sure?';
    this.modalRef.componentInstance.contentMessage = 'You won’t be able to revert this!';
    this.modalRef.componentInstance.deleteRecordIndex = el.id;

    this.modalRef.result.then((result) => {
      if (result && result === el.id) {
        if (this.hasNewDateRangeSelected) {
          el.ranges.splice(el.rangeIndex, 1);
          el.rangeIndex = el.rangeIndex - 1;
          this.getModalPopup(MessagePopUpComponent, 'sm');
          this.setComponentMessages('Success', 'The record has been deleted!');
        } else {
          this.spinnerService.show(this.spinner, SpinnerOptions);
          const model = new DateRangeQueryParms();
          model.dateFrom = el.ranges[el.rangeIndex].dateFrom;
          model.dateTo = el.ranges[el.rangeIndex].dateTo;
          this.deleteScheduleDateRangeSubscription = this.agentSchedulesService.deleteAgentScheduleRange(el.id, model)
            .subscribe((response) => {
              this.spinnerService.hide(this.spinner);
              this.getModalPopup(MessagePopUpComponent, 'sm');
              this.setComponentMessages('Success', 'The record has been deleted!');
              this.loadAgentSchedules();
            }, (error) => {
              this.spinnerService.hide(this.spinner);
              this.getModalPopup(ErrorWarningPopUpComponent, 'sm');
              this.setComponentMessages('Error', error.message);
              this.modalRef.componentInstance.messageType = ContentType.String;
              console.log(error);
            });

          this.subscriptions.push(this.deleteScheduleDateRangeSubscription);
        }
      }
    });
  }

  getSelectedRange(el: AgentSchedulesResponse) {
    let selectedRange = 'Please select a date range';
    if (el.ranges.length && el.ranges[el.rangeIndex]) {
      const formattedDateFrom = el.ranges[el.rangeIndex].dateFrom;
      const formattedDateTo = el.ranges[el.rangeIndex].dateTo; if (el.rangeIndex > -1) {
        selectedRange = `${formattedDateFrom} — ${formattedDateTo}`;
      }
    }

    return selectedRange;
  }

  onDateRangeChange(index: any, el: AgentSchedulesResponse) {
    const rangeIndex = +index;
    if (rangeIndex > -1) {
      el.rangeIndex = rangeIndex;
    }
    if (this.selectedGrid) {
      const item = this.totalSchedulingGridData.find(x => x.id === el.id);
      this.setSelectedGrid(item);
    }
  }

  canShowActivityLog() {
    const rolesPermitted = [1, 2, 3, 5];
    const userRoleId = this.permissionsService.userRoleId;
    return rolesPermitted.indexOf(+userRoleId) > -1;
  }

  getGridMaxWidth() {
    return window.innerWidth > 1350 ? (window.innerWidth - 350) + 'px' : '1250px';
  }

  cancel() {
    this.selectedGrid = JSON.parse(JSON.stringify(this.schedulingGridData));
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
      const item = this.totalSchedulingGridData.find(x => x.id === el.id);
      this.setSelectedGrid(item);
      // this.loadAgentSchedule(el.id);
    }
  }

  getIconFromSelectedGrid(week: number, openTime: any) {
    const weekData = this.selectedGrid?.agentScheduleCharts.find(x => x.day === +week);
    if (weekData) {
      const weekTimeData = weekData?.charts?.find(x => this.convertToDateFormat(openTime) >= this.convertToDateFormat(x?.startTime) &&
        this.convertToDateFormat(openTime) < this.convertToDateFormat(x?.endTime));
      if (weekTimeData) {
        const code = this.schedulingCodes.find(x => x.id === weekTimeData.schedulingCodeId);
        return code ? this.unifiedToNative(code?.icon?.value) : '';
      }
    }

    return '';
  }

  previous() {
    if (this.startIcon > 0) {
      this.startIcon = this.startIcon - 30;
      this.endIcon = this.endIcon - 30;
      if (this.startIcon < 0 ){
        this.startIcon = 0;
        this.endIcon = this.iconCount;
      }
    }
  }

  next() {
    if (this.endIcon < this.schedulingCodes.length) {
      this.startIcon = this.startIcon + 30;
      this.endIcon = this.endIcon + 30;
      if (this.endIcon > this.schedulingCodes.length) {
        this.startIcon = this.schedulingCodes.length - this.iconCount
        this.endIcon = this.schedulingCodes.length;
      }
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

  onRightClick(event) {
    if (this.selectedGrid?.status === SchedulingStatus.Released) {
      return;
    }
    this.isMouseDown = true;
    if (this.isMouseDown && !this.icon) {
      this.isDelete = true;
      setRowCellIndex(event.currentTarget.id);
      // call on mouseOver event to simulate the mouseOver event on single icon deleting       
      this.onMouseOver(event);
      // call on mouseUp event to simulate the mouseUp event on single icon deleting        
      this.onMouseUp(event);
    }

    event.preventDefault();

    return false;
  }

  
  disableRightClick(e){
    e.preventDefault();
  }

  onMouseUp(event) {
    if (this.selectedGrid?.status === SchedulingStatus.Released) {
      return;
    }
    this.isMouseDown = false;
    if (this.isDelete) {
      this.saveGridItems();
    } else {
      highlightCell(event.currentTarget.id, this.selectedCellClassName);
      this.saveGridItems();
    }
  }

  onMouseDown(event) {
    if (this.selectedGrid?.status === SchedulingStatus.Released) {
      return;
    }

    if (event.button === 0) {
      let days;
      this.isMouseDown = true;
      const time = event.currentTarget.attributes.time.nodeValue;
      const meridiem = event.currentTarget.attributes.meridiem.nodeValue;
      const week = event.currentTarget.attributes.week.nodeValue;
      days = this.selectedGrid?.agentScheduleCharts.find(x => x?.day === +week);
      const fromTime = time + ' ' + meridiem;
      const object = days?.charts.find(x => this.convertToDateFormat(x?.startTime) <= this.convertToDateFormat(fromTime) &&
        this.convertToDateFormat(x?.endTime) > this.convertToDateFormat(fromTime));
      if (object) {
        const code = this.schedulingCodes.find(x => x.id === object?.schedulingCodeId);
        this.icon = code?.icon?.value ?? undefined;
        if (this.isMouseDown && this.icon) {
          setRowCellIndex(event.currentTarget.id);
        }
      }else{
        return;
      }
    }else if(event.button === 2){
      this.isMouseDown = true;
      if (this.isMouseDown && !this.icon) {
        this.isDelete = true;
        setRowCellIndex(event.currentTarget.id);
      }
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
    this.timeStampUpdate = new Date().getTime();
  }

  changePageSize(pageSize: number) {
    if (this.agentSchedulingGroupId) {
      this.pageSize = pageSize;
      this.loadAgentSchedules();
      if (this.selectedGrid) {
        this.cancel();
      }
    }
  }

  changePage(page: number) {
    if (this.agentSchedulingGroupId) {
      this.currentPage = page;
      this.loadAgentSchedules();
      this.selectedGrid = null;
    }
  }

  setSchedulingStatus(event: any, el: AgentSchedulesResponse) {
    this.getModalPopup(ConfirmationPopUpComponent, 'sm');
    this.modalRef.componentInstance.deleteRecordIndex = el.id;
    this.modalRef.componentInstance.headingMessage = 'Are you sure ?';
    this.modalRef.componentInstance.contentMessage = 'You can no longer revert the action to Pending status';
    this.modalRef.componentInstance.confirmSchedulingStatus = true;
    this.modalRef.result.then((result) => {
      if (result && result === el.id) {
        el.ranges[el?.rangeIndex].status = +event.target.value;
        this.updateAgentSchedule(el);
      } else {
        event.target.selectedIndex = 0;
      }
    });
  }

  getSchedulingStatus(el: AgentSchedulesResponse, status: SchedulingStatus) {
    return +el?.ranges[el?.rangeIndex]?.status;
  }

  sort(columnName: string, sortBy: string) {
    if (this.agentSchedulingGroupId) {
      this.sortBy = sortBy === 'asc' ? 'desc' : 'asc';
      this.orderBy = columnName;

      this.loadAgentSchedules();
      this.selectedGrid = null;
    }
  }

  private changeToUTCDate(date) {
    return new Date(new Date(date).toString().replace(/\sGMT.*$/, " GMT+0000"));
  }

  save(gridChart: AgentSchedulesResponse) {
    if (this.matchSchedulingGridDataChanges()) {
      this.spinnerService.show(this.spinner, SpinnerOptions);
      const chartModel = new UpdateAgentschedulechart();
      let gridData = new AgentScheduleGridResponse();
      gridData = this.getUpdatedScheduleChart();
      if (gridData.agentScheduleCharts.length > 0) {
        gridData.agentScheduleCharts.forEach(ele => {
          this.formatendTime(ele.charts, true);
        });
      }
      chartModel.dateFrom = gridData?.dateFrom;
      chartModel.dateTo = gridData?.dateTo;
      chartModel.status = gridData?.status;
      chartModel.agentScheduleCharts = gridData?.agentScheduleCharts;

      // console.log(gridData.agentScheduleCharts)
      chartModel.activityOrigin = ActivityOrigin.CSS;
      chartModel.modifiedUser = +this.authService.getLoggedUserInfo()?.employeeId;
      chartModel.modifiedBy = this.authService.getLoggedUserInfo()?.displayName;

      this.updateAgentScheduleChartSubscription = this.agentSchedulesService
        .updateAgentScheduleChart(gridChart.id, chartModel)
        .subscribe(() => {
          this.hasNewDateRangeSelected = false;
          this.spinnerService.hide(this.spinner);
          this.getModalPopup(MessagePopUpComponent, 'sm');
          this.setComponentMessages('Success', 'The record has been updated!');
          this.modalRef.result.then(() => {
            if (!this.selectedGrid?.agentScheduleCharts?.find(x => x?.charts?.length > 0)) {
              const rangeData = this.totalSchedulingGridData.find(x => x.id === this.selectedGrid?.id);
              if (rangeData?.rangeIndex > -1) {
                rangeData?.ranges?.splice(rangeData.rangeIndex, 1);
              }
              rangeData.rangeIndex = rangeData?.rangeIndex !== 0 ? rangeData?.rangeIndex - 1 : rangeData?.rangeIndex;
              this.selectedGrid = undefined;
              this.schedulingGridData = undefined;
            } else {
              this.schedulingGridData = JSON.parse(JSON.stringify(this.selectedGrid));
              gridChart.ranges[gridChart?.rangeIndex].scheduleCharts = this.selectedGrid.agentScheduleCharts;
              gridChart.ranges[gridChart?.rangeIndex].modifiedBy = this.authService.getLoggedUserInfo()?.displayName;
              gridChart.ranges[gridChart?.rangeIndex].modifiedDate = new Date();
            }
          });
        }, (error) => {
          this.spinnerService.hide(this.spinner);
          console.log(error);
        });

      this.subscriptions.push(this.updateAgentScheduleChartSubscription);
    } else {
      this.getModalPopup(MessagePopUpComponent, 'sm');
      this.setComponentMessages('Success', 'No changes has been made!');
    }
  }

  openActivityLogs(el: AgentSchedulesResponse) {
    this.getModalPopup(ActivityLogsScheduleComponent, 'xl');
    this.modalRef.componentInstance.activityType = ActivityType.SchedulingGrid;
    this.modalRef.componentInstance.employeeId = el.employeeId;
    this.modalRef.componentInstance.employeeName = this.selectedGrid.lastName + ', ' + this.selectedGrid.firstName;
    this.modalRef.componentInstance.startDate = this.startDate;
    this.modalRef.componentInstance.dateFrom = el?.ranges[el?.rangeIndex]?.dateFrom;
    this.modalRef.componentInstance.dateTo = el?.ranges[el?.rangeIndex]?.dateTo;
    this.modalRef.componentInstance.schedulingCodes = this.schedulingCodes;
  }

  openCopySchedule(el: AgentSchedulesResponse) {
    this.getModalPopup(CopyScheduleComponent, 'lg');
    this.modalRef.componentInstance.agentSchedulingGroupId = el?.ranges[el?.rangeIndex]?.agentSchedulingGroupId;
    this.modalRef.componentInstance.agentScheduleId = el?.id;
    this.modalRef.componentInstance.employeeId = el?.employeeId;
    this.modalRef.componentInstance.dateFrom = el?.ranges[el?.rangeIndex]?.dateFrom;
    this.modalRef.componentInstance.dateTo = el?.ranges[el?.rangeIndex]?.dateTo;
    this.modalRef.componentInstance.agentScheduleType = AgentScheduleType.Scheduling;
    this.modalRef.componentInstance.fromDate = new Date(this.startDate);

    this.modalRef.result.then((result) => { 
      if (result.needRefresh) {
        this.getModalPopup(MessagePopUpComponent, 'sm');
        this.setComponentMessages('Success', 'The record has been copied!');
        this.modalRef.result.then(() => {
          // this.loadAgentSchedule(agentScheduleId);
          this.loadAgentSchedules();
        });
      } else {
        this.getModalPopup(MessagePopUpComponent, 'sm');
        this.setComponentMessages('Success', 'No changes has been made!');
      }
    });

  }

  getIconDescription(week: number, openTime: any) {
    const weekData = this.selectedGrid?.agentScheduleCharts.find(x => x.day === +week);

    if (weekData) {
      const weekTimeData = weekData.charts.find(x => this.convertToDateFormat(openTime) >= this.convertToDateFormat(x?.startTime) &&
        this.convertToDateFormat(openTime) < this.convertToDateFormat(x?.endTime));
      if (weekTimeData) {
        const code = this.schedulingCodes.find(x => x.id === weekTimeData?.schedulingCodeId);
        return code?.description;
      }
    }

    return '';

  }

  onSchedulingGroupChange(schedulingGroupId: number) {
    this.agentSchedulingGroupId = schedulingGroupId;
    if (this.agentSchedulingGroupId) {
      this.currentPage = 1;
      this.pageSize = 3;
      this.loadAgentSchedules();
      this.selectedGrid = null;
    } else {
      this.totalSchedulingGridData = [];
      this.totalSchedulingRecord = undefined;
    }
  }

  search(searchText: string) {
    this.searchText = searchText;
    if (this.agentSchedulingGroupId) {
      this.loadAgentSchedules();
      if (this.selectedGrid) {
        this.cancel();
      }
    }
  }

  onSelectStartDate(date: string) {
    this.startDate = date;
    if (this.agentSchedulingGroupId) {
      this.loadAgentSchedules();
      if (this.selectedGrid) {
        this.cancel();
      }
    }
  }

  private showImportFinished(contentMessage: string, errors: string[], needRefresh = true) {
    this.getModalPopup(MessagePopUpComponent, 'md');
    this.modalRef.componentInstance.headingMessage = 'Import Finished';
    this.modalRef.componentInstance.contentMessage = contentMessage;
    this.modalRef.componentInstance.importErrors = errors;
  }

  openImportSchedule() {
    this.getModalPopup(ImportScheduleComponent, 'lg');
    this.modalRef.componentInstance.agentScheduleType = AgentScheduleType.Scheduling;

    this.modalRef.result.then((result) => {
      const message = result.partialImport ? 'The record has been paritially imported!' : 'The record has been imported!';
      // const res = result.msg;
      // const errors = res['errors'];
      // const importStatus = res["importStatus"];
      this.getModalPopup(MessagePopUpComponent, 'sm');
      this.setComponentMessages('Success', message);
      // this.showImportFinished(importStatus, errors);
      this.modalRef.result.then(() => {
        this.loadAgentSchedules();
        this.selectedGrid = null;
      });
    });
  }
  
  exportToExcel() {
    this.agentSchedulesService
      .exportAgentSchedulingGrid(this.agentSchedulingGroupId)
      .subscribe(response => {
        this.exportData = response;



        const today = new Date();
        const year = String(today.getFullYear());
        const month = String((today.getMonth() + 1)).length === 1 ?
          ('0' + String((today.getMonth() + 1))) : String((today.getMonth() + 1));
        const day = String(today.getDate()).length === 1 ?
          ('0' + String(today.getDate())) : String(today.getDate());
        const date = year + month + day;
        let fileName = `${this.exportFileName + date}.csv`;;
        let columnNames = ["EmployeeId", "StartDate", "EndDate", "ActivityCode", "StartTime", "EndTime"];
        let header = columnNames.join(',');
        let csv = header;
        csv += '\r\n';



        this.exportData.map(exportData => {
          csv += [
            exportData.employeeId,
            exportData.startDate,
            exportData.endDate,
            exportData.activityCode,
            exportData.startTime,
            exportData.endTime

          ].join(',');
          csv += '\r\n';
        })

        var blob = new Blob([csv], { type: "text/csv" });
        var link = document.createElement("a");
        if (link.download !== undefined) {
          var url = URL.createObjectURL(blob);
          link.setAttribute("href", url);
          link.setAttribute("download", fileName);
          document.body.appendChild(link);
          link.click();
          document.body.removeChild(link);

        }
      },
        error => {
          this.exportData = [];
        });


  }
  
  exportToExcelByEmployee(employeeId : string) {
    this.agentSchedulesService
      .exportAgentSchedulingGridByEmployee(employeeId)
      .subscribe(response => {
        this.exportData = response;



        const today = new Date();
        const year = String(today.getFullYear());
        const month = String((today.getMonth() + 1)).length === 1 ?
          ('0' + String((today.getMonth() + 1))) : String((today.getMonth() + 1));
        const day = String(today.getDate()).length === 1 ?
          ('0' + String(today.getDate())) : String(today.getDate());
        const date = year + month + day;
        let fileName = `${this.exportFileName + date + "_" + employeeId}.csv`;;
        let columnNames = ["EmployeeId", "StartDate", "EndDate", "ActivityCode", "StartTime", "EndTime"];
        let header = columnNames.join(',');
        let csv = header;
        csv += '\r\n';



        this.exportData.map(exportData => {
          csv += [
            exportData.employeeId,
            exportData.startDate,
            exportData.endDate,
            exportData.activityCode,
            exportData.startTime,
            exportData.endTime

          ].join(',');
          csv += '\r\n';
        })

        var blob = new Blob([csv], { type: "text/csv" });
        var link = document.createElement("a");
        if (link.download !== undefined) {
          var url = URL.createObjectURL(blob);
          link.setAttribute("href", url);
          link.setAttribute("download", fileName);
          document.body.appendChild(link);
          link.click();
          document.body.removeChild(link);

        }
      },
        error => {
          this.exportData = [];
        });


  }


  // exportToExcel() {
  //   const today = new Date();
  //   const year = String(today.getFullYear());
  //   const month = String((today.getMonth() + 1)).length === 1 ?
  //     ('0' + String((today.getMonth() + 1))) : String((today.getMonth() + 1));
  //   const day = String(today.getDate()).length === 1 ?
  //     ('0' + String(today.getDate())) : String(today.getDate());

  //   const date = year + month + day;
  //   this.excelService.exportAsExcelCSVFile(SchedulingExcelExportData, this.exportFileName + date);
  // }

  private setNewDateRangeInMemory(el: AgentSchedulesResponse, result: ScheduleDateRangeBase) {
    const range = new AgentScheduleRange();
    range.dateFrom = result?.dateFrom;
    range.dateTo = result?.dateTo;
    
    range.status = SchedulingStatus['Pending Schedule'];
    range.agentSchedulingGroupId = el.activeAgentSchedulingGroupId;
    range.scheduleCharts = [];
    el.ranges.push(range);
    el.rangeIndex = el.ranges.length - 1;
    el.modifiedBy = this.authService.getLoggedUserInfo().displayName;
    el.modifiedDate = new Date();

    el.ranges.sort((a, b) => a.dateFrom < b.dateFrom ? -1 : a.dateFrom > b.dateFrom ? 1 : 0);

    this.onDateRangeChange(el.rangeIndex, el);
    this.getModalPopup(MessagePopUpComponent, 'sm');
  }

  private setSelectedGrid(el: AgentSchedulesResponse) {
    this.selectedGrid = new AgentScheduleGridResponse();
    this.selectedGrid.id = el?.id;
    this.selectedGrid.employeeId = el?.employeeId;
    this.selectedGrid.firstName = el?.firstName;
    this.selectedGrid.lastName = el?.lastName;
    this.selectedGrid.dateFrom = el?.ranges[el?.rangeIndex]?.dateFrom;
    this.selectedGrid.dateTo = el?.ranges[el?.rangeIndex]?.dateTo;
    this.selectedGrid.status = el?.ranges[el?.rangeIndex]?.status;
    this.selectedGrid.activeAgentSchedulingGroupId = el?.activeAgentSchedulingGroupId;
    this.selectedGrid.agentScheduleCharts = el?.ranges[el?.rangeIndex]?.scheduleCharts;

    if (this.selectedGrid?.agentScheduleCharts?.length > 0) {
      this.selectedGrid.agentScheduleCharts.forEach(ele => {
        this.formatendTime(ele.charts, false);
      });
      this.selectedGrid?.agentScheduleCharts.map(x => x?.charts.map(y => {
        y.startTime = y?.startTime?.trim().toLowerCase();
        y.endTime = y?.endTime?.trim().toLowerCase();
      }));
    } else {
      this.selectedGrid.agentScheduleCharts = [];
    }
    this.sortSelectedGridCalendarTimes();
    this.formatTimeValuesInSchedulingGrid();
    this.schedulingGridData = JSON.parse(JSON.stringify(this.selectedGrid));

    this.timeStampUpdate = new Date().getTime();

    // console.log(this.schedulingGridData);
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

      week = elem.attributes.week.value;
      weekDays = this.selectedGrid?.agentScheduleCharts;
      weekData = weekDays.find(x => x.day === +week);

      if (this.icon && !this.isDelete) {

        if (weekData) {
          this.insertIconToGrid(weekData, iconModel);
        } else {
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

    this.timeStampUpdate = new Date().getTime();
  }

  private insertIconToGrid(weekData: AgentScheduleChart, insertIcon: ScheduleChart) {
    if (weekData.charts.find(x => x.startTime === insertIcon.startTime && x.endTime === insertIcon.endTime)) {
      const item = weekData.charts.find(x => x.startTime === insertIcon.startTime && x.endTime === insertIcon.endTime);
      item.schedulingCodeId = insertIcon.schedulingCodeId;
    }
    else if (weekData.charts.filter(x => this.convertToDateFormat(x.startTime) >= this.convertToDateFormat(insertIcon.startTime) &&
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
      const index = this.selectedGrid?.agentScheduleCharts.findIndex(x => x.day === weekData?.day);
      if (index > -1) {
        this.selectedGrid.agentScheduleCharts[index].charts = [];
      }
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
    const agentScheduleCharts = this.selectedGrid?.agentScheduleCharts;
    agentScheduleCharts.forEach((element) => {
      if (element.charts.length > 0) {
        element.charts = this.adjustSchedulingCalendarTimesRange(element.charts);
      }
    });
  }

  private sortSelectedGridCalendarTimes() {
    const agentScheduleCharts = this.selectedGrid?.agentScheduleCharts;
    agentScheduleCharts.forEach((element) => {
      if (element.charts.length > 0) {
        element.charts.sort((a, b): number => {
          if (this.convertToDateFormat(a?.startTime) < this.convertToDateFormat(b?.startTime)) {
            return -1;
          } else if (this.convertToDateFormat(a?.startTime) > this.convertToDateFormat(b?.startTime)) {
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
    agentSchedulesQueryParams.pageNumber = this.currentPage;
    agentSchedulesQueryParams.pageSize = this.pageSize;
    agentSchedulesQueryParams.searchKeyword = this.searchText ?? '';
    agentSchedulesQueryParams.skipPageSize = false;
    agentSchedulesQueryParams.orderBy = `${this.orderBy} ${this.sortBy}`;
    agentSchedulesQueryParams.fields = fields ?? undefined;
    agentSchedulesQueryParams.employeeIds = undefined;

    return agentSchedulesQueryParams;
  }

  private formatDateMoment(date) {
    // let dt = new Date(date).toUTCString();
    const transformedDate = moment(date).utc().format('YYYY-MM-DD');
    return transformedDate;
  }


  private loadAgentSchedules() {
    const queryParams = this.getQueryParams();
    this.spinnerService.show(this.spinner, SpinnerOptions);

    this.getAgentSchedulesSubscription = this.agentSchedulesService.getAgentSchedules(queryParams)
      .subscribe((response) => {
        this.totalSchedulingGridData = response.body;
        this.totalSchedulingGridData.map(x => {
          x.rangeIndex = 0;
          x.ranges.map(r => {
            r.dateFrom = this.formatDateMoment(r.dateFrom);
            r.dateTo = this.formatDateMoment(r.dateTo);
          })
          x.ranges.sort((a, b) => a.dateFrom < b.dateFrom ? -1 : a.dateFrom > b.dateFrom ? 1 : 0);
        });

        if (this.selectedGrid) {
          this.setSelectedGrid(this.totalSchedulingGridData.find(x => x.id === this.selectedGrid.id));
        }
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

  private updateAgentSchedule(el: AgentSchedulesResponse) {
    this.spinnerService.show(this.spinner, SpinnerOptions);
    const updateModel = new UpdateAgentSchedule();
    updateModel.dateFrom = el?.ranges[el?.rangeIndex]?.dateFrom;
    updateModel.dateTo = el?.ranges[el?.rangeIndex]?.dateTo;
    updateModel.status = el?.ranges[el?.rangeIndex]?.status;
    updateModel.modifiedBy = this.authService.getLoggedUserInfo()?.displayName;
    updateModel.activityOrigin = ActivityOrigin.CSS;
    updateModel.modifiedUser = +el?.employeeId;

    console.log(updateModel)

    const scheduleId = this.totalSchedulingGridData.find(x => x.id === el?.id)?.id;

    this.updateAgentScheduleSubscription = this.agentSchedulesService.
      updateAgentSchedule(scheduleId, updateModel)
      .subscribe(() => {
        this.spinnerService.hide(this.spinner);
        if (this.selectedGrid) {
          this.selectedGrid.status = el?.ranges[el?.rangeIndex]?.status;
        }
        el.modifiedBy = updateModel.modifiedBy;
        el.modifiedDate = new Date();
        this.getModalPopup(MessagePopUpComponent, 'sm');
        this.setComponentMessages('Success', 'The schedule status has been changed!');
      }, (error) => {
        this.spinnerService.hide(this.spinner);
        this.getModalPopup(ErrorWarningPopUpComponent, 'sm');
        this.setComponentMessages('Error', error?.message);
        this.modalRef.componentInstance.messageType = ContentType.String;
        this.loadAgentSchedules();
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
      let gridData = new AgentScheduleGridResponse();
      gridData = this.getUpdatedScheduleChart();
      if (gridData.agentScheduleCharts.length > 0) {
        gridData.agentScheduleCharts.forEach(ele => {
          this.formatendTime(ele.charts, true);
        });
      }
      chartModel.dateFrom = this.formatDateMoment(gridData?.dateFrom);
      chartModel.dateTo = this.formatDateMoment(gridData?.dateTo);
      chartModel.status = gridData?.status;
      chartModel.agentScheduleCharts = gridData?.agentScheduleCharts;
      chartModel.activityOrigin = ActivityOrigin.CSS;
      chartModel.modifiedUser = +this.authService.getLoggedUserInfo()?.employeeId;
      chartModel.modifiedBy = this.authService.getLoggedUserInfo()?.displayName;

      this.updateAgentScheduleChartSubscription = this.agentSchedulesService
        .updateAgentScheduleChart(agentScheduleId, chartModel)
        .subscribe(() => {
          this.spinnerService.hide(this.spinner);
          this.getModalPopup(MessagePopUpComponent, 'sm');
          this.setComponentMessages('Success', 'The record has been updated!');
          this.modalRef.result.then(() => {
            this.schedulingGridData = JSON.parse(JSON.stringify(this.selectedGrid));
            this.loadAgentSchedules();
          });
        }, (error) => {
          this.spinnerService.hide(this.spinner);
          console.log(error);
        });

      this.subscriptions.push(this.updateAgentScheduleChartSubscription);
    } else {
      this.getModalPopup(MessagePopUpComponent, 'sm');
      this.setComponentMessages('Success', 'No changes has been made!');
    }
  }

  private getUpdatedScheduleChart() {
    const updatedChart = JSON.parse(JSON.stringify(this.selectedGrid));
    this.selectedGrid?.agentScheduleCharts.forEach((x, index) => {
      if (this.schedulingGridData?.agentScheduleCharts.findIndex(y => y.day === x?.day) > -1) {
        const gridIndex = this.schedulingGridData?.agentScheduleCharts.findIndex(y => y.day === x.day);
        if (gridIndex > -1) {
          if (JSON.stringify(this.selectedGrid?.agentScheduleCharts[index]?.charts) ===
            JSON.stringify(this.schedulingGridData?.agentScheduleCharts[gridIndex]?.charts)) {
            const updateIndex = updatedChart.agentScheduleCharts.findIndex(y => y.day === x.day);
            if (updateIndex > -1) {
              updatedChart.agentScheduleCharts.splice(updateIndex, 1);
            }
          }
        }
      }
    });

    return updatedChart;
  }
  private formatFilterTimeFormat(timeDataISO: string) {

    let timeData = timeDataISO;

    if (timeData?.trim().toLowerCase().slice(0, 2) === '00') {
      timeData = '12' + timeData?.trim().toLowerCase().slice(2, 8);
    }
    if (timeData.trim().toLowerCase() === '11:60 pm') {
      timeData = '12:00 am';
    }

    return timeData;
  }
  // private getTimeFormat(dateISO: string){
  //   let date = new Date(dateISO);
  //   (charts.endTime),['h:mm a']).format("hh:mm a")
  //   const transformedDate = moment(date,).format();
  //   return transformedDate;
  // }

  private formatendTime(charts: ScheduleChart[], updateChart: boolean) {
    // for (const weekData of scheduleResponse?.ranges[this.selectedGrid?.rangeIndex]?.scheduleCharts) {
    //   if (weekData.charts.length > 0) {
    if (!updateChart) {
      const responseIndex = charts.findIndex(x => x?.endTime?.trim().toLowerCase() === '12:00 am');
      if (responseIndex > -1) {
        charts[responseIndex].endTime = '11:60 pm';
      }
      const twelveHourTime = charts.filter(x => x?.endTime?.trim().toLowerCase().slice(0, 2) === '12' ||
        x?.startTime?.trim().toLowerCase().slice(0, 2) === '12');
      if (twelveHourTime.length > 0) {
        twelveHourTime.map(x => {
          if (x?.endTime?.trim().toLowerCase().slice(0, 2) === '12') {
            x.endTime = '00' + x?.endTime?.trim().toLowerCase().slice(2, 8);
          }
          if (x?.startTime?.trim().toLowerCase().slice(0, 2) === '12') {
            x.startTime = '00' + x?.startTime?.trim().toLowerCase().slice(2, 8);
          }
        });
        this.sortSelectedGridCalendarTimes();
        this.formatTimeValuesInSchedulingGrid();
      }
    } else {
      const requestIndex = charts.findIndex(x => x?.endTime?.trim().toLowerCase() === '11:60 pm');
      if (requestIndex > -1) {
        charts[requestIndex].endTime = '12:00 am';
      }
      const zeroHourTime = charts.filter(x => x?.endTime?.trim().toLowerCase().slice(0, 2) === '00' ||
        x?.startTime?.trim().toLowerCase().slice(0, 2) === '00');
      if (zeroHourTime.length > 0) {
        zeroHourTime.map(x => {
          if (x?.endTime?.trim().toLowerCase().slice(0, 2) === '00') {
            x.endTime = '12' + x?.endTime?.trim().toLowerCase().slice(2, 8);
          }
          if (x?.startTime?.trim().toLowerCase().slice(0, 2) === '00') {
            x.startTime = '12' + x?.startTime?.trim().toLowerCase().slice(2, 8);
          }
        });
        this.sortSelectedGridCalendarTimes();
        this.formatTimeValuesInSchedulingGrid();
      }
    }

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

  private subscribeToTranslations() {
    this.getTranslationSubscription = this.languagePreferenceService.userLanguageChanged.subscribe(
      (language) => {
        if (language) {
          this.loadTranslations();
        }
      });

    this.subscriptions.push(this.getTranslationSubscription);
  }

  private preLoadTranslations() {
    // Preload the user language //
    const browserLang = this.route.snapshot.data.languagePreference.languagePreference;
    this.currentLanguage = browserLang ? browserLang : 'en';
    this.translate.use(this.currentLanguage);
  }

  private loadTranslations() {
    // load the user language from api //
    this.languagePreferenceService.getLanguagePreference(this.LoggedUser.employeeId).subscribe((langPref: LanguagePreference) => {
      this.currentLanguage = langPref.languagePreference ? langPref.languagePreference : 'en';
      this.translate.use(this.currentLanguage);
    });
  }

  private loadSchedulingCodes() {
    const queryParams = new SchedulingCodeQueryParams();
    queryParams.skipPageSize = true;
    queryParams.fields = 'id, description, icon';

    this.getSchedulingCodesSubscription = this.schedulingCodeService.getSchedulingCodes(queryParams)
      .subscribe((response) => {
        if (response.body) {
          this.schedulingCodes = response.body;
          this.iconCount = (this.schedulingCodes.length <= 30) ? this.schedulingCodes.length : this.maxIconCount;
          this.endIcon = this.iconCount;
        }
      }, (error) => {
        console.log(error);
      });

    this.subscriptions.push(this.getSchedulingCodesSubscription);
  }

  private getModalPopup(component: any, size: string) {
    const options: NgbModalOptions = { backdrop: 'static', centered: true, size };
    this.modalRef = this.modalService.open(component, options);
  }

  private getCalendarPopup(component: any, size: string, el: AgentSchedulesResponse) {
    const options: NgbModalOptions = { backdrop: 'static', centered: true, size };
    this.modalRef = this.modalService.open(component, options);
    this.modalRef.componentInstance.el = el;
  }


  private setComponentMessages(headingMessage: string, contentMessage: string) {
    this.modalRef.componentInstance.headingMessage = headingMessage;
    this.modalRef.componentInstance.contentMessage = contentMessage;
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

  private getFormattedDate(date: Date) {
    const transformedDate = this.datepipe.transform(date.toString().replace("Z", ""), 'yyyy-MM-dd');
    return new Date(`${transformedDate} 00:00`);
  }

  private getFormattedDateString(date: Date) {
    return this.datepipe.transform(date.toString().replace("Z", ""), 'yyyy-MM-dd');
  }
  // batch release
  openBatchRelease(batchRelease) {
    const options: NgbModalOptions = { backdrop: 'static', centered: true, size: 'lg' };
    this.modalService.open(batchRelease, options);
    this.clearBatchReleaseData();

  }




  private getQueryParamsForAsg(searchkeyword?: string) {
    const queryParams = new AgentSchedulingGroupQueryParams();
    queryParams.clientId = undefined;
    queryParams.clientLobGroupId = undefined;
    queryParams.skillGroupId = undefined;
    queryParams.skillTagId = undefined;
    queryParams.pageSize = this.agentSchedulingGroupItemsBufferSize;
    queryParams.pageNumber = this.pageNumber;
    queryParams.searchKeyword = searchkeyword ?? this.searchKeyWord;
    queryParams.skipPageSize = false;
    queryParams.orderBy = 'name';
    queryParams.sortBy = 'asc';
    queryParams.fields = 'id, name';

    return queryParams;
  }
  private getAgentSchedulingGroups(searchKeyword?: string) {
    const queryParams = this.getQueryParamsForAsg(searchKeyword);
    if (this.dropdownSearchKeyWord !== queryParams.searchKeyword) {
      this.pageNumber = 1;
      queryParams.pageNumber = 1;
    }
    this.dropdownSearchKeyWord = queryParams.searchKeyword;
    return this.agentSchedulingGroupService.getAgentSchedulingGroups(queryParams);
  }
  private subscribeToAgentSchedulingGroups(needBufferAdd?: boolean) {
    this.loading = true;
    this.getAgentSchedulingGroupSubscription = this.getAgentSchedulingGroups(this.dropdownSearchKeyWord).subscribe(
      response => {

        if (response?.body) {
          this.setPaginationValues(response);
          this.agentSchedulingGroupItemsBuffer = needBufferAdd ? this.agentSchedulingGroupItemsBuffer.concat(response.body) : response.body;
          this.asg = this.agentSchedulingGroupItemsBuffer;
        }
        this.loading = false;
      }, err => this.loading = false);

    this.subscriptions.push(this.getAgentSchedulingGroupSubscription);
  }
  private setPaginationValues(response: any) {
    const paging = JSON.parse(response.headers.get('x-pagination'));
    if (paging) {
      this.totalItems = paging.totalCount;
      this.totalPages = paging.totalPages;
    }
  }
  onAgentSchedulingGroupScrollToEnd() {
    this.fetchMoreAgentSchedulingGroups();
  }

  onAgentSchedulingGroupScroll({ end }) {
    if (this.loading || this.agentSchedulingGroupItemsBufferSize <= this.agentSchedulingGroupItemsBuffer.length) {
      return;
    }

    if (end + this.numberOfItemsFromEndBeforeFetchingMore >= this.agentSchedulingGroupItemsBuffer.length) {
      this.fetchMoreAgentSchedulingGroups();
    }
  }
  private subscribeToSearching() {
    this.typeAheadValueSubscription = this.typeAheadInput$.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      switchMap(term => this.getAgentSchedulingGroups(term))
    ).subscribe(response => {
      if (response.body) {
        this.setPaginationValues(response);
        this.asg = response.body;
      }
    }, (error) => {
      console.log(error);
    });

    this.subscriptions.push(this.typeAheadValueSubscription);
  }
  fetchMoreAgentSchedulingGroups() {
    if (this.pageNumber < this.totalPages) {
      this.pageNumber += 1;
      this.subscribeToAgentSchedulingGroups(true);
    }
  }
  // fetchMore(needBufferAdd?: boolean) {

  //       this.loading = true;
  //       this.getAgentSchedulingGroupSubscription = this.getAgentSchedulingGroups(this.dropdownSearchKeyWord).subscribe(
  //         response => {

  //           if (response?.body) {
  //             this.setPaginationValues(response);
  //             this.agentSchedulingGroupItemsBuffer = needBufferAdd ? this.agentSchedulingGroupItemsBuffer.concat(response.body) : response.body;
  //             this.itemList = this.agentSchedulingGroupItemsBuffer;
  //           }

  //         }, err => this.loading = false);
  //         this.agentSchedulingGroupService.getAgentSchedulingGroups
  // }
  fetchMore(event: any) {
    this.subscribeToAgentSchedulingGroups();
  }
  batchReleaseLoad() {

    // console.log(this.getAgentSchedulingGroupSubscription);
    this.subscribeToSearching();

    this.itemList = [];


    this.settings = {
      text: "Select Items",

      classes: "agent_multi_select",
      enableSearchFilter: true,
      lazyLoading: true,
      labelKey: 'id',
      limitSelection: 4,
      maxHeight: 200,

    };
    this.date_range_setting = {
      text: "Select date range",
      labelKey: 'id',
      limitSelection: 4,
      maxHeight: 200,
      singleSelection: false
    }

  }
  asgClose() {
    this.loadDateRange();
  }
  onDateSelect(item: any) {





    console.log(item)
  }

  onItemSelect(item: any) {



    this.agentSchedId = this.selectedItems.map(a => a.id);


  }
  OnItemDeSelect(item: any) {
    this.agentSchedId = this.selectedItems.map(a =>
      a.id,
    );


  }
  OnDateDeSelect(item: any) {
    console.log(item)
  }
  onSelectAll(items: any) {
    console.log(items);
  }

  private loadDateRange() {
    this.agentSchedulingGroupService.getDateRange(this.selectedAsg).subscribe(data => {
      data.map(x => {
        x.fullrange = x.dateFrom + ' - ' + x.dateTo;
      })
      this.dateRange = data;
      // console.log(data)

    }, (error) => {
      console.log(error)
      this.date_ranges = [];
    }
    )
  }
  dateRangeOpen() {

    this.loadDateRange();

  }
  onClose() {
    this.loadDateRange();
  }

  clearBatchReleaseData() {
    this.selectedAsg = [];
    this.selectedRange = [];
  }

  private release() {
    let pushArr = Array();
    let releaseObject: any;
    this.selectedRange.forEach(element => {
      releaseObject = {
        agentSchedulingGroupId: element.agentSchedulingGroupId,
        dateFrom: this.changeToUTCDate(this.getFormattedDate(element.dateFrom)),
        dateTo: this.changeToUTCDate(this.getFormattedDate(element.dateTo)),
        activityOrigin: ActivityOrigin.CSS,

      }
      pushArr.push(releaseObject)
    });

    let releaseObjArrays: any[];
    let insertObject: BatchReleaseModel;
    releaseObjArrays = pushArr;
    insertObject = {
      batchReleaseDetails:
        releaseObjArrays,
      modifiedUser: +this.authService.getLoggedUserInfo()?.employeeId,
      modifiedBy: this.authService.getLoggedUserInfo()?.displayName
    };

    this.agentSchedulesService.batchRelease(insertObject).subscribe(res => {
      this.modalService.dismissAll();
      releaseObject = [];
      insertObject = undefined;
      pushArr = [];
      this.selectedAsg = [];
      this.selectedRange = [];
      this.toast.success(`${res} Date range release`, "Operation successful");
      this.loadAgentSchedules();
    }, (error) =>{
      console.log(error)
      this.toast.error(error);
      this.modalService.dismissAll();
      this.loadAgentSchedules();
      releaseObject = [];
      insertObject = undefined;
      pushArr = [];
      this.selectedAsg = [];
      this.selectedRange = [];
    });

  }
  confirmRelease() {
    // this.getModalPopup(GenericPopUpComponent, 'md');
    // this.setConfirmDialogMessages(`Are you sure you? You won't be able to revert this!`, ``, `Yes`, `No`);
    this.getModalPopup(GenericPopUpComponent, 'sm');



    this.setComponentMessages('Batch Release', "Are you sure ?");
    this.modalRef.componentInstance.warning = true;
    this.modalRef.result.then((result) => {
      if (result && result === true) {
        this.release();
      }
    });
  }

}
