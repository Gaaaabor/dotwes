﻿namespace DungeonOfTheWickedEventSourcing.Api.Application.Dungeon.Commands
{
    public interface IDungeonCommand : IDungeonWideCommand
    {
        public Guid DungeonId { get; set; }
    }
}
