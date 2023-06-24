namespace RTS;

public class Chunk
{
    static List<Vector2I> check = new();

    public bool Generated { get; set; }

    public Chunk(Node parent, int chunkX, int chunkY)
    {
        if (check.Contains(new Vector2I(chunkX, chunkY)))
        {
            Logger.LogWarning($"TRIED TO DUPLICATE CHUNK AT ({chunkX}, {chunkY})");
            return;
        }
        else
        {
            check.Add(new Vector2I(chunkX, chunkY));
            Logger.Log($"There are now {check.Count} chunks in the scene");
        }

        Generated = true;

        var chunkParent = new Node2D();

        foreach (var atlas in World.TileLayers)
        {
            chunkParent.AddChild(GenerateMesh(chunkParent, chunkX, chunkY, atlas));
        }

        parent.AddChild(chunkParent);
    }

    MeshInstance2D GenerateMesh(Node2D parent, int chunkX, int chunkY, TileLayer tileLayer)
    {
        var size = World.ChunkSize;
        var vertices = new Vector3[4 * size * size];
        //var normals  = new Vector3[4 * size * size];
        var uvs      = new Vector2[4 * size * size];
        var colors   = new   Color[4 * size * size];
        var indices  = new     int[6 * size * size];

        for (int n = 0; n < colors.Length; n++)
            colors[n] = Colors.White;

        for (int m = 0; m < uvs.Length; m++)
            uvs[m] = new Vector2(0, 0);

        var image = tileLayer.TileSetImage;

        var tileOffset = World.TileSize / 2; // hard coded size
        var width = tileOffset * 2; // width

        var chunkSize = width * size;
        var chunkCoords = new Vector2(chunkX, chunkY);
        var tileChunkPos = chunkCoords * chunkSize; // (6400, 6400)
        var chunkPos = chunkCoords * size; // (100, 100)

        // Adding s adds hardcoded offset to align with godots grid
        // Also offset by (-chunkSize / 2) to center chunk
        var pos = new Vector3(
            x: tileOffset + (-chunkSize / 2) + tileChunkPos.X, 
            y: tileOffset + (-chunkSize / 2) + tileChunkPos.Y, 
            z: 0);

        var iIndex = 0;
        var vIndex = 0;

        var tileX = 0;
        var tileY = 0;

        var imageSize = image.GetSize();

        var tileWidth = World.TileSize / imageSize.X;
        var tileHeight = World.TileSize / imageSize.Y;

        for (int x = 0; x < size; x++)
        {
            for (int y = 0; y < size; y++)
            {
                // Vertices
                vertices[vIndex]     = new Vector3(-tileOffset, -tileOffset, 0) + pos;
                vertices[vIndex + 1] = new Vector3(-tileOffset,  tileOffset, 0) + pos;
                vertices[vIndex + 2] = new Vector3( tileOffset,  tileOffset, 0) + pos;
                vertices[vIndex + 3] = new Vector3( tileOffset, -tileOffset, 0) + pos;

                // Indices
                indices[iIndex]     = vIndex;
                indices[iIndex + 1] = vIndex + 1;
                indices[iIndex + 2] = vIndex + 2;

                indices[iIndex + 3] = vIndex + 2;
                indices[iIndex + 4] = vIndex + 3;
                indices[iIndex + 5] = vIndex + 0;

                // Obtain the appropriate tile based on current noise
                var globalX = (chunkX * World.ChunkSize) + x;
                var globalY = (chunkY * World.ChunkSize) + y;
                var currentNoise = tileLayer.FNL.GetNoise2D(globalX, globalY);

                foreach (var tile in tileLayer.TileData)
                {
                    if (currentNoise < tile.Weight)
                    {
                        // Create collision if tile has one
                        if (tile.Collision)
                        {
                            var staticBody2D = new StaticBody2D();

                            var collision = new CollisionShape2D
                            {
                                Position = new Vector2(pos.X, pos.Y),
                                Shape = new RectangleShape2D
                                {
                                    Size = Vector2.One * World.TileSize
                                }
                            };

                            staticBody2D.AddChild(collision);

                            parent.AddChild(staticBody2D);
                        }

                        tileX = tile.UV.X;
                        tileY = tile.UV.Y;
                        break;
                    }
                }

                // UVs
                var u = (World.TileSize * tileX) / imageSize.X;
                var v = (World.TileSize * tileY) / imageSize.Y;

                uvs[vIndex] = new Vector2(u, v);
                uvs[vIndex + 1] = new Vector2(u, v + tileHeight);
                uvs[vIndex + 2] = new Vector2(u + tileWidth, v + tileHeight);
                uvs[vIndex + 3] = new Vector2(u + tileWidth, v);

                // Normals
                //normals     [vIndex] = new Vector3( 0, 0,  s);
                //normals [vIndex + 1] = new Vector3( 0, 0, s );
                //normals [vIndex + 2] = new Vector3( 0, 0, s );
                //normals [vIndex + 3] = new Vector3( 0, 0, s );

                vIndex += 4;
                iIndex += 6;

                // Move to next column
                pos += new Vector3(width, 0, 0);
            }

            // Reset columns and move to next row
            pos += new Vector3(-width * size, width, 0);
        }

        var arrays = new Godot.Collections.Array();
        arrays.Resize((int)Mesh.ArrayType.Max);
        arrays[(int)Mesh.ArrayType.Vertex] = vertices;
        //arrays[(int)Mesh.ArrayType.Normal] = normals;
        arrays[(int)Mesh.ArrayType.TexUV] = uvs;
        arrays[(int)Mesh.ArrayType.Color] = colors;
        arrays[(int)Mesh.ArrayType.Index] = indices;

        var mesh = new ArrayMesh();
        mesh.AddSurfaceFromArrays(Mesh.PrimitiveType.Triangles, arrays);

        return new MeshInstance2D
        {
            Mesh = mesh,
            ZIndex = tileLayer.ZIndex,
            Texture = image
        };
    }
}
