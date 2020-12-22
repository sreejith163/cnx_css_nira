import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import * as XLSX from 'xlsx';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
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

  constructor(
    public activeModal: NgbActiveModal,
    private spinnerService: NgxSpinnerService,
    private schedulingCodeService: SchedulingCodeService,
    private agentSchedulesService: AgentSchedulesService,
    private authService: AuthService,
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
      const activityCodes = [];
      this.jsonData.forEach(element => {
        activityCodes.push(element.ActivityCode);
      });
      this.loadSchedulingCodes(activityCodes);
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
          if (activityCodes.length > 0 && this.schedulingCodes.length > 0) {
            this.importAgentScheduleChart();
          }
        }
        this.spinnerService.hide(this.spinner);
      }, (error) => {
        this.spinnerService.hide(this.spinner);
        console.log(error);
      });

    this.subscriptions.push(this.getSchedulingCodesSubscription);
  }

  private importAgentScheduleChart() {
    const model = this.getImportAgentScheduleChartModel();
    this.spinnerService.show(this.spinner, SpinnerOptions);

    this.importAgentScheduleChartSubscription = this.agentSchedulesService.importAgentScheduleChart(this.agentScheduleId, model)
      .subscribe(() => {
        this.spinnerService.hide(this.spinner);
        this.activeModal.close({needRefresh: true});
      }, (error) => {
        this.spinnerService.hide(this.spinner);
        console.log(error);
      });

    this.subscriptions.push(this.importAgentScheduleChartSubscription);
  }

  private getImportAgentScheduleChartModel() {
    const chartArray = new Array<AgentScheduleChart>();
    const chartModel = new UpdateAgentschedulechart();
    chartModel.agentScheduleType = AgentScheduleType.Scheduling;
    chartModel.modifiedBy = this.authService.getLoggedUserInfo()?.displayName;
    this.jsonData.forEach((ele, index) => {
      const data = this.schedulingCodes.find(x => x.description === ele.ActivityCode);
      if (data) {
        const chartData = new AgentScheduleChart();
        const chart = new ScheduleChart(ele.StartTime, ele.Endtime, data.id);
        chartData.day = 0;
        chartData.charts.push(chart);
        chartArray.push(chartData);
      }
    });
    chartModel.agentScheduleCharts = chartArray;

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

}
