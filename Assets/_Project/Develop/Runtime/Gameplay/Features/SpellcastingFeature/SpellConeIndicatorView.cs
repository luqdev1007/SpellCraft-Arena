using UnityEngine;

namespace Assets._Project.Develop.Runtime.Gameplay.Features.SpellcastingFeature
{
    [RequireComponent(typeof(MeshFilter), typeof(MeshRenderer))]
    public class SpellConeIndicatorView : MonoBehaviour
    {
        private const int Segments = 24;

        private float _timer;

        public void Init(float range, float angleDeg, float duration)
        {
            MeshFilter filter = GetComponent<MeshFilter>();
            MeshRenderer rend = GetComponent<MeshRenderer>();

            filter.mesh = BuildSectorMesh(range, angleDeg);

            Material mat = new Material(Shader.Find("Sprites/Default"));
            mat.color = new Color(0.25f, 0.7f, 1f, 0.35f);
            rend.material = mat;
            rend.shadowCastingMode = UnityEngine.Rendering.ShadowCastingMode.Off;
            rend.receiveShadows = false;

            transform.localPosition = new Vector3(0f, 0.05f, 0f);
            _timer = duration;
        }

        private void Update()
        {
            _timer -= Time.deltaTime;
            if (_timer <= 0f)
                Destroy(gameObject);
        }

        private static Mesh BuildSectorMesh(float range, float angleDeg)
        {
            float halfRad = angleDeg * 0.5f * Mathf.Deg2Rad;

            Vector3[] verts = new Vector3[Segments + 2];
            int[] tris = new int[Segments * 3];
            Vector2[] uvs = new Vector2[verts.Length];

            verts[0] = Vector3.zero;
            uvs[0] = new Vector2(0.5f, 0f);

            for (int i = 0; i <= Segments; i++)
            {
                float a = Mathf.Lerp(-halfRad, halfRad, (float)i / Segments);
                verts[i + 1] = new Vector3(Mathf.Sin(a), 0f, Mathf.Cos(a)) * range;
                uvs[i + 1] = new Vector2((Mathf.Sin(a) + 1f) * 0.5f, Mathf.Cos(a));
            }

            for (int i = 0; i < Segments; i++)
            {
                tris[i * 3 + 0] = 0;
                tris[i * 3 + 1] = i + 1;
                tris[i * 3 + 2] = i + 2;
            }

            Mesh mesh = new Mesh { name = "SpellCone" };
            mesh.vertices = verts;
            mesh.triangles = tris;
            mesh.uv = uvs;
            mesh.RecalculateNormals();
            return mesh;
        }

#if UNITY_EDITOR
        // Gizmo drawn when the parent hero GO is selected in the editor.
        // Set _debugRange/_debugAngle on this component in play mode to preview.
        [Header("Editor Preview (Scene view only)")]
        [SerializeField] private float _debugRange = 5f;
        [SerializeField] private float _debugAngle = 60f;

        private void OnDrawGizmosSelected()
        {
            DrawConeGizmo(_debugRange, _debugAngle);
        }

        private void DrawConeGizmo(float range, float angleDeg)
        {
            UnityEditor.Handles.color = new Color(0.25f, 0.7f, 1f, 0.5f);
            float halfRad = angleDeg * 0.5f * Mathf.Deg2Rad;

            Vector3 origin = transform.position + Vector3.up * 0.05f;
            Vector3 leftEdge = origin + (Quaternion.Euler(0f, -angleDeg * 0.5f, 0f) * transform.forward) * range;
            Vector3 rightEdge = origin + (Quaternion.Euler(0f, angleDeg * 0.5f, 0f) * transform.forward) * range;

            UnityEditor.Handles.DrawLine(origin, leftEdge);
            UnityEditor.Handles.DrawLine(origin, rightEdge);
            UnityEditor.Handles.DrawWireArc(origin, Vector3.up, Quaternion.Euler(0f, -angleDeg * 0.5f, 0f) * transform.forward, angleDeg, range);
            UnityEditor.Handles.color = new Color(0.25f, 0.7f, 1f, 0.15f);
            UnityEditor.Handles.DrawSolidArc(origin, Vector3.up, Quaternion.Euler(0f, -angleDeg * 0.5f, 0f) * transform.forward, angleDeg, range);
        }
#endif
    }
}
