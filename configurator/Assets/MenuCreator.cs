using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.Linq;


public class MenuCreator : MonoBehaviour
{
    [SerializeField] Button btnPrefab;
    [SerializeField] GameObject subMenu;
    [SerializeField] GameObject listElementPrefab;
    [SerializeField] GameObject listElementsContainer;
    MenuElement[] mainMenuArray, engineMenuArray, colorMenuArray, wheelsMenuArray, lightsMenuArray, audioMenuArray;
    MenuElement[][] submenuJaggedArray;
    [SerializeField] Color32[] bodyColor;
    [SerializeField] Material bodyMaterial;
    [SerializeField] GridLayoutGroup gridSubmenu;
    [SerializeField] GameObject[] wheelsArray;
    [SerializeField] GameObject[] lightsArray;
    [SerializeField] GameObject emailConfirmPanel;
    string[] choosenOptions;
    GameObject[] mainMenuButtons;

    float totalPrice, basePrice;
    float[] priceArray;

    void Start()
    {
        basePrice = 49000f;
        CreateMainMenu();
        
    }


    void CreateMainMenu()
    {

        subMenu.SetActive(false);
        mainMenuArray = new MenuElement[]
        {
            new MenuElement("ENGINE"),
            new MenuElement("COLOR"),
            new MenuElement("WHEELS"),
            new MenuElement("LIGHTS"),
            new MenuElement("AUDIO")
        };

        choosenOptions = new string[mainMenuArray.Length];

        priceArray = new float[mainMenuArray.Length];

        // Create the list of elements
        for(int i = 0; i < mainMenuArray.Length + 2; i++)
        {
            GameObject listElement = Instantiate(listElementPrefab);
            listElement.GetComponent<RectTransform>().SetParent(listElementsContainer.transform, false);
            Text[] txtElements = listElement.GetComponentsInChildren<Text>();
            if (i != 0 && i != mainMenuArray.Length + 1)
            {
                txtElements[0].text = mainMenuArray[i - 1].ElementName;
               
            }
            else if(i == 0)
            {
                txtElements[0].text = "CAR";
                txtElements[1].text = "Manufacturer & model";
                txtElements[2].text = basePrice.ToString();
            }
            else
            {
                txtElements[0].text = "TOTAL";
                CalculateTotalPrice();
                txtElements[2].text = totalPrice.ToString();
            }
        }

        // create main menu buttons
        for (int i = 0; i < mainMenuArray.Length; i++)
        {
            Button btn = Instantiate(btnPrefab as Button);
            btn.GetComponent<RectTransform>().SetParent(transform, false);
            btn.GetComponentInChildren<Text>().text = mainMenuArray[i].ElementName;
            int tempi = i;
            btn.onClick.AddListener(()=> CreateSubMenu(tempi));
            btn.transform.tag = "MenuButton";

        }
    }

    void UpdateMainMenuButtons()
    {
        mainMenuButtons = GameObject.FindGameObjectsWithTag("MenuButton");
        for(var i = 0; i < mainMenuButtons.Length; i++)
        if (choosenOptions[i] != null)
        {
            mainMenuButtons[i].GetComponent<Image>().color = new Color32(120, 220, 120, 255);
        }
    }
    void CreateSubMenu(int menuNumber)
    {
        engineMenuArray = new MenuElement[]
        {
            new MenuElement("1.2 fsi 110KM", 0),
            new MenuElement("1.4 fsi 160KM", 400),
            new MenuElement("1.9 TDI 160KM", 600),
            new MenuElement("2.5 TDI 210KM", 1400),
            new MenuElement("4.2 SXi 510KM", 4500)
        };

        colorMenuArray = new MenuElement[]
        {
            new MenuElement("White", 0),
            new MenuElement("Red", 300),
            new MenuElement("Blue", 200),
            new MenuElement("Silver", 200),
            new MenuElement("Black", 200),
            new MenuElement("Black ceramic coating", 400)
        };

        wheelsMenuArray = new MenuElement[]
        {
            new MenuElement("Steel rim 15\"", 0),
            new MenuElement("Alu Star rim 16\"", 300),
            new MenuElement("Alu Extra rim 17 \"", 700),
            new MenuElement("Alu RS rim 18 \"", 2300)
        };

        lightsMenuArray = new MenuElement[]
        {
            new MenuElement("Standard", 0),
            new MenuElement("Anti-fog lights F&B", 200),
            new MenuElement("AFL + Front, back - LED", 700),
            new MenuElement("AFL + Front, back - LED + smart Interior", 1200)
        };        
        
        audioMenuArray = new MenuElement[]
        {
            new MenuElement("Standard", 0),
            new MenuElement("10\" radio, 6 x speakers", 400),
            new MenuElement("12\" radio/sat nav, 8 x speakers", 700),
            new MenuElement("14\" touch radio/sat nav, 12 x speakers", 1200),
        };


        submenuJaggedArray = new MenuElement[5][];
        submenuJaggedArray[0] = engineMenuArray;
        submenuJaggedArray[1] = colorMenuArray;
        submenuJaggedArray[2] = wheelsMenuArray;
        submenuJaggedArray[3] = lightsMenuArray;
        submenuJaggedArray[4] = audioMenuArray;

        subMenu.SetActive(true);

        // Destroy all previous submenu elements
        GameObject[] submenuElements = GameObject.FindGameObjectsWithTag("SubmenuElement");
        if(submenuElements.Length > 0)
        {
            foreach(var i in submenuElements)
            {
                Destroy(i);
            }
        }

        // Create back button
        Button btnBack = Instantiate(btnPrefab as Button);
        btnBack.GetComponent<RectTransform>().SetParent(subMenu.transform, false);
        btnBack.transform.tag = "SubmenuElement";
        btnBack.GetComponentInChildren<Text>().text = "<< BACK";
        btnBack.onClick.AddListener(() => OpenMainMenu());

            gridSubmenu.cellSize = new Vector2(1920f/ (submenuJaggedArray[menuNumber].Length + 1), 200f);
        // Create submenu buttons
        for (int i = 0; i < submenuJaggedArray[menuNumber].Length; i++)
        {
            Button btn = Instantiate(btnPrefab as Button);
            btn.GetComponent<RectTransform>().SetParent(subMenu.transform, false);
            btn.transform.tag = "SubmenuElement";
            string subMenuName = submenuJaggedArray[menuNumber][i].Description;
            btn.GetComponentInChildren<Text>().text = subMenuName;
            int tempi = i;
            float tempprice = submenuJaggedArray[menuNumber][i].Price;
            btn.onClick.AddListener(() => SetChoosenOption(menuNumber,tempi, tempprice));
            //btn.GetComponent<RectTransform>().sizeDelta = new Vector2(200f, 200f);
        }
    }

    void OpenMainMenu()
    {
        subMenu.SetActive(false);
        UpdateMainMenuButtons();
    }
    void SetChoosenOption(int menuNumber, int tempOption, float price)
    {
        priceArray[menuNumber] = price;
        CalculateTotalPrice();
        print("You chose" + mainMenuArray[menuNumber].ElementName + ","+submenuJaggedArray[menuNumber][tempOption].Description + ", cost: " + price);
        print("total price: " + totalPrice);
        UpdateElement(menuNumber, tempOption);

    }

    void CalculateTotalPrice()
    {
        totalPrice = priceArray.Sum() + basePrice;
    }

    void PrepareListOfElements()
    {
        string list = "";
        for(int i = 0; i < choosenOptions.Length; i++)
        {
            list += mainMenuArray[i].ElementName + ": " + choosenOptions[i] + "\n\n";
        }

        list += "\nTOTAL PRICE: " + totalPrice;
        print(list);
        StartCoroutine(DisplayConfirmEmailPanel(list));
    }

    public void SendToEmail()
    {
        PrepareListOfElements();
    }

    IEnumerator DisplayConfirmEmailPanel(string listElements)
    {
        emailConfirmPanel.SetActive(true);
        emailConfirmPanel.GetComponentInChildren<Text>().text = listElements;
        yield return new WaitForSeconds(2);
        emailConfirmPanel.SetActive(false);

    }
    void UpdateElement(int menuNumber, int tempOption)
    {
        GameObject[] elementsList = GameObject.FindGameObjectsWithTag("ListElement");

        Text[] txtElement = elementsList[menuNumber + 1].GetComponentsInChildren<Text>();
        txtElement[0].text = mainMenuArray[menuNumber].ElementName;
        txtElement[1].text = submenuJaggedArray[menuNumber][tempOption].Description;
        txtElement[2].text = submenuJaggedArray[menuNumber][tempOption].Price.ToString();
        choosenOptions[menuNumber] = submenuJaggedArray[menuNumber][tempOption].Description;

        Text[] txtTotalArray = elementsList[elementsList.Length - 1].GetComponentsInChildren<Text>();
        txtTotalArray[0].text = "TOTAL";
        txtTotalArray[1].text = "";
        txtTotalArray[2].text = totalPrice.ToString();

        Text[] txtBaseArray = elementsList[0].GetComponentsInChildren<Text>();
        txtBaseArray[0].text = "CAR";
        txtBaseArray[1].text = "Manufacturer & model";
        txtBaseArray[2].text = basePrice.ToString();

        if(menuNumber == 1)
        {
            bodyMaterial.color = bodyColor[tempOption];
        }

        if(menuNumber == 2)
        {
            foreach(var wheel in wheelsArray)
            {
                wheel.SetActive(false);
            }
            wheelsArray[tempOption].SetActive(true);
        }

        if (menuNumber == 3)
        {
            print(menuNumber + ", " + tempOption);
            foreach (var light in lightsArray)
            {
                light.SetActive(false);
            }
            if (tempOption == 0)
            {
                lightsArray[0].SetActive(true);
            }
            else if (tempOption == 1)
            {
                lightsArray[0].SetActive(true);
                lightsArray[1].SetActive(true);
            }
            else
            {
                lightsArray[1].SetActive(true);
                lightsArray[2].SetActive(true);
            }
        }
    }

}
