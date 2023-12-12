import { Component, Input, OnInit } from '@angular/core';
import { RankingService } from '../services/ranking.service';
import { PlayerScore } from '../models/player-score';

@Component({
  selector: 'app-ranking',
  templateUrl: './ranking.component.html'
})
export class RankingComponent implements OnInit {

  @Input()
  page: number;

  @Input()
  size: number;

  ranking$: PlayerScore[];

  constructor(private rankingService: RankingService) { }

  ngOnInit(): void {
    this.rankingService.getGlobalRanking(this.page, this.size).subscribe(data => {
      this.ranking$ = data;
    });
  }
}
