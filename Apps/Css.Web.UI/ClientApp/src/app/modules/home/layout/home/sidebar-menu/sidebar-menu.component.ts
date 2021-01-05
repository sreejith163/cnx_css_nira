import { Component, OnInit } from '@angular/core';
import { LoggedUserInfo } from 'src/app/core/models/logged-user-info.model';
import { AuthService } from 'src/app/core/services/auth.service';
import { SubscriptionLike as ISubscription } from 'rxjs';
import { GenericStateManagerService } from 'src/app/shared/services/generic-state-manager.service';
import { CSS_LANGUAGES } from 'src/app/shared/models/language-value.model';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-sidebar-menu',
  templateUrl: './sidebar-menu.component.html',
  styleUrls: ['./sidebar-menu.component.css']
})
export class SidebarMenuComponent implements OnInit {
  currentLanguage: string;
  menuLength: string;
  loggedUser: LoggedUserInfo;
  getTranslationSubscription: ISubscription;
  subscriptionList: ISubscription[] = [];

  constructor(
    public translate: TranslateService,
    private authService: AuthService,
    private genericStateManagerService: GenericStateManagerService,
  ) { }

  ngOnInit(): void {
    this.loggedUser = this.authService.getLoggedUserInfo();
    this.loadTranslations();
    this.subscribeToTranslations();
  }

  private subscribeToTranslations() {
    this.getTranslationSubscription = this.genericStateManagerService.userLanguageChanged.subscribe(
      (language) => {
        if (language) {
          this.loadTranslations();
        }
      }
    );
    this.subscriptionList.push(this.getTranslationSubscription);
  }

  private loadTranslations() {
    const browserLang = this.genericStateManagerService.getLanguage();
    this.currentLanguage = browserLang ? browserLang : 'en';
    this.translate.use(this.currentLanguage);

    this.menuLength = CSS_LANGUAGES.find(x => x.code === this.currentLanguage).sidebarLength;

  }

}
