using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotNavigation
{
    public class Position
    {
        private int _x, _y;
        public Position(int x, int y)
        {
            _x = x;
            _y = y;
        }
        public int X { get { return _x; } }
        public int Y { get { return _y; } }
        public void setPosition(int x, int y)
        {
            _x = x;
            _y = y;
        }
        public void setPosition(Position location)
        {
            _x = location.X;
            _y = location.Y;
        }
        public int[] getCoordinate()
        {
            return new int[] { _x, _y };
        }
    }
}
