using DojoApp.Core.Entities;
using DojoApp.Core.Katas;
using System;
using System.Collections.Generic;
using System.Text;
using Xunit;

namespace DojoApp.Test
{
    public class GameOfLifeTest
    {
        [Fact]
        public void AnyLiveCellWithFewerThanTwoLiveCellsNeighbor_Dies()
        {
            GameOfLife game = new GameOfLife();

            List<Coordinate2D> liveCellCoordinateList = new List<Coordinate2D>();
            liveCellCoordinateList.Add(new Coordinate2D(2, 2));

            var newLiveCellCoordinateLis = game.NextGeneration(liveCellCoordinateList);

            Assert.Null(newLiveCellCoordinateLis.Find(c => c.Equals(2, 2)));
        }
    }
}
