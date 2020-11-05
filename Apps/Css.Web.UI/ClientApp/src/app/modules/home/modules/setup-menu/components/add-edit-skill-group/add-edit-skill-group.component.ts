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
import { Translation } from 'src/app/shared/models/translation.model';
import { MessagePopUpComponent } from 'src/app/shared/popups/message-pop-up/message-pop-up.component';
import { CustomValidators } from 'src/app/shared/util/validations.util';
import { ClientBaseModel } from '../../models/client-base.model';
import { SkillGroupDetails, SkillGroupOpenHours } from '../../models/skill-group.model';
import { TimeZone } from '../../models/time-zone.model';
import { DropdownListService } from '../../services/dropdown-list.service';
import { SkillGroupsService } from '../../services/skill-groups.service';

@Component({
  selector: 'app-add-edit-skill-group',
  templateUrl: './add-edit-skill-group.component.html',
  styleUrls: ['./add-edit-skill-group.component.scss']
})
export class AddEditSkillGroupComponent implements OnInit {

  formSubmitted: boolean;
  disableTime: boolean;
  hasMismatch: boolean;
  isEdit: boolean;
  weekDays: Array<WeekDay>;
  openTypes: Array<any>;
  openTime: Array<any>;
  skillGroupModel: SkillGroupDetails;
  skillGroupForm: FormGroup;
  clientNamesList: ClientBaseModel[] = [];
  timeZoneList: TimeZone[] = [];

  @Input() title: string;
  @Input() skillGroupData: SkillGroupDetails;
  @Input() translationValues: Translation;

  constructor(
    private skillGroupListService: SkillGroupsService,
    private formBuilder: FormBuilder,
    public activeModal: NgbActiveModal,
    private modalService: NgbModal,
    private dropdownService: DropdownListService
  ) { }

  ngOnInit(): void {
    this.weekDays = Object.keys(WeekDay).filter(key => isNaN(WeekDay[key])).map(x => +x);
    this.openTypes = this.skillGroupListService.openOptions();
    this.openTime = this.skillGroupListService.openTimes();
    this.getDropdownValues();
    this.checkAddOrEditSkillGroupDetails();
  }

  get form() {
    return this.skillGroupForm.controls;
  }

  get operatingHours() {
    return this.skillGroupForm.get('operatingHours') as FormArray;
  }

  getWeekDay(weekDay: number) {
    return WeekDay[weekDay];
  }

  getTranslationIdForWeek(weekDay: number) {
    return 'radio_add_edit_first_day_of_week_' + this.getWeekDay(weekDay)?.toLowerCase();
  }

  changeOperatingHoursDays(startDay: WeekDay) {
    this.sortOperatingHoursArray(startDay);
  }

  hasFormControlValidationError(control: string) {
    return (
      this.formSubmitted &&
      this.skillGroupForm.controls[control].errors?.required
    );
  }

  hasOpenHoursValidationError(controlName: string, index: number) {
    return (
      this.formSubmitted &&
      this.operatingHours.controls[index].get(controlName).errors?.required
    );
  }

  onOptionTypeChange(index: number) {
    if (this.operatingHours.controls[index]
      .get('open').value === this.openTypes[1].open) {
      this.operatingHours.controls[index].get('from')
        .setValidators([Validators.required]);
      this.operatingHours.controls[index].get('to')
        .setValidators([Validators.required]);
    } else {
      // clear from to values on open type change
      this.operatingHours.controls[index].patchValue({
        from: '',
        to: ''
      });
      this.operatingHours.controls[index].get('from')
        .clearValidators();
      this.operatingHours.controls[index].get('to')
        .clearValidators();
    }
    this.operatingHours.controls[index].get('from')
      .updateValueAndValidity();
    this.operatingHours.controls[index].get('to')
      .updateValueAndValidity();
  }

  save() {
    this.formSubmitted = true;
    if (this.skillGroupForm.valid) {
      this.saveSkillGroupDetailsOnModel();
      if (this.isEdit) {
        this.updateSkillGroupDetails();
      } else {
        this.addSkillGroupDetails();
      }
    }
  }

  private addSkillGroupDetails() {
    this.skillGroupListService.addSkillGroup(this.skillGroupModel);
    this.activeModal.close();
    this.showSuccessPopUpMessage('The record has been added!');
  }

  private updateSkillGroupDetails() {
    this.matchSkillGroupDataChanges();

    if (this.hasMismatch) {
      this.skillGroupModel.id = this.skillGroupData.id;
      this.skillGroupModel.refId = this.skillGroupData.refId;
      this.skillGroupModel.createdDate = this.skillGroupData.createdDate;
      this.skillGroupModel.createdBy = this.skillGroupData.createdBy;
      this.skillGroupListService.updateSkillGroup(this.skillGroupModel);
      this.activeModal.close(this.skillGroupModel);
      this.showSuccessPopUpMessage('The record has been updated!');
    } else {
      this.activeModal.close();
      this.showSuccessPopUpMessage('No changes has been made!');
    }
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

  private createOperatingHoursArray() {
    const operatingHours = new FormArray([]);
    const sortedDays = this.getSortedWeekDays(this.skillGroupData?.firstDayOfWeek ?? this.weekDays[0]);
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
          open: this.skillGroupData.details[index].open,
          from: this.skillGroupData.details[index].from,
          to: this.skillGroupData.details[index].to,
        });
      }
      operatingHours.push(operatingHoursGroup);
    });

    return operatingHours;
  }

  private sortOperatingHoursArray(startDay) {
    if (this.skillGroupForm) {
      const sortedDays = this.getSortedWeekDays(startDay);
      const currentFormArrayValues = this.skillGroupForm.controls.operatingHours.value;
      const sortedFormArray = this.getSortedOperatingHoursArray(sortedDays, currentFormArrayValues);
      this.skillGroupForm.controls.operatingHours.patchValue(sortedFormArray.value);
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

  private matchSkillGroupDataChanges() {
    for (const propertyName in this.skillGroupForm.value) {
      if (propertyName !== 'operatingHours') {
        if (
          this.skillGroupModel[propertyName] !== this.skillGroupData[propertyName]
        ) {
          this.hasMismatch = true;
          break;
        }
      } else {
        for (const index in this.skillGroupForm.controls.operatingHours.value) {
          if (
            this.skillGroupModel.details[index].day !== this.skillGroupData.details[index].day ||
            this.skillGroupModel.details[index].open !== this.skillGroupData.details[index].open ||
            this.skillGroupModel.details[index].from !== this.skillGroupData.details[index].from ||
            this.skillGroupModel.details[index].to !== this.skillGroupData.details[index].to
          ) {
            this.hasMismatch = true;
            break;
          }
        }
      }
    }
  }

  private saveSkillGroupDetailsOnModel() {
    this.skillGroupModel = new SkillGroupDetails();
    this.skillGroupModel.clientName = this.skillGroupForm.controls.clientName.value;
    this.skillGroupModel.clientLOBGroupName = this.skillGroupForm.controls.clientLOBGroupName.value;
    this.skillGroupModel.skillGroupName = this.skillGroupForm.controls.skillGroupName.value;
    this.skillGroupModel.firstDayOfWeek = this.skillGroupForm.controls.firstDayOfWeek.value;
    this.skillGroupModel.timeZoneForReporting = this.skillGroupForm.controls.timeZoneForReporting.value;
    this.skillGroupModel.details = this.formatOperatingHoursDetails(
      this.skillGroupForm.value.operatingHours
    );
  }

  private formatOperatingHoursDetails(formValue) {
    const operatingHourDetails: SkillGroupOpenHours[] = [];
    for (let i = 0; i < 7; i++) {
      const skillGroupOperatingHours = new SkillGroupOpenHours();
      skillGroupOperatingHours.day = formValue[i].day;
      skillGroupOperatingHours.open = formValue[i].open;
      skillGroupOperatingHours.from = formValue[i].from;
      skillGroupOperatingHours.to = formValue[i].to;
      operatingHourDetails.push(skillGroupOperatingHours);
    }

    return operatingHourDetails;
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

  private checkAddOrEditSkillGroupDetails() {
    if (this.title === 'Edit') {
      this.isEdit = true;
      this.skillGroupFormIntialization();
      this.populateSkillGroupDetailsOnForm();
    }
    if (this.title === 'Add') {
      this.skillGroupFormIntialization();
      this.skillGroupForm.controls.firstDayOfWeek.setValue(0);
    }
  }

  private populateSkillGroupDetailsOnForm() {
    this.skillGroupForm.patchValue({
      clientName: this.skillGroupData.clientName,
      clientLOBGroupName: this.skillGroupData.clientLOBGroupName,
      skillGroupName: this.skillGroupData.skillGroupName,
      firstDayOfWeek: this.skillGroupData.firstDayOfWeek,
      timeZoneForReporting: this.skillGroupData.timeZoneForReporting,
    });
  }

  private getDropdownValues() {
    this.clientNamesList = this.dropdownService.getClientNameList();
    this.timeZoneList = this.skillGroupListService.getTimeZoneList();
  }

  private skillGroupFormIntialization() {
    this.skillGroupForm = this.formBuilder.group({
      clientName: new FormControl('', Validators.required),
      clientLOBGroupName: new FormControl('', Validators.compose([Validators.required, CustomValidators.cannotContainSpace])),
      skillGroupName: new FormControl('', Validators.compose([Validators.required, CustomValidators.cannotContainSpace])),
      firstDayOfWeek: new FormControl('', Validators.required),
      timeZoneForReporting: new FormControl('', Validators.required),
      operatingHours: this.createOperatingHoursArray(),
    });
  }

}
