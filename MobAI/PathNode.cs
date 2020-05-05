using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace The_Game.MobAI
{
    public class PathNode<T> : IEnumerable<T>
    {
        public T Value { get; }
        public float Cost { get; set; }
        public PathNode<T> Previous { get; set; }

        public PathNode(T val, float cost, PathNode<T> prev)
        {
            Value = val;
            Cost = cost;
            Previous = prev;
        }

        public IEnumerator<T> GetEnumerator()
        {
            var currNode = this;
            while (currNode != null)
            {
                yield return currNode.Value;
                currNode = currNode.Previous;
            }
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}
