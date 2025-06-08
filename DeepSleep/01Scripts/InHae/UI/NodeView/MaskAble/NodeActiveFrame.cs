using DG.Tweening;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasRenderer))]
public class NodeActiveFrame : MaskableGraphic
{
    [SerializeField] private Material _frameMaterial;
    private int _frameFillHash = Shader.PropertyToID("_FillValue");
    private Material _currentFrameMat;
    
    protected override void Awake()
    {
        base.Awake();
        _currentFrameMat = new Material(_frameMaterial);
    }

    public override Material GetModifiedMaterial(Material baseMaterial)
    {
        var toUse = _currentFrameMat;
            
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

        Material mat = new Material(toUse);
        _currentFrameMat = mat;
        return mat;
    }


    public void ActiveFrameInit()
    {
        _currentFrameMat.SetFloat(_frameFillHash, 0.6f);
    }

    public void ActiveFrameEnable()
    {
        _currentFrameMat.DOFloat(-0.5f, _frameFillHash, 0.3f);
    }
}
