using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NonogramSolver
{
    class Program
    {
        static void Main(string[] args)
        {
            string line;

            System.IO.StreamReader file = new System.IO.StreamReader("picture.txt");

            int numOfRows = Convert.ToInt32(file.ReadLine());
            int numOfCols = Convert.ToInt32(file.ReadLine());

            List<Line> rows = new List<Line>();
            List<Line> columns = new List<Line>();

            for (int i = 0; i < numOfRows; i++)
            {
                line = file.ReadLine();
                List<int> clues = new List<int>(Array.ConvertAll(line.Split(';'), int.Parse));
                Line l = new Line(clues, numOfCols, LineType.row, i);
                rows.Add(l);

            }

            for (int i = 0; i < numOfCols; i++)
            {
                line = file.ReadLine();
                List<int> clues = new List<int>(Array.ConvertAll(line.Split(';'), int.Parse));
                Line l = new Line(clues, numOfRows, LineType.col, i);
                columns.Add(l);

            }

            Grid grid = new Grid(numOfRows, numOfCols, rows, columns);

            foreach (Line row in grid.rows)
            {
                ruleSolver(row.clues, row.length, row.possibleSolutions);
            }
            foreach (Line column in grid.columns)
            {
                ruleSolver(column.clues, column.length, column.possibleSolutions);
            }

            void ruleSolver(List<int> rules, int rowSize, List<List<bool>> possibilities)
            {
                List<bool> possibility = new List<bool>();
                List<int> ruleCounts = new List<int>(rules);
                for (int i = 0; i < rowSize; i++)
                {
                    possibility.Add(false);
                }
                collectPossibleLayouts(possibility, 0, ruleCounts, possibilities);
            }

            void collectPossibleLayouts(List<bool> possibility, int startingOffset, List<int> ruleCounts, List<List<bool>> possibilities)
            {
                if (ruleCounts.Count == 0)
                {
                    possibilities.Add(possibility);
                    return;
                }

                int firstCount = ruleCounts[0];
                List<int> remainingCounts = ruleCounts.GetRange(1, ruleCounts.Count - 1);
                for (int offset = startingOffset; offset <= possibility.Count - firstCount; offset++)
                {
                    List<bool> possibilityCopy = new List<bool>(possibility);

                    for (int i = 0; i < firstCount; i++)
                    {
                        possibilityCopy[offset + i] = true;

                    }

                    if (remainingCounts.Count == 0)
                    {
                        possibilities.Add(possibilityCopy);
                    }
                    else
                    {
                        int newStartingOffset = offset + firstCount + 1;
                        collectPossibleLayouts(possibilityCopy, newStartingOffset, remainingCounts, possibilities);
                    }
                }
            }

            List<Line> variables = new List<Line>();
            List<Line> assignment = new List<Line>();

            foreach (Line l in grid.rows)
            {
                variables.Add(l);
            }
            foreach (Line l in grid.columns)
            {
                variables.Add(l);
            }


            bool valid(LineType lt, int index, List<bool> possibleSolution, List<Line> solution)
            {
                for (int i = 0; i < solution.Count; i++)
                {
                    Line temp = solution[i];
                    if (temp.lineType != lt)
                    {

                        if (temp.currentSolution[index] != possibleSolution[temp.index])
                        {
                            return false;
                        }
                    }

               }
                return true;
            }

            bool backtrack(List<Line> vars, int level, List<Line> solution)
            {
                
                if (level == vars.Count)
                {
                    return true;
                }
                int currentIndex = vars[level].index;
             
                Console.WriteLine("level " + level);
                

                for (int i = 0; i < vars[level].possibleSolutions.Count; i++)
                {
                    
                    bool isValid = valid(vars[level].lineType, vars[level].index, vars[level].possibleSolutions[i], solution);
                    

                    if (!isValid)
                        continue;
                    
                    solution.Add(vars[level]);
                    solution[solution.Count - 1].currentSolution = vars[level].possibleSolutions[i];
                    bool recursiveCall = backtrack(vars, ++level, solution);
                    if(recursiveCall)
                    {
                        return true;
                    }
                    else
                    {

                        solution.RemoveAll(item => item.index == currentIndex);

                    }


                }


                return false;
            }

            backtrack(variables, 0, assignment);
            foreach(Line l in variables)
            {
                Console.WriteLine(l.index);
                Console.WriteLine(l.lineType);
                foreach(bool b in l.currentSolution)
                {
                    if (b)
                        Console.Write("#");
                    else
                    {
                        Console.Write("-");
                    }
                }
                Console.WriteLine();
            }


        }


    }
}
