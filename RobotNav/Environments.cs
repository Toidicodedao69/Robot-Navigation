using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotNavigation
{
    public class Environments
    {
        private Cell[,] _map;
        private Position _initial; // The start position of the robot
        private List<Position> _goal; // All positions of goal cells

        private int _eastBorder, _southBorder;
        public Environments()
        {
            _goal = new List<Position>();
        }
        private void FillGrids(Cell[,] map, int x, int y, int column, int row, string type)
        {
            for (int i = y; i < y + row; ++i)
            {
                for (int j = x; j < x + column; ++j)
                {
                    map[j, i] = new Cell(type);
                }
            }
        }
        public void SetMapUnvisited()
        {
            for (int i = 0; i < _southBorder + 1; i++)
            {
                for (int j = 0; j < _eastBorder + 1; j++)
                {
                    _map[j, i].Visited = false;
                }
            }
        }
        public void ReadFile(string FileName)
        {
            FileStream stream = new FileStream(FileName, FileMode.Open);
            StreamReader reader = new StreamReader(stream);

            try
            {
                string MapDimensions = reader.ReadLine();

                // The dimension is in format [W,H]
                MapDimensions = MapDimensions.Trim(new char[] { '[', ']' });
                string[] BothDimension = MapDimensions.Split(",");

                int row = Convert.ToInt32(BothDimension[0]);
                int column = Convert.ToInt32(BothDimension[1]);

                // Read in the position of the robot
                string location = reader.ReadLine();
                location = location.Trim(new char[] { '(', ')' });

                string[] coords = location.Split(",");
                int x = Convert.ToInt32(coords[0]);
                int y = Convert.ToInt32(coords[1]);

                _initial = new Position(x, y); 

                // The map is made of a 2 dimensional array of Cell
                _map = new Cell[column, row];
                FillGrids(_map, 0, 0, column, row, "Path");

                // Create goals in the map
                string goal = reader.ReadLine();
                string[] GoalGrid = goal.Split(" | ");
                for (int i = 0; i < GoalGrid.Length; ++i)
                {
                    GoalGrid[i] = GoalGrid[i].Trim(new char[] { '(', ')' });
                    string[] contents = GoalGrid[i].Split(",");
                    _goal.Add(new Position(Convert.ToInt32(contents[0]), Convert.ToInt32(contents[1])));
                    _map[Convert.ToInt32(contents[0]), Convert.ToInt32(contents[1])] = new Cell("Goal");
                }

                // Create walls in the map
                string line;
                while ((line = reader.ReadLine()) != null)
                {
                    line = line.Trim(new char[] { '(', ')' });
                    string[] wall = line.Split(",");
                    FillGrids(_map, Convert.ToInt32(wall[0]), Convert.ToInt32(wall[1]), Convert.ToInt32(wall[2]), Convert.ToInt32(wall[3]), "Wall");
                }
                reader.Close();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                Environment.Exit(1);
            }
            _eastBorder = _map.GetUpperBound(0);
            _southBorder = _map.GetUpperBound(1);
        }

        // Check to see if the robot arrives at goal grid
        public bool isSolved(Position pos)  
        {
            return _map[pos.X, pos.Y].Type.Equals("Goal");
        }
        public List<State> discoverMoveSet (State state)
        {
            List<State> moves = new List<State> ();
            int currentX = state.getPosition.X; // current X coordinate of the state
            int currentY = state.getPosition.Y; // current Y coordinate of the state

            // 2 conditions to check:
            // - The the current position is at border of the map
            // - The the next position is not a wall      

            // When all else is equal, the robot should move UP -> LEFT -> DOWN -> RIGHT
     
            if (currentY > 0 && (_map[currentX, currentY - 1].Type != "Wall"))
            {
                moves.Add(new State(state, "up; ", new Position(currentX, currentY - 1)));        // Move Up
            }
            if (currentX > 0 && (_map[currentX - 1, currentY].Type != "Wall"))
            {
                moves.Add(new State(state, "left; ", new Position(currentX - 1, currentY)));        // Move Left
            }
            if (currentY < _southBorder && (_map[currentX, currentY + 1].Type != "Wall"))
            {
                moves.Add(new State(state, "down; ", new Position(currentX, currentY + 1)));        // Move Down
            }
            if (currentX < _eastBorder && (_map[currentX + 1, currentY].Type != "Wall"))
            {
                moves.Add(new State(state, "right; ", new Position(currentX + 1, currentY)));        // Move Right
            }

            return moves;
        }

        // Finding move set with costs
        public List<State> discoverMoveSet (State state, Position goal)
        {
            List<State> moves = new List<State>();
            int currentX = state.getPosition.X; // current X coordinate of the state
            int currentY = state.getPosition.Y; // current Y coordinate of the state

            if (currentY > 0 && (_map[currentX, currentY - 1].Type != "Wall"))
            {
                moves.Add(new State(state, "up; ", new Position(currentX, currentY - 1), calculateCost(new Position(currentX, currentY - 1), goal)));        // Move Up
            }
            if (currentX > 0 && (_map[currentX - 1, currentY].Type != "Wall"))
            {
                moves.Add(new State(state, "left; ", new Position(currentX - 1, currentY), calculateCost(new Position(currentX - 1, currentY), goal)));        // Move Left
            }
            if (currentY < _southBorder && (_map[currentX, currentY + 1].Type != "Wall"))
            {
                moves.Add(new State(state, "down; ", new Position(currentX, currentY + 1), calculateCost( new Position(currentX, currentY + 1), goal)));        // Move Down
            }
            if (currentX < _eastBorder && (_map[currentX + 1, currentY].Type != "Wall"))
            {
                moves.Add(new State(state, "right; ", new Position(currentX + 1, currentY), calculateCost( new Position(currentX + 1, currentY), goal)));        // Move Right
            }

            return moves;
        }
        public List<State> discoverMoveSet(State state, Position goal, int additioncost)
        {
            List<State> moves = new List<State>();
            int currentX = state.getPosition.X; // current X coordinate of the state
            int currentY = state.getPosition.Y; // current Y coordinate of the state

            if (currentY > 0 && (_map[currentX, currentY - 1].Type != "Wall"))
            {
                moves.Add(new State(state, "up; ", new Position(currentX, currentY - 1), calculateCost(new Position(currentX, currentY - 1), goal) + additioncost));        // Move Up
            }
            if (currentX > 0 && (_map[currentX - 1, currentY].Type != "Wall"))
            {
                moves.Add(new State(state, "left; ", new Position(currentX - 1, currentY), calculateCost(new Position(currentX - 1, currentY), goal) + additioncost));        // Move Left
            }
            if (currentY < _southBorder && (_map[currentX, currentY + 1].Type != "Wall"))
            {
                moves.Add(new State(state, "down; ", new Position(currentX, currentY + 1), calculateCost(new Position(currentX, currentY + 1), goal) + additioncost));        // Move Down
            }
            if (currentX < _eastBorder && (_map[currentX + 1, currentY].Type != "Wall"))
            {
                moves.Add(new State(state, "right; ", new Position(currentX + 1, currentY), calculateCost(new Position(currentX + 1, currentY), goal) + additioncost));        // Move Right
            }

            return moves;
        }
        public Cell getCellAt(Position pos)
        {
            return _map[pos.X, pos.Y];
        }
        // Calculates the Manhattan distance between current position and the goal
        public int calculateCost(Position current, Position goal)
        {
            return Math.Abs(current.X - goal.X) + Math.Abs(current.Y - goal.Y);  // Manhanttan distance
        }
        
        // This method will find the closest goal position to the initial position of the robot
        public Position findNearestGoal()
        {
            Position nearest = _goal[0];

            foreach(Position c in _goal)
            {
                if (calculateCost(_initial, c) < calculateCost(_initial, nearest))
                {
                    nearest = c;
                }
            }
            return nearest;
        }

        public Cell[,] getMap { get { return _map; } }
        public Position getInitial { get { return _initial; } }
    }
}
