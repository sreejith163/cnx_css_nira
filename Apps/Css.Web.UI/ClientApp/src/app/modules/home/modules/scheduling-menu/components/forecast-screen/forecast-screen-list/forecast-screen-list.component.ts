import { Component, Input, EventEmitter, OnChanges, OnDestroy, OnInit, Output, Injectable } from '@angular/core';
import { SortingType } from '../../../enums/sorting-type.enum';

import { Subject, SubscriptionLike as ISubscription } from 'rxjs';
import { fromPromise } from 'rxjs/internal-compatibility';
import { FormArray, FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { ForecastScreenService } from '../../../services/forecast-screen.service';
import { Forecast } from '../../../models/forecast.model';
import { ForecastDataModel } from '../../../models/forecast-data.model';
import { SkillGroupDetails } from '../../../../setup-menu/models/skill-group-details.model';
import { NgbCalendar, NgbDate, NgbDateAdapter, NgbDateStruct, NgbModal, NgbModalOptions, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { MessagePopUpComponent } from 'src/app/shared/popups/message-pop-up/message-pop-up.component';
import { catchError, debounceTime, distinctUntilChanged, switchMap } from 'rxjs/operators';
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
import { AgentScheduleType } from '../../../enums/agent-schedule-type.enum';
import { SkillGroupQueryParameters } from '../../../../setup-menu/models/skill-group-query-parameters.model';
import { ImportScheduleComponent } from '../../scheduling-grid/import-schedule/import-schedule.component';
import { ForecastExcelExportData } from '../../../constants/forecast-excel-export-data';
import { SpinnerOptions } from 'src/app/shared/util/spinner-options.util';
import { stringify } from '@angular/compiler/src/util';
import { element } from 'protractor';
import { HttpClient } from '@angular/common/http';
@Component({
  selector: 'app-forecast-screen-list',
  templateUrl: './forecast-screen-list.component.html',
  styleUrls: ['./forecast-screen-list.component.scss']
})

export class ForecastScreenListComponent implements OnInit, OnDestroy, OnChanges {

  skillgroupID: number;
  forecastModelBinder: Forecast[];
  DateModel: NgbDateStruct;
  pageNumber = 1;
  skillGroupItemsBufferSize = 10;
  numberOfItemsFromEndBeforeFetchingMore = 10;
  characterSplice = 25;
  totalItems = 0;
  totalPages: number;
  searchKeyWord = '';
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
  forecastData: ForecastDataModel[];
  forecastForm: FormGroup;
  forecastFormModel: any = [];
  dataJson: Forecast[] = [];
  skillGroupForecast: ForecastDataModel;
  capabilityForm: FormGroup;
  forecastFormArray = new FormArray([]);
  sumForecastContact: number;
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
    private ngbCalendar: NgbCalendar, private dateAdapter: NgbDateAdapter<string>,
    private http: HttpClient,

  ) {

  }

  calculate(forecastDataCalc: Forecast[]): number {
    return forecastDataCalc.reduce((acc, product) => acc + parseInt(product.AHT), 0)
  }
  ngOnInit() {

    this.getForecastDefaultValue();
    this.tabIndex = AgentScheduleType.Scheduling;
    this.subscribeToSkillGroups();
    this.subscribeToSearching();
    this.http.get('/assets/time-table.json')
      .subscribe((data: any[]) => {
        this.capabilityForm = this.formBuilder.group({
          capabilities: this.formBuilder.array(data.map(datum => this.generateDatumFormGroup(datum)))
        });
      });
  }
  private generateDatumFormGroup1(datum) {
    return this.formBuilder.group({
      Time: this.formBuilder.control({ value: datum.time, disabled: false }),
      ForecastedContact: this.formBuilder.control({ value: datum.forecastedContact, disabled: false }),
      AHT: this.formBuilder.control({ value: datum.aht, disabled: false }),
      ForecastedReq: this.formBuilder.control({ value: datum.forecastedReq, disabled: false }),
      ScheduledOpen: this.formBuilder.control({ value: datum.scheduledOpen, disabled: false })
    });
  }
  private generateDatumFormGroup(datum) {
  
 this.sumForecastContact = this.sumForecastContact + parseFloat(datum.ForecastedContact);
 console.log(this.sumForecastContact);
    return this.formBuilder.group({
      Time: this.formBuilder.control({ value: datum.Time, disabled: false }),
      ForecastedContact: this.formBuilder.control({ value: datum.ForecastedContact, disabled: false }),
      AHT: this.formBuilder.control({ value: datum.AHT, disabled: false }),
      ForecastedReq: this.formBuilder.control({ value: datum.ForecastedReq, disabled: false }),
      ScheduledOpen: this.formBuilder.control({ value: datum.ScheduledOpen, disabled: false })
    });
  }
  get formData() { return <FormArray>this.capabilityForm.get('capabilities'); }

  exportToExcel() {
    const today = new Date();
    const year = String(today.getFullYear());
    const month = String((today.getMonth() + 1)).length === 1 ?
      ('0' + String((today.getMonth() + 1))) : String((today.getMonth() + 1));
    const day = String(today.getDate()).length === 1 ?
      ('0' + String(today.getDate())) : String(today.getDate());

    const date = year + month + day;
    this.excelService.exportAsExcelFile(this.tabIndex === AgentScheduleType.Scheduling ?
      ForecastExcelExportData : ForecastExcelExportData, this.exportFileName + date);
  }

  openImportSchedule() {
    this.getModalPopup(ImportScheduleComponent, 'lg');
    this.modalRef.componentInstance.translationValues = this.translationValues;
    this.modalRef.componentInstance.agentScheduleType = this.tabIndex;

    // this.modalRef.result.then((result) => {
    //   const message = result.partialImport ? 'The record has been paritially imported!' : 'The record has been imported!';
    //   this.getModalPopup(MessagePopUpComponent, 'sm', message);
    //   this.modalRef.result.then(() => {
    //     //this.tabIndex === AgentScheduleType.Scheduling ? this.loadAgentSchedules() : this.loadAgentScheduleManger();
    //   });
    // });
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
  get today() {
    return this.dateAdapter.toModel(this.ngbCalendar.getToday())!;
  }
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

    console.log(skillGroup)
    this.skillGroupBinder = skillGroup;


  }

  loadSkillGroup() {
    this.forecastFormArray.reset()
    var dateParse = `${this.DateModel.month}-${this.DateModel.day}-${this.DateModel.year}`
    this.getSkillGroupForecast = this.forecastService.getForecastDataById(this.skillGroupBinder?.id, dateParse).subscribe((data: any[]) => {
      console.log(data['forecastData'])
      this.capabilityForm = this.formBuilder.group({
     
        capabilities: this.formBuilder.array(data['forecastData'].map(datum => this.generateDatumFormGroup1(datum)))
      });
    }, (error) => {
      console.log(error);
      // if (error.status == 404) {

      // }
    });

    this.subscriptions.push(this.getSkillGroupForecast);
  }

  // date: { year: number, month: number };


  change(event) {
    console.log(this.DateModel);
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
    this.getSkillGroupsSubscription = this.getSkillGroups().subscribe(
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
    return this.skillGroupService.getSkillGroups(queryParams);
  }


  getForecastDefaultValue() {
    this.forecastService.getForecastData().subscribe(res => {
      this.dataJson = res;
      this.dataJson.forEach(element => {
        this.arrayGenerator(
          element.Time,
          element.ForecastedContact,
          element.AHT,
          element.ForecastedReq,
          element.ScheduledOpen
        );
      }
      );

    }
    )
  }

  arrayGenerator(timeValue, forecastedContactValue, ahtValue, forecastedReqValue, scheduledOpenValue) {
    // this.forecastFormArray.reset();
    const group = new FormGroup({
      Time: new FormControl(timeValue, Validators.required),
      ForecastedContact: new FormControl(forecastedContactValue, Validators.required),
      AHT: new FormControl(ahtValue, Validators.required),
      ForecastedReq: new FormControl(forecastedReqValue, Validators.required),
      ScheduledOpen: new FormControl(scheduledOpenValue, Validators.required)
    });

    this.forecastFormArray.push(group);
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
 
  addForecastData() {
    var insertObject: ForecastDataModel;
    var forecastObjArrays: Forecast[];
    var dateParse = `${this.DateModel.month}-${this.DateModel.day}-${this.DateModel.year}`
    forecastObjArrays = this.formData.value;
    insertObject = {
      Date: dateParse,
      SkillGroupId: this.skillGroupBinder?.id,
      ForecastData:
        forecastObjArrays
    };
    this.forecastService.addForecast(insertObject).subscribe(res => {
      this.showSuccessPopUpMessage('The record has been added!');
      this.enableSaveButton = false;
    },
      error => {
        this.showErrorWarningPopUpMessage('Error');
      }
    )
  }

}
