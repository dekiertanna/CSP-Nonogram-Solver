using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace NonogramSolver
{
    public class Solution
    {
        public List<bool> solution;
        public LineType lineType;
        public int index;
        public bool assigned;

       public Solution(List<bool> solution, LineType lineType, int index)
        {
            this.solution = solution;
            this.lineType = lineType;
            this.index = index;
            this.assigned = false;
        }
    }
}
