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

            System.IO.StreamReader file = new System.IO.StreamReader("picture2.txt");

            int numOfRows = Convert.ToInt32(file.ReadLine());
            int numOfCols = Convert.ToInt32(file.ReadLine());

            List<Line> rows = new List<Line>();
            List<Line> columns = new List<Line>();

            for (int i = 0; i < numOfRows; i++)
            {
                line = file.ReadLine();
                List<int> clues = new List<int>(Array.ConvertAll(line.Split(';'), int.Parse));
                Line l = new Line(clues, numOfCols,LineType.row,i);
                rows.Add(l);

            }

            for (int i = 0; i < numOfCols; i++)
            {
                line = file.ReadLine();
                List<int> clues = new List<int>(Array.ConvertAll(line.Split(';'), int.Parse));
                Line l = new Line(clues, numOfRows,LineType.col,i);
                columns.Add(l);

            }

            Grid grid = new Grid(numOfRows, numOfCols, rows, columns);
            
            foreach(Line row in grid.rows)
            {
                ruleSolver(row.clues, row.length, row.possibleSolutions);
            }
            foreach(Line column in grid.columns)
            {
                ruleSolver(column.clues, column.length, column.possibleSolutions);
            }

            void ruleSolver(List<int> rules,int rowSize, List<List<bool>> possibilities)
            {
                List<bool> possibility = new List<bool>();
                List<int> ruleCounts = new List<int>(rules);
                for(int i =0;i<rowSize;i++)
                {
                    possibility.Add(false);
                }
                collectPossibleLayouts(possibility, 0, ruleCounts, possibilities);
            }

            void collectPossibleLayouts(List<bool> possibility, int startingOffset, List<int> ruleCounts, List<List<bool>> possibilities)
            {
                if(ruleCounts.Count==0)
                {
                    possibilities.Add(possibility);
                    return;
                }

                int firstCount = ruleCounts[0];
                List<int> remainingCounts = ruleCounts.GetRange(1, ruleCounts.Count-1);
                for(int offset = startingOffset;offset<=possibility.Count-firstCount;offset++)
                {
                    List<bool> possibilityCopy = new List<bool>(possibility);

                    for(int i =0; i<firstCount;i++)
                    {
                        possibilityCopy[offset+i] = true;
                        
                    }

                    if(remainingCounts.Count ==0)
                    {
                        possibilities.Add(possibilityCopy);
                    }
                    else
                    {
                        int newStartingOffset = offset + firstCount + 1;
                        collectPossibleLayouts(possibilityCopy, newStartingOffset, remainingCounts,possibilities);
                    }
                }
            }

            List<Line> variables = new List<Line>();
            List<Line> assignment = new List<Line>();

            foreach(Line l in grid.rows)
            {
                variables.Add(l);
            }
            foreach (Line l in grid.columns)
            {
                variables.Add(l);
            }

            bool backtrack(List<Line> a,List<Line> v)
            {
                
                if (a.Count == grid.numOfCols + grid.numOfRows)
                {
                    return true;
                }
                Line x = v[0];
                v.Remove(x);
                foreach(List<bool> value in x.possibleSolutions)
                {
                    bool consistent = true;
                    if(x.lineType == LineType.row)
                    {
                        foreach(Line l in a.Where(n=> n.lineType == LineType.col))
                        {
                            foreach (bool field in value)
                            {
                                if (field == l.currentSolution[x.index])
                                {
                                    consistent = true;
                                }
                                else
                                    consistent = false;
                            }
                        }
                    }
                    if (x.lineType == LineType.col)
                    {
                        foreach (Line l in a.Where(n => n.lineType == LineType.row))
                        {
                            if (consistent == false)
                                break;
                            foreach (bool field in value)
                            {
                                if (consistent == false)
                                    break;
                                if (field == l.currentSolution[x.index])
                                {
                                    consistent = true;
                                }
                                else
                                {
                                    consistent = false;
                                }
                            }
                        }
                    }

                    if(consistent)
                    {
                        x.currentSolution = value;
                        a.Add(x);
                        backtrack(a, v);
                        foreach(Line l in a.Where(n=> n.index == x.index))
                        {
                            l.possibleSolutions.Remove(value);
                            break;
                        }
                    }
                }
                return false;
            }
            bool success = backtrack(assignment, variables);
            foreach(Line l in assignment)
            {
                List<bool> lb = l.currentSolution;
                foreach(bool b in lb)
                {
                    if (b)
                    {
                        Console.Write("#");
                    }
                    else
                        Console.Write("-");
                }
                Console.WriteLine();
            }
         
        }


    }
}
