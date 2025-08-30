using UnityEngine;

[RequireComponent(typeof(MeshFilter))]
[RequireComponent(typeof(MeshRenderer))]
public class MeshSquare : MonoBehaviour
{
    public Texture2D texture;
    public string shaderName = "Standard";

    private void Awake()
    {
        MeshFilter filter = gameObject.GetComponent<MeshFilter>();
        MeshRenderer renderer = gameObject.GetComponent<MeshRenderer>();
        renderer.material = new Material(Shader.Find(shaderName));
        renderer.material.mainTexture = texture;

        Mesh mesh = new();

        /** Vertices **/
        Vector3[] vertices = new Vector3[]
        {
            new Vector3(-1f, -1f, 0f),
            new Vector3(-1f, 1f, 0f),
            new Vector3(1f, 1f, 0f),
            new Vector3(1f, -1f, 0f)
        };

        /** UV **/
        Vector2[] uv = new Vector2[]
        {
            new Vector2(0f, 0f),
            new Vector2(0f, 1f),
            new Vector2(1f, 1f),
            new Vector2(1f, 0f)
        };

        /** Triangles **/
        int[] triangles = new int[]
        {
            0, 1, 2,
            2, 3, 0
        };

        mesh.vertices = vertices;
        mesh.uv = uv;
        mesh.triangles = triangles;
        mesh.RecalculateNormals();

        filter.mesh = mesh;
    }
}
