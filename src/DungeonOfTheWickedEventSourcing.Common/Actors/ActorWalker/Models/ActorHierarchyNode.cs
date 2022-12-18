namespace DungeonOfTheWickedEventSourcing.Common.Actors.ActorWalker.Models
{
    public class ActorHierarchyNode
    {
        private readonly List<ActorHierarchyNode> _children = new List<ActorHierarchyNode>();

        public long Id { get; init; }
        public string Name { get; init; }

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
