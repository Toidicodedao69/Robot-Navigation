using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace RobotNavigation
{
    public abstract class SearchAlgorithm
    {
        public abstract void SolveProblem();

        public void displaySolution(State state, int searched)
        {
            Stack<string> stack = new Stack<string>();

            while (state != null && state.getMessage != null)
            {
                stack.Push(state.getMessage);
                state = state.getParent;
            }

            Console.WriteLine("Searched: " + searched);

            while (stack.Count > 0)
            {
                Console.Write(stack.Pop() + " ");
            }
        }
    }
}
