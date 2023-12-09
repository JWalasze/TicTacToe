import { Component, OnDestroy, OnInit } from '@angular/core';
import { animate, state, style, transition, trigger } from '@angular/animations';
import { SignalRService } from '../services/hub.service';
import { Board, NextMove, Piece } from '../models/board';


@Component({
  selector: 'app-game-board',
  templateUrl: './game-board.component.html',
  styleUrls: ['./game-board.component.css'],
  animations: [
    //trigger('changeColor', [
    //  state('initial', style({
    //    backgroundColor: 'red'
    //  })),
    //  state('final', style({
    //    backgroundColor: 'green'
    //  })),
    //  transition('initial <=> final', animate('1000ms'))
    //])
  ]
})
export class GameBoardComponent implements OnInit, OnDestroy {

  playerId: number;
  playerUsername: string;
  playerPiece: Piece;

  opponentId: number;
  opponentUsername: string;
  opponentPiece: Piece;

  board: Board = {
    TicTacToeBoard: [[Piece.Empty, Piece.Empty, Piece.Empty],
    [Piece.Empty, Piece.Empty, Piece.Empty],
    [Piece.Empty, Piece.Empty, Piece.Empty]]
  };

  currentPieceTurn: NextMove; //Wyjebac w koncu to nextMove i dac cos rozsadnego

  colorState = 'initial';

  constructor(private signalRService: SignalRService) { }

  ngOnInit(): void {
    this.signalRService.startConnection();
    this.signalRService.observeChangingBoard().subscribe(([board, nextMove]) => {
      console.log("XDDDDDDDDDDDDDDDDDDDDDD");
      this.board = board;
    });
    this.signalRService.observeStartOfTheGame().subscribe((groupName) => {
      console.log("DXDDDDDDDDDDDDDDDDDDDDDD");
      console.log(groupName);
    });
  }

  ngOnDestroy(): void {
    this.signalRService.endConnection();
  }

  toggleColor() {
    this.colorState = (this.colorState === 'initial') ? 'final' : 'initial';
  }

  startGameSession(): void {
    this.signalRService.startConnection();
  }

  endGameSession(): void {
    this.signalRService.endConnection();
  }

  updateBoard(): void {
    //this.signalRService.updateBoard();
  }

  onMoveClick(row: number, col: number): void {
    if (this.board.TicTacToeBoard[row][col] == Piece.Empty) {
      this.updateLocalBoard(row, col);
      this.signalRService.updateBoard(this.board, NextMove.Circle);
    }

    this.board.TicTacToeBoard[row][col] = Piece.Circle;
    
  }

  private updateLocalBoard(row: number, col: number): void {
    this.board.TicTacToeBoard[row][col] = this.playerPiece;
  }

  exitGame(): void {
    console.log("body");
  }

  fun(row: number, col: number) {
    //if (1) {
    //  return 'green';
    //}
    //else {
    //  return 'blue';
    //}
  }

  private getCurrentConnectionState(): void {
    console.log(this.signalRService.getConnectionState());
  }

}
