import { Component, OnInit } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';
import { CookieService } from 'ngx-cookie-service';
import { environment } from 'src/environments/environment';
import { SubscriptionLike as ISubscription } from 'rxjs';
import { CssLanguageService } from '../../../../../shared/services/css-language.service';
import { KeyValue } from 'src/app/shared/models/key-value.model';
import { GenericStateManagerService } from 'src/app/shared/services/generic-state-manager.service';
import { TranslateService } from '@ngx-translate/core';
import { Language, CSS_LANGUAGES } from 'src/app/shared/models/language-value.model';
import { NgbModal, NgbModalOptions, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { GenericPopUpComponent } from 'src/app/shared/popups/generic-pop-up/generic-pop-up.component';
import { AuthService } from 'src/app/core/services/auth.service';
import { LanguagePreferenceService } from 'src/app/shared/services/language-preference.service';
import { LanguagePreference } from 'src/app/shared/models/language-preference.model';
@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {

  modalRef: NgbModalRef;
  cssLanguages: Language[];
  getTranslationSubscription: ISubscription;
  subscriptionList: ISubscription[] = [];
  currentLanguage: string;
  LoggedUser;

  constructor(
    private languagePreferenceService: LanguagePreferenceService,
    private authService: AuthService,
    private modalService: NgbModal,
    public translate: TranslateService,
    private cookieService: CookieService,
    private router: Router,
    private route: ActivatedRoute
  ) {

    this.cssLanguages = CSS_LANGUAGES;
    this.LoggedUser = this.authService.getLoggedUserInfo();
  }

  ngOnInit(): void {
    this.preLoadTranslations();
    this.loadTranslations();
    this.subscribeToTranslations();
  }

  logout() {
    this.cookieService.deleteAll(environment.settings.cookiePath);
    this.router.navigate(['login']);
  }


  changeLanguage(language) {
    this.languagePreferenceService.setLanguagePreference(this.LoggedUser.employeeId, language.code).subscribe(
      resp => { }
    );
  }

  // modal properties
  private getModalPopup(component: any, size: string) {
    const options: NgbModalOptions = { backdrop: 'static', centered: true, size };
    this.modalRef = this.modalService.open(component, options);
  }

  // heading and content message for modal
  private setComponentMessages(headingMessage: string, contentMessage: string, confirmButton: string, cancelButton: string) {
    this.modalRef.componentInstance.headingMessage = headingMessage;
    this.modalRef.componentInstance.contentMessage = contentMessage;
    this.modalRef.componentInstance.confirmButton = confirmButton;
    this.modalRef.componentInstance.cancelButton = cancelButton;
  }

  confirmChangeLanguage(language) {
    this.getModalPopup(GenericPopUpComponent, 'md');
    this.setComponentMessages(`${this.translate.instant('Change_language_to')} ${this.translate.instant(language.name)}?`, ``, `Yes`, `No`);
    this.modalRef.result.then((result) => {
      if (result && result === true) {
        this.changeLanguage(language);
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

    this.subscriptionList.push(this.getTranslationSubscription);
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
