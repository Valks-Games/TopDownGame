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

        var texSize = tex.GetSize();

        var tileWidth = World.TileSize / texSize.X;
        var tileHeight = World.TileSize / texSize.Y;

        for (int z = 0; z < size; z++)
        {
            for (int x = 0; x < size; x++)
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

                // UVs
                var u = (World.TileSize * tileX) / texSize.X;
                var v = (World.TileSize * tileY) / texSize.Y;

                uvs[vIndex]     = new Vector2(u, v);
                uvs[vIndex + 1] = new Vector2(u, v + tileHeight);
                uvs[vIndex + 2] = new Vector2(u + tileWidth, v + tileHeight);
                uvs[vIndex + 3] = new Vector2(u + tileWidth, v);

                //normals     [vIndex] = new Vector3( 0, 0,  s);
                //normals [vIndex + 1] = new Vector3( 0, 0, s );
                //normals [vIndex + 2] = new Vector3( 0, 0, s );
                //normals [vIndex + 3] = new Vector3( 0, 0, s );

                vIndex += 4;
                iIndex += 6;

                // Move down column by 1
                pos += new Vector3(width, 0, 0);
            }

            // Reset column and move down 1 row
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

        var meshInstance = new MeshInstance2D
        {
            Mesh = mesh,
            ZIndex = -10,
            Texture = tex
        };

        parent.AddChild(meshInstance);
    }
}
