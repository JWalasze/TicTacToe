export interface Board {
  TicTacToeBoard: Piece[][];
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
