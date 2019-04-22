import { NgModule } from '@angular/core';
import { Routes, RouterModule } from '@angular/router';
import { GameOfLifeComponent } from './components/game-of-life/game-of-life.component';

const routes: Routes = [
  { path: 'live-game'            , component: GameOfLifeComponent }
];

@NgModule({
  imports: [RouterModule.forChild(routes)],
  exports: [RouterModule]
})
export class KatasRoutingModule { }
