import { Component, Input, OnInit } from '@angular/core';
import { NgbActiveModal, NgbModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-generic-pop-up',
  templateUrl: './generic-pop-up.component.html',
  styleUrls: ['./generic-pop-up.component.scss']
})
export class GenericPopUpComponent implements OnInit {

  @Input() headingMessage = '';
  @Input() contentMessage = '';
  @Input() confirmButton = 'Yes' ;
  @Input() cancelButton = 'No' ;
  @Input() warning = false;

  constructor(
    public activeModal: NgbActiveModal,
  ) { }

  ngOnInit(): void {
  }

  accept(){
    this.activeModal.close(true);
  }

  dismiss() {
    this.activeModal.close(false);
  }
}
