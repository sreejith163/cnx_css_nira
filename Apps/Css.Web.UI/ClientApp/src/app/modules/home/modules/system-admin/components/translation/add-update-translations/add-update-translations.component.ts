import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { NgbActiveModal, NgbModal, NgbModalOptions } from '@ng-bootstrap/ng-bootstrap';
import { SubscriptionLike as ISubscription } from 'rxjs';

import { CssLanguageService } from '../../../../../../../shared/services/css-language.service';
import { CssMenuService } from '../../../services/css-menu.service';
import { AuthService } from 'src/app/core/services/auth.service';
import { NgxSpinnerService } from 'ngx-spinner';
import { LanguageTranslationService } from 'src/app/shared/services/language-translation.service';

import { UpdateTranslation } from '../../../../../../../shared/models/update-translation.model';
import { KeyValue } from 'src/app/shared/models/key-value.model';
import { ComponentOperation } from 'src/app/shared/enums/component-operation.enum';
import { TranslationBase } from '../../../../../../../shared/models/translation-base.model';
import { AddTranslation } from '../../../../../../../shared/models/add-translation.model';

import { SpinnerOptions } from 'src/app/shared/util/spinner-options.util';
import { CustomValidators } from 'src/app/shared/util/validations.util';
import { Constants } from 'src/app/shared/util/constants.util';
import { ErrorWarningPopUpComponent } from 'src/app/shared/popups/error-warning-pop-up/error-warning-pop-up.component';
import { TranslationDetails } from 'src/app/shared/models/translation-details.model';
import { VariableResponse } from '../../../models/variable-response.model';
import { ContentType } from 'src/app/shared/enums/content-type.enum';

@Component({
  selector: 'app-add-update-translations',
  templateUrl: './add-update-translations.component.html',
  styleUrls: ['./add-update-translations.component.scss']
})

export class AddUpdateTranslationsComponent implements OnInit, OnDestroy {

  menuId: number;
  formSubmitted: boolean;
  spinner = 'AddUpdateTransaltion';
  maxLength = Constants.DefaultTextMaxLength;

  translationForm: FormGroup;
  translationData: TranslationBase;
  languages: KeyValue[] = [];
  menus: KeyValue[] = [];
  variables: VariableResponse[] = [];

  updateLanguageTranslationSubscription: ISubscription;
  addLanguageTranslationSubscription: ISubscription;
  getLanguageTranslationSubscription: ISubscription;
  getCssMenuVariablesSubscription: ISubscription;
  getCssMenuSubscription: ISubscription;
  getCssLanguageSubscription: ISubscription;
  subscriptions: ISubscription[] = [];

  @Input() operation: ComponentOperation;
  @Input() translationId: number;
  @Input() translationValues: TranslationDetails[];

  constructor(
    private formBuilder: FormBuilder,
    private modalService: NgbModal,
    private translationService: LanguageTranslationService,
    private cssMenuService: CssMenuService,
    private cssLanguageService: CssLanguageService,
    private spinnerService: NgxSpinnerService,
    private authService: AuthService,
    public activeModal: NgbActiveModal,
  ) { }

  ngOnInit(): void {
    this.translationFormIntialization();
    this.getCssLanguage();
    this.getCssMenu();
    if (this.operation === ComponentOperation.Edit) {
      this.loadTranslation();
    }
  }

  ngOnDestroy() {
    this.subscriptions.forEach(subscription => {
      if (subscription) {
        subscription.unsubscribe();
      }
    });
  }

  get form() {
    return this.translationForm.controls;
  }

  getTitle() {
    return ComponentOperation[this.operation];
  }

  hasFormControlValidationError(control: string) {
    return (
      this.formSubmitted &&
      this.translationForm.controls[control].errors?.required
    );
  }

  setMenuId(menu: number) {
    this.menuId = menu;
    this.translationForm.controls.variableId.setValue('');
    this.translationForm.controls.description.setValue('');
    this.variables = [];
    this.getCssMenuVariables(this.menuId);
  }

  setVariableDescription(variableId: number) {
    const variable = this.variables.find(x => x.id === +variableId);
    this.translationForm.controls.description.setValue(variable ? variable.description : '');
  }

  saveTranslation() {
    this.formSubmitted = true;
    if (this.translationForm.valid) {
      this.operation === ComponentOperation.Edit ? this.updateTranslation() : this.addTranslation();
    }
  }

  private addTranslation() {
    const addTransaltion = this.translationForm.value as AddTranslation;
    addTransaltion.createdBy = this.authService.getLoggedUserInfo().displayName;

    this.spinnerService.show(this.spinner, SpinnerOptions);
    this.addLanguageTranslationSubscription = this.translationService.addLanguageTranslation(addTransaltion)
      .subscribe(() => {
        this.spinnerService.hide(this.spinner);
        this.activeModal.close();
      }, (error) => {
        this.spinnerService.hide(this.spinner);
        if (error.status === 409) {
          this.showErrorWarningPopUpMessage(error.error);
        }
      });

    this.subscriptions.push(this.addLanguageTranslationSubscription);
  }

  private updateTranslation() {
    if (this.hasTranslationDetailsMismatch()) {
      const updateTranslationModel = this.translationForm.value as UpdateTranslation;
      updateTranslationModel.modifiedBy = this.authService.getLoggedUserInfo().displayName;

      this.spinnerService.show(this.spinner, SpinnerOptions);
      this.updateLanguageTranslationSubscription = this.translationService.updateLanguageTranslation
        (this.translationId, updateTranslationModel)
        .subscribe(() => {
          this.spinnerService.hide(this.spinner);
          this.activeModal.close({ needRefresh: true });
        }, (error) => {
          this.spinnerService.hide(this.spinner);
          if (error.status === 409) {
            this.showErrorWarningPopUpMessage(error.error);
          }
        });
      this.subscriptions.push(this.updateLanguageTranslationSubscription);
    } else {
      this.activeModal.close({ needRefresh: false });
    }
  }

  private hasTranslationDetailsMismatch() {
    for (const property in this.translationForm.value) {
      if (property !== 'description' && property !== 'translation') {
        if (+this.translationForm.controls[property].value !==
          this.translationData[property]) {
          return true;
        }
      } else if (property === 'translation') {
        if (this.translationForm.controls[property].value !==
          this.translationData[property]) {
          return true;
        }
      }
    }

  }

  private showErrorWarningPopUpMessage(contentMessage: string) {
    const options: NgbModalOptions = { backdrop: 'static', centered: true, size: 'sm' };
    const modalRef = this.modalService.open(ErrorWarningPopUpComponent, options);
    modalRef.componentInstance.headingMessage = 'Error';
    modalRef.componentInstance.contentMessage = contentMessage;
    modalRef.componentInstance.messageType = ContentType.String;

    return modalRef;
  }

  private getCssLanguage() {
    this.spinnerService.show(this.spinner, SpinnerOptions);

    this.getCssLanguageSubscription = this.cssLanguageService.getCssLanguages()
      .subscribe((response) => {
        if (response) {
          this.languages = response;
        }
        this.spinnerService.hide(this.spinner);
      }, (error) => {
        this.spinnerService.hide(this.spinner);
        console.log(error);
      });

    this.subscriptions.push(this.getCssLanguageSubscription);
  }

  private getCssMenu() {
    this.spinnerService.show(this.spinner, SpinnerOptions);

    this.getCssMenuSubscription = this.cssMenuService.getCssMenu()
      .subscribe((response) => {
        if (response) {
          this.menus = response;
        }
        this.spinnerService.hide(this.spinner);
      }, (error) => {
        this.spinnerService.hide(this.spinner);
        console.log(error);
      });

    this.subscriptions.push(this.getCssMenuSubscription);
  }

  private getCssMenuVariables(menu: number) {
    this.spinnerService.show(this.spinner, SpinnerOptions);

    this.getCssMenuVariablesSubscription = this.cssMenuService.getCssMenuVariables(menu)
      .subscribe((response) => {
        this.variables = response;
        if (this.operation === ComponentOperation.Edit && !this.menuId) {
          this.populateFormDetails();
        }
        this.spinnerService.hide(this.spinner);
      }, (error) => {
        this.spinnerService.hide(this.spinner);
        console.log(error);
      });

    this.subscriptions.push(this.getCssMenuVariablesSubscription);
  }

  private loadTranslation() {
    this.spinnerService.show(this.spinner, SpinnerOptions);

    this.getLanguageTranslationSubscription = this.translationService.getLanguageTranslation(this.translationId)
      .subscribe((response) => {
        if (response) {
          this.spinnerService.hide(this.spinner);
          this.translationData = response;
          this.getCssMenuVariables(this.translationData?.menuId);
        }
      }, (error) => {
        this.spinnerService.hide(this.spinner);
        console.log(error);
      });

    this.subscriptions.push(this.getLanguageTranslationSubscription);
  }

  private populateFormDetails() {
    this.translationForm.controls.languageId.setValue(
      this.translationData.languageId
    );
    this.translationForm.controls.menuId.setValue(
      this.translationData.menuId
    );
    this.translationForm.controls.variableId.setValue(
      this.translationData.variableId
    );
    this.translationForm.controls.description.setValue(
      this.translationData.variableDescription
    );
    this.translationForm.controls.translation.setValue(
      this.translationData.translation
    );
  }

  private translationFormIntialization() {
    this.translationForm = this.formBuilder.group({
      languageId: new FormControl('', Validators.required),
      menuId: new FormControl('', Validators.required),
      variableId: new FormControl('', Validators.required),
      description: new FormControl('', Validators.required),
      translation: new FormControl('', Validators.compose([
        Validators.required,
        Validators.maxLength(Constants.DefaultTextMaxLength),
        CustomValidators.cannotContainSpace])),
    });
  }
}
