namespace RTS;

public partial class Player : Entity
{
    public float Speed { get; set; } = 10;
    public float Friction { get; set; } = 0.1f;

    int prevChunkX, prevChunkY;
    int chunkSpawnRadius = 3;

    protected override void Update()
    {
        var pixelChunkSize = World.ChunkSize * World.TileSize;

        var chunkX = (int)Mathf.Floor(Position.X / pixelChunkSize);
        var chunkY = (int)Mathf.Floor(Position.Y / pixelChunkSize);

        if (prevChunkX != chunkX || prevChunkY != chunkY)
        {
            for (int x = -chunkSpawnRadius / 2; x <= chunkSpawnRadius / 2; x++)
            {
                for (int y = -chunkSpawnRadius / 2; y <= chunkSpawnRadius / 2; y++)
                {
                    var posX = chunkX + x;
                    var posY = chunkY + y;

                    // No key exists in the dictionary so no chunk has been generated here before
                    if (!World.ChunkGenerated.ContainsKey(new Vector2I(posX, posY)))
                    {
                        World.Instance.GenerateChunk(posX, posY);
                    }
                    else
                    {
                        // A chunk was generated here before but it has been removed
                        if (!World.ChunkGenerated[new Vector2I(posX, posY)])
                        {
                            World.Instance.GenerateChunk(posX, posY);
                        }
                    }
                }
            }
        }

        prevChunkX = chunkX;
        prevChunkY = chunkY;
    }

    protected override State InitialState() => Move();

    State Move()
    {
        var state = new State("Move");

        state.Update = () =>
        {
            // Velocity is mutiplied by delta for us already
            Velocity += GUtils.GetMovementInput() * Speed;
            Velocity = Velocity.Lerp(Vector2.Zero, Friction);
        };

        return state;
    }
}
