import { Component, OnInit } from '@angular/core';
import { Router } from '@angular/router';
import { CookieService } from 'ngx-cookie-service';
import { environment } from 'src/environments/environment';
import { SubscriptionLike as ISubscription } from 'rxjs';
import { CssLanguageService } from '../../../../../shared/services/css-language.service';
import { KeyValue } from 'src/app/shared/models/key-value.model';
import { GenericStateManagerService } from 'src/app/shared/services/generic-state-manager.service';

@Component({
  selector: 'app-header',
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {

  cssLanguages: KeyValue[];
  cssLanguagesSubscription: ISubscription;
  subscriptionList: ISubscription[] = [];

  constructor(
    private cssLanguageervice: CssLanguageService,
    private genericStateManagerService: GenericStateManagerService,
    private cookieService: CookieService,
    private router: Router
  ) { }

  ngOnInit(): void {
    this.loadLanguages();
  }

  logout() {
    this.cookieService.deleteAll(environment.settings.cookiePath);
    this.router.navigate(['login']);
  }

  setCsslanguage(languageId: number) {
    this.genericStateManagerService.setCurrentLanguage(this.cssLanguages.find(x => x.id === +languageId));
  }

  private loadLanguages() {
    this.cssLanguagesSubscription = this.cssLanguageervice.getCssLanguages()
      .subscribe((response) => {
        if (response) {
          this.cssLanguages = response;
          this.genericStateManagerService.setCurrentLanguage(this.cssLanguages.find(x => x.value === 'English'));
        }
      }, (error) => {
        console.log(error);
      });

    this.subscriptionList.push(this.cssLanguagesSubscription);
  }
}
