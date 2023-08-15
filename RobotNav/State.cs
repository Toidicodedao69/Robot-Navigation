using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotNavigation
{
    public class State
    {
        // The position of the current state
        private Position _position;

        // Parent node
        private State _parent;
        private string _message;
        private int _cost;

        public State(State parent, string message, Position position)
        {
            _parent = parent;
            _message = message;
            _position = position;
        }

        public State(State parent, string message, Position position, int cost)
        {
            _position = position;
            _parent = parent;
            _message = message;
            _cost = cost;
        }

        public State getParent { get { return _parent; } }
        public Position getPosition { get { return _position; } }
        public string getMessage { get { return _message; } }
        public int getCost { get { return _cost; } }
    }
}
