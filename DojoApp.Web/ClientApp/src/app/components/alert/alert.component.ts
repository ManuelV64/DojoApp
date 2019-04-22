import { Component, OnInit, Input } from '@angular/core';
import { IAlert } from 'src/app/interfaces/i-alert';
import { NgbActiveModal } from '@ng-bootstrap/ng-bootstrap';

@Component({
  selector: 'app-alert',
  templateUrl: './alert.component.html',
  styles: []
})
export class AlertComponent implements OnInit {
  @Input() Alert: IAlert;

  constructor(public activeModal: NgbActiveModal) { }

  ngOnInit() {
  }
}
