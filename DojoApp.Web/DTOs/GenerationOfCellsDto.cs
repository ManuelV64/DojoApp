using DojoApp.Core.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DojoApp.Web.DTOs
{
    public class GenerationOfCellsDto
    {
        public int genNumber { get; set; }
        public List<Coordinate2D> liveCellList { get; set; }
        public bool? gameOver { get; set; }
    }
}
