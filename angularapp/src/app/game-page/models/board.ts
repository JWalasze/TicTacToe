export interface Board {
  TicTacToeBoard: Tile[][];
}

export enum Tile {
  Cross = 'CROSS',
  Circle = 'CIRCLE',
  Empty = 'EMPTY'
}

export enum Piece {
  Cross = 'CROSS',
  Circle = 'CIRCLE'
}
