using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotNavigation
{
    public class IDAStar : SearchAlgorithm
    {
        private Environments _env;
        private Stack<State> _frontier;
        private PriorityQueue<State, int> _pruned; // A priority list to order the pruned nodes
        private int _fscore; // A threshold used to prune any node that has cost > _fscore

        private int _searched;
        public IDAStar(Environments env)
        {
            _env = env;
            _frontier = new Stack<State> { };
            _pruned = new PriorityQueue<State, int>();
            _searched = 0;
        }
        public override void SolveProblem()
        {
            Position goal = _env.findNearestGoal(); // This is the nearest goal to the initial position of the robot 
            State initial_state = new State(null, null, _env.getInitial, _env.calculateCost(_env.getInitial, goal));
            _frontier.Push(initial_state);
            _fscore = initial_state.getCost;

            State state = null;

            while (_frontier.Count > 0)
            {
                state = _frontier.Pop();

                _searched++;
                
                if (_env.isSolved(state.getPosition))
                {
                    break;
                }
                _env.getCellAt(state.getPosition).Visited = true;
                AddNodesToFrontier(_env.discoverMoveSet(state, goal, 1)); // The cost of moving from a node to an adjacent node (either up, left, down, right) is 1.

                if (_frontier.Count == 0)
                {
                    // Update the threshold to the minimum pruned value
                    _fscore = _pruned.Dequeue().getCost;
                    Console.WriteLine(_fscore);


                    // The search starts all over again with new threshold value
                    _frontier.Push(initial_state);

                    _env.SetMapUnvisited(); 
                }
            }
            displaySolution(state, _searched);
        }

        public void AddNodesToFrontier(List<State> states)
        {
            states.Reverse();
            foreach (State s in states)
            {
                if (s.getCost > _fscore)
                {
                    _pruned.Enqueue(s, s.getCost);
                }
                else if (!_env.getCellAt(s.getPosition).Visited)
                {
                    _frontier.Push(s);
                }
            }
        }
    }
}
