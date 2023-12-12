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

  getHistory(playerId: number, page: number, size: number): Observable<HistoryItem[]> {
    return this.http.get<HistoryItem[]>(environment.apiUrl + environment.apiEndpoints.playerHistory
      + "?playerId=" + playerId
      + "&page=" + page
      + "&size=" + size);
  }

  getGlobalRanking(page: number, size: number): Observable<PlayerScore[]> {
    return this.http.get<PlayerScore[]>(environment.apiUrl + environment.apiEndpoints.globalRanking
      + "?page=" + page
      + "&size=" + size);
  }

  getPlayerScore(playerId: number): Observable<PlayerScore> {
    return this.http.get<PlayerScore>(environment.apiUrl + environment.apiEndpoints.playerScore
      + "?playerId=" + playerId);
  }
}
