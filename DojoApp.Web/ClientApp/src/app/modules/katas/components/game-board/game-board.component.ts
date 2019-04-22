import { Component, Input } from '@angular/core';

@Component({
  selector: 'app-game-board',
  templateUrl: './game-board.component.html',
  styles: []
})
export class GameBoardComponent {
  @Input() cellList: number[][];

  imgLiveCell = '/assets/LiveCell6.png';
  imgDiedCell = '/assets/DiedCell6.png';

  constructor() {}
}
