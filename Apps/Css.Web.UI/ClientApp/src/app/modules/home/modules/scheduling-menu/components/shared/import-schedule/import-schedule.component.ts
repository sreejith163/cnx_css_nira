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
import { ImportScheduleChart, ImportScheduleGridData, SchedulingGridExcelScheduleData, ShedulingGridImportModel } from '../../../models/import-schedule-chart.model';
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
import { NgxCsvParser, NgxCSVParserError } from 'ngx-csv-parser';
import * as moment from 'moment';
import { HttpEventType, HttpResponse } from '@angular/common/http';

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
  schedulinGridImportModel: ShedulingGridImportModel;
  csvData: any[] = [];
  scheduleColumns = ['EmployeeId', 'StartDate', 'EndDate', 'ActivityCode', 'startTime', 'endTime'];
  schedulingManagerColumns = ['EmployeeId', 'Date', 'ActivityCode', 'startTime', 'endTime'];
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
    private papa: Papa,
    private ngxCsvParser: NgxCsvParser
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
    if (!this.uploadFile) {
      this.fileFormatValidation = true;
      return;
    }
    // if (this.validateImportDatafields()) {
    //   return;
    // }
    // if (this.agentScheduleType === AgentScheduleType.Scheduling) {
    //   if (this.checkDateRange()) {
    //     return;
    //   }
    // }

    this.fileSubmitted = true;
    if (this.csvData.length > 0) {
      // this.csvData.map((ele) => {
      //   if (ele.startTime.split(':')[0].length === 1) {
      //     ele.startTime = '0' + ele.startTime.split(':')[0] + ':' + ele.startTime.split(':')[1];
      //   }
      //   if (ele.startTime.split(':')[0] === '12') {
      //     ele.startTime = '00' + ':' + ele.startTime.split(':')[1];
      //   }
      //   if (ele.endTime.split(':')[0].length === 1) {
      //     ele.endTime = '0' + ele.endTime.split(':')[0] + ':' + ele.endTime.split(':')[1];
      //   }
      //   if (ele.endTime.split(':')[0] === '12') {
      //     ele.endTime = '00' + ':' + ele.endTime.split(':')[1];
      //   }
      //   if (ele.endTime.trim().toLowerCase() === '12:00 am' || ele.endTime.trim().toLowerCase() === '00:00 am') {
      //     ele.endTime = '11:60 pm';
      //   }
      // });
      // if (!this.fileFormatValidation && !this.validateHeading()) {
        
      if (!this.fileFormatValidation) {
          const employees = new Array<string>();
          this.csvData.forEach(data => {
            if (employees.filter(x => x === data.EmployeeId).length === 0) {
              employees.push(data.EmployeeId);
            }
          });
          // console.log(this.jsonData)
          this.loadAgentSchedules(employees);
      } else {
        this.showErrormessage();
      }
    } else {
      this.showErrormessage();
    }
  }

  hasFileSelected() {
    if (this.fileSubmitted) {
      return this.uploadFile ? true : false;
    }
    return true;
  }

  browse(files: any) {
    this.fileFormatValidation = false;
    this.csvData = [];
    this.fileUploaded = files[0];
    this.uploadFile = this.fileUploaded?.name;
    if (this.uploadFile.split('.')[1].toLowerCase() === 'csv'.toLowerCase()) {
      this.readCsvFile();
    } else {
      this.fileFormatValidation = true;
    }
  }

  private validateImportDatafields() {
   
    for (const item of this.csvData) {
      if (!item.startTime || !item.endTime || !item.ActivityCode || !item.EmployeeId) {
        this.showErrormessage();
        return true;
      } else {
        if (this.agentScheduleType === AgentScheduleType.Scheduling) {
          if (!item.StartDate || !item.EndDate) {
            // console.log(item)
            this.showErrormessage();
            return true;
          }
        } else {
          if (!item.Date) {
            this.showErrormessage();
            return true;
          }
        }
      }
    }
  }

  private showErrormessage() {
    const errorMessage = `“An error occurred upon importing the file. Please check the following”<br>Duplicated Record<br>Incorrect Columns<br>Invalid Date Range and Time<br>Not recognized Employee ID`;
    this.showErrorWarningPopUpMessage(errorMessage);
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
      } 
    }
  }


  private readCsvFile() {

    let scheduleGridColumns = ['EmployeeId', 'StartDate', 'EndDate', 'ActivityCode', 'startTime', 'endTime'];
    
    // Parse the file you want to select for the operation along with the configuration
    this.ngxCsvParser.parse(this.fileUploaded, { header: false, delimiter: ',' })
    .pipe().subscribe((result: Array<any>) => {
      const csvTableData = [...result.slice(1, result.length)];
      // check the csv first if contents are valid
      this.csvTableHeader = result[0];

      // convert the headers to proper headers
      this.csvTableHeader = Object.assign(this.csvTableHeader, scheduleGridColumns);
      for (const ele of csvTableData) {
        const csvJson = new SchedulingGridExcelScheduleData();
        if (ele.length > 0) {
          for (let i = 0; i < ele.length; i++) {
            csvJson[this.csvTableHeader[i]] = ele[i];              
          }
        } 

        if (csvJson.StartDate !== '' && csvJson.ActivityCode !== '' && csvJson.EmployeeId !== ''
        && csvJson.EndDate !== '' && csvJson.endTime !== '' && csvJson.startTime !=='') {
          this.csvData.push(csvJson);
        }
      }


      this.csvData.map(x => {
        x.StartDate = x?.StartDate.slice(0, 4) + '/' + x?.StartDate.slice(4, 6) + '/' + x?.StartDate.slice(6, 8);
        x.EndDate = x?.EndDate.slice(0, 4) + '/' + x?.EndDate.slice(4, 6) + '/' + x?.EndDate.slice(6, 8);
        x.startTime = x?.startTime.trim().toLowerCase();
        x.endTime = x?.endTime.trim().toLowerCase();

        // if (x?.endTime?.trim()?.toLowerCase()?.slice(0, 2) === '00') {
        //   x.endTime = '12' + x?.endTime?.trim()?.toLowerCase()?.slice(2, 8);
        // }
        // if (x?.startTime?.trim()?.toLowerCase()?.slice(0, 2) === '00') {
        //   x.startTime = '12' + x?.startTime?.trim()?.toLowerCase()?.slice(2, 8);
        // }

        x.ActivityCode = x?.ActivityCode.trim().toLowerCase();

        if((x.StartDate !== x.EndDate) && 
            moment(x.endTime, ["h:mm a"]).format("HH:mm") > 
            moment("12:00 am", ["h:mm a"]).format("HH:mm")
          ){
          
          // if (x?.endTime === '11:60 pm') {
          //   x.endTime = '12:00 am';
          // }

          const originalEndTime = x.endTime;
          x.endTime = "12:00 am";

          let halfSched:SchedulingGridExcelScheduleData = new SchedulingGridExcelScheduleData();
          halfSched.startTime = x.endTime;
          halfSched.endTime = originalEndTime;
          halfSched.EmployeeId = x.EmployeeId;
          halfSched.StartDate = x?.EndDate;
          halfSched.EndDate = x?.EndDate;
          halfSched.ActivityCode = x?.ActivityCode;
          
          this.csvData.push(halfSched);
        }
      });

      
      this.csvData.map(x => {
        x.startTime = moment(x.startTime, ["h:mm a"]).format("hh:mm a");
        x.endTime = moment(x.endTime, ["h:mm a"]).format("hh:mm a");
      });

      this.csvData = this.csvData.filter(x=> x.startTime !== x.endTime);

    }, (error: NgxCSVParserError) => {

      this.modalService.dismissAll();
      this.showErrorWarningPopUpMessage('Invalid File Format. Please upload a CSV file only.');

    });    
    
  }

 
  exportErrorListFromImport(errorList){
    if(errorList !== undefined){
      let csv = "Errors";
      csv += '\r\n';
      errorList.forEach(element => {
        csv += element;
        csv += '\r\n';
      });

      var blob = new Blob([csv], { type: "text/csv" });
      var link = document.createElement("a");
      if (link.download !== undefined) {
        var url = URL.createObjectURL(blob);
        link.setAttribute("href", url);
        link.setAttribute("download", "ImportErrorList.csv");
        document.body.appendChild(link);
        link.click();
        document.body.removeChild(link);
      }
    }
  }
 

  private importAgentScheduleChart(scheduleResponse: AgentSchedulesResponse[], schedulingCodes: SchedulingCode[], hasMismatch?: boolean) {
    const importModelArray = this.shapeImportModel(schedulingCodes);

    // check if modelArray has value
    if(importModelArray !== undefined){
      let importFinalModel = new ShedulingGridImportModel();
      importFinalModel.agentScheduleImportData = importModelArray;
      importFinalModel.activityOrigin = ActivityOrigin.CSS;
      importFinalModel.modifiedBy = this.authService.getLoggedUserInfo()?.displayName;

        this.spinnerService.show(this.spinner, SpinnerOptions);
        this.importAgentScheduleChartSubscription = this.agentSchedulesService.importAgentScheduleChart(importFinalModel)
          .subscribe((res:any) => {
            this.spinnerService.hide(this.spinner);
            this.activeModal.close({ partialImport: hasMismatch });
          }, (error) => {
            this.spinnerService.hide(this.spinner);
            const errorMessage = `An error occurred upon importing the file. Please check the following<br>Duplicated Record<br>Incorrect Columns<br>Invalid Date Range and Time<br>Not recognized Employee ID`;

            // this.exportErrorListFromImport(error.error);
            this.showErrorWarningPopUpMessage(errorMessage);
          });
        this.subscriptions.push(this.importAgentScheduleChartSubscription);
    }else{
      // based on the scheduling code validation,
      // return activity code error if model is undefined undefined
      this.spinnerService.hide(this.spinner);
      const errorMessage = `Invalid Activity Code(s) detected.<br>Please check your file and try again.`;
      this.showErrorWarningPopUpMessage(errorMessage);
    }
  }



  private loadAgentSchedules(employees: string[]) {
    let agentSchedule;
    let agentManagerSchedule;
    const activityCodes = Array<string>();

    this.csvData.forEach(element => {
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
          // this.agentScheduleType === AgentScheduleType.Scheduling ?
            this.importAgentScheduleChart(scheduleRepsonse, schedulingCodesResponse, hasMismatch);
            // :
            // this.updateManagerChart(scheduleRepsonse, schedulingCodesResponse, hasMismatch);
        } else {
          const errorMessage = `An error occurred upon importing the file. Please check the following<br>Duplicated Record<br>Incorrect Columns<br>Invalid Date Range and Time<br>Not recognized Employee ID`;
          this.showErrorWarningPopUpMessage(errorMessage);
        }
      }, error => {
        this.spinnerService.hide(this.spinner);
        console.log(error);
        this.showErrormessage();
      });
  }

  private shapeImportModel(schedulingCodes: SchedulingCode[]){
    const importCsvData = this.csvData;
    let importModelArray: ImportScheduleGridData[] = [];
    
    // get the original length of the csvData
    // this will be used for scheduling code validation
    let csvLength = importCsvData.length;

    importCsvData.map(x => {
        let importObj = new ImportScheduleGridData();
        importObj.startDate = x?.StartDate;
        importObj.endDate = x?.EndDate;
        importObj.startTime = x?.startTime;
        importObj.endTime = x?.endTime;
        // if the activity code provided is invalid, give undefined value as default
        // then filter the array by valid scheduling code ids
        const schedCode = schedulingCodes.find(c => c.description.trim().toLowerCase() === x?.ActivityCode.trim().toLowerCase());
        importObj.schedulingCodeId = schedCode ? schedCode.id : undefined;
        importObj.employeeId = x?.EmployeeId;

        importModelArray.push(importObj);        
      });
      
      // filter the array by scheduling code
      // remove all the items with undefined schedulingCodeId
      var filtered = importModelArray.filter(x => x.schedulingCodeId !== undefined);

      // compare the original csvLength with filtered length
      // if unequal, return undefined
      if(csvLength !== filtered.length){
          importModelArray = undefined;
      }

    return importModelArray;
  }



  private getAdjacentDate(date: Date, count: number) {
    const endDate = date;
    const nextDate = new Date(endDate);
    nextDate.setDate(endDate.getDate() + count);
    const adjacentDate  = this.datepipe.transform(nextDate, 'yyyy/MM/dd');
    return adjacentDate;
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
      console.log(time)
      return time;
    }
  }
}