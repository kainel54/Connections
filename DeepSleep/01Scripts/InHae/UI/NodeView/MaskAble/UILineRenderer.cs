using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

namespace IH.UI
{
    [RequireComponent(typeof(CanvasRenderer))]
    public class UILineRenderer : MaskableGraphic
    {
        public Vector2[] points;
        public float thickness = 1f;
        public bool center = true;
        public Color lineColor;
        
        [SerializeField] private Material _lineMaterial;
        private readonly int _lineColorParam = Shader.PropertyToID("_LineColor");
        private Material _uiLineMaterial;

        protected override void Awake()
        {
            base.Awake();
            _uiLineMaterial = new Material(_lineMaterial);
        }

        // protected override void OnValidate()
        // {
        //     base.OnValidate();
        //     SetVerticesDirty();
        // }

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();
            if (points.Length < 2)
                return;
            
            for (int i = 0; i < points.Length - 1; i++)
            {
                CreateLineSegment(points[i], points[i + 1], vh);
                //뭔가 할꺼야
                int index = i * 5;

                vh.AddTriangle(index, index + 1, index + 3);
                vh.AddTriangle(index + 3, index + 2, index);

                if (i != 0)
                {
                    vh.AddTriangle(index, index - 1, index - 3);
                    vh.AddTriangle(index + 1, index - 1, index - 2);
                }
            }
        }

        public override Material GetModifiedMaterial(Material baseMaterial)
        {
            var toUse = _uiLineMaterial;

            if (m_ShouldRecalculateStencil)
            {
                if (maskable)
                {
                    var rootCanvas = MaskUtilities.FindRootSortOverrideCanvas(transform);
                    m_StencilValue = MaskUtilities.GetStencilDepth(transform, rootCanvas);
                }
                else
                    m_StencilValue = 0;

                m_ShouldRecalculateStencil = false;
            }

            if (m_StencilValue > 0 && !isMaskingGraphic)
            {
                var maskMat = StencilMaterial.Add(toUse, (1 << m_StencilValue) - 1,
                    StencilOp.Keep, CompareFunction.Equal, ColorWriteMask.All, (1 << m_StencilValue) - 1, 0);
                StencilMaterial.Remove(m_MaskMaterial);
                m_MaskMaterial = maskMat;
                toUse = m_MaskMaterial;
            }

            _uiLineMaterial = toUse;
            return toUse;
        }

        private void CreateLineSegment(Vector3 point1, Vector3 point2, VertexHelper vh)
        {
            Vector3 offset = center ? (rectTransform.sizeDelta * 0.5f) : Vector3.zero;
    
            UIVertex vertex = UIVertex.simpleVert;
            vertex.color = lineColor;
    
            Quaternion point1Rot = Quaternion.Euler(0, 0, RotatePointToward(point1, point2) + 90f);
            vertex.position = point1Rot * new Vector3(-thickness * 0.5f, 0); //왼쪽에 있는 점 회전
            vertex.position += point1 - offset;
            vh.AddVert(vertex);
    
            vertex.position = point1Rot * new Vector3(thickness * 0.5f, 0); //오른쪽에 있는 점 회전
            vertex.position += point1 - offset;
            vh.AddVert(vertex);
    
            Quaternion point2Rot = Quaternion.Euler(0, 0, RotatePointToward(point2, point1) - 90f);
            vertex.position = point2Rot * new Vector3(-thickness * 0.5f, 0); //왼쪽에 있는 점 회전
            vertex.position += point2 - offset;
            vh.AddVert(vertex);
    
            vertex.position = point2Rot * new Vector3(thickness * 0.5f, 0); //오른쪽에 있는 점 회전
            vertex.position += point2 - offset;
            vh.AddVert(vertex);

            vertex.position = point2 - offset;
            vh.AddVert(vertex);
        }

        private float RotatePointToward(Vector3 vertex, Vector3 target)
            => Mathf.Atan2(target.y - vertex.y, target.x - vertex.x) * Mathf.Rad2Deg;

        public void LineEnable()
        {
            _uiLineMaterial.SetColor(_lineColorParam, Color.yellow);
        }

        public void LineDisable()
        {
            _uiLineMaterial.SetColor(_lineColorParam, new Color(0.3f, 0.3f, 0.3f));
        }
    }
}
