import { Component, OnInit } from '@angular/core';
import { AgentAdminDetails } from '../../../models/agent-admin-details.model';
import { AgentAdminResponse } from '../../../models/agent-admin-response.model';
import { AgentAdminService } from '../../../services/agent-admin.service';
import { Observable, SubscriptionLike as ISubscription } from 'rxjs';
import { ContentType } from 'src/app/shared/enums/content-type.enum';
import { ErrorWarningPopUpComponent } from 'src/app/shared/popups/error-warning-pop-up/error-warning-pop-up.component';
import { NgbModal, NgbModalOptions, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { AgentAdminQueryParameter } from '../../../models/agent-admin-query-parameter.model';
import { Constants } from 'src/app/shared/util/constants.util';
import { NgxSpinnerService } from 'ngx-spinner';
import { HeaderPagination } from 'src/app/shared/models/header-pagination.model';
import { SpinnerOptions } from 'src/app/shared/util/spinner-options.util';
import { AuthService } from 'src/app/core/services/auth.service';
import { GenericPopUpComponent } from 'src/app/shared/popups/generic-pop-up/generic-pop-up.component';
import { LanguagePreferenceService } from 'src/app/shared/services/language-preference.service';
import { TranslateService } from '@ngx-translate/core';
import { ActivatedRoute } from '@angular/router';
import { LanguagePreference } from 'src/app/shared/models/language-preference.model';
import { MoveAgentsService } from '../../../services/move-agents.service';

@Component({
  selector: 'app-move-agents',
  templateUrl: './move-agents.component.html',
  styleUrls: ['./move-agents.component.scss']
})
export class MoveAgentsComponent implements OnInit {

  agentListSpinnerLeft = "agentListSpinnerLeft";
  agentListSpinnerRight = "agentListSpinnerRight";

  selectedAgentAdminIds$: Observable<Array<string>>;

  selectedAgentSchedulingGroupIdRight$: Observable<number>;

  // lists positions
  leftList = 'left';
  rightList = 'right';

  LoggedUser;
  modalRef: NgbModalRef;

  currentPageLeft = 1;
  pageSizeLeft = 10;
  characterSpliceLeft = 25;
  paginationSizeLeft = Constants.paginationSize;
  maxLengthLeft = Constants.DefaultTextMaxLength;
  orderByLeft = 'createdDate';
  sortByLeft = 'desc';
  spinnerLeft = 'agentAdminsLeft';
  headerPaginationValuesLeft: HeaderPagination;

  currentPageRight = 1;
  pageSizeRight = 10;
  characterSpliceRight = 25;
  paginationSizeRight = Constants.paginationSize;
  maxLengthRight = Constants.DefaultTextMaxLength;
  orderByRight = 'createdDate';
  sortByRight = 'desc';
  spinnerRight = 'agentAdminsRight';
  headerPaginationValuesRight: HeaderPagination;

  totalAgentAdminsRecordLeft: number;
  totalAgentAdminsRecordRight: number;
  searchKeywordLeft: string;
  searchKeywordRight: string;

  agentSchedulingGroupIdLeft?: number;
  agentSchedulingGroupIdRight?: number;
  agentAdmin: AgentAdminResponse;
  agentAdminsLeftSide: AgentAdminDetails[] = [];
  agentAdminsRightSide: AgentAdminDetails[] = [];

  isSelected: boolean[] = [];
  selectedAgentAdminIds: string[] = [];

  getAgentAdminsSubscriptionLeft: ISubscription;
  getAgentAdminsSubscriptionRight: ISubscription;
  subscriptions: any[] = [];
  currentLanguage: string;
  getTranslationSubscription: ISubscription;

  constructor(
    private spinnerService: NgxSpinnerService,
    private modalService: NgbModal,
    private agentAdminService: AgentAdminService,
    private authService: AuthService,
    private languagePreferenceService: LanguagePreferenceService,
    public translate: TranslateService,
    private route: ActivatedRoute,
    private moveAgentService: MoveAgentsService
  ) {
    this.LoggedUser = this.authService.getLoggedUserInfo();
  }

  ngOnInit() {
    this.preLoadTranslations();
    this.loadTranslations();
    this.subscribeToTranslations();
    this.selectedAgentAdminIds$ = this.moveAgentService.selectedAgentAdminIds$;
    this.selectedAgentSchedulingGroupIdRight$ = this.moveAgentService.agentSchedulingGroupIdRightSubject$;
  }

  toggleSelected(agentAdminId){
    // console.log(agentAdmin)
   
    if(!(this.selectedAgentAdminIds.includes(agentAdminId))){
      // push the ids to an array
      this.selectedAgentAdminIds.push(agentAdminId);
      // console.log(this.selectedAgentAdminIds);
    }else{
      console.log(this.selectedAgentAdminIds, "hey");
      const indexId = this.selectedAgentAdminIds.indexOf(agentAdminId,0);
      // remove the ids from the array
      if (indexId > -1 ) {
        this.selectedAgentAdminIds.splice(indexId, 1);
      }
      // console.log(agentAdmin.employeeId)

    }
  }
  

  moveAgents(){
    this.isSelected = [];
    this.searchKeywordLeft = '';
    this.searchKeywordRight = '';
    this.moveAgentService.moveAgents();
  }


  private getModalPopup(component: any, size: string) {
    const options: NgbModalOptions = { backdrop: 'static', centered: true, size };
    this.modalRef = this.modalService.open(component, options);
  }

  private setComponentMessages(headingMessage: string, contentMessage: string) {
    this.modalRef.componentInstance.headingMessage = headingMessage;
    this.modalRef.componentInstance.contentMessage = contentMessage;
  }

  confirmMoveAgents() {
    // this.getModalPopup(GenericPopUpComponent, 'md');
    // this.setConfirmDialogMessages(`Are you sure you? You won't be able to revert this!`, ``, `Yes`, `No`);
    this.getModalPopup(GenericPopUpComponent, 'sm');
    var msg;

    this.moveAgentService.selectedAgentAdminIds$?.subscribe(selectedIds=>{
      if(selectedIds?.length > 1){
        msg = "Are you sure you want to move these agents?";
      }else if(selectedIds?.length === 1){
        msg = "Are you sure you want to move this agent?";
      }
    });

    this.setComponentMessages('', msg);
    this.modalRef.componentInstance.warning = true;
    this.modalRef.result.then((result) => {
      if (result && result === true) {
        this.moveAgents();
      }
    });
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
