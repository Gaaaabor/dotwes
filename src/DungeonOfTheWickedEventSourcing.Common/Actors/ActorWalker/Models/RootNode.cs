using System.Net;

namespace DungeonOfTheWickedEventSourcing.Common.Actors.ActorWalker.Models
{
    public class RootNode : ActorHierarchyNode
    {
        public string HostName { get; init; }
        public int Width { get; private set; }
        public int Height { get; private set; }

        public RootNode(long id, string name)
        {
            Id = id;
            Name = name;
            HostName = Dns.GetHostName();
        }

        public void Measure()
        {
            Measure(Children);
        }

        private void Measure(IEnumerable<ActorHierarchyNode> childrens)
        {
            if (!childrens.Any())
            {
                return;
            }

            Height++;

            var childCount = childrens.Count(); // TODO: Sum the width on each level
            if (Width < childCount)
            {
                Width = childCount;
            }

            Measure(childrens.SelectMany(x => x.Children));
        }

        public override string ToString()
        {
            return $"{HostName} - {base.ToString()}";
        }
    }
}
