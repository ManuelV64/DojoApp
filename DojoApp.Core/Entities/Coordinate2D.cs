using System.Collections.Generic;

namespace DojoApp.Core.Entities
{
    public class Coordinate2D
    {
        private int _x;
        private int _y;

        #region Properties
        public int X
        {
            get { return _x; }
            set { _x = value; }
        }

        public int Y
        {
            get { return _y; }
            set { _y = value; }
        }

        #endregion

        #region Constructor
        public Coordinate2D(int x, int y)
        {
            _x = x;
            _y = y;
        }
        #endregion

        public List<Coordinate2D> NeighboursList()
        {
            var Neighbours = new List<Coordinate2D>();

            Neighbours.Add(new Coordinate2D(_x - 1, _y - 1));
            Neighbours.Add(new Coordinate2D(_x - 1, _y));
            Neighbours.Add(new Coordinate2D(_x - 1, _y + 1));

            Neighbours.Add(new Coordinate2D(_x, _y - 1));
            Neighbours.Add(new Coordinate2D(_x, _y + 1));

            Neighbours.Add(new Coordinate2D(_x + 1, _y - 1));
            Neighbours.Add(new Coordinate2D(_x + 1, _y));
            Neighbours.Add(new Coordinate2D(_x + 1, _y + 1));

            return Neighbours;
        }

        public bool Equals(int x, int y)
            => (x == _x & y == _y);

        public bool Equals(Coordinate2D c)
            => Equals(c.X, c.Y);

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;

            return Equals(obj as Coordinate2D);
        }

        public static bool operator ==(Coordinate2D item1, Coordinate2D item2)
        {
            if (object.ReferenceEquals(item1, item2)) { return true; }
            if ((object)item1 == null || (object)item2 == null) { return false; }
            return (item1.X == item2.X & item1.Y == item2.Y);
        }

        public static bool operator !=(Coordinate2D item1, Coordinate2D item2)
            => !(item1 == item2);


        public override int GetHashCode()
            => $"({X}-{Y})".GetHashCode();

        public override string ToString()
            => $"({X}-{Y})";
    }
}
