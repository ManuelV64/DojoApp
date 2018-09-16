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

        [Fact]
        public void AnyLiveCellWithMoreThanThreeLiveNeighbours_Dies()
        {
            GameOfLife game = new GameOfLife();

            List<Coordinate2D> liveCellCoordinateList = new List<Coordinate2D>();
            liveCellCoordinateList.Add(new Coordinate2D(2, 2));
            liveCellCoordinateList.Add(new Coordinate2D(1, 1));
            liveCellCoordinateList.Add(new Coordinate2D(1, 3));
            liveCellCoordinateList.Add(new Coordinate2D(3, 2));
            liveCellCoordinateList.Add(new Coordinate2D(3, 3));

            var newLiveCellCoordinateLis = game.NextGeneration(liveCellCoordinateList);

            Assert.Null(newLiveCellCoordinateLis.Find(c => c.Equals(2, 2)));
        }

        [Fact]
        public void AnyLiveCellWithTwoOrTreeLiveNeighbours_LivesOnToTheNextGeneration()
        {
            GameOfLife game = new GameOfLife();

            List<Coordinate2D> liveCellCoordinateList = new List<Coordinate2D>();
            liveCellCoordinateList.Add(new Coordinate2D(2, 2));
            liveCellCoordinateList.Add(new Coordinate2D(1, 1));
            liveCellCoordinateList.Add(new Coordinate2D(1, 3));

            var newLiveCellCoordinateLis = game.NextGeneration(liveCellCoordinateList);

            Assert.NotNull(newLiveCellCoordinateLis.Find(c => c.Equals(2, 2)));
        }

        [Fact]
        public void AnyDeadCellWithExactlyThreeLiveNeighbours_Becomes_a_LiveCell()
        {
            GameOfLife game = new GameOfLife();

            List<Coordinate2D> liveCellCoordinateList = new List<Coordinate2D>();
            liveCellCoordinateList.Add(new Coordinate2D(1, 1));
            liveCellCoordinateList.Add(new Coordinate2D(2, 2));
            liveCellCoordinateList.Add(new Coordinate2D(2, 3));

            var newLiveCellCoordinateLis = game.NextGeneration(liveCellCoordinateList);

            Assert.NotNull(newLiveCellCoordinateLis.Find(c => c.Equals(1, 2)));
        }

        /// <summary>
        /// Any Live Cell With Fewer Than Two Live Cells Neighbor Dies
        /// </summary>
        /// <param name="liveCellCoordinateList"></param>
        /// <param name="coordinateLiveCell"></param>
        [Theory]
        [MemberData(nameof(Data_3))]
        public void AnyLiveCellWithFewerThanTwoLiveCellsNeighbor_Dies_3(
            List<Coordinate2D> liveCellCoordinateList,
            Coordinate2D coordinateLiveCell)
        {
            GameOfLife game = new GameOfLife();

            var newLiveCellCoordinateLis = game.NextGeneration(liveCellCoordinateList);

            Assert.Null(newLiveCellCoordinateLis.Find(co => co.Equals(coordinateLiveCell)));
        }

        public static IEnumerable<object[]> Data_3 =>
        new List<object[]>
        {
            new object[] { new List<Coordinate2D>() { new Coordinate2D(2, 2) }
                            , new Coordinate2D(2, 2)},
            new object[] { new List<Coordinate2D>() { new Coordinate2D(2, 2) ,
                                                        new Coordinate2D(2, 3)}
                            , new Coordinate2D(2, 2)},
        };

        /// <summary>
        /// Any Live Cell With More Than Three Live Neighbours Dies
        /// </summary>
        /// <param name="liveCellCoordinateList"></param>
        /// <param name="coordinateLiveCell"></param>
        [Theory]
        [MemberData(nameof(Data_4))]
        public void AnyLiveCellWithMoreThanThreeLiveNeighbours_Dies_4(
            List<Coordinate2D> liveCellCoordinateList,
            Coordinate2D coordinateLiveCell)
        {
            GameOfLife game = new GameOfLife();

            var newLiveCellCoordinateLis = game.NextGeneration(liveCellCoordinateList);

            Assert.Null(newLiveCellCoordinateLis.Find(co => co.Equals(coordinateLiveCell)));
        }

        public static IEnumerable<object[]> Data_4 =>
        new List<object[]>
        {
            new object[] { new List<Coordinate2D>() { new Coordinate2D(1, 1) ,
                                                        new Coordinate2D(1, 3) ,
                                                        new Coordinate2D(2, 2) ,
                                                        new Coordinate2D(3, 2) ,
                                                        new Coordinate2D(3, 3) }
                            , new Coordinate2D(2, 2)},
        };

        /// <summary>
        /// Any Live Cell With Two Or Tree Live Neighbours Lives On To The NextGeneration
        /// </summary>
        /// <param name="liveCellCoordinateList"></param>
        /// <param name="coordinateLiveCell"></param>
        [Theory]
        [MemberData(nameof(Data_5))]
        public void AnyLiveCellWithTwoOrTreeLiveNeighbours_LivesOnToTheNextGeneration5(
            List<Coordinate2D> liveCellCoordinateList,
            Coordinate2D coordinateLiveCell)
        {
            GameOfLife game = new GameOfLife();

            var newLiveCellCoordinateLis = game.NextGeneration(liveCellCoordinateList);

            Assert.NotNull(newLiveCellCoordinateLis.Find(co => co.Equals(coordinateLiveCell)));
        }


        public static IEnumerable<object[]> Data_5 =>
        new List<object[]>
        {
            new object[] { new List<Coordinate2D>() { new Coordinate2D(1, 1) ,
                                                      new Coordinate2D(2, 2) ,
                                                      new Coordinate2D(2, 3) }
                            , new Coordinate2D(2, 2)},
            new object[] { new List<Coordinate2D>() { new Coordinate2D(1, 1) ,
                                                      new Coordinate2D(2, 2) ,
                                                      new Coordinate2D(2, 3) ,
                                                      new Coordinate2D(3, 3) }
                            , new Coordinate2D(2, 2)},

        };
        /// <summary>
        /// Any Dead Cell With Exactly Three Live Neighbours Becomes a LiveCell
        /// </summary>
        /// <param name="liveCellCoordinateList"></param>
        /// <param name="coordinateDeadCell"></param>
        [Theory]
        [MemberData(nameof(Data_6))]
        public void AnyDeadCellWithExactlyThreeLiveNeighbours_Becomes_a_LiveCell6(
            List<Coordinate2D> liveCellCoordinateList,
            Coordinate2D coordinateDeadCell)
        {
            GameOfLife game = new GameOfLife();

            var newLiveCellCoordinateLis = game.NextGeneration(liveCellCoordinateList);

            Assert.NotNull(newLiveCellCoordinateLis.Find(co => co.Equals(coordinateDeadCell)));
        }

        public static IEnumerable<object[]> Data_6 =>
        new List<object[]>
        {
            new object[] { new List<Coordinate2D>() { new Coordinate2D(1, 1) ,
                                                      new Coordinate2D(1, 3) ,
                                                      new Coordinate2D(2, 2) }
                            , new Coordinate2D(1, 2)},
        };
    }
}
