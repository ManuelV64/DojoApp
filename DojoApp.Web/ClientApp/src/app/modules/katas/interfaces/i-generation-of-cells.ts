import { ICoordinate2D } from './i-coordinate2-d';

/*
 * the values for each cell can be:
 *   1 - live cell
 *   0 - dead cell
*/
export interface IGenerationOfCells {
  cellList: number[][];
  genNumber: number;
}

/*
 *  lisveCellList contain coordinates of live cells
*/
export interface IGenerationOfCellsDto {
  liveCellList: ICoordinate2D[];
  genNumber: number;
}
