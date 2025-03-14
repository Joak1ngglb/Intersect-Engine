﻿namespace Intersect.Framework.Core.GameObjects.Maps;

public partial class NpcSpawn
{
    public NpcSpawnDirection Direction;

    public Guid NpcId;

    public int X;

    public int Y;

    public NpcSpawn()
    {
    }

    public NpcSpawn(NpcSpawn copy)
    {
        NpcId = copy.NpcId;
        X = copy.X;
        Y = copy.Y;
        Direction = copy.Direction;
    }
}
