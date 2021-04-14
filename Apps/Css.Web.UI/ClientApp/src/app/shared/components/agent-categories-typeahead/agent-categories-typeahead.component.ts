import { Component, Input, OnInit } from '@angular/core';
import { Subject } from 'rxjs';

@Component({
  selector: 'app-agent-categories-typeahead',
  templateUrl: './agent-categories-typeahead.component.html',
  styleUrls: ['./agent-categories-typeahead.component.scss']
})
export class AgentCategoriesTypeaheadComponent implements OnInit {

  pageNumber = 1;
  agentCategoriesBufferSize = 100;
  numberOfItemsFromEndBeforeFetchingMore = 10;
  characterSplice = 25;
  totalItems = 0;
  totalPages: number;
  searchKeyWord = '';
  dropdownSearchKeyWord = '';
  loading = false;

  agentCategoriesBuffer: any[] = [];
  typeAheadInput$ = new Subject<string>();

  @Input() agentCategoryId: number;

  constructor() { }

  ngOnInit(): void {
  }

  onAgentCategoriesScroll(event) {

  }

  onAgentCategoriesScrollToEnd() {

  }

  onAgentCategoriesChange(event) {

  }

  clearAgentCategoriesValues() {

  }

}
