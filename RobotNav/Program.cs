using System.Data.Common;
using static System.Net.Mime.MediaTypeNames;

namespace RobotNavigation
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Environments env = new Environments();

            env.ReadFile(args[0]);
            Cell[,] map = env.getMap;

            // Check if the function reads the map correctly
            for (int i = 0; i < map.GetLength(1); i++)
            {
                for (int j = 0; j < map.GetLength(0); j++)
                {
                    Console.Write(map[j, i].Type + " ");
                }
                Console.WriteLine();
            }

            SearchAlgorithm search;
            switch (args[1].ToLower())
            {
                case "bfs":
                    search = new BFS(env);
                    search.SolveProblem();
                    break;
                case "dfs":
                    search = new DFS(env);
                    search.SolveProblem();
                    break;
                case "gbfs":
                    search = new GBFS(env);
                    search.SolveProblem();
                    break;
                case "astar":
                    search = new AStar(env);
                    search.SolveProblem();
                    break;
                case "bidi":
                    search = new Bidirectional(env);
                    search.SolveProblem();
                    break;
                case "ida":
                    search = new IDAStar(env);
                    search.SolveProblem();
                    break;
            }



        }
    }
}