import { Component, OnInit } from '@angular/core';
import * as $ from 'jquery';
import * as AdminLte from 'admin-lte';
import { Router } from '@angular/router';
import { SubscriptionLike as ISubscription } from 'rxjs';
import { GenericStateManagerService } from 'src/app/shared/services/generic-state-manager.service';
import { CSS_LANGUAGES } from 'src/app/shared/models/language-value.model';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent implements OnInit {
  currentLanguage: string;
  menu_length: string;
  getTranslationSubscription: ISubscription;
  subscriptionList: ISubscription[] = [];

  constructor(
    public translate: TranslateService,
    private genericStateManagerService: GenericStateManagerService,
    private router: Router
  ) { }

  ngOnInit(): void {
    $('[data-widget="treeview"]').each(x => {
      AdminLte.Treeview._jQueryInterface.call($(this), 'init');
    });
    this.loadTranslations();
    this.subscribeToTranslations();
  }

  navigateToAgentAdmin() {
    this.router.navigate(['add-agent-profile']);
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
