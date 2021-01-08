import { Component, EventEmitter, Input, OnChanges, OnDestroy, OnInit, Output, SimpleChanges } from '@angular/core';
import { Subject, SubscriptionLike as ISubscription } from 'rxjs';
import { debounceTime, distinctUntilChanged, switchMap } from 'rxjs/operators';
import { AgentSchedulingGroupBase } from 'src/app/modules/home/modules/setup-menu/models/agent-scheduling-group-base.model';
import { AgentSchedulingGroupQueryParams } from 'src/app/modules/home/modules/setup-menu/models/agent-scheduling-group-query-params.model';
import { AgentSchedulingGroupService } from '../../services/agent-scheduling-group.service';

@Component({
  selector: 'app-agent-scheduling-group-typeahead',
  templateUrl: './agent-scheduling-group-typeahead.component.html',
  styleUrls: ['./agent-scheduling-group-typeahead.component.scss']
})
export class AgentSchedulingGroupTypeaheadComponent implements OnInit, OnDestroy, OnChanges {

  pageNumber = 1;
  agentSchedulingGroupItemsBufferSize = 10;
  numberOfItemsFromEndBeforeFetchingMore = 10;
  characterSplice = 25;
  totalItems = 0;
  totalPages: number;
  searchKeyWord = '';
  loading = false;

  agentSchedulingGroupItemsBuffer: AgentSchedulingGroupBase[] = [];
  typeAheadInput$ = new Subject<string>();

  typeAheadValueSubscription: ISubscription;
  getAgentSchedulingGroupSubscription: ISubscription;
  subscriptions: ISubscription[] = [];

  @Input() clientId: number;
  @Input() clientLobGroupId: number;
  @Input() skillGroupId: number;
  @Input() skillTagId: number;
  @Input() agentSchedulingGroupId: number;
  @Output() agentSchedulingGroupSelected = new EventEmitter();

  constructor(
    private agentSchedulingGroupService: AgentSchedulingGroupService,
  ) { }

  ngOnInit(): void {
    this.subscribeToAgentSchedulingGroups(false, true);
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

  private subscribeToAgentSchedulingGroups(needBufferAdd?: boolean, sendInitialValue?: boolean) {
    this.loading = true;
    this.getAgentSchedulingGroupSubscription = this.getAgentSchedulingGroups().subscribe(
      response => {
        if (response?.body) {
          this.setPaginationValues(response);
          this.agentSchedulingGroupItemsBuffer = needBufferAdd ? this.agentSchedulingGroupItemsBuffer.concat(response.body) : response.body;
          if (sendInitialValue) {
            this.agentSchedulingGroupId = this.agentSchedulingGroupItemsBuffer[0]?.id;
            this.agentSchedulingGroupSelected.emit(this.agentSchedulingGroupId);
          }
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
    queryParams.clientId = this.clientId ?? undefined;
    queryParams.clientLobGroupId = this.clientLobGroupId ?? undefined;
    queryParams.skillGroupId = this.skillGroupId ?? undefined;
    queryParams.skillTagId = this.skillTagId ?? undefined;
    queryParams.pageSize = this.agentSchedulingGroupItemsBufferSize;
    queryParams.pageNumber = this.pageNumber;
    queryParams.searchKeyword = searchkeyword ?? this.searchKeyWord;
    queryParams.orderBy = undefined;
    queryParams.fields = 'id, name';

    return queryParams;
  }

  private getAgentSchedulingGroups(searchKeyword?: string) {
    const queryParams = this.getQueryParams(searchKeyword);
    return this.agentSchedulingGroupService.getAgentSchedulingGroups(queryParams);
  }
}

