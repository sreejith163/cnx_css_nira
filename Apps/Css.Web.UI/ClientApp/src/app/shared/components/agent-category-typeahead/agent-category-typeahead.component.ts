import { Component, EventEmitter, Input, OnChanges, OnDestroy, OnInit, Output, SimpleChanges } from '@angular/core';
import { Subject, SubscriptionLike as ISubscription } from 'rxjs';
import { debounceTime, distinctUntilChanged, switchMap } from 'rxjs/operators';
import { AgentCategoryBase } from 'src/app/modules/home/modules/system-admin/models/agent-category-base.model';
import { AgentCategoryQueryParams } from 'src/app/modules/home/modules/system-admin/models/agent-category-query-params.model';
import { AgentCategoryService } from 'src/app/modules/home/modules/system-admin/services/agent-category.service';

@Component({
  selector: 'app-agent-category-typeahead',
  templateUrl: './agent-category-typeahead.component.html',
  styleUrls: ['./agent-category-typeahead.component.scss']
})
export class AgentCategoryTypeaheadComponent implements OnInit, OnDestroy, OnChanges {

  pageNumber = 1;
  agentCategoryItemsBufferSize = 100;
  numberOfItemsFromEndBeforeFetchingMore = 10;
  characterSplice = 25;
  totalItems = 0;
  totalPages: number;
  searchKeyWord = '';
  dropdownSearchKeyWord = '';
  loading = false;

  agentCategoryItemsBuffer: AgentCategoryBase[] = [];
  typeAheadInput$ = new Subject<string>();

  typeAheadValueSubscription: ISubscription;
  getAgentCategorySubscription: ISubscription;
  subscriptions: ISubscription[] = [];

  @Input() clientId: number;
  @Input() clientLobGroupId: number;
  @Input() skillGroupId: number;
  @Input() skillTagId: number;
  @Input() agentCategoryId: number;
  @Input() hierarchy: boolean;
  @Output() agentCategorySelected = new EventEmitter();

  constructor(
    private adminCategoryService: AgentCategoryService,
  ) { }

  ngOnInit(): void {
    if (!this.hierarchy) {
      this.subscribeToAgentCategory();
    } else {
      this.agentCategoryItemsBuffer = [];
      this.totalItems = 0;
    }
    this.subscribeToSearching();
  }

  ngOnDestroy() {
    this.subscriptions.forEach(subscription => {
      if (subscription) {
        subscription.unsubscribe();
      }
    });
  }

  ngOnChanges() {
    if (this.skillTagId || this.agentCategoryId) {
      this.pageNumber = 1;
      this.subscribeToAgentCategory();
    } else {
      this.agentCategoryItemsBuffer = [];
      this.totalItems = 0;
    }
  }

  onAgentCategoryScrollToEnd() {
    this.fetchMoreAgentCategory();
  }

  onAgentCategoryScroll({ end }) {
    if (this.loading || this.agentCategoryItemsBufferSize <= this.agentCategoryItemsBuffer.length) {
      return;
    }

    if (end + this.numberOfItemsFromEndBeforeFetchingMore >= this.agentCategoryItemsBuffer.length) {
      this.fetchMoreAgentCategory();
    }
  }

  onAgentCategoryChange(event: AgentCategoryBase) {
    this.agentCategorySelected.emit(event?.id);
  }

  clearAgentCategoryValues() {
    this.searchKeyWord = '';
    this.pageNumber = 1;
    this.subscribeToAgentCategory();
  }

  private fetchMoreAgentCategory() {
    if (this.pageNumber < this.totalPages) {
      this.pageNumber += 1;
      this.subscribeToAgentCategory(true);
    }
  }

  private subscribeToAgentCategory(needBufferAdd?: boolean) {
    this.loading = true;
    this.getAgentCategorySubscription = this.getAgentCategory(this.dropdownSearchKeyWord).subscribe(
      response => {
        if (response?.body) {
          this.setPaginationValues(response);
          this.agentCategoryItemsBuffer = needBufferAdd ? this.agentCategoryItemsBuffer.concat(response.body) : response.body;
        }
        this.loading = false;
      }, err => this.loading = false);

    this.subscriptions.push(this.getAgentCategorySubscription);
  }

  private subscribeToSearching() {
    this.typeAheadValueSubscription = this.typeAheadInput$.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      switchMap(term => this.getAgentCategory(term))
    ).subscribe(response => {
      if (response.body) {
        this.setPaginationValues(response);
        this.agentCategoryItemsBuffer = response.body;
      }
    }, (error) => {
      console.log(error);
    });

    this.subscriptions.push(this.typeAheadValueSubscription);
  }

  private setPaginationValues(response: any) {
    const paging = JSON.parse(response.headers.get('x-pagination'));
    if (paging) {
      this.totalItems = paging.totalCount;
      this.totalPages = paging.totalPages;
    }
  }

  private getQueryParams(searchkeyword?: string) {
    const queryParams = new AgentCategoryQueryParams();
    queryParams.pageSize = this.agentCategoryItemsBufferSize;
    queryParams.pageNumber = this.pageNumber;
    queryParams.searchKeyword = searchkeyword ?? this.searchKeyWord;
    queryParams.skipPageSize = false;
    queryParams.orderBy = 'name';
    queryParams.sortBy = 'asc';
    queryParams.fields = 'id, name';

    return queryParams;
  }

  private getAgentCategory(searchKeyword?: string) {
    const queryParams = this.getQueryParams(searchKeyword);
    if(this.dropdownSearchKeyWord !== queryParams.searchKeyword) {
      this.pageNumber = 1;
      queryParams.pageNumber = 1;
    }
    this.dropdownSearchKeyWord = queryParams.searchKeyword;
    return this.adminCategoryService.getAgentcategories(queryParams);
  }

}
