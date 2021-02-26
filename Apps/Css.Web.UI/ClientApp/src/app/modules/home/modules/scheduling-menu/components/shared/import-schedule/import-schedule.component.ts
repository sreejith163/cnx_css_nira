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
import { AgentScheduleManagerChart } from '../../../models/agent-schedule-manager-chart.model';
import { ActivityOrigin } from '../../../enums/activity-origin.enum';

@Component({
  selector: 'app-import-schedule',
  templateUrl: './import-schedule.component.html',
  styleUrls: ['./import-schedule.component.scss']
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
      if (!this.fileFormatValidation && !this.validateHeading() && !this.validateDateAndTime()) {
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
    } else {
      const errorMessage = `“An error occurred upon importing the file. Please check the following”<br>Duplicated Record<br>Incorrect Columns<br>Invalid Date Range and Time`;
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
    const exportFileName = 'Attendance_scheduling';
    this.fileUploaded = files[0];
    this.uploadFile = this.fileUploaded?.name;
    if (this.uploadFile.split('.')[1].toLowerCase() === 'csv'.toLowerCase()) {
      // const dateSection = this.uploadFile.split(exportFileName)[1].split('.')[0];
      // const year = dateSection?.substr(0, 4);
      // const month = dateSection?.substr(4, 2);
      // const day = dateSection?.substr(6, 2);
      // const date = new Date(year + '/' + month + '/' + day);
      // if (date instanceof Date && dateSection.length === 8) {
      this.readCsvFile();
      //   this.fileFormatValidation = false;
      // }
      // else {
      //   this.fileFormatValidation = true;
      // }

    } else {
      this.fileFormatValidation = true;
    }
  }

  private formatTimeFormat(importRecord: any[]) {
    for (const item of importRecord) {
      if (this.agentScheduleType === AgentScheduleType.Scheduling) {
        for (const record of item.agentScheduleCharts) {
          record.charts.map(x => {
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
      } else {
        const chartData = item.agentScheduleManagerChart;
        chartData.charts.map(x => {
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
          x.StartTime = x?.StartTime.trim().toLowerCase();
          x.EndTime = x?.EndTime.trim().toLowerCase();
          x.ActivityCode = x?.ActivityCode.trim().toLowerCase();
        });
      }
    };
  }

  private validateInputRecord(importRecord: any[]) {
    if (importRecord.length > 0) {
      if (this.agentScheduleType === AgentScheduleType.Scheduling &&
        !importRecord.every(x => Date.parse(x?.dateFrom) === Date.parse(importRecord[0]?.dateFrom) &&
          Date.parse(x?.dateTo) === Date.parse(importRecord[0]?.dateTo))) {
        return true;
      } else if (this.agentScheduleType === AgentScheduleType.SchedulingManager) {
        const date = this.jsonData[0]?.Date;
        const year = date?.split('/')[2];
        const day = date?.split('/')[1]?.length === 1 ? '0' + date?.split('/')[1] : date?.split('/')[1];
        const month = date?.split('/')[0]?.length === 1 ? '0' + date?.split('/')[0] : date?.split('/')[0];

        const isValidDate = Date.parse(`${day}/${month}/${year}`);
        if (isNaN(isValidDate)) {
          return false;
        }

        if (!this.jsonData.every(x => x?.Date.trim() === this.jsonData[0]?.Date.trim())) {
          return true;
        }
      }
    } else {
      return true;
    }
  }

  private setSchedulingImportChartToUpdate(charts: ScheduleChart[]) {
    const eleArray = [];
    const newArray = new Array<ScheduleChart>();
    charts.forEach(x => {
      eleArray.push(x.startTime);
      eleArray.push(x.endTime);
    });
    eleArray.sort((a, b): number => {
      if (this.convertTimeFormat(a) < this.convertTimeFormat(b)) {
        return -1;
      } else if (this.convertTimeFormat(a) > this.convertTimeFormat(b)) {
        return 1;
      }
      else {
        return 0;
      }
    });
    for (let i = 0; i < eleArray.length; i++) {
      if (i !== eleArray.length - 1) {
        const icon = charts.find(x => this.convertTimeFormat(x?.startTime) === this.convertTimeFormat(eleArray[i]))?.schedulingCodeId;
        const calendarTime = new ScheduleChart(eleArray[i], eleArray[i + 1], icon);
        if (calendarTime.startTime !== calendarTime.endTime) {
          newArray.push(calendarTime);
        }
      }
    }
    const items = newArray.filter(x => !x.schedulingCodeId);
    items.forEach(ele => {
      const icon = charts.find(x => this.convertTimeFormat(x?.startTime) < this.convertTimeFormat(ele?.startTime) &&
        this.convertTimeFormat(x?.endTime) >= this.convertTimeFormat(ele?.endTime))?.schedulingCodeId;
      if (icon) {
        ele.schedulingCodeId = icon;
      } else {
        const index = newArray.findIndex(x => x.startTime === ele.startTime);
        if (index > -1) {
          newArray.splice(index, 1);
        }
      }
    });
    const finalArray = newArray.length > 0 ? this.adjustSchedulingCalendarTimesRange(newArray) : newArray;

    return finalArray;
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

  private validateDateAndTime() {
    for (const item of this.jsonData) {
      if (!item.StartTime || !item.EndTime) {
        return true;
      }
      if (item?.EndTime?.trim().toLowerCase().slice(2, 3) !== ':' || item?.StartTime?.trim().toLowerCase().slice(2, 3) !== ':') {
        return true;
      }
      if (item.EndTime.trim().toLowerCase() === '00:00 am') {
        item.EndTime = '11:60 pm';
      }
      if (this.convertTimeFormat(item.StartTime) >= this.convertTimeFormat(item.EndTime)) {
        return true;
      }
      if (this.agentScheduleType === AgentScheduleType.Scheduling) {
        if (item?.StartTime && item?.EndTime) {
          if (item.StartTime.indexOf(':') > -1 && item.StartTime.indexOf(' ') > -1 &&
            item.EndTime.indexOf(':') > -1 && item.EndTime.indexOf(' ') > -1) {
            if (!item.StartTime.split(':')[0] && !item.StartTime.split(':')[1].split(' ')[0] &&
              !item.EndTime.split(':')[0] && !item.EndTime.split(':')[1].split(' ')[0]) {
              return true;
            }
          } else {
            return true;
          }
        } else {
          return true;
        }
      } else {
        if (item?.StartDate !== item?.EndDate) {
          if (item.StartDate > item.EndDate) {
            return true;
          }
        }
      }
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
      const errorMessage = `“An error occurred upon importing the file. Please check the following”<br>Duplicated Record<br>Incorrect Columns<br>Invalid Date Range and Time`;
      this.showErrorWarningPopUpMessage(errorMessage);
    }
  }

  private updateManagerChart(scheduleResponse: AgentSchedulesResponse[], schedulingCodes: SchedulingCode[], hasMismatch?: boolean) {
    const model = this.getImportAgentManagerChartModel(scheduleResponse, schedulingCodes);
    if (!this.validateInputRecord(model?.agentScheduleManagers)) {
      this.formatTimeFormat(model?.agentScheduleManagers);
      this.spinnerService.show(this.spinner, SpinnerOptions);

      this.updateManagerChartSubscription = this.agentSchedulesService.updateScheduleManagerChart(model)
        .subscribe(() => {
          this.spinnerService.hide(this.spinner);
          this.activeModal.close({ partialImport: hasMismatch });
        }, (error) => {
          this.spinnerService.hide(this.spinner);
          console.log(error);
        });

      this.subscriptions.push(this.updateManagerChartSubscription);

    } else {
      const errorMessage = `“An error occurred upon importing the file. Please check the following”<br>Duplicated Record<br>Incorrect Columns<br>Invalid Date Range and Time`;
      this.showErrorWarningPopUpMessage(errorMessage);
    }


  }

  private loadAgentSchedules(employees: number[]) {
    const activityCodes = Array<string>();
    this.jsonData.forEach(element => {
      if (activityCodes.findIndex(x => x.trim().toLowerCase() === element.ActivityCode.trim().toLowerCase()) === -1) {
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
      if (scheduleRepsonse?.length > 0 && schedulingCodesResponse?.length > 0) {
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
    chartModel.activityOrigin = ActivityOrigin.CSS;
    chartModel.modifiedUser = +this.authService.getLoggedUserInfo()?.employeeId;
    chartModel.modifiedBy = this.authService.getLoggedUserInfo()?.displayName;
    for (const employee of schedules) {
      const employeeDetails = this.jsonData.filter(x => +x.EmployeeId === +employee.employeeId);
      const importData = new ImportScheduleData();
      const chartArray = new Array<ScheduleChart>();
      importData.employeeId = employee.employeeId;
      importData.dateFrom = new Date(employeeDetails[0].StartDate);
      importData.dateTo = new Date(employeeDetails[0].EndDate);
      employeeDetails.forEach(ele => {
        const data = schedulingCodes.find(x => x.description.trim().toLowerCase() === ele.ActivityCode.trim().toLowerCase());
        if (data) {
          const chart = new ScheduleChart(ele.StartTime, ele.EndTime, data.id);
          chartArray.push(chart);
        }
      });
      const scheduleChartArray = this.setSchedulingImportChartToUpdate(chartArray);
      for (let i = 0; i < 7; i++) {
        const chartData = new AgentScheduleChart();
        chartData.day = i;
        chartData.charts = scheduleChartArray;
        importData.agentScheduleCharts.push(chartData);
      }
      chartModel.importAgentScheduleCharts.push(importData);
    }

    return chartModel;

  }

  private getImportAgentManagerChartModel(schedules: AgentSchedulesResponse[], schedulingCodes: SchedulingCode[]) {
    const chartModel = new UpdateAgentScheduleMangersChart();
    chartModel.activityOrigin = ActivityOrigin.CSS;
    chartModel.modifiedUser = +this.authService.getLoggedUserInfo()?.employeeId;
    chartModel.modifiedBy = this.authService.getLoggedUserInfo()?.displayName;
    for (const employee of schedules) {
      const employeeDetails = this.jsonData.filter(x => +x.EmployeeId === +employee.employeeId);
      const importData = new AgentShceduleMangerData();
      const chartArray = new Array<ScheduleChart>();
      importData.employeeId = employee.employeeId;
      employeeDetails.forEach(ele => {
        const data = schedulingCodes.find(x => x.description.trim().toLowerCase() === ele.ActivityCode.trim().toLowerCase());
        if (data) {
          const chart = new ScheduleChart(ele.StartTime, ele.EndTime, data.id);
          chartArray.push(chart);
        }
      });
      const chartData = new AgentScheduleManagerChart();
      chartData.date = new Date(this.jsonData[0].Date);
      chartData.charts = this.setSchedulingImportChartToUpdate(chartArray);
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
}
