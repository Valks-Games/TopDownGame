namespace RTS;

public class Chunk
{
    int chunkX, chunkY;

    public Chunk(Node parent, int chunkX, int chunkY)
    {
        this.chunkX = chunkX;
        this.chunkY = chunkY;

        GenerateMesh(parent);
    }

    void GenerateMesh(Node parent)
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

        var tex = GD.Load<Texture2D>("res://Sprites/basictiles.png");

        var s = World.TileSize / 2; // hard coded size
        var w = s * 2; // width

        var chunkSize = w * size;
        var chunkCoords = new Vector2(chunkX, chunkY);
        var tileChunkPos = chunkCoords * chunkSize; // (6400, 6400)
        var chunkPos = chunkCoords * size; // (100, 100)

        // Adding s adds hardcoded offset to align with godots grid
        // Also offset by (-chunkSize / 2) to center chunk
        var posVec3 = 
            new Vector3(
                x: s + (-chunkSize / 2) + tileChunkPos.X, 
                y: s + (-chunkSize / 2) + tileChunkPos.Y, 
                z: 0);

        SetUVs(ref uvs, tex.GetSize());

        var i = 0;
        var v = 0;

        GD.Print(uvs.Length);  // 400
        GD.Print(size * size); // 100

        for (int z = 0; z < size; z++)
        {
            for (int x = 0; x < size; x++)
            {
                vertices[v] = new Vector3(-s, -s, 0) + posVec3;
                vertices[v + 1] = new Vector3(-s, s, 0) + posVec3;
                vertices[v + 2] = new Vector3(s, s, 0) + posVec3;
                vertices[v + 3] = new Vector3(s, -s, 0) + posVec3;

                indices[i] = v;
                indices[i + 1] = v + 1;
                indices[i + 2] = v + 2;

                indices[i + 3] = v + 2;
                indices[i + 4] = v + 3;
                indices[i + 5] = v + 0;

                //normals     [v] = new Vector3( 0, 0,  s);
                //normals [v + 1] = new Vector3( 0, 0, s );
                //normals [v + 2] = new Vector3( 0, 0, s );
                //normals [v + 3] = new Vector3( 0, 0, s );

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

    void SetUVs(ref Vector2[] uvs, Vector2 texSize)
    {
        // The texture has 8 x 15 tiles
        // Each tile is 16 pixels in size
        var texTiles = new Vector2(8, 15);

        var tileX = 0;
        var tileY = 0;

        var tileWidth = World.TileSize / texSize.X;
        var tileHeight = World.TileSize / texSize.Y;

        for (int i = 0; i < uvs.Length; i += 4)
        {
            //GD.Print($"({tileX}, {tileY})");

            var x = (World.TileSize * tileX) / texSize.X;
            var y = (World.TileSize * tileY) / texSize.Y;

            uvs[i] = new Vector2(x, y);
            uvs[i + 1] = new Vector2(x, y + tileHeight);
            uvs[i + 2] = new Vector2(x + tileWidth, y + tileHeight);
            uvs[i + 3] = new Vector2(x + tileWidth, y);

            tileX += 1;

            if (tileX % 8 == 0)
            {
                tileX -= 8;
                tileY += 1;
            }
        }
    }
}
