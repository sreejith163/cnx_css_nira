import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { NgbDateStruct } from '@ng-bootstrap/ng-bootstrap';
import { TranslateService } from '@ngx-translate/core';
import { SubscriptionLike as ISubscription } from 'rxjs';
import { LanguagePreference } from 'src/app/shared/models/language-preference.model';
import { LanguagePreferenceService } from 'src/app/shared/services/language-preference.service';
import { Constants } from 'src/app/shared/util/constants.util';

@Component({
  selector: 'app-agent-category-value',
  templateUrl: './agent-category-value.component.html',
  styleUrls: ['./agent-category-value.component.scss']
})
export class AgentCategoryValueComponent implements OnInit {

  agentSchedulingGroupId: number;
  agentCategoryId: number;
  totalRecord: number;
  characterSplice = 25;
  pageSize = 10;
  currentPage = 5;
  currentLanguage: string;
  sortBy: string;
  orderBy: string;
  LoggedUser;
  spinner = 'agent-category-value';

  paginationSize = Constants.paginationSize;
  model: NgbDateStruct;
  agentCategoryValues: any[] = [];

  getTranslationSubscription: ISubscription;
  subscriptions: ISubscription[] = [];

  constructor(
    private route: ActivatedRoute,
    private languagePreferenceService: LanguagePreferenceService,
    public translate: TranslateService,
  ) { }

  ngOnInit(): void {
    this.preLoadTranslations();
    this.loadTranslations();
    this.subscribeToTranslations();
  }

  changePageSize(pageSize: number) {
    this.pageSize = pageSize;
  }

  changePage(page: number) {
    this.currentPage = page;
  }

  onAgentCategoryChange(agentCategoryId: number) {
    this.agentCategoryId = agentCategoryId;
  }

  onSchedulingGroupChange(agentSchedulingGroupId: number) {
    this.agentSchedulingGroupId = agentSchedulingGroupId;
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
    this.languagePreferenceService.getLanguagePreference(this.LoggedUser?.employeeId).subscribe((langPref: LanguagePreference) => {
      this.currentLanguage = langPref.languagePreference ? langPref.languagePreference : 'en';
      this.translate.use(this.currentLanguage);
    });
  }

}
