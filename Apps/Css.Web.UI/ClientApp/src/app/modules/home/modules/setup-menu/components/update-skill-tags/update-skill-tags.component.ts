import { Component, OnInit, Input } from '@angular/core';
import { NgbModalOptions, NgbModal, NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { Validators, FormBuilder, FormGroup, FormArray } from '@angular/forms';
import { MessagePopUpComponent } from 'src/app/shared/popups/message-pop-up/message-pop-up.component';
import { SkillTagsService } from '../../services/skill-tags.service';
import { SkillTag, SkillTagOpenHours } from '../../models/skill-tag.model';
import { ClientLobGroupDropdownService } from '../../services/client-lob-group-dropdown.service';
import { Translation } from 'src/app/shared/models/translation.model';
import { WeekDay } from '@angular/common';
import { CustomValidators } from 'src/app/shared/util/validations.util';

@Component({
  selector: 'app-update-skill-tags',
  templateUrl: './update-skill-tags.component.html',
  styleUrls: ['./update-skill-tags.component.scss']
})
export class UpdateSkillTagsComponent implements OnInit {

  isEdit: boolean;
  formSubmitted: boolean;
  hasMismatch: boolean;
  skillTagForm: FormGroup;
  skillTagData: SkillTag;
  weekDays: Array<WeekDay>;
  clientNames: Array<any>;
  clientLOBNames: Array<any>;
  skillGroups: Array<any>;
  openTypes: Array<any>;
  openTime: Array<any>;

  @Input() title: string;
  @Input() skillTag: SkillTag;
  @Input() translationValues: Translation[];

  constructor(
    private formBuilder: FormBuilder,
    public activeModal: NgbActiveModal,
    private modalService: NgbModal,
    private skillTagService: SkillTagsService,
    private clientLobGroupDropDownService: ClientLobGroupDropdownService
  ) { }

  get form() {
    return this.skillTagForm.controls;
  }

  get openHours() {
    return this.skillTagForm.get('openHours') as FormArray;
  }

  ngOnInit(): void {
    this.weekDays = Object.keys(WeekDay).filter(key => isNaN(WeekDay[key])).map(x => +x);
    this.openTypes = this.skillTagService.openOptions();
    this.openTime = this.skillTagService.openTimes();
    this.clientLOBNames = this.clientLobGroupDropDownService.getClientLOBNameList();
    this.clientNames = this.clientLobGroupDropDownService.getClientNameList();
    this.skillGroups = this.skillTagService.getSkillGroupList();
    this.skillTagForm = this.formBuilder.group({
      clientName: ['', Validators.required],
      clientLobGroup: ['', Validators.required],
      skillGroup: ['', Validators.required],
      skillTagName: ['', Validators.compose([Validators.required, CustomValidators.cannotContainSpace])],
      openHours: this.createOperatingHoursArray()
    });
    if (this.title === 'Edit') {
      this.isEdit = true;
      this.populateFormDetails();
    }
  }

  hasFormControlValidationError(control: string) {
    return (
      this.formSubmitted &&
      this.skillTagForm.controls[control].errors?.required
    );
  }


  hasOpenHoursValidationError(controlName: string, index: number) {
    return (
      this.formSubmitted &&
      this.openHours.controls[index].get(controlName).errors?.required
    );
  }

  getWeekDay(weekDay: number) {
    return WeekDay[weekDay];
  }

  onOptionTypeChange(index: number) {
    if (this.openHours.controls[index]
      .get('open').value === this.openTypes[1].open) {
      this.openHours.controls[index].get('from')
        .setValidators([Validators.required]);
      this.openHours.controls[index].get('to')
        .setValidators([Validators.required]);
    } else {
      // clear from to values on open type change
      this.openHours.controls[index].patchValue({
        from: '',
        to: ''
      });
      this.openHours.controls[index].get('from')
        .clearValidators();
      this.openHours.controls[index].get('to')
        .clearValidators();
    }
    this.openHours.controls[index].get('from')
      .updateValueAndValidity();
    this.openHours.controls[index].get('to')
      .updateValueAndValidity();
  }

  save() {
    this.formSubmitted = true;
    if (this.skillTagForm.valid) {
      this.saveTagDetails();
      if (this.isEdit) {
        this.updateTagDetails();
      } else {
        this.addTagDetails();
      }
    }
  }

  private addTagDetails() {
    this.skillTagService.addSkillTagDetails(this.skillTagData);
    this.activeModal.close();
    this.showSuccessPopUpMessage('The record has been added!');
  }

  private updateTagDetails() {
    this.matchSkillTagDataChanges();

    if (this.hasMismatch) {
      this.skillTagData.id = this.skillTag.id;
      this.skillTagData.createdDate = this.skillTag.createdDate;
      this.skillTagData.createdBy = this.skillTag.createdBy;
      this.skillTagService.updateSkillTagDetials(this.skillTagData);
      this.activeModal.close(this.skillTagData);
      this.showSuccessPopUpMessage('The record has been updated!');
    } else {
      this.activeModal.close();
      this.showSuccessPopUpMessage('No changes has been made!');
    }
  }

  private createOperatingHoursArray() {
    const operatingHours = new FormArray([]);
    this.weekDays.forEach((element, index) => {
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
          open: this.skillTag.details[index].open,
          from: this.skillTag.details[index].from,
          to: this.skillTag.details[index].to,
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

  private matchSkillTagDataChanges() {
    for (const propertyName in this.skillTagForm.value) {
      if (propertyName !== 'openHours') {
        if (
          this.skillTagForm.value[propertyName] !== this.skillTag[propertyName]
        ) {
          this.hasMismatch = true;
          break;
        }
      } else {
        for (const index in this.skillTagForm.controls.openHours.value) {
          if (
            this.skillTagData.details[index].day !== this.skillTag.details[index].day ||
            this.skillTagData.details[index].open !== this.skillTag.details[index].open ||
            this.skillTagData.details[index].from !== this.skillTag.details[index].from ||
            this.skillTagData.details[index].to !== this.skillTag.details[index].to
          ) {
            this.hasMismatch = true;
            break;
          }
        }
      }
    }
  }

  private saveTagDetails() {
    this.skillTagData = { ...this.skillTag };
    this.skillTagData.clientName = this.skillTagForm.value.clientName;
    this.skillTagData.clientLobGroup = this.skillTagForm.value.clientLobGroup;
    this.skillTagData.skillGroup = this.skillTagForm.value.skillGroup;
    this.skillTagData.skillTagName = this.skillTagForm.value.skillTagName;
    this.skillTagData.details = this.formatOpenHoursDeatils(this.skillTagForm.value.openHours);
  }

  private formatOpenHoursDeatils(formValue) {
    const skillTagDetails: SkillTagOpenHours[] = [];
    for (let i = 0; i < 7; i++) {
      const skillTagOpenhours = new SkillTagOpenHours();
      skillTagOpenhours.day = formValue[i].day;
      skillTagOpenhours.open = formValue[i].open;
      skillTagOpenhours.from = formValue[i].from;
      skillTagOpenhours.to = formValue[i].to;
      skillTagDetails.push(skillTagOpenhours);
    }
    return skillTagDetails;
  }

  private populateFormDetails() {
    this.skillTagForm.patchValue({
      clientName: this.skillTag.clientName,
      clientLobGroup: this.skillTag.clientLobGroup,
      skillGroup: this.skillTag.skillGroup,
      skillTagName: this.skillTag.skillTagName
    });
  }

  private rangeValidator(openHours: FormGroup) {
    const start = openHours.get('from') ? openHours.get('from').value : '';
    const end = openHours.get('to') ? openHours.get('to').value : '';
    let startTime;
    let endTime;
    if (start && end) {
      startTime = new Date().setHours(this.skillTagService.getHours(start),
        this.skillTagService.getMinutes(start), 0);
      endTime = new Date().setHours(this.skillTagService.getHours(end),
        this.skillTagService.getMinutes(end), 0);
      return (start && end && startTime < endTime)
        ? null
        : { rangeError: true };
    } else {
      return null;
    }

  }
}
