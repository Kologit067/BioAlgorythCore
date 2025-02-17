using BaseContract.Interfaces;
using ExactStringCompare.Helpers;
using System.Diagnostics;

namespace ExactStringCompare
{
    //--------------------------------------------------------------------------------------
    // class SuffixTreeBase 
    //--------------------------------------------------------------------------------------
    public abstract class SuffixTreeBase
    {
        protected Stopwatch stopwatch;
        protected SuffixTreeNode root;
        public ISuffixTreeAccumulator StatisticAccumulator { get; set; }
        public abstract SuffixTreeNode Execute(string text);
        //--------------------------------------------------------------------------------------
        public string NodePresentationAsString()
        {
            return NodePresentationAsString(root);
        }
        //--------------------------------------------------------------------------------------
        protected string NodePresentationAsString(SuffixTreeNode node)
        {
            return $"[{node.StarSegment}-{node.EndSegment}]({string.Join(",", node.Chields.OrderBy(n => n.Key).Select(n => NodePresentationAsString(n.Value)))})";
        }
        //--------------------------------------------------------------------------------------
    }
    //--------------------------------------------------------------------------------------
}
