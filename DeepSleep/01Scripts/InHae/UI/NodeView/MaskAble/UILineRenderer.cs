using DG.Tweening;
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
        private readonly int _lineEnableValueParam = Shader.PropertyToID("_UVXValue");
        private Material _uiLineMaterial;

        private Tween _chargeTween;
        
        protected override void OnEnable()
        {
            base.OnEnable();
            InitMaterialIfNeeded();
        }

        public void InitMaterialIfNeeded()
        {
            if (_uiLineMaterial == null && _lineMaterial != null)
            {
                _uiLineMaterial = new Material(_lineMaterial);
                _uiLineMaterial.name = "UILineMaterial (Instance)";
                _uiLineMaterial.hideFlags = HideFlags.HideAndDontSave;
                SetVerticesDirty();
                SetMaterialDirty();
            }
        }

        public void Init()
        {
            points = new Vector2[points.Length];
        }

        protected override void OnPopulateMesh(VertexHelper vh)
        {
            vh.Clear();
            if (points.Length < 2)
                return;
            
            for (int i = 0; i < points.Length - 1; i++)
            {
                CreateLineSegment(points[i], points[i + 1], vh);
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
            return _uiLineMaterial;
        }

        private void CreateLineSegment(Vector3 point1, Vector3 point2, VertexHelper vh)
        {
            Vector3 offset = center ? (rectTransform.sizeDelta * 0.5f) : Vector3.zero;
    
            UIVertex vertex = UIVertex.simpleVert;
            vertex.color = lineColor;
    
            Quaternion point1Rot = Quaternion.Euler(0, 0, RotatePointToward(point1, point2) + 90f);
            vertex.position = point1Rot * new Vector3(-thickness * 0.5f, 0);
            vertex.position += point1 - offset;
            vertex.uv0 = new Vector2(0f, 0f);
            vh.AddVert(vertex);
    
            vertex.position = point1Rot * new Vector3(thickness * 0.5f, 0);
            vertex.position += point1 - offset;
            vertex.uv0 = new Vector2(0f, 1f);
            vh.AddVert(vertex);
    
            Quaternion point2Rot = Quaternion.Euler(0, 0, RotatePointToward(point2, point1) - 90f);
            vertex.position = point2Rot * new Vector3(-thickness * 0.5f, 0);
            vertex.position += point2 - offset;
            vertex.uv0 = new Vector2(1f, 0f);
            vh.AddVert(vertex);
    
            vertex.position = point2Rot * new Vector3(thickness * 0.5f, 0);
            vertex.position += point2 - offset;
            vertex.uv0 = new Vector2(1f, 1f);
            vh.AddVert(vertex);

            vertex.position = point2 - offset;
            vertex.uv0 = new Vector2(0.5f, 0.5f);
            vh.AddVert(vertex);
        }

        private float RotatePointToward(Vector3 vertex, Vector3 target)
            => Mathf.Atan2(target.y - vertex.y, target.x - vertex.x) * Mathf.Rad2Deg;

        public void LineEnable()
        {
            InitMaterialIfNeeded();
            _uiLineMaterial.SetFloat(_lineEnableValueParam, 1.0f);
        }

        public void LineDisable()
        {
            if (_chargeTween.IsActive() && _chargeTween.IsPlaying())
            {
                _chargeTween.Kill();
            }
            
            InitMaterialIfNeeded();
            _uiLineMaterial.SetFloat(_lineEnableValueParam, 0.0f);
        }

        public void LineLerpEnable()
        {
            InitMaterialIfNeeded();
            
            _chargeTween = _uiLineMaterial.DOFloat(1.0f, _lineEnableValueParam, 0.75f).SetUpdate(true)
                .SetEase(Ease.OutExpo);
        }
    }
}
