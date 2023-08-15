using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.AccessControl;
using System.Text;
using System.Threading.Tasks;

namespace RobotNavigation
{
    public class BFS : SearchAlgorithm
    {
        private Environments _env;
        private Queue<State> _frontier;

        private int _searched;
        public BFS(Environments env) 
        {
            _env = env;
            _frontier = new Queue<State>();
            _searched = 0;
        }
        public override void SolveProblem()
        {
            _frontier.Enqueue(new State (null, null, _env.getInitial));

            State state = null;

            while (_frontier.Count > 0)
            {
                // Get and remove the first node in the frontier
                state = _frontier.Dequeue();

                _searched++;

                if (_env.isSolved(state.getPosition))
                {
                    break;
                }
                _env.getCellAt(state.getPosition).Visited = true;

                AddNodesToFrontier(_env.discoverMoveSet(state));
            }
            displaySolution(state, _searched);
        }

        public void AddNodesToFrontier(List<State> states)
        {
            foreach(State s in states)
            {
                    if (!_env.getCellAt(s.getPosition).Visited)
                    {
                        _frontier.Enqueue(s);
                    }               
            }
        }

    }
}
