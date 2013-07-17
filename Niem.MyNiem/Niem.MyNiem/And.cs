using System;
using System.Collections.Generic;
using System.Text;

namespace Niem.MyNiem
{
    public class And : Operator
    {
        public And()
        {
        }

        public And(IEnumerable<Expression> expressions) : base(expressions)
        {
            
        }

        protected override string OpName
        {
            get { return "And"; }
        }
    }
}
