namespace DungeonOfTheWickedEventSourcing.Common.Akka.ActorWalker.Models
{
    public class ActorHierarchyNode
    {
        private readonly List<ActorHierarchyNode> _children = new List<ActorHierarchyNode>();
        
        public string Name { get; set; }
        public IEnumerable<ActorHierarchyNode> Children => _children.ToArray();

        public void AddChildren(IEnumerable<ActorHierarchyNode> nodes)
        {
            _children.AddRange(nodes);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
