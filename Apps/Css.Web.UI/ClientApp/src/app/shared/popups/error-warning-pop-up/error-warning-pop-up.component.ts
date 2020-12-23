import { Component, Input, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { MessageType } from '../../enums/message-type.enum';

@Component({
  selector: 'app-error-warning-pop-up',
  templateUrl: './error-warning-pop-up.component.html',
  styleUrls: ['./error-warning-pop-up.component.scss']
})
export class ErrorWarningPopUpComponent implements OnInit {

  @Input() headingMessage: string;
  @Input() contentMessage: any;
  @Input() messageType: MessageType;
  constructor(
    public activeModal: NgbActiveModal
  ) { }

  ngOnInit(): void {
  }

}
