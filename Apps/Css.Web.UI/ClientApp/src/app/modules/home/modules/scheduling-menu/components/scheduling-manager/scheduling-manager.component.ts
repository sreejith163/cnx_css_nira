import { CdkDragDrop } from '@angular/cdk/drag-drop';
import { Component, OnDestroy, OnInit } from '@angular/core';
import { NgbModal, NgbModalOptions, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { SubscriptionLike as ISubscription } from 'rxjs';
import { NgxSpinnerService } from 'ngx-spinner';
import { DatePipe, WeekDay } from '@angular/common';

import { Constants } from 'src/app/shared/util/constants.util';
import { SpinnerOptions } from 'src/app/shared/util/spinner-options.util';
import { SortingType } from '../../enums/sorting-type.enum';
import { AgentScheduleType } from '../../enums/agent-schedule-type.enum';

import { AgentChartResponse } from '../../models/agent-chart-response.model';
import { SchedulingCode } from '../../../system-admin/models/scheduling-code.model';
import { ScheduleChart } from '../../models/schedule-chart.model';
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

  iconDescription: string;
  icon: string;
  iconCode: string;
  selectedIconId: string;
  startTimeFilter: string;
  endTimeFilter: string;
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
  managerCharts: AgentChartResponse[] = [];
  schedulingMangerChart: AgentChartResponse[] = [];

  getSchedulingCodesSubscription: ISubscription;
  getTranslationSubscription: ISubscription;
  updateAgentManagerChartSubscription: ISubscription;
  getAgentInfoSubscription: ISubscription;
  getAgentSchedulesSubscription: ISubscription;
  subscriptions: ISubscription[] = [];

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
  }

  ngOnInit(): void {
    this.openTimes = this.getOpenTimes();
    this.weekDays = Object.keys(WeekDay).filter(key => isNaN(WeekDay[key]));
    this.sortingType = Object.keys(SortingType).filter(key => isNaN(SortingType[key]));
    this.loadSchedulingCodes();
    this.preLoadTranslations();
    this.loadTranslations();
    this.subscribeToTranslations();
  }

  ngOnDestroy() {
    this.subscriptions.forEach(subscription => {
      if (subscription) {
        subscription.unsubscribe();
      }
    });
  }

  canShowActivityLog() {
    const rolesPermitted = [1, 2, 3, 5];
    const userRoleId = this.permissionsService.userRoleId;
    return rolesPermitted.indexOf(+userRoleId) > -1;
  }

  onSchedulingGroupChange(schedulingGroupId: number) {
    this.agentSchedulingGroupId = schedulingGroupId;
    if (this.agentSchedulingGroupId) {
      this.loadAgentScheduleManger();
    } else {
      this.clearIconFilters();
      this.managerCharts = [];
      this.totalSchedulingRecord = undefined;
    }
  }

  search(searchText: string) {
    this.searchText = searchText;
    if (this.agentSchedulingGroupId) {
      this.loadAgentScheduleManger();
    }
  }

  onSelectStartDate(date: string) {
    this.startDate = date;
    if (this.agentSchedulingGroupId) {
      this.loadAgentScheduleManger();
    }
  }

  openImportSchedule() {
    this.getModalPopup(ImportScheduleComponent, 'lg');
    this.modalRef.componentInstance.agentScheduleType = AgentScheduleType.SchedulingManager;

    this.modalRef.result.then((result) => {
      const message = result.partialImport ? 'The record has been paritially imported!' : 'The record has been imported!';
      this.getModalPopup(MessagePopUpComponent, 'sm', message);
      this.modalRef.result.then(() => {
        this.loadAgentScheduleManger();
      });
    });
  }

  exportToExcel() {
    const today = new Date();
    const year = String(today.getFullYear());
    const month = String((today.getMonth() + 1)).length === 1 ?
      ('0' + String((today.getMonth() + 1))) : String((today.getMonth() + 1));
    const day = String(today.getDate()).length === 1 ?
      ('0' + String(today.getDate())) : String(today.getDate());

    const date = year + month + day;
    this.excelService.exportAsExcelFile(SchedulingMangerExcelExportData, this.exportFileName + date);
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
    this.setIconFilters(employeeId);
    this.setAgentIconFilters(employeeId);
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
      this.startTimeFilter = this.formatFilterTimeFormat(agent?.startTime);
      this.endTimeFilter = this.formatFilterTimeFormat(agent?.endTime);
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
          this.openTimeAgentIcon.startTime = this.formatFilterTimeFormat(agentScheduleData?.
            charts[openTimeIndex]?.startTime);
          this.openTimeAgentIcon.endTime = this.formatFilterTimeFormat(agentScheduleData?.
            charts[openTimeIndex]?.endTime);
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
          this.lunchAgentIcon.startTime = this.formatFilterTimeFormat(agentScheduleData?.
            charts[lunchIndex]?.startTime);
          this.lunchAgentIcon.endTime = this.formatFilterTimeFormat(agentScheduleData?.
            charts[lunchIndex]?.endTime);
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
    this.startTimeFilter = undefined;
    this.endTimeFilter = undefined;
    this.iconCode = undefined;
    this.openTimeAgentIcon = undefined;
    this.lunchAgentIcon = undefined;
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

  onMouseDown(event: any, cell: number, row: number) {
    let object;
    this.isMouseDown = true;
    const time = event.currentTarget.attributes.time.nodeValue;
    const meridiem = event.currentTarget.attributes.meridiem.nodeValue;
    const fromTime = time + ' ' + meridiem;
    const employeeId = event.currentTarget.attributes.employeeId.nodeValue;
    const chart = this.managerCharts.find(x => x.employeeId === +employeeId);
    object = chart?.charts.find(x => this.convertToDateFormat(x.startTime) <= this.convertToDateFormat(fromTime) &&
      this.convertToDateFormat(x.endTime) > this.convertToDateFormat(fromTime));
    if (object) {
      const code = this.schedulingCodes.find(x => x.id === object?.schedulingCodeId);
      this.icon = code?.icon?.value ?? undefined;
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

  getAgentIconDescription(employeeId: number, openTime: string) {
    const chart = this.managerCharts.find(x => x.employeeId === +employeeId);

    if (chart?.charts?.length > 0) {
      const weekTimeData = chart?.charts?.find(x => this.convertToDateFormat(openTime) >= this.convertToDateFormat(x.startTime) &&
        this.convertToDateFormat(openTime) < this.convertToDateFormat(x.endTime));
      if (weekTimeData) {
        const code = this.schedulingCodes.find(x => x.id === weekTimeData?.schedulingCodeId);
        return code?.description;
      }
    }

    return '';
  }

  getIconFromSelectedAgent(employeeId: number, openTime: string) {
    const chart = this.managerCharts.find(x => x.employeeId === +employeeId);

    if (chart?.charts?.length > 0) {
      const weekTimeData = chart?.charts?.find(x => this.convertToDateFormat(openTime) >= this.convertToDateFormat(x.startTime) &&
        this.convertToDateFormat(openTime) < this.convertToDateFormat(x.endTime));
      if (weekTimeData) {
        const code = this.schedulingCodes.find(x => x.id === weekTimeData?.schedulingCodeId);
        return code ? this.unifiedToNative(code?.icon?.value) : '';
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
    this.getModalPopup(ActivityLogsScheduleComponent, 'xl');
    this.modalRef.componentInstance.activityType = ActivityType.SchedulingManagerGrid;
    this.modalRef.componentInstance.employeeId = this.managerCharts[index]?.employeeId;
    this.modalRef.componentInstance.employeeName = this.agentInfo?.lastName + ', ' + this.agentInfo?.firstName;
    this.modalRef.componentInstance.startDate = new Date(this.startDate);
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
    this.managerCharts = JSON.parse(JSON.stringify(this.schedulingMangerChart));
  }

  save() {
    if (this.matchManagerChartDataChanges()) {
      this.formatTime(true);
      const managerChartModel = new UpdateAgentScheduleMangersChart();
      for (const item of this.managerCharts) {
        if (JSON.stringify(item.charts) !== JSON.stringify(this.schedulingMangerChart.find(x => x.id === item.id).charts)) {
          const employeeData = new AgentShceduleMangerData();
          employeeData.employeeId = item?.employeeId;
          managerChartModel.date = this.getFormattedDate(item?.date);
          employeeData.charts = item?.charts;
          managerChartModel.agentScheduleManagers.push(employeeData);
        }
      }
      managerChartModel.activityOrigin = ActivityOrigin.CSS;
      managerChartModel.modifiedUser = +this.authService.getLoggedUserInfo().employeeId;
      managerChartModel.modifiedBy = this.authService.getLoggedUserInfo().displayName;
      managerChartModel.isImport = false;

      this.updateAgentManagerChartSubscription = this.agentScheduleMangerService.updateScheduleManagerChart(managerChartModel)
        .subscribe((response) => {
          this.spinnerService.hide(this.spinner);
          this.schedulingMangerChart = JSON.parse(JSON.stringify(this.managerCharts));
          this.getModalPopup(MessagePopUpComponent, 'sm', 'The record has been updated!');
          this.modalRef.result.then(() => {
            this.loadAgentScheduleManger();
          });
        }, (error) => {
          this.spinnerService.hide(this.spinner);
          console.log(error);
        });

      this.subscriptions.push(this.updateAgentManagerChartSubscription);
    } else {
      this.getModalPopup(MessagePopUpComponent, 'sm', 'No changes has been made!');
    }

  }

  private formatFilterTimeFormat(timeData: string) {
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
    if (JSON.stringify(this.schedulingMangerChart) !== JSON.stringify(this.managerCharts)) {
      return true;
    }
  }

  private loadAgentInfo(employeeId: number) {
    this.spinnerService.show(this.spinner, SpinnerOptions);

    this.getAgentInfoSubscription = this.agentAdminService.getAgentInfo(employeeId)
      .subscribe((response) => {
        this.agentInfo = response;
        this.spinnerService.hide(this.spinner);
      }, (error) => {
        this.spinnerService.hide(this.spinner);
        console.log(error);
      });

    this.subscriptions.push(this.getAgentInfoSubscription);
  }

  private getQueryParams() {
    const agentSchedulesQueryParams = new AgentScheduleManagersQueryParams();
    agentSchedulesQueryParams.agentSchedulingGroupId = this.agentSchedulingGroupId;
    agentSchedulesQueryParams.date = this.getDateInStringFormat(this.startDate);
    agentSchedulesQueryParams.pageNumber = undefined;
    agentSchedulesQueryParams.pageSize = undefined;
    agentSchedulesQueryParams.searchKeyword = this.searchText ?? '';
    agentSchedulesQueryParams.orderBy = `${this.orderBy} ${this.sortBy}`;
    agentSchedulesQueryParams.fields = undefined;
    agentSchedulesQueryParams.employeeId = undefined;
    agentSchedulesQueryParams.skipPageSize = true;

    return agentSchedulesQueryParams;
  }

  private loadAgentScheduleManger() {
    this.clearIconFilters();
    const queryParams = this.getQueryParams();
    this.spinnerService.show(this.spinner, SpinnerOptions);

    this.getAgentSchedulesSubscription = this.agentScheduleMangerService.getAgentScheduleManagers(queryParams)
      .subscribe((response) => {
        this.managerCharts = response.body;
        this.managerCharts.map(x => {
          if (!x.charts || x.charts.length === 0) {
            x.charts = [];
          } else {
            x?.charts?.map(chart => {
              chart.endTime = chart?.endTime?.trim().toLowerCase();
              chart.startTime = chart?.startTime?.trim().toLowerCase();
            });
          }
        });
        this.formatTime(false);
        this.schedulingMangerChart = JSON.parse(JSON.stringify(this.managerCharts));
        const employeeId = this.managerCharts[0]?.employeeId;
        const agentScheduleChart = this.managerCharts[0]?.charts?.length > 0 ? this.managerCharts[0].charts[0] : undefined;
        if (agentScheduleChart) {
          this.setIconFilters(employeeId);
        }
        this.setAgent(employeeId, 0);
        this.spinnerService.hide(this.spinner);
      }, (error) => {
        this.spinnerService.hide(this.spinner);
        console.log(error);
      });

    this.subscriptions.push(this.getAgentSchedulesSubscription);
  }

  private formatTime(updateChart: boolean) {
    for (const item of this.managerCharts) {
      if (!updateChart && item?.charts?.length > 0) {
        const index = item?.charts.findIndex(x => x?.endTime?.trim().toLowerCase() === '00:00 am' ||
          x.endTime.trim().toLowerCase() === '12:00 am');
        if (index > -1) {
          item.charts[index].endTime = '11:60 pm';
        }
        item?.charts.map(x => {
          if (x?.endTime?.trim().toLowerCase().slice(0, 2) === '12') {
            x.endTime = '00' + x?.endTime?.trim().toLowerCase().slice(2, 8);
          }
          if (x?.startTime?.trim().toLowerCase().slice(0, 2) === '12') {
            x.startTime = '00' + x?.startTime?.trim().toLowerCase().slice(2, 8);
          }
        });
        if (item.charts.length === 0) {
          item.charts = [];
        }
      } else if (updateChart && item.charts.length > 0) {
        const index = item?.charts.findIndex(x => x?.endTime?.trim().toLowerCase() === '11:60 pm');
        if (index > -1) {
          item.charts[index].endTime = '12:00 am';
        }
        item?.charts.map(x => {
          if (x?.endTime?.trim().toLowerCase().slice(0, 2) === '00') {
            x.endTime = '12' + x?.endTime?.trim().toLowerCase().slice(2, 8);
          }
          if (x?.startTime?.trim().toLowerCase().slice(0, 2) === '00') {
            x.startTime = '12' + x?.startTime?.trim().toLowerCase().slice(2, 8);
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
      let to;
      this.spinnerService.show(this.spinner, SpinnerOptions);
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

      const code = this.schedulingCodes.find(x => x?.icon?.value?.trim().toLowerCase() === this.icon?.trim().toLowerCase());
      const iconModel = new ScheduleChart(fromTime, to, code?.id);

      const date = new Date(this.startDate);

      employeeId = elem?.attributes?.employeeId?.value;
      const chart = this.managerCharts.find(x => x.employeeId === +employeeId);
      chartArray = chart?.charts?.length > 0 ? chart.charts : chart.charts = [];
      chart.date = date;

      if (this.icon && !this.isDelete) {

        if (chartArray?.length > 0) {
          this.insertIconToGrid(chartArray, iconModel);
        } else {
          // const weekDay = new AgentScheduleManagerChart();
          const calendarTime = new ScheduleChart(fromTime, to, code.id);
          // weekDay.charts.push(calendarTime);
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
    this.sortSelectedGridCalendarTimes();
    this.formatTimeValuesInSchedulingGrid();
    this.spinnerService.hide(this.spinner);
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

  private insertIconToGrid(charts: ScheduleChart[], insertIcon: ScheduleChart) {
    if (charts.find(x => x.startTime === insertIcon.startTime && x.endTime === insertIcon.endTime)) {
      const item = charts.find(x => x.startTime === insertIcon.startTime && x.endTime === insertIcon.endTime);
      item.schedulingCodeId = insertIcon.schedulingCodeId;
    } else if (charts.filter(x => this.convertToDateFormat(x.startTime) >= this.convertToDateFormat(insertIcon.startTime) &&
      this.convertToDateFormat(x.endTime) <= this.convertToDateFormat(insertIcon.endTime)).length > 0) {
      const timeDataArray = charts.filter(x => this.convertToDateFormat(x.startTime) >=
        this.convertToDateFormat(insertIcon.startTime) &&
        this.convertToDateFormat(x.endTime) <= this.convertToDateFormat(insertIcon.endTime));
      timeDataArray.forEach(ele => {
        const startIndex = charts.findIndex(x =>
          this.convertToDateFormat(x.startTime) === this.convertToDateFormat(ele.startTime));
        if (startIndex > -1) {
          charts.splice(startIndex, 1);
        }
      });
      if (timeDataArray.length > 0) {
        const calendarTime = new ScheduleChart(insertIcon.startTime, insertIcon.endTime, insertIcon.schedulingCodeId);
        charts.push(calendarTime);
      }
      this.sortSelectedGridCalendarTimes();
      this.formatTimeValuesInSchedulingGrid();
    }
    if (!charts.find(x => x.startTime === insertIcon.startTime && x.endTime ===
      insertIcon.endTime && x.schedulingCodeId === insertIcon.schedulingCodeId)) {
      charts.forEach(ele => {
        if (this.convertToDateFormat(ele.startTime) < this.convertToDateFormat(insertIcon.startTime) &&
          this.convertToDateFormat(ele.endTime) === this.convertToDateFormat(insertIcon.startTime)) {
          ele.endTime = insertIcon.startTime;
        } else if (this.convertToDateFormat(ele.startTime) === this.convertToDateFormat(insertIcon.startTime) &&
          this.convertToDateFormat(ele.endTime) < this.convertToDateFormat(insertIcon.endTime)) {
          ele.endTime = insertIcon.endTime;
        } else if (this.convertToDateFormat(ele.startTime) > this.convertToDateFormat(insertIcon.startTime) &&
          this.convertToDateFormat(ele.endTime) <= this.convertToDateFormat(insertIcon.endTime)) {
          ele.startTime = insertIcon.endTime;
        } else if (this.convertToDateFormat(ele.startTime) === this.convertToDateFormat(insertIcon.startTime) &&
          this.convertToDateFormat(ele.endTime) > this.convertToDateFormat(insertIcon.endTime)) {
          ele.startTime = insertIcon.endTime;
        } else if (this.convertToDateFormat(ele.startTime) < this.convertToDateFormat(insertIcon.startTime) &&
          this.convertToDateFormat(insertIcon.endTime) < this.convertToDateFormat(ele.endTime)) {
          const calendarTime = new ScheduleChart(insertIcon.endTime, ele.endTime, ele.schedulingCodeId);
          charts.push(calendarTime);
          ele.endTime = insertIcon.startTime;
        } else if (this.convertToDateFormat(ele.endTime) === this.convertToDateFormat(insertIcon.endTime) &&
          this.convertToDateFormat(ele.startTime) < this.convertToDateFormat(insertIcon.startTime)) {
          ele.endTime = insertIcon.startTime;
        } else if (this.convertToDateFormat(ele.startTime) > this.convertToDateFormat(insertIcon.startTime) &&
          this.convertToDateFormat(ele.endTime) > this.convertToDateFormat(insertIcon.endTime)) {
          // const calendarTime = new ScheduleChart(insertIcon.startTime, insertIcon.endTime, insertIcon.schedulingCodeId);
          // weekData.charts.push(calendarTime);
          // ele.startTime = insertIcon.endTime;
        }
      });
      const timeDataArray = charts.filter(x => this.convertToDateFormat(x.startTime) >=
        this.convertToDateFormat(insertIcon.startTime) &&
        this.convertToDateFormat(x.endTime) <= this.convertToDateFormat(insertIcon.endTime));
      if (timeDataArray.length > 0) {
        timeDataArray.forEach(ele => {
          ele.schedulingCodeId = insertIcon.schedulingCodeId;
        });
      } else {
        const calendarTime = new ScheduleChart(insertIcon.startTime, insertIcon.endTime, insertIcon.schedulingCodeId);
        charts.push(calendarTime);
      }
    }
  }

  private clearIconFromGrid(charts: ScheduleChart[], icon: ScheduleChart) {
    if (charts.findIndex(x => x.startTime === icon.startTime && x.endTime === icon.endTime) > -1) {
      const startIndex = charts.findIndex(x => x.startTime === icon.startTime && x.endTime === icon.endTime);
      charts.splice(startIndex, 1);
    } else if (charts.filter(x => this.convertToDateFormat(x.startTime) >= this.convertToDateFormat(icon.startTime) &&
      this.convertToDateFormat(x.endTime) <= this.convertToDateFormat(icon.endTime)).length > 0) {
      const timeDataArray = charts.filter(x => this.convertToDateFormat(x.startTime) >= this.convertToDateFormat(icon.startTime) &&
        this.convertToDateFormat(x.endTime) <= this.convertToDateFormat(icon.endTime));
      timeDataArray.forEach(ele => {
        const startIndex = charts.findIndex(x =>
          this.convertToDateFormat(x.startTime) === this.convertToDateFormat(ele.startTime));
        if (startIndex > -1) {
          charts.splice(startIndex, 1);
        }
      });
    }
    if (charts.findIndex(x => x.startTime === icon.startTime && x.endTime === icon.endTime) < 0) {
      charts.forEach(ele => {
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
          charts.push(calendarTime);
          ele.endTime = icon.startTime;
        } else if (this.convertToDateFormat(ele.endTime) === this.convertToDateFormat(icon.endTime) &&
          this.convertToDateFormat(ele.startTime) < this.convertToDateFormat(icon.startTime)) {
          ele.endTime = icon.startTime;
        } else if (this.convertToDateFormat(ele.startTime) > this.convertToDateFormat(icon.startTime) &&
          this.convertToDateFormat(ele.endTime) > this.convertToDateFormat(icon.endTime)) {
          // ele.startTime = icon.endTime;
        } else if (this.convertToDateFormat(ele.endTime) > this.convertToDateFormat(icon.startTime) &&
          this.convertToDateFormat(ele.startTime) <= this.convertToDateFormat(icon.startTime)) {
          // ele.endTime = icon.startTime;
          // weekData.charts.find(x => x.startTime || x.endTime)
        }
      });
      const timeDataArray = charts.filter(x => this.convertToDateFormat(x.startTime) >= this.convertToDateFormat(icon.startTime) &&
        this.convertToDateFormat(x.endTime) <= this.convertToDateFormat(icon.endTime));
      if (timeDataArray.length > 0) {
        timeDataArray.forEach(ele => {
          const startIndex = charts.findIndex(x =>
            this.convertToDateFormat(x.startTime) === this.convertToDateFormat(ele.startTime));
          if (startIndex > -1) {
            charts.splice(startIndex, 1);
          }
        });
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
}
