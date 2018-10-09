import { Component , ViewChild} from '@angular/core';
import { AlertComponent} from "../components/alert/alert.component";

@Component({
  selector: 'app-counter-component',
  templateUrl: './counter.component.html'
})
export class CounterComponent {
  @ViewChild(AlertComponent) alert: AlertComponent;
  public currentCount = 0;

  public incrementCounter() {
    this.currentCount++;

    if (this.currentCount==5)
      this.alert.danger("Mensaje de peligro");
    else 
      this.alert.hide();    
  }
}
