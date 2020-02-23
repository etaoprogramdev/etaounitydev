using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.Advertisements;
using UnityEngine.Analytics;
//using UnityEngine.Purchasing;

public class GameManager : MonoBehaviour
{
    //public Purchaser purchaseManager;
    //Ads
    private string storeID = "3408991";
    #region Skin System
    static public int HulkPrice = 5000;
    static public int WizardPrice = 10;
    static public int BusinessPrice = 20;
    static public int CupidPrice = 10000;
    static public int DemonPrice = 60;
    static public int SamuraiPrice = 100000;
    static public int NinjaPrice = 100;
    #endregion
    #region Variable Bank and Functions
    #region Variables
    //Others
    static public bool isPaused = false;
    public GameObject gameOverMenu;
    public TextMeshProUGUI Score;
    public AudioSource menuClick;
    public AudioSource coinSound;
    public AudioSource opalSound;
    public AudioSource upgradeSound;
    public TextMeshProUGUI Money;
    public TextMeshProUGUI Currency;
    public Animator babyShowcase;
    public TextMeshProUGUI MoneyShop;
    public TextMeshProUGUI MoneyLooks;
    public GameObject shopUI;
    public GameObject shopAndSkinUI;
    public TextMeshProUGUI Thrust;
    public TextMeshProUGUI Boost;
    public TextMeshProUGUI Tough;
    public TextMeshProUGUI Bounce;
    public TextMeshProUGUI Thruststat;
    public TextMeshProUGUI Booststat;
    public TextMeshProUGUI Toughstat;
    public TextMeshProUGUI Bouncestat;
    public GameObject pauseMenu;
    public GameObject GUI;
    public GameObject LooksMenu;
    public GameObject GuideMenu;
    public GameObject UpgradeMenu;
    public Image UpgradeButton;
    public Image LooksButton;
    public Image GuideButton;
    public TextMeshProUGUI highScoreTxt;
    public SurfaceEffector2D[] floorMaterial;
    public GameObject infoMenu;
    #endregion
    //Cost
    public int thrustCost = 20;
    public int boostCost = 100;
    public int toughCost = 100;
    public int bounceCost = 150;
    //SliderUI
    public Slider boostSlider;
    public Slider thrustSlider;
    public Slider toughSlider;
    public Slider bounceSlider;
    //VariableBank
    static public float _thrust = 1000f;
    static public int _toughness = 5;
    static public float _boost = 10f;
    static public float _bounce = 1f;
    static public float _money = 0f;
    static public int _spriteIndex = 0;
    static public int _currency;
    static public int _highScore;
    #region Tag GameObjects
    [Header("Sprite Tags")]
    public GameObject HulkTag;
    public GameObject WizardTag;
    public GameObject BusinessTag;
    public GameObject CupidTag;
    public GameObject DemonTag;
    public GameObject SamuraiTag;
    public GameObject NinjaTag;
    #endregion
    #endregion
    #region Standard Methods
    public void Start()
    {
        Time.timeScale = 1f;
        BabyController.gameEnd = false;
        for (var i = 0; i < floorMaterial.Length; i++)
        {
            floorMaterial[i].speed = 10;
        }
        LoadBySerialisation();
        Advertisement.Initialize(storeID, false);
    }
    public void OnDestroy()
    {
            SaveBySerialisation();
    }
    public void OnApplicationFocus(bool focusing)
    {
        if (!focusing)
        {
            SaveBySerialisation();
        }
    }
    public void Update()
    {
        //High Score logic
        if (BabyController.scoreValue > _highScore)
        {
            _highScore = Mathf.RoundToInt(BabyController.scoreValue);
        }

        #region Hide Tags
        if (HulkPrice == 0)
        {
            HulkTag.SetActive(false);
        } else if(HulkPrice > 0)
        {
            HulkTag.SetActive(true);
        }

        if (WizardPrice == 0)
        {
            WizardTag.SetActive(false);
        }
        else if (WizardPrice > 0)
        {
            WizardTag.SetActive(true);
        }

        if (BusinessPrice == 0)
        {
            BusinessTag.SetActive(false);
        }
        else if (BusinessPrice > 0)
        {
            BusinessTag.SetActive(true);
        }

        if (CupidPrice == 0)
        {
            CupidTag.SetActive(false);
        }
        else if (CupidPrice > 0)
        {
            CupidTag.SetActive(true);
        }

        if (DemonPrice == 0)
        {
            DemonTag.SetActive(false);
        }
        else if (DemonPrice > 0)
        {
            DemonTag.SetActive(true);
        }

        if (SamuraiPrice == 0)
        {
            SamuraiTag.SetActive(false);
        }
        else if (SamuraiPrice > 0)
        {
            SamuraiTag.SetActive(true);
        }
        
        if (NinjaPrice == 0)
        {
            NinjaTag.SetActive(false);
        }
        else if (NinjaPrice > 0)
        {
            NinjaTag.SetActive(true);
        }
        #endregion
        //Declare Sprite Index
        babyShowcase.SetInteger("SpriteIndex", _spriteIndex);
        Money.text = _money.ToString();
        MoneyShop.text = _money.ToString();
        MoneyLooks.text = _money.ToString();
        Currency.text = _currency.ToString();

        if (BabyController.gameEnd)
        {
            gameOverMenu.SetActive(true);
            GUI.SetActive(false);
            Score.text = BabyController.scoreValue.ToString();
            Time.timeScale = 0f;
            for (var i = 0; i < floorMaterial.Length; i++)
            {
                floorMaterial[i].speed = 0;
            }
        }
        //Update Slider Values
        boostSlider.value = _boost;
        thrustSlider.value = _thrust;
        toughSlider.value = _toughness;
        bounceSlider.value = _bounce;
        //Update Cost
        Thrust.text = "$:" + thrustCost;
        Boost.text = "$:" + boostCost;
        Tough.text = "$:" + toughCost;
        Bounce.text = "$:" + bounceCost;
        //Update Stats
        Thruststat.text = "THRUST :   " + Mathf.Round(_thrust/ thrustSlider.maxValue * 100) +"%";
        Booststat.text = "BOOST :   " + Mathf.Round(_boost / boostSlider.maxValue * 100) + "%";
        Toughstat.text = "TOUGH :   " + Mathf.Round(_toughness / toughSlider.maxValue * 100) + "%";
        Bouncestat.text = "BOUNCE :   " + Mathf.Round(_bounce / bounceSlider.maxValue * 100) + "%";
        highScoreTxt.text = "HIGH SCORE:   " + _highScore.ToString();
    }
    private void HandleShowResult(ShowResult result)
    {
        switch (result)
        {
            case ShowResult.Finished:
                Debug.Log("The ad was successfully shown.");
                _currency += 3;
                opalSound.Play();
                break;
            case ShowResult.Skipped:
                Debug.Log("The ad was skipped before reaching the end.");
                break;
            case ShowResult.Failed:
                Debug.LogError("The ad failed to be shown.");
                AdManager.noAds = true;
                break;
        }
    }
    #endregion
    #region Save System
    public void SaveBySerialisation()
    {
        SaveState save = CreateSaveGameObject();
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream fileStream = File.Create(Application.persistentDataPath + "/gameData.ss");
        formatter.Serialize(fileStream, save);
        fileStream.Close();
    }
    public void LoadBySerialisation()
    {
        if(File.Exists(Application.persistentDataPath + "/gameData.ss"))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream fileStream = File.Open(Application.persistentDataPath + "/gameData.ss", FileMode.Open);

            SaveState save = formatter.Deserialize(fileStream) as SaveState;
            fileStream.Close();

            _thrust = save.thrust;
            _boost = save.boost;
            _toughness = save.toughness;
            _money = save.money;
            _bounce = save.bounce;
            _spriteIndex = save.spriteIndex;
            _currency = save.currency;
            thrustCost = save.thrustCost;
            boostCost = save.boostCost;
            toughCost = save.toughCost;
            bounceCost = save.bounceCost;
            _highScore = save.highScore;
            //Sprite
            HulkPrice = save.HulkPrice;
            WizardPrice = save.WizardPrice;
            BusinessPrice = save.BusinessPrice;
            CupidPrice = save.CupidPrice;
            DemonPrice = save.DemonPrice;
            SamuraiPrice = save.SamuraiPrice;
            NinjaPrice = save.NinjaPrice;

        }
    }

    private SaveState CreateSaveGameObject()
    {
        SaveState save = new SaveState();
        save.thrust = _thrust;
        save.boost = _boost;
        save.toughness = _toughness;
        save.money = _money;
        save.bounce = _bounce;
        save.spriteIndex = _spriteIndex;
        save.toughCost = toughCost;
        save.bounceCost = bounceCost;
        save.thrustCost = thrustCost;
        save.boostCost = boostCost;
        save.currency = _currency;
        save.highScore = _highScore;
        //Sprite
        save.HulkPrice = HulkPrice;
        save.WizardPrice = WizardPrice;
        save.BusinessPrice = BusinessPrice;
        save.CupidPrice = CupidPrice;
        save.DemonPrice = DemonPrice;
        save.SamuraiPrice = SamuraiPrice;
        save.NinjaPrice = NinjaPrice;
        return save;
    }
    #endregion
    //#region In Game Purchases 
    //public void AddMoney2000()
    //{
    //    if (_currency >= 10)
    //    {
    //        _money += 2000;
    //        _currency -= 10;
    //        coinSound.Play();
    //    }
    //}
    //public void AddMoney10000()
    //{
    //    if (_currency >= 100)
    //    {
    //        _money += 10000;
    //        _currency -= 100;
    //        coinSound.Play();
    //    }
    //}
    //public void AddMoney50000()
    //{
    //    if (_currency >= 500)
    //    {
    //        _money += 50000;
    //        _currency -= 500;
    //        coinSound.Play();
    //    }
    //}
    //public void Buy50Opal()
    //{
    //    purchaseManager.Buy50Opal();
    //}
    //public void Buy100Opal()
    //{
    //    purchaseManager.Buy100Opal();
    //}
    //public void Buy500Opal()
    //{
    //    purchaseManager.Buy500Opal();
    //}
    //public void Buy1000Opal()
    //{
    //    purchaseManager.Buy1000Opal();
    //}
    //#endregion
    #region Button functions
    public void PlayClick()
    {
        menuClick.Play();
    }
    public void PlayUpgrade()
    {
        upgradeSound.Play();
    }
    public void Restart()
    {
        SceneManager.LoadScene(sceneBuildIndex: 1);
        Time.timeScale = 1f;
    }
    public void Home()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene(sceneBuildIndex: 0);
    }

    public void AddToughness()
    {
        if (_money >= toughCost)
        {
            _toughness += 1;
            _money -= toughCost;
            toughCost += 300;
        }
    }
    public void OpenLeaderBoard()
    {
        PlayGamesScript.ShowLeaderBoard();
        PlayGamesScript.AddScoreToLeaderBoard(GPGSIds.leaderboard_high_score, _highScore);
    }
    public void AddBoost()
    {
        if (_money >= boostCost)
        {
            _boost += 1.0f;
            _money -= boostCost;
            boostCost += 100;
        }
    }
    public void AddThrust()
    {
        if (_money >= thrustCost)
        {
            _thrust += 5;
            _money -= thrustCost;
            thrustCost += 50;
        }
    }
    public void AddBounce()
    {
        if (_money >= bounceCost)
        {
            _bounce += 0.1f;
            _money -= bounceCost;
            bounceCost += 200;
        }
    }
    public void ScoreConvert()
    {
            _money += Mathf.Round(BabyController.scoreValue / 3);
    }
    public void CloseShop()
    {
        shopUI.SetActive(false);
    }
    public void OpenShop()
    {
        shopUI.SetActive(true);
    }
    public void OpenShopping()
    {
        shopAndSkinUI.SetActive(true);
    }
    public void CloseShopping()
    {
        shopAndSkinUI.SetActive(false);
    }
    public void ResetValues()
    {
        _thrust = 1000f;
        _boost = 10f;
        _toughness = 5;
        _money = 0;
        _currency = 0;
        _bounce = 1.0f;
        _highScore = 0;
        thrustCost = 20;
        boostCost = 100;
        toughCost = 100;
        bounceCost = 150;
        //Prices
        HulkPrice = 5000;
        WizardPrice = 10;
        BusinessPrice = 20;
        CupidPrice = 10000;
        DemonPrice = 60;
        SamuraiPrice = 100000;
        NinjaPrice = 100;
    }
    public void OpenPause()
    {
        Time.timeScale = 0f;
        pauseMenu.SetActive(true);
        isPaused = true;
    }
    public void ClosePause()
    {
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
        isPaused = false;
    }
    public void OpenLooksMenu()
    {
        UpgradeMenu.SetActive(false);
        UpgradeButton.color = new Color32(235, 235, 235, 202);
        LooksButton.color = new Color32(235, 235, 235, 255);
        LooksMenu.SetActive(true);
    }
    public void OpenInfoMenu()
    {
        infoMenu.SetActive(true);
    }
    public void CloseInfoMenu()
    {
        infoMenu.SetActive(false);
    }
    public void CloseLooksMenu()
    {
        UpgradeMenu.SetActive(true);
        UpgradeButton.color = new Color32(235, 235, 235, 255);
        LooksButton.color = new Color32(235, 235, 235, 202);
        LooksMenu.SetActive(false);
    }
    public void OpenGuideMenu()
    {
        GuideButton.color = new Color32(235, 235, 235, 255);
        GuideMenu.SetActive(true);
        UpgradeMenu.SetActive(false);
        UpgradeButton.color = new Color32(235, 235, 235, 202);
        LooksButton.color = new Color32(235, 235, 235, 202);
        LooksMenu.SetActive(false);
    }
    public void CloseGuideMenu()
    {
        GuideButton.color = new Color32(235, 235, 235, 202);
        GuideMenu.SetActive(false);
    }
    public void OpenAd()
    {
        if (Advertisement.IsReady("rewardedVideo"))
        {
            var options = new ShowOptions { resultCallback = HandleShowResult };
            Advertisement.Show("rewardedVideo", options);
        }
    }
    #endregion
    #region Sprite Buy System
    //Normal Baby
    public void NormalBabyBuy()
    {
        if (_spriteIndex != 0)
        {
            _spriteIndex = 0;
        } else if (_spriteIndex == 0)
        {
            Debug.Log("Look is already in use");
        }
    }
    public void HulkBabyBuy()
    {
        if (_spriteIndex != 1 && _money >= HulkPrice)
        {
            _spriteIndex = 1;
            _money -= HulkPrice;
            HulkPrice = 0;
        }
        else if (_spriteIndex == 1)
        {
            Debug.Log("Look is already in use");
        }
    }
    public void WizardBuy()
    {
        if (_spriteIndex != 2 && _currency >= WizardPrice)
        {
            _spriteIndex = 2;
            _currency -= WizardPrice;
            WizardPrice = 0;
        }
        else if (_spriteIndex == 2)
        {
            Debug.Log("Look is already in use");
        }
    }
    public void BusinessBuy()
    {
        if (_spriteIndex != 3 && _currency >= BusinessPrice)
        {
            _spriteIndex = 3;
            _currency -= BusinessPrice;
            BusinessPrice = 0;
        }
        else if (_spriteIndex == 3)
        {
            Debug.Log("Look is already in use");
        }
    }
    public void CupidBuy()
    {
        if (_spriteIndex != 4 && _money >= CupidPrice)
        {
            _spriteIndex = 4;
            _money -= CupidPrice;
            CupidPrice = 0;
        }
        else if (_spriteIndex == 4)
        {
            Debug.Log("Look is already in use");
        }
    }
    public void DemonBuy()
    {
        if (_spriteIndex != 5 && _currency >= DemonPrice)
        {
            _spriteIndex = 5;
            _currency -= DemonPrice;
            DemonPrice = 0;
        }
        else if (_spriteIndex == 5)
        {
            Debug.Log("Look is already in use");
        }
    }
    public void SamuraiBuy()
    {
        if (_spriteIndex != 6 && _money >= SamuraiPrice)
        {
            _spriteIndex = 6;
            _money -= SamuraiPrice;
            SamuraiPrice = 0;
        }
        else if (_spriteIndex == 6)
        {
            Debug.Log("Look is already in use");
        }
    }
    public void NinjaBuy()
    {
        if (_spriteIndex != 7 && _currency >= NinjaPrice)
        {
            _spriteIndex = 7;
            _currency -= NinjaPrice;
            NinjaPrice = 0;
        }
        else if (_spriteIndex == 7)
        {
            Debug.Log("Look is already in use");
        }
    }
    #endregion
}
