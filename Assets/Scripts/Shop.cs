using UnityEngine.UI;
using UnityEngine;
using TMPro;
public class Shop : MonoBehaviour // внутриигровой магазин
{
    private GameObject skinViewHolder; // держатель скинов
    private Color whiteButton, grayButton; // цвета кнопок
    [SerializeField] private RectTransform arrowTransform; // трансформ кнопок для переключения скинов
    [SerializeField] private GameObject[] tabs = new GameObject[2]; // вкладки в магазине
    [SerializeField] private Image[] tabButtons = new Image[2]; // изображения кнопок для переключения вкладок
    [SerializeField] private AudioSource purchaseSound; // звук приобретения скина
    [SerializeField] private GameObject acceptPanel; // окно подтверждения покупки
    [SerializeField] private Animator canvasHandler, skinViewAnimator, buyButtonAnimator; // аниматоры канваса, представления скина, кнопки покупки
    [SerializeField] private TextMeshProUGUI skinName, buttonLabel, moneyView, secondMoneyView; // текста  имени скина, кнопки покупки, монет на 1 и 2 вкладке
    [SerializeField] private MeshRenderer meshRenderer; // рендер шарика, на котором изображён скин
    private Material skinView; // материал скина
    [SerializeField]
    private Button skinBuyButton, acceptButton; // кнопки покупки, подтверждения покупки
    [SerializeField]
    private SkinData[] skinsData; // информация о скинах
    private Skin[] skins; // скины
    private int index = 0; // индекс текущего скина
    private int money = 0; // кол-во монет
    private bool[] isPurchased; // купленные скины
    private int equipedIndex; // индекс одетого скина
    private void Awake() // инициализация значений
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
    private void Start() // иницализация значений, выравнивание скина
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
    private void Buy(int price) // покупка скина
    {
        if (!acceptPanel.activeSelf)
        {
            var skin = skins[index];
            var priceToBuy = price;
            if (price > money)
            {
#if !UNITY_EDITOR && !UNITY_WEBGL
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
    private void Equip(int index) // одеть/снять скин
    {
        if (equipedIndex != index)
        {
            equipedIndex = index;
            buttonLabel.text = LocalizeManager.GetLocalizedString(Translation.Remove, false);
        }
        else
        {
            equipedIndex = -1;
            buttonLabel.text = LocalizeManager.GetLocalizedString(Translation.Equip, false);
        }
        Preferences.EquipedSkin = equipedIndex;
    }
    private void SetListener(Skin skin) // установить листенер на кнопку в зависимости от скина
    {
        skinBuyButton.onClick.RemoveAllListeners();
        if (!skin.isPurchased)
        {
            var price = skin.skinData.Price;
            buttonLabel.text = $"{LocalizeManager.GetLocalizedString(Translation.BuyFor, false)}{price}$";
            skinBuyButton.onClick.AddListener(delegate
            {
                Buy(price);
            });
        }
        else
        {
            buttonLabel.text = equipedIndex == index ? LocalizeManager.GetLocalizedString(Translation.Remove, false) :
                LocalizeManager.GetLocalizedString(Translation.Equip, false);
            skinBuyButton.onClick.AddListener(delegate
            {
                Equip(index);
            });
        }
    }
    public void ChooseTab(int tab) // переключение вкладок
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
    public void Move(int value) // смена скина
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
    public void CloseAccept() // закрыть диалог подтверждения
    {
        acceptPanel.SetActive(false);
    }
    public void CloseShop() // закрыть магазин
    {
        if (!acceptPanel.activeSelf)
        {
            canvasHandler.Play("CloseShop");
            if (skinViewHolder.activeSelf)
                skinViewAnimator.Play("CloseSkinView");
        }
    }
    public void BuyMoney(int money) // купить монеты, заготовка для доната в игре
    {
        this.money += money;
        secondMoneyView.text = this.money.ToString();
        Preferences.SetMoney(this.money);
    }
  

}
