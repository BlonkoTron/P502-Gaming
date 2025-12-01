using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using TMPro;
using FMOD.Studio;
using FMODUnity;

public class OrderFeedbackUI : MonoBehaviour
{
    private OrderController orderController;

    [SerializeField] private Image OrderFullfillImage;
    [SerializeField] private GameObject OrderFullFilledPanel;
    [SerializeField] private Sprite orderCorrectSprite, orderWrongSprite;
    [SerializeField] private TMP_Text orderCountText;
    private float panelAnimationTime = 4;

    private EventInstance HappyAlien;
    [SerializeField] private EventReference GladAlien;

    private EventInstance SadAlien;
    [SerializeField] private EventReference Saddestalien;
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
            HappyAlien = Audiomanager.instance.PlaySound(GladAlien, transform.position);
        } else
        {
            OrderFullfillImage.sprite = orderWrongSprite;
            SadAlien = Audiomanager.instance.PlaySound(Saddestalien, transform.position);
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
