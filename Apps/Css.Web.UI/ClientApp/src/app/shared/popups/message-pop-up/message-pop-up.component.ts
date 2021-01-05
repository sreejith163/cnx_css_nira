import { Component, Input, OnInit } from '@angular/core';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';
import { TranslateService } from '@ngx-translate/core';

@Component({
  selector: 'app-message-pop-up',
  templateUrl: './message-pop-up.component.html',
  styleUrls: ['./message-pop-up.component.css']
})
export class MessagePopUpComponent implements OnInit {

  @Input() headingMessage = '';
  @Input() contentMessage = '';
  constructor(
    public translate: TranslateService,
    public activeModal: NgbActiveModal
  ) { }

  ngOnInit(): void {
  }

}
