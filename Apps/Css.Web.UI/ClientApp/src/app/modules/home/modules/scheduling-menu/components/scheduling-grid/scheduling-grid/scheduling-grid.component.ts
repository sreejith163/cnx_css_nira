import { Component, OnDestroy, OnInit, ViewChild } from '@angular/core';
import { NgbCalendar, NgbDateParserFormatter, NgbModal, NgbModalOptions, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { MessagePopUpComponent } from 'src/app/shared/popups/message-pop-up/message-pop-up.component';

import { SubscriptionLike as ISubscription } from 'rxjs';
import { NgxSpinnerService } from 'ngx-spinner';
import { SpinnerOptions } from 'src/app/shared/util/spinner-options.util';
import { SchedulingCode } from '../../../../system-admin/models/scheduling-code.model';
import { SchedulingCodeService } from 'src/app/shared/services/scheduling-code.service';
import { SchedulingCodeQueryParams } from '../../../../system-admin/models/scheduling-code-query-params.model';
import { AuthService } from 'src/app/core/services/auth.service';
import { AgentScheduleType } from '../../../enums/agent-schedule-type.enum';
import { ExcelService } from 'src/app/shared/services/excel.service';


import { ImportScheduleComponent } from '../import-schedule/import-schedule.component';
import { ExcelData } from '../../../models/excel-data.model';
import { SchedulingExcelExportData } from '../../../constants/scheduling-excel-export-data';
import { LanguagePreference } from 'src/app/shared/models/language-preference.model';
import { ActivatedRoute } from '@angular/router';
import { LanguagePreferenceService } from 'src/app/shared/services/language-preference.service';
import { TabsetComponent } from 'ngx-bootstrap/tabs';
import { SchedulingMangerExcelExportData } from '../../../constants/scheduling-manager-excel-export-data';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-scheduling-grid',
  templateUrl: './scheduling-grid.component.html',
  styleUrls: ['./scheduling-grid.component.scss']
})
export class SchedulingGridComponent implements OnInit, OnDestroy {

  tabIndex: number;
  agentSchedulingGroupId: number;
  searchText: string;
  scheduleSpinner = 'SchedulingSpinner';
  exportFileName = 'Attendance_scheduling';
  startDate: string;
  currentLanguage: string;
  refreshMangerTab: boolean;
  refreshSchedulingTab: boolean;
  LoggedUser;

  modalRef: NgbModalRef;

  schedulingCodes: SchedulingCode[] = [];
  importedData: ExcelData[] = [];

  getSchedulingCodesSubscription: ISubscription;
  getTranslationSubscription: ISubscription;
  subscriptions: ISubscription[] = [];

  @ViewChild('staticTabs', { static: false }) staticTabs: TabsetComponent;

  constructor(
    private modalService: NgbModal,
    public ngbDateParserFormatter: NgbDateParserFormatter,
    private spinnerService: NgxSpinnerService,
    private schedulingCodeService: SchedulingCodeService,
    private excelService: ExcelService,
    private authService: AuthService,
    private route: ActivatedRoute,
    private languagePreferenceService: LanguagePreferenceService,
    public translate: TranslateService,
  ) {
    this.LoggedUser = this.authService.getLoggedUserInfo();
  }

  ngOnInit(): void {
    this.tabIndex = AgentScheduleType.Scheduling;
    this.loadSchedulingCodes();
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

  onSchedulingGroupChange(schedulingGroupId: number) {
    this.agentSchedulingGroupId = schedulingGroupId;
  }

  search(searchText: string) {
    if (this.agentSchedulingGroupId) {
      this.searchText = searchText;
    }
  }

  onSelectStartDate(date: string) {
    this.startDate = date;
  }

  openImportSchedule() {
    this.getModalPopup(ImportScheduleComponent, 'lg');
    this.modalRef.componentInstance.agentScheduleType = this.tabIndex;

    this.modalRef.result.then((result) => {
      const message = result.partialImport ? 'The record has been paritially imported!' : 'The record has been imported!';
      this.getModalPopup(MessagePopUpComponent, 'sm', message);
      this.modalRef.result.then(() => {
        this.tabIndex === AgentScheduleType.Scheduling ? this.refreshSchedulingTab = true : this.refreshMangerTab = true;
      });
    });
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
      SchedulingExcelExportData : SchedulingMangerExcelExportData, this.exportFileName + date);
  }

  openTab(tabIndex: number) {
    this.tabIndex = tabIndex;
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

  private loadSchedulingCodes() {
    const queryParams = new SchedulingCodeQueryParams();
    queryParams.skipPageSize = true;
    queryParams.fields = 'id, description, icon';
    this.spinnerService.show(this.scheduleSpinner, SpinnerOptions);

    this.getSchedulingCodesSubscription = this.schedulingCodeService.getSchedulingCodes(queryParams)
      .subscribe((response) => {
        if (response.body) {
          this.schedulingCodes = response.body;
        }
        this.spinnerService.hide(this.scheduleSpinner);
      }, (error) => {
        this.spinnerService.hide(this.scheduleSpinner);
        console.log(error);
      });

    this.subscriptions.push(this.getSchedulingCodesSubscription);
  }

  private getModalPopup(component: any, size: string, contentMessage?: string) {
    const options: NgbModalOptions = { backdrop: 'static', centered: true, size };
    this.modalRef = this.modalService.open(component, options);
    this.modalRef.componentInstance.headingMessage = 'Success';
    this.modalRef.componentInstance.contentMessage = contentMessage;
  }

}
