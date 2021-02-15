import { HttpClient } from '@angular/common/http';
import { Component, ElementRef, Injectable, OnInit, ViewChild } from '@angular/core';
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

@Injectable()
export class CustomAdapter extends NgbDateAdapter<string> {

  readonly DELIMITER = '/';

  fromModel(value: string | null): NgbDateStruct | null {
    if (value) {
      let date = value.split(this.DELIMITER);
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
      let date = value.split(this.DELIMITER);
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
  selector: 'app-view-ou-screen',
  templateUrl: './view-ou-screen.component.html',
  styleUrls: ['./view-ou-screen.component.scss'],
  providers: [
    { provide: NgbDateAdapter, useClass: CustomAdapter },
    { provide: NgbDateParserFormatter, useClass: CustomDateParserFormatter }
  ]
})
export class ViewOuScreenComponent implements OnInit {

  @ViewChild('epltable', { static: false }) epltable: ElementRef;
  @ViewChild("epltable") divView: ElementRef;
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
  sumForecastedReq: string;
  sumScheduledOpen: string;
  forecastSpinner = 'forecastSpinner';
  InsertUpdate = false;
  OU: any;
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
    return forecastDataCalc.reduce((acc, product) => acc + parseInt(product.aht), 0)
  }
  ngOnInit() {
    this.model2 = this.today;
    this.getForecastDefaultValue();

    this.subscribeToSkillGroups();


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

      this.sumForecastContact = parseFloat(this.sumForecastContact).toFixed(2)
      this.sumAHT = parseFloat(this.sumAHT).toFixed(2)
      this.sumForecastedReq = parseFloat(this.sumForecastedReq).toFixed(2)
      this.sumScheduledOpen = parseFloat(this.sumScheduledOpen).toFixed(2)

      this.spinnerService.hide(this.spinner);
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
  loadForecast() {
    const queryParams = this.getQueryParams();
    this.spinnerService.show(this.spinner, SpinnerOptions);

    this.getSkillGroupForecast = this.forecastService.getForecastDataById(this.skillGroupBinder?.id, this.model2)
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
          this.showErrorWarningPopUpMessage('No Forecast Found!');
          this.InsertUpdate = true;


        } else {
          this.InsertUpdate = false;
        }
      });

    this.subscriptions.push(this.getSkillGroupForecast);
  }
  loadSkillGroup() {
    this.forecastFormArray.reset()
    // var dateParse = `${this.DateModel.month}-${this.DateModel.day}-${this.DateModel.year}`

    this.spinnerService.show(this.forecastSpinner, SpinnerOptions);
    this.getSkillGroupForecast = this.forecastService.getForecastDataById(this.skillGroupBinder?.id, this.model2).subscribe((data) => {
      this.spinnerService.hide(this.forecastSpinner);
      this.forecastID = data.forecastId;
      this.forecastForm = this.formBuilder.group({

        forecastFormArrays: this.formBuilder.array(data['forecastData'].map(datum => this.generateDatumFormGroup(datum))),

      });

      //console.log(data);
      this.sumForecastContact = data['forecastData'].reduce((a, b) => +a + +b.forecastedContact, 0);
      this.sumAHT = data['forecastData'].reduce((a, b) => +a + +b.aht, 0);
      this.sumForecastedReq = data['forecastData'].reduce((a, b) => +a + +b.forecastedReq, 0);
      this.sumScheduledOpen = data['forecastData'].reduce((a, b) => +a + +b.scheduledOpen, 0);

      this.sumForecastContact = parseFloat(this.sumForecastContact).toFixed(2)
      this.sumAHT = parseFloat(this.sumAHT).toFixed(2)
      this.sumForecastedReq = parseFloat(this.sumForecastedReq).toFixed(2)
      this.sumScheduledOpen = parseFloat(this.sumScheduledOpen).toFixed(2)

    }, (error) => {

      this.spinnerService.hide(this.forecastSpinner);
      if (error.status == 404) {
        this.getForecastDefaultValue();
        this.showErrorWarningPopUpMessage('No Forecast Found!');
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



}
