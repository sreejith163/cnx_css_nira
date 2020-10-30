import { WeekDay } from '@angular/common';
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
import { ClientLOBName } from '../../../scheduling-menu/models/client-lob-name.model';
import { ClientLOBGroupDetails } from '../../models/client-lob-group-details.model';
import { ClientNameList } from '../../models/client-name-list.model';
import { TimeZone } from '../../models/time-zone.model';
import { ClientLobGroupDropdownService } from '../../services/client-lob-group-dropdown.service';
import { ClientLobGroupListService } from '../../services/client-lob-group-list.service';

@Component({
  selector: 'app-add-edit-client-lob-group',
  templateUrl: './add-edit-client-lob-group.component.html',
  styleUrls: ['./add-edit-client-lob-group.component.scss'],
})
export class AddEditClientLobGroupComponent implements OnInit {

  formSubmitted: boolean;
  hasMismatch: boolean;
  isEdit: boolean;
  weekDays: Array<WeekDay>;
  clientLobGroupModel: ClientLOBGroupDetails;
  clientLOBGroupForm: FormGroup;
  weekDay = WeekDay;
  clientNames: ClientNameList[] = [];
  clientLOBGroups: ClientLOBName[] = [];
  timeZoneList: TimeZone[] = [];


  @Input() title: string;
  @Input() clientLobGroupData: ClientLOBGroupDetails;
  @Input() translationValues: Translation[];

  constructor(
    private clientLobGroupListService: ClientLobGroupListService,
    private formBuilder: FormBuilder,
    public activeModal: NgbActiveModal,
    private modalService: NgbModal,
    private clientLobGroupDropdownService: ClientLobGroupDropdownService
  ) { }

  ngOnInit(): void {
    this.weekDays = Object.keys(WeekDay).filter(key => isNaN(WeekDay[key])).map(x => +x);
    this.agentFormIntialization();
    this.getDropdownValues();
    this.checkAddOrEditClientLOBGroupDetails();
  }

  get form() {
    return this.clientLOBGroupForm.controls;
  }

  save() {
    this.formSubmitted = true;
    if (this.clientLOBGroupForm.valid) {
      this.saveClientLOBGroupDetailsOnModel();
      if (this.isEdit) {
        this.updateClientLOBGroupDetails();
      } else {
        this.addClientLOBGroupDetails();
      }
    }
  }

  hasFormControlValidationError(control: string) {
    return (
      this.formSubmitted &&
      this.clientLOBGroupForm.controls[control].errors?.required
    );
  }

  getWeekDay(weekDay: number) {
    return WeekDay[weekDay];
  }

  getTranslationIdForWeek(weekDay: number) {
    return 'radio_add_edit_first_day_of_week_' + this.getWeekDay(weekDay)?.toLowerCase();
  }

  private checkAddOrEditClientLOBGroupDetails() {
    if (this.title === 'Edit') {
      this.isEdit = true;
      this.populateAgentDetailsOnAgentForm();
    }
    if (this.title === 'Add') {
      this.clientLOBGroupForm.controls.firstDayOfWeek.setValue(0);
    }
  }

  private addClientLOBGroupDetails() {
    this.clientLobGroupListService.addClientLOBGroup(this.clientLobGroupModel);
    this.activeModal.close();
    this.showSuccessPopUpMessage('The record has been added!');
  }

  private updateClientLOBGroupDetails() {
    this.matchClientLOBGroupDataChanges();

    if (this.hasMismatch) {
      this.clientLobGroupModel.id = this.clientLobGroupData.id;
      this.clientLobGroupModel.refId = this.clientLobGroupData.refId;
      this.clientLobGroupModel.createdDate = this.clientLobGroupData.createdDate;
      this.clientLobGroupModel.createdBy = this.clientLobGroupData.createdBy;
      this.clientLobGroupListService.updateClientLOBGroup(this.clientLobGroupModel);
      this.activeModal.close();
      this.showSuccessPopUpMessage('The record has been updated!');
    } else {
      this.activeModal.close();
      this.showSuccessPopUpMessage('No changes has been made!');
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

  private saveClientLOBGroupDetailsOnModel() {
    this.clientLobGroupModel = new ClientLOBGroupDetails();
    this.clientLobGroupModel.clientName = this.clientLOBGroupForm.controls.clientName.value;
    this.clientLobGroupModel.clientLOBGroupName = this.clientLOBGroupForm.controls.clientLOBGroup.value;
    this.clientLobGroupModel.firstDayOfWeek = this.clientLOBGroupForm.controls.firstDayOfWeek.value;
    this.clientLobGroupModel.timeZoneForReporting = this.clientLOBGroupForm.controls.timeZoneForReporting.value;
  }

  private matchClientLOBGroupDataChanges() {

    for (const propertyName in this.clientLOBGroupForm.value) {
      if (
        this.clientLobGroupModel[propertyName] !==
        this.clientLobGroupData[propertyName]
      ) {
        this.hasMismatch = true;
        break;
      }
    }
  }

  private populateAgentDetailsOnAgentForm() {
    this.clientLOBGroupForm.controls.clientName.setValue(
      this.clientLobGroupData.clientName
    );
    this.clientLOBGroupForm.controls.clientLOBGroup.setValue(
      this.clientLobGroupData.clientLOBGroupName
    );
    this.clientLOBGroupForm.controls.firstDayOfWeek.setValue(
      this.clientLobGroupData.firstDayOfWeek
    );
    this.clientLOBGroupForm.controls.timeZoneForReporting.setValue(
      this.clientLobGroupData.timeZoneForReporting
    );
  }

  private getDropdownValues() {
    this.clientNames = this.clientLobGroupDropdownService.getClientNameList();
    this.clientLOBGroups = this.clientLobGroupDropdownService.getClientLOBNameList();
    this.timeZoneList = this.clientLobGroupListService.getTimeZoneList();
  }

  private agentFormIntialization() {
    this.clientLOBGroupForm = this.formBuilder.group({
      clientName: new FormControl('', Validators.compose([Validators.required, CustomValidators.cannotContainSpace])),
      clientLOBGroup: new FormControl('', Validators.required),
      firstDayOfWeek: new FormControl('', Validators.required),
      timeZoneForReporting: new FormControl('', Validators.required),
    });
  }
}
