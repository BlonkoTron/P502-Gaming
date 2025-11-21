using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;

public class OrderFeedbackUI : MonoBehaviour
{
    private OrderController orderController;

    [SerializeField] private Image OrderFullfillImage;
    [SerializeField] private GameObject OrderFullFilledPanel;
    [SerializeField] private Sprite orderCorrectSprite, orderWrongSprite;
    [SerializeField] private TMP_Text orderCountText;
    private float panelAnimationTime = 4;
    void Start()
    {
        orderController = OrderController.Instance;
        orderController.OnOrderFullfilled.AddListener(OrderController_OnOrderFullfilled);
        OrderFullFilledPanel.SetActive(false);
        UpdateOrderCountText();
    }
    private void OnDestroy()
    {
        orderController.OnOrderFullfilled.RemoveListener(OrderController_OnOrderFullfilled);
    }
    private void OrderController_OnOrderFullfilled(bool correctOrder)
    {
        StartCoroutine(OrderFullfillpanelAnimate());
        if (correctOrder)
        {
            OrderFullfillImage.sprite = orderCorrectSprite;
        } else
        {
            OrderFullfillImage.sprite = orderWrongSprite;
        }
        UpdateOrderCountText();
    }
    private IEnumerator OrderFullfillpanelAnimate()
    {
        OrderFullFilledPanel.SetActive(true);
        yield return new WaitForSeconds(panelAnimationTime);
        OrderFullFilledPanel.SetActive(false);
    }
    private void UpdateOrderCountText()
    {
        if (orderCountText != null)
        {
            orderCountText.text = orderController.correctOrdersServed.ToString();
        }
    }
}
