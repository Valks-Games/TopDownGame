namespace RTS;

public class Chunk
{
    public Chunk(Node parent, int chunkX, int chunkY)
    {
        GenerateMesh(parent, new Vector2(chunkX, chunkY), World.ChunkSize);
        //this.GenerateChunk(chunkX, chunkY);
    }

    public void GenerateMesh(Node parent, Vector2 chunkCoords, int size)
    {
        var vertices = new Vector3[4 * size * size];
        //var normals  = new Vector3[4 * size * size];
        var uvs      = new Vector2[4 * size * size];
        var colors   = new   Color[4 * size * size];
        var indices  = new     int[6 * size * size];

        for (int n = 0; n < colors.Length; n++)
        {
            colors[n] = Colors.White;
        }

        for (int m = 0; m < uvs.Length; m++)
        {
            uvs[m] = new Vector2(0, 0);
        }

        var tex = GD.Load<Texture2D>("res://Sprites/basictiles.png");

        {
            // The texture has 8 x 15 tiles
            // Each tile is 16 pixels in size
            var texTiles = new Vector2(8, 15);

            var tileX = 2;
            var tileY = 1;

            var x = (World.TileSize * tileX) / tex.GetSize().X;
            var y = (World.TileSize * tileY) / tex.GetSize().Y;

            var tileWidth = World.TileSize / tex.GetSize().X;
            var tileHeight = World.TileSize / tex.GetSize().Y;

            uvs[0] = new Vector2(x, y);
            uvs[1] = new Vector2(x, y + tileHeight);
            uvs[2] = new Vector2(x + tileWidth, y + tileHeight);
            uvs[3] = new Vector2(x + tileWidth, y);
        }

        var s = World.TileSize; // hard coded size
        var w = s * 2; // width

        var chunkSize = w * size;
        var tileChunkPos = chunkCoords * chunkSize; // (6400, 6400)
        var chunkPos = chunkCoords * size; // (100, 100)

        // Adding s adds hardcoded offset to align with godots grid
        // Also offset by (-chunkSize / 2) to center chunk
        var posVec3 = new Vector3(s + (-chunkSize / 2) + tileChunkPos.X, s + (-chunkSize / 2) + tileChunkPos.Y, 0);

        var i = 0;
        var v = 0;

        for (int z = 0; z < size; z++)
        {
            for (int x = 0; x < size; x++)
            {
                //Biomes[biomeData[x, z]].Generate(colors, v);

                vertices[v] = new Vector3(-s, -s, 0) + posVec3;
                //normals     [v] = new Vector3( 0, 0,  s);
                //uvs         [v] = new Vector2( 0, 0    );

                vertices[v + 1] = new Vector3(-s, s, 0) + posVec3;
                //normals [v + 1] = new Vector3( 0, 0, s );
                //uvs     [v + 1] = new Vector2( 0, s    );

                vertices[v + 2] = new Vector3(s, s, 0) + posVec3;
                //normals [v + 2] = new Vector3( 0, 0, s );
                //uvs     [v + 2] = new Vector2( s, s    );	

                vertices[v + 3] = new Vector3(s, -s, 0) + posVec3;
                //normals [v + 3] = new Vector3( 0, 0, s );
                //uvs     [v + 3] = new Vector2( s, 0    );

                indices[i] = v;
                indices[i + 1] = v + 1;
                indices[i + 2] = v + 2;

                indices[i + 3] = v + 2;
                indices[i + 4] = v + 3;
                indices[i + 5] = v + 0;

                v += 4;
                i += 6;

                // Move down column by 1
                posVec3 += new Vector3(w, 0, 0);
            }

            // Reset column and move down 1 row
            posVec3 += new Vector3(-w * size, w, 0);
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

        var meshInstance = new MeshInstance2D
        {
            Mesh = mesh,
            ZIndex = -10,
            Texture = tex
        };

        parent.AddChild(meshInstance);
    }

    void GenerateChunk(int chunkX, int chunkY)
    {
        foreach (var atlas in World.Atlases)
            for (int x = 0; x < World.ChunkSize; x++)
                for (int y = 0; y < World.ChunkSize; y++)
                    GenerateTile(chunkX, chunkY, x, y, atlas);
    }

    void GenerateTile(int chunkX, int chunkY, int x, int y, Atlas atlas)
    {
        var globalX = (chunkX * World.ChunkSize) + x;
        var globalY = (chunkY * World.ChunkSize) + y;

        string type = "";
        var currentNoise = atlas.FNL.GetNoise2D(globalX, globalY);

        foreach (var atlasValue in atlas.TileData)
        {
            if (currentNoise < atlasValue.Value.Weight)
            {
                type = atlasValue.Key;
                break;
            }
        }

        SetCell(
            atlas.TileMap, 
            type,
            globalX, 
            globalY, 
            atlas.TileData[type].TilePosition);
    }

    void SetCell(TileMap tileMap, string typeName, int x, int y, Vector2I atlasPos)
    {
        if (typeName != "empty")
            tileMap.SetCell(0, new Vector2I(x, y), 0, atlasPos);
    }
}
