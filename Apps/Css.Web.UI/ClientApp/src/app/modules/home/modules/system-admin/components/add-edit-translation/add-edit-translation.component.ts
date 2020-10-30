import { Component, Input, OnInit } from '@angular/core';
import {
  FormBuilder,
  FormControl,
  FormGroup,
  Validators,
} from '@angular/forms';
import {
  NgbActiveModal,
  NgbModal,
  NgbModalOptions,
} from '@ng-bootstrap/ng-bootstrap';
import { Translation } from 'src/app/shared/models/translation.model';
import { MessagePopUpComponent } from 'src/app/shared/popups/message-pop-up/message-pop-up.component';
import { CustomValidators } from 'src/app/shared/util/validations.util';
import { TranslationService } from '../../services/translation.service';

@Component({
  selector: 'app-add-edit-translation',
  templateUrl: './add-edit-translation.component.html',
  styleUrls: ['./add-edit-translation.component.scss']
})
export class AddEditTranslationComponent implements OnInit {

  formSubmitted: boolean;
  hasMismatch: number;
  isEdit: boolean;
  translationModel: Translation;
  translationForm: FormGroup;
  languages: string[] = [];
  menus: string[] = [];
  variables: string[] = [];
  editLanguageDetails: Translation;

  @Input() language;
  @Input() menu;
  @Input() variable;
  @Input() title;
  @Input() description;
  @Input() translation;
  @Input() translationData;
  @Input() translationValues: Translation[];

  constructor(
    private formBuilder: FormBuilder,
    public activeModal: NgbActiveModal,
    private modalService: NgbModal,
    private translationService: TranslationService
  ) { }

  ngOnInit(): void {
    this.translationFormIntialization();
    this.getDropdownValues();
    this.checkAddOrEditTranslationDetails();
  }

  get form() {
    return this.translationForm.controls;
  }

  save() {
    this.formSubmitted = true;
    if (this.isEdit && this.translationForm.valid) {
      if (this.matchTranslationDataChanges()) {
        this.saveTranslationDetailsOnModel();
      }
      this.showSuccessPopUpMessage();
    }
    if (!this.isEdit && this.translationForm.valid) {
      this.saveTranslationDetailsOnModel();
      this.showSuccessPopUpMessage();
    }
  }

  hasFormControlValidationError(control: string) {
    return (
      this.formSubmitted &&
      this.translationForm.controls[control].errors?.required
    );
  }

  private checkAddOrEditTranslationDetails() {
    if (this.title === 'Edit') {
      this.isEdit = true;
      this.editLanguageDetails = this.translationService.getTranslationItem(
        this.language, this.menu, this.variable
      );
      this.populateLanguageDetailsOnForm();
    }
  }

  private showSuccessPopUpMessage() {
    const modalRef = this.setSuccessPopUpModalOptions();

    if (this.hasMismatch > 0) {
      modalRef.componentInstance.contentMessage =
        'The record has been updated!';
    }
    if (this.hasMismatch === 0) {
      modalRef.componentInstance.contentMessage = 'No changes has been made!';
      this.activeModal.close('Close click');
    }
    if (!this.isEdit) {
      modalRef.componentInstance.contentMessage = 'The record has been added!';
    }
  }

  private setSuccessPopUpModalOptions() {
    const options: NgbModalOptions = {
      backdrop: false,
      centered: true,
      size: 'sm',
    };
    const modalRef = this.modalService.open(MessagePopUpComponent, options);
    modalRef.componentInstance.headingMessage = 'Success';

    return modalRef;
  }

  private saveTranslationDetailsOnModel() {
    this.translationModel = new Translation();
    if (!this.isEdit) {
      const agentListCount = this.translationService.getTranslationList();
      this.translationModel.createdDate = String(new Date());
      this.translationModel.createdBy = 'User';
    } else if (this.isEdit) {
      this.translationModel.createdDate = this.editLanguageDetails.createdDate;
      this.translationModel.createdBy = this.editLanguageDetails.createdBy;
      this.translationModel.modifiedDate = String(new Date());
      this.translationModel.modifiedBy = this.editLanguageDetails
        .modifiedBy
        ? this.editLanguageDetails.modifiedBy
        : 'User';
    }
    this.translationModel.language = this.translationForm.controls.language.value;
    this.translationModel.menu = this.translationForm.controls.menu.value;
    this.translationModel.variableId = this.translationForm.controls.variableId.value;
    this.translationModel.description = this.translationForm.controls.description.value;
    this.translationModel.translation = this.translationForm.controls.translation.value;

    this.activeModal.close(this.translationModel);
  }

  private matchTranslationDataChanges(): boolean {
    this.hasMismatch = 0;
    this.hasMismatch =
    this.translationForm.controls.language.value ===
        this.translationData.language
        ? this.hasMismatch
        : this.hasMismatch + 1;
    this.hasMismatch =
    this.translationForm.controls.menu.value  ===
        this.translationData.menu
        ? this.hasMismatch
        : this.hasMismatch + 1;
    this.hasMismatch =
    this.translationForm.controls.variableId.value  ===
        this.translationData.variableId
        ? this.hasMismatch
        : this.hasMismatch + 1;
    this.hasMismatch =
    this.translationForm.controls.description.value  ===
        this.translationData.description
        ? this.hasMismatch
        : this.hasMismatch + 1;
    this.hasMismatch =
    this.translationForm.controls.translation.value  ===
        this.translationData.translation
        ? this.hasMismatch
        : this.hasMismatch + 1;

    return this.hasMismatch > 0;
  }

  private populateLanguageDetailsOnForm() {
    this.translationForm.controls.language.setValue(
      this.editLanguageDetails.language
    );
    this.translationForm.controls.menu.setValue(
      this.editLanguageDetails.menu
    );
    this.translationForm.controls.variableId.setValue(
      this.editLanguageDetails.variableId
    );
    this.translationForm.controls.description.setValue(
      this.description
    );
    this.translationForm.controls.translation.setValue(
      this.translation
    );
  }

  private getDropdownValues() {
    this.languages = this.translationService.language;
    const menus = this.translationService.getTranslationList().map((a) => a.menu);
    this.menus = menus.filter((n, i) => menus.indexOf(n) === i);
    const variables = this.translationService.getTranslationList().map((a) => a.variableId);
    this.variables = variables.filter((n, i) => variables.indexOf(n) === i);
  }

  private translationFormIntialization() {
    this.translationForm = this.formBuilder.group({
      language: new FormControl('', Validators.required),
      menu: new FormControl('', Validators.required),
      variableId: new FormControl('', Validators.required),
      description: new FormControl('', Validators.compose([Validators.required, CustomValidators.cannotContainSpace]) ),
      translation: new FormControl('', Validators.compose([Validators.required, CustomValidators.cannotContainSpace]) ),
    });
  }
}
