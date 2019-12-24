using System.Collections;
using System.Collections.Generic;
using System.Runtime.Serialization.Formatters.Binary;
using System.IO;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    #region Skin System
    static public int HulkPrice = 5;
    #endregion
    #region Variable Bank and Functions
    #region Variables
    //Others
    public GameObject gameOverMenu;
    public TextMeshProUGUI Score;
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
    public GameObject UpgradeMenu;
    public Image UpgradeButton;
    public Image LooksButton;
    public SurfaceEffector2D[] floorMaterial;
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
    }

    public void OnDestroy()
    {
        SaveBySerialisation();
    }
    public void Update()
    {
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
    }
    #endregion
    #region Save System
    public void SaveBySerialisation()
    {
        SaveState save = CreateSaveGameObject();
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream fileStream = File.Create(Application.dataPath + "/gameData.ss");
        formatter.Serialize(fileStream, save);
        fileStream.Close();
    }
    public void LoadBySerialisation()
    {
        if(File.Exists(Application.dataPath + "/gameData.ss"))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream fileStream = File.Open(Application.dataPath + "/gameData.ss", FileMode.Open);

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
            //Sprite
            HulkPrice = save.HulkPrice;
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
        //Sprite
        save.HulkPrice = HulkPrice;
        return save;
    }
    #endregion
    #region In Game Purchases 
    public void AddMoney2000()
    {
        if (_currency >= 10)
        {
            _money += 2000;
            _currency -= 10;
        }
    }
    public void AddMoney10000()
    {
        if (_currency >= 100)
        {
            _money += 10000;
            _currency -= 100;
        }
    }
    public void AddMoney50000()
    {
        if (_currency >= 500)
        {
            _money += 50000;
            _currency -= 500;
        }
    }
    public void Grant50Opal()
    {
        _currency += 50;
    }
    public void Grant100Opal()
    {
        _currency += 100;
    }
    public void Grant500Opal()
    {
        _currency += 500;
    }
    public void Grant1000Opal()
    {
        _currency += 1000;
    }
    #endregion
    #region Button functions
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
        _money = 100000;
        _currency = 0;
        _bounce = 1.0f;
        thrustCost = 20;
        boostCost = 100;
        toughCost = 100;
        bounceCost = 150;
        HulkPrice = 5;
}
    public void OpenPause()
    {
        Time.timeScale = 0f;
        pauseMenu.SetActive(true);
    }
    public void ClosePause()
    {
        Time.timeScale = 1f;
        pauseMenu.SetActive(false);
    }
    public void OpenLooksMenu()
    {
        UpgradeMenu.SetActive(false);
        UpgradeButton.color = new Color32(235, 235, 235, 202);
        LooksButton.color = new Color32(235, 235, 235, 255);
        LooksMenu.SetActive(true);
    }
    public void CloseLooksMenu()
    {
        UpgradeMenu.SetActive(true);
        UpgradeButton.color = new Color32(235, 235, 235, 255);
        LooksButton.color = new Color32(235, 235, 235, 202);
        LooksMenu.SetActive(false);
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
        if (_spriteIndex != 1 && _currency >= HulkPrice)
        {
            _spriteIndex = 1;
            _currency -= HulkPrice;
            HulkPrice = 0;
        }
        else if (_spriteIndex == 1)
        {
            Debug.Log("Look is already in use");
        }
    }
    #endregion
}
