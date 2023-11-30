// app-routing.module.ts

import { NgModule } from '@angular/core';
import { RouterModule, Routes } from '@angular/router';

import { BeforeGameComponent } from './wait-room/before-game/before-game.component';
import { LoginComponent } from './auth-page/login/login.component';
import { GameBoardComponent } from './game-page/game-board/game-board.component';
import { CreateAccountComponent } from './auth-page/create-account/create-account.component';

const routes: Routes = [
  { path: 'auth', component: LoginComponent },
  { path: 'wait_room', component: BeforeGameComponent },
  { path: 'game', component: GameBoardComponent },
  { path: 'create_profile', component: CreateAccountComponent}
];

@NgModule({
  imports: [RouterModule.forRoot(routes)],
  exports: [RouterModule],
})
export class AppRoutingModule { }
