import { Component, OnInit ,OnDestroy , ViewChild} from '@angular/core';
import { HubConnection, HubConnectionBuilder } from '@aspnet/signalr';
import { AlertComponent} from "../alert/alert.component";

@Component({
  selector: 'app-game-of-life',
  templateUrl: './game-of-life.component.html',
  styleUrls: ['./game-of-life.component.css']
})

export class GameOfLifeComponent implements OnInit ,OnDestroy {
  @ViewChild(AlertComponent) alert: AlertComponent;

  private connectionIsEstablished = false;  
  private _hubConnectionGame : HubConnection;  
  private _hubConnectionStopGame: HubConnection;  

  private MAX_X=20;
  private MAX_Y=20;

  imgLiveCell= "../../../assets/LiveCell6.png";
  imgDiedCell= "../../../assets/DiedCell6.png";

  public board: number[][]=[];
  public coordinateLiveCellList: Coordinate2D[]=[];

  private playerId         : string="";
  private generationNumber : number=0;

  private game=Game;
  private stateGame :Game;
  private percentLiveCels  : number =0;

  constructor() { 
    for(var x = 0; x<this.MAX_X; x++) {
      this.board[x]=[];
    }

    this.ClearBoard();
    this.stateGame=Game.withoutGeneration0;
  }

  ngOnInit() {
    this.createConnection();
    this.registerOnServerEvents();  

    this.startConnection(this._hubConnectionGame);
    this.startConnection(this._hubConnectionStopGame);

  }
  
  ngOnDestroy() { 
    if (this.stateGame=Game.started) {
      this.StopGame();
    }

    this.stopConnetions();
  }


  //------------------------------------------------------------
  //  Create Connetions
  //
  private createConnection() {
    this._hubConnectionGame = new HubConnectionBuilder()  
      .withUrl('/hubs/gameOfLifeHub')  
      .build();     
    this._hubConnectionStopGame = new HubConnectionBuilder()  
      .withUrl('/hubs/gameOfLifeHub')  
      .build();  
  }

  //-----------------------------------------------------------
  //    Server Events
  //
  private registerOnServerEvents() {
    this._hubConnectionGame.on("ReceiveInitialGeneration", (coordinateLiveCellList: Coordinate2D[]) => {
      this.coordinateLiveCellList=coordinateLiveCellList;
      this.RefreshBoard();
      this.stateGame=Game.withGeneration0;
    });

    this._hubConnectionGame.on("ReceivePlayerId", (playerId) => {
      this.playerId=playerId;
    });

    this._hubConnectionGame.on("ReceiveNextGeneration", (coordinateLiveCellList: Coordinate2D[]) => {
      this.coordinateLiveCellList=coordinateLiveCellList;
      this.RefreshBoard();
      this.generationNumber++;
    });

    this._hubConnectionGame.on("ReceiveGameStopped", (message) => {
      this.alert.success(message);
      this.stateGame=Game.stopped;
    });

    this._hubConnectionGame.on("ReceiveGameOver", (message) => {
      this.alert.danger(message);
      this.stateGame=Game.gameOver;
    });
  }

  //-----------------------------------------------------------
  //    Sart Connnection
  //
  private startConnection(hubConnection: HubConnection) {
    hubConnection  
      .start()  
      .then(() => {  
        this.connectionIsEstablished = true;   
      })  
      .catch(err => { 
        this.alert.danger('Error while establishing connection, retrying...');
        setTimeout(this.startConnection(hubConnection), 5000);  
      });  
  }

  //-----------------------------------------------------------
  //    Stop Connnection
  //
  private stopConnetions(){
    this._hubConnectionStopGame.stop();
    this._hubConnectionGame.stop();
  }

  //-----------------------------------------------------------
  //    Calculate initial generation
  //
  private CalculateInitialGeneration(percentLiveCels:number){
    this._hubConnectionGame.invoke("SendInitialGeneration" ,percentLiveCels);
    this.generationNumber=0;
  }

  private StartGame(){
    this._hubConnectionGame.invoke("SendStartGame" ,this.coordinateLiveCellList);
    this.stateGame=Game.started;
    this.alert.hide();  
  }

  private StopGame(){
    this._hubConnectionStopGame.invoke("SendStopGame" ,this.playerId);
    this.playerId="";
  }

  private NewGame(){
    this.ClearBoard();
    this.stateGame=Game.withoutGeneration0;
    this.generationNumber=0;
    this.alert.hide(); 
  }

  private ClearBoard(){
    for(var x = 0; x<this.MAX_X; x++) {
      for(var y = 0; y<this.MAX_Y; y++) {
        this.board[x][y]=0;
      }
    }    
  }

  RefreshBoard(){
    this.ClearBoard();

    this.coordinateLiveCellList.forEach(c => { 
      this.board[c.x-1][c.y-1]=1;
    });
  }  
}

//===========================================================
//    enum Game
//===========================================================
enum Game {withoutGeneration0,withGeneration0,started,stopped,gameOver}

//===========================================================
//    Class Coordinate2D
//===========================================================
class Coordinate2D {
  constructor(x: number ,y: number) {
    this.x=x;
    this.y=y;
  }

  x: number;
  y: number;
}