using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEditor.SceneManagement;

namespace MyNamespace
{
    public static class EditorMeshSplitter
    {
        [MenuItem("Faza/Split mesh to islands")]
        public static void SplitMeshToIsland()
        {
            var go = Selection.activeGameObject;
            if (go == null) return;

            var smr = go.GetComponent<SkinnedMeshRenderer>();
            if (smr == null) return;

            var prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
            var root = prefabStage.prefabContentsRoot;

            var mesh = smr.sharedMesh;
            var material = smr.sharedMaterial;

            var verts = mesh.vertices;
            var tris = mesh.triangles;
            var uvs = mesh.uv;
            var normals = mesh.normals;

            var triCount = tris.Length / 3;

            var uniquePositions = new List<Vector3>();
            var positionMap = new int[verts.Length]; // original vert index => unique position index

            for (var i = 0; i < verts.Length; i++)
            {
                var current = verts[i];
                var found = false;

                for (int j = 0; j < uniquePositions.Count; j++)
                {
                    if (Vector3.Distance(uniquePositions[j], current) < 0.001f)
                    {
                        positionMap[i] = j;
                        found = true;
                        break;
                    }
                }

                if (found == false)
                {
                    positionMap[i] = uniquePositions.Count;
                    uniquePositions.Add(current);
                }
            }

            // key = index of vertex
            // value = list of triangle indexes
            var vertToTri = new Dictionary<int, List<int>>();
            // adjacency of vertex to triangle

            for (var i = 0; i < triCount; i++)
            {
                var i0 = positionMap[tris[i * 3]];
                var i1 = positionMap[tris[i * 3 + 1]];
                var i2 = positionMap[tris[i * 3 + 2]];

                if (vertToTri.ContainsKey(i0) == false) vertToTri[i0] = new List<int>();
                if (vertToTri.ContainsKey(i1) == false) vertToTri[i1] = new List<int>();
                if (vertToTri.ContainsKey(i2) == false) vertToTri[i2] = new List<int>();

                // i = triangle index

                vertToTri[i0].Add(i);
                vertToTri[i1].Add(i);
                vertToTri[i2].Add(i);
            }

            var triToTri = new List<int>[triCount];
            // adjacency of triangle to triangle
            for (var i = 0; i < triCount; i++)
            {
                triToTri[i] = new List<int>();

                var i0 = positionMap[tris[i * 3]];
                var i1 = positionMap[tris[i * 3 + 1]];
                var i2 = positionMap[tris[i * 3 + 2]];

                var neighbors = new HashSet<int>();

                foreach (var t in vertToTri[i0]) neighbors.Add(t);
                foreach (var t in vertToTri[i1]) neighbors.Add(t);
                foreach (var t in vertToTri[i2]) neighbors.Add(t);

                neighbors.Remove(i);

                triToTri[i].AddRange(neighbors);
            }

            var visited = new bool[triCount];
            var islands = new List<List<int>>();

            for (var i = 0; i < triCount; i++)
            {
                if (visited[i]) continue;

                var stack = new Stack<int>();
                var islandTris = new List<int>();

                stack.Push(i);
                visited[i] = true;

                while (stack.Count > 0)
                {
                    var current = stack.Pop();
                    islandTris.Add(current);

                    foreach (var neighbor in triToTri[current])
                    {
                        if (visited[neighbor] == false)
                        {
                            visited[neighbor] = true;
                            stack.Push(neighbor);
                        }
                    }
                }

                islands.Add(islandTris);
            }

            for (var i = 0; i < islands.Count; i++)
            {
                var triIndexes = islands[i];

                var newVerts = new List<Vector3>();
                var newTris = new List<int>();
                var newUvs = new List<Vector2>();
                var newNormals = new List<Vector3>();
                var vertexMap = new Dictionary<int, int>();

                foreach (var triIndex in triIndexes)
                {
                    int[] oldTri =
                    {
                        tris[triIndex * 3],
                        tris[triIndex * 3 + 1],
                        tris[triIndex * 3 + 2],
                    };

                    for (int j = 0; j < 3; j++)
                    {
                        var oldVertex = oldTri[j];
                        if (vertexMap.ContainsKey(oldVertex) == false)
                        {
                            vertexMap[oldVertex] = newVerts.Count;
                            newVerts.Add(verts[oldVertex]);
                            newUvs.Add(uvs[oldVertex]);
                            newNormals.Add(normals[oldVertex]);
                        }
                        newTris.Add(vertexMap[oldVertex]);
                    }
                }

                var newMesh = new Mesh();
                newMesh.vertices = newVerts.ToArray();
                newMesh.triangles = newTris.ToArray();
                newMesh.uv = newUvs.ToArray();
                newMesh.normals = newNormals.ToArray();
                newMesh.RecalculateBounds();

                var name = $"{go.name}_island_{i}";

                var valid = AssetDatabase.IsValidFolder("Assets/_GeneratedMeshes");
                if (valid == false)
                {
                    AssetDatabase.CreateFolder("Assets", "_GeneratedMeshes");
                }

                AssetDatabase.CreateAsset(newMesh, $"Assets/_GeneratedMeshes/{name}.asset");
                AssetDatabase.SaveAssets();
                AssetDatabase.Refresh();

                var islandGo = new GameObject(name);
                var meshFilter = islandGo.AddComponent<MeshFilter>();
                var meshRenderer = islandGo.AddComponent<MeshRenderer>();

                meshFilter.sharedMesh = newMesh;
                meshRenderer.sharedMaterial = material;

                islandGo.transform.position = smr.transform.position;
                islandGo.transform.rotation = smr.transform.rotation;
                islandGo.transform.localScale = smr.transform.localScale;

                islandGo.transform.SetParent(root.transform, true);

                #region Gib
                islandGo.layer = LayerMask.NameToLayer("Gibs");
                var rigidbody = islandGo.AddComponent<Rigidbody>();
                rigidbody.collisionDetectionMode = CollisionDetectionMode.Continuous;
                rigidbody.interpolation = RigidbodyInterpolation.Interpolate;
                rigidbody.angularDrag = 0f;
                var meshCollider = islandGo.AddComponent<BoxCollider>();
                meshCollider.sharedMaterial = Resources.Load<PhysicMaterial>("Gib");
                #endregion
            }

            EditorSceneManager.MarkSceneDirty(prefabStage.scene);
        }
    } 
}
