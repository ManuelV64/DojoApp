using DojoApp.Core.Entities;
using System;
using System.Collections.Generic;
using System.Text;

namespace DojoApp.Core.Contracts
{
    public interface IGameOfLife
    {
        List<Coordinate2D> NextGeneration(List<Coordinate2D> liveCellCoordinateList);
    }
}
