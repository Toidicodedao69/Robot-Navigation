  using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotNavigation
{
    public class Cell
    { 
        private bool _visited;
        private string _type; // "Path", "Goal", "Wall"

        public Cell (string type)
        {
            _visited = false;
            _type = type; // Once type is initialized, it can't be changed
        }
        public bool Visited 
        { 
            get { return _visited; }
            set { _visited = value; }
        }
        public string Type { get { return _type; } }
    }
}
