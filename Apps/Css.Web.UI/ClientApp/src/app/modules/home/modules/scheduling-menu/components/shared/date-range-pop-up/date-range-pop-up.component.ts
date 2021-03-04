import { Component, Input, OnInit } from '@angular/core';
import { NgbActiveModal, NgbCalendar, NgbDate, NgbDateParserFormatter } from '@ng-bootstrap/ng-bootstrap';
import { TranslateService } from '@ngx-translate/core';
import { ComponentOperation } from '../../../../../../../shared/enums/component-operation.enum';

@Component({
  selector: 'app-date-range-pop-up',
  templateUrl: './date-range-pop-up.component.html',
  styleUrls: ['./date-range-pop-up.component.scss']
})
export class DateRangePopUpComponent implements OnInit {
  hoveredDate: NgbDate | null = null;
  toDate: NgbDate | null = null;
  fromDate = this.calendar.getToday();
  today = this.calendar.getToday();
  startDate: any;
  endDate: any;

  @Input() dateFrom: Date;
  @Input() dateTo: Date;
  @Input() operation: ComponentOperation;

  constructor(
    public activeModal: NgbActiveModal,
    private calendar: NgbCalendar,
    public formatter: NgbDateParserFormatter,
    public translate: TranslateService,
  ) {}

  ngOnInit(): void {
    this.startDate = this.dateFrom;
    this.endDate = this.dateTo;
    if (this.startDate && this.endDate) {
      this.fromDate = this.convertToNgbDate(this.startDate) ?? this.today;
      this.toDate = this.convertToNgbDate(this.endDate) ?? this.today;
    }
  }

  getTitle() {
    return ComponentOperation[this.operation];
  }

  onDateSelection(date: NgbDate) {
    if (!this.fromDate && !this.toDate) {
      this.fromDate = date;
      const newDate = new Date(this.fromDate.year, this.fromDate.month - 1, this.fromDate.day, 0, 0, 0, 0);
      this.dateFrom = newDate;
    } else if (this.fromDate && !this.toDate && date && !date.before(this.fromDate)) {
      this.toDate = date;
      const newDate = new Date(this.toDate.year, this.toDate.month - 1, this.toDate.day, 0, 0, 0, 0);
      this.dateTo = newDate;
    } else {
      this.toDate = null;
      this.dateTo = null;
      this.fromDate = date;
      const newDate = new Date(this.fromDate.year, this.fromDate.month - 1, this.fromDate.day, 0, 0, 0, 0);
      this.dateFrom = newDate;
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

  validateInput(currentValue: NgbDate | null, input: string): NgbDate | null {
    const parsed = this.formatter.parse(input);
    return parsed && this.calendar.isValid(NgbDate.from(parsed)) ? NgbDate.from(parsed) : currentValue;
  }

  save() {
    if (this.operation === ComponentOperation.Edit) {
      if (this.startDate !== this.fromDate || this.endDate !== this.toDate) {
        // update api call
        this.activeModal.close({ needRefresh: true });
      } else {
        this.activeModal.close({ needRefresh: false });
      }
    } else {
      const model = {
        fromDate: this.dateFrom,
        toDate: this.dateTo
      }
      this.activeModal.close(model);
    }
  }

  private convertToNgbDate(date: Date) {
    if (date) {
      date = new Date(date);
      const newDate: NgbDate = new NgbDate(date.getUTCFullYear(), date.getUTCMonth() + 1, date.getUTCDate() + 1);
      return newDate ?? undefined;
    }
  }

}
