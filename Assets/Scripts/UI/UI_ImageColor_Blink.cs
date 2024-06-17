using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class UI_ImageColor_Blink : MonoBehaviour
{

    [SerializeField] private float interval = 1.0f;
    [SerializeField] private Color endColor;

    private Image imageComponent;

    // Start is called before the first frame update
    void Start()
    {
        imageComponent = GetComponent<Image>();
        imageComponent.DOColor(endColor, interval).SetLoops(-1, LoopType.Yoyo).SetEase(Ease.InOutQuad);
    }
}
