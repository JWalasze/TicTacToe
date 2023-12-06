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

  colorState = 'initial';

  clicked = false;

  board: Board = {
    TicTacToeBoard: [[Piece.Empty, Piece.Empty, Piece.Empty],
    [Piece.Empty, Piece.Empty, Piece.Empty],
    [Piece.Empty, Piece.Empty, Piece.Empty]]
  };

  whoIsNext: NextMove = NextMove.Circle;

  testCircle: NextMove = NextMove.Circle;

  constructor(private signalRService: SignalRService) { }

  ngOnInit(): void {
    this.signalRService.startConnection();
    this.signalRService.observeChangingBoard().subscribe(([board, nextMove]) => {
      console.log("XDDDDDDDDDDDDDDDDDDDDDD");
      this.whoIsNext = nextMove;
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
    this.whoIsNext = NextMove.Cross;
    console.log("KLIKNIĘTE");
    console.log(row + "," + col);
    this.board.TicTacToeBoard[row][col] = Piece.Circle;
    this.signalRService.updateBoard(this.board, this.whoIsNext);

    this.clicked = true;
  }

  exitGame(): void {

  }

  fun() {
    if (this.clicked) {
      return 'green';
    }
    else {
      return 'blue';
    }
  }

  private getCurrentConnectionState(): void {
    console.log(this.signalRService.getConnectionState());
  }

}
