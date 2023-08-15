using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotNavigation
{
    public class Bidirectional : SearchAlgorithm
    {
        private Environments _env;
        private bool[,] visited_r, visited_g;
        private Queue<State> _robotfrontier, _goalfrontier;

        private int _discovered;
        public Bidirectional(Environments env)
        {
            _env = env;
            _robotfrontier = new Queue<State>();
            _goalfrontier = new Queue<State>();
            _discovered = 0;
            visited_r = new bool[11, 5];
            visited_g = new bool[11, 5];
        }
        public override void SolveProblem()
        {
            for (int i = 0; i < 5; i++)
            {
                for (int j = 0; j < 11; j++)
                {
                    visited_g[j, i] = false;
                    visited_r[j, i] = false;
                }
            }
            _robotfrontier.Enqueue(new State(null, "start", _env.getInitial));
            _goalfrontier.Enqueue(new State(null, "goal", new Position(7, 0)));

            State robotstate = null;
            State goalstate = null;

            while (_robotfrontier.Count > 0 && _goalfrontier.Count > 0)
            {
                // Get and remove the first node in the frontier
                robotstate = _robotfrontier.Dequeue();
                goalstate = _goalfrontier.Dequeue();


                Console.WriteLine(robotstate.getMessage);
                Console.WriteLine($"{robotstate.getPosition.X}, {robotstate.getPosition.Y}");

                Console.WriteLine(goalstate.getMessage);
                Console.WriteLine($"{goalstate.getPosition.X}, {goalstate.getPosition.Y}");

                visited_r[robotstate.getPosition.X, robotstate.getPosition.Y] = true;
                visited_g[goalstate.getPosition.X, goalstate.getPosition.Y] = true;



                if (robotstate.getPosition.X == goalstate.getPosition.X && robotstate.getPosition.Y == goalstate.getPosition.Y)
                {
                    break;
                }

                AddNodesToRobotFrontier(_env.discoverMoveSet(robotstate));
                AddNodesToGoalFrontier(_env.discoverMoveSet(goalstate));
            }
            displaySolution(robotstate, _discovered);
            displaySolution(goalstate, _discovered);

        }

        public void AddNodesToRobotFrontier(List<State> states)
        {
            foreach (State s in states)
            {
                if (!visited_r[s.getPosition.X, s.getPosition.Y])
                {
                    _robotfrontier.Enqueue(s);
                    _discovered++;
                }
            }
        }
        public void AddNodesToGoalFrontier(List<State> states)
        {
            foreach (State s in states)
            {
                if (!visited_g[s.getPosition.X, s.getPosition.Y])
                {
                    _goalfrontier.Enqueue(s);
                    _discovered++;
                }
            }
        }
    }
}
