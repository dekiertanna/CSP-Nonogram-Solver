using System;
using System.Collections.Generic;
using System.Drawing;
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

            System.IO.StreamReader file = new System.IO.StreamReader("serce.txt");

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
            List<Solution> assignment = new List<Solution>();

            foreach (Line l in grid.rows)
            {
                variables.Add(l);
            }
            foreach (Line l in grid.columns)
            {
                variables.Add(l);
            }
            List<bool> defaultSolution = new List<bool>();
            foreach(Line l in variables)
            {
                foreach(bool b in l.possibleSolutions[0])
                {
                    defaultSolution.Add(false);
                }

                assignment.Add(new Solution(defaultSolution, l.lineType, l.index));
                defaultSolution.Clear();
            }
            variables = variables.OrderBy(o => o.possibleSolutions.Count).ToList();
            List<Line> safeVars = new List<Line>(variables);

            foreach(Line l in safeVars)
            {
                foreach(List<bool> b in l.possibleSolutions)
                {
                    foreach(bool bb in b)
                    {
                        Console.WriteLine(bb);
                    }
                }
            }

            bool valid(LineType lt, int index, List<bool> possibleSolution, List<Solution> solution)
            {
                for (int i = 0; i < solution.Count; i++)
                {
                    Solution temp = solution[i];
                    if (temp.lineType != lt && temp.assigned)
                    {

                        if (temp.solution[index] != possibleSolution[temp.index])
                        {
                            return false;
                        }
                    }

                }
                return true;
            }
            System.IO.StreamWriter fileTest = new System.IO.StreamWriter("testing.txt",true);
            bool forwardChecking(List<Line> vars,List<bool> value,LineType lt, int index)
            {
                for(int i=0;i<vars.Count;i++)
                {
                    if(vars[i].lineType!=lt)
                    {
                        List<List<bool>> possibleSolutionsCopy = new List<List<bool>>();
                        for(int z=0;z<vars[i].possibleSolutions.Count;z++)
                        {
                            possibleSolutionsCopy.Add(vars[i].possibleSolutions[z]);
                        }
                        for(int j=0;j<possibleSolutionsCopy.Count;j++)
                        {
                            Console.WriteLine(possibleSolutionsCopy.Count);
                            List<bool> solution = possibleSolutionsCopy[j];
                            for(int k =0; k<value.Count;k++)
                            {
                                if (value[k] != solution[index])
                                {
                                    vars[i].possibleSolutions.RemoveAll(n=> n==possibleSolutionsCopy[j]);
                                }
                            }
                        }
                        if (vars[i].possibleSolutions.Count == 0)
                        {
                           
                            return false;
                        }
                    }
                    
                }
                return true;
                
            }
            bool backtrack(List<Line> vars, int level,List<Solution> sol)
            {

                Console.WriteLine(level);
                fileTest.WriteLine(level);
                if(level == vars.Count)
                {
                    return true;
                }

                Line l = vars[level];
                List<List<bool>> possibilities = new List<List<bool>>();
                List<bool> value;
                for(int i=0;i<l.possibleSolutions.Count;i++)
                {
                    if(l.possibleSolutions[i].Count!=0)
                        possibilities.Add(l.possibleSolutions[i]);
                }

                for(int i=0;i <possibilities.Count;i++)
                {
                    value = possibilities[i];
                    int idx = sol.FindIndex(n => n.index == l.index && n.lineType == l.lineType);
                    sol[idx].solution = value;
                    sol[idx].assigned = true;
                    if (valid(l.lineType, l.index, value, sol))
                    {
                        
                        if (backtrack(vars,level+1,sol))
                        {
                            return true;
                        }
                    }
                }
                int idx2 = sol.FindIndex(n => n.index == l.index && n.lineType == l.lineType);
                defaultSolution.Clear();
                for(int i=0;i<possibilities[0].Count;i++)
                {
                    defaultSolution.Add(false);
                }
                sol[idx2].solution = defaultSolution;
                sol[idx2].assigned = false;
                return false;
            }

            backtrack(variables, 0,assignment);
            foreach (Solution l in assignment)
            {
                Console.WriteLine(l.index);
                Console.WriteLine(l.lineType);
                foreach (bool b in l.solution)
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

            Bitmap output = new Bitmap(grid.numOfCols, grid.numOfRows);
            List<Solution> toDraw = new List<Solution>();
            
            foreach(Solution s in assignment)
            {
                if (s.lineType == LineType.row)
                    toDraw.Add(s);
            }
            
            for(int i =0;i<toDraw.Count;i++)
            {
                for(int j=0;j<toDraw[i].solution.Count;j++)
                {
                    if(toDraw[i].solution[j])
                    {
                        output.SetPixel(j, i, Color.Black);
                    }
                    else
                    {
                        output.SetPixel(j, i, Color.White);
                    }
                }
            }

            Bitmap resize(Bitmap img, int scale)
            {
                try
                {
                    Bitmap b = new Bitmap(img.Width * scale, img.Height * scale);
                    using (Graphics g = Graphics.FromImage((Image)b))
                    {
                        g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                        g.DrawImage(img, 0, 0, img.Width * scale, img.Height * scale);
                    }
                    return b;

                }
                catch
                {
                    Console.WriteLine("Could not resize");
                    return img;
                }
            }

            output = resize(output, 100);
            output.Save("output.png");


        }


    }
}