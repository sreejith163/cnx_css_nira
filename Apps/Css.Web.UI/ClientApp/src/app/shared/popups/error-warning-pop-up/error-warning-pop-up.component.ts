import { Component, Input, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { ContentType } from '../../enums/content-type.enum';

@Component({
  selector: 'app-error-warning-pop-up',
  templateUrl: './error-warning-pop-up.component.html',
  styleUrls: ['./error-warning-pop-up.component.scss']
})
export class ErrorWarningPopUpComponent implements OnInit {

  renderType = ContentType;

  @Input() headingMessage: string;
  @Input() contentMessage: any;
  @Input() messageType: ContentType;

  constructor(
    public activeModal: NgbActiveModal
  ) { }

  ngOnInit(): void {
  }
}
