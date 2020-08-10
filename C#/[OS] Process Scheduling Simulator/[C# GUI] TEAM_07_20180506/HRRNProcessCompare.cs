using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TEAM_07
{
    class HRRNProcessCompare : IComparer<Process>
    {
        public int Compare(Process x, Process y)
        {
            return y.getResponseRatio().CompareTo(x.getResponseRatio());
        }
    }
}
