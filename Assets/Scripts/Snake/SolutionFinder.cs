using System;
using System.Collections.Generic;
using System.Linq;
using static WFC.Helper;

namespace SnakeGame
{
    public class SolutionFinder
    {
        private readonly Cell[] objectives;
        private readonly bool[][] grid;
        private List<Cell> visited;
        private readonly Cell startingPoint;
        private Cell currentDestination;


        public SolutionFinder(IEnumerable<(int x, int y)> objectives, (int x, int y) start, bool[][] grid)
        {
            this.startingPoint = new Cell(start.x, start.y);
            var objectiveList = new List<Cell>(); 
            foreach (var objective in objectives)
            {
                objectiveList.Add(new Cell(objective.x, objective.y, startingPoint));
            }

            this.objectives = objectiveList.ToArray();
            
            this.grid = grid;
            visited = new List<Cell>( );
            
            this.objectives.SetDestination(startingPoint);
            this.objectives.Sort();
        }

        public bool DoesSolutionExist()
        {
            if (!AreObjectivesWithinGrid()) return false;
            
            Cell[] auxObjectives = (Cell[])objectives.Clone();
            bool found = false;
            
            for (int i = 0; i < objectives.Length && !found; i++)
            {
                Init();
                
                found = AStar(objectives[i]);

                if(found) auxObjectives = objectives.Skip(i).ToArray();

                while (found && auxObjectives.Length > 0)
                {
                    auxObjectives.SetDestination(visited[^1]);
                    auxObjectives.Sort();
                    
                    found = AStar(auxObjectives[0]);
                    
                    if(found) auxObjectives = auxObjectives.Skip(i).ToArray();
                }
            }
            return found;
        }

        private void Init()
        {
            visited = new List<Cell> { startingPoint };
                
            objectives.SetDestination(startingPoint);
            objectives.Sort();
        }

        private bool AStar(Cell currentObjective)
        {
            Cell currentPosition = visited[^1];
            
            if (currentPosition.Equals(currentObjective)) return true; 
            
            List<Cell> validNeighbours = ValidateNeighbours(currentPosition.GetNeighbours());
            
            Cell[] options = validNeighbours.ToArray().SetDestination(currentObjective);
            options.Sort();
            
            var solved = false;
            
            while(options.Length > 0 && !solved){
                
                visited.Add(options[0]);
                solved = AStar(currentObjective);
                if(!solved)visited.Remove(options[0]);
                options = options.Skip(0).ToArray();
            }

            return solved;
        }

        private bool AreObjectivesWithinGrid()
        {
            bool objectivesAreWithinGrid= true;
            var width = grid.Length;
            var height = grid[0].Length;

          
            for (int i = 0; i < objectives.Length && objectivesAreWithinGrid; i++)
                objectivesAreWithinGrid = objectives[i].X >= 0 && 
                                          objectives[i].X < width && 
                                          objectives[i].Y >= 0 && 
                                          objectives[i].Y < height;
            return objectivesAreWithinGrid;

        }

        private bool IsCellWithinGrid(Cell cell)
        {
            return cell.X >= 0 && 
                   cell.X < grid.Length && 
                   cell.Y >= 0 && 
                   cell.Y < grid[0].Length;
        }

        private bool IsCellWall(Cell cell)
        {
            return grid[cell.X][cell.Y];
        }

        private bool IsVisited(Cell cell)
        {
            return visited.Contains(cell);
        }

        private List<Cell> ValidateNeighbours(List<Cell> neighbours)
        {
            return neighbours.Where(neighbour => IsCellWithinGrid(neighbour) && !IsCellWall(neighbour) && !IsVisited(neighbour)).ToList();
        }
    }

    public class Cell
    {
        public Cell(int x, int y)
        {
            this.X = x;
            this.Y = y;
            distance = -1;
        }
        
        public Cell(int x, int y, Cell destination)
        {
            this.X = x;
            this.Y = y;
            ManhattanDistance(destination);
        }
        public Cell(Cell cell)
        {
            X = cell.X;
            Y = cell.Y;
        }

        public int Distance=> distance;
        private int distance = -1;
        public int X { get; }
        public int Y { get; }

        public int ManhattanDistance(Cell destination)
        {
            this.distance = Math.Abs(X - destination.X) + Math.Abs(Y - destination.Y);
            return distance;
        }

        public List<Cell> GetNeighbours()
        {
            return new List<Cell>
            {
                new Cell (X + dx[UP], Y + dy[UP]),
                new Cell (X + dx[DOWN], Y + dy[DOWN]),
                new Cell (X+dx[LEFT], Y+dy[LEFT]),
                new Cell (X+dx[RIGHT], Y+dy[RIGHT])
            };
        }

        public bool Equals(Cell other)
        {
            return X == other.X && Y == other.Y;
        }
    }
}
