using UnityEngine;
using UnityEngine.EventSystems;

public class BaseNodeContent : MonoBehaviour, IDragHandler, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private float _moveSpeed;
    [SerializeField] private float _whellSpeed;
    
    [SerializeField] protected RectTransform _visual;

    private float _currentScale = 1;
    private bool _isMouseOver;
    
    public void OnDrag(PointerEventData eventData)
    {
        if(eventData.button != PointerEventData.InputButton.Left)
            return;

        _visual.localPosition += (Vector3)eventData.delta * _moveSpeed;
    }

    private void Update()
    {
        if(!_isMouseOver)
            return;

        if (Input.GetAxis("Mouse ScrollWheel") > 0f)
            _currentScale += _whellSpeed;
        else if (Input.GetAxis("Mouse ScrollWheel") < 0f)
            _currentScale -= _whellSpeed;
        
        _currentScale = Mathf.Clamp(_currentScale, 0.3f, 1.5f);
        _visual.localScale = Vector3.one * _currentScale; 
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        _isMouseOver = true;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        _isMouseOver = false;
    }
}
