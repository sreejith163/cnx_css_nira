import { Component, OnChanges, OnDestroy, OnInit, Output, Injectable, ElementRef, ViewChild, EventEmitter, ChangeDetectorRef, AfterViewInit } from '@angular/core';

import { Subject, SubscriptionLike as ISubscription } from 'rxjs';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ForecastScreenService } from '../../../services/forecast-screen.service';
import { Forecast } from '../../../models/forecast.model';
import { ForecastDataModel } from '../../../models/forecast-data.model';
import { SkillGroupDetails } from '../../../../setup-menu/models/skill-group-details.model';
import { NgbCalendar, NgbDate, NgbDateAdapter, NgbDateStruct, NgbModal, NgbModalOptions, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { MessagePopUpComponent } from 'src/app/shared/popups/message-pop-up/message-pop-up.component';
import { ErrorWarningPopUpComponent } from 'src/app/shared/popups/error-warning-pop-up/error-warning-pop-up.component';
import { ContentType } from 'src/app/shared/enums/content-type.enum';
import { LanguagePreferenceService } from 'src/app/shared/services/language-preference.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { SkillTagService } from '../../../../setup-menu/services/skill-tag.service';
import { TranslateService } from '@ngx-translate/core';
import { ExcelService } from 'src/app/shared/services/excel.service';
import { SkillGroupService } from '../../../../setup-menu/services/skill-group.service';
import { TranslationDetails } from 'src/app/shared/models/translation-details.model';
import { SkillTagDetails } from '../../../../setup-menu/models/skill-tag-details.model';
import { HeaderPagination } from 'src/app/shared/models/header-pagination.model';
import { Constants } from 'src/app/shared/util/constants.util';
import { AgentSchedulesResponse } from '../../../models/agent-schedules-response.model';

import { SkillGroupQueryParameters } from '../../../../setup-menu/models/skill-group-query-parameters.model';

import { SpinnerOptions } from 'src/app/shared/util/spinner-options.util';

import { HttpClient } from '@angular/common/http';

import { UpdateForecastData } from '../../../models/update-forecast-data.model';
import { getLocaleDateTimeFormat } from '@angular/common';

import { ActivatedRoute } from '@angular/router';
import { LanguagePreference } from 'src/app/shared/models/language-preference.model';
import { AuthService } from 'src/app/core/services/auth.service';

import { ForecastExcelData } from '../../../models/forecast-excel.model';
import { NgxCsvParser, NgxCSVParserError } from 'ngx-csv-parser';

import { ForecastScreenDataDetails } from '../../../models/forecast-data-details';
import { ForecastDataResponse } from '../../../models/import-forecast-response';
import { AgentChartResponse } from '../../../models/agent-chart-response.model';
import { ScheduledOpenResponse } from '../../../models/scheduled-open-response.model';
import * as moment from 'moment';
import { debounceTime, distinctUntilChanged, switchMap } from 'rxjs/operators';
import { CursorError } from '@angular/compiler/src/ml_parser/lexer';



@Component({
  selector: 'app-forecast-screen-list',
  templateUrl: './forecast-screen-list.component.html',
  styleUrls: ['./forecast-screen-list.component.scss'],
})

export class ForecastScreenListComponent implements OnInit, OnDestroy, OnChanges {
  @ViewChild('epltable', { static: false }) epltable: ElementRef;
  @Output() dateSelect = new EventEmitter<NgbDateStruct>();
  @ViewChild('epltable') divView: ElementRef;
  isEnabled: boolean[] = [];
  forecastModelBinder: Forecast[];
  forecastBinder: ForecastDataModel;
  pageNumber = 1;
  skillGroupItemsBufferSize = 10;
  numberOfItemsFromEndBeforeFetchingMore = 10;
  characterSplice = 25;
  totalItems = 0;
  totalPages: number;
  searchKeyWord = '';
  dropdownSearchKeyWord = '';
  loading = false;
  skillGroupItemsBuffer: SkillGroupDetails[] = [];
  typeAheadInput$ = new Subject<string>();
  typeAheadValueSubscription: ISubscription;
  getSkillGroupsSubscription: ISubscription;
  getSkillGroupForecast: ISubscription;
  subscriptions: ISubscription[] = [];
  currentPage = 1;
  pageSize = 10;
  currentDate: string;
  paginationSize = Constants.paginationSize;
  translationValues: TranslationDetails[] = [];
  maxLength = Constants.DefaultTextMaxLength;
  orderBy = 'createdDate';
  sortBy = 'desc';
  spinner = 'skillTags';
  importSpinner = "spinner";
  forecastID: number;
  clientId: number;
  clientLobGroupId: number;
  skillTags: SkillTagDetails[] = [];
  totalSkillTagsRecord: number;
  searchKeyword: string;
  headerPaginationValues: HeaderPagination;
  dateValue: string;
  tabIndex: number;
  totalSchedulingGridData: AgentSchedulesResponse[] = [];
  forecastScreenGridData: Forecast[] = [];
  formatsDateTest = 'MM/dd/yyyy';
  dateModel: any;
  today = this.calendar.getToday();

  dateNow: Date = new Date();
  hoveredDate: NgbDate | null = null;
  toDate: NgbDate | null = null;
  fromDate = this.calendar.getToday();
  exportFileName = 'Forecast_Template';
  modalRef: NgbModalRef;
  agentSchedulingGroupId?: number;
  skillGroupObj: SkillGroupDetails;
  enableSaveButton = false;
  enableCancelButton = false;
  enableImportButton = false;
  forecastData: ForecastDataModel[];
  forecastForm: FormGroup;
  forecastFormModel: any = [];
  dataJson: Forecast[] = [];
  skillGroupForecast: ForecastDataModel;
  capabilityForm: FormGroup;
  forecastFormArray = new FormArray([]);
  forecastDataAttribute: Forecast[] = [];
  sumForecastContact: string;
  sumAHT: string;

  sumForecastedReq: string;
  sumScheduledOpen: string;
  avgForecastContact: number;
  avgAHT: number;
  avgForecastedReq: number;
  avgScheduledOpen: number;
  scheduledOpenResponse: ScheduledOpenResponse[] = [];
  avgForecastContactValue: string;
  avgAHTValue: string;
  avgForecastedReqValue: string;
  avgScheduledOpenValue: string;
  forecastSpinner = 'forecastSpinner';
  InsertUpdate = false;
  currentLanguage: string;
  LoggedUser;
  getTranslationSubscription: ISubscription;
  uploadFile: string;
  importForecastData: any;

  jsonData: any[] = [];
  forecastColumns = ['date', 'time', 'forecastedContact', 'aht', 'forecastedReq', 'scheduledOpen'];
  csvTableHeader: string[];
  csvData: any;
  importBtn: boolean;
  importForecastDataModel: ForecastExcelData; 
  dateValidator: boolean;
  allImportForecastData: any = [];
  importForecastDataArray: any = [];
  fileUploaded: File;
  constructor(
    private formBuilder: FormBuilder,
    private forecastService: ForecastScreenService,
    private calendar: NgbCalendar,
    private spinnerService: NgxSpinnerService,
    private languagePreferenceService: LanguagePreferenceService,
    public translate: TranslateService,
    private excelService: ExcelService,
    private modalService: NgbModal,
    private skillGroupService: SkillGroupService,
    private http: HttpClient,
    private route: ActivatedRoute,
    private authService: AuthService,
    private ngxCsvParser: NgxCsvParser,
    private cd: ChangeDetectorRef
  ) {
    this.LoggedUser = this.authService.getLoggedUserInfo();

  }

  calculate(forecastDataCalc: Forecast[]): number {
    return forecastDataCalc.reduce((acc, product) => acc + +product.aht, 0);
  }
  ngOnInit() {
    this.importBtn = false;
    this.setStartDateAsToday();
    this.getForecastDefaultValue();
    this.subscribeToSkillGroups();
    this.subscribeToSearching();
    this.preLoadTranslations();
    this.loadTranslations();
    this.subscribeToTranslations();
    this.cd.detectChanges();

  }

  getScheduledOpenCount(time: string) {
    var convertedTime = moment(time, 'hh:mm A').format('HH:mm:ss')
    var chart =  this.scheduledOpenResponse.find(x => x.time === convertedTime.toString());
    // console.log(chart);
      if (chart) {    
        var obj = {};
        for ( var i=0, len= this.scheduledOpenResponse.length; i < len; i++ ) 
        obj[this.scheduledOpenResponse[i]['time']] = this.scheduledOpenResponse[i];
        this.scheduledOpenResponse = new Array();
        for ( var key in obj )
        this.scheduledOpenResponse.push(obj[key]);
        var parse_string = chart?.scheduleOpen.toString();
        var sched_open_sum = this.scheduledOpenResponse.reduce((a, b) => +a + +b.scheduleOpen,0);
        this.sumScheduledOpen = sched_open_sum.toString();
        this.sumScheduledOpen = parseFloat(this.sumScheduledOpen).toFixed(2);
        
        var sched_open_length = this.scheduledOpenResponse.length;
        this.avgScheduledOpen = parseFloat(this.sumScheduledOpen) / parseFloat(sched_open_length.toString());
       this.avgScheduledOpenValue = parseFloat(this.avgScheduledOpen.toString()).toFixed(2);     
        return parseFloat(parse_string).toFixed(2);
       
      }
      
      return '0.00';
  }


//   exportAsXLSX(): void {

//     const replaceKeys = {
//       time: "Time",
//       forecastedContact: "Forecasted Contacts",
//       aht: "AHT",
//       forecastedReq: "Forecasted Req",
//       scheduledOpen: "Scheduled Open"
//     };
//     for (var i = 0; i < this.dataJson.length; i++)
//       delete this.dataJson[i].scheduledOpen;

//     const newArray = this.changeKeyObjects(this.dataJson, replaceKeys);

//     this.excelService.exportAsExcelFile(newArray, `Forecast-Template`);
//   }

_keyUp(event) {
  if (event.length == 0 && event.which == 48 ){
      return false;
   }
}

  download() {
    let fileName = `ForecastTemplate-${this.skillGroupObj?.name}-${this.convertNgbDateToString(this.dateModel)}.csv`;
    let columnNames = ["Date", "Time", "Forecasted Contact", "AHT", "Forecasted Req"];
    let header = columnNames.join(',');

    let csv = header;
    csv += '\r\n';
  
    let exportData:any = this.dataJson;
    let date = this.convertNgbDateToString(this.dateModel);
    let dateNoSeparators = date.replace(/-/g, '');

    exportData.map(c => {
      let fc = c["forecastedContact"];
      // console.log(fc);
      csv += [dateNoSeparators, c["time"], fc , c["aht"], c["forecastedReq"]].join(',');
      csv += '\r\n';
      // console.log(csv);
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

  checkSkillId() {

    if (this.skillGroupObj?.id == null) {
      this.showErrorWarningPopUpMessage('Please select skill group first');
    }
  }

  enableInput(i) {

    this.isEnabled[i] = false;

  }

  private enableField(datum) {

    return this.formBuilder.group({
      time: this.formBuilder.control({ value: datum.time, disabled: false }),
      forecastedContact: this.formBuilder.control({ value: parseFloat(datum.forecastedContact).toFixed(2), disabled: false }),
      aht: this.formBuilder.control({ value: parseFloat(datum.aht).toFixed(2), disabled: false }),
      forecastedReq: this.formBuilder.control({ value: parseFloat(datum.forecastedReq).toFixed(2), disabled: false }),
      scheduledOpen: this.formBuilder.control({ value: datum.scheduledOpen, disabled: false })
    });
  }

  private generateDatumFormGroup(datum) {

    return this.formBuilder.group({
      time: this.formBuilder.control({ value: datum.time, disabled: false }),
      forecastedContact: this.formBuilder.control({ value: parseFloat(datum.forecastedContact).toFixed(2), disabled: false }),
      aht: this.formBuilder.control({ value: parseFloat(datum.aht).toFixed(2), disabled: false }),
      forecastedReq: this.formBuilder.control({ value: parseFloat(datum.forecastedReq).toFixed(2), disabled: false }),
      scheduledOpen: this.formBuilder.control({ value: '0.00', disabled: false })
    });
  }
 
  get formData() { return this.forecastForm.get('forecastFormArrays') as FormArray; }


  openImportModal(content) {

    // var specific_date = new Date(this.convertNgbDateToString(this.dateModel));
    // var current_date = new Date();

    // if (current_date.getTime() > specific_date.getTime()) {

    //   this.showErrorWarningPopUpMessage('Please select other dates to import a file.');
     
      
    // }
    // else {
      this.modalService.open(content, { centered: true, size: 'lg' });
    // }
   
  }

  ngOnDestroy() {
    this.subscriptions.forEach(subscription => {
      if (subscription) {
        subscription.unsubscribe();
      }
    });
  }

  ngOnChanges() {
    if (this.clientLobGroupId) {
      this.pageNumber = 1;
      this.subscribeToSkillGroups();
    } else {
      this.skillGroupItemsBuffer = [];
      this.totalItems = 0;
    }
  }
  // get today() {
  //   return this.dateAdapter.toModel(this.ngbCalendar.getToday());
  // }

  onSkillGroupScrollToEnd() {
    this.fetchMoreSkillGroups();
  }

  onSkillGroupScroll({ end }) {
    if (this.loading || this.skillGroupItemsBufferSize <= this.skillGroupItemsBuffer.length) {
      return;
    }

    if (end + this.numberOfItemsFromEndBeforeFetchingMore >= this.skillGroupItemsBuffer.length) {
      this.fetchMoreSkillGroups();
    }
  }

  onSkillGroupChange(skillGroup: SkillGroupDetails) {
    this.skillGroupObj = skillGroup;
    this.loadSkillGroup();
  }

  searchForecast(){
    if(this.skillGroupObj == undefined || this.skillGroupObj?.id == null){
      this.showErrorWarningPopUpMessage('Please select skill group first');
    }else{
      this.loadSkillGroup();
    }
  }
  showErrorDate() {
    this.showErrorWarningPopUpMessage('You cannot edit records from previous dates. Please select current date or future dates.');
  }
  loadSkillGroup() {
 

    this.prevDate();
    this.forecastFormArray.reset();
    // var dateParse = `${this.convertNgbDateToString(this.dateModel).month}-${this.convertNgbDateToString(this.dateModel).day}-${this.convertNgbDateToString(this.dateModel).year}`

    this.spinnerService.show(this.forecastSpinner, SpinnerOptions);
    this.getScheduledOpen();
    this.getSkillGroupForecast = this.forecastService.getForecastDataById(this.skillGroupObj?.id, this.convertNgbDateToString(this.dateModel)).subscribe((data) => {
      this.spinnerService.hide(this.forecastSpinner);
      this.forecastID = data.forecastId;
      this.forecastForm = this.formBuilder.group({

        forecastFormArrays: this.formBuilder.array(data.forecastData.map(datum => this.generateDatumFormGroup(datum))),

      });
      
      this.dataJson = data.forecastData;
      this.dataJson.forEach(ele => {
        this.arrayGenerator(
          ele.time,
          ele.forecastedContact,
          ele.aht,
          ele.forecastedReq,
          ele.scheduledOpen
        );
      });
      // console.log(data);
    
      console.log(this.dateValidator);
      this.sumForecastContact = data.forecastData.reduce((a, b) => +a + +b.forecastedContact, 0);
      this.sumAHT = data.forecastData.reduce((a, b) => +a + +b.aht, 0);
      this.sumForecastedReq = data.forecastData.reduce((a, b) => +a + +b.forecastedReq, 0);
      // this.sumScheduledOpen = data.forecastData.reduce((a, b) => +a + +b.scheduledOpen, 0);


      let nonZeroforecastedContact = data.forecastData.map(item => item.forecastedContact).filter(item => (isFinite(item) && item !== '0.00'));
      let nonZeroaht = data.forecastData.map(item => item.aht).filter(item => (isFinite(item) && item !== '0.00'));
      let nonZeroForecastedReq = data.forecastData.map(item => item.forecastedReq).filter(item => (isFinite(item) && item !== '0.00'));

      let nonZeroScheduledOpen = data.forecastData.map(item => item.scheduledOpen).filter(item => (isFinite(item) && item !== '0.00'));
      //sum
      this.sumForecastContact = parseFloat(this.sumForecastContact).toFixed(2);
      this.sumAHT = parseFloat(this.sumAHT).toFixed(2);
      this.sumForecastedReq = parseFloat(this.sumForecastedReq).toFixed(2);
      // this.sumScheduledOpen = parseFloat(this.sumScheduledOpen).toFixed(2);


      //avg

      this.avgForecastContact = parseFloat(this.sumForecastContact) / nonZeroforecastedContact.length;
      this.avgAHT = parseFloat(this.sumAHT) / nonZeroaht.length;
      this.avgForecastedReq = parseFloat(this.sumForecastedReq) / nonZeroForecastedReq.length;
      this.avgScheduledOpen = parseFloat(this.sumScheduledOpen) / nonZeroScheduledOpen.length;

      // parse to string first
      this.avgForecastContactValue = this.avgForecastContact.toString();
      this.avgAHTValue = this.avgAHT.toString();
      this.avgForecastedReqValue = this.avgForecastedReq.toString();
      this.avgScheduledOpenValue = '0.00';


      this.avgAHTValue = parseFloat(this.avgAHTValue).toFixed(2);
      this.avgForecastContactValue = parseFloat(this.avgForecastContactValue).toFixed(2);
      this.avgForecastedReqValue = parseFloat(this.avgForecastedReqValue).toFixed(2);
    
   
    }, (error) => {
     
      this.spinnerService.hide(this.forecastSpinner);
      if (error.status === 404) {
        this.getForecastDefaultValue();

        this.InsertUpdate = true;


      } else {
        this.InsertUpdate = false;
      }
    });

    this.subscriptions.push(this.getSkillGroupForecast);
  }

  // date: { year: number, month: number };

  private getDateInStringFormat(startDate: any): string {
    if (!startDate) {
      return undefined;
    }
    const date = new Date(startDate);
    return date.toDateString();
  }
  change(event) {
    this.enableImportButton = true;
  }


  clearSkillGroupValues() {
    this.searchKeyWord = '';
    this.pageNumber = 1;
    this.subscribeToSkillGroups();
    this.enableImportButton = false;
  }

  private fetchMoreSkillGroups() {
    if (this.pageNumber < this.totalPages) {
      this.pageNumber += 1;
      this.subscribeToSkillGroups(true);
    }
  }

  private subscribeToSkillGroups(needBufferAdd?: boolean) {
    this.loading = true;
    this.getSkillGroupsSubscription = this.getSkillGroups(this.dropdownSearchKeyWord).subscribe(
      response => {
        if (response?.body) {
          this.setPaginationValues(response);
          this.skillGroupItemsBuffer = needBufferAdd ? this.skillGroupItemsBuffer.concat(response.body) : response.body;
        }
        this.loading = false;
      }, err => this.loading = false);

    this.subscriptions.push(this.getSkillGroupsSubscription);
  }

  private subscribeToSearching() {
    this.typeAheadValueSubscription = this.typeAheadInput$.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      switchMap(term => this.getSkillGroups(term))
    ).subscribe(response => {
      if (response.body) {
        this.setPaginationValues(response);
        this.skillGroupItemsBuffer = response.body;
      }
    }, (error) => {
      console.log(error);
    });

    this.subscriptions.push(this.typeAheadValueSubscription);
  }

  private setPaginationValues(response: any) {
    const paging = JSON.parse(response.headers.get('x-pagination'));
    if (paging) {
      this.totalItems = paging.totalCount;
      this.totalPages = paging.totalPages;
    }
  }


  private getQueryParams(searchkeyword?: string) {
    const queryParams = new SkillGroupQueryParameters();

    queryParams.clientId = this.clientId ?? undefined;
    queryParams.clientLobGroupId = this.clientLobGroupId ?? undefined;
    queryParams.pageSize = this.skillGroupItemsBufferSize;
    queryParams.pageNumber = this.pageNumber;
    queryParams.searchKeyword = searchkeyword ?? this.searchKeyWord;
    queryParams.orderBy = undefined;
    queryParams.fields = 'id, name';

    return queryParams;
  }

  private getSkillGroups(searchKeyword?: string) {
    const queryParams = this.getQueryParams(searchKeyword);
    if(this.dropdownSearchKeyWord !== queryParams.searchKeyword) {
      this.pageNumber = 1;
      queryParams.pageNumber = 1;
    }
    this.dropdownSearchKeyWord = queryParams.searchKeyword;
    return this.skillGroupService.getSkillGroups(queryParams);
  }

  private getForecastDefaultValue() {
    this.http.get('/assets/time-table.json')
      .subscribe((data: any[]) => {
        this.forecastForm = this.formBuilder.group({
          forecastFormArrays: this.formBuilder.array(data.map(datum => this.enableField(datum)))
        });

        this.dataJson = data;


        this.sumForecastContact = data.reduce((a, b) => +a + +b.forecastedContact, 0);
        this.sumAHT = data.reduce((a, b) => +a + +b.aht, 0);
        this.sumForecastedReq = data.reduce((a, b) => +a + +b.forecastedReq, 0);
        this.sumScheduledOpen = data.reduce((a, b) => +a + +b.scheduledOpen, 0);

        this.sumForecastContact = parseFloat(this.sumForecastContact).toFixed(2);
        this.sumAHT = parseFloat(this.sumAHT).toFixed(2);
        this.sumForecastedReq = parseFloat(this.sumForecastedReq).toFixed(2);
        this.sumScheduledOpen = parseFloat(this.sumScheduledOpen).toFixed(2);

        this.avgScheduledOpenValue = '0.00';


        this.avgAHTValue = '0.00';
        this.avgForecastContactValue = '0.00';
        this.avgForecastedReqValue = '0.00';
      });
  }

  arrayGenerator(timeValue, forecastedContactValue, ahtValue, forecastedReqValue, scheduledOpenValue) {
    // this.forecastFormArray.reset();
    const group = new FormGroup({
      time: new FormControl(timeValue, Validators.required),
      forecastedContact: new FormControl(forecastedContactValue, Validators.required),
      aht: new FormControl(ahtValue, Validators.required),
      forecastedReq: new FormControl(forecastedReqValue, Validators.required),
      scheduledOpen: new FormControl(scheduledOpenValue, Validators.required)
    });

    this.forecastFormArray.push(group);
  }

  showBtn() {
    this.enableSaveButton = true;
    this.enableCancelButton = true;
  }
hideBtn  () {
    this.enableSaveButton = false;
    this.enableCancelButton = false;
    this.loadSkillGroup();

  }
  private getModalPopup(component: any, size: string) {
    const options: NgbModalOptions = { backdrop: 'static', centered: true, size };
    this.modalRef = this.modalService.open(component, options);
  }
  private setComponentMessages(headingMessage: string, contentMessage: string) {
    this.modalRef.componentInstance.headingMessage = headingMessage;
    this.modalRef.componentInstance.contentMessage = contentMessage;
  }
  private showSuccessPopUpMessage(contentMessage: string, needRefresh = true) {
    this.getModalPopup(MessagePopUpComponent, 'sm');
    this.setComponentMessages('Success', contentMessage);
  }

  private showImportFinished(contentMessage: string, errors: string[], needRefresh = true) {
    this.getModalPopup(MessagePopUpComponent, 'md');
    this.modalRef.componentInstance.headingMessage = 'Import Finished';
    this.modalRef.componentInstance.contentMessage = contentMessage;
    this.modalRef.componentInstance.importErrors = errors;
  }

  private showErrorWarningPopUpMessage(contentMessage: string) {
    const options: NgbModalOptions = { backdrop: 'static', centered: true, size: 'sm' };
    const modalRef = this.modalService.open(ErrorWarningPopUpComponent, options);
    modalRef.componentInstance.headingMessage = 'Error';
    modalRef.componentInstance.contentMessage = contentMessage;
    modalRef.componentInstance.messageType = ContentType.String;

    return modalRef;
  }


  validateDateImport(file){

    this.importForecastDataModel = new ForecastExcelData();

    this.importForecastDataModel.skillGroupId = this.skillGroupObj?.id;
    this.importForecastDataModel.forecastScreenDataDetails = [];

    this.spinnerService.show(this.importSpinner, SpinnerOptions);
      // Parse the file you want to select for the operation along with the configuration
      this.ngxCsvParser.parse(file, { header: false, delimiter: ',' })
      .pipe().subscribe((result: Array<any>) => {
        const csvTableData = [...result.slice(1, result.length)];
        // check the csv first if contents are valid
        this.csvTableHeader = result[0];

        // convert the headers to proper headers
        this.csvTableHeader = Object.assign(this.csvTableHeader, this.forecastColumns);
        for (const ele of csvTableData) {
          const csvJson = new ForecastScreenDataDetails();
          if (ele.length > 0) {
            for (let i = 0; i < ele.length; i++) {
              csvJson[this.csvTableHeader[i]] = ele[i];
            }
          }
          if (csvJson.date) {
            this.importForecastDataModel.forecastScreenDataDetails.push(csvJson);
          }
        }

          this.spinnerService.hide(this.importSpinner);
          this.uploadFile = file.name;

      }, (error: NgxCSVParserError) => {

          this.spinnerService.hide(this.importSpinner);
          this.modalService.dismissAll();
          this.showErrorWarningPopUpMessage('Invalid File Format. Please upload a CSV file only.');
          this.handleClear();
  
        console.log('Error', error);
      });                
  }

  onChangeFile(files: File[]) {

    if(files[0]){
      this.validateDateImport(files[0]);
      this.importBtn = true;
    }

  }


  importForeCastData(){
    this.spinnerService.show(this.importSpinner, SpinnerOptions);
      this.forecastService.importForecastData(this.importForecastDataModel).subscribe((res:ForecastDataResponse)=>{
        this.spinnerService.hide(this.importSpinner);
        this.loadSkillGroup();
        this.modalService.dismissAll();
        this.showImportFinished(res.importStatus, res.errors);
        this.handleClear();
        
      }, err =>{
        this.spinnerService.hide(this.importSpinner);
        this.modalService.dismissAll();
        this.showErrorWarningPopUpMessage('Invalid File contents.');
        this.handleClear();

      });
  }


  changeKeyObjects = (arr, replaceKeys) => {
    return arr.map(item => {
      const newItem = {};
      Object.keys(item).forEach(index => {
        newItem[replaceKeys[index]] = item[index];
      });
      return newItem;
    });
  };

  deleteColnameRecursive(obj) {
    delete obj.scheduledOpen;
    if (obj.children) {
      for (var i = 0; i < obj.children.length; i++)
        this.deleteColnameRecursive(obj.children[i]);
    }
  }

  prevDate() {
    var specific_date = new Date(this.convertNgbDateToString(this.dateModel));
    var current_date = new Date();
    

    if (current_date.getTime() > specific_date.getTime()) {
      this.dateValidator = true;

    } else {
      this.dateValidator = false;
    }
  }

private getScheduledOpen() {
  var date = this.convertNgbDateToString(this.dateModel);
  this.forecastService.getScheduleOpen(this.skillGroupObj?.id,this.getDateInStringFormat(date)).subscribe(response => {
      this.scheduledOpenResponse = response;
      console.log(this.scheduledOpenResponse);
       },
         error => {
           this.scheduledOpenResponse  = []; 
         }
       );
}


  // updateImport() {
  //   const replaceKeys = {
  //     "Time": "Time",
  //     "Forecasted Contact": "forecastedContact",
  //     "AHT": "Aht",
  //     "Forecasted Req": "forecastedReq"
  //   };
  //   const newArrays = this.changeKeyObjects(this.importForecastData, replaceKeys);
  //   let updateForecastData: UpdateForecastData;

  //   let forecastTest: Forecast[];


  //   forecastTest = newArrays;


  //   // var returnedTarget = Object.assign(forecastTest, this.formData.value);
  //   updateForecastData = new UpdateForecastData();
  //   updateForecastData = { forecastData: forecastTest };


  //   this.forecastService.updateForecast(this.forecastID, updateForecastData).subscribe(res => {
  //     this.modalService.dismissAll();
  //     this.showSuccessPopUpMessage('The record has been updated!');
  //     this.handleClear();
  //     this.enableSaveButton = false;
  //     this.enableCancelButton = false;
  //     this.loadSkillGroup();
  //   },
  //     error => {
  //     if(error.status == 400){
  //       this.showErrorWarningPopUpMessage('Data mismatch');
  //     }
  //     }
  //   );
  // }
  handleClear() {
    this.uploadFile = '';
    this.importBtn = false;
  }


  addForecastData() {

    // if (this.InsertUpdate === true) {
      let forecastObjArrays: Forecast[];

      const now = +new Date(this.convertNgbDateToString(this.dateModel));


      // console.log(this.formData.value);
      
      let insertObject: ForecastDataModel;
      // let intDate = new getLocaleDateTimeFormat();
      this.forecastID = this.skillGroupObj?.id;
      // console.log(this.forecastID);
      
      forecastObjArrays = this.formData.value;

      insertObject = {
        ForecastId: this.forecastID,
        Date: this.convertNgbDateToString(this.dateModel),
        SkillGroupId: this.skillGroupObj?.id,
        ForecastData:
          forecastObjArrays
      };

       this.forecastService.addForecast(insertObject).subscribe(res => {


       this.showSuccessPopUpMessage('The record has been added!');
       this.enableSaveButton = false;
       this.enableCancelButton = false;
       this.loadSkillGroup();

      },
        error => {

          if (error.status === 409) {
           this.updateForecast();
          }

        }
     );
    // } else {
     // var forecastObjArrays: Forecast[];

    //   this.updateForecast();
    //  }


  }

  setStartDateAsToday() {
    this.dateModel = this.today;
    // const currentDate = this.setCurrentDate();
  }

  convertNgbDateToString(date) {
    const day = date.day < 10 ? '0' + date.day : date.day;
    const month = date.month < 10 ? '0' + date.month : date.month;

    return date.year + '-' + month + '-' + day;
  }

  updateForecast() {
    let updateForecastData: UpdateForecastData;

    let forecastTest: Forecast[];


    forecastTest = this.formData.value;


    // var returnedTarget = Object.assign(forecastTest, this.formData.value);
    updateForecastData = new UpdateForecastData();
    updateForecastData = { forecastData: forecastTest };


    this.forecastService.updateForecast(this.skillGroupObj?.id, this.convertNgbDateToString(this.dateModel),updateForecastData).subscribe(res => {
      this.showSuccessPopUpMessage('The record has been updated!');
      this.enableSaveButton = false;
      this.enableCancelButton = false;
      this.loadSkillGroup();
    },
      error => {
        this.showErrorWarningPopUpMessage('Error');
      }
    );
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
    this.languagePreferenceService.getLanguagePreference(this.LoggedUser.employeeId).subscribe((langPref: LanguagePreference) => {
      this.currentLanguage = langPref.languagePreference ? langPref.languagePreference : 'en';
      this.translate.use(this.currentLanguage);
    });
  }
}
