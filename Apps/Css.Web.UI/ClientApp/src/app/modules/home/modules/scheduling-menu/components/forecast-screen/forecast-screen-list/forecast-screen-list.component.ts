import { Component, Input, EventEmitter, OnChanges, OnDestroy, OnInit, Output, Injectable, ElementRef, ViewChild } from '@angular/core';
import { SortingType } from '../../../enums/sorting-type.enum';

import { Subject, SubscriptionLike as ISubscription } from 'rxjs';
import { fromPromise } from 'rxjs/internal-compatibility';
import { FormArray, FormBuilder, FormControl, FormGroup, NumberValueAccessor, Validators } from '@angular/forms';
import { ForecastScreenService } from '../../../services/forecast-screen.service';
import { Forecast } from '../../../models/forecast.model';
import { ForecastDataModel } from '../../../models/forecast-data.model';
import { SkillGroupDetails } from '../../../../setup-menu/models/skill-group-details.model';
import { NgbCalendar, NgbDate, NgbDateAdapter, NgbDateParserFormatter, NgbDateStruct, NgbModal, NgbModalOptions, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
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
import { ImportScheduleComponent } from '../../shared/import-schedule/import-schedule.component';
import { ForecastExcelExportData } from '../../../constants/forecast-excel-export-data';
import { SpinnerOptions } from 'src/app/shared/util/spinner-options.util';
import { stringify } from '@angular/compiler/src/util';
import { element } from 'protractor';
import { HttpClient } from '@angular/common/http';
import { UpdateAgentAdmin } from '../../../models/update-agent-admin.model';
import { UpdateForecastData } from '../../../models/update-forecast-data.model';
import { getLocaleDateTimeFormat } from '@angular/common';
import * as xlsx from 'xlsx';
/**
 * This Service handles how the date is represented in scripts i.e. ngModel.
 */
@Injectable()
export class CustomAdapter extends NgbDateAdapter<string> {

  readonly DELIMITER = '/';

  fromModel(value: string | null): NgbDateStruct | null {
    if (value) {
      const date = value.split(this.DELIMITER);
      return {
        month: parseInt(date[0], 10),
        day: parseInt(date[1], 10),
        year: parseInt(date[2], 10)
      };
    }
    return null;
  }

  toModel(date: NgbDateStruct | null): string | null {
    return date ? date.month + this.DELIMITER + date.day + this.DELIMITER + date.year : null;
  }
}

@Injectable()
export class CustomDateParserFormatter extends NgbDateParserFormatter {

  readonly DELIMITER = '/';

  parse(value: string): NgbDateStruct | null {
    if (value) {
      const date = value.split(this.DELIMITER);
      return {
        month: parseInt(date[0], 10),
        day: parseInt(date[1], 10),
        year: parseInt(date[2], 10)
      };
    }
    return null;
  }

  format(date: NgbDateStruct | null): string {
    return date ? date.month + this.DELIMITER + date.day + this.DELIMITER + date.year : '';
  }
}



@Component({
  selector: 'app-forecast-screen-list',
  templateUrl: './forecast-screen-list.component.html',
  styleUrls: ['./forecast-screen-list.component.scss'],
  // NOTE: For this example we are only providing current component, but probably
  // NOTE: you will want to provide your main App Module
  providers: [
    { provide: NgbDateAdapter, useClass: CustomAdapter },
    { provide: NgbDateParserFormatter, useClass: CustomDateParserFormatter }
  ]
})

export class ForecastScreenListComponent implements OnInit, OnDestroy, OnChanges {
  @ViewChild('epltable', { static: false }) epltable: ElementRef;
  @ViewChild('epltable') divView: ElementRef;
  isEnabled: boolean[] = [];
  model2: string;
  skillgroupID: number;
  forecastModelBinder: Forecast[];
  forecastBinder: ForecastDataModel;
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
  startDate: any = this.calendar.getToday();
  dateNow: Date = new Date();
  hoveredDate: NgbDate | null = null;
  toDate: NgbDate | null = null;
  fromDate = this.calendar.getToday();
  exportFileName = 'Forecast_Template';
  modalRef: NgbModalRef;
  agentSchedulingGroupId?: number;
  skillGroupBinder: SkillGroupDetails;
  enableSaveButton = false;
  enableImportButton = false;
  forecastData: ForecastDataModel[];
  forecastForm: FormGroup;
  forecastFormModel: any = [];
  dataJson: Forecast[] = [];
  skillGroupForecast: ForecastDataModel;
  capabilityForm: FormGroup;
  forecastFormArray = new FormArray([]);
  sumForecastContact: string;
  sumAHT: string;
  sumForecastedReq: string;
  sumScheduledOpen: string;
  forecastSpinner = 'forecastSpinner';
  InsertUpdate = false;
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


  ) {

  }

  calculate(forecastDataCalc: Forecast[]): number {
    return forecastDataCalc.reduce((acc, product) => acc + +product.aht, 0);
  }
  ngOnInit() {
    this.model2 = this.today;
    this.getForecastDefaultValue();

    this.subscribeToSkillGroups();


  }
  exportToExcel() {
    const ws: xlsx.WorkSheet =
      xlsx.utils.table_to_sheet(this.epltable.nativeElement);
    const wb: xlsx.WorkBook = xlsx.utils.book_new();
    xlsx.utils.book_append_sheet(wb, ws, 'Sheet1');
    xlsx.writeFile(wb, 'epltable.xlsx');
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
      time: this.formBuilder.control({ value: datum.time, disabled: false }),
      forecastedContact: this.formBuilder.control({ value: datum.forecastedContact, disabled: false }),
      aht: this.formBuilder.control({ value: datum.aht, disabled: false }),
      forecastedReq: this.formBuilder.control({ value: datum.forecastedReq, disabled: false }),
      scheduledOpen: this.formBuilder.control({ value: datum.scheduledOpen, disabled: false })
    });
  }

  private generateDatumFormGroup(datum) {

    return this.formBuilder.group({
      time: this.formBuilder.control({ value: datum.time, disabled: false }),
      forecastedContact: this.formBuilder.control({ value: datum.forecastedContact, disabled: false }),
      aht: this.formBuilder.control({ value: datum.aht, disabled: false }),
      forecastedReq: this.formBuilder.control({ value: datum.forecastedReq, disabled: false }),
      scheduledOpen: this.formBuilder.control({ value: datum.scheduledOpen, disabled: false })
    });
  }
  get formData() { return this.forecastForm.get('forecastFormArrays') as FormArray; }



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
    return this.dateAdapter.toModel(this.ngbCalendar.getToday());
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


    this.skillGroupBinder = skillGroup;
    this.loadSkillGroup();

  }

  loadSkillGroup() {
    this.forecastFormArray.reset();
    // var dateParse = `${this.DateModel.month}-${this.DateModel.day}-${this.DateModel.year}`

    this.spinnerService.show(this.forecastSpinner, SpinnerOptions);
    this.getSkillGroupForecast = this.forecastService.getForecastDataById(this.skillGroupBinder?.id, this.model2).subscribe((data) => {
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
      this.sumScheduledOpen = data.forecastData.reduce((a, b) => +a + +b.scheduledOpen, 0);

      this.sumForecastContact = parseFloat(this.sumForecastContact).toFixed(2);
      this.sumAHT = parseFloat(this.sumAHT).toFixed(2);
      this.sumForecastedReq = parseFloat(this.sumForecastedReq).toFixed(2);
      this.sumScheduledOpen = parseFloat(this.sumScheduledOpen).toFixed(2);

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


  change(event) {
    this.enableImportButton = true;
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

    if (this.InsertUpdate === true) {
      let forecastObjArrays: Forecast[];

      const now = +new Date(this.model2);

      let insertObject: ForecastDataModel;
      // let intDate = new getLocaleDateTimeFormat();
      this.forecastID = +`${this.skillGroupBinder?.id}${now}`;
      console.log(this.forecastID);
      forecastObjArrays = this.formData.value;
      insertObject = {
        ForecastId: this.forecastID,
        Date: this.model2,
        SkillGroupId: this.skillGroupBinder?.id,
        ForecastData:
          forecastObjArrays
      };

      this.forecastService.addForecast(insertObject).subscribe(res => {
        this.showSuccessPopUpMessage('The record has been added!');
        this.enableSaveButton = false;
this.loadSkillGroup();
      },
        error => {

          if (error.status === 409) {
            this.updateForecast();
          }

        }
      );
    } else {
      // var forecastObjArrays: Forecast[];

      this.updateForecast();
    }


  }
  updateForecast() {
    let updateForecastData: UpdateForecastData;

    let forecastTest: Forecast[];


    forecastTest = this.formData.value;


    // var returnedTarget = Object.assign(forecastTest, this.formData.value);
    updateForecastData = new UpdateForecastData();
    updateForecastData = { forecastData: forecastTest };


    this.forecastService.updateForecast(this.forecastID, updateForecastData).subscribe(res => {
      this.showSuccessPopUpMessage('The record has been updated!');
      this.enableSaveButton = false;
      this.loadSkillGroup();
    },
      error => {
        this.showErrorWarningPopUpMessage('Error');
      }
    );
  }
}
