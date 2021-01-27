import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { NgbActiveModal, NgbModal, NgbModalOptions } from '@ng-bootstrap/ng-bootstrap';
import { TranslationDetails } from 'src/app/shared/models/translation-details.model';
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
import { AgentScheduleManagerChart } from '../../../models/agent-schedule-manager-chart.model';

@Component({
  selector: 'app-import-schedule',
  templateUrl: './import-schedule.component.html',
  styleUrls: ['./import-schedule.component.scss']
})
export class ImportScheduleComponent implements OnInit, OnDestroy {

  uploadFile: string;
  spinner = 'import';
  fileUploaded: File;
  storeData: any;
  worksheet: any;
  fileFormatValidation: boolean;
  fileSubmitted: boolean;
  jsonData: any[] = [];
  scheduleColumns = ['EmployeeId', 'StartDate', 'EndDate', 'ActivityCode', 'StartTime', 'EndTime'];
  schedulingManagerColumns = ['EmployeeId', 'Date', 'ActivityCode', 'StartTime', 'EndTime'];
  csvTableHeader: string[];

  getAgentSchedulesSubscription: ISubscription;
  getSchedulingCodesSubscription: ISubscription;
  importAgentScheduleChartSubscription: ISubscription;
  subscriptions: ISubscription[] = [];

  @Input() agentScheduleType: AgentScheduleType;
  @Input() translationValues: TranslationDetails[];

  constructor(
    public activeModal: NgbActiveModal,
    private spinnerService: NgxSpinnerService,
    private schedulingCodeService: SchedulingCodeService,
    private agentSchedulesService: AgentSchedulesService,
    private authService: AuthService,
    private modalService: NgbModal,
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
        const errorMessage = `“An error occurred upon importing the file. Please check the following”<br>Duplicated Record<br>Incorrect Columns<br>Invalid Date Range and Time`;
        this.showErrorWarningPopUpMessage(errorMessage);
      }
    }
  }

  hasFileSelected() {
    if (this.fileSubmitted) {
      return this.uploadFile ? true : false;
    }
    return true;
  }

  browse(files: any) {
    const exportFileName = 'Attendance_scheduling';
    this.fileUploaded = files[0];
    this.uploadFile = this.fileUploaded.name;
    if (this.uploadFile.split('.')[1] === 'csv') {
      const dateSection = this.uploadFile.split(exportFileName)[1].split('.')[0];
      const year = dateSection?.substr(0, 4);
      const month = dateSection?.substr(4, 2);
      const day = dateSection?.substr(6, 2);
      const date = new Date(year + '/' + month + '/' + day);
      if (date instanceof Date && dateSection.length === 8) {
        this.readCsvFile();
        this.fileFormatValidation = false;
      } else {
        this.fileFormatValidation = true;
      }

    } else {
      this.fileFormatValidation = true;
    }
  }

  private validateHeading() {
    for (const item of this.csvTableHeader) {
      if (this.agentScheduleType === AgentScheduleType.Scheduling &&
        this.scheduleColumns.findIndex(x => x === item) === -1) {
        return true;
      }
      if (this.agentScheduleType === AgentScheduleType.SchedulingManager &&
        this.schedulingManagerColumns.findIndex(x => x === item) === -1) {
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
      }
    };
  }

  private validateInputRecord(importRecord: any[]) {
    if (importRecord.length > 0) {
      if (this.agentScheduleType === AgentScheduleType.Scheduling &&
        !importRecord.every(x => Date.parse(x?.dateFrom) === Date.parse(importRecord[0]?.dateFrom) &&
          Date.parse(x?.dateTo) === Date.parse(importRecord[0]?.dateTo))) {
        return true;
      } else if (this.agentScheduleType === AgentScheduleType.SchedulingManager &&
        !this.jsonData.every(x => x.Date === this.jsonData[0]?.Date)) {
        return true;
      }
      for (const item of importRecord) {
        if (this.agentScheduleType === AgentScheduleType.Scheduling) {
          for (const record of item.agentScheduleCharts) {
            return this.validateChart(record.charts);
          }
        } else {
          const chartData = item.agentScheduleManagerChart;
          return this.validateChart(chartData.charts);
        }
      }
    } else {
      return true;
    }
  }

  private validateChart(charts: ScheduleChart[]) {
    for (const chartItem of charts) {
      if (charts.filter(x => this.convertToDateFormat(x.startTime) === this.convertToDateFormat(chartItem.startTime) &&
        this.convertToDateFormat(x.endTime) === this.convertToDateFormat(chartItem.endTime)).length > 1) {
        return true;
      }
      if (charts.filter(x => this.convertToDateFormat(x.startTime) >= this.convertToDateFormat(chartItem.startTime) &&
        this.convertToDateFormat(x.startTime) < this.convertToDateFormat(chartItem.endTime)).length > 1) {
        return true;
      }
      if (charts.filter(x => this.convertToDateFormat(x.startTime) > this.convertToDateFormat(chartItem.startTime) &&
        this.convertToDateFormat(x.endTime) <= this.convertToDateFormat(chartItem.endTime)).length > 1) {
        return true;
      }
      if (charts.find(x => this.convertToDateFormat(x.startTime) < this.convertToDateFormat(chartItem.startTime) &&
        this.convertToDateFormat(x.endTime) >= this.convertToDateFormat(chartItem.endTime))) {
        return true;
      }
      if (charts.find(x => this.convertToDateFormat(x.startTime) < this.convertToDateFormat(chartItem.startTime) &&
        this.convertToDateFormat(x.endTime) === this.convertToDateFormat(chartItem.endTime))) {
        return true;
      }
      if (this.validateTimeFormat(chartItem) === true) {
        return true;
      }
    }
  }

  private validateTimeFormat(data: ScheduleChart) {
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
    if (!this.validateInputRecord(model.importAgentScheduleCharts)) {
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
      const errorMessage = `“An error occurred upon importing the file. Please check the following”<br>Duplicated Record<br>Incorrect Columns<br>Invalid Date Range and Time`;
      this.showErrorWarningPopUpMessage(errorMessage);
    }
  }

  private updateManagerChart(scheduleResponse: AgentSchedulesResponse[], schedulingCodes: SchedulingCode[], hasMismatch?: boolean) {
    const model = this.getImportAgentManagerChartModel(scheduleResponse, schedulingCodes);
    if (!this.validateInputRecord(model.agentScheduleManagers)) {
      this.spinnerService.show(this.spinner, SpinnerOptions);

      this.importAgentScheduleChartSubscription = this.agentSchedulesService.updateScheduleManagerChart(model)
        .subscribe(() => {
          this.spinnerService.hide(this.spinner);
          this.activeModal.close({ partialImport: hasMismatch });
        }, (error) => {
          this.spinnerService.hide(this.spinner);
          console.log(error);
        });

      this.subscriptions.push(this.importAgentScheduleChartSubscription);

    } else {
      const errorMessage = `“An error occurred upon importing the file. Please check the following”<br>Duplicated Record<br>Incorrect Columns<br>Invalid Date Range and Time`;
      this.showErrorWarningPopUpMessage(errorMessage);
    }


  }

  private loadAgentSchedules(employees: number[]) {
    const activityCodes = Array<string>();
    this.jsonData.forEach(element => {
      if (activityCodes.findIndex(x => x === element.ActivityCode) === -1) {
        activityCodes.push(element.ActivityCode);
      }
    });

    const agentScheduleQueryParams = new AgentSchedulesQueryParams();
    if (employees.length > 0) {
      agentScheduleQueryParams.employeeIds = [];
      agentScheduleQueryParams.employeeIds = employees;
    }

    const schedulingCodeQueryParams = new SchedulingCodeQueryParams();
    schedulingCodeQueryParams.skipPageSize = true;
    schedulingCodeQueryParams.fields = activityCodes?.length > 0 ? 'id, description' : undefined;
    schedulingCodeQueryParams.activityCodes = activityCodes?.length > 0 ? activityCodes : undefined;

    const agentSchedule = this.agentSchedulesService.getAgentSchedules(agentScheduleQueryParams);
    const schedulingCodes = this.schedulingCodeService.getSchedulingCodes(schedulingCodeQueryParams);

    this.spinnerService.show(this.spinner, SpinnerOptions);
    forkJoin([agentSchedule, schedulingCodes]).subscribe((data: any) => {
      let scheduleRepsonse;
      let schedulingCodesResponse;

      if (data[0] && Object.entries(data[0]).length !== 0 && data[0].body) {
        scheduleRepsonse = data[0].body as AgentSchedulesResponse;
      }
      if (data[1] && Object.entries(data[1]).length !== 0 && data[1].body) {
        schedulingCodesResponse = data[1].body as SchedulingCode;
      }

      this.spinnerService.hide(this.spinner);
      if (scheduleRepsonse.length > 0 && schedulingCodesResponse.length > 0) {
        const hasMismatch = activityCodes.length !== schedulingCodesResponse?.length;
        this.agentScheduleType === AgentScheduleType.Scheduling ?
          this.importAgentScheduleChart(scheduleRepsonse, schedulingCodesResponse, hasMismatch) :
          this.updateManagerChart(scheduleRepsonse, schedulingCodesResponse, hasMismatch);
      } else {
        const errorMessage = `An error occurred upon importing the file. Please check the following<br>Duplicated Record<br>Incorrect Columns<br>Invalid Date Range and Time`;
        this.showErrorWarningPopUpMessage(errorMessage);
      }
    }, error => {
      this.spinnerService.hide(this.spinner);
      console.log(error);
    });
  }

  private getImportAgentScheduleChartModel(schedules: AgentSchedulesResponse[], schedulingCodes: SchedulingCode[]) {
    const chartModel = new ImportShceduleChart();
    chartModel.modifiedBy = this.authService.getLoggedUserInfo()?.displayName;
    for (const employee of schedules) {
      const employeeDetails = this.jsonData.filter(x => +x.EmployeeId === +employee.employeeId);
      const importData = new ImportScheduleData();
      const chartArray = new Array<ScheduleChart>();
      importData.employeeId = employee.employeeId;
      importData.dateFrom = new Date(employeeDetails[0].StartDate);
      importData.dateTo = new Date(employeeDetails[0].EndDate);
      employeeDetails.forEach(ele => {
        const data = schedulingCodes.find(x => x.description === ele.ActivityCode);
        if (data) {
          const chart = new ScheduleChart(ele.StartTime, ele.EndTime, data.id);
          chartArray.push(chart);
        }
      });
      for (let i = 0; i < 7; i++) {
        const chartData = new AgentScheduleChart();
        chartData.day = i;
        chartData.charts = chartArray;
        importData.agentScheduleCharts.push(chartData);
      }
      chartModel.importAgentScheduleCharts.push(importData);
    }

    return chartModel;

  }

  private getDateInFormat(date: string) {
    const year = date.split('/')[0];
    const month = date.split('/')[1].split('/')[0];
    const day = date.split(`${month}/`)[1];

    const newDate = new Date(+year, (+month) - 1, (+day) + 1, 0, 0, 0, 0);
    return newDate;

  }

  private getImportAgentManagerChartModel(schedules: AgentSchedulesResponse[], schedulingCodes: SchedulingCode[]) {
    const chartModel = new UpdateAgentScheduleMangersChart();
    chartModel.modifiedBy = this.authService.getLoggedUserInfo()?.displayName;
    for (const employee of schedules) {
      const employeeDetails = this.jsonData.filter(x => +x.EmployeeId === +employee.employeeId);
      const importData = new AgentShceduleMangerData();
      const chartArray = new Array<ScheduleChart>();
      importData.employeeId = employee.employeeId;
      employeeDetails.forEach(ele => {
        const data = schedulingCodes.find(x => x.description === ele.ActivityCode);
        if (data) {
          const chart = new ScheduleChart(ele.StartTime, ele.EndTime, data.id);
          chartArray.push(chart);
        }
      });
      const chartData = new AgentScheduleManagerChart();
      chartData.date = this.getDateInFormat(this.jsonData[0].Date);
      chartData.charts = chartArray;
      importData.agentScheduleManagerChart = chartData;
      chartModel.agentScheduleManagers.push(importData);
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

  private convertToDateFormat(time: string) {
    if (time) {
      const count = time.split(' ')[1] === 'pm' || time.split(' ')[1] === 'PM' ? 12 : undefined;
      if (count) {
        time = (+time.split(':')[0] + 12) + ':' + time.split(':')[1].split(' ')[0];
      } else {
        time = time.split(':')[0] + ':' + time.split(':')[1].split(' ')[0];
      }

      return time;
    }
  }
}
