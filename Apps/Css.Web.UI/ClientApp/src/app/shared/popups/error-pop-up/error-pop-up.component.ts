import { Component, Input, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-error-pop-up',
  templateUrl: './error-pop-up.component.html',
  styleUrls: ['./error-pop-up.component.scss']
})
export class ErrorPopUpComponent implements OnInit {

  @Input() headingMessage = '';
  @Input() contentMessage = '';

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
