using UnityEngine;
using UnityEngine.InputSystem;

public class Chest : MonoBehaviour
{
    public GameObject dummy;

    [SerializeField] private Mesh _closedChest;
    [SerializeField] private Mesh _openedChest;
    [SerializeField] private Material _chestMaterial;

    private MeshFilter _dummyMeshFilter;
    private MeshRenderer _dummyMaterial;

    private void Start()
    {
        GameObject chest = Instantiate(dummy, transform.position, Quaternion.identity);
        chest.transform.Rotate(90, 0, 0);
        _dummyMeshFilter = chest.GetComponent<MeshFilter>();
        _dummyMaterial = chest.GetComponent<MeshRenderer>();

        _dummyMeshFilter.mesh = _closedChest;
        _dummyMaterial.material = _chestMaterial;
    }

    private void Update()
    {
        if(Mouse.current.leftButton.wasPressedThisFrame)
        {
            _dummyMeshFilter.mesh = _openedChest;
        }
    }
}
