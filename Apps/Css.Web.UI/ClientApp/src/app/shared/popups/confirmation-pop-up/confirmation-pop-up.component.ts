import { Component, Input, OnInit } from '@angular/core';
import { NgbActiveModal, NgbModal, NgbModalOptions } from '@ng-bootstrap/ng-bootstrap';
import { MessagePopUpComponent } from '../message-pop-up/message-pop-up.component';

@Component({
  selector: 'app-confirmation-pop-up',
  templateUrl: './confirmation-pop-up.component.html',
  styleUrls: ['./confirmation-pop-up.component.css']
})
export class ConfirmationPopUpComponent implements OnInit {
  @Input() headingMessage = '';
  @Input() contentMessage = '';
  @Input() deleteRecordIndex ;

  constructor(
    public activeModal: NgbActiveModal,
    private modalService: NgbModal
  ) { }

  ngOnInit(): void {
  }

  confirmDelete() {
    this.activeModal.close(this.deleteRecordIndex);
  }

}
