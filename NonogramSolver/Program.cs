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
                Line l = new Line(clues, numOfCols);
                rows.Add(l);

            }

            for (int i = 0; i < numOfCols; i++)
            {
                line = file.ReadLine();
                List<int> clues = new List<int>(Array.ConvertAll(line.Split(';'), int.Parse));
                Line l = new Line(clues, numOfRows);
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
            
            bool allAssigned(Grid grid)
            {
                foreach()
            }

            void backtrack(int level, Grid grid)
            {

            }
            

         
        }


    }
}
