import { Component, Input, EventEmitter, OnChanges, OnDestroy, OnInit, Output, Injectable } from '@angular/core';
import { ActivatedRoute } from '@angular/router';

import { NgbModal, NgbModalOptions, NgbModalRef } from '@ng-bootstrap/ng-bootstrap';
import { NgxSpinnerService } from 'ngx-spinner';
import { SubscriptionLike as ISubscription } from 'rxjs';
import { AuthService } from 'src/app/core/services/auth.service';
import { ClientDetails } from '../../../setup-menu/models/client-details.model';
import { ClientService } from '../../../setup-menu/services/client.service';
import { EntityAgentSchedulingGroupDetails, EntityClientDetails, EntityClientLOBDetails, EntityHierarchyModel } from '../../models/entity-hierarchy.model'
import { EntityHierarchyService } from '../../services/entity-hierarchy.service';
import { LanguagePreferenceService } from 'src/app/shared/services/language-preference.service';
import { TranslateService } from '@ngx-translate/core';
import { LanguagePreference } from 'src/app/shared/models/language-preference.model';


@Component({
  selector: 'app-entity-hierarchy',
  templateUrl: './entity-hierarchy.component.html',
  styleUrls: ['./entity-hierarchy.component.scss']
})

export class EntityHierarchyComponent implements OnInit {
  clientId: number;
  
  spinnerLeft = 'entityLeft';
  spinnerRight = 'entityRight';
  isHidden: boolean[] = [];
  
  subscriptionList: ISubscription[] = [];
  entityHierarchy: EntityHierarchyModel;
  entityClient: EntityClientDetails;
  entityAgentSchedulingGroups: EntityAgentSchedulingGroupDetails[] = [];
  entityClientLOBs: EntityClientLOBDetails[] = [];
  subscriptions: ISubscription[] = [];

  currentLanguage: string;
  LoggedUser;
  getTranslationSubscription: ISubscription;

  constructor(
    private spinnerService: NgxSpinnerService,
    private clientService: ClientService,
    private entityHierarchyService: EntityHierarchyService,
    private route: ActivatedRoute,
    private authService: AuthService,
    private languagePreferenceService: LanguagePreferenceService,
    public translate: TranslateService,
    ) {
      this.LoggedUser = this.authService.getLoggedUserInfo();
  }


  ngOnInit() {
    this.preLoadTranslations();
    this.loadTranslations();
    this.subscribeToTranslations();
  }

  setClient(client: number) {
    this.clientId = client;
    // logic

    this.getEntityHierarchy();
  }

  getEntityHierarchy(){
    if (this.clientId != null){
      let result;
      result = this.entityHierarchyService.getEntityHierarchyDataById(this.clientId)
      .subscribe((response) => {
        console.log(response)
        this.spinnerService.hide(this.spinnerLeft);
        if (response) {
          this.entityHierarchy = response;
          this.entityClient = this.entityHierarchy.client;
          this.entityAgentSchedulingGroups = this.entityHierarchy.agentSchedulingGroups;
          this.entityClientLOBs = this.entityClient.clientLOBs;
        }
      }, (error) => {
        this.spinnerService.hide(this.spinnerLeft);
        console.log(error);
      });
    }
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
