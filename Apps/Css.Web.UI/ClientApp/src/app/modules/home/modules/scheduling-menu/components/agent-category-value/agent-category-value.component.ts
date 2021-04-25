import { Component, EventEmitter, OnDestroy, OnInit, Output } from '@angular/core';
import { NgbCalendar, NgbDateParserFormatter, NgbModal, NgbModalOptions, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';

import { SubscriptionLike as ISubscription } from 'rxjs';
import { SchedulingCodeService } from 'src/app/shared/services/scheduling-code.service';
import { PermissionsService } from '../../../system-admin/services/permissions.service';
import { AuthService } from 'src/app/core/services/auth.service';
import { ExcelService } from 'src/app/shared/services/excel.service';

import { LanguagePreference } from 'src/app/shared/models/language-preference.model';
import { ActivatedRoute } from '@angular/router';
import { LanguagePreferenceService } from 'src/app/shared/services/language-preference.service';
import { TranslateService } from '@ngx-translate/core';
import { SchedulingStatus } from '../../enums/scheduling-status.enum';
import { Constants } from 'src/app/shared/util/constants.util';
import { DatePipe } from '@angular/common';
import { NgxSpinnerService } from 'ngx-spinner';
import { SpinnerOptions } from 'src/app/shared/util/spinner-options.util';
import { animate, state, style, transition, trigger } from '@angular/animations';
import { AgentSchedulingGridExport } from '../../models/agent-scheduling-grid-export.model';
import { AgentCategoryValueQueryParameter } from '../../models/agent-category-value-query-parameter.model';
import { AgentCategoryValueService } from '../../services/agent-category-value.service';
import { AgentCategoryService } from '../../../system-admin/services/agent-category.service';


declare function setRowCellIndex(cell: string);
declare function highlightSelectedCells(table: string, cell: string);
declare function highlightCell(cell: string, className: string);
import { AgentCategoryValueResponse } from '../../models/agent-category-value-response.model';
import { HeaderPagination } from 'src/app/shared/models/header-pagination.model';
import { AgentCategoryValueImport } from '../../models/agent-category-value-import.model';
import { NgxCsvParser, NgxCSVParserError } from 'ngx-csv-parser';
import { ErrorWarningPopUpComponent } from 'src/app/shared/popups/error-warning-pop-up/error-warning-pop-up.component';
import { ContentType } from 'src/app/shared/enums/content-type.enum';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-agent-category-value',
  animations: [
    trigger(
      'enterAnimation', [
      state('true', style({ opacity: 1, height: 'auto' })),
      state('void', style({ opacity: 0, height: 0 })),
      transition(':enter', animate('400ms ease-in-out')),
      transition(':leave', animate('400ms ease-in-out'))
    ]
    )
  ],
  templateUrl: './agent-category-value.component.html',
  styleUrls: ['./agent-category-value.component.scss'],
  providers: [DatePipe]
})
export class AgentCategoryValueComponent implements OnInit, OnDestroy {
  uploadFile: string;
  agentSchedulingGroupId: number;
  agentCategoryId: number;
  currentPage = 1;
  pageSize = 50;
  characterSplice = 25;
  maxIconCount = 30;
  iconCount: number;
  startIcon = 0;
  importSpinner = "spinner";
  endIcon: number;
  importBtn: boolean;
  totalagentCategoryValueRecord: number;
  rangeIndex: number;
  status = SchedulingStatus;
  importAgentValue: AgentCategoryValueImport;
  agentCategoryImportColumns = ['employeeId', 'agentCategory', 'startDate', 'value'];
  selectedIconId: string;
  icon: string;
  spinner = 'scheduling-tab';
  scheduleSpinner = 'scheduling-spinner';
  selectedCellClassName = 'cell-selected';
  tableClassName = 'schedulingGridTable';
  headerPaginationValues: HeaderPagination;
  orderBy = 'createdDate';
  sortBy = 'desc';
  searchText: string;
  exportFileName = 'Attendance_scheduling';
  startDate: string;
  currentLanguage: string;
  csvTableHeader: string[];
  isMouseDown: boolean;
  isDelete: boolean;
  refreshSchedulingTab: boolean;
  hasNewDateRangeSelected: boolean;
  LoggedUser;
  exportData: AgentSchedulingGridExport[] = [];
  agentCategoryValueResponse: AgentCategoryValueResponse[] = [];
  modalRef: NgbModalRef;
  scheduleStatus = SchedulingStatus;
  paginationSize = Constants.agentCategoryValuePaginationSize;
  schedulingIntervals = Constants.schedulingIntervals;
  csvRecords: any[] = [];
  getAgentCategoryValueSubscription: ISubscription;
  importAgentCategorySubscription: ISubscription;
  getTranslationSubscription: ISubscription;
  subscriptions: ISubscription[] = [];

  @Output() agentSchedulingGroupSelected = new EventEmitter();
  @Output() agentCategorySelected = new EventEmitter();
  @Output() keywordToSearch = new EventEmitter();

  constructor(
    private spinnerService: NgxSpinnerService,
    public ngbDateParserFormatter: NgbDateParserFormatter,
    private authService: AuthService,
    private route: ActivatedRoute,
    private languagePreferenceService: LanguagePreferenceService,
    private agentCategoryValueService: AgentCategoryValueService,
    private adminCategoryService: AgentCategoryService,
    public translate: TranslateService,
    private datepipe: DatePipe,
    private modalService: NgbModal,
    private ngxCsvParser: NgxCsvParser,
    private toast: ToastrService
  ) {
    this.LoggedUser = this.authService.getLoggedUserInfo();
  }

  ngOnInit(): void {
    this.importBtn = false;
    this.preLoadTranslations();
    this.loadTranslations();
    this.subscribeToTranslations();
  }

  ngOnDestroy() {
    this.subscriptions.forEach(subscription => {
      if (subscription) {
        subscription.unsubscribe();
      }
    });
  }

  // onSchedulingGroupChange(agentSchedulingGroupId: number) {
  //   this.agentSchedulingGroupSelected.emit(agentSchedulingGroupId);
  // }

  onSchedulingGroupChange(schedulingGroupId: number) {
    this.agentSchedulingGroupId = schedulingGroupId;
    this.loadAgentCategoryValues();
  }

  onAgentCategoryChange(agentCategoryId: number) {
    this.agentCategoryId = agentCategoryId;
    this.loadAgentCategoryValues();
    //this.agentCategorySelected.emit(agentCategoryId);
  }

  getAgentCategoryValue(values: { startDate: string, categoryId: number, categoryValue: string }[]) {
    if (values === undefined) {
      return '';
    }
    const selectedCategory = values.find(x => x.categoryId === this.agentCategoryId);
    if (selectedCategory !== undefined) {
      return selectedCategory.categoryValue;
    }
    return '';
  }

  getAgentCategoryStartDate(values: { startDate: string, categoryId: number, categoryValue: string }[]) {
    if (values === undefined) {
      return '';
    }
    const selectedCategory = values.find(x => x.categoryId === this.agentCategoryId);
    if (selectedCategory !== undefined) {

      return this.datepipe.transform(selectedCategory.startDate?.replace("Z", ""), 'yyyy-MM-dd');//moment(selectedCategory.startDate).format("YYYY-MM-DD");
    }
    return '';
  }

  changePageSize(pageSize: number) {
    this.pageSize = pageSize;
    this.loadAgentCategoryValues();
  }

  changePage(page: number) {
    this.currentPage = page;
    this.loadAgentCategoryValues();
  }

  sort(columnName: string, sortBy: string) {
    this.sortBy = sortBy === 'asc' ? 'desc' : 'asc';
    this.orderBy = columnName;

    this.loadAgentCategoryValues();
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

  private getQueryParams() {
    const agentCategoryValueQueryParams = new AgentCategoryValueQueryParameter();
    agentCategoryValueQueryParams.pageNumber = this.currentPage;
    agentCategoryValueQueryParams.pageSize = this.pageSize;
    agentCategoryValueQueryParams.searchKeyword = '';
    agentCategoryValueQueryParams.orderBy = `${this.orderBy} ${this.sortBy}`;
    agentCategoryValueQueryParams.fields = '';
    agentCategoryValueQueryParams.agentSchedulingGroupId = this.agentSchedulingGroupId;
    agentCategoryValueQueryParams.agentCategoryId = this.agentCategoryId;
    return agentCategoryValueQueryParams;
  }

  private loadAgentCategoryValues() {
    this.agentCategoryValueResponse = [];
    if (this.agentSchedulingGroupId != undefined && this.agentCategoryId != undefined) {
      const queryParams = this.getQueryParams();
      this.spinnerService.show(this.spinner, SpinnerOptions);

      this.getAgentCategoryValueSubscription = this.agentCategoryValueService.getAgentCategorieValues(queryParams)
        .subscribe((response) => {
          if (response.body) {
            this.agentCategoryValueResponse = response.body;
            this.headerPaginationValues = JSON.parse(response.headers.get('x-pagination'));
            this.totalagentCategoryValueRecord = this.headerPaginationValues.totalCount;
          }
          this.spinnerService.hide(this.spinner);
        }, (error) => {
          this.spinnerService.hide(this.spinner);
        });

      this.subscriptions.push(this.getAgentCategoryValueSubscription);
    }

  }
  openImportModal(content) {
    const options: NgbModalOptions = { backdrop: 'static', centered: true, size: 'lg' };
  
    this.modalService.open(content, options);
  }

  onChangeFile(files: File[]) {

    if (files[0]) {
      this.fileChangeListener(files[0]);
      this.importBtn = true;
    }

  }


  // Your applications input change listener for the CSV File
  fileChangeListener(file): void {

    // Select the files from the event


    // Parse the file you want to select for the operation along with the configuration
    this.ngxCsvParser.parse(file, { header: false, delimiter: ',' })
      .pipe().subscribe((result: Array<any>) => {


        const csvTableData = [...result.slice(1, result.length)];
        this.csvTableHeader = result[0];

        // convert the headers to proper headers
        this.csvTableHeader = Object.assign(this.csvTableHeader, this.agentCategoryImportColumns);
        for (const ele of csvTableData) {
          const csvJson = new Object;
          if (ele.length > 0) {
            for (let i = 0; i < ele.length; i++) {
              csvJson[this.csvTableHeader[i]] = ele[i];
            }
          }
          this.uploadFile = file.name;
          this.csvRecords.push(csvJson);

        }

     
      }, (error: NgxCSVParserError) => {

        this.showErrorWarningPopUpMessage(error.message)
      });

  }

  importAgentCategoryValue() {
    this.spinnerService.show(this.importSpinner, SpinnerOptions);
    
    this.agentCategoryValueService.importAgentCategoryValue(this.csvRecords,this.authService.getLoggedUserInfo()?.displayName)
      .subscribe((response) => {
        this.modalService.dismissAll();
        this.importClear(); 
        this.spinnerService.hide(this.importSpinner);
        this.toast.success(response);
        this.loadAgentCategoryValues();
      this.csvRecords = [];
      }, (error) => {
        this.spinnerService.hide(this.importSpinner);
        this.modalService.dismissAll();
        this.showErrorWarningPopUpMessage(error.error);    
        this.importClear();
        this.csvRecords = []; 
        this.loadAgentCategoryValues();
        //console.clear();
      });
  }
  importClear() {
    this.uploadFile = '';
    this.importBtn = false;
  }
  exportToExcel() {
    let fileName = `AgentCategoryValue${this.datepipe.transform(new Date().toString().replace("Z", ""), 'yyyyMMdd')}.csv`;
    let columnNames = ["Employee Id", "Agent Category", "Start Date", "Value"];
    let header = columnNames.join(',');

    let csv = header;
    csv += '\r\n';

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
  search() {

    this.loadAgentCategoryValues();
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
