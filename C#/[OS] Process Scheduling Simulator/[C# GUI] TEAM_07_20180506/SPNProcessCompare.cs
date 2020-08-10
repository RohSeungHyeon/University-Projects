using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TEAM_07
{
    class SPNProcessCompare : IComparer<Process>
    {
        public int Compare(Process x, Process y)
        {
            return x.getBurstTime().CompareTo(y.getBurstTime());
        }
    }
}
