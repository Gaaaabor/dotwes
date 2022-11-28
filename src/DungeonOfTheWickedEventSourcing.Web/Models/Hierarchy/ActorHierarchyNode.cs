namespace DungeonOfTheWickedEventSourcing.Web.Models.Hierarchy
{
    public class ActorHierarchyNode
    {
        public string Name { get; set; }
        public List<ActorHierarchyNode> Children { get; set; }

        public override string ToString()
        {
            return Name;
        }
    }
}
