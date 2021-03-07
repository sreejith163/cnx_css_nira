import { Component, ElementRef, Input, OnInit, ViewChild } from '@angular/core';
import { NgxSpinnerService } from 'ngx-spinner';
import { Observable } from 'rxjs';
import { AgentAdminDetails } from '../../../models/agent-admin-details.model';
import { MoveAgentsService } from '../../../services/move-agents.service';

@Component({
  selector: 'app-move-agents-list',
  templateUrl: './move-agents-list.component.html',
  styleUrls: ['./move-agents-list.component.scss']
})
export class MoveAgentsListComponent implements OnInit {

  @ViewChild('agentListTable', {static: false}) agentListTable: ElementRef;
  @ViewChild('agentListTableContainer', {static: false}) agentListTableContainer: ElementRef;

  searchKeyword: string;

  @Input() listPosition: string;
  listPositionTypeAhead: string;
  
  @Input() spinner: string;

  agentAdmins$: Observable<Array<AgentAdminDetails>>;
  totalAgentAdminsRecord$: Observable<number>;

  agentSchedulingGroupId?: number;

  isSelected: boolean[] = [];
  selectedAgentAdminIds: string[] = [];

  constructor(private moveAgentAdminService: MoveAgentsService, private spinnerService: NgxSpinnerService) { 
    this.subscribeToMoveAgentsUpdate();
  }

  ngOnInit(): void {
    this.listPositionTypeAhead = this.listPosition;
  }

  subscribeToMoveAgentsUpdate(){
    this.moveAgentAdminService.agentAdminsUpdatedSubject$.subscribe(isUpdated=>{
        if(this.listPosition == 'left'){
          this.isSelected = [];  
          this.selectedAgentAdminIds = [];
          this.moveAgentAdminService.selectedAgentAdminIdsSubject$.next(null);
          this.loadAgentAdminsLeft(this.agentSchedulingGroupId);
        }
    
        if(this.listPosition == 'right'){
          this.loadAgentAdminsRight(this.agentSchedulingGroupId);
        }
    });
  }

  search(){
    this.loadAgentAdminList();
  }

  clearSearchText() {
    this.searchKeyword = undefined;
    this.loadAgentAdminList();
  }
  
  onScrollingFinished() {
    if(this.agentListTableContainer.nativeElement.offsetHeight + 
      this.agentListTableContainer.nativeElement.scrollTop 
      >= this.agentListTable.nativeElement.offsetHeight){
        if(this.listPosition == 'left'){
          this.moveAgentAdminService.loadMoreLeft();
        }

        if(this.listPosition == 'right'){
          this.moveAgentAdminService.loadMoreRight();
        }
      }
  }

  loadAgentAdminList(){
    if(this.listPosition == 'left'){
      this.loadAgentAdminsLeft(this.agentSchedulingGroupId);
    }

    if(this.listPosition == 'right'){
      this.loadAgentAdminsRight(this.agentSchedulingGroupId);
    }
  }


  onSchedulingGroupChange(schedulingGroupId: number) {

    if(this.listPosition == 'left'){
      this.isSelected = [];  
      this.selectedAgentAdminIds = [];
      this.moveAgentAdminService.selectedAgentAdminIdsSubject$.next(null);
      this.loadAgentAdminsLeft(schedulingGroupId);
    }

    if(this.listPosition == 'right'){
      this.loadAgentAdminsRight(schedulingGroupId);
    }

  }

  loadAgentAdminsLeft(schedulingGroupId: number){
    this.agentSchedulingGroupId = schedulingGroupId;
    if (this.agentSchedulingGroupId) {

      this.agentAdmins$ = null;
      this.moveAgentAdminService.unloadAgentAdminsLeft();
      this.totalAgentAdminsRecord$ = null;
      this.moveAgentAdminService.totalAgentAdminsSubjectLeft$.next(null);
      
      var searchKeyword = this.searchKeyword ?? '';

      this.moveAgentAdminService.loadAgentAdminsLeft(schedulingGroupId, searchKeyword);
      this.totalAgentAdminsRecord$ = this.moveAgentAdminService.totalAgentAdminsLeft$;
      this.agentAdmins$ = this.moveAgentAdminService.agentAdminsLeft$;

    } else {
      this.moveAgentAdminService.selectedAgentAdminIdsSubject$.next(null);

      this.totalAgentAdminsRecord$ = null;
      this.moveAgentAdminService.totalAgentAdminsSubjectLeft$.next(null);
      this.agentSchedulingGroupId = 0;
      this.agentAdmins$ = null;
      this.moveAgentAdminService.unloadAgentAdminsLeft();
      this.selectedAgentAdminIds = [];
      this.isSelected = [];
    }
  }

  loadAgentAdminsRight(schedulingGroupId: number){
    this.agentSchedulingGroupId = schedulingGroupId;
    if (this.agentSchedulingGroupId) {
      this.agentAdmins$ = null;
      this.moveAgentAdminService.unloadAgentAdminsRight();
      this.isSelected = [];
      this.totalAgentAdminsRecord$ = null;
      this.moveAgentAdminService.totalAgentAdminsSubjectRight$.next(null);
      this.selectedAgentAdminIds = [];
      
      var searchKeyword = this.searchKeyword ?? '';

      this.moveAgentAdminService.loadAgentAdminsRight(schedulingGroupId, searchKeyword);
      this.totalAgentAdminsRecord$ = this.moveAgentAdminService.totalAgentAdminsRight$;
      this.agentAdmins$ = this.moveAgentAdminService.agentAdminsRight$;

    } else {
      this.totalAgentAdminsRecord$ = null;
      this.moveAgentAdminService.totalAgentAdminsSubjectRight$.next(null);
      this.agentSchedulingGroupId = 0;
      this.agentAdmins$ = null;
      this.moveAgentAdminService.unloadAgentAdminsRight();
      this.selectedAgentAdminIds = [];
      this.isSelected = [];
    }
  }

  toggleSelected(agentAdminId){
    // console.log(agentAdmin)
    if(!(this.selectedAgentAdminIds.includes(agentAdminId))){
      // push the ids to an array
      this.selectedAgentAdminIds.push(agentAdminId);
    }else{
      const indexId = this.selectedAgentAdminIds.indexOf(agentAdminId,0);
      // remove the ids from the array
      if (indexId > -1 ) {
        this.selectedAgentAdminIds.splice(indexId, 1);
      }
    }

    this.moveAgentAdminService.selectedAgentAdminIdsSubject$.next(this.selectedAgentAdminIds);
  }

}
