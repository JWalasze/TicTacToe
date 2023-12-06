import { Time } from "@angular/common";

export interface HistoryItemDto {
  id: number;
  player1Username: string;
  player2Username: string;
  duration: string | null;
  isWon: boolean;
}
