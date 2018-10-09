import { Component, OnInit } from '@angular/core';

@Component({
  selector: 'app-alert',
  templateUrl: './alert.component.html',
  styleUrls: ['./alert.component.css']
})
export class AlertComponent implements OnInit {

  private _alertClass : string;
  private _alertHeader: string;
  private _alertMsg : string;
  private _showAlert : boolean;

  constructor() { }

  ngOnInit() {
  }


  success(msg:string){
    this._alertClass="alert-success";
    this.showAlert(msg);
  }

  danger(msg:string){
    this._alertClass="alert-danger";
    this.showAlert(msg);
  }

  private showAlert(msg:string){
    this._alertMsg=msg;
    this._showAlert=true;
  }

  hide (){ 
    this._showAlert=false; 
  }

}
