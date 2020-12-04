import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { NgbActiveModal, NgbModal, NgbModalOptions } from '@ng-bootstrap/ng-bootstrap';
import { SubscriptionLike as ISubscription } from 'rxjs';


import { CssLanguageService } from '../../../services/css-language.service';
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
import { MessagePopUpComponent } from 'src/app/shared/popups/message-pop-up/message-pop-up.component';
import { ErrorWarningPopUpComponent } from 'src/app/shared/popups/error-warning-pop-up/error-warning-pop-up.component';
import { TranslationDetails } from 'src/app/shared/models/translation-details.model';



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
  languages: KeyValue;
  menus: KeyValue;
  variables: KeyValue[] = [];

  updateLanguageTranslationSubscription: ISubscription;
  addLanguageTranslationSubscription: ISubscription;
  getLanguageTranslationSubscription: ISubscription;
  getCssMenuVariablesSubscription: ISubscription;
  getCssMenusSubscription: ISubscription;
  getCssLanguagesSubscription: ISubscription;
  subscriptions: ISubscription[] = [];

  @Input() operation: ComponentOperation;
  @Input() translationId: number;
  @Input() translationValues: TranslationDetails[];

  constructor(
    private formBuilder: FormBuilder,
    public activeModal: NgbActiveModal,
    private modalService: NgbModal,
    private translationService: LanguageTranslationService,
    private cssMenuService: CssMenuService,
    private cssLanguageService: CssLanguageService,
    private spinnerService: NgxSpinnerService,
    private authService: AuthService
  ) { }

  ngOnInit(): void {
    this.translationFormIntialization();
    this.getCssLanguages();
    this.getCssMenus();
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
    this.getCssMenuVariables(this.menuId);
  }

  getCssMenuVariables(menu: number) {
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
        this.activeModal.close({ needRefresh: true });
        this.showSuccessPopUpMessage('The record has been added!');
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
          this.showSuccessPopUpMessage('The record has been updated!');
        }, (error) => {
          this.spinnerService.hide(this.spinner);
          if (error.status === 409) {
            this.showErrorWarningPopUpMessage(error.error);
          }
        });
      this.subscriptions.push(this.updateLanguageTranslationSubscription);
    } else {
      this.activeModal.close({ needRefresh: false });
      this.showSuccessPopUpMessage('No changes has been made!');
    }
  }

  private hasTranslationDetailsMismatch() {
    for (const property in this.translationForm.value) {
      if (this.translationForm.controls[property].value !== this.translationData[property]) {
        return true;
      }
    }
  }

  private showSuccessPopUpMessage(contentMessage: string) {
    const options: NgbModalOptions = {
      backdrop: false,
      centered: true,
      size: 'sm',
    };
    const modalRef = this.modalService.open(MessagePopUpComponent, options);
    modalRef.componentInstance.headingMessage = 'Success';
    modalRef.componentInstance.contentMessage = contentMessage;

    return modalRef;
  }

  private showErrorWarningPopUpMessage(contentMessage: string) {
    const options: NgbModalOptions = { backdrop: false, centered: true, size: 'sm' };
    const modalRef = this.modalService.open(ErrorWarningPopUpComponent, options);
    modalRef.componentInstance.headingMessage = 'Error';
    modalRef.componentInstance.contentMessage = contentMessage;

    return modalRef;
  }

  private getCssLanguages() {
    this.getCssLanguagesSubscription = this.cssLanguageService.getCssLanguages()
      .subscribe((response) => {
        if (response) {
          this.languages = response;
        }
        this.spinnerService.hide(this.spinner);
      }, (error) => {
        this.spinnerService.hide(this.spinner);
        console.log(error);
      });

    this.subscriptions.push(this.getCssLanguagesSubscription);
  }

  private getCssMenus() {
    this.getCssMenusSubscription = this.cssMenuService.getCssMenus()
      .subscribe((response) => {
        if (response) {
          this.menus = response;
        }
        this.spinnerService.hide(this.spinner);
      }, (error) => {
        this.spinnerService.hide(this.spinner);
        console.log(error);
      });

    this.subscriptions.push(this.getCssMenusSubscription);
  }

  private loadTranslation() {
    this.getLanguageTranslationSubscription = this.translationService.getLanguageTranslation(this.translationId)
      .subscribe((response) => {
        if (response) {
          this.translationData = response;
          this.getCssMenuVariables(this.translationData?.menuId);
        }
        this.spinnerService.hide(this.spinner);
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
      this.translationData.description
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
      description: new FormControl('', Validators.compose([
        Validators.required,
        Validators.maxLength(Constants.DefaultTextMaxLength),
        CustomValidators.cannotContainSpace])),
      translation: new FormControl('', Validators.compose([
        Validators.required,
        Validators.maxLength(Constants.DefaultTextMaxLength),
        CustomValidators.cannotContainSpace])),
    });
  }

}
