import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
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
@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {

  modalRef: NgbModalRef;
  menu_length: string;;
  cssLanguages: Language[];
  getTranslationSubscription: ISubscription;
  subscriptionList: ISubscription[] = [];
  currentLanguage: string;

  constructor(
    private modalService: NgbModal,
    public translate: TranslateService,
    private cssLanguageservice: CssLanguageService,
    private genericStateManagerService: GenericStateManagerService,
    private cookieService: CookieService,
    private router: Router
  ) { 

    this.cssLanguages = CSS_LANGUAGES;
    // translate.setDefaultLang('en');
    const browserLang = this.genericStateManagerService.getLanguage();
    this.currentLanguage = browserLang;
    translate.use(browserLang ? browserLang : 'en');
    this.menu_length = CSS_LANGUAGES.find(x=>x.code == this.currentLanguage).sidebar_length;
  }

  ngOnInit(): void {
    this.loadTranslations();
    this.subscribeToTranslations();
  }

  logout() {
    this.cookieService.deleteAll(environment.settings.cookiePath);
    this.router.navigate(['login']);
  }


  changeLanguage(language){
    this.genericStateManagerService.setLanguage(language.code);
    this.currentLanguage = language.code;
    this.translate.use(language.code);
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
    this.getModalPopup(GenericPopUpComponent, 'sm');
    this.setComponentMessages(`${this.translate.instant('Change_language_to')} ${language.name}?`, ``, `Yes`, `No`);
    this.modalRef.result.then((result) => {
      if (result && result === true) {
          this.changeLanguage(language);
      }
    });
  }

  private subscribeToTranslations(){
    this.getTranslationSubscription = this.genericStateManagerService.userLanguageChanged.subscribe(
      (language) => {
        if (language) {
          this.loadTranslations();
        }
      }
    );
    this.subscriptionList.push(this.getTranslationSubscription);
  }

  private loadTranslations(){
    const browserLang = this.genericStateManagerService.getLanguage();
    this.currentLanguage = browserLang ? browserLang : 'en';
    this.translate.use(this.currentLanguage);

    this.menu_length = CSS_LANGUAGES.find(x=>x.code == this.currentLanguage).sidebar_length;
    
  }

}
