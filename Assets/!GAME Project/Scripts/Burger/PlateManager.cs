using UnityEngine;
using System.Collections.Generic;
using UnityEngine.XR.Interaction.Toolkit;
using UnityEngine.XR.Interaction.Toolkit.Interactables;

public class PlateManager : MonoBehaviour
{
    [Header("Snap Settings")]
    public Transform topSnapPoint;
    public float snapRange = 0.05f;  // How close an ingredient must be to snap

    public List<IngredientStackable> stackedIngredients = new List<IngredientStackable>();

    public void TrySnap(IngredientStackable ingredient)
    {
        // 1. Check if ingredient is close enough to the top snap point
        float distance = Vector3.Distance(ingredient.snapBottom.position, topSnapPoint.position);
        if (distance > snapRange)
        {
            ingredient.Unlock(); // Not close enough, unlock if it was locked
            return;
        }
        

        // 2. Align ingredient position and rotation
        ingredient.transform.position = topSnapPoint.position - (ingredient.snapBottom.position - ingredient.transform.position);
        ingredient.transform.rotation = topSnapPoint.rotation;

        // 3. Lock it in place
        ingredient.LockInPlace(transform);

        // 4. Add to stack list
        stackedIngredients.Add(ingredient);

        // 5. Disable grabbing on previous ingredient
        if (stackedIngredients.Count > 1)
        {
            var previousTop = stackedIngredients[stackedIngredients.Count - 2];
            previousTop.GetComponent<XRGrabInteractable>().enabled = false;
        }

        // 6. Make new top grab-enabled
        ingredient.GetComponent<XRGrabInteractable>().enabled = true;

        // 7. Move top snap point upward
        MoveSnapPointUp(ingredient);
    }

    public void OnIngredientRemoved(IngredientStackable ingredient)
    {
        ingredient.transform.SetParent(null); // Detach from plate
        if (!stackedIngredients.Contains(ingredient)) return;
       
        // Unlock the removed ingredient
       // ingredient.Unlock();
        

        // Remove from stack list
        int index = stackedIngredients.IndexOf(ingredient);

        // Remove this and all above (if multi-pick removal is ever added)
        stackedIngredients.RemoveRange(index, stackedIngredients.Count - index);

        // Move snap point down to new top (or plate)
        MoveSnapPointDown();

        // Re-enable grabbing for new top if exists
        if (stackedIngredients.Count > 0)
        {
            var newTop = stackedIngredients[stackedIngredients.Count - 1];
            newTop.GetComponent<XRGrabInteractable>().enabled = true;
        }
        
    }

    private void MoveSnapPointUp(IngredientStackable ingredient)
    {
        // Move the snap point upward by the height of the new ingredient
        Bounds bounds = GetCombinedColliderBounds(ingredient);
        topSnapPoint.position += Vector3.up * bounds.size.y;
    }

    private void MoveSnapPointDown()
    {
        if (stackedIngredients.Count == 0)
        {
            // Reset to plate level
            topSnapPoint.localPosition = Vector3.up * 0.02f;
            return;
        }

        // Move down to top of the new highest ingredient
        IngredientStackable top = stackedIngredients[stackedIngredients.Count - 1];
        Bounds bounds = GetCombinedColliderBounds(top);
        topSnapPoint.position = top.transform.position + Vector3.up * bounds.size.y / 2f;
    }

    private Bounds GetCombinedColliderBounds(IngredientStackable ingredient)
    {
        Collider[] colliders = ingredient.GetComponentsInChildren<Collider>();
        Bounds bounds = colliders[0].bounds;
        foreach (var col in colliders)
            bounds.Encapsulate(col.bounds);
        return bounds;
    }
}

