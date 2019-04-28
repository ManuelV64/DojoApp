import { Injectable, Inject } from '@angular/core';
import { HttpClient, HttpParams } from '@angular/common/http';
import { Observable, Subject, BehaviorSubject } from 'rxjs';
import { IAlert } from 'src/app/interfaces/i-alert';
import { Alert } from 'src/app/models/alert';
import { GameState } from './life-game-s-r.service';
import { IGenerationOfCells, IGenerationOfCellsDto } from '../interfaces/i-generation-of-cells';
import { takeWhile, delay } from 'rxjs/operators';

@Injectable({
  providedIn: 'root'
})
export class LifeGameHttpService {
  private url              = `${this.baseUrl}LifeGame`;
  private MAX_X = 20;
  private MAX_Y = 20;
  private emptyGeneration: IGenerationOfCells = this.genDtoToGenCells({ liveCellList: [], genNumber: 0 });
  private Game = GameState;

  private alertSubject     = new Subject<IAlert>();
  private gameStateSubject         = new BehaviorSubject<GameState>(this.Game.withoutGeneration0);
  private generationOfCellsSubject = new BehaviorSubject<IGenerationOfCells>(this.emptyGeneration);
  private delayNextInvoke = this.generationOfCellsSubject
            .pipe(delay(500),
                  takeWhile(() => this.gameStateSubject.value === this.Game.started ));

  private gameId: number;


  /* -----------------------------------------------------------
   *    constructor
  */
  constructor(private http: HttpClient,
              @Inject('BASE_URL') private baseUrl: string) {
    this.NewGame();
  }


  /* -----------------------------------------------------------
      private methods
  */
  private newAlert(obj?: any) {
    const newAlert = new Alert(obj);
    if ( newAlert.header == null) {
      newAlert.header = 'Life Game.';
    }
    this.alertSubject.next(newAlert.AlertMessage());
  }

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


  /*
   *  http Observables
  */
  private getGameId$(): Observable<number> {
    return this.http.get<number>(`${this.url}/GameId`);
  }

  private getInitialGeneration$(percentLiveCells: number ): Observable<any> {
    return this.http.get<IGenerationOfCellsDto>(
      `${this.url}/InitialGeneration/${this.gameId},${percentLiveCells}`,
      { observe: 'response' }
    );
  }

  private getNextGeneration$(): Observable<any> {
    return this.http.get(`${this.url}/NextGeneration/${this.gameId}`, { observe: 'response' });
  }

  private deleteGame$(gameId: number): Observable<any> {
    return this.http.delete(`${this.url}/DeleteGame/${gameId}`, { observe: 'response' });
  }

  /* -----------------------------------------------------------
      public methods
  */
  public CreateGameId() {
    if (this.gameId > 0) {
      this.DeleteGameId();
    }

    this.getGameId$().subscribe(
      idGame => {this.gameId = idGame;},
      err    => this.newAlert(err)
    );
  }

  public DeleteGameId() {
    this.deleteGame$(this.gameId).subscribe(
      response => { if (response.ok)  { this.gameId = 0; }},
      err      => this.newAlert(err)
    );
  }

  public NewGame(){
    this.gameStateSubject.next(this.Game.withoutGeneration0);
    this.generationOfCellsSubject.next(this.emptyGeneration);
  }

  public getInitialGeneration(percentLiveCells: number) {
    this.getInitialGeneration$(percentLiveCells).subscribe(
      response => {
        this.gameStateSubject.next(this.Game.withGeneration0);
        this.generationOfCellsSubject.next(this.genDtoToGenCells(response.body)); },
      err      => this.newAlert(err)
    );
  }

  public StartGame() {
    this.gameStateSubject.next(this.Game.started);
    this.delayNextInvoke.subscribe(() => this.getNextGeneration());
  }

  public StopGame() { this.gameStateSubject.next(this.Game.stopped); }

  private getNextGeneration(){
    this.getNextGeneration$().subscribe(
      response => {
        if (response.body.gameOver) {
          this.gameStateSubject.next(this.Game.gameOver);
          this.newAlert({message: 'Game Over.'});
        } else {
          this.generationOfCellsSubject.next(this.genDtoToGenCells(response.body));
      }},
      err    =>  {
        this.gameStateSubject.next(this.Game.stopped);
        this.newAlert(err);
      }
    );
  }

  /*
   *  Observables
  */
  public getGenerationOfCells$(): Observable<IGenerationOfCells> {
    return this.generationOfCellsSubject.asObservable();
  }

  public getGameState$(): Observable<GameState>{
    return this.gameStateSubject.asObservable();
  }

  public getAlert$(): Observable<IAlert>{
    return this.alertSubject.asObservable();
  }


}
