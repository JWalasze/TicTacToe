import { Component, OnInit } from '@angular/core';
import { RankingService } from '../services/ranking.service';
import { PlayerScore } from '../models/player-score';

@Component({
  selector: 'app-my-score',
  templateUrl: './my-score.component.html',
  styleUrls: ['./my-score.component.css']
})
export class MyScoreComponent implements OnInit {

  playerId!: number;

  score$!: PlayerScore;

  constructor(private rankingService: RankingService) { }

  ngOnInit(): void {
    this.rankingService.getPlayerScore().subscribe(data => {
      this.score$ = data;
      console.log(this.score$);
    });
  } 
}
