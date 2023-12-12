import { Component, Input, OnInit } from '@angular/core';
import { RankingService } from '../services/ranking.service';
import { HistoryItemDto } from '../models/history-item-dto';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-my-history',
  templateUrl: './my-history.component.html'
})
export class MyHistoryComponent implements OnInit {

  @Input()
  playerId: number;

  @Input()
  username: string;

  @Input()
  page: number;

  @Input()
  size: number;

  history$: HistoryItemDto[];

  constructor(private rankingService: RankingService, private datePipe: DatePipe) { }

  ngOnInit(): void {
    this.rankingService.getHistory(this.playerId, this.page, this.size).subscribe(data => {
      this.history$ = [];
      data.map(item => {
        this.history$.push({
          id: item.id,
          player1Username: item.player1Username,
          player2Username: item.player2Username,
          duration: this.countDuration(item.endTime, item.startTime),
          status: this.selectGameStatus(item.winnerUsername)
        });
      });
    });
  }

  private countDuration(endTime: Date, startTime: Date): string {
    const duration = this.datePipe.transform(new Date(endTime).getTime() - new Date(startTime).getTime(), 'hh:mm:ss');
    if (duration == null) {
      return '-';
    }

    return duration;
  }

  private selectGameStatus(winnerUsername: string | undefined): string {
    if (winnerUsername == null) {
      return 'Draw';
    }

    if (winnerUsername == this.username) {
      return 'Won';
    }

    return 'Lost';
  }
}
