import { Component } from '@angular/core';
import { RankingService } from '../services/ranking.service';
import { PlayerScore } from '../models/player-score';

@Component({
  selector: 'app-ranking',
  templateUrl: './ranking.component.html',
  styleUrls: ['./ranking.component.css']
})
export class RankingComponent {

  ranking$!: PlayerScore[];

  constructor(private rankingService: RankingService) { }

  ngOnInit(): void {
    this.rankingService.getGlobalRanking().subscribe(data => {
      console.log(data);
      this.ranking$ = data;
    });
  }
}
