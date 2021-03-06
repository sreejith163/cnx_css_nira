import { WeekDay } from '@angular/common';
import { Component, Input, OnDestroy, OnInit } from '@angular/core';
import { FormBuilder, FormControl, FormGroup, Validators } from '@angular/forms';
import { NgbActiveModal, NgbModal, NgbModalOptions } from '@ng-bootstrap/ng-bootstrap';
import { NgxSpinnerService } from 'ngx-spinner';
import { SubscriptionLike as ISubscription } from 'rxjs';

import { AddClientLobGroup } from '../../../models/add-client-lob-group.model';
import { ClientLOBGroupDetails } from '../../../models/client-lob-group-details.model';
import { UpdateClientLobGroup } from '../../../models/update-client-lob-group.model';
import { TimeZone } from 'src/app/shared/models/time-zone.model';
import { ComponentOperation } from 'src/app/shared/enums/component-operation.enum';

import { Constants } from 'src/app/shared/util/constants.util';
import { SpinnerOptions } from 'src/app/shared/util/spinner-options.util';

import { TimezoneService } from 'src/app/shared/services/timezone.service';
import { ClientLobGroupService } from '../../../services/client-lob-group.service';
import { ErrorWarningPopUpComponent } from 'src/app/shared/popups/error-warning-pop-up/error-warning-pop-up.component';
import { AuthService } from 'src/app/core/services/auth.service';
import { TranslationDetails } from 'src/app/shared/models/translation-details.model';
import { ContentType } from 'src/app/shared/enums/content-type.enum';

@Component({
  selector: 'app-add-update-client-lob-group',
  templateUrl: './add-update-client-lob-group.component.html',
  styleUrls: ['./add-update-client-lob-group.component.scss']
})
export class AddUpdateClientLobGroupComponent implements OnInit, OnDestroy {

  formSubmitted: boolean;
  clientLOBGroupForm: FormGroup;
  maxLength = Constants.DefaultTextMaxLength;
  spinner = 'spinner';

  addClientLOBGroupSubscription: ISubscription;
  updateClientLOBGroupSubscription: ISubscription;
  getTimeZonesSubscription: ISubscription;
  subscriptionList: ISubscription[] = [];
  weekDays: Array<WeekDay>;
  weekDay = WeekDay;
  clientId?: number;
  timeZones: TimeZone[] = [];
  editClientId: number;

  @Input() operation: ComponentOperation;
  @Input() clientLOBGroupDetails: ClientLOBGroupDetails;
  @Input() translationValues: TranslationDetails[] = [];

  constructor(
    private formBuilder: FormBuilder,
    private modalService: NgbModal,
    private authService: AuthService,
    private clientLobGroupService: ClientLobGroupService,
    private timezoneService: TimezoneService,
    private spinnerService: NgxSpinnerService,
    public activeModal: NgbActiveModal,
  ) { }

  get form() {
    return this.clientLOBGroupForm.controls;
  }

  ngOnInit(): void {
    this.weekDays = Object.keys(WeekDay).filter(key => isNaN(WeekDay[key])).map(x => +x);
    this.intializeClientLobGroupForm();
    this.getTimeZones();
    if (this.operation === ComponentOperation.Edit) {
      this.populateClientLobGroupFormDetails();
    }
    else {
      this.clientLOBGroupForm.controls.firstDayOfWeek.setValue(
        WeekDay.Sunday
      );
    }
    if (this.clientLOBGroupDetails !== undefined && this.clientLOBGroupDetails.clientId !== undefined) {
      this.clientId = this.clientLOBGroupDetails.clientId;
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
    return (this.formSubmitted && this.clientLOBGroupForm.controls[control].errors?.required);
  }

  getTitle() {
    return ComponentOperation[this.operation];
  }

  saveClientLobGroupDetails() {
    this.formSubmitted = true;
    if (this.clientLOBGroupForm.valid && this.clientId) {
      this.operation === ComponentOperation.Edit ? this.updateClientLobGroupDetails() : this.addClientLobGroupDetails();
    }
  }

  private hasClientLobGroupDetailsMismatch() {
    if (this.clientLOBGroupDetails.refId !== this.clientLOBGroupForm.controls.refId.value || 
      this.clientLOBGroupDetails.clientId !== this.clientId ||
      this.clientLOBGroupDetails.name !== this.clientLOBGroupForm.controls.name.value ||
      this.clientLOBGroupDetails.firstDayOfWeek !== this.clientLOBGroupForm.controls.firstDayOfWeek.value ||
      this.clientLOBGroupDetails.timezoneId !== this.clientLOBGroupForm.controls.timeZoneId.value) {
      return true;
    }

    return false;
  }

  private addClientLobGroupDetails() {
    this.clientLOBGroupForm.controls.refId.setValue(this.clientLOBGroupForm.controls.refId.value.toString().replace(/[^0-9]/gi,""));
    const addClientLobGroupModel = this.clientLOBGroupForm.value as AddClientLobGroup;
    addClientLobGroupModel.createdBy = this.authService.getLoggedUserInfo()?.displayName;
    addClientLobGroupModel.clientId = this.clientId;

    this.spinnerService.show(this.spinner, SpinnerOptions);
    this.addClientLOBGroupSubscription = this.clientLobGroupService.addClientLOBGroup(addClientLobGroupModel)
      .subscribe(() => {
        this.spinnerService.hide(this.spinner);
        this.activeModal.close();
      }, (error) => {
        this.spinnerService.hide(this.spinner);
        if (error.status === 409) {
          this.showErrorWarningPopUpMessage(error.error);
        }
      });

    this.subscriptionList.push(this.addClientLOBGroupSubscription);
  }

  isSpecialChar(event)
  {   
    var k;  
    k = event.charCode;
    return((k > 64 && k < 91) || (k > 96 && k < 123) || k == 8 || k == 32 || (k >= 48 && k <= 57)); 
  }

  private updateClientLobGroupDetails() {
    if (this.hasClientLobGroupDetailsMismatch()) {
      this.clientLOBGroupForm.controls.refId.setValue(this.clientLOBGroupForm.controls.refId.value.toString().replace(/[^0-9]/gi,""));
      const updateClientLobGroupModel = this.clientLOBGroupForm.value as UpdateClientLobGroup;
      updateClientLobGroupModel.ModifiedBy = this.authService.getLoggedUserInfo()?.displayName;
      updateClientLobGroupModel.clientId = this.clientId;

      this.spinnerService.show(this.spinner, SpinnerOptions);
      this.updateClientLOBGroupSubscription = this.clientLobGroupService.updateClientLOBGroup(
        this.clientLOBGroupDetails.id, updateClientLobGroupModel)
        .subscribe(() => {
          this.spinnerService.hide(this.spinner);
          this.activeModal.close({ needRefresh: true });
        }, (error) => {
          this.spinnerService.hide(this.spinner);
          if (error.status === 409) {
            this.showErrorWarningPopUpMessage(error.error);
          }
        });

      this.subscriptionList.push(this.updateClientLOBGroupSubscription);
    } else {
      this.activeModal.close({ needRefresh: false });
    }
  }

  private getTimeZones() {
    this.spinnerService.show(this.spinner, SpinnerOptions);
    this.getTimeZonesSubscription = this.timezoneService.getTimeZones()
      .subscribe((response) => {
        this.timeZones = response;
        this.spinnerService.hide(this.spinner);
      }, (error) => {
        console.log(error);
        this.spinnerService.hide(this.spinner);
      });

    this.subscriptionList.push(this.getTimeZonesSubscription);
  }

  setClient(client: number) {
    this.clientId = client;
  }

  private populateClientLobGroupFormDetails() {
    this.clientLOBGroupForm.controls.refId.setValue(this.clientLOBGroupDetails.refId);
    this.clientLOBGroupForm.controls.name.setValue(
      this.clientLOBGroupDetails.name);
    this.editClientId = this.clientLOBGroupDetails.clientId;
    this.clientLOBGroupForm.controls.firstDayOfWeek.setValue(
      this.clientLOBGroupDetails.firstDayOfWeek
    );
    this.clientLOBGroupForm.controls.timeZoneId.setValue(
      this.clientLOBGroupDetails.timezoneId
    );
  }

  private showErrorWarningPopUpMessage(contentMessage: string) {
    const options: NgbModalOptions = { backdrop: 'static', centered: true, size: 'sm' };
    const modalRef = this.modalService.open(ErrorWarningPopUpComponent, options);
    modalRef.componentInstance.headingMessage = 'Error';
    modalRef.componentInstance.contentMessage = contentMessage;
    modalRef.componentInstance.messageType = ContentType.String;

    return modalRef;
  }

  private intializeClientLobGroupForm() {
    this.clientLOBGroupForm = this.formBuilder.group({
      refId: new FormControl('', Validators.compose([
        Validators.maxLength(10)])),
      name: new FormControl('', Validators.compose([
        Validators.required,
        Validators.maxLength(50)])),
      firstDayOfWeek: new FormControl('', Validators.required),
      timeZoneId: new FormControl('', Validators.required),
    });
  }

  getWeekDay(weekDay: number) {
    return WeekDay[weekDay];
  }

  getTranslationIdForWeek(weekDay: number) {
    return 'radio_add_edit_first_day_of_week_' + this.getWeekDay(weekDay)?.toLowerCase();
  }
}
