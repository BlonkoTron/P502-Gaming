using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class OrderFeedbackUI : MonoBehaviour
{
    private OrderController orderController;

    [SerializeField] private Image OrderFullfillImage;
    [SerializeField] private GameObject OrderFullFilledPanel;
    [SerializeField] private Sprite orderCorrectSprite, orderWrongSprite;
    void Start()
    {
        orderController = OrderController.Instance;
        orderController.OnOrderFullfilled.AddListener(OrderController_OnOrderFullfilled);
        OrderFullFilledPanel.SetActive(false);
    }
    private void OnDestroy()
    {
        orderController.OnOrderFullfilled.RemoveListener(OrderController_OnOrderFullfilled);

    }
    private void OrderController_OnOrderFullfilled(bool correctOrder)
    {
        OrderFullFilledPanel.SetActive(true);
        if (correctOrder)
        {
            OrderFullfillImage.sprite = orderCorrectSprite;
        } else
        {
            OrderFullfillImage.sprite = orderWrongSprite;
        }
    }
}
