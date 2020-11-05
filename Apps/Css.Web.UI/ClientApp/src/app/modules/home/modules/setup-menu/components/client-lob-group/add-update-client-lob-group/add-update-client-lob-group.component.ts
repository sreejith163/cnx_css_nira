import { Component, OnInit, Input, OnDestroy } from '@angular/core';

import { FormGroup, FormBuilder, FormControl, Validators } from '@angular/forms';
import { NgbActiveModal, NgbModal, NgbModalOptions } from '@ng-bootstrap/ng-bootstrap';
import { Translation } from 'src/app/shared/models/translation.model';
import { MessagePopUpComponent } from 'src/app/shared/popups/message-pop-up/message-pop-up.component';
import { CustomValidators } from 'src/app/shared/util/validations.util';
import { AddClient } from '../../../models/add-client.model';
import { ClientDetails } from '../../../models/client-details.model';
import { UpdateClient } from '../../../models/update-client.model';
import { ClientService } from '../../../services/client.service';
import { SubscriptionLike as ISubscription } from 'rxjs';
import { ComponentOperation } from 'src/app/shared/enums/component-operation.enum';
import { ClientLOBGroupDetails } from '../../../models/client-lob-group-details.model';
import { ClientLobGroupService } from '../../../services/client-lob-group.service';
import { AddClientLobGroup } from '../../../models/add-client-lob-group.model';
import { UpdateClientLobGroup } from '../../../models/update-client-lob-group.model';
import { WeekDay } from '@angular/common';
import { LobGroupTimezone } from '../../../models/lob-group-timezone.model';

@Component({
  selector: 'app-add-update-client-lob-group',
  templateUrl: './add-update-client-lob-group.component.html',
  styleUrls: ['./add-update-client-lob-group.component.scss']
})
export class AddUpdateClientLobGroupComponent implements OnInit, OnDestroy {

  formSubmitted: boolean;
  clientLOBGroupForm: FormGroup;

  addClientLOBGroupSubscription: ISubscription;
  updateClientLOBGroupSubscription: ISubscription;
  getTimeZonesSubscription: ISubscription;
  subscriptionList: ISubscription[] = [];
  weekDays: Array<WeekDay>;
  weekDay = WeekDay;
  clientId: number;
  timeZones: LobGroupTimezone[] = [];



  @Input() operation: ComponentOperation;
  @Input() clientLOBGroupDetails: ClientLOBGroupDetails;
  @Input() translationValues: Translation[] = [];
  
  constructor(
    private formBuilder: FormBuilder,
    private modalService: NgbModal,
    private clientLobGroupService: ClientLobGroupService,
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
    if (this.operation === ComponentOperation.Add) {
      this.clientLOBGroupForm.controls.firstDayOfWeek.setValue(0);
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
    console.log("savestart");
    this.formSubmitted = true;
    if (this.clientLOBGroupForm.valid) {
      console.log("formvalid");
      this.operation === ComponentOperation.Edit ? this.updateClientLobGroupDetails() : this.addClientLobGroupDetails();
    }
  }

  private hasClientLobGroupDetailsMismatch() {
    return true;
  }

  private addClientLobGroupDetails() {
    console.log("addClientLobGroupModel");

    const addClientLobGroupModel = new AddClientLobGroup();
    addClientLobGroupModel.refId = 1;
    addClientLobGroupModel.name = this.clientLOBGroupForm.controls.clientLOBGroup.value;
    addClientLobGroupModel.createdBy = 'User';
    addClientLobGroupModel.clientId = this.clientId;
    addClientLobGroupModel.firstDayOfWeek = this.clientLOBGroupForm.controls.firstDayOfWeek.value;
    addClientLobGroupModel.timeZoneId = this.clientLOBGroupForm.controls.timeZoneForReporting.value;
 
    console.log(addClientLobGroupModel);

    this.addClientLOBGroupSubscription = this.clientLobGroupService.addClientLOBGroup(addClientLobGroupModel)
      .subscribe(() => {
        this.activeModal.close();
        this.showSuccessPopUpMessage('The record has been added!');
      }, (error) => {
        console.log(error);
      });

    this.subscriptionList.push(this.addClientLOBGroupSubscription);
  }

  private updateClientLobGroupDetails() {
    if (this.hasClientLobGroupDetailsMismatch()) {
      const updateClientLobGroupModel = new UpdateClientLobGroup();
      updateClientLobGroupModel.name = this.clientLOBGroupForm.controls.clientLobGroup.value;
      updateClientLobGroupModel.ModifiedBy = 'User';
      updateClientLobGroupModel.firstDayOfWeek = this.clientLOBGroupForm.controls.firstDayOfWeek.value;
      updateClientLobGroupModel.timeZoneForReporting = this.clientLOBGroupForm.controls.timeZoneForReporting.value;
 
      
      this.updateClientLOBGroupSubscription = this.clientLobGroupService.updateClientLOBGroup(this.clientLOBGroupDetails.id, updateClientLobGroupModel)
        .subscribe(() => {
          this.activeModal.close();
          this.showSuccessPopUpMessage('The record has been updated!');
        }, (error) => {
          console.log(error);
        });

      this.subscriptionList.push(this.updateClientLOBGroupSubscription);
    } else {
      this.activeModal.close();
      this.showSuccessPopUpMessage('No changes has been made!');
    }
  }

  private showSuccessPopUpMessage(contentMessage: string) {
    const options: NgbModalOptions = { backdrop: false, centered: true, size: 'sm' };
    const modalRef = this.modalService.open(MessagePopUpComponent, options);
    modalRef.componentInstance.headingMessage = 'Success';
    modalRef.componentInstance.contentMessage = contentMessage;

    return modalRef;
  }

  private getTimeZones() {
    this.getTimeZonesSubscription = this.clientLobGroupService.getTimeZones()
      .subscribe((response) => {
        this.timeZones = response;
      }, (error) => {
        console.log(error);
      });

    this.subscriptionList.push(this.getTimeZonesSubscription);
  }

  setClient(client: number) {
    this.clientId = client;
  }

  private populateClientLobGroupFormDetails() {
    this.clientLOBGroupForm.controls.clientLobGroup.setValue(
      this.clientLOBGroupDetails.name);

    // this.clientLOBGroupForm.controls.clientName.setValue(
    //   this.clientLOBGroupDetails.clientName
    // );   
    this.clientLOBGroupForm.controls.firstDayOfWeek.setValue(
      this.clientLOBGroupDetails.firstDayOfWeek
    );
    this.clientLOBGroupForm.controls.timeZoneForReporting.setValue(
      this.clientLOBGroupDetails.timeZoneForReporting
    );
  }

  private intializeClientLobGroupForm() {  

    this.clientLOBGroupForm = this.formBuilder.group({
      clientLOBGroup: new FormControl('', Validators.required),
      firstDayOfWeek: new FormControl('', Validators.required),
      timeZoneForReporting: new FormControl('', Validators.required),
    });
  }

  getWeekDay(weekDay: number) {	
    return WeekDay[weekDay];	
  }

  getTranslationIdForWeek(weekDay: number) {
    return 'radio_add_edit_first_day_of_week_' + this.getWeekDay(weekDay)?.toLowerCase();
  }
}
