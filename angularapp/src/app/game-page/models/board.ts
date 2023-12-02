export interface Board {
  board: Piece[][];

  //constructor() {
  //  this.board = [
  //    [Piece.Empty, Piece.Empty, Piece.Empty],
  //    [Piece.Empty, Piece.Empty, Piece.Empty],
  //    [Piece.Empty, Piece.Empty, Piece.Empty]
  //  ];
  //}
}

export enum Piece {
  Cross = 'CROSS',
  Circle = 'CIRCLE',
  Empty = 'EMPTY'
}

export enum NextMove {
  Cross = 'CROSS',
  Circle = 'CIRCLE'
}
