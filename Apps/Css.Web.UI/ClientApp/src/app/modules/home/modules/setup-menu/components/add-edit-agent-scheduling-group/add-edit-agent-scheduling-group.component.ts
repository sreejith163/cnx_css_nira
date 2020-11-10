import { WeekDay } from '@angular/common';
import { Component, Input, OnInit } from '@angular/core';
import {
  FormArray,
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
import { TimeZone } from 'src/app/shared/models/time-zone.model';
import { Translation } from 'src/app/shared/models/translation.model';
import { MessagePopUpComponent } from 'src/app/shared/popups/message-pop-up/message-pop-up.component';
import { CustomValidators } from 'src/app/shared/util/validations.util';
import { ClientLOBName } from '../../../scheduling-menu/models/client-lob-name.model';
import { ClientBaseModel } from '../../models/client-base.model';
import {
  SchedulingGroupDetails,
  SchedulingGroupOpenHours,
} from '../../models/scheduling-group-details.model';
import { SkillGroupList } from '../../models/skill-group-list.model';
import { SkillTagList } from '../../models/skill-tag-list.model';
import { AgentSchedulingGroupService } from '../../services/agent-scheduling-group.service';
import { DropdownListService } from '../../services/dropdown-list.service';

@Component({
  selector: 'app-add-edit-agent-scheduling-group',
  templateUrl: './add-edit-agent-scheduling-group.component.html',
  styleUrls: ['./add-edit-agent-scheduling-group.component.scss']
})
export class AddEditAgentSchedulingGroupComponent implements OnInit {

  hasMismatch: boolean;
  formSubmitted: boolean;
  disableTime: boolean;
  isEdit: boolean;
  weekDays: Array<WeekDay>;
  weekDay = WeekDay;
  openTypes: Array<any>;
  openTime: Array<any>;
  schedulingGroupModel: SchedulingGroupDetails;
  schedulingGroupForm: FormGroup;
  clientNamesList: ClientBaseModel[] = [];
  clientLOBNames: ClientLOBName[] = [];
  skillGroupList: SkillGroupList[] = [];
  skillTagList: SkillTagList[] = [];
  timeZoneList: TimeZone[] = [];

  @Input() title: string;
  @Input() schedulingGroupData: SchedulingGroupDetails;
  @Input() translationValues: Translation[];

  constructor(
    private schedulingGroupListService: AgentSchedulingGroupService,
    private formBuilder: FormBuilder,
    public activeModal: NgbActiveModal,
    private modalService: NgbModal,
    private dropdownService: DropdownListService
  ) {}

  ngOnInit(): void {
    this.weekDays = Object.keys(WeekDay).filter(key => isNaN(WeekDay[key])).map(x => +x);
    this.openTypes = this.schedulingGroupListService.openOptions();
    this.openTime = this.schedulingGroupListService.openTimes();
    this.checkAddOrEditSchedulingGroupDetails();
    this.getDropdownValues();
  }

  get form() {
    return this.schedulingGroupForm.controls;
  }

  get operatingHours() {
    return this.schedulingGroupForm.get('operatingHours') as FormArray;
  }

  getWeekDay(weekDay: number) {
    return WeekDay[weekDay];
  }

  getTranslationIdForWeek(weekDay: number) {
    return 'radio_add_edit_first_day_of_week_' + this.getWeekDay(weekDay)?.toLowerCase();
  }

  hasFormControlValidationError(control: string) {
    return (
      this.formSubmitted &&
      this.schedulingGroupForm.controls[control].errors?.required
    );
  }

  hasOpenHoursValidationError(controlName: string, index: number) {
    return (
      this.formSubmitted &&
      this.operatingHours.controls[index].get(controlName).errors?.required
    );
  }

  changeOperatingHoursDays(startDay: number) {
    this.sortOperatingHoursArray(startDay);
  }

  onOptionTypeChange(index: number) {
    if (
      this.operatingHours.controls[index].get('open').value ===
      this.openTypes[1].open
    ) {
      this.operatingHours.controls[index]
        .get('from')
        .setValidators([Validators.required]);
      this.operatingHours.controls[index]
        .get('to')
        .setValidators([Validators.required]);
    } else {
      // clear from to values on open type change
      this.operatingHours.controls[index].patchValue({
        from: '',
        to: '',
      });
      this.operatingHours.controls[index].get('from').clearValidators();
      this.operatingHours.controls[index].get('to').clearValidators();
    }
    this.operatingHours.controls[index].get('from').updateValueAndValidity();
    this.operatingHours.controls[index].get('to').updateValueAndValidity();
  }

  save() {
    this.formSubmitted = true;
    if (this.schedulingGroupForm.valid) {
      this.saveSchedulingGroupDetailsOnModel();
      if (this.isEdit) {
        this.updateSchedulingGroupDetails();
      } else {
        this.addSchedulingGroupDetails();
      }
    }
  }

  private addSchedulingGroupDetails() {
    this.schedulingGroupListService.addSchedulingGroup(this.schedulingGroupModel);
    this.activeModal.close();
    this.showSuccessPopUpMessage('The record has been added!');
  }

  private updateSchedulingGroupDetails() {
    this.matchSchedulingGroupDataChanges();

    if (this.hasMismatch) {
      this.schedulingGroupModel.id = this.schedulingGroupData.id;
      this.schedulingGroupModel.refId = this.schedulingGroupData.refId;
      this.schedulingGroupModel.createdDate = this.schedulingGroupData.createdDate;
      this.schedulingGroupModel.createdBy = this.schedulingGroupData.createdBy;
      this.schedulingGroupListService.updateSchedulingGroup(this.schedulingGroupModel);
      this.activeModal.close(this.schedulingGroupModel);
      this.showSuccessPopUpMessage('The record has been updated!');
    } else {
      this.activeModal.close();
      this.showSuccessPopUpMessage('No changes has been made!');
    }
  }

  private rangeValidator(operatingHours: FormGroup) {
    const start = operatingHours.get('from') ? operatingHours.get('from').value : '';
    const end = operatingHours.get('to') ? operatingHours.get('to').value : '';
    let startTime;
    let endTime;
    if (start && end) {
      startTime = new Date().setHours(
        this.getHours(start),
        this.getMinutes(start),
        0
      );
      endTime = new Date().setHours(
        this.getHours(end),
        this.getMinutes(end),
        0
      );
      return start && end && startTime < endTime ? null : { rangeError: true };
    } else {
      return null;
    }
  }

  private getHours(time: string) {
    const timeArray = time.split(':');
    if (timeArray[1].split(' ')[1] === 'pm') {
      return parseInt(timeArray[0], 10) + 12;
    } else {
      return parseInt(timeArray[0], 10);
    }
  }

  private getMinutes(time: string) {
    return +(time.split(':')[1].split(' ')[0]);
  }

  private sortOperatingHoursArray(startDay: number) {
    if (this.schedulingGroupForm) {
      const sortedDays = this.getSortedWeekDays(startDay);
      const currentFormArrayValues = this.schedulingGroupForm.controls.operatingHours.value;
      const sortedFormArray = this.getSortedOperatingHoursArray(sortedDays, currentFormArrayValues);
      this.schedulingGroupForm.controls.operatingHours.patchValue(sortedFormArray.value);
    }
  }

  private getSortedOperatingHoursArray(sortedDays: any[], currentFormArrayValues: any[]) {
    const operatingHours = new FormArray([]);
    sortedDays.forEach((element) => {
      const operatingHoursGroup = this.formBuilder.group(
        {
          day: [element, Validators.required],
          open: [currentFormArrayValues.find(x => x.day === element)?.open, Validators.required],
          from: [currentFormArrayValues.find(x => x.day === element)?.from],
          to: [currentFormArrayValues.find(x => x.day === element)?.to],
        },
        { validators: this.rangeValidator.bind(this) }
      );
      operatingHours.push(operatingHoursGroup);
    });

    return operatingHours;
  }

  private getSortedWeekDays(day: number) {
    const days = [];
    const previousDays = [];
    let found = false;

    this.weekDays.forEach((e) => {
      if (e === day && !found) {
        found = true;
        days.push(e);
      } else if (!found) {
        previousDays.push(e);
      } else {
        days.push(e);
      }
    });

    return (days.concat(previousDays));
  }

  private matchSchedulingGroupDataChanges() {
    for (const propertyName in this.schedulingGroupForm.value) {
      if (propertyName !== 'operatingHours') {
        if (
          this.schedulingGroupData[propertyName] !== this.schedulingGroupModel[propertyName]
        ) {
          this.hasMismatch = true;
          break;
        }
      } else {
        for (const index in this.schedulingGroupForm.controls.operatingHours.value) {
          if (
            this.schedulingGroupModel.details[index].day !== this.schedulingGroupData.details[index].day ||
            this.schedulingGroupModel.details[index].open !== this.schedulingGroupData.details[index].open ||
            this.schedulingGroupModel.details[index].from !== this.schedulingGroupData.details[index].from ||
            this.schedulingGroupModel.details[index].to !== this.schedulingGroupData.details[index].to
          ) {
            this.hasMismatch = true;
            break;
          }
        }
      }
    }
  }

  private saveSchedulingGroupDetailsOnModel() {
    this.schedulingGroupModel = new SchedulingGroupDetails();
    this.schedulingGroupModel.clientName = this.schedulingGroupForm.controls.clientName.value;
    this.schedulingGroupModel.clientLOBGroupName = this.schedulingGroupForm.controls.clientLOBGroupName.value;
    this.schedulingGroupModel.skillGroup = this.schedulingGroupForm.controls.skillGroup.value;
    this.schedulingGroupModel.skillTag = this.schedulingGroupForm.controls.skillTag.value;
    this.schedulingGroupModel.schedulingGroupName = this.schedulingGroupForm.controls.schedulingGroupName.value;
    this.schedulingGroupModel.firstDayOfWeek = this.schedulingGroupForm.controls.firstDayOfWeek.value;
    this.schedulingGroupModel.timeZoneForReporting = this.schedulingGroupForm.controls.timeZoneForReporting.value;
    this.schedulingGroupModel.details = this.formatOperatingHoursDetails(
      this.schedulingGroupForm.value.operatingHours
    );
  }

  private formatOperatingHoursDetails(formValue) {
    const operatingHourDetails: SchedulingGroupOpenHours[] = [];
    for (let i = 0; i < 7; i++) {
      const schedulingGroupOperatingHours = new SchedulingGroupOpenHours();
      schedulingGroupOperatingHours.day = formValue[i].day;
      schedulingGroupOperatingHours.open = formValue[i].open;
      schedulingGroupOperatingHours.from = formValue[i].from;
      schedulingGroupOperatingHours.to = formValue[i].to;
      operatingHourDetails.push(schedulingGroupOperatingHours);
    }
    return operatingHourDetails;
  }

  private createOperatingHoursArray() {
    const operatingHours = new FormArray([]);
    const sortedDays = this.getSortedWeekDays(this.schedulingGroupData?.firstDayOfWeek ?? this.weekDays[0]);
    sortedDays.forEach((element, index) => {
      const operatingHoursGroup = this.formBuilder.group(
        {
          day: [element, Validators.required],
          open: ['', Validators.required],
          from: [''],
          to: [''],
        },
        { validators: this.rangeValidator.bind(this) }
      );
      if (this.title === 'Edit') {
        operatingHoursGroup.patchValue({
          open: this.schedulingGroupData.details[index].open,
          from: this.schedulingGroupData.details[index].from,
          to: this.schedulingGroupData.details[index].to,
        });
      }
      operatingHours.push(operatingHoursGroup);
    });

    return operatingHours;
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

  private checkAddOrEditSchedulingGroupDetails() {
    if (this.title === 'Edit') {
      this.isEdit = true;
      this.schedulingGroupFormIntialization();
      this.populateSchedulingGroupDetailsOnForm();
    }
    if (this.title === 'Add') {
      this.schedulingGroupFormIntialization();
      this.schedulingGroupForm.controls.firstDayOfWeek.setValue(0);
    }
  }

  private populateSchedulingGroupDetailsOnForm() {
    this.schedulingGroupForm.patchValue({
      clientName: this.schedulingGroupData.clientName,
      clientLOBGroupName: this.schedulingGroupData.clientLOBGroupName,
      skillGroup: this.schedulingGroupData.skillGroup,
      skillTag: this.schedulingGroupData.skillTag,
      schedulingGroupName: this.schedulingGroupData.schedulingGroupName,
      firstDayOfWeek: this.schedulingGroupData.firstDayOfWeek,
      timeZoneForReporting: this.schedulingGroupData.timeZoneForReporting,
    });
  }

  private getDropdownValues() {
    this.clientNamesList = this.dropdownService.getClientNameList();
    this.clientLOBNames = this.dropdownService.getClientLOBNameList();
    this.skillGroupList = this.dropdownService.getSkillGroup();
    this.skillTagList = this.dropdownService.getSkillTag();
    this.timeZoneList = this.schedulingGroupListService.getTimeZoneList();
  }

  private schedulingGroupFormIntialization() {
    this.schedulingGroupForm = this.formBuilder.group({
      clientName: new FormControl('', Validators.required),
      clientLOBGroupName: new FormControl('', Validators.required),
      skillGroup: new FormControl('', Validators.required),
      skillTag: new FormControl('', Validators.required),
      schedulingGroupName: new FormControl('', Validators.compose([Validators.required, CustomValidators.cannotContainSpace])),
      firstDayOfWeek: new FormControl('', Validators.required),
      timeZoneForReporting: new FormControl('', Validators.required),
      operatingHours: this.createOperatingHoursArray(),
    });
  }

}
