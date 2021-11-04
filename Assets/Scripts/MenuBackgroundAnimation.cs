using System.Collections;
using UnityEngine;
using UnityEngine.UI;
public class MenuBackgroundAnimation : MonoBehaviour
{
    private Camera change;
    private float colorH = 0f;
    [SerializeField] private Image buttonImage, buyButtonImage;
    [SerializeField] private Image[] moneyBuyImages = new Image[4];
    [SerializeField] private Image[] languageBorders = new Image[2];
    [SerializeField] private GameObject SettingsPanel;
    private void Start()
    {
        change = GetComponent<Camera>();
        StartCoroutine(ColorChange());
    }

    private IEnumerator ColorChange()
    {
        void UpdateMoneyBuyImagesColor(Color color)
        {
            foreach (var i in moneyBuyImages)
                i.color = color;
        }
        void UpdateLangugageBordersColor(Color color)
        {
            foreach (var i in languageBorders)
                if (i.IsActive())
                    i.color = color;
        }
        var delay = new WaitForSeconds(0.1f);
        var whiteColor = Color.white;
        while (true)
        {
            while (colorH < 1f)
            {
                colorH += 0.01f;
                var color = Color.HSVToRGB(colorH, 0.5f, 1f);
                change.backgroundColor = color;
                var lerpedColor = Color.Lerp(color, whiteColor, 0.5f);
                if (buttonImage.IsActive())
                {
                    buttonImage.color = lerpedColor;
                    if (SettingsPanel.activeSelf)
                        UpdateLangugageBordersColor(lerpedColor);
                }
                else if (buyButtonImage.IsActive())
                    buyButtonImage.color = lerpedColor;
                else
                    UpdateMoneyBuyImagesColor(lerpedColor);
                yield return delay;
            }
            while (colorH > 0f)
            {
                colorH -= 0.01f;
                var color = Color.HSVToRGB(colorH, 0.5f, 1f);
                change.backgroundColor = color;
                var lerpedColor = Color.Lerp(color, whiteColor, 0.5f);
                if (buttonImage.IsActive())
                {
                    buttonImage.color = lerpedColor;
                    if (SettingsPanel.activeSelf)
                        UpdateLangugageBordersColor(lerpedColor);
                }
                else if (buyButtonImage.IsActive())
                    buyButtonImage.color = lerpedColor;
                else
                    UpdateMoneyBuyImagesColor(lerpedColor);
                yield return delay;
            }
        }
    }
}
