import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { TranslateService } from '@ngx-translate/core';
import { LanguagePreferenceService } from 'src/app/shared/services/language-preference.service';
import { SubscriptionLike as ISubscription } from 'rxjs';
import { AuthService } from 'src/app/core/services/auth.service';
import { LanguagePreference } from 'src/app/shared/models/language-preference.model';

@Component({
  selector: 'app-agent-schedule',
  templateUrl: './agent-schedule.component.html',
  styleUrls: ['./agent-schedule.component.scss']
})
export class AgentScheduleComponent implements OnInit {
  currentLanguage: string;
  LoggedUser;
  subscriptions: ISubscription[] = [];


  getTranslationSubscription: ISubscription;

  constructor(
    public translate: TranslateService,
    private route: ActivatedRoute,
    private authService: AuthService,
    private languagePreferenceService: LanguagePreferenceService
  ) {

    this.LoggedUser = this.authService.getLoggedUserInfo();

  }

  ngOnInit() {
    this.preLoadTranslations();
    this.loadTranslations();
    this.subscribeToTranslations();
  }

  unifiedToNative(unified: string) {
    const codePoints = unified.split('-').map(u => parseInt(`0x${u}`, 16));
    return String.fromCodePoint(...codePoints);
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
