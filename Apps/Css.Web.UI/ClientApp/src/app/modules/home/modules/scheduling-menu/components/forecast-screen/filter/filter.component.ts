import { Component, Input, EventEmitter, OnChanges, OnDestroy, OnInit, Output, Injectable } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';
import { TranslationDetails } from 'src/app/shared/models/translation-details.model';
import { Constants } from 'src/app/shared/util/constants.util';
import { SpinnerOptions } from 'src/app/shared/util/spinner-options.util';
import { SkillTagQueryParams } from '../../../../setup-menu/models/skill-tag-query-params.model';
import { Subject, SubscriptionLike as ISubscription } from 'rxjs';
import { SkillTagService } from '../../../../setup-menu/services/skill-tag.service';
import { SkillTagDetails } from '../../../../setup-menu/models/skill-tag-details.model';
import { HeaderPagination } from 'src/app/shared/models/header-pagination.model';
import { NgbCalendar, NgbDate, NgbDateAdapter, NgbDateParserFormatter, NgbDateStruct, NgbModal, NgbModalOptions, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { AgentSchedulesResponse } from '../../../models/agent-schedules-response.model';
import { AgentScheduleType } from '../../../enums/agent-schedule-type.enum';
import { TranslateService } from '@ngx-translate/core';
import { LanguagePreferenceService } from 'src/app/shared/services/language-preference.service';
import { ExcelService } from 'src/app/shared/services/excel.service';
import { SchedulingMangerExcelExportData } from '../../../constants/scheduling-manager-excel-export-data';
import { SchedulingExcelExportData } from '../../../constants/scheduling-excel-export-data';
import { ImportScheduleComponent } from '../../scheduling-grid/import-schedule/import-schedule.component';
import { MessagePopUpComponent } from 'src/app/shared/popups/message-pop-up/message-pop-up.component';
import { ForecastExcelExportData } from '../../../constants/forecast-excel-export-data';
import { SkillGroupService } from '../../../../setup-menu/services/skill-group.service';
import { SkillGroupDetails } from '../../../../setup-menu/models/skill-group-details.model';
import { debounceTime, distinctUntilChanged, switchMap } from 'rxjs/operators';
import { SkillGroupQueryParameters } from '../../../../setup-menu/models/skill-group-query-parameters.model';

@Component({
  selector: 'app-filter',
  templateUrl: './filter.component.html',
  styleUrls: ['./filter.component.scss'],
})

export class FilterComponent implements OnInit, OnDestroy, OnChanges {
  model: NgbDateStruct;
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
  getSkillTagsSubscription: ISubscription;
  getSkillTagSubscription: ISubscription;
  clientId: number;
  clientLobGroupId: number;
  skillTags: SkillTagDetails[] = [];
  totalSkillTagsRecord: number;
  searchKeyword: string;
  headerPaginationValues: HeaderPagination;
  dateValue: string;
  tabIndex: number;
  totalSchedulingGridData: AgentSchedulesResponse[] = [];
  formatsDateTest: string = "MM/dd/yyyy";
  startDate: any = this.calendar.getToday();
  dateNow: Date = new Date();
  hoveredDate: NgbDate | null = null;
  toDate: NgbDate | null = null;
  fromDate = this.calendar.getToday();
  exportFileName = 'Forecast_Template';
  modalRef: NgbModalRef;
  @Input() skillGroupId: number;
  @Output() skillGroupSelected = new EventEmitter();
  @Output() selectedDateChange = new EventEmitter<Date>();
  constructor(
    private calendar: NgbCalendar,
    private spinnerService: NgxSpinnerService,
    private skillTagSevice: SkillTagService,
    private languagePreferenceService: LanguagePreferenceService,
    public translate: TranslateService,
    private excelService: ExcelService,
    private modalService: NgbModal,
    private skillGroupService: SkillGroupService,
    private ngbCalendar: NgbCalendar, private dateAdapter: NgbDateAdapter<string>
  ) {

  }

  ngOnInit(): void {
    this.tabIndex = AgentScheduleType.Scheduling;
    this.subscribeToSkillGroups();
    this.subscribeToSearching();
  }

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

    this.modalRef.result.then((result) => {
      const message = result.partialImport ? 'The record has been paritially imported!' : 'The record has been imported!';
      this.getModalPopup(MessagePopUpComponent, 'sm', message);
      this.modalRef.result.then(() => {
        //this.tabIndex === AgentScheduleType.Scheduling ? this.loadAgentSchedules() : this.loadAgentScheduleManger();
      });
    });
  }

  private getModalPopup(component: any, size: string, contentMessage?: string) {
    const options: NgbModalOptions = { backdrop: 'static', centered: true, size };
    this.modalRef = this.modalService.open(component, options);
    this.modalRef.componentInstance.translationValues = this.translationValues;
    this.modalRef.componentInstance.headingMessage = 'Success';
    this.modalRef.componentInstance.contentMessage = contentMessage;
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

  onSkillGroupChange(event: SkillGroupDetails) {
    this.skillGroupSelected.emit(event);
    // console.log(event)
  }

  date: { year: number, month: number };


  change(event) {

    console.log(this.model);
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

}
