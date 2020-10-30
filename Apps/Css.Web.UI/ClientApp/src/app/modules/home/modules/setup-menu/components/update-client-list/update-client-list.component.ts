import { Component, OnInit, Input } from '@angular/core';
import { FormGroup, FormBuilder, FormControl, Validators } from '@angular/forms';
import { NgbActiveModal, NgbModal, NgbModalOptions } from '@ng-bootstrap/ng-bootstrap';
import { Translation } from 'src/app/shared/models/translation.model';
import { MessagePopUpComponent } from 'src/app/shared/popups/message-pop-up/message-pop-up.component';
import { CustomValidators } from 'src/app/shared/util/validations.util';
import { ClientDetails } from '../../models/client-details.model';
import { ClientNameListService } from '../../services/client-name-list.service';

@Component({
  selector: 'app-update-client-list',
  templateUrl: './update-client-list.component.html',
  styleUrls: ['./update-client-list.component.css']
})
export class UpdateClientListComponent implements OnInit {

  isEdit: boolean;
  formSubmitted: boolean;
  hasMismatch: boolean;
  clientForm: FormGroup;
  clientModel: ClientDetails;

  @Input() title: string;
  @Input() clientDetails: ClientDetails;
  @Input() translationValues: Translation[];

  constructor(
    private formBuilder: FormBuilder,
    public activeModal: NgbActiveModal,
    private modalService: NgbModal,
    private clienNameListService: ClientNameListService,
  ) { }

  get form() {
    return this.clientForm.controls;
  }

  ngOnInit(): void {
    this.clientForm = this.formBuilder.group({
      clientName: new FormControl('', Validators.compose([Validators.required, CustomValidators.cannotContainSpace]))
    });

    if (this.title === 'Edit') {
      this.isEdit = true;
      this.populateFormDetails();
    }
  }

  hasFormControlValidationError(control: string) {
    return (
      this.formSubmitted &&
      this.clientForm.controls[control].errors?.required
    );
  }

  save() {
    this.formSubmitted = true;
    if (this.clientForm.valid) {
      this.saveClientProfileDetailsOnModel();
      if (this.isEdit) {
        this.updateClientProfileDetails();
      } else {
        this.addClientProfileDetails();
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

  private matchClientDataChages() {
    this.hasMismatch =
      this.clientDetails.clientName === this.clientForm.value.clientName ? false : true;
  }

  private updateClientProfileDetails() {
    this.matchClientDataChages();

    if (this.hasMismatch) {
      this.clientModel.id = this.clientDetails.id;
      this.clientModel.refId = this.clientDetails.refId;
      this.clientModel.createdDate = this.clientDetails.createdDate;
      this.clientModel.createdBy = this.clientDetails.createdBy;
      this.clienNameListService.updateClientDetials(this.clientModel);
      this.activeModal.close();
      this.showSuccessPopUpMessage('The record has been updated!');
    } else {
      this.activeModal.close();
      this.showSuccessPopUpMessage('No changes has been made!');
    }
  }

  private addClientProfileDetails() {
    this.clienNameListService.addClientDetails(this.clientModel);
    this.activeModal.close();
    this.showSuccessPopUpMessage('The record has been added!');
  }

  private saveClientProfileDetailsOnModel() {
    this.clientModel = new ClientDetails();
    this.clientModel.clientName = this.clientForm.controls.clientName.value;
  }

  private populateFormDetails() {
    this.clientForm.controls.clientName.setValue(
      this.clientDetails.clientName
    );
  }
}
