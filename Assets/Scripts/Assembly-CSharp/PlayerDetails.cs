using System;
using System.Collections;
using I2.Loc;
using UnityEngine;
using UnityEngine.UI;

[Serializable]
public class PlayerDetails : MonoBehaviour
{
    public bool showingBackups;

    public bool showingFemales;

    public bool alwaysShowFemaleHere;

    public bool alwaysShowMaleHere;

    public bool upgradeScreen;

    public bool timeoutScreen;

    public Text fullName;
    [Header("玩家名字"), Space(5)] public Text fullNameInputField;

    public Image starIcon;

    public Text starValue;

    public Image[] starIconsEven;

    public Image[] starIconsOdd;

    public Sprite starIconSprite;

    public Sprite starHalfIconSprite;

    public Text bballValue;

    public Text bballMaxValue;

    public PlayerVisual playerVisual;

    public GameObject[] guiItems;

    public GameObject buyButton;

    public GameObject buyButtonCash;

    public ViewDealButton viewDealButton;

    public GameObject upgradeButton;

    public GameObject activateButton;

    public GameObject sellButton;

    public GameObject activeLabel;

    public LockedButton lockedButton;

    public GameObject customizeButton;

    public Text price;

    public Text priceCash;

    public PlayerStatsDisplay playerStatsDisplay;

    public GameObject customizePlayerBox;

    public Localize customizePlayerBoxHeading;

    public PlayerDetails customizePlayerBoxPlayerDetails;

    public TopNavBar topNavBar;

    public GetGoldButton getGoldButton;

    public CurrencyExchangeBox currencyExchangeBox;

    private Player player;

    private Players players;

    private SessionVars sessionVars;

    private CustomItems customItems;

    private CharacterSprites characterSprites;

    public GameObject playersButton;

    public Transform playerVisualHead;

    public Transform playerVisualBall;

    private Animator playerVisualAnim;

    private float defaultScale;

    public bool scaleCharacter;

    private bool scaledCharacter;

    public bool flickerWhenSettingCharacter;

    private bool flicker;

    public bool autoShowCorrectPlayer;

    private bool startFunctionCompleted;

    private GameSounds gameSounds;

    public PlayerDetails()
    {
        autoShowCorrectPlayer = true;
    }

    public virtual void Awake()
    {
        sessionVars = SessionVars.GetInstance();
        gameSounds = GameSounds.GetInstance();
        players = (Players)base.gameObject.GetComponent(typeof(Players));
        players.InstantiatePlayers();
        playerVisualAnim = (Animator)playerVisual.GetComponent(typeof(Animator));
        GameObject gameObject = GameObject.Find("CustomItems");
        if (gameObject != null)
        {
            customItems = (CustomItems)gameObject.GetComponent(typeof(CustomItems));
        }
        characterSprites = (CharacterSprites)GameObject.Find("CharacterSprites").GetComponent(typeof(CharacterSprites));
        if (alwaysShowFemaleHere)
        {
            showingFemales = true;
        }
        else if (alwaysShowMaleHere)
        {
            showingFemales = false;
        }
        defaultScale = playerVisual.gameObject.transform.localScale.x;
    }

    public virtual void OnEnable()
    {
        if (upgradeScreen)
        {
            showingBackups = sessionVars.showingBackups;
            showingFemales = sessionVars.showingFemales;
        }
        else if (timeoutScreen)
        {
            if (sessionVars.goToPractice)
            {
                showingFemales = sessionVars.showingFemales;
            }
            else
            {
                showingFemales = sessionVars.currentTournament.isFemale;
            }
        }
        if (autoShowCorrectPlayer)
        {
            ShowCorrectPlayer();
        }
        if (Player.playerNamesMayHaveChanged && player != null)
        {
            string fullNameKey = player.fullNameKey;
            player.fullName = ((!(PlayerPrefs.GetString(fullNameKey) != string.Empty)) ? fullNameKey : PlayerPrefs.GetString(fullNameKey));
            fullName.text = player.fullName;
        }
    }

    public virtual void ShowCorrectPlayer()
    {
        if (!base.gameObject.activeInHierarchy)
        {
            return;
        }
        if (!showingBackups && Players.GetActiveStarterNum(showingFemales) == -1)
        {
            SetToEmpty(true);
            return;
        }
        SetToEmpty(false);
        if (showingBackups)
        {
            StartCoroutine(SetPlayer(Players.GetActiveBackupNum(showingFemales)));
        }
        else
        {
            StartCoroutine(SetPlayer(Players.GetActiveStarterNum(showingFemales)));
        }
    }

    public virtual void SetPlayerAndGender(int num, bool isFemale)
    {
        showingFemales = isFemale;
        StartCoroutine(SetPlayer(num));
    }

    public virtual IEnumerator SetPlayer(int num)
    {
        scaledCharacter = false;
        if (showingBackups)
        {
            player = players.GetBackup(showingFemales, num);
        }
        else
        {
            player = players.GetStarter(showingFemales, num);
        }
        if (flicker)
        {
            playerVisual.gameObject.SetActive(false);
        }
        if (Player.playerNamesMayHaveChanged)
        {
            string fullNameKey = player.fullNameKey;
            player.fullName = ((!(PlayerPrefs.GetString(fullNameKey) != string.Empty)) ? fullNameKey : PlayerPrefs.GetString(fullNameKey));
        }
        fullName.text = player.fullName;
        if (fullNameInputField != null)
        {
            fullNameInputField.text = player.fullName;
        }
        SetStars(player.GetStarValue());
        if (bballValue != null)
        {
            bballValue.text = player.GetStatTotal().ToString();
        }
        if (bballMaxValue != null)
        {
            bballMaxValue.text = "/" + player.GetStatMaxTotal();
        }
        if (price != null && priceCash != null)
        {
            if (player.goldPrice < 0)
            {
                int num2 = player.goldPrice * -1;
                priceCash.text = num2.ToString("n0");
            }
            else
            {
                price.text = player.goldPrice.ToString("n0");
            }
        }
        int activePlayerNum2 = 0;
        activePlayerNum2 = ((!showingBackups) ? Players.GetActiveStarterNum(showingFemales) : Players.GetActiveBackupNum(showingFemales));
        if (buyButton != null)
        {
            buyButton.SetActive(false);
        }
        if (viewDealButton != null)
        {
            viewDealButton.gameObject.SetActive(false);
        }
        if (buyButtonCash != null)
        {
            buyButtonCash.SetActive(false);
        }
        if (lockedButton != null)
        {
            lockedButton.gameObject.SetActive(false);
        }
        if (customizeButton != null)
        {
            customizeButton.SetActive(false);
        }
        if (upgradeButton != null)
        {
            upgradeButton.SetActive(false);
        }
        if (activateButton != null)
        {
            activateButton.SetActive(false);
        }
        if (sellButton != null)
        {
            sellButton.SetActive(false);
        }
        if (activeLabel != null)
        {
            activeLabel.SetActive(false);
        }
        if (player.num == activePlayerNum2)
        {
            if (upgradeButton != null)
            {
                upgradeButton.SetActive(true);
            }
            if (activeLabel != null)
            {
                activeLabel.SetActive(true);
            }
            if (customizeButton != null)
            {
                customizeButton.SetActive(true);
            }
        }
        else if (player.IsOwned())
        {
            if (upgradeButton != null)
            {
                upgradeButton.SetActive(true);
            }
            if (activeLabel != null)
            {
                activateButton.SetActive(true);
            }
            if (customizeButton != null)
            {
                customizeButton.SetActive(true);
            }
            if (!(sellButton != null))
            {
            }
        }
        else
        {
            if (lockedButton != null)
            {
                lockedButton.minXP = player.reqXP;
                lockedButton.gameObject.SetActive(true);
            }
            if (player.goldPrice < 0)
            {
                if (buyButtonCash != null)
                {
                    buyButtonCash.SetActive(true);
                }
            }
            else if (buyButton != null)
            {
                buyButton.SetActive(true);
            }
            if (viewDealButton != null && !showingBackups && !showingFemales)
            {
                for (int i = 0; i < PurchaseGiver.dealPlayers.Length; i++)
                {
                    if (player.num == PurchaseGiver.dealPlayers[i])
                    {
                        viewDealButton.gameObject.SetActive(true);
                        viewDealButton.SetDeal(i);
                    }
                }
            }
        }
        if (playerStatsDisplay != null)
        {
            playerStatsDisplay.UpdateDisplay(player);
        }
        if (upgradeButton != null && Stats.GetNumWins() < 1)
        {
            upgradeButton.SetActive(false);
        }
        if (flicker)
        {
            yield return new WaitForSeconds(0.05f);
            playerVisual.gameObject.SetActive(true);
        }
        playerVisualAnim.SetTrigger("GuiAnim");
        playerVisual.SetVisual(player, characterSprites, customItems, 0);
        if (flickerWhenSettingCharacter)
        {
            flicker = true;
        }
        if (scaleCharacter)
        {
            StartCoroutine(SetScale(player.GetStatByNum(Players.SIZE)));
        }
    }

    private IEnumerator SetScale(float sizeStat)
    {
        float scale = GameRoster.GetScaleFromSizeStat((int)sizeStat);
        if (!(defaultScale <= 0f))
        {
            float displayScale = defaultScale * scale * scale;
            playerVisualAnim.gameObject.transform.localScale = new Vector3(displayScale, displayScale, displayScale);
            float defaultY = 2f;
            float multY = 130f;
            if (defaultScale > 50f)
            {
                defaultY = 40f;
                multY = 220f;
            }
            else if (defaultScale < 30f)
            {
                defaultY = -2.5f;
                multY = 80f;
            }
            float displayY = defaultY - (1f - displayScale / defaultScale) * multY;
            playerVisualAnim.gameObject.transform.localPosition = new Vector3(0f, displayY, 0f);
            float defaultHeadScale = 1.09f;
            float displayHeadScale = defaultHeadScale + 1f - displayScale / defaultScale;
            playerVisualHead.localScale = new Vector3(displayHeadScale, displayHeadScale, displayHeadScale);
            float defaultBallScale = 0.84f;
            float displayBallScale = defaultBallScale + 1f - displayScale / defaultScale;
            playerVisualBall.localScale = new Vector3(displayBallScale, displayBallScale, displayBallScale);
            yield return new WaitForSeconds(0.02f);
            playerVisualAnim.gameObject.transform.localPosition = new Vector3(0f, displayY, 0f);
        }
    }

    private void SetToEmpty(bool empty)
    {
        GameObject[] array = guiItems;
        foreach (GameObject gameObject in array)
        {
            if (gameObject != null)
            {
                gameObject.SetActive(!empty);
            }
        }
        if (playersButton != null)
        {
            playersButton.SetActive(empty);
        }
    }

    public virtual bool BuyPlayer()
    {
        int goldPrice = player.goldPrice;
        if (goldPrice < 0)
        {
            int num = goldPrice * -1;
            if (Currency.SpendCash(num))
            {
                player.Buy();
                if (topNavBar != null)
                {
                    topNavBar.UpdateCurrencyDisplay();
                }
                StartCoroutine(CompletedBuy());
                return true;
            }
            gameSounds.Play_one_dribble();
            currencyExchangeBox.gameObject.SetActive(true);
            currencyExchangeBox.SetCashPrice(num, num / 800);
            return false;
        }
        if (Currency.SpendGold(goldPrice, "player"))
        {
            player.Buy();
            if (topNavBar != null)
            {
                topNavBar.UpdateCurrencyDisplay();
            }
            StartCoroutine(CompletedBuy());
            return true;
        }
        getGoldButton.ShowGetGoldBox();
        /*AdMediation.ShowTjpInsufficientCurrency();*/
        return false;
    }

    public virtual void ExchangeGoldForCashAndBuyPlayer()
    {
        int num = player.goldPrice * -1;
        int num2 = num - Currency.GetCurrentCash();
        int cashGoldPrice = Currency.GetCashGoldPrice(num2, num / 800);
        if (Currency.SpendGold(cashGoldPrice, "gold_for_buyplayer_cash"))
        {
            currencyExchangeBox.gameObject.SetActive(false);
            Currency.AddCash(num2);
            BuyPlayer();
        }
        else
        {
            getGoldButton.ShowGetGoldBox();
            /*AdMediation.ShowTjpInsufficientCurrency();*/
        }
    }

    private IEnumerator CompletedBuy()
    {
        yield return new WaitForSeconds(0.25f);
        ShowCustomizeBoxForThisPlayer(true);
    }

    public virtual void ShowCustomizeBoxForThisPlayer(bool isNewPlayer)
    {
        gameSounds.Play_select();
        customizePlayerBox.SetActive(true);
        customizePlayerBoxHeading.SetTerm((!isNewPlayer) ? "CUSTOMIZE" : "CUSTOMIZE YOUR NEW PLAYER", null);
        customizePlayerBoxPlayerDetails.showingBackups = player.isBackup;
        customizePlayerBoxPlayerDetails.showingFemales = player.isFemale;
        StartCoroutine(customizePlayerBoxPlayerDetails.SetPlayer(player.num));
    }

    public virtual void SellPlayer()
    {
        int goldSellReward = player.goldSellReward;
        if (goldSellReward > 0)
        {
            Currency.AddGold(goldSellReward);
        }
        else
        {
            Currency.AddCash(goldSellReward * -1);
        }
        player.Sell();
        if (topNavBar != null)
        {
            topNavBar.UpdateCurrencyDisplay();
        }
    }

    public virtual void SavePlayerName()
    {
        player.SetName(fullNameInputField.text);
    }

    public virtual void GoToUpgradeScreenForThisPlayer()
    {
        sessionVars.showingBackups = showingBackups;
        sessionVars.showingFemales = player.isFemale;
        if (showingBackups)
        {
            if (sessionVars.showingFemales)
            {
                sessionVars.selectedBackupFemaleNum = player.num;
            }
            else
            {
                sessionVars.selectedBackupNum = player.num;
            }
        }
        else if (sessionVars.showingFemales)
        {
            sessionVars.selectedStarterFemaleNum = player.num;
        }
        else
        {
            sessionVars.selectedStarterNum = player.num;
        }
        TabChanger tabChanger = (TabChanger)GameObject.Find("BottomNavBar").GetComponent(typeof(TabChanger));
        gameSounds.Play_one_dribble();
        TabChanger.currentBackAction = backAction.CurrentTab;
        tabChanger.SetToScreen(screenEnum.Upgrade);
    }

    public virtual Player GetPlayer()
    {
        return player;
    }

    private void SetStars(float starVal)
    {
        Image[] array = starIconsOdd;
        foreach (Image image in array)
        {
            image.gameObject.SetActive(false);
        }
        Image[] array2 = starIconsEven;
        foreach (Image image2 in array2)
        {
            image2.gameObject.SetActive(false);
        }
        if (starVal > 5f)
        {
            starIcon.gameObject.SetActive(true);
            starValue.gameObject.SetActive(true);
            starValue.text = "X " + starVal;
            return;
        }
        starIcon.gameObject.SetActive(false);
        starValue.gameObject.SetActive(false);
        bool flag = starVal % 1f == 0f;
        int num = (int)Mathf.Floor(starVal);
        if (!flag)
        {
            num++;
        }
        bool flag2 = num % 2 == 0;
        if (starIconsEven == null || starIconsEven.Length == 0)
        {
            flag2 = false;
        }
        if (flag2)
        {
            for (int k = 0; k < starIconsEven.Length; k++)
            {
                if (k < num)
                {
                    Image image3 = starIconsEven[k];
                    image3.gameObject.SetActive(true);
                    if (flag || k + 1 < num)
                    {
                        image3.sprite = starIconSprite;
                    }
                    else
                    {
                        image3.sprite = starHalfIconSprite;
                    }
                }
            }
            return;
        }
        for (int l = 0; l < starIconsOdd.Length; l++)
        {
            if (l < num)
            {
                Image image4 = starIconsOdd[l];
                image4.gameObject.SetActive(true);
                if (flag || l + 1 < num)
                {
                    image4.sprite = starIconSprite;
                }
                else
                {
                    image4.sprite = starHalfIconSprite;
                }
            }
        }
    }
}