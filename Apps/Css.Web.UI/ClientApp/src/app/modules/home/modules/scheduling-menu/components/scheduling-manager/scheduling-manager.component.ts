import { CdkDragDrop } from '@angular/cdk/drag-drop';
import { Component, Input, OnChanges, OnDestroy, OnInit } from '@angular/core';
import { NgbModal, NgbModalOptions, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { from, SubscriptionLike as ISubscription } from 'rxjs';
import { mergeMap } from 'rxjs/operators';
import { NgxSpinnerService } from 'ngx-spinner';
import { WeekDay } from '@angular/common';

import { Constants } from 'src/app/shared/util/constants.util';
import { SpinnerOptions } from 'src/app/shared/util/spinner-options.util';
import { SortingType } from '../../enums/sorting-type.enum';
import { AgentScheduleType } from '../../enums/agent-schedule-type.enum';

import { AgentSchedulesQueryParams } from '../../models/agent-schedules-query-params.model';
import { AgentSchedulesResponse } from '../../models/agent-schedules-response.model';
import { AgentChartResponse } from '../../models/agent-chart-response.model';
import { SchedulingCode } from '../../../system-admin/models/scheduling-code.model';
import { ScheduleChart } from '../../models/schedule-chart.model';
import { AgentScheduleManagerChart } from '../../models/agent-schedule-manager-chart.model';
import { AgentInfo } from '../../models/agent-info.model';
import { UpdateAgentScheduleMangersChart } from '../../models/update-agent-schedule-managers-chart.model';
import { AgentShceduleMangerData } from '../../models/agent-schedule-manager-data.model';
import { ScheduleChartQueryParams } from '../../models/schedule-chart-query-params.model';

import { AgentSchedulesService } from '../../services/agent-schedules.service';
import { AgentAdminService } from '../../services/agent-admin.service';
import { AuthService } from 'src/app/core/services/auth.service';

import { CopyScheduleComponent } from '../shared/copy-schedule/copy-schedule.component';
import { MessagePopUpComponent } from 'src/app/shared/popups/message-pop-up/message-pop-up.component';
import { ActivityLogsScheuldeComponent } from '../shared/activity-logs-schedule/activity-logs-schedule.component';

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


@Component({
  selector: 'app-scheduling-manager',
  templateUrl: './scheduling-manager.component.html',
  styleUrls: ['./scheduling-manager.component.scss']
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
  totalSchedulingGridData: AgentSchedulesResponse[] = [];
  weekDays: Array<string> = [];
  managerCharts: AgentChartResponse[] = [];
  schedulingMangerChart: AgentChartResponse[] = [];
  employeeChartData: AgentChartResponse[] = [];

  getSchedulingCodesSubscription: ISubscription;
  getTranslationSubscription: ISubscription;
  updateAgentManagerChartSubscription: ISubscription;
  getAgentInfoSubscription: ISubscription;
  getAgentSchedulesSubscription: ISubscription;
  subscriptions: ISubscription[] = [];

  constructor(
    private agentSchedulesService: AgentSchedulesService,
    private spinnerService: NgxSpinnerService,
    private agentAdminService: AgentAdminService,
    private authService: AuthService,
    private modalService: NgbModal,
    private excelService: ExcelService,
    private schedulingCodeService: SchedulingCodeService,
    private route: ActivatedRoute,
    private languagePreferenceService: LanguagePreferenceService,
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

  onSchedulingGroupChange(schedulingGroupId: number) {
    this.agentSchedulingGroupId = schedulingGroupId;
    if (this.agentSchedulingGroupId) {
      this.loadAgentScheduleManger();
    } else {
      this.clearIconFilters();
      this.totalSchedulingGridData = [];
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

  setAgent(id: string, index: number) {
    this.setIconFilters(id);
    this.setAgentIconFilters(id);
    this.selectedRow = index;
    const employeeId = this.totalSchedulingGridData[index]?.employeeId;
    if (employeeId) {
      this.loadAgentInfo(employeeId);
    }
  }

  setIconFilters(id: string) {
    const agent = this.schedulingMangerChart.find(x => x.id === id)?.agentScheduleManagerCharts[0]?.charts[0];
    if (agent) {
      const schedulingCode = this.schedulingCodes.find(x => x.id === agent?.schedulingCodeId);
      this.iconDescription = schedulingCode?.description;
      this.startTimeFilter = agent?.startTime;
      this.endTimeFilter = agent?.endTime;
      this.iconCode = schedulingCode?.icon?.value;
    } else {
      this.clearIconFilters();
    }
  }

  setAgentIconFilters(id: string) {
    const openTime = this.schedulingCodes?.find(x => x.description.trim().toLowerCase() === 'open time');
    const lunch = this.schedulingCodes?.find(x => x.description.trim().toLowerCase() === 'lunch');
    const agentScheduleData = this.schedulingMangerChart.find(x => x.id === id);
    if (agentScheduleData) {
      if (openTime) {
        const openTimeIndex = this.schedulingMangerChart.find(x => x.id === id)?.agentScheduleManagerCharts[0]?.charts
          .findIndex(x => x.schedulingCodeId === openTime?.id);
        if (openTimeIndex > -1) {
          this.openTimeAgentIcon = new AgentIconFilter();
          this.openTimeAgentIcon.codeValue = openTime?.icon?.value;
          this.openTimeAgentIcon.startTime = agentScheduleData?.
            agentScheduleManagerCharts[0]?.charts[openTimeIndex]?.startTime;
          this.openTimeAgentIcon.endTime = agentScheduleData?.
            agentScheduleManagerCharts[0]?.charts[openTimeIndex]?.endTime;
        } else {
          this.openTimeAgentIcon = undefined;
        }
      }
      if (lunch) {
        const lunchIndex = this.schedulingMangerChart.find(x => x.id === id)?.agentScheduleManagerCharts[0]?.charts
          .findIndex(x => x.schedulingCodeId === lunch?.id);
        if (lunchIndex > -1) {
          this.lunchAgentIcon = new AgentIconFilter();
          this.lunchAgentIcon.codeValue = lunch?.icon?.value;
          this.lunchAgentIcon.startTime = agentScheduleData?.
            agentScheduleManagerCharts[0]?.charts[lunchIndex]?.startTime;
          this.lunchAgentIcon.endTime = agentScheduleData?.
            agentScheduleManagerCharts[0]?.charts[lunchIndex]?.endTime;
        } else {
          this.lunchAgentIcon = undefined;
        }
      }
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

  getAgentIconDescription(scheduleId: string, openTime: string) {
    const chart = this.managerCharts.find(x => x.id === scheduleId);

    if (chart?.agentScheduleManagerCharts?.length > 0) {
      for (const item of chart.agentScheduleManagerCharts) {
        const weekTimeData = item?.charts?.find(x => this.convertToDateFormat(openTime) >= this.convertToDateFormat(x.startTime) &&
          this.convertToDateFormat(openTime) < this.convertToDateFormat(x.endTime));
        if (weekTimeData) {
          const code = this.schedulingCodes.find(x => x.id === weekTimeData?.schedulingCodeId);
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
        const weekTimeData = item?.charts?.find(x => this.convertToDateFormat(openTime) >= this.convertToDateFormat(x.startTime) &&
          this.convertToDateFormat(openTime) < this.convertToDateFormat(x.endTime));
        if (weekTimeData) {
          const code = this.schedulingCodes.find(x => x.id === weekTimeData?.schedulingCodeId);
          return code ? this.unifiedToNative(code?.icon?.value) : '';
        }
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
    this.getModalPopup(ActivityLogsScheuldeComponent, 'xl');
    this.modalRef.componentInstance.activityType = ActivityType.SchedulingManagerGrid;
    this.modalRef.componentInstance.employeeId = this.totalSchedulingGridData[index].employeeId;
    this.modalRef.componentInstance.employeeName = this.agentInfo.lastName + ' ' + this.agentInfo.firstName;
    this.modalRef.componentInstance.startDate = new Date(this.startDate);
  }

  openCopySchedule(index: number) {
    const agentScheduleId = this.totalSchedulingGridData[index]?.id;
    const employeeId = this.totalSchedulingGridData[index]?.employeeId;
    this.getModalPopup(CopyScheduleComponent, 'lg');
    this.modalRef.componentInstance.agentSchedulingGroupId = this.agentSchedulingGroupId;
    this.modalRef.componentInstance.agentScheduleId = agentScheduleId;
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
      const managerChartModel = new UpdateAgentScheduleMangersChart();
      for (const item of this.employeeChartData) {
        if (item.agentScheduleManagerCharts.length > 0) {
          const employeeData = new AgentShceduleMangerData();
          employeeData.employeeId = this.totalSchedulingGridData.find(x => x.id === item?.id)?.employeeId;
          const index = item?.agentScheduleManagerCharts[0]?.charts.findIndex(x => x.endTime === '11:60 pm');
          if (index > -1) {
            item.agentScheduleManagerCharts[0].charts[index].endTime = '00:00 am';
          }
          const nullValueIndex = item?.agentScheduleManagerCharts[0]?.charts.findIndex(x => x.schedulingCodeId === null);
          if (nullValueIndex > -1) {
            item.agentScheduleManagerCharts[0].charts.splice(nullValueIndex, 1);
          }
          employeeData.agentScheduleManagerChart = item.agentScheduleManagerCharts[0];
          managerChartModel.agentScheduleManagers.push(employeeData);
        }
      }
      managerChartModel.activityOrigin = ActivityOrigin.CSS;
      managerChartModel.modifiedUser = +this.authService.getLoggedUserInfo().employeeId;
      managerChartModel.modifiedBy = this.authService.getLoggedUserInfo().displayName;

      this.updateAgentManagerChartSubscription = this.agentSchedulesService.updateScheduleManagerChart(managerChartModel)
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
    const agentSchedulesQueryParams = new AgentSchedulesQueryParams();
    agentSchedulesQueryParams.agentSchedulingGroupId = this.agentSchedulingGroupId;
    agentSchedulesQueryParams.fromDate = this.getDateInStringFormat(this.startDate);
    agentSchedulesQueryParams.pageNumber = undefined;
    agentSchedulesQueryParams.pageSize = undefined;
    agentSchedulesQueryParams.searchKeyword = this.searchText ?? '';
    agentSchedulesQueryParams.orderBy = `${this.orderBy} ${this.sortBy}`;
    agentSchedulesQueryParams.fields = 'id,employeeId,firstName,lastName';
    agentSchedulesQueryParams.employeeIds = undefined;
    agentSchedulesQueryParams.skipPageSize = true;

    return agentSchedulesQueryParams;
  }

  private loadAgentScheduleManger() {
    this.clearIconFilters();
    const queryParams = this.getQueryParams();
    this.spinnerService.show(this.spinner, SpinnerOptions);

    this.getAgentSchedulesSubscription = this.agentSchedulesService.getAgentSchedules(queryParams)
      .pipe(mergeMap(data => {
        this.spinnerService.hide(this.spinner);
        this.totalSchedulingGridData = data.body;
        this.totalSchedulingRecord = this.totalSchedulingGridData.length;
        this.managerCharts = [];
        const ids = Array<string>();
        this.totalSchedulingGridData.forEach(element => {
          ids.push(element.id);
        });
        return this.getAgentCharts(ids);
      }))
      .subscribe((response) => {
        this.spinnerService.show(this.spinner, SpinnerOptions);
        if (response) {
          const index = response?.agentScheduleManagerCharts[0]?.charts.findIndex(x => x.endTime.trim().toLowerCase() === '00:00 am');
          if (index > -1) {
            response.agentScheduleManagerCharts[0].charts[index].endTime = '11:60 pm';
          }
          const scheduleIndex = response?.agentScheduleCharts?.charts.findIndex(x => x.endTime.trim().toLowerCase() === '00:00 am');
          if (scheduleIndex > -1) {
            response.agentScheduleManagerCharts[0].charts[scheduleIndex].endTime = '11:60 pm';
          }
        }
        if (response?.agentScheduleManagerCharts[0]?.charts.length === 0) {
          response.agentScheduleManagerCharts = [];
          this.managerCharts.push(response);
        } else {
          this.managerCharts.push(response);
        }
        this.spinnerService.hide(this.spinner);
      }, (error) => {
        this.spinnerService.hide(this.spinner);
        console.log(error);
      }, () => {
        this.managerCharts.map(x => x?.agentScheduleManagerCharts[0]?.charts.map(chart => {
          chart.endTime = chart?.endTime.trim().toLowerCase();
          chart.startTime = chart?.startTime.trim().toLowerCase();
        }));
        this.employeeChartData = JSON.parse(JSON.stringify(this.managerCharts));
        this.setManagerGridData();
        this.schedulingMangerChart = JSON.parse(JSON.stringify(this.managerCharts));
        const agentScheduleId = this.totalSchedulingGridData[0]?.id;
        const agentScheduleChart = this.managerCharts.find(x => x.id === agentScheduleId)?.agentScheduleManagerCharts[0]?.charts[0];
        if (agentScheduleChart) {
          this.setIconFilters(agentScheduleId);
        }
        this.setAgent(agentScheduleId, 0);
      });

    this.subscriptions.push(this.getAgentSchedulesSubscription);
  }

  private setManagerGridData() {
    this.managerCharts.forEach(x => {
      if (x.agentScheduleChart && x.agentScheduleManagerCharts.length > 0) {
        const managerChart = x?.agentScheduleManagerCharts[0]?.charts;
        x.agentScheduleManagerCharts[0].charts = x?.agentScheduleChart?.charts;
        managerChart.forEach(y => {
          const iconModel = new ScheduleChart(y?.startTime, y?.endTime, y?.schedulingCodeId);
          const weekDays = x?.agentScheduleManagerCharts;
          const weekData = weekDays?.length > 0 ? weekDays[0] : undefined;
          if (weekData) {
            this.insertIconToGrid(weekData, iconModel, true);
          } else {
            const weekDay = new AgentScheduleManagerChart();
            weekDay.date = new Date(this.startDate);
            const calendarTime = new ScheduleChart(y?.startTime, y?.endTime, y?.schedulingCodeId);
            weekDay.charts.push(calendarTime);
            weekDays.push(weekDay);
          }
        });
      } else if (x.agentScheduleChart && x.agentScheduleChart.charts.length > 0) {
        const agentScheduleManagerChart = new AgentScheduleManagerChart();
        agentScheduleManagerChart.charts = x?.agentScheduleChart?.charts;
        agentScheduleManagerChart.date = new Date(this.startDate);
        x.agentScheduleManagerCharts.push(agentScheduleManagerChart);
      }
    });

  }

  private getAgentCharts(ids: string[]) {
    const queryParams = new ScheduleChartQueryParams();
    queryParams.date = this.getDateInStringFormat(this.startDate);
    queryParams.day = new Date(this.startDate).getDay();
    queryParams.agentScheduleType = AgentScheduleType.SchedulingManager;
    return from(ids).pipe(
      mergeMap(id => this.agentSchedulesService.getCharts(id.toString(), queryParams)
      ));
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
      let weekData = new AgentScheduleManagerChart();
      let scheduleId;
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

      scheduleId = elem?.attributes?.scheduleId?.value;
      const chart = this.managerCharts.find(x => x.id === scheduleId);
      weekDays = chart?.agentScheduleManagerCharts;
      weekData = weekDays?.length > 0 ? weekDays[0] : undefined;

      const employeeChart = this.employeeChartData.find(x => x.id === scheduleId);
      const employeeWeekDays = employeeChart?.agentScheduleManagerCharts;
      const employeeWeekData = employeeWeekDays?.length > 0 ? employeeWeekDays[0] : undefined;

      if (this.icon && !this.isDelete) {

        if (weekData) {
          this.insertIconToGrid(weekData, iconModel, true);
        } else {
          const weekDay = new AgentScheduleManagerChart();
          weekDay.date = date;
          const calendarTime = new ScheduleChart(fromTime, to, code.id);
          weekDay.charts.push(calendarTime);
          weekDays.push(weekDay);
        }
        if (employeeWeekData) {
          this.insertIconToGrid(employeeWeekData, iconModel, false);
        } else {
          const weekDay = new AgentScheduleManagerChart();
          weekDay.date = date;
          const calendarTime = new ScheduleChart(fromTime, to, code.id);
          weekDay.charts.push(calendarTime);
          employeeWeekDays.push(weekDay);
        }
      } else if (this.isDelete) {
        if (weekData) {
          this.clearIconFromGrid(weekData, iconModel);
          if (weekData.charts.length === 0) {
            weekData.charts = [];
          }
        }
        if (employeeWeekData) {
          this.clearIconFromGrid(employeeWeekData, iconModel);
          if (employeeWeekData.charts.length === 0) {
            employeeWeekData.charts = [];
          }
        }
      }
    });
    this.sortSelectedGridCalendarTimes(this.managerCharts);
    this.formatTimeValuesInSchedulingGrid(this.managerCharts);
    this.sortSelectedGridCalendarTimes(this.employeeChartData);
    this.formatTimeValuesInSchedulingGrid(this.employeeChartData);
    this.spinnerService.hide(this.spinner);
    this.isDelete = false;
    this.icon = undefined;

    table.find('.' + this.selectedCellClassName).removeClass(this.selectedCellClassName);
  }

  private formatTimeValuesInSchedulingGrid(chartArray: AgentChartResponse[]) {
    for (const item of chartArray) {
    const weekDays = item.agentScheduleManagerCharts;
    weekDays.forEach((element) => {
      element.charts = this.adjustSchedulingCalendarTimesRange(element.charts);
    });
  }
  }

  private sortSelectedGridCalendarTimes(chartArray: AgentChartResponse[]) {
    for (const item of chartArray) {
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

  private insertIconToGrid(weekData: AgentScheduleManagerChart, insertIcon: ScheduleChart, managerData: boolean) {
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
      if (managerData) {
        this.sortSelectedGridCalendarTimes(this.managerCharts);
        this.formatTimeValuesInSchedulingGrid(this.managerCharts);
      } else {
        this.sortSelectedGridCalendarTimes(this.employeeChartData);
        this.formatTimeValuesInSchedulingGrid(this.employeeChartData);
      }

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

  private clearIconFromGrid(weekData: AgentScheduleManagerChart, icon: ScheduleChart) {
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

}
