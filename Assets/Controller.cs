using System.Globalization;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;
using System.Linq;

public class Controller : MonoBehaviour
{
    [SerializeField] GameObject inputField;
    [SerializeField] Button addButton;
    [SerializeField] Button continueButton;

    [SerializeField] TMP_Dropdown typeW;

    [SerializeField] GameObject[] listShape;

    [SerializeField] TMP_InputField airTem;
    [SerializeField] TMP_InputField relativeH;


    [SerializeField] TextMeshProUGUI resultTitle;
    [SerializeField] TextMeshProUGUI result;
    [SerializeField] TextMeshProUGUI result2;

    [SerializeField] TextMeshProUGUI waterVP;
    [SerializeField] TextMeshProUGUI saturationWaterVP;
    [SerializeField] TextMeshProUGUI aboluteH;
    [SerializeField] TextMeshProUGUI moistureVC;
    [SerializeField] TextMeshProUGUI moistureWC;


    [SerializeField] GameObject listFieldParent;

    [SerializeField] Button calButton;

    private List<GameObject> list = new List<GameObject>();
    private List<double> listInt = new List<double>();
    private int amount = 0;

    public string CURRENCY_FORMAT = "#,##0.00";
    public NumberFormatInfo NFI = new NumberFormatInfo { NumberDecimalSeparator = ",", NumberGroupSeparator = "." };

    private int type = 0;

    [SerializeField] Color[] listColor;

    //Singleton
    public static Controller Instance { get; private set; }
    private void Awake()
    {
        if(Instance != null && Instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            Instance = this;
        }
    }


    private void Start()
    {
        Clear();
        typeW.options.Clear();
        List<string> items = new List<string>();
        List<string> itemETs = new List<string>();

        items.Add("Celsius °C");
        items.Add("Fahrenheit °F");
        items.Add("Kelvin");


        foreach (var item in items)
        {
            typeW.options.Add(new TMP_Dropdown.OptionData() { text = item });
        } 

        //typeW.onValueChanged.AddListener(delegate { DropdownitemSelected(); });
        //typeET.onValueChanged.AddListener(delegate { DropdownitemSelected(); });
        typeW.value = 0;
        type = 0;
        //listShape[0].SetActive(true);
    }

    double[] poundConvert = { 1, 2204.62262, 2.20462262 };
    double[] secondConvert = {1, 60, 3600, 86400 };

    private void DropdownitemSelected()
    {
        //SwitchToVolume();
    }



    public void OnValueChanged()
    {
        if(CheckValidate())
        {
            calButton.interactable = true;
        }
        else
        {
            calButton.interactable= false;
        }
    }

    private bool CheckValidate()
    {
        if (airTem.text == "" || relativeH.text == "")
            return false;
        //return text.All(char.IsDigit);
        return true;
    }

    float[] temperature = { 1, 33.8f, 274.15f }; // C - F - K
    public void Sum()
    {
        CalWithAdult();
        listFieldParent.SetActive(true);
    }

    private void CalWithAdult()
    {
        
        double rh = double.Parse(relativeH.text);
        double T_C = double.Parse(airTem.text);

        var T = CtoF(TemtoC(T_C, typeW.value));
        var rh2 = rh * rh;
        var T2 = T * T;

        var index = -42.379f + (2.04901523d * T) + (10.14333127d * rh) - (0.22475541d * T * rh) - (6.83783d * T2 / 1000) - (5.481717d * rh2 / 100) + (1.22874d * T2 * rh / 1000) + (8.5282d * T * rh2 / 10000) - (1.99 * T2 * rh2 / 1000000);
        var C = TemtoC(index, 1);
        result.text = C.ToString("0.00") + "°C";   
        result2.text = index.ToString("0.00") + "°F" + " or " + (CtoK(C).ToString("0.00")) + "K";   


        if(C >= 27 && C < 32)
        {
            listMethods[0].SetActive(true);
        }
        else if(C >= 32 && C < 41)
        {
            listMethods[1].SetActive(true);
        }
        else if(C >= 41 && C < 54)
        {
            listMethods[2].SetActive(true);
        }
        else if(C >= 54)
        {
            listMethods[3].SetActive(true);
        }
    }

    double TemtoC(double tem, int type)
    {
        if (type == 1)
        {
            return (tem - 32) / 1.8f;
        }
        else if(type == 2)
        {
            return tem - 273.15f;
        }

        return tem;
    }

    double CtoF(double c)
    {
        return c * 1.8f + 32;
    }
    double CtoK(double c)
    {
        return c + 273.15f;
    }

    void SwitchToVolume()
    {
        for(int i = 0; i < listShape.Length; i++)
        {
            listShape[i].SetActive(i==type);
        }
    }

    double m2toft2 = 10.7639104;
    double m2toin2 = 1550.0031;


    double M2ToFt2(double m2)
    {
        return m2 * m2toft2;
    }
    
    double M2ToIn2(double m2)
    {
        return m2 * m2toin2;
    }

    public void Continue()
    {
        Clear();
    }

    [SerializeField] GameObject[] listMethods;
    public void Clear()
    {
        listFieldParent.SetActive(false);

        typeW.value = 0;
        //type2.value = 0;

        airTem.text = "";
        relativeH.text = "";

        calButton.interactable = false;
        //result2.gameObject.SetActive(false);

        for(int i = 0; i < listMethods.Length; i++)
        {
            listMethods[i].SetActive(false);
        }
    }

    public void Quit()
    {
        Clear();
        Application.Quit();
    }
}
