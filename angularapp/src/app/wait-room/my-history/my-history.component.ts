import { Component } from '@angular/core';
import { HistoryItem } from '../models/history-item';
import { RankingService } from '../services/ranking.service';

@Component({
  selector: 'app-my-history',
  templateUrl: './my-history.component.html',
  styleUrls: ['./my-history.component.css']
})
export class MyHistoryComponent {
  playerId!: number;

  history$!: HistoryItem[];
  constructor(private rankingService: RankingService) { }

  ngOnInit(): void {
    this.rankingService.getHistory().subscribe(data => {
      this.history$ = data;
      console.log(data);
    });
  }
}
