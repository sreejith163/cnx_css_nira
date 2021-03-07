import { Component, EventEmitter, Input, OnChanges, OnDestroy, OnInit, Output, SimpleChanges } from '@angular/core';
import { Subject, SubscriptionLike as ISubscription } from 'rxjs';
import { debounceTime, distinctUntilChanged, switchMap } from 'rxjs/operators';
import { ClientLobGroupBase } from 'src/app/modules/home/modules/setup-menu/models/client-lob-group-base.model';
import { ClientLobGroupQueryParameters } from 'src/app/modules/home/modules/setup-menu/models/client-lob-group-query-parameters.model';
import { ClientLobGroupService } from 'src/app/modules/home/modules/setup-menu/services/client-lob-group.service';

@Component({
  selector: 'app-client-lob-group-typeahead',
  templateUrl: './client-lob-group-typeahead.component.html',
  styleUrls: ['./client-lob-group-typeahead.component.scss']
})

export class ClientLobGroupTypeaheadComponent implements OnInit, OnDestroy, OnChanges {

  pageNumber = 1;
  clientLobItemsBufferSize = 10;
  numberOfItemsFromEndBeforeFetchingMore = 10;
  characterSplice = 25;
  totalItems = 0;
  totalPages: number;
  searchKeyWord = '';
  loading = false;

  clientLobItemsBuffer: ClientLobGroupBase[] = [];
  typeAheadInput$ = new Subject<string>();

  typeAheadValueSubscription: ISubscription;
  getClientLobNamesSubscription: ISubscription;
  subscriptions: ISubscription[] = [];

  @Input() clientId: number;
  @Input() clientLobId: number;
  @Input() heirarchy: boolean;
  @Output() clientLobSelected = new EventEmitter();

  constructor(
    private clientLobService: ClientLobGroupService,
  ) { }

  ngOnInit(): void {
    if (!this.heirarchy) {
      this.subscribeToClientLobs();
    } else {
      this.clientLobItemsBuffer = [];
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
    if (this.clientId) {
      this.pageNumber = 1;
      this.subscribeToClientLobs();
    } else {
      this.clientLobItemsBuffer = [];
      this.totalItems = 0;
    }
  }

  onClientLobScrollToEnd() {
    this.fetchMoreClientLobs();
  }

  onClientLobScroll({ end }) {
    if (this.loading || this.clientLobItemsBufferSize <= this.clientLobItemsBuffer.length) {
      return;
    }

    if (end + this.numberOfItemsFromEndBeforeFetchingMore >= this.clientLobItemsBuffer.length) {
      this.fetchMoreClientLobs();
    }
  }

  onClientLobChange(event: ClientLobGroupBase) {
    this.clientLobSelected.emit(event?.id);
  }

  clearSelectedValues() {
    this.searchKeyWord = '';
    this.pageNumber = 1;
    this.subscribeToClientLobs();
  }

  private fetchMoreClientLobs() {
    if (this.pageNumber < this.totalPages) {
      this.pageNumber += 1;
      this.subscribeToClientLobs(true);
    }
  }

  private subscribeToClientLobs(needBufferAdd?: boolean) {
    this.loading = true;
    this.getClientLobNamesSubscription = this.getClientLobs().subscribe(
      response => {
        if (response?.body) {
          this.setPaginationValues(response);
          this.clientLobItemsBuffer = needBufferAdd ? this.clientLobItemsBuffer.concat(response.body) : response.body;
        }
        this.loading = false;
      }, err => this.loading = false);

    this.subscriptions.push(this.getClientLobNamesSubscription);
  }

  private subscribeToSearching() {
    this.typeAheadValueSubscription = this.typeAheadInput$.pipe(
      debounceTime(200),
      distinctUntilChanged(),
      switchMap(term => this.getClientLobs(term))
    ).subscribe(response => {
      if (response.body) {
        this.setPaginationValues(response);
        this.clientLobItemsBuffer = response.body;
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
    const queryParams = new ClientLobGroupQueryParameters();
    queryParams.clientId = this.clientId ?? undefined;
    queryParams.pageSize = this.clientLobItemsBufferSize;
    queryParams.pageNumber = this.pageNumber;
    queryParams.searchKeyword = searchkeyword ?? this.searchKeyWord;
    queryParams.skipPageSize = false;
    queryParams.orderBy = undefined;
    queryParams.fields = 'id, name';

    return queryParams;
  }

  private getClientLobs(searchKeyword?: string) {
    const queryParams = this.getQueryParams(searchKeyword);
    return this.clientLobService.getClientLOBGroups(queryParams);
  }
}
