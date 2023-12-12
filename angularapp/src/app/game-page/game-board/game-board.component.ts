import { Component, HostListener, OnDestroy, OnInit } from '@angular/core';
import { animate, state, style, transition, trigger } from '@angular/animations';
import { SignalRService } from '../services/hub.service';
import { Board, Piece, Tile } from '../models/board';
import { map } from 'rxjs';
import { ActivatedRoute } from '@angular/router';


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
    TicTacToeBoard: [[Tile.Empty, Tile.Empty, Tile.Empty],
    [Tile.Empty, Tile.Empty, Tile.Empty],
    [Tile.Empty, Tile.Empty, Tile.Empty]]
  };

  currentPieceTurn: Piece = Piece.Circle;

  tiles = {
    circle: Tile.Circle,
    cross: Tile.Cross,
    empty: Tile.Empty
  };

  colorState = 'initial';

  @HostListener('mouseenter')
  mouseenter() {
    if (this.currentPieceTurn == Piece.Circle) {
      console.log("OMG It's a Mouse!!!");
    }
  }

  @HostListener('mouseover')
  mouseover() {
    if (this.currentPieceTurn == Piece.Circle) {
      console.log("OMG It's still here!!!");
    }
  }

  @HostListener('mouseout')
  mouseout() {
    if (this.currentPieceTurn == Piece.Circle) {
      console.log('Phew thank god it left!')
    }
  }

  constructor(private signalRService: SignalRService, public activatedRoute: ActivatedRoute) { }

  ngOnInit(): void {
    
    this.activatedRoute.paramMap
      .pipe(map(() => window.history.state)).subscribe(data => {
        console.log(data);
      });
    
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
    if (this.board.TicTacToeBoard[row][col] == Tile.Empty) {
      this.updateLocalBoard(row, col);
      this.signalRService.updateBoard(this.board, Piece.Circle);
      this.currentPieceTurn = Piece.Cross;
    }
  }

  private updateLocalBoard(row: number, col: number): void {
    if (this.playerPiece == Piece.Circle) {
      this.board.TicTacToeBoard[row][col] = Tile.Circle;
    }
    else {
      this.board.TicTacToeBoard[row][col] = Tile.Cross;
    }
    console.log(this.board.TicTacToeBoard[row][col]);
  }

  exitGame(): void {
    console.log("body");
  }

  fun(row: number, col: number) {
    if (this.board.TicTacToeBoard[row][col] != Tile.Empty) {
      return '#f2f2f2';
    }

    return '';
  }

  private getCurrentConnectionState(): void {
    console.log(this.signalRService.getConnectionState());
  }

}
