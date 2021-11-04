using UnityEngine.UI;
using UnityEngine;
using TMPro;
public class Shop : MonoBehaviour
{
    private GameObject skinViewHolder;
    private Color whiteButton, grayButton;
    [SerializeField] private RectTransform arrowTransform;
    [SerializeField] private GameObject[] tabs = new GameObject[2];
    [SerializeField] private Image[] tabButtons = new Image[2];
    [SerializeField] private AudioSource purchaseSound;
    [SerializeField] private GameObject acceptPanel;
    [SerializeField] private Animator canvasHandler, skinViewAnimator, buyButtonAnimator;
    [SerializeField] private TextMeshProUGUI skinName, buttonLabel, moneyView, secondMoneyView;
    [SerializeField] private MeshRenderer meshRenderer;
    private Material skinView;
    [SerializeField]
    private Button skinBuyButton, acceptButton;
    [SerializeField]
    private SkinData[] skinsData;
    private Skin[] skins;
    private int index = 0;
    private int money = 0;
    private bool[] isPurchased;
    private int equipedIndex;
    private void Awake()
    {
        whiteButton = Color.white;
        grayButton = new Color(0.9f, 0.9f, 0.9f);
        skinView = meshRenderer.material;
        equipedIndex = Preferences.EquipedSkin;
        money = Preferences.Money;
        moneyView.text = money.ToString();
        isPurchased = Preferences.PurchasedSkins;
        skins = new Skin[Preferences.SkinsCount];
        for (int i = 0; i < Preferences.SkinsCount; i++)
            skins[i] = new Skin(skinsData[i], isPurchased[i]);
    }
    private void Start()
    {
        skinViewHolder = SkinViewController.SharedGameObject;
        var pos = skinViewHolder.transform.position;
        var arrowPos = arrowTransform.position;   
        pos.y = Camera.main.ScreenToWorldPoint(new Vector3(arrowPos.x, arrowPos.y, 3f)).y;
        skinViewHolder.transform.position = pos;
        SetListener(skins[0]);
        LocalizeManager.AddChangeListener(delegate
        {
            SetListener(skins[index]);
        });
    }
    private void Buy(int price)
    {
        if (!acceptPanel.activeSelf)
        {
            var skin = skins[index];
            var priceToBuy = price;
            if (price > money)
            {
#if !UNITY_EDITOR
            Handheld.Vibrate();
#endif
                buyButtonAnimator.Play("NoMoney");
            }
            else
            {
                acceptPanel.SetActive(true);
                acceptButton.onClick.RemoveAllListeners();
                acceptButton.onClick.AddListener(delegate
                {
                    CloseAccept();
                    purchaseSound.Play();
                    isPurchased[index] = true;
                    equipedIndex = index;
                    skin.isPurchased = true;
                    skins[index] = skin;
                    SetListener(skin);
                    money -= price;
                    moneyView.text = money.ToString();
                    Preferences.EquipedSkin = equipedIndex;
                    Preferences.PurchasedSkins = isPurchased;
                    Preferences.SetMoney(money);
                });
            }
        }
    }
    private void Equip(int index)
    {
        if (equipedIndex != index)
        {
            equipedIndex = index;
            buttonLabel.text = LocalizeManager.GetLocalizedString(LocalizeManager.Remove, false);
        }
        else
        {
            equipedIndex = -1;
            buttonLabel.text = LocalizeManager.GetLocalizedString(LocalizeManager.Equip, false);
        }
        Preferences.EquipedSkin = equipedIndex;
    }
    private void SetListener(Skin skin)
    {
        skinBuyButton.onClick.RemoveAllListeners();
        if (!skin.isPurchased)
        {
            var price = skin.skinData.Price;
            buttonLabel.text = $"{LocalizeManager.GetLocalizedString(LocalizeManager.BuyFor, false)}{price}$";
            skinBuyButton.onClick.AddListener(delegate
            {
                Buy(price);
            });
        }
        else
        {
            buttonLabel.text = equipedIndex == index ? LocalizeManager.GetLocalizedString(LocalizeManager.Remove, false) :
                LocalizeManager.GetLocalizedString(LocalizeManager.Equip, false);
            skinBuyButton.onClick.AddListener(delegate
            {
                Equip(index);
            });
        }
    }
    public void ChooseTab(int tab)
    {
        var choosedIndex = tab;
        var isChoosedIndexEqualsOne = choosedIndex == 1;
        var anotherIndex =  isChoosedIndexEqualsOne ? 0 : 1;
        skinViewHolder.SetActive(!isChoosedIndexEqualsOne);
        tabButtons[choosedIndex].color = grayButton;
        tabButtons[anotherIndex].color = whiteButton;
        tabs[anotherIndex].SetActive(false);
        tabs[choosedIndex].SetActive(true);
        if (isChoosedIndexEqualsOne)
            secondMoneyView.text = money.ToString();
        else
            moneyView.text = money.ToString();
    }
    public void Move(int value)
    {
        if (!acceptPanel.activeSelf)
        {
            var temp = index + value;
            index = temp == Preferences.SkinsCount ? 0 : temp == -1 ? 4 : temp;
            var skin = skins[index];
            skinName.text = skin.skinData.Name;
            skinView.mainTexture = skin.skinData.SkinTexture;
            SetListener(skin);
        }
    }
    public void CloseAccept()
    {
        acceptPanel.SetActive(false);
    }
    public void CloseShop()
    {
        if (!acceptPanel.activeSelf)
        {
            canvasHandler.Play("CloseShop");
            if (skinViewHolder.activeSelf)
                skinViewAnimator.Play("CloseSkinView");
        }
    }
    public void BuyMoney(int money)
    {
        this.money += money;
        secondMoneyView.text = this.money.ToString();
        Preferences.SetMoney(this.money);
    }
  

}
