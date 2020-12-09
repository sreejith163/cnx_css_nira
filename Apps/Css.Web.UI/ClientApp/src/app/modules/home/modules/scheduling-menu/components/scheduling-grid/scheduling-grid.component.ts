import { CdkDragDrop } from '@angular/cdk/drag-drop';
import { WeekDay } from '@angular/common';
import { Component, OnInit } from '@angular/core';
import { NgbCalendar, NgbDate, NgbDateParserFormatter, NgbDateStruct, NgbModal, NgbModalOptions } from '@ng-bootstrap/ng-bootstrap';
import { PaginationSize } from 'src/app/shared/models/pagination-size.model';
import { MessagePopUpComponent } from 'src/app/shared/popups/message-pop-up/message-pop-up.component';
import { Constants } from 'src/app/shared/util/constants.util';
import { SchedulingStatus } from '../../enums/scheduling-status.enum';
import { SchedulingGrid } from '../../models/scheduling-grid.model';
import { SchedulingGridService } from '../../services/scheduling-grid.service';
import { SchedulingCalendarDays } from '../../models/scheduling-calendar-days.model';
import { SchedulingCalendarTime } from '../../models/scheduling-calendar-times.model';
import { trigger, state, style, transition, animate } from '@angular/animations';
import { SchedulingInterval } from '../../models/scheduling-interval.model';
import { ICON_DB } from 'src/app/shared/util/icon.data';

import * as $ from 'jquery';
import { CssMenu } from 'src/app/shared/enums/css-menu.enum';
import { LanguageTranslationService } from 'src/app/shared/services/language-translation.service';
import { TranslationDetails } from 'src/app/shared/models/translation-details.model';
import { SubscriptionLike as ISubscription } from 'rxjs';
import { GenericStateManagerService } from 'src/app/shared/services/generic-state-manager.service';

declare function setRowCellIndex(cell: string);
declare function highlightSelectedCells(table: string, cell: string);
declare function removeHighlightedCells(table: string, className: string);
declare function highlightCell(cell: string, className: string);

@Component({
  selector: 'app-scheduling-grid',
  animations: [
    trigger(
      'enterAnimation', [
      state('true', style({ opacity: 1, height: 'auto' })),
      state('void', style({ opacity: 0, height: 0 })),
      transition(':enter', animate('400ms ease-in-out')),
      transition(':leave', animate('400ms ease-in-out'))
    ]
    )
  ],
  templateUrl: './scheduling-grid.component.html',
  styleUrls: ['./scheduling-grid.component.scss']
})
export class SchedulingGridComponent implements OnInit {

  timeIntervals = 5;
  currentPage = 1;
  pageSize = 10;
  characterSplice = 25;
  iconCount = 30;
  startIcon = 0;
  endIcon: number;
  totalSchedulingRecord: number;

  selectedIconId: string;
  tableClassName = 'schedulingGridTable';
  selectedCellClassName = 'cell-selected';
  hasMismatch: boolean;

  iconData = ICON_DB;
  hoveredDate: NgbDate | null = null;
  fromDate: NgbDate;
  toDate: NgbDate | null = null;
  today = this.calendar.getToday();

  model: NgbDateStruct;
  weekDay = WeekDay;
  scheduleStatus = SchedulingStatus;
  selectedGrid: SchedulingGrid;
  schedulingGridData: SchedulingGrid;

  openTimes: Array<any>;
  translationValues: TranslationDetails[] = [];
  schedulingIntervals: SchedulingInterval[] = [];
  paginationSize: PaginationSize[] = [];
  totalSchedulingGridData: SchedulingGrid[] = [];
  schedulingStatus: any[] = [];
  weekDays: Array<string> = [];

  languageSelectionSubscription: ISubscription;
  getTranslationValuesSubscription: ISubscription;
  subscriptions: ISubscription[] = [];

  constructor(
    private schedulingGridService: SchedulingGridService,
    private calendar: NgbCalendar,
    private modalService: NgbModal,
    public ngbDateParserFormatter: NgbDateParserFormatter,
    private translationService: LanguageTranslationService,
    private genericStateManagerService: GenericStateManagerService
  ) { }

  ngOnInit(): void {
    this.loadTranslationValues();
    this.subscribeToUserLanguage();
    this.endIcon = this.iconCount;
    this.openTimes = this.getOpenTimes();
    this.weekDays = Object.keys(WeekDay).filter(key => isNaN(WeekDay[key]));
    this.schedulingIntervals = Constants.schedulingIntervals;
    this.schedulingStatus = Object.keys(SchedulingStatus).filter(key => isNaN(SchedulingStatus[key]));
    this.totalSchedulingGridData = this.schedulingGridService.getSchedulingGridData().sort(
      (a, b) =>
        new Date(b.employeeId).getTime() - new Date(a.employeeId).getTime()
    );
    this.totalSchedulingRecord = this.totalSchedulingGridData.length;
    this.paginationSize = Constants.paginationSize;
  }

  getGridMaxWidth() {
    return window.innerWidth > 1350 ? (window.innerWidth - 350) + 'px' : '1250px';
  }

  onDateSelection(date: NgbDate, index: number) {
    if (!this.fromDate && !this.toDate) {
      this.fromDate = date;
      const newDate = this.ngbDateParserFormatter.format(this.fromDate);
      this.totalSchedulingGridData[index].fromDate = newDate;
      this.selectedGrid.fromDate = newDate;
    } else if (this.fromDate && !this.toDate && date && date.after(this.fromDate)) {
      this.toDate = date;
      const newDate = this.ngbDateParserFormatter.format(this.toDate);
      this.totalSchedulingGridData[index].toDate = newDate;
      this.selectedGrid.toDate = newDate;
    } else {
      this.toDate = null;
      this.fromDate = date;
      const newDate = this.ngbDateParserFormatter.format(this.fromDate);
      this.totalSchedulingGridData[index].fromDate = newDate;
      this.selectedGrid.fromDate = newDate;
    }
  }

  isHovered(date: NgbDate) {
    return this.fromDate && !this.toDate && this.hoveredDate && date.after(this.fromDate) && date.before(this.hoveredDate);
  }

  isInside(date: NgbDate) {
    return this.toDate && date.after(this.fromDate) && date.before(this.toDate);
  }

  isRange(date: NgbDate) {
    return date.equals(this.fromDate) || (this.toDate && date.equals(this.toDate)) || this.isInside(date) || this.isHovered(date);
  }

  unifiedToNative(unified: string) {
    const codePoints = unified.split('-').map(u => parseInt(`0x${u}`, 16));
    return String.fromCodePoint(...codePoints);
  }

  validateInput(currentValue: NgbDate | null, input: string): NgbDate | null {
    const parsed = this.ngbDateParserFormatter.parse(input);
    return parsed && this.calendar.isValid(NgbDate.from(parsed)) ? NgbDate.from(parsed) : currentValue;
  }

  toggleDetails(el: SchedulingGrid) {
    this.schedulingGridData = JSON.parse(JSON.stringify(el));

    if (this.selectedGrid && this.selectedGrid.employeeId === el.employeeId) {
      this.selectedGrid = null;
    } else {
      this.selectedGrid = el;
    }
  }

  getIconFromSelectedGrid(week: number, openTime: any) {
    const meridiem = openTime.split(':')[1].split(' ')[1];
    const time = openTime.split(' ')[0];
    const weekData = this.selectedGrid.calendar?.weekDays.find(x => x.day === +week);

    if (weekData) {
      const weekTimeData = weekData.times.find(x => x.from === time && x.meridiem === meridiem);
      if (weekTimeData) {
        return this.unifiedToNative(weekTimeData?.icon);
      }
    }

    return '';
  }

  previous() {
    if (this.startIcon > 0) {
      this.startIcon = this.startIcon - 1;
      this.endIcon = this.endIcon - 1;
    }
  }

  next() {
    if (this.endIcon < this.iconData.length) {
      this.startIcon = this.startIcon + 1;
      this.endIcon = this.endIcon + 1;
    }
  }

  toBeginning() {
    this.startIcon = 0;
    this.endIcon = this.iconCount;
  }

  toEnd() {
    this.startIcon = this.iconData.length - this.iconCount;
    this.endIcon = this.iconData.length;
  }

  onIconClick(event) {
    this.selectedIconId = event.target.id;
    this.saveGridItems(this.selectedIconId);
  }

  isMainMinute(data: any) {
    return data.split(':')[1] === '00 am' || data.split(':')[1] === '00 pm';
  }

  setCellSelected(event) {
    if (event.shiftKey) {
      highlightSelectedCells(this.tableClassName, event.currentTarget.id);
    } else if (!event.ctrlKey) {
      removeHighlightedCells(this.tableClassName, this.selectedCellClassName);
    }

    setRowCellIndex(event.currentTarget.id);
    highlightCell(event.currentTarget.id, this.selectedCellClassName);

  }

  dragged(event: CdkDragDrop<any>) {
    this.saveGridItems(event.item.element.nativeElement.id);
  }

  changeTimeInterval(event) {
    this.timeIntervals = Number(event.target.value);
    this.openTimes = this.getOpenTimes();
  }

  changePageSize(pageSize: number) {
    this.pageSize = pageSize;
  }

  changePage(page: number) {
    this.currentPage = page;
  }

  setSchedulingStatus(status: string) {
    this.selectedGrid.status = Number(status);
  }

  save() {
    this.matchSchedulingGridDataChanges();
    this.schedulingGridData = JSON.parse(JSON.stringify(this.selectedGrid));
    this.showPopUpMessage();
  }

  private showPopUpMessage() {
    const options: NgbModalOptions = { backdrop: 'static', centered: true, size: 'sm' };
    const modalRef = this.modalService.open(MessagePopUpComponent, options);
    modalRef.componentInstance.headingMessage = 'Success';
    modalRef.componentInstance.contentMessage = (this.hasMismatch) ? 'The record has been updated!' : 'No changes has been made!';

    this.hasMismatch = false;
  }

  private matchSchedulingGridDataChanges() {
    if (JSON.stringify(this.schedulingGridData) !== JSON.stringify(this.selectedGrid)) {
      this.hasMismatch = true;
    }
  }

  private getOpenTimes() {
    const x = this.timeIntervals;
    const times = [];
    let tt = 0;
    const ap = ['am', 'pm'];

    for (let i = 0; tt < 24 * 60; i++) {
      const hh = Math.floor(tt / 60);
      const mm = tt % 60;
      times[i] =
        ('0' + (hh % 12)).slice(-2) +
        ':' +
        ('0' + mm).slice(-2) +
        ' ' +
        ap[Math.floor(hh / 12)];
      tt = tt + x;
    }
    times.forEach((ele, index) => {
      if (ele.split(' ')[0] === '00:00') {
        const meridian = ele.split(' ')[1];
        times.splice(index, 1, '12:00 ' + meridian);
      }
    });
    return times;
  }

  private saveGridItems(icon) {
    const table = $('#' + this.tableClassName);
    table.find('.' + this.selectedCellClassName).each((index, elem) => {
      const week = elem.attributes.week.value;
      const from = elem.attributes.time.value;
      const minuteValue = Number(from.split(':')[1]) + this.timeIntervals;
      const minute = minuteValue === 5 ? '05' : minuteValue;
      const to = from.split(':')[0] + ':' + minute;
      const meridiem = elem.attributes.meridiem.value;

      const weekDays = this.selectedGrid?.calendar?.weekDays;
      const weekData = weekDays.find(x => x.day === +week);

      if (weekData) {
        const timeData = weekData.times.find(x => x.from === from && x.meridiem === meridiem);
        if (timeData) {
          timeData.icon = icon;
        } else {
          const calendarTime = new SchedulingCalendarTime(meridiem, from, to, icon);
          weekData.times.push(calendarTime);
        }
      } else {
        const weekDay = new SchedulingCalendarDays();
        weekDay.day = +week;
        const calendarTime = new SchedulingCalendarTime(meridiem, from, to, icon);
        weekDay.times.push(calendarTime);

        weekDays.push(weekDay);
      }
    });
    table.find('.' + this.selectedCellClassName).removeClass(this.selectedCellClassName);
  }

  private loadTranslationValues() {
    const languageId = this.genericStateManagerService.getCurrentLanguage()?.id;
    const menuId = CssMenu.SchedulingGrid;

    this.getTranslationValuesSubscription = this.translationService.getMenuTranslations(languageId, menuId)
      .subscribe((response) => {
        if (response) {
          this.translationValues = response;
        }
      }, (error) => {
        console.log(error);
      });

    this.subscriptions.push(this.getTranslationValuesSubscription);
  }

  private subscribeToUserLanguage() {
    this.languageSelectionSubscription = this.genericStateManagerService.userLanguageChanged.subscribe(
      (languageId: number) => {
        if (languageId) {
          this.loadTranslationValues();
        }
      }
    );

    this.subscriptions.push(this.languageSelectionSubscription);
  }
}
