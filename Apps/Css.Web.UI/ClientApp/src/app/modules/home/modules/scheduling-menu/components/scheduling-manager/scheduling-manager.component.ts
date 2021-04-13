import { CdkDragDrop } from '@angular/cdk/drag-drop';
import { Component, ElementRef, OnDestroy, OnInit, ViewChild, AfterViewInit, HostListener } from '@angular/core';
import { NgbModal, NgbModalOptions, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { from, Observable, SubscriptionLike as ISubscription } from 'rxjs';
import { NgxSpinnerService } from 'ngx-spinner';
import { DatePipe, formatDate, WeekDay } from '@angular/common';

import { Constants } from 'src/app/shared/util/constants.util';
import { SpinnerOptions } from 'src/app/shared/util/spinner-options.util';
import { SortingType } from '../../enums/sorting-type.enum';
import { AgentScheduleType } from '../../enums/agent-schedule-type.enum';

import { AgentChartResponse } from '../../models/agent-chart-response.model';
import { SchedulingCode } from '../../../system-admin/models/scheduling-code.model';
import { AgentScheduleManagerChart } from '../../models/agent-schedule-manager-chart.model';
import { AgentInfo } from '../../models/agent-info.model';
import { UpdateAgentScheduleMangersChart } from '../../models/update-agent-schedule-managers-chart.model';
import { AgentShceduleMangerData } from '../../models/agent-schedule-manager-data.model';

import { AgentAdminService } from '../../services/agent-admin.service';
import { AuthService } from 'src/app/core/services/auth.service';

import { CopyScheduleComponent } from '../shared/copy-schedule/copy-schedule.component';
import { MessagePopUpComponent } from 'src/app/shared/popups/message-pop-up/message-pop-up.component';
import { ActivityLogsScheduleComponent } from '../shared/activity-logs-schedule/activity-logs-schedule.component';

import { AgentIconFilter } from '../../models/agent-icon-filter.model';
import { ActivityOrigin } from '../../enums/activity-origin.enum';
import { ActivityType } from 'src/app/shared/enums/activity-type.enum';

declare function setManagerRowCellIndex(cell, row);
declare function highlightManagerSelectedCells(table: string, cell: string);
declare function highlightCell(cell: string, className: string);
import * as $ from 'jquery';
import { ExcelData } from '../../models/excel-data.model';
import { ImportScheduleComponent } from '../shared/import-schedule/import-schedule.component';
import { ExcelService } from 'src/app/shared/services/excel.service';
import { SchedulingMangerExcelExportData } from '../../constants/scheduling-manager-excel-export-data';
import { LanguagePreferenceService } from 'src/app/shared/services/language-preference.service';
import { TranslateService } from '@ngx-translate/core';
import { LanguagePreference } from 'src/app/shared/models/language-preference.model';
import { SchedulingCodeQueryParams } from '../../../system-admin/models/scheduling-code-query-params.model';
import { ActivatedRoute } from '@angular/router';
import { SchedulingCodeService } from 'src/app/shared/services/scheduling-code.service';
import { PermissionsService } from '../../../system-admin/services/permissions.service';
import { AgentScheduleManagersService } from '../../services/agent-schedule-managers.service';
import { AgentScheduleManagersQueryParams } from '../../models/agent-schedule-mangers-query-params.model';
import * as moment from 'moment';
import { OpenTimeData, ScheduleManagerAgentChartResponse, ScheduleManagerAgentCharts, ScheduleManagerAgentData, ScheduleManagerChart, ScheduleManagerChartDisplay, ScheduleManagerChartUpdate, ScheduleManagerGridChartDisplay } from '../../models/schedule-manager-chart.model';
import { GenericPopUpComponent } from 'src/app/shared/popups/generic-pop-up/generic-pop-up.component';
import { ActivityLogsScheduleManagerComponent } from '../shared/activity-logs-schedule-manager/activity-logs-schedule-manager.component';
import { HeaderPagination } from 'src/app/shared/models/header-pagination.model';
import { PaginationSize } from 'src/app/shared/models/pagination-size.model';



@Component({
  selector: 'app-scheduling-manager',
  templateUrl: './scheduling-manager.component.html',
  styleUrls: ['./scheduling-manager.component.scss'],
  providers: [DatePipe]
})
export class SchedulingManagerComponent implements OnInit, OnDestroy {
  startIcon = 0;
  maxIconCount = 30;
  timeIntervals = 15;
  characterSplice = 25;
  endIcon: number;
  iconCount: number;
  selectedRow: number;
  totalSchedulingRecord: number;
  agentSchedulingGroupId: number;

  currentPage = 1;
  pageSize = 10;
  paginationSize: PaginationSize[] = [
    {
      count: 10,
      text: '10/Page'
    },
    {
      count: 25,
      text: '25/Page'
    },
    {
      count: 50,
      text: '50/Page'
    }
  ];

  headerPaginationValues: HeaderPagination;
  totalScheduleMangerRecord: number;

  iconDescription: string;
  icon: string;
  iconCode: string;
  selectedIconId: string;
  startDateTimeFilter: string;
  endDateTimeFilter: string;
  orderBy = 'createdDate';
  sortBy = 'desc';
  spinner = 'scheduling-manager';
  selectedCellClassName = 'cell-selected';
  tableClassName = 'schedulingManagerTable';
  scheduleSpinner = 'SchedulingSpinner';
  exportFileName = 'Attendance_scheduling';
  startDate: string;
  searchText: string;
  currentLanguage: string;
  isMouseDown: boolean;

  isDelete: boolean;
  LoggedUser;

  schedulingIntervals = Constants.schedulingIntervals;
  sortTypeValue = SortingType.Ascending;
  sortType = SortingType;
  openTimes: Array<any>;
  modalRef: NgbModalRef;
  agentInfo: AgentInfo;
  openTimeAgentIcon: AgentIconFilter;
  lunchAgentIcon: AgentIconFilter;

  schedulingCodes: SchedulingCode[] = [];
  importedData: ExcelData[] = [];
  sortingType: any[] = [];
  weekDays: Array<string> = [];
  managerCharts: ScheduleManagerAgentChartResponse[] = [];
  schedulingMangerChart: ScheduleManagerAgentChartResponse[] = [];

  getSchedulingCodesSubscription: ISubscription;
  getTranslationSubscription: ISubscription;
  updateAgentManagerChartSubscription: ISubscription;
  getAgentInfoSubscription: ISubscription;
  getAgentSchedulesSubscription: ISubscription;
  subscriptions: ISubscription[] = [];

  dateHeaders: Array<any>;
  headerColSpan: number;

  // managerGridDisplay: ScheduleManagerGridChartDisplay[] = [];
  managerGridDisplay$: Observable<Array<ScheduleManagerGridChartDisplay>>;
  managerGridDisplayObj: ScheduleManagerGridChartDisplay[] = [];

  timeStampUpdate: number;

  @ViewChild('stickyMenu') menuElement: ElementRef;
  sticky: boolean = false;
  elementPosition: any;

  @ViewChild('stickyMenu1') menuElement1: ElementRef;
  sticky1: boolean = false;
  elementPosition1: any;

  @ViewChild('stickyMenu2') menuElement2: ElementRef;
  sticky2: boolean = false;
  elementPosition2: any;



  constructor(
    private agentScheduleMangerService: AgentScheduleManagersService,
    private spinnerService: NgxSpinnerService,
    private agentAdminService: AgentAdminService,
    private authService: AuthService,
    private modalService: NgbModal,
    private excelService: ExcelService,
    private schedulingCodeService: SchedulingCodeService,
    private route: ActivatedRoute,
    private languagePreferenceService: LanguagePreferenceService,
    private permissionsService: PermissionsService,
    private datepipe: DatePipe,
    public translate: TranslateService,
  ) {
    this.LoggedUser = this.authService.getLoggedUserInfo();
    this.managerGridDisplay$ = this.agentScheduleMangerService.scheduleMangerChartsGrid$;
  }

  ngOnInit(): void {
    this.weekDays = Object.keys(WeekDay).filter(key => isNaN(WeekDay[key]));
    this.sortingType = Object.keys(SortingType).filter(key => isNaN(SortingType[key]));
    this.loadSchedulingCodes();
    this.preLoadTranslations();
    this.loadTranslations();
    this.subscribeToTranslations();
    this.openTimes = this.getOpenTimes();
    // console.log(this.getOpenTimesHeaders());
  }
  ngAfterViewInit() {
    this.elementPosition = this.menuElement.nativeElement.offsetTop;
    this.elementPosition1 = this.menuElement.nativeElement.offsetTop;
    this.elementPosition2 = this.menuElement.nativeElement.offsetTop;
  }
  @HostListener('window:scroll', ['$event'])
  handleScroll() {
    const windowScroll = window.pageYOffset;
    if (windowScroll >= this.elementPosition) {
      this.sticky = true;
    } else {
      this.sticky = false;
    }
    if (windowScroll >= this.elementPosition1) {
      this.sticky1 = true;
    } else {
      this.sticky1 = false;
    }
    if (windowScroll >= this.elementPosition2) {
      this.sticky2 = true;
    } else {
      this.sticky2 = false;
    }
  }
  popOverAction(popover) {
    if (popover.isOpen()) {
      popover.close();
    } else {
      popover.open();
    }
  }

  ngOnDestroy() {
    this.subscriptions.forEach(subscription => {
      if (subscription) {
        subscription.unsubscribe();
      }
    });
    this.managerGridDisplayObj = [];
    this.agentScheduleMangerService.scheduleMangerChartsGridSubject$.next(undefined);
    // this.reloadAgentScheduleManager();
  }

  canShowActivityLog() {
    const rolesPermitted = [1, 2, 3, 5];
    const userRoleId = this.permissionsService.userRoleId;
    return rolesPermitted.indexOf(+userRoleId) > -1;
  }

  onSchedulingGroupChange(schedulingGroupId: number) {
    this.agentSchedulingGroupId = schedulingGroupId;
    this.openTimes = this.getOpenTimes();
    if (this.agentSchedulingGroupId) {
      this.loadAgentScheduleManger();

    } else {
      this.clearIconFilters();
      this.managerCharts = [];
      this.totalSchedulingRecord = undefined;
      this.totalScheduleMangerRecord = undefined;
      this.managerGridDisplayObj = [];
      this.agentScheduleMangerService.scheduleMangerChartsGridSubject$.next(undefined);
    }

  }

  changePageSize(pageSize: number) {
    this.pageSize = pageSize;
    // if (this.agentSchedulingGroupId) {
    this.loadAgentScheduleManger();
    // }
  }

  changePage(page: number) {
    this.currentPage = page;
    // if (this.agentSchedulingGroupId) {
    this.loadAgentScheduleManger();
    // }
  }

  search(searchText: string) {
    this.searchText = searchText;
    if (this.agentSchedulingGroupId) {
      this.loadAgentScheduleManger();
    }
  }

  onSelectStartDate(date: string) {
    this.startDate = date;
    // console.log(date)
    // this.startDate = new Date(new Date(date).toString().replace(/\sGMT.*$/, " GMT+0000")).toString();
    // console.log(this.startDate)
    this.openTimes = this.getOpenTimes();
    if (this.agentSchedulingGroupId) {
      if (this.agentSchedulingGroupId) {
        this.loadAgentScheduleManger();
      }
    }
  }

  openImportSchedule() {
    this.getModalPopup(ImportScheduleComponent, 'lg');
    this.modalRef.componentInstance.agentScheduleType = AgentScheduleType.SchedulingManager;
    this.modalRef.componentInstance.agentSchedulingGroupId = this.agentSchedulingGroupId;

    this.modalRef.result.then((result) => {
      const message = result.partialImport ? 'The record has been paritially imported!' : 'The record has been imported!';
      this.getModalPopup(MessagePopUpComponent, 'sm', message);
      this.modalRef.result.then(() => {
        this.loadAgentScheduleManger();
      });
    });
  }


  exportToExcel() {

    let columnNames = ["EmployeeId", "Date", "Activity Code", "Start DateTime", "End DateTime"];
    let header = columnNames.join(',');

    let csv = header;
    csv += '\r\n';

    let exportData = this.managerCharts;
    let date = this.startDate;
    let dateNoSeparators = date.replace(/-/g, '');
    let fileName = `${this.exportFileName + date}.csv`;

    exportData.map(c => {
      c.charts.map(x => {
        if (x.schedulingCodeId && x.schedulingCodeId !== undefined) {
          // const startDateTime = moment(x.startDateTime).format('yyyy-MM-DD hh:mm A');
          // const endDateTime = moment(x.endDateTime).format('yyyy-MM-DD hh:mm A');

          const code = this.schedulingCodes.find(a => a.id === x?.schedulingCodeId);
          csv += [c.employeeId,
            dateNoSeparators,
          code.description,
          x.startDateTime,
          x.endDateTime].join(',');
          csv += '\r\n';
        }
      });
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
  }

  // exportToExcels() {
  //   const today = new Date();
  //   const year = String(today.getFullYear());
  //   const month = String((today.getMonth() + 1)).length === 1 ?
  //     ('0' + String((today.getMonth() + 1))) : String((today.getMonth() + 1));
  //   const day = String(today.getDate()).length === 1 ?
  //     ('0' + String(today.getDate())) : String(today.getDate());

  //   const date = year + month + day;
  //   this.excelService.exportAsExcelCSVFile(SchedulingMangerExcelExportData, this.exportFileName + date);
  // }

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
    if (this.agentSchedulingGroupId) {
      this.sortBy = sortBy === 'asc' ? 'desc' : 'asc';
      this.orderBy = columnName;

      this.loadAgentScheduleManger();
    }
  }

  isMainMinute(data: any) {
    return data.split(':')[1].trim().toLowerCase() === '00 am' || data.split(':')[1].trim().toLowerCase() === '00 pm';
  }

  isRowSelected(index: number) {
    return this.selectedRow === index;
  }

  setAgent(employeeId: number, index: number) {
    // this.setIconFilters(employeeId);
    // this.setAgentIconFilters(employeeId);
    this.selectedRow = index;
    if (employeeId) {
      this.loadAgentInfo(employeeId);
    }
  }

  setIconFilters(employeeId: number) {
    if (this.schedulingMangerChart.find(x => x.employeeId === +employeeId)?.charts?.length > 0) {
      const agent = this.schedulingMangerChart.find(x => x.employeeId === +employeeId)?.charts[0];
      const schedulingCode = this.schedulingCodes.find(x => x.id === agent?.schedulingCodeId);
      this.iconDescription = schedulingCode?.description;
      this.startDateTimeFilter = this.formatFilterTimeFormat(agent?.startDateTime);
      this.endDateTimeFilter = this.formatFilterTimeFormat(agent?.endDateTime);
      this.iconCode = schedulingCode?.icon?.value;
    } else {
      this.clearIconFilters();
    }

  }

  setAgentIconFilters(employeeId: number) {
    const openTime = this.schedulingCodes?.find(x => x.description.trim().toLowerCase() === 'open time');
    const lunch = this.schedulingCodes?.find(x => x.description.trim().toLowerCase() === 'lunch');
    const agentScheduleData = this.schedulingMangerChart.find(x => x.employeeId === +employeeId);
    if (agentScheduleData?.charts?.length > 0) {
      if (openTime) {
        const openTimeIndex = this.schedulingMangerChart.find(x => x.employeeId === +employeeId)?.charts
          .findIndex(x => x.schedulingCodeId === openTime?.id);
        if (openTimeIndex > -1) {
          this.openTimeAgentIcon = new AgentIconFilter();
          this.openTimeAgentIcon.codeValue = openTime?.icon?.value;
          this.openTimeAgentIcon.startDateTime = this.formatFilterTimeFormat(agentScheduleData?.
            charts[openTimeIndex]?.startDateTime);
          this.openTimeAgentIcon.endDateTime = this.formatFilterTimeFormat(agentScheduleData?.
            charts[openTimeIndex]?.endDateTime);
        } else {
          this.openTimeAgentIcon = undefined;
        }
      }
      if (lunch) {
        const lunchIndex = this.schedulingMangerChart.find(x => x.employeeId === +employeeId)?.charts
          .findIndex(x => x.schedulingCodeId === lunch?.id);
        if (lunchIndex > -1) {
          this.lunchAgentIcon = new AgentIconFilter();
          this.lunchAgentIcon.codeValue = lunch?.icon?.value;
          this.lunchAgentIcon.startDateTime = this.formatFilterTimeFormat(agentScheduleData?.
            charts[lunchIndex]?.startDateTime);
          this.lunchAgentIcon.endDateTime = this.formatFilterTimeFormat(agentScheduleData?.
            charts[lunchIndex]?.endDateTime);
        } else {
          this.lunchAgentIcon = undefined;
        }
      }
    } else {
      this.openTimeAgentIcon = undefined;
      this.lunchAgentIcon = undefined;
    }
  }

  clearIconFilters() {
    this.iconDescription = undefined;
    this.startDateTimeFilter = undefined;
    this.endDateTimeFilter = undefined;
    this.iconCode = undefined;
    this.openTimeAgentIcon = undefined;
    this.lunchAgentIcon = undefined;
  }

  onMouseUp(event) {
    this.isMouseDown = false;
    if (this.isDelete) {
      this.saveGridItems();
      this.removeHighlightedCells();
    } else {
      $(event.currentTarget).addClass(this.selectedCellClassName);
      // highlightCell(event.currentTarget.id, this.selectedCellClassName);
      this.saveGridItems();
      this.removeHighlightedCells();
    }
  }

  onMouseDown(event: any, cell: number, row: number) {
    if (event.button === 0) {
      let object;
      // this.removeHighlightedCells();
      this.isMouseDown = true;

      const dateTime = event.currentTarget.attributes.date.nodeValue;
      const employeeId = event.currentTarget.attributes.employeeId.nodeValue;
      const chart = this.managerGridDisplayObj.find(x => x.employeeId === +employeeId);
      object = chart?.charts?.find(x => this.getTimeStamp(dateTime) >= this.getTimeStamp(x?.startDateTime) &&
        this.getTimeStamp(dateTime) < this.getTimeStamp(x?.endDateTime));

      if (object) {
        // console.log(object)
        const code = this.schedulingCodes.find(x => x.id === object?.schedulingCodeId);
        this.icon = code?.icon?.value ?? undefined;
        if (this.isMouseDown && this.icon) {
          // console.log(cell, row)
          // setManagerRowCellIndex(cell, row);
          $(event.currentTarget).addClass(this.selectedCellClassName);
        }
      }
      // if (this.isMouseDown && !this.icon) {
      //   this.isDelete = true;
      //   // setManagerRowCellIndex(cell, row);
      //   $(event.currentTarget).addClass(this.selectedCellClassName);
      // } 
    }
    return false;
  }


  onRightClick(event) {
    const startDateTime = event.currentTarget.attributes.startDateTime.nodeValue;
    const endDateTime = event.currentTarget.attributes.endDateTime.nodeValue;
    const employeeId = event.currentTarget.attributes.employeeId.nodeValue;
    const chart = this.managerGridDisplayObj.find(x => x.employeeId === +employeeId);

    // let insertIcon: ScheduleManagerChartDisplay;


    // const dateTime = event.currentTarget.attributes.date.nodeValue;
    // const employeeId = event.currentTarget.attributes.employeeId.nodeValue;
    // const chart = this.managerGridDisplayObj.find(x => x.employeeId === +employeeId);
    // itemChart = chart?.charts?.find(x => moment(dateTime).isSameOrAfter(new Date(x.startDateTime).toISOString()) && 
    //   moment(dateTime).isBefore(new Date(x.endDateTime).toISOString()));

    // if (itemChart.schedulingCodeId !== undefined) {
    //   itemChart.schedulingCodeId = undefined;
    //   itemChart.schedulingIcon = '';
    //   // const options: NgbModalOptions = { backdrop: 'static', centered: true, size: 'sm' };
    //   // this.modalRef = this.modalService.open(GenericPopUpComponent, options);
    //   // this.modalRef.componentInstance.headingMessage = '';
    //   // this.modalRef.componentInstance.contentMessage = 'Are you sure you want to remove this item?';

    //   // this.modalRef.componentInstance.warning = true;
    //   // this.modalRef.result.then((result) => {
    //   //   if (result && result === true) {
    //       // itemChart.schedulingCodeId = undefined;
    //       // itemChart.schedulingIcon = '';
    //       // this.getModalPopup(MessagePopUpComponent, 'sm', "Deleted Successfully!");
    //       // this.modalRef.result.then(() => {
    //       //   this.loadAgentScheduleManger();
    //       // });
    //     // }
    //   // });
    // }


    // charts?.find(x => {
    //   if(moment(insertIcon.startDateTime).isSame(x.startDateTime) && 
    //   moment(insertIcon.endDateTime).isSame(x.endDateTime)){
    //     x.schedulingCodeId = insertIcon.schedulingCodeId;
    //     x.schedulingIcon = this.unifiedToNative(insertIcon.schedulingIcon);
    //   }
    // });

    chart.charts.find(x => {
      if (this.getTimeStamp(startDateTime) == this.getTimeStamp(x?.startDateTime) &&
        this.getTimeStamp(endDateTime) == this.getTimeStamp(x?.endDateTime)) {
        x.schedulingCodeId = undefined;
      }
    });

    const timeDataArray = chart.charts.filter(x => this.getTimeStamp(x.startDateTime) >=
      this.getTimeStamp(startDateTime) &&
      this.getTimeStamp(x.endDateTime) <= this.getTimeStamp(endDateTime));

    if (timeDataArray.length > 0) {

      timeDataArray.forEach(ele => {
        ele.schedulingCodeId = undefined;
      });

      timeDataArray.filter(ele => {
        ele.schedulingCodeId !== undefined;
      });

    }

    // this.agentScheduleMangerService.scheduleMangerChartsGridSubject$.next(this.managerGridDisplayObj);

    // const timeDataArray = weekData.charts.filter(x => this.convertToDateFormat(x.startTime) >=
    //   this.convertToDateFormat(insertIcon.startTime) &&
    //   this.convertToDateFormat(x.endTime) <= this.convertToDateFormat(insertIcon.endTime));
    // if (timeDataArray.length > 0) {
    //   // timeDataArray.forEach(ele => {
    //   //   ele.schedulingCodeId = insertIcon.schedulingCodeId;
    //   // });
    // } else {
    //   // const calendarTime = new ScheduleChart(insertIcon.startTime, insertIcon.endTime, insertIcon.schedulingCodeId);
    //   // weekData.charts.push(calendarTime);
    // }
    this.managerCharts = Object.assign(this.managerCharts, this.managerGridDisplayObj);

    this.timeStampUpdate = new Date().getTime();

    event.preventDefault();

    return false;
  }

  disableRightClick(e){
    e.preventDefault();
  }

  onMouseOver(event) {
    // this.removeHighlightedCells();
    if (this.isMouseDown && this.icon) {
      $(event.currentTarget).addClass(this.selectedCellClassName);
    }
    // else if(this.isMouseDown && this.isDelete){
    //   $(event.currentTarget).addClass(this.selectedCellClassName);
    // }
    // if (this.isMouseDown && this.icon) {
    //   highlightManagerSelectedCells(this.tableClassName, event.currentTarget.id);
    // } else if (this.isMouseDown && this.isDelete) {
    //   highlightManagerSelectedCells(this.tableClassName, event.currentTarget.id);
    // }
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
    // if (this.agentSchedulingGroupId) {
    //   this.loadAgentScheduleManger();
    // }
    // this.reloadAgentScheduleManager();
  }

  dragged(event: CdkDragDrop<any>) {
    this.icon = event.item.element.nativeElement.id;
  }


  getGridIconDescription(employeeId: number, dateTime) {
    const chart = this.managerCharts.find(x => x.employeeId === +employeeId);

    if (chart?.charts?.length > 0) {
      const weekTimeData = chart?.charts?.find(x => this.getTimeStamp(dateTime) >= this.getTimeStamp(x?.startDateTime) &&
        this.getTimeStamp(dateTime) < this.getTimeStamp(x?.endDateTime));

      if (weekTimeData) {
        const code = this.schedulingCodes.find(x => x.id === weekTimeData?.schedulingCodeId);
        return code?.description;
      }
    }

    return '';
  }

  getTimeStamp(date: any) {
    if (date) {
      date = new Date(date)?.getTime()
    }

    return date;
  }

  getGridIcon(employeeId: number, dateTime: any) {

    const chart = this.managerCharts?.find(x => x.employeeId === +employeeId);
    if (chart?.charts?.length > 0) {
      const weekTimeData = chart?.charts?.find(x => this.getTimeStamp(dateTime) >= this.getTimeStamp(x?.startDateTime) &&
        this.getTimeStamp(dateTime) < this.getTimeStamp(x?.endDateTime));

      if (weekTimeData) {
        const code = this.schedulingCodes.find(x => x.id === weekTimeData?.schedulingCodeId);
        // // console.log(i++)
        return code ? this.unifiedToNative(code?.icon?.value) : '';
        // return weekTimeData.schedulingCodeId;
      }
    }

    return '';
  }


  getAgentHireDate() {
    if (this.agentInfo?.agentData.length > 0) {
      let value;
      for (const item of this.agentInfo?.agentData) {
        if (item?.group?.description.trim().toLowerCase() === 'hire date') {
          value = item?.group?.value;
          break;
        }
      }
      return value;
    }

    return '';
  }

  openActivityLogs(index: number) {
    this.getModalPopup(ActivityLogsScheduleManagerComponent, 'xl');
    this.modalRef.componentInstance.activityType = ActivityType.SchedulingManagerGrid;
    this.modalRef.componentInstance.employeeId = this.managerCharts[index]?.employeeId;
    this.modalRef.componentInstance.employeeName = this.agentInfo?.lastName + ', ' + this.agentInfo?.firstName;
    this.modalRef.componentInstance.startDate = new Date(this.startDate);
    this.modalRef.componentInstance.schedulingCodes = this.schedulingCodes;
  }

  openCopySchedule(index: number) {
    const employeeId = this.managerCharts[index]?.employeeId;
    this.getModalPopup(CopyScheduleComponent, 'lg');
    this.modalRef.componentInstance.agentSchedulingGroupId = this.agentSchedulingGroupId;
    this.modalRef.componentInstance.employeeId = employeeId;
    this.modalRef.componentInstance.agentScheduleType = AgentScheduleType.SchedulingManager;
    this.modalRef.componentInstance.fromDate = new Date(this.startDate);

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

  cancel() {
    this.loadAgentScheduleManger();
  }

  removeEmptyGrids(charts: ScheduleManagerChart[]) {
    return charts.filter(x => x.schedulingCodeId !== null)
  }

  save() {
    if (this.matchManagerChartDataChanges()) {
      // this.formatTime(true);
      const managerChartModel = new ScheduleManagerChartUpdate();
      this.spinnerService.show(this.scheduleSpinner, SpinnerOptions);
      for (const item of this.managerCharts) {
        if (JSON.stringify(item.charts) !== JSON.stringify(this.schedulingMangerChart.find(x => x.id === item.id).charts)) {
          let chartsForUpdate: ScheduleManagerChart[];
          chartsForUpdate = item.charts.filter(x => x.schedulingCodeId !== undefined);

          const employeeData = new ScheduleManagerAgentData();
          employeeData.employeeId = item?.employeeId;
          const agentScheduleManagerChart = new ScheduleManagerAgentCharts();
          // let now = new Date(`${item?.date} 00:00`);
          // console.log('dt = ' + now.toString().replace(/\sGMT.*$/, " GMT+0000"));
          agentScheduleManagerChart.date = item?.date;
          agentScheduleManagerChart.charts = chartsForUpdate;


          employeeData.agentScheduleManagerCharts.push(agentScheduleManagerChart);
          managerChartModel.scheduleManagers.push(employeeData);
        }
      }
      managerChartModel.activityOrigin = ActivityOrigin.CSS;
      managerChartModel.modifiedUser = +this.authService.getLoggedUserInfo().employeeId;
      managerChartModel.modifiedBy = this.authService.getLoggedUserInfo().displayName;
      managerChartModel.isImport = false;


      this.updateAgentManagerChartSubscription = this.agentScheduleMangerService.updateScheduleManagerChart(managerChartModel)
        .subscribe((response) => {
          this.spinnerService.hide(this.scheduleSpinner);
          this.schedulingMangerChart = JSON.parse(JSON.stringify(this.managerCharts));
          this.getModalPopup(MessagePopUpComponent, 'sm', 'The record has been updated!');
          this.modalRef.result.then(() => {
            this.loadAgentScheduleManger();
          });
        }, (error) => {
          this.spinnerService.hide(this.scheduleSpinner);
          console.log(error);
        });

      this.subscriptions.push(this.updateAgentManagerChartSubscription);
    } else {
      this.getModalPopup(MessagePopUpComponent, 'sm', 'No changes has been made!');
    }

  }

  private formatFilterTimeFormat(timeDataISO: string) {

    let timeData = this.getFormattedTime(timeDataISO);

    if (timeData?.trim().toLowerCase().slice(0, 2) === '00') {
      timeData = '12' + timeData?.trim().toLowerCase().slice(2, 8);
    }
    if (timeData.trim().toLowerCase() === '11:60 pm') {
      timeData = '12:00 am';
    }

    return timeData;
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
    this.languagePreferenceService.getLanguagePreference(this.LoggedUser?.employeeId).subscribe((langPref: LanguagePreference) => {
      this.currentLanguage = langPref.languagePreference ? langPref.languagePreference : 'en';
      this.translate.use(this.currentLanguage);
    });
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

  private matchManagerChartDataChanges() {
    if (JSON.stringify(this.schedulingMangerChart) !== JSON.stringify(this.managerGridDisplayObj)) {
      return true;
    }
  }

  private loadAgentInfo(employeeId: number) {
    this.spinnerService.show(this.scheduleSpinner, SpinnerOptions);

    this.getAgentInfoSubscription = this.agentAdminService.getAgentInfo(employeeId)
      .subscribe((response) => {
        this.agentInfo = response;
        this.spinnerService.hide(this.scheduleSpinner);
      }, (error) => {
        this.spinnerService.hide(this.scheduleSpinner);
        console.log(error);
      });

    this.subscriptions.push(this.getAgentInfoSubscription);
  }

  private getQueryParams() {
    const agentSchedulesQueryParams = new AgentScheduleManagersQueryParams();
    agentSchedulesQueryParams.agentSchedulingGroupId = this.agentSchedulingGroupId;
    agentSchedulesQueryParams.date = this.startDate.toString();
    agentSchedulesQueryParams.pageNumber = this.currentPage;
    agentSchedulesQueryParams.pageSize = this.pageSize;
    agentSchedulesQueryParams.searchKeyword = this.searchText ?? '';
    agentSchedulesQueryParams.orderBy = `${this.orderBy} ${this.sortBy}`;
    agentSchedulesQueryParams.fields = '';
    agentSchedulesQueryParams.employeeId = undefined;
    // agentSchedulesQueryParams.skipPageSize = true;

    return agentSchedulesQueryParams;
  }

  private reloadAgentScheduleManager() {
    this.agentScheduleMangerService.scheduleMangerChartsGridSubject$.next(this.managerGridDisplayObj);
    // this.managerGridDisplay$ = this.agentScheduleMangerService.scheduleMangerChartsGrid$;
  }

  private loadAgentScheduleManger() {
    this.clearIconFilters();
    if (this.agentSchedulingGroupId) {
      const queryParams = this.getQueryParams();
      this.spinnerService.show(this.scheduleSpinner, SpinnerOptions);
        this.getAgentSchedulesSubscription = this.agentScheduleMangerService.getAgentScheduleManagers(queryParams)
          .subscribe((response) => {
            // unsubscribe first
            this.agentScheduleMangerService.scheduleMangerChartsGridSubject$.next(undefined);
            this.managerGridDisplayObj = [];
            this.managerCharts = response.body;

            this.headerPaginationValues = JSON.parse(response.headers.get('x-pagination'));
            this.totalScheduleMangerRecord = this.headerPaginationValues.totalCount;

            if (this.managerCharts.length > 0) {
              this.managerGridDisplayObj = Object.assign(this.managerGridDisplayObj, this.managerCharts);

              // this.managerGridDisplayObj.map(x => {
              //   x.charts = this.getGridIconData(x.employeeId);      
              // });

              this.managerGridDisplayObj.map(x => {
                if (!x.charts || x.charts.length === 0) {
                  x.charts = [];
                }
              });

              // console.log(this.managerCharts)

              this.reloadAgentScheduleManager();

              this.managerCharts = Object.assign(this.managerCharts, this.managerGridDisplayObj);

              // this.agentScheduleMangerService.scheduleMangerChartsGridSubject$.next(this.managerGridDisplayObj);

              // this.managerGridDisplay$ = this.agentScheduleMangerService.scheduleMangerChartsGrid$;

              // this.formatTime(false);
              this.schedulingMangerChart = JSON.parse(JSON.stringify(this.managerCharts));

              const employeeId = this.managerCharts[0]?.employeeId;
              // const agentScheduleManagerChart = this.managerCharts[0]?.charts?.length > 0 ? this.managerCharts[0].charts[0] : undefined;
              // if (agentScheduleManagerChart) {
              //   this.setIconFilters(employeeId);
              // }
              this.setAgent(employeeId, 0);
            } else {
              this.agentScheduleMangerService.scheduleMangerChartsGridSubject$.next(undefined);
            }

            this.spinnerService.hide(this.scheduleSpinner);
          }, (error) => {
            this.spinnerService.hide(this.scheduleSpinner);
            console.log(error);
          });

        this.subscriptions.push(this.getAgentSchedulesSubscription);
    }
  }

  private formatTime(updateChart: boolean) {
    for (const item of this.managerCharts) {
      if (!updateChart && item?.charts?.length > 0) {
        const index = item?.charts.findIndex(x => x?.endDateTime?.trim().toLowerCase() === '00:00 am' ||
          x.endDateTime.trim().toLowerCase() === '12:00 am');
        if (index > -1) {
          item.charts[index].endDateTime = '11:60 pm';
        }
        item?.charts.map(x => {
          if (x?.endDateTime?.trim().toLowerCase().slice(0, 2) === '12') {
            x.endDateTime = '00' + x?.endDateTime?.trim().toLowerCase().slice(2, 8);
          }
          if (x?.startDateTime?.trim().toLowerCase().slice(0, 2) === '12') {
            x.startDateTime = '00' + x?.startDateTime?.trim().toLowerCase().slice(2, 8);
          }
        });
        if (item.charts.length === 0) {
          item.charts = [];
        }
      } else if (updateChart && item.charts.length > 0) {
        const index = item?.charts.findIndex(x => x?.endDateTime?.trim().toLowerCase() === '11:60 pm');
        if (index > -1) {
          item.charts[index].endDateTime = '12:00 am';
        }
        item?.charts.map(x => {
          if (x?.endDateTime?.trim().toLowerCase().slice(0, 2) === '00') {
            x.endDateTime = '12' + x?.endDateTime?.trim().toLowerCase().slice(2, 8);
          }
          if (x?.startDateTime?.trim().toLowerCase().slice(0, 2) === '00') {
            x.startDateTime = '12' + x?.startDateTime?.trim().toLowerCase().slice(2, 8);
          }
        });
        const nullValueIndex = item?.charts?.findIndex(x => x.schedulingCodeId === null);
        if (nullValueIndex > -1) {
          item.charts.splice(nullValueIndex, 1);
        }
      }
    }
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
      let chartArray;
      let employeeId;
      this.spinnerService.show(this.scheduleSpinner, SpinnerOptions);

      const fromTime = elem.attributes.startDateTime.value;
      const toTime = elem.attributes.endDateTime.value;

      const code = this.schedulingCodes.find(x => x?.icon?.value?.trim().toLowerCase() === this.icon?.trim().toLowerCase());
      var iconModel = new ScheduleManagerChartDisplay(fromTime, toTime, code?.id);
      // iconModel.schedulingIcon = code?.icon.value;


      const date = new Date(this.startDate);

      employeeId = elem?.attributes?.employeeId?.value;
      // const chart = this.managerCharts.find(x => x.employeeId === +employeeId);
      const chart = this.managerGridDisplayObj.find(x => x.employeeId === +employeeId);
      chartArray = chart?.charts?.length > 0 ? chart.charts : chart.charts = [];
      chart.date = date;

      if (this.icon && !this.isDelete) {

        if (chartArray?.length > 0) {
          this.insertGridIcon(chartArray, iconModel);
        } else {
          const calendarTime = new ScheduleManagerChart(fromTime, toTime, code?.id);
          chartArray.push(calendarTime);
        }
      } else if (this.isDelete) {
        if (chartArray.length > 0) {
          this.clearIconFromGrid(chartArray, iconModel);
          if (chartArray.length === 0) {
            chartArray = [];
          }
        }
      }
    });
    // this.sortSelectedGridCalendarTimes();
    // this.formatTimeValuesInSchedulingGrid();

    //update pipe transform timestamp
    this.timeStampUpdate = new Date().getTime();

    this.spinnerService.hide(this.scheduleSpinner);
    this.isDelete = false;
    this.icon = undefined;

    table.find('.' + this.selectedCellClassName).removeClass(this.selectedCellClassName);

  }

  private formatTimeValuesInSchedulingGrid() {
    for (const item of this.managerCharts) {
      item.charts = this.adjustSchedulingCalendarTimesRange(item.charts);
    }
  }

  private sortSelectedGridCalendarTimes() {
    this.managerCharts.forEach((element) => {
      if (element?.charts?.length > 0) {
        element?.charts.sort((a, b): number => {
          if (this.convertToDateFormat(a.startDateTime) < this.convertToDateFormat(b.startDateTime)) {
            return -1;
          } else if (this.convertToDateFormat(a.startDateTime) > this.convertToDateFormat(b.startDateTime)) {
            return 1;
          }
          else {
            return 0;
          }
        });
      }
    });
  }


  private insertGridIcon1(charts: ScheduleManagerChartDisplay[], insertIcon: ScheduleManagerChartDisplay) {

    // charts?.find(x => {
    //   if(moment(insertIcon.startDateTime).isSame(x.startDateTime) && 
    //   moment(insertIcon.endDateTime).isSame(x.endDateTime)){
    //     x.schedulingCodeId = insertIcon.schedulingCodeId;
    //     x.schedulingIcon = this.unifiedToNative(insertIcon.schedulingIcon);
    //   }
    // });

    // charts.map(x =>{
    //   if(this.getTimeStamp(insertIcon.startDateTime) == this.getTimeStamp(x?.startDateTime) &&
    //   this.getTimeStamp(insertIcon.endDateTime) == this.getTimeStamp(x?.endDateTime))
    //   {
    //     x.schedulingCodeId = insertIcon.schedulingCodeId;
    //   }else if(this.getTimeStamp(x.startDateTime) >=
    //   this.getTimeStamp(insertIcon.startDateTime) &&
    //   this.getTimeStamp(x.endDateTime) <= this.getTimeStamp(insertIcon.endDateTime)
    //   ){
    //     x.schedulingCodeId = insertIcon.schedulingCodeId;
    //   }
    // });

    const timeDataArray = charts.filter(x => this.getTimeStamp(x.startDateTime) >=
      this.getTimeStamp(insertIcon.startDateTime) &&
      this.getTimeStamp(x.endDateTime) <= this.getTimeStamp(insertIcon.endDateTime));

    if (timeDataArray.length > 0) {

      timeDataArray.forEach(ele => {
        ele.schedulingCodeId = insertIcon.schedulingCodeId;
      });

    } else {
      const calendarTime = new ScheduleManagerChartDisplay(insertIcon.startDateTime, insertIcon.endDateTime, insertIcon.schedulingCodeId);
      charts.push(calendarTime);
    }

    // this.agentScheduleMangerService.scheduleMangerChartsGridSubject$.next(this.managerGridDisplayObj);

    // const timeDataArray = weekData.charts.filter(x => this.convertToDateFormat(x.startTime) >=
    //   this.convertToDateFormat(insertIcon.startTime) &&
    //   this.convertToDateFormat(x.endTime) <= this.convertToDateFormat(insertIcon.endTime));
    // if (timeDataArray.length > 0) {
    //   // timeDataArray.forEach(ele => {
    //   //   ele.schedulingCodeId = insertIcon.schedulingCodeId;
    //   // });
    // } else {
    //   // const calendarTime = new ScheduleChart(insertIcon.startTime, insertIcon.endTime, insertIcon.schedulingCodeId);
    //   // weekData.charts.push(calendarTime);
    // }
    this.managerCharts = Object.assign(this.managerCharts, this.managerGridDisplayObj);
  }

  private insertGridIcon(charts: ScheduleManagerChartDisplay[], insertIcon: ScheduleManagerChartDisplay) {
    if (charts.find(x => this.getTimeStamp(x.startDateTime) === this.getTimeStamp(insertIcon.startDateTime) && this.getTimeStamp(x.endDateTime) === this.getTimeStamp(insertIcon.endDateTime))) {
      const item = charts.find(x => this.getTimeStamp(x.startDateTime) === this.getTimeStamp(insertIcon.startDateTime) && this.getTimeStamp(x.endDateTime) === this.getTimeStamp(insertIcon.endDateTime));
      item.schedulingCodeId = insertIcon.schedulingCodeId;
    } else if (charts.filter(x => this.getTimeStamp(x.startDateTime) >= this.getTimeStamp(insertIcon.startDateTime) &&
      this.getTimeStamp(x.endDateTime) <= this.getTimeStamp(insertIcon.endDateTime)).length > 0) {
      const timeDataArray = charts.filter(x => this.getTimeStamp(x.startDateTime) >=
        this.getTimeStamp(insertIcon.startDateTime) &&
        this.getTimeStamp(x.endDateTime) <= this.getTimeStamp(insertIcon.endDateTime));
      timeDataArray.forEach(ele => {
        const startIndex = charts.findIndex(x =>
          this.getTimeStamp(x.startDateTime) === this.getTimeStamp(ele.startDateTime));
        if (startIndex > -1) {
          charts.splice(startIndex, 1);
        }
      });
      if (timeDataArray.length > 0) {
        const calendarTime = new ScheduleManagerChartDisplay(insertIcon.startDateTime, insertIcon.endDateTime, insertIcon.schedulingCodeId);
        charts.push(calendarTime);
      }
      this.sortSelectedGridCalendarTimes();
      this.formatTimeValuesInSchedulingGrid();
    }
    if (!charts.find(x => x.startDateTime === insertIcon.startDateTime && x.endDateTime ===
      insertIcon.endDateTime && x.schedulingCodeId === insertIcon.schedulingCodeId)) {
      charts.forEach(ele => {
        if (this.getTimeStamp(ele.startDateTime) < this.getTimeStamp(insertIcon.startDateTime) &&
          this.getTimeStamp(ele.endDateTime) === this.getTimeStamp(insertIcon.startDateTime)) {
          ele.endDateTime = insertIcon.startDateTime;
        } else if (this.getTimeStamp(ele.startDateTime) === this.getTimeStamp(insertIcon.startDateTime) &&
          this.getTimeStamp(ele.endDateTime) < this.getTimeStamp(insertIcon.endDateTime)) {
          ele.endDateTime = insertIcon.endDateTime;
        } else if (this.getTimeStamp(ele.startDateTime) > this.getTimeStamp(insertIcon.startDateTime) &&
          this.getTimeStamp(ele.endDateTime) <= this.getTimeStamp(insertIcon.endDateTime)) {
          ele.startDateTime = insertIcon.endDateTime;
        } else if (this.getTimeStamp(ele.startDateTime) === this.getTimeStamp(insertIcon.startDateTime) &&
          this.getTimeStamp(ele.endDateTime) > this.getTimeStamp(insertIcon.endDateTime)) {
          ele.startDateTime = insertIcon.endDateTime;
        } else if (this.getTimeStamp(ele.startDateTime) < this.getTimeStamp(insertIcon.startDateTime) &&
          this.getTimeStamp(insertIcon.endDateTime) < this.getTimeStamp(ele.endDateTime)) {
          const calendarTime = new ScheduleManagerChartDisplay(insertIcon.endDateTime, ele.endDateTime, ele.schedulingCodeId);
          charts.push(calendarTime);
          ele.endDateTime = insertIcon.startDateTime;
        } else if (this.getTimeStamp(ele.endDateTime) === this.getTimeStamp(insertIcon.endDateTime) &&
          this.getTimeStamp(ele.startDateTime) < this.getTimeStamp(insertIcon.startDateTime)) {
          ele.endDateTime = insertIcon.startDateTime;
        } else if (this.getTimeStamp(ele.startDateTime) > this.getTimeStamp(insertIcon.startDateTime) &&
          this.getTimeStamp(ele.endDateTime) > this.getTimeStamp(insertIcon.endDateTime)) {
          // const calendarTime = new ScheduleManagerChart(insertIcon.startDateTime, insertIcon.endDateTime, insertIcon.schedulingCodeId);
          // weekData.charts.push(calendarTime);
          // ele.startDateTime = insertIcon.endDateTime;
        }
      });
      const timeDataArray = charts.filter(x => this.getTimeStamp(x.startDateTime) >=
        this.getTimeStamp(insertIcon.startDateTime) &&
        this.getTimeStamp(x.endDateTime) <= this.getTimeStamp(insertIcon.endDateTime));
      if (timeDataArray.length > 0) {
        timeDataArray.forEach(ele => {
          ele.schedulingCodeId = insertIcon.schedulingCodeId;
        });
      } else {
        const calendarTime = new ScheduleManagerChartDisplay(insertIcon.startDateTime, insertIcon.endDateTime, insertIcon.schedulingCodeId);
        charts.push(calendarTime);
      }
    }



  }

  private clearIconFromGrid(charts: ScheduleManagerChartDisplay[], icon: ScheduleManagerChartDisplay) {
    if (charts.findIndex(x => this.getTimeStamp(x.startDateTime) === this.getTimeStamp(icon.startDateTime) && this.getTimeStamp(x.endDateTime) === this.getTimeStamp(icon.endDateTime)) > -1) {
      const startIndex = charts.findIndex(x => this.getTimeStamp(x.startDateTime) === this.getTimeStamp(icon.startDateTime) && this.getTimeStamp(x.endDateTime) === this.getTimeStamp(icon.endDateTime));
      charts.splice(startIndex, 1);
    } else if (charts.filter(x => this.getTimeStamp(x.startDateTime) >= this.getTimeStamp(icon.startDateTime) &&
      this.getTimeStamp(x.endDateTime) <= this.getTimeStamp(icon.endDateTime)).length > 0) {
      const timeDataArray = charts.filter(x => this.getTimeStamp(x.startDateTime) >= this.getTimeStamp(icon.startDateTime) &&
        this.getTimeStamp(x.endDateTime) <= this.getTimeStamp(icon.endDateTime));
      timeDataArray.forEach(ele => {
        const startIndex = charts.findIndex(x =>
          this.getTimeStamp(x.startDateTime) === this.getTimeStamp(ele.startDateTime));
        if (startIndex > -1) {
          charts.splice(startIndex, 1);
        }
      });
    }
    if (charts.findIndex(x => x.startDateTime === icon.startDateTime && x.endDateTime === icon.endDateTime) < 0) {
      charts.forEach(ele => {
        if (this.getTimeStamp(ele.startDateTime) < this.getTimeStamp(icon.startDateTime) &&
          this.getTimeStamp(ele.endDateTime) === this.getTimeStamp(icon.startDateTime)) {
          ele.endDateTime = icon.startDateTime;
        } else if (this.getTimeStamp(ele.startDateTime) > this.getTimeStamp(icon.startDateTime) &&
          this.getTimeStamp(ele.endDateTime) <= this.getTimeStamp(icon.endDateTime)) {
          ele.startDateTime = icon.endDateTime;
        } else if (ele.startDateTime === icon.startDateTime && this.getTimeStamp(ele.endDateTime) > this.getTimeStamp(icon.endDateTime)) {
          ele.startDateTime = icon.endDateTime;
        } else if (this.getTimeStamp(ele.startDateTime) < this.getTimeStamp(icon.startDateTime) &&
          this.getTimeStamp(icon.endDateTime) < this.getTimeStamp(ele.endDateTime)) {
          const calendarTime = new ScheduleManagerChartDisplay(icon.endDateTime, ele.endDateTime, ele.schedulingCodeId);
          charts.push(calendarTime);
          ele.endDateTime = icon.startDateTime;
        } else if (this.getTimeStamp(ele.endDateTime) === this.getTimeStamp(icon.endDateTime) &&
          this.getTimeStamp(ele.startDateTime) < this.getTimeStamp(icon.startDateTime)) {
          ele.endDateTime = icon.startDateTime;
        } else if (this.getTimeStamp(ele.startDateTime) > this.getTimeStamp(icon.startDateTime) &&
          this.getTimeStamp(ele.endDateTime) > this.getTimeStamp(icon.endDateTime)) {
          // ele.startDateTime = icon.endDateTime;
        } else if (this.getTimeStamp(ele.endDateTime) > this.getTimeStamp(icon.startDateTime) &&
          this.getTimeStamp(ele.startDateTime) <= this.getTimeStamp(icon.startDateTime)) {
          // ele.endDateTime = icon.startDateTime;
          // weekData.charts.find(x => x.startDateTime || x.endDateTime)
        }
      });
      const timeDataArray = charts.filter(x => this.getTimeStamp(x.startDateTime) >= this.getTimeStamp(icon.startDateTime) &&
        this.getTimeStamp(x.endDateTime) <= this.getTimeStamp(icon.endDateTime));
      if (timeDataArray.length > 0) {
        timeDataArray.forEach(ele => {
          const startIndex = charts.findIndex(x =>
            this.getTimeStamp(x.startDateTime) === this.getTimeStamp(ele.startDateTime));
          if (startIndex > -1) {
            charts.splice(startIndex, 1);
          }
        });
      }
    }
  }

  private adjustSchedulingCalendarTimesRange(times: Array<ScheduleManagerChart>) {
    const newTimesarray = new Array<ScheduleManagerChart>();
    let calendarTimes = new ScheduleManagerChart(null, null, null);

    for (const index in times) {
      if (+index === 0) {
        calendarTimes = times[index];
        if (+index === times.length - 1) {
          break;
        }
      } else if (calendarTimes.endDateTime === times[index].startDateTime && calendarTimes.schedulingCodeId === times[index].schedulingCodeId) {
        calendarTimes.endDateTime = times[index].endDateTime;
        if (+index === times.length - 1) {
          break;
        }
      } else {
        const model = new ScheduleManagerChart(calendarTimes.startDateTime, calendarTimes.endDateTime, calendarTimes.schedulingCodeId);
        newTimesarray.push(model);
        calendarTimes = times[index];
        if (+index === times.length - 1) {
          break;
        }
      }
    }

    const modelvalue = new ScheduleManagerChart(calendarTimes.startDateTime, calendarTimes.endDateTime, calendarTimes.schedulingCodeId);
    console.log(modelvalue)
    newTimesarray.push(modelvalue);

    return newTimesarray;

  }

  private getDateInStringFormat(startDate: any): string {
    if (!startDate) {
      return undefined;
    }

    const date = new Date(startDate);
    return date.toISOString();
  }

  private changeToUTCDate(date) {
    return new Date(new Date(date).toString().replace(/\sGMT.*$/, " GMT+0000"));
  }


  // public getGridIconData(employeeId){
  //   const chart = this.managerCharts.find(x => x.employeeId === +employeeId);

  //   const x = this.timeIntervals;
  //   var schedulManagerCharts:ScheduleManagerChartDisplay[] = [];
  //   let tt = 0;
  //   // let now = moment().startOf('day');

  //   // // const ap = ['am', 'pm'];
  //   // if(this.startDate){
  //   //   now = moment(this.startDate);
  //   // }

  //   var now = new Date(new Date().setHours(0,0,0,0));

  //   if(this.startDate){
  //     now = new Date(new Date(this.startDate).setHours(0,0,0,0));
  //   }    

  //   now = new Date(new Date(now.toString().replace(/\sGMT.*$/, " GMT+0000")).setHours(0,0,0,0));

  //   // console.log(now.toString().replace(/\sGMT.*$/, " GMT+0000"));
  //   // console.log(new Date(now.toString().replace(/\sGMT.*$/, " GMT+0000")));
  //   // var time;

  //   if(chart?.charts?.length > 0){
  //     for (let i = 0; tt < 48 * 60; i++) {
  //       let time = this.datepipe.transform(now, 'hh:mm a').toLowerCase();
  //       let date = this.changeToUTCDate(now);

  //       const weekTimeData = chart?.charts?.find(x => this.getTimeStamp(date) >= this.getTimeStamp(x?.startDateTime) &&
  //       this.getTimeStamp(date) < this.getTimeStamp(x?.endDateTime));

  //       if (weekTimeData) {          
  //         schedulManagerCharts[i] = new ScheduleManagerChartDisplay(weekTimeData.startDateTime, weekTimeData.endDateTime, weekTimeData.schedulingCodeId);
  //         schedulManagerCharts[i].time = time;
  //         schedulManagerCharts[i].date = this.changeToUTCDate(now).toISOString().slice(0,-5)+"Z";
  //         const code = this.schedulingCodes.find(x => x.id === weekTimeData?.schedulingCodeId);
  //         schedulManagerCharts[i].schedulingIcon = this.unifiedToNative(code.icon.value);
  //       }else{
  //         let startDateTime = this.changeToUTCDate(now).toISOString().slice(0,-5)+"Z";

  //         let et = this.addIntervalDate(now, 'minute', x);
  //         let endDateTime = this.changeToUTCDate(et).toISOString().slice(0,-5)+"Z";

  //         schedulManagerCharts[i] = new ScheduleManagerChartDisplay(startDateTime, endDateTime, undefined);
  //         schedulManagerCharts[i].time = time;
  //         schedulManagerCharts[i].date = this.changeToUTCDate(now).toISOString().slice(0,-5)+"Z";
  //         schedulManagerCharts[i].schedulingIcon = ''; 
  //       }

  //       now = this.addIntervalDate(now, 'minute', x);
  //       tt = tt + x;
  //     }
  //   }else{
  //     for (let i = 0; tt < 48 * 60; i++) {
  //       let time = this.datepipe.transform(now, 'hh:mm a').toLowerCase();

  //       let startDateTime = this.changeToUTCDate(now).toISOString().slice(0,-5)+"Z";

  //       let et = this.addIntervalDate(now, 'minute', x);
  //       let endDateTime = this.changeToUTCDate(et).toISOString().slice(0,-5)+"Z";


  //       schedulManagerCharts[i] = new ScheduleManagerChartDisplay(startDateTime, endDateTime, undefined);
  //       schedulManagerCharts[i].time = time;
  //       schedulManagerCharts[i].date = this.changeToUTCDate(now).toISOString().slice(0,-5)+"Z";
  //       // schedulManagerCharts[i].schedulingCodeId = undefined;
  //       // schedulManagerCharts[i].startDateTime = moment(now).toISOString();
  //       // schedulManagerCharts[i].endDateTime = moment(now).add(x, 'minutes').toISOString();
  //       schedulManagerCharts[i].schedulingIcon = '';        

  //       now = this.addIntervalDate(now, 'minute', x);
  //       tt = tt + x;
  //     }
  //   }
  //   // console.log(schedulManagerCharts)
  //   return schedulManagerCharts;
  // }

  private getOpenTimesHeaders() {
    const x = this.timeIntervals;
    var times: OpenTimeData[] = [];
    let tt = 0;

    let now = moment().startOf('day');
    if (this.startDate) {
      now = moment(this.startDate);
    }

    for (let i = 0; tt < 48 * 60; i++) {
      let dateTime = now.format("hh:mm a");
      times[i] = new OpenTimeData;
      times[i].time = dateTime;

      times[i].date = moment(now).toISOString();
      times[i].dateHeader = moment(now).format('yyyy-MM-DD');
      now = moment(now).add(x, 'minutes');
      tt = tt + x;
    }

    return times;

  }

  private getOpenTimes() {
    // const x = this.timeIntervals;
    // var times:OpenTimeData[] = [];
    // let tt = 0;

    // var now = new Date(new Date().setHours(0,0,0,0));

    // if(this.startDate){
    //   now = new Date(new Date(this.startDate).setHours(0,0,0,0));
    // }    

    // now = new Date(new Date(now.toString().replace(/\sGMT.*$/, " GMT+0000")).setHours(0,0,0,0));

    // for (let i = 0; tt < 48 * 60; i++) {
    //   let dateTime = this.datepipe.transform(now, 'hh:mm a').toLowerCase();
    //   times[i] = new OpenTimeData;
    //   times[i].time = dateTime;

    //   times[i].date = this.changeToUTCDate(now).toISOString().slice(0,-5)+"Z";
    //   times[i].dateHeader = this.datepipe.transform(now, 'yyyy-MM-dd');
    //   now = this.addIntervalDate(now, 'minute', x);
    //   tt = tt + x;
    // }

    // return times;

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

    // now = new Date(new Date(now.toString().replace(/\sGMT.*$/, " GMT+0000")).setHours(0,0,0,0));
    // now = new Date(new Date(now.toString().replace(/\sGMT.*$/, " GMT+0000")).setHours(0,0,0,0));

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

  private getFormattedDate(date: Date) {
    const transformedDate = this.datepipe.transform(date, 'yyyy-MM-dd');
    return new Date(transformedDate);
  }

  private getFormattedTime(dateISO: string) {
    let date = new Date(dateISO);
    const transformedDate = this.datepipe.transform(date, 'yyyy-MM-dd h:mm a');
    return transformedDate;
  }

  private convertToDateTime(dateString) {
    const transformedDate = new Date(dateString);
    return transformedDate;
  }


  private addIntervalDate(date, interval, units) {
    if (!(date instanceof Date))
      return undefined;
    var ret = new Date(date); //don't change original date
    var checkRollover = function () { if (ret.getDate() != date.getDate()) ret.setDate(0); };
    switch (String(interval).toLowerCase()) {
      case 'year': ret.setFullYear(ret.getFullYear() + units); checkRollover(); break;
      case 'quarter': ret.setMonth(ret.getMonth() + 3 * units); checkRollover(); break;
      case 'month': ret.setMonth(ret.getMonth() + units); checkRollover(); break;
      case 'week': ret.setDate(ret.getDate() + 7 * units); break;
      case 'day': ret.setDate(ret.getDate() + units); break;
      case 'hour': ret.setTime(ret.getTime() + units * 3600000); break;
      case 'minute': ret.setTime(ret.getTime() + units * 60000); break;
      case 'second': ret.setTime(ret.getTime() + units * 1000); break;
      default: ret = undefined; break;
    }
    return ret;
  }


}
