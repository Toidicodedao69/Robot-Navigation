using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotNavigation
{
    public class GBFS : SearchAlgorithm
    {
        private Environments _env;
        private PriorityQueue<State, int> _frontier;

        private int _searched;
        public GBFS(Environments env) 
        {
            _env = env;
            _frontier = new PriorityQueue<State, int>();
            _searched = 0;
        }

        public override void SolveProblem()
        {
            _frontier.Enqueue(new State(null, null, _env.getInitial), 0);

            Position goal = _env.findNearestGoal(); // This is the nearest goal to the initial position of the robot 
            State state = null;

            while (_frontier.Count > 0)
            {
                // Get and remove the node with highest priority (lowest cost) in the frontier
                state = _frontier.Dequeue();

                _searched++;

                if (_env.isSolved(state.getPosition))
                {
                    break;
                }
                _env.getCellAt(state.getPosition).Visited = true;

                AddNodesToFrontier(_env.discoverMoveSet(state, goal));
            }
            displaySolution(state, _searched);

        }
        public void AddNodesToFrontier(List<State> states)
        {
            foreach (State s in states)
            {
                if (!_env.getCellAt(s.getPosition).Visited)
                {
                    _frontier.Enqueue(s, s.getCost);
                }
            }
        }
    }
}
