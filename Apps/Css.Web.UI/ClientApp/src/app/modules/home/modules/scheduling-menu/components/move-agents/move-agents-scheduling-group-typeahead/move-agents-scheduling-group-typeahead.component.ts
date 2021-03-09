import { Component, EventEmitter, Input, OnChanges, OnDestroy, OnInit, Output, SimpleChanges, ViewChild } from '@angular/core';
import { Subject, SubscriptionLike as ISubscription } from 'rxjs';
import { debounceTime, distinctUntilChanged, switchMap } from 'rxjs/operators';
import { AgentSchedulingGroupBase } from 'src/app/modules/home/modules/setup-menu/models/agent-scheduling-group-base.model';
import { AgentSchedulingGroupQueryParams } from 'src/app/modules/home/modules/setup-menu/models/agent-scheduling-group-query-params.model';
import { AgentSchedulingGroupService } from 'src/app/shared/services/agent-scheduling-group.service';
import { MoveAgentsService } from '../../../services/move-agents.service';

@Component({
  selector: 'app-move-agents-scheduling-group-typeahead',
  templateUrl: './move-agents-scheduling-group-typeahead.component.html',
  styleUrls: ['./move-agents-scheduling-group-typeahead.component.scss']
})

export class MoveAgentsSchedulingGroupTypeaheadComponent implements OnInit, OnDestroy, OnChanges {

  pageNumber = 1;
  agentSchedulingGroupItemsBufferSize = 10;
  numberOfItemsFromEndBeforeFetchingMore = 10;
  characterSplice = 25;
  totalItems = 0;
  totalPages: number;
  searchKeyWord = '';
  dropdownSearchKeyWord = '';
  loading = false;

  agentSchedulingGroupItemsBuffer: AgentSchedulingGroupBase[] = [];
  typeAheadInput$ = new Subject<string>();

  typeAheadValueSubscription: ISubscription;
  getAgentSchedulingGroupSubscription: ISubscription;
  invalidSelectionSubscription: ISubscription;
  subscriptions: ISubscription[] = [];

  @Input() listPosition: string;
  @Input() agentSchedulingGroupId: number;
  @Output() agentSchedulingGroupSelected = new EventEmitter();

  constructor(
    private agentSchedulingGroupService: AgentSchedulingGroupService,
    private moveAgentService: MoveAgentsService
  ) {
   }

  ngOnInit(): void {
    this.subscribeToAgentSchedulingGroups();
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
    if (this.agentSchedulingGroupId) {
      this.pageNumber = 1;
      this.subscribeToAgentSchedulingGroups();
    } else {
      this.agentSchedulingGroupItemsBuffer = [];
      this.totalItems = 0;
    }
  }

  onAgentSchedulingGroupScrollToEnd() {
    this.fetchMoreAgentSchedulingGroups();
  }

  onAgentSchedulingGroupScroll({ end }) {
    if (this.loading || this.agentSchedulingGroupItemsBufferSize <= this.agentSchedulingGroupItemsBuffer.length) {
      return;
    }

    if (end + this.numberOfItemsFromEndBeforeFetchingMore >= this.agentSchedulingGroupItemsBuffer.length) {
      this.fetchMoreAgentSchedulingGroups();
    }
  }

  onAgentSchedulingGroupChange(event: AgentSchedulingGroupBase) {
    this.agentSchedulingGroupSelected.emit(event?.id);
  }

  clearAgentSchedulingGroupValues() {
    if(this.listPosition == 'left'){
      this.moveAgentService.agentSchedulingGroupIdLeftSubject$.next(undefined);
    }

    if(this.listPosition == 'right'){
      this.moveAgentService.agentSchedulingGroupIdRightSubject$.next(undefined);
    }

    this.searchKeyWord = '';
    this.pageNumber = 1;
    this.subscribeToAgentSchedulingGroups();
  }

  private fetchMoreAgentSchedulingGroups() {
    if (this.pageNumber < this.totalPages) {
      this.pageNumber += 1;
      this.subscribeToAgentSchedulingGroups(true);
    }
  }

  private subscribeToAgentSchedulingGroups(needBufferAdd?: boolean) {
    this.loading = true;
    this.getAgentSchedulingGroupSubscription = this.getAgentSchedulingGroups(this.dropdownSearchKeyWord).subscribe(
      response => {
        if (response?.body) {
          this.setPaginationValues(response);
          this.agentSchedulingGroupItemsBuffer = needBufferAdd ? this.agentSchedulingGroupItemsBuffer.concat(response.body) : response.body;
        }
        this.loading = false;
      }, err => this.loading = false);

    this.subscriptions.push(this.getAgentSchedulingGroupSubscription);
  }


  private subscribeToSearching() {
    this.typeAheadValueSubscription = this.typeAheadInput$.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      switchMap(term => this.getAgentSchedulingGroups(term))
    ).subscribe(response => {
      if (response.body) {
        this.setPaginationValues(response);
        this.agentSchedulingGroupItemsBuffer = response.body;
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
    const queryParams = new AgentSchedulingGroupQueryParams();
    queryParams.pageSize = this.agentSchedulingGroupItemsBufferSize;
    queryParams.pageNumber = this.pageNumber;
    queryParams.searchKeyword = searchkeyword ?? this.searchKeyWord;
    queryParams.skipPageSize = false;
    queryParams.orderBy = 'name';
    queryParams.sortBy = 'asc';
    queryParams.fields = 'id, name';

    return queryParams;
  }

  private getAgentSchedulingGroups(searchKeyword?: string) {
    const queryParams = this.getQueryParams(searchKeyword);
    if(this.dropdownSearchKeyWord !== queryParams.searchKeyword) {
      this.pageNumber = 1;
      queryParams.pageNumber = 1;
    }
    this.dropdownSearchKeyWord = queryParams.searchKeyword;
    return this.agentSchedulingGroupService.getAgentSchedulingGroups(queryParams);
  }
}