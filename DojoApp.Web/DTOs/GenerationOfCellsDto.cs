using DojoApp.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DojoApp.Web.DTOs
{
    public class GenerationOfCellsDto
    {
        public List<Coordinate2D> liveCellList { get; set; }
        public int genNumber { get; set; }
    }
}
