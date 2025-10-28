using UnityEngine;

public class Receipt : MonoBehaviour
{
    private MeshRenderer _meshRenderer;

    private void Awake()
    {
        _meshRenderer = GetComponent<MeshRenderer>();
    }
    public void UpdateReceiptMaterial(Material mat)
    {
        _meshRenderer.material = mat;
    }
}
