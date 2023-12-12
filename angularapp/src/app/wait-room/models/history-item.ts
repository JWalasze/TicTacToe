export interface HistoryItem {
  id: number;
  player1Username: string;
  player2Username: string;
  startTime: Date;
  endTime: Date;
  winnerUsername?: string;
}
