import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import * as XLSX from 'xlsx';
import { NgbActiveModal, NgbModal, NgbModalOptions } from '@ng-bootstrap/ng-bootstrap';
import { TranslationDetails } from 'src/app/shared/models/translation-details.model';
import { ExcelData } from '../../../models/excel-data.model';
import { SchedulingCodeQueryParams } from '../../../../system-admin/models/scheduling-code-query-params.model';
import { NgxSpinnerService } from 'ngx-spinner';
import { SpinnerOptions } from 'src/app/shared/util/spinner-options.util';
import { SubscriptionLike as ISubscription } from 'rxjs';
import { SchedulingCodeService } from 'src/app/shared/services/scheduling-code.service';
import { SchedulingCode } from '../../../../system-admin/models/scheduling-code.model';
import { AgentSchedulesService } from '../../../services/agent-schedules.service';
import { AgentScheduleChart } from '../../../models/agent-schedule-chart.model';
import { UpdateAgentschedulechart } from '../../../models/update-agent-schedule-chart.model';
import { AgentScheduleType } from '../../../enums/agent-schedule-type.enum';
import { ScheduleChart } from '../../../models/schedule-chart.model';
import { AuthService } from 'src/app/core/services/auth.service';
import { ErrorWarningPopUpComponent } from 'src/app/shared/popups/error-warning-pop-up/error-warning-pop-up.component';
import { ContentType } from 'src/app/shared/enums/content-type.enum';
import { Papa } from 'ngx-papaparse';
import { AgentSchedulesQueryParams } from '../../../models/agent-schedules-query-params.model';

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
  jsonData: ExcelData[] = [];
  schedulingCodes: SchedulingCode[] = [];
  columns = ['EmployeeId', 'StartDate', 'EndDate', 'ActivityCode', 'StartTime', 'EndTime'];

  getAgentSchedulesSubscription: ISubscription;
  getSchedulingCodesSubscription: ISubscription;
  importAgentScheduleChartSubscription: ISubscription;
  subscriptions: ISubscription[] = [];

  @Input() translationValues: TranslationDetails[];
  @Input() agentScheduleId: string;

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
    if (this.uploadFile && !this.fileFormatValidation && this.jsonData.length > 0 && !this.validateInputRecord()) {
      this.loadAgentSchedules(this.jsonData[0].EmployeeId);
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
    this.fileUploaded = files[0];
    this.uploadFile = this.fileUploaded.name;
    if (this.uploadFile.split('.')[1] === 'xlsx') {
      this.fileFormatValidation = false;
      this.readExcel();
    } else if (this.uploadFile.split('.')[1] === 'csv') {
      this.fileFormatValidation = false;
      this.readCsvFile();
    } else {
      this.fileFormatValidation = true;
    }
  }

  private readCsvFile() {
    const reader: FileReader = new FileReader();
    reader.readAsText(this.fileUploaded);
    reader.onload = e => {
      const csv = reader.result;
      const results = this.papa.parse(csv as string, { header: false });
      if (results !== null && results !== undefined && results.data !== null &&
        results.data !== undefined && results.data.length > 0 && results.errors.length === 0) {
        const csvTableHeader = results.data[0];
        const csvTableData = [...results.data.slice(1, results.data.length)];
        csvTableData.forEach((ele, index) => {
          const csvJson = new ExcelData();
          if (ele.length > 0) {
            for (let i = 0; i < ele.length; i++) {
              csvJson[csvTableHeader[i]] = ele[i];
            }
          }
          if (csvJson.ActivityCode) {
            this.jsonData.push(csvJson);
          }
        });
      }
    };
  }

  private validateInputRecord() {
    if (this.jsonData.length > 0) {
      for (const item of this.jsonData) {
        if (item.StartDate && item.EndDate) {
          if (item.StartDate !== item.EndDate) {
            return true;
          }
          if (!this.jsonData.every(x => x.StartDate === item.StartDate &&
          x.EndDate === item.EndDate)) {
            return true;
          }
        } else {
          return true;
        }
        if (!this.jsonData.every(x => x.EmployeeId === item.EmployeeId)) {
          return true;
        }
        if (this.jsonData.filter(x => this.convertToDateFormat(x.StartTime) === this.convertToDateFormat(item.StartTime) &&
        this.convertToDateFormat(x.EndTime) === this.convertToDateFormat(item.EndTime)).length > 1) {
          return true;
        }
        if (this.jsonData.filter(x => this.convertToDateFormat(x.StartTime) >= this.convertToDateFormat(item.StartTime) &&
        this.convertToDateFormat(x.StartTime) < this.convertToDateFormat(item.EndTime)).length > 1) {
          return true;
        }
        if (this.jsonData.filter(x => this.convertToDateFormat(x.StartTime) > this.convertToDateFormat(item.StartTime) &&
        this.convertToDateFormat(x.EndTime) <= this.convertToDateFormat(item.EndTime)).length > 1) {
          return true;
        }
        if (this.jsonData.find(x => this.convertToDateFormat(x.StartTime) < this.convertToDateFormat(item.StartTime) &&
        this.convertToDateFormat(x.EndTime) >= this.convertToDateFormat(item.EndTime))) {
          return true;
        }
        if (this.jsonData.find(x => this.convertToDateFormat(x.StartTime) < this.convertToDateFormat(item.StartTime) &&
        this.convertToDateFormat(x.EndTime) === this.convertToDateFormat(item.EndTime))) {
          return true;
        }
        if (item) {
          for (const property in item) {
            if (this.columns.findIndex(x => x === property) === -1) {
              return true;
            }
          }
        }
        if (!this.jsonData.every(x => x.EmployeeId === this.jsonData[0].EmployeeId)) {
          return false;
        }
        return this.validateTimeFormat(item);
      }
    } else {
      return true;
    }
  }

  private validateTimeFormat(data: ExcelData) {
    if (data.StartTime && data.EndTime) {
      if (data.StartTime.indexOf(':') > -1 && data.StartTime.indexOf(' ') > -1 &&
        data.EndTime.indexOf(':') > -1 && data.EndTime.indexOf(' ') > -1) {
        if (!data.StartTime.split(':')[0] && !data.StartTime.split(':')[1].split(' ')[0] &&
          !data.EndTime.split(':')[0] && !data.EndTime.split(':')[1].split(' ')[0]) {
          return true;
        }
      } else {
        return true;
      }
    } else {
      return true;
    }
  }

  private loadSchedulingCodes(activityCodes?: Array<string>) {
    const queryParams = new SchedulingCodeQueryParams();
    queryParams.skipPageSize = true;
    queryParams.fields = activityCodes?.length > 0 ? 'id, description' : undefined;
    queryParams.activityCodes = activityCodes?.length > 0 ? activityCodes : undefined;
    this.spinnerService.show(this.spinner, SpinnerOptions);

    this.getSchedulingCodesSubscription = this.schedulingCodeService.getSchedulingCodes(queryParams)
      .subscribe((response) => {
        if (response.body) {
          this.schedulingCodes = response.body;
          this.matchActivitycodes(activityCodes);
        }
        this.spinnerService.hide(this.spinner);
      }, (error) => {
        this.spinnerService.hide(this.spinner);
        console.log(error);
      });

    this.subscriptions.push(this.getSchedulingCodesSubscription);
  }

  private matchActivitycodes(activityCodes: Array<string>) {
    let hasMismatch = false;
    if (activityCodes.length > 0) {
      for (const index in activityCodes) {
        if (this.schedulingCodes.findIndex(x => x.description === activityCodes[index]) === -1) {
          hasMismatch = true;
          break;
        }
      }
    }

    this.importAgentScheduleChart(hasMismatch);
  }

  private importAgentScheduleChart(hasMismatch?: boolean) {
    const model = this.getImportAgentScheduleChartModel();
    this.spinnerService.show(this.spinner, SpinnerOptions);

    this.importAgentScheduleChartSubscription = this.agentSchedulesService.importAgentScheduleChart(this.agentScheduleId, model)
      .subscribe(() => {
        this.spinnerService.hide(this.spinner);
        this.activeModal.close({ partialImport: hasMismatch });
      }, (error) => {
        this.spinnerService.hide(this.spinner);
        console.log(error);
      });

    this.subscriptions.push(this.importAgentScheduleChartSubscription);
  }

  private loadAgentSchedules(employeeId: number) {
    const queryParams = new AgentSchedulesQueryParams();
    queryParams.employeeIds = [];
    const employee = +employeeId ?? 0;
    queryParams.employeeIds.push(employee);
    this.spinnerService.show(this.spinner, SpinnerOptions);

    this.getAgentSchedulesSubscription = this.agentSchedulesService.getAgentSchedules(queryParams)
      .subscribe((response) => {
        this.spinnerService.hide(this.spinner);
        if (response.body.length > 0) {
          const activityCodes = Array<string>();
          this.jsonData.forEach(element => {
            activityCodes.push(element.ActivityCode);
          });
          this.loadSchedulingCodes(activityCodes);
        } else {
          const errorMessage = `“An error occurred upon importing the file. Please check the following”<br>Duplicated Record<br>Incorrect Columns<br>Invalid Date Range and Time`;
          this.showErrorWarningPopUpMessage(errorMessage);
        }

      }, (error) => {
        this.spinnerService.hide(this.spinner);
        console.log(error);
      });

    this.subscriptions.push(this.getAgentSchedulesSubscription);
  }

  private getImportAgentScheduleChartModel() {
    const chartModel = new UpdateAgentschedulechart();
    chartModel.agentScheduleCharts = [];
    chartModel.agentScheduleType = AgentScheduleType.Scheduling;
    chartModel.modifiedBy = this.authService.getLoggedUserInfo()?.displayName;
    for (let i = 0; i < 7; i++) {
      const chartData = new AgentScheduleChart();
      chartData.day = i;
      this.jsonData.forEach((ele, index) => {
        const data = this.schedulingCodes.find(x => x.description === ele.ActivityCode);
        if (data) {
          const chart = new ScheduleChart(ele.StartTime, ele.EndTime, data.id);
          chartData.charts.push(chart);
        }
      });
      chartModel.agentScheduleCharts.push(chartData);
    }

    return chartModel;

  }

  private readExcel() {
    const readFile = new FileReader();
    readFile.onload = (e) => {
      this.storeData = readFile.result;
      const data = new Uint8Array(this.storeData);
      const arr = new Array();
      for (let i = 0; i !== data.length; ++i) {
        arr[i] = String.fromCharCode(data[i]);
      }
      const bstr = arr.join('');
      const workbook = XLSX.read(bstr, { type: 'binary' });
      const firstSheetName = workbook.SheetNames[0];
      this.worksheet = workbook.Sheets[firstSheetName];
      this.jsonData = XLSX.utils.sheet_to_json(this.worksheet, { raw: false });
    };
    readFile.readAsArrayBuffer(this.fileUploaded);
  }

  private showErrorWarningPopUpMessage(contentMessage: any) {
    const options: NgbModalOptions = { backdrop: 'static', centered: true, size: 'sm' };
    const modalRef = this.modalService.open(ErrorWarningPopUpComponent, options);
    modalRef.componentInstance.headingMessage = 'Error';
    modalRef.componentInstance.contentMessage = contentMessage;
    modalRef.componentInstance.messageType = ContentType.Html;

    return modalRef;
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
