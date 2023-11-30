import { HttpClient } from '@angular/common/http';
import { Injectable } from '@angular/core';
import { HistoryItem } from '../models/history-item';
import { Observable } from 'rxjs';
import { environment } from '../../../environment';
import { PlayerScore } from '../models/player-score';

@Injectable({
  providedIn: 'root'
})
export class RankingService {

  constructor(private http: HttpClient) { }

  getHistory(): Observable<HistoryItem[]> {
    return this.http.get<HistoryItem[]>(environment.apiUrl + "/Ranking/GetPlayerHistory?playerId=1&page=1&size=10");
  }

  getGlobalRanking(): Observable<PlayerScore[]> {
    return this.http.get<PlayerScore[]>(environment.apiUrl + "/Ranking/GetRanking?page=1&size=10");
  }

  getPlayerScore(): Observable<PlayerScore> {
    return this.http.get<PlayerScore>(environment.apiUrl + "/Ranking/GetPlayerScore?playerId=1");
  }
}
