// Modules
import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule, ReactiveFormsModule } from '@angular/forms';
import { KatasRoutingModule } from './katas-routing.module';

// Services
import { LifeGameSRService } from './services/life-game-s-r.service';

// Components
import { GameBoardComponent } from './components/game-board/game-board.component';
import { GameOfLifeComponent } from './components/game-of-life/game-of-life.component';



@NgModule({
  declarations: [GameBoardComponent, GameOfLifeComponent],
  imports: [
    CommonModule,
    FormsModule,
    ReactiveFormsModule,
    KatasRoutingModule
  ],
  providers: [LifeGameSRService]
})
export class KatasModule { }
