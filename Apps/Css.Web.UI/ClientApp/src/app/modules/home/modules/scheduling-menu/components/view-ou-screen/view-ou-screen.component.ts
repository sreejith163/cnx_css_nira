import { HttpClient } from '@angular/common/http';
import { Component, ElementRef, EventEmitter, Injectable, OnInit, Output, ViewChild } from '@angular/core';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { NgbCalendar, NgbDate, NgbDateAdapter, NgbDateParserFormatter, NgbDateStruct, NgbModal, NgbModalOptions, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { TranslateService } from '@ngx-translate/core';
import { NgxSpinnerService } from 'ngx-spinner';
import { Subject, SubscriptionLike as ISubscription } from 'rxjs';
import { ContentType } from 'src/app/shared/enums/content-type.enum';
import { HeaderPagination } from 'src/app/shared/models/header-pagination.model';
import { TranslationDetails } from 'src/app/shared/models/translation-details.model';
import { ErrorWarningPopUpComponent } from 'src/app/shared/popups/error-warning-pop-up/error-warning-pop-up.component';
import { MessagePopUpComponent } from 'src/app/shared/popups/message-pop-up/message-pop-up.component';
import { ExcelService } from 'src/app/shared/services/excel.service';
import { LanguagePreferenceService } from 'src/app/shared/services/language-preference.service';
import { ActivatedRoute } from '@angular/router';
import { Constants } from 'src/app/shared/util/constants.util';
import { SpinnerOptions } from 'src/app/shared/util/spinner-options.util';
import { SkillGroupDetails } from '../../../setup-menu/models/skill-group-details.model';
import { SkillGroupQueryParameters } from '../../../setup-menu/models/skill-group-query-parameters.model';
import { SkillTagDetails } from '../../../setup-menu/models/skill-tag-details.model';
import { SkillGroupService } from '../../../setup-menu/services/skill-group.service';
import { SkillTagService } from '../../../setup-menu/services/skill-tag.service';
import { AgentSchedulesResponse } from '../../models/agent-schedules-response.model';
import { ForecastDataModel } from '../../models/forecast-data.model';
import { Forecast } from '../../models/forecast.model';
import { ForecastScreenService } from '../../services/forecast-screen.service';
import { LanguagePreference } from 'src/app/shared/models/language-preference.model';
import { AuthService } from 'src/app/core/services/auth.service';
import { ScheduledOpenResponse } from '../../models/scheduled-open-response.model';
import * as moment from 'moment';
import { debounceTime, distinctUntilChanged, switchMap } from 'rxjs/operators';



@Component({
  selector: 'app-view-ou-screen',
  templateUrl: './view-ou-screen.component.html',
  styleUrls: ['./view-ou-screen.component.scss'],
  
})
export class ViewOuScreenComponent implements OnInit {

  @ViewChild('epltable', { static: false }) epltable: ElementRef;
  @ViewChild("epltable") divView: ElementRef;
  @Output() dateSelect = new EventEmitter<NgbDateStruct>();
  isEnabled: boolean[] = [];
  model2: string;
  skillgroupID: number;
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
  scheduledOpenResponse: ScheduledOpenResponse[] = [];
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
  dateModel: any;
  today = this.calendar.getToday();

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
  formatsDateTest: string = "MM/dd/yyyy";
  startDate: any = this.calendar.getToday();
  dateNow: Date = new Date();
  hoveredDate: NgbDate | null = null;
  toDate: NgbDate | null = null;
  fromDate = this.calendar.getToday();
  exportFileName = 'Forecast_Template';
  modalRef: NgbModalRef;
  agentSchedulingGroupId?: number;
  skillGroupBinder: SkillGroupDetails;
  enableSaveButton: boolean = false;
  enableImportButton: boolean = false;
  forecastData: ForecastDataModel[];
  forecastForm: FormGroup;
  forecastFormModel: any = [];
  dataJson: Forecast[] = [];
  skillGroupForecast: ForecastDataModel;
  capabilityForm: FormGroup;
  forecastFormArray = new FormArray([]);
  sumForecastContact: string;
  sumAHT: string;
  sumOU: string;
  sumOUInt: number;
  sumForecastedReq: string;
  sumScheduledOpen: string;
  forecastSpinner = 'forecastSpinner';
  InsertUpdate = false;
  OU: any;
  currentLanguage: string;
  LoggedUser;
  getTranslationSubscription: ISubscription;
  avgForecastContact: number;
  avgAHT: number;
  avgForecastedReq: number;
  avgScheduledOpen: number;
avgOU: number;
  avgForecastContactValue: string;
  avgAHTValue: string;
  avgForecastedReqValue: string;
  avgScheduledOpenValue: string;
  avgOUValue: string;
  constructor(
    private formBuilder: FormBuilder,
    private forecastService: ForecastScreenService,

    private calendar: NgbCalendar,
    private spinnerService: NgxSpinnerService,
    private skillTagSevice: SkillTagService,
    private languagePreferenceService: LanguagePreferenceService,
    public translate: TranslateService,
    private excelService: ExcelService,
    private modalService: NgbModal,
    private skillGroupService: SkillGroupService,
    private ngbCalendar: NgbCalendar,
    private dateAdapter: NgbDateAdapter<string>,
    private http: HttpClient,
    private route: ActivatedRoute,
    private authService: AuthService


  ) {
    this.LoggedUser = this.authService.getLoggedUserInfo();
  }

  calculate(forecastDataCalc: Forecast[]): number {
    return forecastDataCalc.reduce((acc, product) => acc + product.aht, 0)
  }
  ngOnInit() {
    this.setStartDateAsToday();
    this.getForecastDefaultValue();
    this.subscribeToSkillGroups();
    this.subscribeToSearching();
    this.preLoadTranslations();
    this.loadTranslations();
    this.subscribeToTranslations();


  }
  setStartDateAsToday() {
    this.dateModel = this.today;

    // const currentDate = this.setCurrentDate();
  }


  checkSkillId() {

    if (this.skillGroupBinder?.id == null) {
      this.showErrorWarningPopUpMessage('Please select skill group first');
    }
  }
  enableInput(i) {

    this.isEnabled[i] = false;

  }

  private enableField(datum) {

    return this.formBuilder.group({
      time: this.formBuilder.control({ value: datum.time, disabled:  true  }),
      forecastedContact: this.formBuilder.control({ value: datum.forecastedContact, disabled:  true  }),
      aht: this.formBuilder.control({ value: datum.aht, disabled:  true  }),
      forecastedReq: this.formBuilder.control({ value: datum.forecastedReq, disabled:  true }),
      scheduledOpen: this.formBuilder.control({ value: '0.00', disabled: true })
    });
  }

  private generateDatumFormGroup(datum) {

    return this.formBuilder.group({
      time: this.formBuilder.control({ value: datum.time, disabled: true }),
      forecastedContact: this.formBuilder.control({ value: datum.forecastedContact, disabled: true }),
      aht: this.formBuilder.control({ value: datum.aht, disabled: true }),
      forecastedReq: this.formBuilder.control({ value: datum.forecastedReq, disabled: true}),
      scheduledOpen: this.formBuilder.control({ value: 0, disabled: true })
    });
  }
  get formData() { return <FormArray>this.forecastForm.get('forecastFormArrays'); }






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


    this.skillGroupBinder = skillGroup;
    this.loadSkillGroup();


  }

  private getForecastDefaultValue() {
    this.http.get('/assets/time-table.json').subscribe((response: any) => {
      if (response) {
        this.dataJson = response;


      }

      this.sumForecastContact = response.reduce((a, b) => +a + +b.forecastedContact, 0);
      this.sumAHT = response.reduce((a, b) => +a + +b.aht, 0);
      this.sumForecastedReq = response.reduce((a, b) => +a + +b.forecastedReq, 0);
      this.sumScheduledOpen = response.reduce((a, b) => +a + +b.scheduledOpen, 0);

      this.sumForecastContact = parseFloat(this.sumForecastContact).toFixed(2);
      this.sumAHT = parseFloat(this.sumAHT).toFixed(2);
      this.sumForecastedReq = parseFloat(this.sumForecastedReq).toFixed(2);
      this.sumScheduledOpen = parseFloat(this.sumScheduledOpen).toFixed(2);
      this.sumOU = '0.00';
      this.avgAHT = 0;
      this.avgForecastedReq = 0;
      this.avgScheduledOpen =  0;
      this.avgOU = 0;
    // parse to string first
    this.avgAHTValue = parseFloat('0.00').toFixed(2);
    this.avgForecastContactValue = parseFloat('0.00').toFixed(2);
    this.avgForecastedReqValue = parseFloat('0.00').toFixed(2);
    this.avgScheduledOpenValue = parseFloat('0.00').toFixed(2);
    this.avgOUValue = parseFloat(this.avgOU.toString()).toFixed(2);
    }, (error) => {
      this.spinnerService.hide(this.spinner);
      console.log(error);
    });


    // .subscribe((data: any[]) => {
    //   this.forecastForm = this.formBuilder.group({

    //   });

    //   this.dataJson = data;


    //   this.sumForecastContact = data.reduce((a, b) => +a + +b.forecastedContact, 0);
    //   this.sumAHT = data.reduce((a, b) => +a + +b.aht, 0);
    //   this.sumForecastedReq = data.reduce((a, b) => +a + +b.forecastedReq, 0);
    //   this.sumScheduledOpen = data.reduce((a, b) => +a + +b.scheduledOpen, 0);

    //   this.sumForecastContact = parseFloat(this.sumForecastContact).toFixed(2)
    //   this.sumAHT = parseFloat(this.sumAHT).toFixed(2)
    //   this.sumForecastedReq = parseFloat(this.sumForecastedReq).toFixed(2)
    //   this.sumScheduledOpen = parseFloat(this.sumScheduledOpen).toFixed(2)
    // });
  }

  private getScheduledOpen() {

    var date = this.convertNgbDateToString(this.dateModel);
    this.forecastService.getScheduleOpen(this.skillGroupBinder?.id, this.getDateInStringFormat(date)).subscribe(response => {
      if (response != "No scheduled open") {
        this.scheduledOpenResponse = response;
        this.scheduledOpenResponse.map(x => {
          const timeLapses = [":05", ":10", ":15", ":20", ":25", ":35", ":40", ":45", ":50", ":55"];
          timeLapses.some(i => {
            if (x.time.includes(i))
              x.scheduleOpen = .5;
          }
          )
        }
        );
        const x = this.scheduledOpenResponse;
        x.map(time => {
          const roundDownTo = roundTo => x => Math.floor(x / roundTo) * roundTo;
          const roundUpTo = roundTo => x => Math.ceil(x / roundTo) * roundTo;
          const roundDownTo5Minutes = roundDownTo(1000 * 60 * 30);
          const roundUpTo5Minutes = roundUpTo(1000 * 60 * 30);
          var dt = moment(time.time, ["h:mm A"]).format("HH:mm");

          var datenow = new Date(date + " " + dt)
          const now = new Date(datenow)
          const msdown = roundDownTo5Minutes(now)
          const msup = roundUpTo5Minutes(now)
          time.time = moment(msup).format("h:mm A");

        });

        var result = [];
        x.reduce(function (res, value) {
          if (!res[value.time]) {
            res[value.time] = { time: value.time, scheduleOpen: 0 };
            result.push(res[value.time])
          }
          res[value.time].scheduleOpen += value.scheduleOpen;
          return res;
        }, {});



        this.scheduledOpenResponse = result;
        // var obj = {};
        // for (var i = 0, len = this.scheduledOpenResponse.length; i < len; i++)
        //   obj[this.scheduledOpenResponse[i]['time']] = this.scheduledOpenResponse[i];
        // this.scheduledOpenResponse = new Array();
        // for (var key in obj)
        //   this.scheduledOpenResponse.push(obj[key]);

        var sched_open_sum = this.scheduledOpenResponse.reduce((a, b) => +a + +b.scheduleOpen, 0);
        this.sumScheduledOpen = sched_open_sum.toString();
        this.sumScheduledOpen = parseFloat(this.sumScheduledOpen).toFixed(2);

        var sched_open_length = this.scheduledOpenResponse.length;
        this.avgScheduledOpen = parseFloat(this.sumScheduledOpen) / parseFloat(sched_open_length.toString());
        this.avgScheduledOpenValue = parseFloat(this.avgScheduledOpen.toString()).toFixed(2);
     
      }
      else {
        this.scheduledOpenResponse = [];
        this.sumScheduledOpen = "0.00";
        this.avgScheduledOpenValue = "0.00";
      }
    },
      error => {
        this.scheduledOpenResponse = [];
        console.log(error)
        this.sumScheduledOpen = "0.00";
        this.avgScheduledOpenValue = "0.00";
      }
    );
  }
  private getDateInStringFormat(startDate: any): string {
    if (!startDate) {
      return undefined;
    }
    const date = new Date(startDate);
    return date.toDateString();
  }
  getScheduledOpenCount1(time: string) {
  
  
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
  
      
        var parse_string = chart?.scheduleOpen;
        var sched_open_sum = this.scheduledOpenResponse.reduce((a, b) => +a + +b.scheduleOpen, 0);
        this.sumScheduledOpen = sched_open_sum.toString();
      
  
        
        return parse_string;
        
      }
  
      return 0;
  }
  getScheduledOpenCount(time: string) {
    var chart = this.scheduledOpenResponse?.find(x => x.time === time.toString());
    if (chart) {
      var parse_string = chart?.scheduleOpen.toString();
      return parseFloat(parse_string).toFixed(2);
    }
    return '0.00';
  }
  loadForecast() {
    const queryParams = this.getQueryParams();
    this.spinnerService.show(this.spinner, SpinnerOptions);

    this.getSkillGroupForecast = this.forecastService.getForecastDataById(this.skillGroupBinder?.id, this.convertNgbDateToString(this.dateModel))
      .subscribe((response: any[]) => {
        if (response) {
          this.dataJson = response['forecastData'];


        }
        this.sumForecastContact = response['forecastData'].reduce((a, b) => +a + +b.forecastedContact, 0);
        this.sumAHT = response['forecastData'].reduce((a, b) => +a + +b.aht, 0);
        this.sumForecastedReq = response['forecastData'].reduce((a, b) => +a + +b.forecastedReq, 0);
        this.sumScheduledOpen = response['forecastData'].reduce((a, b) => +a + +b.scheduledOpen, 0);


        this.sumForecastContact = parseFloat(this.sumForecastContact).toFixed(2)
        this.sumAHT = parseFloat(this.sumAHT).toFixed(2)
        this.sumForecastedReq = parseFloat(this.sumForecastedReq).toFixed(2)
        this.sumScheduledOpen = parseFloat(this.sumScheduledOpen).toFixed(2)
      
        this.spinnerService.hide(this.spinner);
      }, (error) => {
        this.spinnerService.hide(this.spinner);
        if (error.status == 404) {
          this.getForecastDefaultValue();
        
          this.InsertUpdate = true;


        } else {
          this.InsertUpdate = false;
        }
      });

    this.subscriptions.push(this.getSkillGroupForecast);
  }
  loadSkillGroup() {
    this.forecastFormArray.reset();
    // var dateParse = `${this.DateModel.month}-${this.DateModel.day}-${this.DateModel.year}`
    this.getScheduledOpen();
    this.spinnerService.show(this.forecastSpinner, SpinnerOptions);
    this.getSkillGroupForecast = this.forecastService.getForecastDataById(this.skillGroupBinder?.id, this.convertNgbDateToString(this.dateModel)).subscribe((data) => {
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
     
      this.sumForecastContact = data.forecastData.reduce((a, b) => +a + +b.forecastedContact, 0);
      this.sumAHT = data.forecastData.reduce((a, b) => +a + +b.aht, 0);
      this.sumForecastedReq = data.forecastData.reduce((a, b) => +a + +b.forecastedReq, 0);
      //this.sumScheduledOpen = data.forecastData.reduce((a, b) => +a + +b.scheduledOpen, 0);
     
     
      let nonZeroforecastedContact = data.forecastData.map(item => item.forecastedContact).filter(item => (isFinite(item) && item!=='0.00'));
      let nonZeroaht= data.forecastData.map(item => item.aht).filter(item => (isFinite(item) && item!=='0.00'));
      let nonZeroForecastedReq = data.forecastData.map(item => item.forecastedReq).filter(item => (isFinite(item) && item!=='0.00'));
    
      let nonZeroScheduledOpen = data.forecastData.map(item => item.scheduledOpen).filter(item => (isFinite(item) && item!=='0.00'));
     //sum
      this.sumForecastContact = parseFloat(this.sumForecastContact).toFixed(2);
      this.sumAHT = parseFloat(this.sumAHT).toFixed(2);
      this.sumForecastedReq = parseFloat(this.sumForecastedReq).toFixed(2);
      //this.sumScheduledOpen = parseFloat(this.sumScheduledOpen).toFixed(2);

      let sumHolder = parseFloat(this.sumScheduledOpen) - parseFloat(this.sumForecastedReq);
     
      this.sumOU = parseFloat(sumHolder.toString()).toFixed(2); 
      this.sumOUInt = parseFloat(this.sumOU)
      //avg

    this.avgForecastContact  = parseFloat(this.sumForecastContact) / nonZeroforecastedContact.length;
      this.avgAHT = parseFloat(this.sumAHT) / nonZeroaht.length;
      this.avgForecastedReq = parseFloat(this.sumForecastedReq) / nonZeroForecastedReq.length;
      this.avgScheduledOpen = parseFloat(this.sumScheduledOpen) / nonZeroScheduledOpen.length;

      this.avgOU  = parseFloat(this.avgScheduledOpen.toString()) - parseFloat(this.avgForecastedReq.toString());
      
      

      this.avgOUValue = parseFloat(this.avgOU.toString()).toFixed(2)

    // parse to string first
    this.avgForecastContactValue = this.avgForecastContact.toString();
    this.avgAHTValue = this.avgAHT.toString();
    this.avgForecastedReqValue = this.avgForecastedReq.toString();

    
    this.avgAHTValue = parseFloat(this.avgAHTValue).toFixed(2);
    this.avgForecastContactValue = parseFloat(this.avgForecastContactValue).toFixed(2);
    this.avgForecastedReqValue = parseFloat(this.avgForecastedReqValue).toFixed(2);


    }, (error) => {

      this.spinnerService.hide(this.forecastSpinner);
      if (error.status === 404) {
        this.getForecastDefaultValue();
        this.sumOUInt = 0;
        this.InsertUpdate = true;


      } else {
        this.InsertUpdate = false;
      }
    });

    this.subscriptions.push(this.getSkillGroupForecast);
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
  // date: { year: number, month: number };


  change(event) {
    this.enableImportButton = true;
  }


  convertNgbDateToString(date) {
    const day = date.day < 10 ? '0' + date.day : date.day;
    const month = date.month < 10 ? '0' + date.month : date.month;

    return date.year + '-' + month + '-' + day;
  }
  clearSkillGroupValues() {
    this.searchKeyWord = '';
    this.pageNumber = 1;
    this.subscribeToSkillGroups();
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





  showBtn() {
    this.enableSaveButton = true;
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

  private showErrorWarningPopUpMessage(contentMessage: string) {
    const options: NgbModalOptions = { backdrop: 'static', centered: true, size: 'sm' };
    const modalRef = this.modalService.open(ErrorWarningPopUpComponent, options);
    modalRef.componentInstance.headingMessage = 'Error';
    modalRef.componentInstance.contentMessage = contentMessage;
    modalRef.componentInstance.messageType = ContentType.String;

    return modalRef;
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
