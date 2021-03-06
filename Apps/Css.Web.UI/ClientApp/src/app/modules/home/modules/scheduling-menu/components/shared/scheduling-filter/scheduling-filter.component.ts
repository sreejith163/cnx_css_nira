import { Component, EventEmitter, Input, OnInit, Output } from '@angular/core';
import { NgbCalendar, NgbDate } from '@ng-bootstrap/ng-bootstrap';
import { Constants } from 'src/app/shared/util/constants.util';
import { AgentScheduleType } from '../../../enums/agent-schedule-type.enum';

@Component({
  selector: 'app-scheduling-filter',
  templateUrl: './scheduling-filter.component.html',
  styleUrls: ['./scheduling-filter.component.scss']
})
export class SchedulingFilterComponent implements OnInit {

  startDate: any;
  searchKeyword: string;
  today = this.calendar.getToday();
  agentSchedulingGroupId: number;
  maxLength = Constants.DefaultTextMaxLength;

  @Input() agentScheduleType: AgentScheduleType;
  @Output() agentSchedulingGroupSelected = new EventEmitter();
  @Output() startDateSelected = new EventEmitter();
  @Output() keywordToSearch = new EventEmitter();

  constructor(
    private calendar: NgbCalendar,
  ) { 
  }

  ngOnInit(): void {
    this.setStartDateAsToday();
  }
  

  clearSearchText() {
    this.searchKeyword = undefined;
    this.keywordToSearch.emit(this.searchKeyword);
  }

  onSchedulingGroupChange(agentSchedulingGroupId: number) {
    this.agentSchedulingGroupSelected.emit(agentSchedulingGroupId);
  }

  setStartDateAsToday() {
    this.startDate = this.today;
    const currentDate = this.setCurrentDate();
    this.startDateSelected.emit(currentDate);
  }

  onSelectStartDate(date: NgbDate) {
    this.startDate = date;
    const currentDate = this.setCurrentDate();
    this.startDateSelected.emit(currentDate);
  }

  search() {
    this.keywordToSearch.emit(this.searchKeyword);
  }

  setCurrentDate() {

    const day = this.startDate.day < 10 ? '0' + this.startDate.day : this.startDate.day;
    const month = this.startDate.month < 10 ? '0' + this.startDate.month : this.startDate.month;

    return this.startDate.year + '-' + month + '-' + day;
  }

}
