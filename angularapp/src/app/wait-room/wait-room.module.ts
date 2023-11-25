import { NgModule } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BeforeGameComponent } from './before-game/before-game.component';
import { RankingComponent } from './ranking/ranking.component';



@NgModule({
  declarations: [
    BeforeGameComponent,
    RankingComponent
  ],
  imports: [
    CommonModule
  ]
})
export class WaitRoomModule { }
