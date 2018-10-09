using DojoApp.Core.Contracts;
using DojoApp.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DojoApp.Core.Katas
{
    public class GameOfLife : IGameOfLife
    {
        const int MAX_X = 20;
        const int MAX_Y = 20;

        /// <summary>
        /// Actuliza la lista de coordenadas de celulas vivas (liveCellCoordinateList)
        /// </summary>
        /// <param name="liveCellCoordinateList"></param>
        public List<Coordinate2D> NextGeneration(List<Coordinate2D> liveCellCoordinateList)
        {
            List<Coordinate2D> newLiveCellCoordinateLis = new List<Coordinate2D>();
            List<Coordinate2D> AllNeighboursList = new List<Coordinate2D>();

            // cargamos una lista con las coordenadas de las celulas con vecinos vivos
            // una cordenada aparece tantas veces como vecinos vivos tenga.
            liveCellCoordinateList.ForEach(c =>
                AllNeighboursList.AddRange(c.NeighboursList())
            );

            // eliminamos coordenadas que no se encuentran en nuestro tablero
            AllNeighboursList.RemoveAll(c => c.X < 1 || c.Y < 1 || c.X > MAX_X || c.Y > MAX_Y);

            var NeighboursGroupedList = AllNeighboursList
                .GroupBy(x => x.GetHashCode())
                .Select(x => new { CellCoordinate = x.First(), Neighbours = x.Count() })
                .ToList();

            // Una celula viva o muerta con tres vecinos vive en la siguientes generación
            var n2 = NeighboursGroupedList
                        .Where(c => c.Neighbours == 3)
                        .Select(c => c.CellCoordinate);

            // celulas vivas con dos vecinos viven en la sigiente generación.
            var n3 = NeighboursGroupedList
                        .Where(c => liveCellCoordinateList
                                        .FirstOrDefault(x => x == c.CellCoordinate) != null
                                    && c.Neighbours == 2)
                        .Select(c => c.CellCoordinate);

            newLiveCellCoordinateLis.AddRange(n2);
            newLiveCellCoordinateLis.AddRange(n3);

            return newLiveCellCoordinateLis;
        }

        public List<Coordinate2D> RandomGeneration(int percent)
        {
            Random rnd = new Random();
            int element;
            List<Coordinate2D> TotalCellCoordinateList = new List<Coordinate2D>();
            List<Coordinate2D> FirstCellCoordinateList = new List<Coordinate2D>();

            int TotalCells =(int)(MAX_X * MAX_Y * percent / 100);

            for (int x = 1; x <= MAX_X; x++) {
                for (int y = 1; y <= MAX_Y; y++) {
                    TotalCellCoordinateList.Add(new Coordinate2D(x, y));
                }
            }

            for (int i = 0; i < TotalCells; i++)
            {
                element = rnd.Next(0, TotalCellCoordinateList.Count - 1);
                FirstCellCoordinateList.Add(TotalCellCoordinateList.ElementAt(element));
                TotalCellCoordinateList.RemoveAt(element);
            }

            return FirstCellCoordinateList;
        }
    }
}

