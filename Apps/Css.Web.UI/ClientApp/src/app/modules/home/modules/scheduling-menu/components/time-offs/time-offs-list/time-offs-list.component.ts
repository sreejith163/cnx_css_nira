import { Component, OnInit } from '@angular/core';
import { Constants } from 'src/app/shared/util/constants.util';

@Component({
  selector: 'app-time-offs-list',
  templateUrl: './time-offs-list.component.html',
  styleUrls: ['./time-offs-list.component.scss']
})
export class TimeOffsListComponent implements OnInit {

  searchKeyword: string;

  maxLength = Constants.DefaultTextMaxLength;

  constructor() { }

  ngOnInit(): void {
  }

  clearSearchText() {
    this.searchKeyword = undefined;
  }

  addTimeOff() {

  }

  search() {
    
  }

}
