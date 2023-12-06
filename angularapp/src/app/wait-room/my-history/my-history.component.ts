import { Component, OnInit } from '@angular/core';
import { HistoryItem } from '../models/history-item';
import { RankingService } from '../services/ranking.service';
import { HistoryItemDto } from '../models/history-item-dto';
import { DatePipe } from '@angular/common';

@Component({
  selector: 'app-my-history',
  templateUrl: './my-history.component.html',
  styleUrls: ['./my-history.component.css']
})
export class MyHistoryComponent implements OnInit {
  playerId!: string;

  history$: HistoryItemDto[] = [];
  constructor(private rankingService: RankingService, private datePipe: DatePipe) { }

  ngOnInit(): void {
    this.rankingService.getHistory().subscribe(data => {
      //console.log(data[0].endTime.getTime() - data[0].startTime.getTime());
      data.map(item => {
        //console.log(new Date(item.endTime).getTime());
        //console.log(item.startTime);
        //console.log(new Date(item.startTime).getTime());
        this.history$.push({
          id: item.id,
          player1Username: item.player1Username,
          player2Username: item.player2Username,
          //duration: new Date(),
          duration: this.datePipe.transform(new Date(item.endTime).getTime() - new Date(item.startTime).getTime(), 'hh:mm:ss'),
          isWon: this.playerId == item.winnerUsername
        });
      });
      console.log(data);
    });
  }
}
