export class PlayerScore {
  id: number;
  username: string;
  wonGames: number;
  lostGames: number;
  drawGames: number;

  constructor(id: number, username: string, wonGames: number, lostGames: number, drawGames: number) {
    this.id = id;
    this.username = username;
    this.wonGames = wonGames;
    this.lostGames = lostGames;
    this.drawGames = drawGames;
  }
}
