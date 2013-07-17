using System;
using System.Collections.Generic;
using System.Text;

namespace Niem.MyNiem
{
    public class Or : Operator
    {
        public Or()
        {
        }

        public Or(IEnumerable<Expression> expressions) : base(expressions)
        {
            
        }

        protected override string OpName
        {
            get { return "Or"; }
        }
    }
}
