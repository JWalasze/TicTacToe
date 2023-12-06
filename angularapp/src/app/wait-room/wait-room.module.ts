import { NgModule } from '@angular/core';
import { CommonModule, DatePipe } from '@angular/common';
import { BeforeGameComponent } from './before-game/before-game.component';
import { RankingComponent } from './ranking/ranking.component';
import { MyScoreComponent } from './my-score/my-score.component';
import { MyHistoryComponent } from './my-history/my-history.component';
import { HttpClientModule } from '@angular/common/http';


@NgModule({
  declarations: [
    BeforeGameComponent,
    RankingComponent,
    MyScoreComponent,
    MyHistoryComponent
  ],
  imports: [
    CommonModule,
    HttpClientModule
  ],
  providers: [DatePipe],
})
export class WaitRoomModule { }
