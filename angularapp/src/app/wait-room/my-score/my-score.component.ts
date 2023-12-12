import { Component, Input, OnInit } from '@angular/core';
import { RankingService } from '../services/ranking.service';
import { PlayerScore } from '../models/player-score';

@Component({
  selector: 'app-my-score',
  templateUrl: './my-score.component.html'
})
export class MyScoreComponent implements OnInit {

  @Input()
  playerId: number;

  score$: PlayerScore;

  constructor(private rankingService: RankingService) { }

  ngOnInit(): void {
    this.rankingService.getPlayerScore(this.playerId).subscribe(data => {
      this.score$ = data;
    });
  } 
}
