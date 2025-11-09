using UnityEngine;
using System.Collections;

public class AlignAfterXRReady : MonoBehaviour
{
    public Transform armTransform;
    public Transform targetTransform;
    public Vector3 localPositionOffset;
    public Vector3 localRotationOffset;
    public bool continuousFollow = false;

    void Start()
    {
        // Wait one frame to let XR Origin finish repositioning
        StartCoroutine(DelayedAlign());
    }

    IEnumerator DelayedAlign()
    {
        // Wait until the end of the first frame
        yield return null;
        yield return new WaitForEndOfFrame();

        AlignNow();
    }

    void LateUpdate()
    {
        if (continuousFollow)
            AlignNow();
    }

    [ContextMenu("Align Now")]
    public void AlignNow()
    {
        if (armTransform == null || targetTransform == null) return;

        armTransform.SetParent(targetTransform);
        armTransform.localPosition = localPositionOffset;
        armTransform.localRotation = Quaternion.Euler(localRotationOffset);
    }
}
