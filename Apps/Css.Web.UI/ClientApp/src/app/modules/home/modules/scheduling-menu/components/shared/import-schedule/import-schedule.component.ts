import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { NgbActiveModal, NgbModal, NgbModalOptions } from '@ng-bootstrap/ng-bootstrap';
import { ExcelData } from '../../../models/excel-data.model';
import { SchedulingCodeQueryParams } from '../../../../system-admin/models/scheduling-code-query-params.model';
import { NgxSpinnerService } from 'ngx-spinner';
import { SpinnerOptions } from 'src/app/shared/util/spinner-options.util';
import { forkJoin, SubscriptionLike as ISubscription } from 'rxjs';
import { SchedulingCodeService } from 'src/app/shared/services/scheduling-code.service';
import { SchedulingCode } from '../../../../system-admin/models/scheduling-code.model';
import { AgentSchedulesService } from '../../../services/agent-schedules.service';
import { AgentScheduleChart } from '../../../models/agent-schedule-chart.model';
import { AgentScheduleType } from '../../../enums/agent-schedule-type.enum';
import { ScheduleChart } from '../../../models/schedule-chart.model';
import { AuthService } from 'src/app/core/services/auth.service';
import { ErrorWarningPopUpComponent } from 'src/app/shared/popups/error-warning-pop-up/error-warning-pop-up.component';
import { ContentType } from 'src/app/shared/enums/content-type.enum';
import { Papa } from 'ngx-papaparse';
import { AgentSchedulesQueryParams } from '../../../models/agent-schedules-query-params.model';
import { AgentSchedulesResponse } from '../../../models/agent-schedules-response.model';
import { ImportShceduleChart } from '../../../models/import-schedule-chart.model';
import { UpdateAgentScheduleMangersChart } from '../../../models/update-agent-schedule-managers-chart.model';
import { ImportScheduleData } from '../../../models/import-schedule-data.model';
import { AgentShceduleMangerData } from '../../../models/agent-schedule-manager-data.model';
import { ManagerExcelData } from '../../../models/manager-excel-data.model';
import { ActivityOrigin } from '../../../enums/activity-origin.enum';
import { AgentScheduleManagersService } from '../../../services/agent-schedule-managers.service';
import { DatePipe } from '@angular/common';
import { ImportAgentScheduleRanges } from '../../../models/import-agent-schedule-ranges.model';
import { ScheduleDateRangeBase } from '../../../models/schedule-date-range-base.model';
import { DropListRef } from '@angular/cdk/drag-drop';
import { AgentScheduleManagersQueryParams } from '../../../models/agent-schedule-mangers-query-params.model';
import { AgentScheduleManagerChart } from '../../../models/agent-schedule-manager-chart.model';
import { stringify } from '@angular/compiler/src/util';
import { trimTrailingNulls } from '@angular/compiler/src/render3/view/util';

@Component({
  selector: 'app-import-schedule',
  templateUrl: './import-schedule.component.html',
  styleUrls: ['./import-schedule.component.scss'],
  providers: [DatePipe]
})
export class ImportScheduleComponent implements OnInit, OnDestroy {

  uploadFile: string;
  spinner = 'import';
  fileUploaded: File;
  fileFormatValidation: boolean;
  fileSubmitted: boolean;
  jsonData: any[] = [];
  scheduleColumns = ['EmployeeId', 'StartDate', 'EndDate', 'ActivityCode', 'StartTime', 'EndTime'];
  schedulingManagerColumns = ['EmployeeId', 'Date', 'ActivityCode', 'StartTime', 'EndTime'];
  csvTableHeader: string[];

  importAgentScheduleChartSubscription: ISubscription;
  updateManagerChartSubscription: ISubscription;
  subscriptions: ISubscription[] = [];

  @Input() agentScheduleType: AgentScheduleType;
  @Input() agentSchedulingGroupId: number;

  constructor(
    public activeModal: NgbActiveModal,
    private spinnerService: NgxSpinnerService,
    private schedulingCodeService: SchedulingCodeService,
    private agentSchedulesService: AgentSchedulesService,
    private agentScheduleManagerService: AgentScheduleManagersService,
    private authService: AuthService,
    private modalService: NgbModal,
    private datepipe: DatePipe,
    private papa: Papa
  ) { }

  ngOnInit(): void {
  }

  ngOnDestroy() {
    this.subscriptions.forEach(subscription => {
      if (subscription) {
        subscription.unsubscribe();
      }
    });
  }

  import() {
    this.fileSubmitted = true;
    if (this.uploadFile && this.jsonData.length > 0) {
      this.jsonData.map(ele => {
        if (ele.StartTime.split(':')[0].length === 1) {
          ele.StartTime = '0' + ele.StartTime.split(':')[0] + ':' + ele.StartTime.split(':')[1];
        }
        if (ele.StartTime.split(':')[0] === '12') {
          ele.StartTime = '00' + ':' + ele.StartTime.split(':')[1];
        }
        if (ele.EndTime.split(':')[0].length === 1) {
          ele.EndTime = '0' + ele.EndTime.split(':')[0] + ':' + ele.EndTime.split(':')[1];
        }
        if (ele.EndTime.split(':')[0] === '12') {
          ele.EndTime = '00' + ':' + ele.EndTime.split(':')[1];
        }
        if (ele.EndTime.trim().toLowerCase() === '12:00 am' || ele.EndTime.trim().toLowerCase() === '00:00 am') {
          ele.EndTime = '11:60 pm';
        }
      });
      if (!this.fileFormatValidation && !this.validateHeading()) {
        const employees = new Array<number>();
        this.jsonData.forEach(data => {
          if (employees.filter(x => x === +data.EmployeeId).length === 0) {
            employees.push(+data.EmployeeId);
          }
        });
        this.loadAgentSchedules(employees);
      } else {
        const errorMessage = `“An error occurred upon importing the file. Please check the following”<br>Duplicated Record<br>Incorrect Columns<br>Invalid Date Range and Time<br>Not recognized Employee ID`;
        this.showErrorWarningPopUpMessage(errorMessage);
      }
    } else {
      const errorMessage = `“An error occurred upon importing the file. Please check the following”<br>Duplicated Record<br>Incorrect Columns<br>Invalid Date Range and Time<br>Not recognized Employee ID`;
      this.showErrorWarningPopUpMessage(errorMessage);
    }
  }

  hasFileSelected() {
    if (this.fileSubmitted) {
      return this.uploadFile ? true : false;
    }
    return true;
  }

  browse(files: any) {
    this.jsonData = [];
    const exportFileName = 'Attendance_scheduling';
    this.fileUploaded = files[0];
    this.uploadFile = this.fileUploaded?.name;
    if (this.uploadFile.split('.')[1].toLowerCase() === 'csv'.toLowerCase()) {
      this.readCsvFile();
    } else {
      this.fileFormatValidation = true;
    }
  }

  private formatTimeFormat(importRecord: any[]) {
    for (const item of importRecord) {
      if (this.agentScheduleType === AgentScheduleType.Scheduling) {
        for (const record of item.ranges) {
          for (const chartItem of record.agentScheduleCharts) {
            chartItem.charts.map(x => {
              if (x?.endTime?.trim()?.toLowerCase()?.slice(0, 2) === '00') {
                x.endTime = '12' + x?.endTime?.trim()?.toLowerCase()?.slice(2, 8);
              }
              if (x?.startTime?.trim()?.toLowerCase()?.slice(0, 2) === '00') {
                x.startTime = '12' + x?.startTime?.trim()?.toLowerCase()?.slice(2, 8);
              }
              if (x?.endTime === '11:60 pm') {
                x.endTime = '12:00 am';
              }
            });
          }
        }
      } else {
        for (const el of item.agentScheduleManagerCharts) {
          const chartData = el.charts;
          chartData.map(x => {
            if (x?.endTime?.trim().toLowerCase().slice(0, 2) === '00') {
              x.endTime = '12' + x?.endTime?.trim().toLowerCase().slice(2, 8);
            }
            if (x?.startTime?.trim().toLowerCase().slice(0, 2) === '00') {
              x.startTime = '12' + x?.startTime?.trim().toLowerCase().slice(2, 8);
            }
            if (x?.endTime === '11:60 pm') {
              x.endTime = '12:00 am';
            }
          });
        }
      }
    }
  }

  private validateHeading() {
    for (const item of this.csvTableHeader) {
      if (this.agentScheduleType === AgentScheduleType.Scheduling &&
        this.scheduleColumns.findIndex(x => x.trim().toLowerCase() === item.trim().toLowerCase()) === -1) {
        return true;
      }
      if (this.agentScheduleType === AgentScheduleType.SchedulingManager &&
        this.schedulingManagerColumns.findIndex(x => x.trim().toLowerCase() === item.trim().toLowerCase()) === -1) {
        return true;
      }
    }
  }

  private readCsvFile() {
    const reader: FileReader = new FileReader();
    reader.readAsText(this.fileUploaded);
    reader.onload = e => {
      const csv = reader.result;
      const results = this.papa.parse(csv as string, { header: false });
      if (results?.data !== undefined && results?.data.length > 0 && results?.errors.length === 0) {
        this.csvTableHeader = results.data[0];
        const csvTableData = [...results.data.slice(1, results.data.length)];
        for (const ele of csvTableData) {
          const csvJson = this.agentScheduleType === AgentScheduleType.Scheduling ?
            new ExcelData() : new ManagerExcelData();
          if (ele.length > 0) {
            for (let i = 0; i < ele.length; i++) {
              csvJson[this.csvTableHeader[i]] = ele[i];
            }
          }
          if (csvJson.EmployeeId) {
            this.jsonData.push(csvJson);
          }
        }
        this.jsonData.map(x => {
          x.StartDate = x?.StartTime.trim();
          x.EndDate = x?.EndDate.trim();
          x.StartDate = x?.StartDate.slice(0, 4) +"/"+ x?.StartDate.slice(4, 6) +"/"+ x?.StartDate.slice(6, 8);
          x.EndDate = x?.EndDate.slice(0, 4) +"/"+ x?.EndDate.slice(4, 6) +"/"+ x?.EndDate.slice(6, 8);
          x.StartTime = x?.StartTime.trim().toLowerCase();
          x.EndTime = x?.EndTime.trim().toLowerCase();
          x.ActivityCode = x?.ActivityCode.trim().toLowerCase();
        });
      }
    };
  }

  private validateInputRecord(importRecord: ImportScheduleData[]) {
    if (importRecord.length > 0) {
      for (const item of importRecord) {
        if (this.agentScheduleType === AgentScheduleType.Scheduling) {
          for (const x of item.ranges) {
            const fromDate = this.getDateInStringFormat(x?.dateFrom);
            const toDate = this.getDateInStringFormat(x?.dateTo);
            if (!fromDate || !toDate) {
              return true;
            }
            if (fromDate === toDate) {
              return true;
            } else {
              if (Date.parse(fromDate) > Date.parse(toDate)) {
                return true;
              }
            }
            if (item.ranges.filter(y => Date.parse(this.getDateInStringFormat(y.dateFrom)) === Date.parse(fromDate) &&
              Date.parse(this.getDateInStringFormat(y.dateTo)) === Date.parse(toDate)).length > 1) {
              return true;
            }
            if (item.ranges.find(y => Date.parse(this.getDateInStringFormat(y.dateFrom)) > Date.parse(fromDate) &&
              Date.parse(this.getDateInStringFormat(y.dateTo)) < Date.parse(toDate))) {
              return true;
            }
            if (item.ranges.find(y => Date.parse(this.getDateInStringFormat(y.dateFrom)) <= Date.parse(fromDate) &&
              Date.parse(this.getDateInStringFormat(y.dateTo)) < Date.parse(toDate) &&
              Date.parse(this.getDateInStringFormat(y.dateTo)) >= Date.parse(fromDate))) {
              return true;
            }
            if (item.ranges.find(y => Date.parse(this.getDateInStringFormat(y.dateFrom)) > Date.parse(fromDate) &&
              Date.parse(this.getDateInStringFormat(y.dateTo)) >= Date.parse(toDate) &&
              Date.parse(this.getDateInStringFormat(y.dateFrom)) <= Date.parse(toDate))) {
              return true;
            }
            const chartItem = x.agentScheduleCharts[0];
            if (chartItem?.charts?.length > 0) {
              if (this.validateChart(chartItem.charts)) {
                return true;
              }
            }
          }
        }
      }
    } else {
      return true;
    }
  }

  private validateManagerInputRecord(importRecord: AgentShceduleMangerData[]) {
    if (importRecord.length > 0) {
      for (const item of importRecord) {
        for (const chartData of item.agentScheduleManagerCharts) {
          if (!chartData.date) {
            return true;
          }
          if (chartData?.charts.length > 0) {
            if (this.validateChart(chartData.charts)) {
              return true;
            }
          }
        }
      }
    } else {
      return true;
    }
  }

  private validateChart(charts: ScheduleChart[]) {
    for (const chartItem of charts) {
      if (this.validateTimeFormat(chartItem) === true) {
        return true;
      }
      if (charts.filter(x => this.convertTimeFormat(x.startTime) === this.convertTimeFormat(chartItem.startTime) &&
        this.convertTimeFormat(x.endTime) === this.convertTimeFormat(chartItem.endTime)).length > 1) {
        return true;
      }
      if (charts.filter(x => this.convertTimeFormat(x.startTime) >= this.convertTimeFormat(chartItem.startTime) &&
        this.convertTimeFormat(x.startTime) < this.convertTimeFormat(chartItem.endTime)).length > 1) {
        return true;
      }
      if (charts.filter(x => this.convertTimeFormat(x.startTime) > this.convertTimeFormat(chartItem.startTime) &&
        this.convertTimeFormat(x.endTime) <= this.convertTimeFormat(chartItem.endTime)).length > 1) {
        return true;
      }
      if (charts.find(x => this.convertTimeFormat(x.startTime) < this.convertTimeFormat(chartItem.startTime) &&
        this.convertTimeFormat(x.endTime) >= this.convertTimeFormat(chartItem.endTime))) {
        return true;
      }
      if (charts.find(x => this.convertTimeFormat(x.startTime) < this.convertTimeFormat(chartItem.startTime) &&
        this.convertTimeFormat(x.endTime) === this.convertTimeFormat(chartItem.endTime))) {
        return true;
      }
    }
  }

  private validateTimeFormat(data: ScheduleChart) {
    if (this.convertTimeFormat(data.startTime) >= this.convertTimeFormat(data.endTime)) {
      return true;
    }
    if (+data.startTime.slice(4, 6) % 5 !== 0 || +data.endTime.slice(4, 6) % 5 !== 0) {
      return true;
    }
    if (data.startTime && data.endTime) {
      if (data.startTime.indexOf(':') > -1 && data.startTime.indexOf(' ') > -1 &&
        data.endTime.indexOf(':') > -1 && data.endTime.indexOf(' ') > -1) {
        if (!data.startTime.split(':')[0] && !data.startTime.split(':')[1].split(' ')[0] &&
          !data.endTime.split(':')[0] && !data.endTime.split(':')[1].split(' ')[0]) {
          return true;
        }
      } else {
        return true;
      }
    } else {
      return true;
    }
  }

  private importAgentScheduleChart(scheduleResponse: AgentSchedulesResponse[], schedulingCodes: SchedulingCode[], hasMismatch?: boolean) {
    const model = this.getImportAgentScheduleChartModel(scheduleResponse, schedulingCodes);
    if (!this.validateInputRecord(model?.importAgentScheduleCharts)) {
      this.formatTimeFormat(model?.importAgentScheduleCharts);
      this.spinnerService.show(this.spinner, SpinnerOptions);
      this.importAgentScheduleChartSubscription = this.agentSchedulesService.importAgentScheduleChart(model)
        .subscribe(() => {
          this.spinnerService.hide(this.spinner);
          this.activeModal.close({ partialImport: hasMismatch });
        }, (error) => {
          this.spinnerService.hide(this.spinner);
          console.log(error);
        });
      this.subscriptions.push(this.importAgentScheduleChartSubscription);

    } else {
      const errorMessage = `“An error occurred upon importing the file. Please check the following”<br>Duplicated Record<br>Incorrect Columns<br>Invalid Date Range and Time<br>Not recognized Employee ID`;
      this.showErrorWarningPopUpMessage(errorMessage);
    }
  }

  private updateManagerChart(scheduleResponse: AgentSchedulesResponse[], schedulingCodes: SchedulingCode[], hasMismatch?: boolean) {
    const model = this.getImportAgentManagerChartModel(scheduleResponse, schedulingCodes);
    if (!this.validateManagerInputRecord(model?.scheduleManagers)) {
      this.formatTimeFormat(model?.scheduleManagers);
      this.spinnerService.show(this.spinner, SpinnerOptions);

      this.updateManagerChartSubscription = this.agentScheduleManagerService.updateScheduleManagerChart(model)
        .subscribe(() => {
          this.spinnerService.hide(this.spinner);
          this.activeModal.close({ partialImport: hasMismatch });
        }, (error) => {
          this.spinnerService.hide(this.spinner);
          console.log(error);
        });

      this.subscriptions.push(this.updateManagerChartSubscription);

    } else {
      const errorMessage = `“An error occurred upon importing the file. Please check the following”<br>Duplicated Record<br>Incorrect Columns<br>Invalid Date Range and Time<br>Not recognized Employee ID`;
      this.showErrorWarningPopUpMessage(errorMessage);
    }


  }

  private loadAgentSchedules(employees: number[]) {
    let agentSchedule;
    let agentManagerSchedule;
    const activityCodes = Array<string>();
    this.jsonData.forEach(element => {
      if (activityCodes.findIndex(x => x.trim().toLowerCase() === element.ActivityCode.trim().toLowerCase()) === -1) {
        activityCodes.push(element.ActivityCode);
      }
    });

    if (this.agentScheduleType === AgentScheduleType.Scheduling) {
      const agentScheduleQueryParams = new AgentSchedulesQueryParams();
      if (employees.length > 0) {
        agentScheduleQueryParams.employeeIds = [];
        agentScheduleQueryParams.employeeIds = employees;
        agentSchedule = this.agentSchedulesService.getAgentSchedules(agentScheduleQueryParams);
      }
    } else {
      const agentScheduleManagerQueryParams = new AgentScheduleManagersQueryParams();
      agentScheduleManagerQueryParams.agentSchedulingGroupId = this.agentSchedulingGroupId;
      agentScheduleManagerQueryParams.fields = 'employeeId';
      agentManagerSchedule = this.agentScheduleManagerService.getAgentScheduleManagers(agentScheduleManagerQueryParams);
    }

    const schedulingCodeQueryParams = new SchedulingCodeQueryParams();
    schedulingCodeQueryParams.skipPageSize = true;
    schedulingCodeQueryParams.fields = activityCodes?.length > 0 ? 'id, description' : undefined;
    schedulingCodeQueryParams.activityCodes = activityCodes?.length > 0 ? activityCodes : undefined;

    const schedulingCodes = this.schedulingCodeService.getSchedulingCodes(schedulingCodeQueryParams);

    this.spinnerService.show(this.spinner, SpinnerOptions);
    forkJoin([this.agentScheduleType === AgentScheduleType.Scheduling ? agentSchedule : agentManagerSchedule, schedulingCodes])
      .subscribe((data: any) => {
        let scheduleRepsonse;
        let schedulingCodesResponse;

        if (data[0] && Object.entries(data[0]).length !== 0 && data[0].body) {
          scheduleRepsonse = data[0].body as AgentSchedulesResponse;
        }
        if (data[1] && Object.entries(data[1]).length !== 0 && data[1].body) {
          schedulingCodesResponse = data[1].body as SchedulingCode;
        }

        this.spinnerService.hide(this.spinner);
        if (scheduleRepsonse?.length > 0 && schedulingCodesResponse?.length > 0) {
          const hasMismatch = activityCodes.length !== schedulingCodesResponse?.length;
          this.agentScheduleType === AgentScheduleType.Scheduling ?
            this.importAgentScheduleChart(scheduleRepsonse, schedulingCodesResponse, hasMismatch) :
            this.updateManagerChart(scheduleRepsonse, schedulingCodesResponse, hasMismatch);
        } else {
          const errorMessage = `An error occurred upon importing the file. Please check the following<br>Duplicated Record<br>Incorrect Columns<br>Invalid Date Range and Time<br>Not recognized Employee ID`;
          this.showErrorWarningPopUpMessage(errorMessage);
        }
      }, error => {
        this.spinnerService.hide(this.spinner);
        console.log(error);
      });
  }

  private getImportAgentScheduleChartModel(schedules: AgentSchedulesResponse[], schedulingCodes: SchedulingCode[]) {
    const chartModel = new ImportShceduleChart();
    chartModel.activityOrigin = ActivityOrigin.CSS;
    chartModel.modifiedUser = +this.authService.getLoggedUserInfo()?.employeeId;
    chartModel.modifiedBy = this.authService.getLoggedUserInfo()?.displayName;

    for (const employee of schedules) {
      const employeeDetails = this.jsonData.filter(x => +x.EmployeeId === +employee.employeeId);
      const importData = new ImportScheduleData();
      const chartArray = new Array<ScheduleChart>();
      const scheduleRangeList = new Array<ScheduleDateRangeBase>();
      importData.employeeId = employee.employeeId;

      for (const item of employeeDetails) {
        const startDate = new Date(item.StartDate);
        const endDate = new Date(item.EndDate);

        item.StartDate = this.getFormattedDate(startDate);
        item.EndDate = this.getFormattedDate(endDate);

        if (scheduleRangeList.filter(x => this.getDateInStringFormat(x.dateFrom) === this.getDateInStringFormat(item.StartDate) &&
          this.getDateInStringFormat(x.dateTo) === this.getDateInStringFormat(item.EndDate)).length === 0) {
          const range = new ScheduleDateRangeBase();
          range.dateTo = new Date(item.EndDate);
          range.dateFrom = new Date(item.StartDate);
          scheduleRangeList.push(range);
        }
      }

      scheduleRangeList.forEach(ele => {
        const arrayItem = new ImportAgentScheduleRanges();
        arrayItem.dateTo = ele.dateTo;
        arrayItem.dateFrom = ele.dateFrom;
        employeeDetails.forEach(item => {
          if (this.getDateInStringFormat(ele.dateFrom) === this.getDateInStringFormat(item.StartDate) &&
            this.getDateInStringFormat(ele.dateTo) === this.getDateInStringFormat(item.EndDate)) {
            const data = schedulingCodes.find(x => x.description.trim().toLowerCase() === item.ActivityCode.trim().toLowerCase());
            if (data) {
              const chart = new ScheduleChart(item.StartTime, item.EndTime, data.id);
              chartArray.push(chart);
            }
          }
        });
        for (let i = 0; i < 7; i++) {
          const chartData = new AgentScheduleChart();
          chartData.day = i;
          chartData.charts = chartArray;
          arrayItem.agentScheduleCharts.push(chartData);
        }
        importData.ranges.push(arrayItem);
      });
      chartModel.importAgentScheduleCharts.push(importData);
    }

    return chartModel;

  }

  private getImportAgentManagerChartModel(schedules: AgentSchedulesResponse[], schedulingCodes: SchedulingCode[]) {
    const chartModel = new UpdateAgentScheduleMangersChart();
    chartModel.activityOrigin = ActivityOrigin.CSS;
    chartModel.modifiedUser = +this.authService.getLoggedUserInfo()?.employeeId;
    chartModel.modifiedBy = this.authService.getLoggedUserInfo()?.displayName;
    chartModel.isImport = true;
    for (const employee of schedules) {
      const employeeDetails = this.jsonData.filter(x => +x.EmployeeId === +employee.employeeId);
      const dateArray = [];
      employeeDetails.forEach(ele => {
        if (!dateArray.includes(ele.Date)) {
          dateArray.push(ele.Date);
        }
      });
      const importData = new AgentShceduleMangerData();
      importData.employeeId = employee.employeeId;
      for (const ele of employeeDetails) {
        for (const date of dateArray) {
          if (ele.Date === date) {
            const agentScheduleManagerChart = new AgentScheduleManagerChart();
            const dateData = new Date(date);
            agentScheduleManagerChart.date = this.getFormattedDate(dateData);
            const data = schedulingCodes.find(x => x.description.trim().toLowerCase() === ele.ActivityCode.trim().toLowerCase());
            if (data) {
              const chart = new ScheduleChart(ele.StartTime, ele.EndTime, data.id);
              agentScheduleManagerChart.charts.push(chart);
            }
            if (agentScheduleManagerChart.charts.length > 0) {
              importData.agentScheduleManagerCharts.push(agentScheduleManagerChart);
            }
          }
        }
      }
      if (importData.agentScheduleManagerCharts.length > 0) {
        chartModel.scheduleManagers.push(importData);
      }
    }

    return chartModel;

  }

  private showErrorWarningPopUpMessage(contentMessage: any) {
    const options: NgbModalOptions = { backdrop: 'static', centered: true, size: 'md' };
    const modalRef = this.modalService.open(ErrorWarningPopUpComponent, options);
    modalRef.componentInstance.headingMessage = 'Error';
    modalRef.componentInstance.contentMessage = contentMessage;
    modalRef.componentInstance.messageType = ContentType.Html;

    return modalRef;
  }

  private convertTimeFormat(time: string) {
    if (time) {
      const count = time?.split(' ')[1]?.trim().toLowerCase() === 'pm' ? 12 : undefined;
      if (count) {
        time = (+time?.split(':')[0] + 12) + ':' + time?.split(':')[1]?.split(' ')[0];
      } else {
        time = time?.split(':')[0] + ':' + time?.split(':')[1]?.split(' ')[0];
      }

      return time;
    }
  }

  private getFormattedDate(date: Date) {
    const transformedDate = this.datepipe.transform(date, 'yyyy-MM-dd');
    return new Date(transformedDate);
  }

  private getDateInStringFormat(startDate: any): string {
    if (!startDate) {
      return undefined;
    }

    const date = new Date(startDate);
    return date.toDateString();
  }
}
