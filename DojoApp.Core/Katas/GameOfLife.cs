using DojoApp.Core.Contracts;
using DojoApp.Core.Entities;
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
    }
}

