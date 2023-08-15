using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotNavigation
{
    public class DFS : SearchAlgorithm
    {
        private Environments _env;
        private Stack<State> _frontier; // LIFO list
        private int _searched;
        public DFS(Environments env)
        {
            _env = env;
            _frontier = new Stack<State>();
            _searched = 0;
        }
        public override void SolveProblem()
        {
            _frontier.Push(new State(null, null, _env.getInitial));
            State state = null;
           

            while (_frontier.Count > 0)
            {
                // Get and remove the first node in the frontier             
                state = _frontier.Pop();

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
            states.Reverse();
            foreach (State s in states)
            {
                if (!_env.getCellAt(s.getPosition).Visited)
                {
                    _frontier.Push(s);
                }
                            
            }
        }

     }
    
}

