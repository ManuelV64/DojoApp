import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { GameOfLifeComponent } from './components/game-of-life/game-of-life.component';
import { LifeGameComponent } from './components/life-game/life-game.component';

const routes: Routes = [
  { path: 'life-game-1'            , component: GameOfLifeComponent },
  { path: 'life-game-2'            , component: LifeGameComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class KatasRoutingModule { }
