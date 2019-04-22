import { Injectable } from '@angular/core';
import { ICoordinate2D } from '../interfaces/i-coordinate2-d';
import { HubConnection, HubConnectionBuilder } from '@aspnet/signalr';
import { Observable, Subject, observable, pipe, interval, of, BehaviorSubject } from 'rxjs';
import { takeWhile, delay} from 'rxjs/operators';
import { IGenerationOfCells , IGenerationOfCellsDto} from '../interfaces/i-generation-of-cells';
import { IAlert } from 'src/app/interfaces/i-alert';

export enum GameState {withoutGeneration0, withGeneration0, started, stopped, gameOver}

// @Injectable({
//   providedIn: 'root'
// })
export class LifeGameSRService {
  private MAX_X = 20;
  private MAX_Y = 20;
  private emptyGeneration: IGenerationOfCells = this.genDtoToGenCells({ liveCellList: [], genNumber: 0 });
  private Game = GameState;

  private alertSubject             = new Subject<IAlert>();
  private gameStateSubject         = new BehaviorSubject<GameState>(this.Game.withoutGeneration0);
  private generationOfCellsSubject = new BehaviorSubject<IGenerationOfCells>(this.emptyGeneration);
  private delayNextInvoke = this.generationOfCellsSubject
            .pipe(delay(500),
                  takeWhile(() => this.gameStateSubject.value === this.Game.started ));

  // private GenerationOfCells$: Observable<IGenerationOfCells>;

  private _hubConnectionGame: HubConnection;
  private _connectionId = '';

  /* -----------------------------------------------------------
   *    constructor
  */
  constructor() {
    this.createConnection();
    this.registerOnServerEvents();
    this.NewGame();
  }

  /* ------------------------------------------------------------
   *    Create Connections
  */
  private createConnection() {
    this._hubConnectionGame = new HubConnectionBuilder()
      .withUrl('/hubs/lifeGameHub')
      .build();
  }

  /* -----------------------------------------------------------
   *    Server Events
  */
  private registerOnServerEvents(){
    this._hubConnectionGame.on('ReceiveConnectionId', (connectionId) => {
      this._connectionId = connectionId;
    });

    this._hubConnectionGame.on('ReceiveGeneration', (genDto: IGenerationOfCellsDto) => {
      this.generationOfCellsSubject.next(this.genDtoToGenCells(genDto));
    });

    this._hubConnectionGame.on('ReceiveGameOver', () => {
      this.gameStateSubject.next(this.Game.gameOver);
      this.alertSubject.next({ header: 'Juego de la Vida.' , message: 'Game over.' });
    });
  }

  /* -----------------------------------------------------------
      private methods
  */
  private genDtoToGenCells(genDto: IGenerationOfCellsDto): IGenerationOfCells {
    const cells: number[][] = [];

    for ( let f = 0; f < this.MAX_X; f++ ) {
      cells[f] = [];
    }

    for ( let x = 0; x < this.MAX_X; x++ ) {
      for ( let y = 0; y < this.MAX_Y; y++ ) {
        cells[x][y] = (genDto.liveCellList.some(c => c.x === x + 1  && c.y === y + 1 )) ? 1 : 0;
      }
    }
    return { cellList: cells, genNumber:  genDto.genNumber };
  }

  /* -----------------------------------------------------------
      public methods
  */
  public  startConnection(){
    if (this._connectionId === ''){
      this._hubConnectionGame.start()
      .then ()
      .catch(err => {
          this.alertSubject.next({
                header: 'Live Game Service' ,
                message: 'Error while establishing connection, retrying...' }
          );
          setTimeout(this.startConnection, 5000);
      });
    }
  }

  public stopConnetion(){
    if (this._connectionId !== ''){
       this._hubConnectionGame.stop();
       this._connectionId = '';
    }
  }

  public NewGame(){
    this.gameStateSubject.next(this.Game.withoutGeneration0);
    this.generationOfCellsSubject.next(this.emptyGeneration);
  }

  public getInitialGeneration(percentLiveCells: number){
    this.gameStateSubject.next(this.Game.withGeneration0);
    this._hubConnectionGame.invoke('SendInitialGeneration', percentLiveCells);
  }

  public StartGame() {
    this.gameStateSubject.next(this.Game.started);
    this.delayNextInvoke.subscribe(() => this._hubConnectionGame.invoke('SendNextGeneration'));
  }

  public StopGame() { this.gameStateSubject.next(this.Game.stopped); }

  public getConnectionId(): string { return this._connectionId; }

  /*
   *  Observables
  */
  getGenerationOfCells$(): Observable<IGenerationOfCells> {
    return this.generationOfCellsSubject.asObservable();
  }

  getGameState$(): Observable<GameState>{
    return this.gameStateSubject.asObservable();
  }

  getAlert$(): Observable<IAlert>{
    return this.alertSubject.asObservable();
  }
}
