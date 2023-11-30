export class HistoryItem {
  id: number;
  player1Username: string;
  player2Username: string;
  startTime: Date;
  endTime: Date;
  winnerUsername?: string;

  constructor(id: number, player1Username: string, player2Username: string, startTime: Date, endTime: Date, winnerusername: string) {
    this.id = id;
    this.player1Username = player1Username;
    this.player2Username = player2Username;
    this.startTime = startTime;
    this.endTime = endTime;
    this.winnerUsername = winnerusername;
  }
}
