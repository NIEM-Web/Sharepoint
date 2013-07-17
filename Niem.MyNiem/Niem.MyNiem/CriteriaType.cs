using System;
using System.Collections.Generic;
using System.Text;

namespace Niem.MyNiem
{
    public enum CriteriaType
    {
        Eq,
        Neq,
        Gt,
        Geq,
        Lt,
        Leq,
        IsNull,
        IsNotNull,
        BeginsWith,
        Contains,
        DateRangesOverlap
    }
}
