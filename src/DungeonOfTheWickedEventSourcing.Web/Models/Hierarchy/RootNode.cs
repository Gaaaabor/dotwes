namespace DungeonOfTheWickedEventSourcing.Web.Models.Hierarchy
{
    public class RootNode : ActorHierarchyNode
    {
        public string HostName { get; set; }
        public int Width { get; set; }
        public int Height { get; set; }

        public override string ToString()
        {
            return $"{HostName} - {base.ToString()}";
        }
    }
}
