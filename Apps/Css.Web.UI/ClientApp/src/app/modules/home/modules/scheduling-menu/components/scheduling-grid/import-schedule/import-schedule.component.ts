import { Component, ElementRef, Input, OnDestroy, OnInit, ViewChild } from '@angular/core';
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
import { MessageType } from 'src/app/shared/enums/message-type.enum';
import { WeekDay } from '@angular/common';

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
  jsonData: ExcelData[] = [];
  schedulingCodes: SchedulingCode[] = [];

  getSchedulingCodesSubscription: ISubscription;
  importAgentScheduleChartSubscription: ISubscription;
  subscriptions: ISubscription[] = [];

  @Input() translationValues: TranslationDetails[];
  @Input() agentScheduleId: string;

  @ViewChild('recordErrorMsg') recordErrorMsg: ElementRef;

  constructor(
    public activeModal: NgbActiveModal,
    private spinnerService: NgxSpinnerService,
    private schedulingCodeService: SchedulingCodeService,
    private agentSchedulesService: AgentSchedulesService,
    private authService: AuthService,
    private modalService: NgbModal,
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
    if (this.uploadFile && !this.fileFormatValidation) {
      this.jsonData = XLSX.utils.sheet_to_json(this.worksheet, { raw: false });
      const activityCodes = Array<string>();
      this.jsonData.forEach(element => {
        activityCodes.push(element.ActivityCode);
      });
      if (!this.validateDuplicateRecord() && !this.validateTimeFormat() && !this.validateDataModel()) {
        this.loadSchedulingCodes(activityCodes);
      } else {
        const errorMessage = this.recordErrorMsg;
        this.showErrorWarningPopUpMessage(errorMessage);
      }

    }
  }

  browse(files: any) {
    this.fileUploaded = files[0];
    this.uploadFile = this.fileUploaded.name;
    if (this.uploadFile.split('.')[1] === 'xlsx') {
      this.fileFormatValidation = false;
      this.readExcel();
    } else {
      this.fileFormatValidation = true;
    }
  }

  private validateDuplicateRecord() {
    let validation = false;
    this.jsonData.forEach(ele => {
      if (this.jsonData.filter(x => x.ActivityCode === ele.ActivityCode && x.StartTime === ele.StartTime &&
        x.Endtime === ele.Endtime && x.Day === ele.Day).length > 1) {
          validation = true;
      } else {
        validation = false;
      }
    });

    return validation;
  }

  private validateTimeFormat() {
    let validation = false;
    this.jsonData.forEach(ele => {
      if (ele.StartTime && ele.Endtime) {
        if (ele.StartTime.indexOf(':') > -1 && ele.StartTime.indexOf(' ') > -1 &&
          ele.Endtime.indexOf(':') > -1 && ele.Endtime.indexOf(' ') > -1) {
          if (ele.StartTime.split(':')[0] && ele.StartTime.split(':')[1].split(' ')[0] &&
            ele.Endtime.split(':')[0] && ele.Endtime.split(':')[1].split(' ')[0]) {
              validation =  false;
          } else {
            validation =  true;
          }
        } else {
          validation =  true;
        }
      }
    });

    return validation;
  }

  private validateDataModel() {
    let validation = false;
    const columns = ['EmployeeId', 'Day', 'StartDate', 'EndDate', 'ActivityCode', 'StartTime', 'Endtime'];
    this.jsonData.forEach(data => {
      for (const property in data) {
        if (columns.findIndex(x => x === property) === -1 ) {
          validation = true;
        }
      }
    });
    return validation;

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

  private getImportAgentScheduleChartModel() {
    const chartModel = new UpdateAgentschedulechart();
    chartModel.agentScheduleCharts = [];
    chartModel.agentScheduleType = AgentScheduleType.Scheduling;
    chartModel.modifiedBy = this.authService.getLoggedUserInfo()?.displayName;
    for (let i = 0; i < 7; i++) {
      const weekdays = this.jsonData.filter(x => x.Day === WeekDay[i]);
      if (weekdays.length > 0) {
        const chartData = new AgentScheduleChart();
        chartData.day = i;
        weekdays.forEach((ele, index) => {
          const data = this.schedulingCodes.find(x => x.description === ele.ActivityCode);
          if (data) {
            const chart = new ScheduleChart(ele.StartTime, ele.Endtime, data.id);
            chartData.charts.push(chart);
          }
        });
        chartModel.agentScheduleCharts.push(chartData);
      }
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
    };
    readFile.readAsArrayBuffer(this.fileUploaded);
  }

  private showErrorWarningPopUpMessage(contentMessage: any) {
    const options: NgbModalOptions = { backdrop: 'static', centered: true, size: 'sm' };
    const modalRef = this.modalService.open(ErrorWarningPopUpComponent, options);
    modalRef.componentInstance.headingMessage = 'Error';
    modalRef.componentInstance.contentMessage = contentMessage;
    modalRef.componentInstance.messageType = MessageType.html;

    return modalRef;
  }
}
