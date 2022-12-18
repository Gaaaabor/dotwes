using Akka.Actor;
using DungeonOfTheWickedEventSourcing.Common.Actors.ActorWalker.Models;

namespace DungeonOfTheWickedEventSourcing.Common.Actors.ActorWalker
{
    internal class ActorVisitor
    {
        private readonly RootNode _rootNode = new RootNode(0, "ActorSystem");
        private readonly bool _includeSystemNodes;

        public ActorVisitor(bool includeSystemNodes = false)
        {
            _includeSystemNodes = includeSystemNodes;
        }

        internal RootNode Visit(ActorSystem actorSystem)
        {
            var children = Visit(actorSystem as ExtendedActorSystem);
            _rootNode.AddChildren(children);
            _rootNode.Measure();

            return _rootNode;
        }

        internal IReadOnlyCollection<ActorHierarchyNode> Visit(ExtendedActorSystem extendedActorSystem)
        {
            var nodes = new List<ActorHierarchyNode>();
            if (extendedActorSystem is null)
            {
                return nodes;
            }

            if (extendedActorSystem.Guardian.IsLocal)
            {
                var guardianNode = new ActorHierarchyNode
                {
                    Id = extendedActorSystem.Guardian.Path.Uid,
                    Name = extendedActorSystem.Guardian.Path.Name
                };

                nodes.Add(guardianNode);

                var guardianNodes = Visit(extendedActorSystem.Guardian as LocalActorRef);
                guardianNode.AddChildren(guardianNodes);
            }

            if (extendedActorSystem.SystemGuardian.IsLocal && _includeSystemNodes)
            {
                var systemGuardianNode = new ActorHierarchyNode
                {
                    Id = extendedActorSystem.SystemGuardian.Path.Uid,
                    Name = extendedActorSystem.SystemGuardian.Path.Name
                };

                nodes.Add(systemGuardianNode);

                var systemGuardianNodes = Visit(extendedActorSystem.SystemGuardian as LocalActorRef);
                systemGuardianNode.AddChildren(systemGuardianNodes);
            }

            return nodes;
        }

        internal IReadOnlyCollection<ActorHierarchyNode> Visit(LocalActorRef localActorRef)
        {
            var nodes = new List<ActorHierarchyNode>();
            if (localActorRef is null)
            {
                return nodes;
            }

            var children = localActorRef.Children.ToList();
            foreach (var child in children)
            {
                var childNode = new ActorHierarchyNode
                {
                    Id = child.Path.Uid,
                    Name = child.Path.Name
                };

                nodes.Add(childNode);

                var childNodeChildren = Visit(child as RepointableActorRef);
                childNode.AddChildren(childNodeChildren);
            }

            return nodes;
        }

        internal IReadOnlyCollection<ActorHierarchyNode> Visit(RepointableActorRef repointableActorRef)
        {
            var nodes = new List<ActorHierarchyNode>();
            if (repointableActorRef is null)
            {
                return nodes;
            }

            var children = repointableActorRef.Children.ToList();
            foreach (var child in children)
            {
                var childNode = new ActorHierarchyNode
                {
                    Id = child.Path.Uid,
                    Name = child.Path.Name
                };

                nodes.Add(childNode);

                if (child is RepointableActorRef childRepointableActorRef)
                {
                    var childNodeChildren = Visit(childRepointableActorRef);
                    childNode.AddChildren(childNodeChildren);
                }
                else if (child is LocalActorRef childLocalActorRef)
                {
                    var childNodeChildren = Visit(childLocalActorRef);
                    childNode.AddChildren(childNodeChildren);
                }
            }

            return nodes;
        }
    }
}
