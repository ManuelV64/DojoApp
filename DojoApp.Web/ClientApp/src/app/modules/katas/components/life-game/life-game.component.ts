import { Component, OnInit, OnDestroy } from '@angular/core';
import { LifeGameHttpService } from '../../services/life-game-http.service';
import { FormBuilder, FormGroup, FormControl, Validators } from '@angular/forms';
import { NgbModal } from '@ng-bootstrap/ng-bootstrap';
import { Subscription } from 'rxjs';
import { GameState } from '../../services/life-game-s-r.service';
import { IAlert } from 'src/app/interfaces/i-alert';
import { AlertComponent } from 'src/app/components/alert/alert.component';

@Component({
  selector: 'app-life-game',
  templateUrl: './life-game.component.html',
  styles: []
})
export class LifeGameComponent implements OnInit, OnDestroy {
  /* --------------------------------------
   *    properties
  */
 private Game = GameState;
 public cellList: number[][];

 public showPanelBody: boolean;
 public btnStartGame_disabled: boolean;
 public btnStopGame_disabled: boolean;
 public btnNewGame_disabled: boolean;
 public btnCalculateGen0_disabled: boolean;

 private subscriptions = new Subscription();

 public lifeGameForm: FormGroup;

 /* --------------------------------------
  *    constructor
 */
  constructor(private lifeGameService: LifeGameHttpService,
              private _formBuilder: FormBuilder,
              private modalService: NgbModal) { }

  ngOnInit() {
    this.createForm();

    this.subscriptions.add(
      this.lifeGameService.getAlert$()
        .subscribe(alert => this.alert(alert))
    );

    this.subscriptions.add(
      this.lifeGameService.getGameState$()
       .subscribe(gameState => { this.formState(gameState); })
     );

    this.subscriptions.add(
      this.lifeGameService.getGenerationOfCells$()
        .subscribe(generationOfCells => {
          this.cellList = generationOfCells.cellList;
          this.genNumber.setValue(generationOfCells.genNumber);
        })
    );

    this.lifeGameService.CreateGameId();
  }


  ngOnDestroy() {
    this.subscriptions.unsubscribe();
    this.lifeGameService.DeleteGameId();
  }

/* -----------------------------------------------------------
   *    Create form
  */
 private createForm(): void {
  this.lifeGameForm = this._formBuilder.group({
      percentLiveCells: new FormControl('', [
          Validators.required,
          Validators.pattern('[0-9]*'),
          Validators.min(1),
          Validators.max(99)
      ]),
      genNumber: new FormControl('')
  });

  this.percentLiveCells.statusChanges.subscribe(data => {
      this.btnCalculateGen0_disabled = (this.percentLiveCells.status === 'VALID') ? false : true;
  });

  this.genNumber.disable();
}

/* -----------------------------------------------------------
 *    form controls
*/
get percentLiveCells()  { return this.lifeGameForm.get('percentLiveCells'); }
get genNumber()         { return this.lifeGameForm.get('genNumber'); }

/* -----------------------------------------------------------
    private methods
*/
private alert(alert: IAlert) {
  const modalRef = this.modalService.open(AlertComponent);
  modalRef.componentInstance.Alert = alert;
}

private btnCalculateGen0_click(percentLiveCells: number){
  this.lifeGameService.getInitialGeneration(percentLiveCells);
}

private btnStartGame_click() { this.lifeGameService.StartGame(); }

private btnStopGame_click() { this.lifeGameService.StopGame(); }

private btnNewGame_click() {
  this.lifeGameService.NewGame();
  this.percentLiveCells.setValue('');
  this.percentLiveCells.markAsPristine();
}

private formState(gameState: GameState){
  switch (gameState) {
    case this.Game.withoutGeneration0:
        this.showPanelBody = true;
        this.btnStartGame_disabled = true;
        this.btnStopGame_disabled = true;
        this.btnNewGame_disabled = true;
      break;
    case this.Game.withGeneration0:
        this.showPanelBody = true;
        this.btnStartGame_disabled = false;
        this.btnStopGame_disabled = true;
        this.btnNewGame_disabled = true;
      break;
    case this.Game.started:
        this.showPanelBody = false;
        this.btnStartGame_disabled = true;
        this.btnStopGame_disabled = false;
        this.btnNewGame_disabled = true;
      break;
    case this.Game.stopped:
        this.showPanelBody = false;
        this.btnStartGame_disabled = false;
        this.btnStopGame_disabled = true;
        this.btnNewGame_disabled = false;
      break;
    case this.Game.gameOver:
        this.showPanelBody = false;
        this.btnStartGame_disabled = true;
        this.btnStopGame_disabled = true;
        this.btnNewGame_disabled = false;
      break;
    }
  }
}
