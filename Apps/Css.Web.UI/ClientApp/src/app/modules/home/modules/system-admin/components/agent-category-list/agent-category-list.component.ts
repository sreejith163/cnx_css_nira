import { Component, OnInit } from '@angular/core';
import { NgbModal, NgbModalOptions } from '@ng-bootstrap/ng-bootstrap';
import { ConfirmationPopUpComponent } from 'src/app/shared/popups/confirmation-pop-up/confirmation-pop-up.component';
import { MessagePopUpComponent } from 'src/app/shared/popups/message-pop-up/message-pop-up.component';
import { Constants } from 'src/app/shared/util/constants.util';
import { PaginationSize } from 'src/app/shared/models/pagination-size.model';
import { AgentCategory } from '../../models/agent-category.model';
import { AgentCategoryListService } from '../../services/agent-category-list.service';
import { AddAgentCategoryComponent } from '../add-agent-category/add-agent-category.component';
import { DataType } from '../../enum/data-type.enum';
import { Translation } from 'src/app/shared/models/translation.model';

@Component({
  selector: 'app-agent-category-list',
  templateUrl: './agent-category-list.component.html',
  styleUrls: ['./agent-category-list.component.scss']
})
export class AgentCategoryListComponent implements OnInit {

  currentPage = 1;
  pageSize = 10;
  characterSplice = 25;
  dataType = DataType;
  translationValues: Translation[];
  paginationSize: PaginationSize[] = [];
  totalCategoryRecord: number;

  totalCategories: AgentCategory[] = [];
  categories: AgentCategory[] = [];

  constructor(
    private agentCategoryListService: AgentCategoryListService,
    private modalService: NgbModal
  ) { }

  ngOnInit(): void {
    this.translationValues = Constants.agentCategoriesTranslationValues;
    this.totalCategories = this.agentCategoryListService.getAgentCategories();
    this.totalCategoryRecord = this.totalCategories.length;
    this.categories = this.totalCategories.sort(
      (a, b) =>
        new Date(b.createdDate).getTime() - new Date(a.createdDate).getTime()
    );
    this.paginationSize = Constants.paginationSize;
  }

  changePageSize(pageSize) {
    this.pageSize = Number(pageSize);
  }

  changePage(page) {
    this.currentPage = page;
  }

  openModal() {
    const modalRef = this.getModalPopup(AddAgentCategoryComponent, 'lg');
    modalRef.componentInstance.title = 'Add';
    modalRef.componentInstance.translationValues = this.translationValues;
  }

  edit(agentCategoryData: AgentCategory) {
    const modalRef = this.getModalPopup(AddAgentCategoryComponent, 'lg');
    modalRef.componentInstance.title = 'Edit';
    modalRef.componentInstance.agentCategoryData = agentCategoryData;
    modalRef.componentInstance.translationValues = this.translationValues;
  }

  delete(employeeId: number) {
    const modalRef = this.getModalPopup(ConfirmationPopUpComponent, 'sm');
    modalRef.componentInstance.headingMessage = 'Are you sure?';
    modalRef.componentInstance.contentMessage =
      'You won’t be able to revert this!';
    modalRef.componentInstance.deleteRecordIndex = employeeId;
    modalRef.result.then((result) => {
      if (result && result === employeeId) {
        this.agentCategoryListService.deletegentcategory(employeeId);
        const modal = this.getModalPopup(MessagePopUpComponent, 'sm');
        modal.componentInstance.headingMessage = 'Success';
        modal.componentInstance.contentMessage = 'The record has been deleted!';
      }
    });
  }

  private getModalPopup(component: any, size: string) {
    const options: NgbModalOptions = { backdrop: false, centered: true, size };
    return this.modalService.open(component, options);
  }
}
