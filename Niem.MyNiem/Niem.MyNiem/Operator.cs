using System;
using System.Collections.Generic;
using System.Text;

namespace Niem.MyNiem
{
    public abstract class Operator : Expression
    {
        protected List<Expression> _nodes = new List<Expression>();
        protected abstract string OpName { get; }

        public Operator()
        {
        }

        public Operator(IEnumerable<Expression> expressions)
        {
            _nodes.AddRange(expressions);
        }

        public void Add(Expression expression)
        {
            _nodes.Add(expression);
        }

        public void AddRange(IEnumerable<Expression> expressions)
        {
            _nodes.AddRange(expressions);
        }

        protected internal override string GetCAMLInternal()
        {
            if (_nodes.Count < 2)
                throw new Exception("Binary operator must have at least to operands.");

            Queue<Expression> queue = new Queue<Expression>(_nodes);

            return Recurse(queue);
        }

        private string Recurse(Queue<Expression> queue)
        {
            if (queue.Count == 2)
            {
                return string.Format(@"<{0}>
                                           {1}
                                           {2}
                                       </{0}>", OpName, queue.Dequeue().GetCAMLInternal(), queue.Dequeue().GetCAMLInternal());
            }
            else
            {
                return string.Format(@"<{0}>
                                           {1}
                                           {2}
                                       </{0}>", OpName, queue.Dequeue().GetCAMLInternal(), Recurse(queue));

            }
        }

        //
        public static Or Or(params Expression[] expressions)
        {
            return new Or(expressions);
        }

        public static Or Or(IEnumerable<Expression> expressions)
        {
            return new Or(expressions);
        }

        public static And And(params Expression[] expressions)
        {
            return new And(expressions);
        }

        public static And And(IEnumerable<Expression> expressions)
        {
            return new And(expressions);
        }
    }
}
