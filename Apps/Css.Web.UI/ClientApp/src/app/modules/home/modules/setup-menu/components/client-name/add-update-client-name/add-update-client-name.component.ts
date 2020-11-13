import { Component, OnInit, Input, OnDestroy } from '@angular/core';
import { FormGroup, FormBuilder, FormControl, Validators } from '@angular/forms';
import { NgbActiveModal, NgbModal, NgbModalOptions } from '@ng-bootstrap/ng-bootstrap';
import { NgxSpinnerService } from 'ngx-spinner';
import { SubscriptionLike as ISubscription } from 'rxjs';

import { MessagePopUpComponent } from 'src/app/shared/popups/message-pop-up/message-pop-up.component';
import { ErrorWarningPopUpComponent } from 'src/app/shared/popups/error-warning-pop-up/error-warning-pop-up.component';

import { AddClient } from '../../../models/add-client.model';
import { ClientDetails } from '../../../models/client-details.model';
import { UpdateClient } from '../../../models/update-client.model';
import { Translation } from 'src/app/shared/models/translation.model';
import { ComponentOperation } from 'src/app/shared/enums/component-operation.enum';

import { ClientService } from '../../../services/client.service';
import { CustomValidators } from 'src/app/shared/util/validations.util';
import { SpinnerOptions } from 'src/app/shared/util/spinner-options.util';
import { Constants } from 'src/app/shared/util/constants.util';
import { AuthService } from 'src/app/core/services/auth.service';

@Component({
  selector: 'app-add-update-client-list',
  templateUrl: './add-update-client-name.component.html',
  styleUrls: ['./add-update-client-name.component.css']
})
export class AddUpdateClientNameComponent implements OnInit, OnDestroy {

  maxLength = Constants.DefaultTextMaxLength;
  formSubmitted: boolean;
  spinner = 'spinner';
  clientForm: FormGroup;

  addClientSubscription: ISubscription;
  updateClientSubscription: ISubscription;
  subscriptionList: ISubscription[] = [];

  @Input() operation: ComponentOperation;
  @Input() clientDetails: ClientDetails;
  @Input() translationValues: Translation[] = [];

  constructor(
    private formBuilder: FormBuilder,
    private modalService: NgbModal,
    private authService: AuthService,
    private clientService: ClientService,
    public activeModal: NgbActiveModal,
    private spinnerService: NgxSpinnerService,
  ) { }

  get form() {
    return this.clientForm.controls;
  }

  ngOnInit(): void {
    this.intializeClientForm();
    if (this.operation === ComponentOperation.Edit) {
      this.populateClientFormDetails();
    }
  }

  ngOnDestroy() {
    this.subscriptionList.forEach(subscription => {
      if (subscription) {
        subscription.unsubscribe();
      }
    });
  }

  hasFormControlValidationError(control: string) {
    return (this.formSubmitted && this.clientForm.controls[control].errors?.required);
  }

  getTitle() {
    return ComponentOperation[this.operation];
  }

  saveClientDetails() {
    this.formSubmitted = true;
    if (this.clientForm.valid) {
      this.operation === ComponentOperation.Edit ? this.updateClientDetails() : this.addClientDetails();
    }
  }

  private hasClientDetailsMismatch() {
    return this.clientDetails.name !== this.clientForm.value.name;
  }

  private addClientDetails() {
    const addClientModel = this.clientForm.value as AddClient;
    addClientModel.createdBy = this.authService.getLoggedUserInfo()?.displayName;

    this.spinnerService.show(this.spinner, SpinnerOptions);
    this.addClientSubscription = this.clientService.addClient(addClientModel)
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

    this.subscriptionList.push(this.addClientSubscription);
  }

  private updateClientDetails() {
    if (this.hasClientDetailsMismatch()) {
      this.spinnerService.show(this.spinner, SpinnerOptions);
      const updateClientModel = this.clientForm.value as UpdateClient;
      updateClientModel.ModifiedBy = this.authService.getLoggedUserInfo()?.displayName;

      this.updateClientSubscription = this.clientService.updateClient(this.clientDetails.id, updateClientModel)
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
      this.subscriptionList.push(this.updateClientSubscription);
    } else {
      this.activeModal.close({ needRefresh: false });
      this.showSuccessPopUpMessage('No changes has been made!');
    }
  }

  private showErrorWarningPopUpMessage(contentMessage: string) {
    const options: NgbModalOptions = { backdrop: false, centered: true, size: 'sm' };
    const modalRef = this.modalService.open(ErrorWarningPopUpComponent, options);
    modalRef.componentInstance.headingMessage = 'Error';
    modalRef.componentInstance.contentMessage = contentMessage;

    return modalRef;
  }

  private showSuccessPopUpMessage(contentMessage: string) {
    const options: NgbModalOptions = { backdrop: false, centered: true, size: 'sm' };
    const modalRef = this.modalService.open(MessagePopUpComponent, options);
    modalRef.componentInstance.headingMessage = 'Success';
    modalRef.componentInstance.contentMessage = contentMessage;

    return modalRef;
  }

  private populateClientFormDetails() {
    this.clientForm.controls.name.setValue(this.clientDetails.name);
  }

  private intializeClientForm() {
    this.clientForm = this.formBuilder.group({
      name: new FormControl('', Validators.compose([
        Validators.required,
        Validators.maxLength(50),
        CustomValidators.cannotContainSpace
      ]))
    });
  }
}
