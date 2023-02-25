using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

using System.Collections;
using System.Data;
using UnityEngine;


public class DBWorker : MonoBehaviour
{
    // ������� ���� �����������

    public Button addButton;
    public Button changeButton;
    public Button deleteButton;
    public GameObject content;
    public GameObject prefab;
    public GameObject AddFT;
    public GameObject AddFD;
    public GameObject AddCar;
    public GameObject AddFuel;
    public TMP_Dropdown fuelType;
    public TMP_InputField FDName;
    public TMP_InputField FDSpeed;
    public TMP_InputField FTName;
    public TMP_InputField FTVolume;
    public TMP_InputField carName;
    public TMP_InputField carVolume;
    public TMP_Dropdown carFuelType;
    public TMP_InputField fuelName;
    public TMP_InputField fuelPrice;
<<<<<<< Updated upstream
    public List<string> fuelTypeList;//= new List<string> { "ÐÐ-92", "ÐÐ-95" };

    

    public List<EntityInterface> fuelDB = new List<EntityInterface>();
    public List<EntityInterface> carDB = new List<EntityInterface>();
    public List<EntityInterface> fuelTanksDB = new List<EntityInterface>();
    public List<EntityInterface> fuelDispenserDB = new List<EntityInterface>();

=======
    public List<string> fuelTypeList;//= new List<string> { "АИ-92", "АИ-95" };
    public DBTest db;
>>>>>>> Stashed changes

    string stringtochange;
    bool toChange { set; get; }
    private void Awake()
    {
        addButton = GameObject.Find("AddButton").GetComponent<Button>();
        changeButton = GameObject.Find("ChangeButton").GetComponent<Button>();
        deleteButton = GameObject.Find("DeleteButton").GetComponent<Button>();
    }

    public void setToChange(bool value) {
        toChange = value;
    }
    public void changeAddButton(string item) {
        Debug.Log(item);
        addButton.GetComponentInChildren<TextMeshProUGUI>().text = "Добавить " + item; 
    }
    public void deleteComponent()
    {
        int type = prefab.GetComponentsInChildren<ObjectPars>()[0].type;
        string tab ="";
        int depend=0;
        switch(type){
            case 0:{
                tab="Car";
                break;
            }
            case 1:{
                tab="FuelTank";
                
                break;
            }
            case 2:{
                tab="TRK";
                break;
            }
            case 3:{
                tab="Ftype";
                depend = int.Parse(DBManager.ExecuteQueryWithAnswer($"Select count(car_id) from Car where car_ftype_id={prefab.GetComponentsInChildren<ObjectPars>()[0].id}"))+int.Parse(DBManager.ExecuteQueryWithAnswer($"Select count(ftank_id) from FuelTank where ftank_ftype_id={prefab.GetComponentsInChildren<ObjectPars>()[0].id}"));
                break;
            }
        }
        if (depend==0)
        {
            DBManager.ExecuteQueryWithoutAnswer($"Delete from {tab}  where ftank_id={prefab.GetComponentsInChildren<ObjectPars>()[0].id}");
            Destroy(prefab);
            changeButton.GetComponentInChildren<TextMeshProUGUI>().text = "Изменить";
            deleteButton.GetComponentInChildren<TextMeshProUGUI>().text = "Удалить";
            addButton.GetComponentInChildren<TextMeshProUGUI>().text = "Добавить";
        }
        else{
            //место для ошибки
        }
        
    }
    public void changeChangeButton(TextMeshProUGUI item)
    {
        changeButton.GetComponentInChildren<TextMeshProUGUI>().text = "Изменить " + item.text;        
        deleteButton.GetComponentInChildren<TextMeshProUGUI>().text = "удалить " + item.text;        
    }

    public void openAddPanel()
    {
        string type = addButton.GetComponentInChildren<TextMeshProUGUI>().text.Split(" ")[1];
        switch (type){
            case "ТБ":
                AddFT.SetActive(true);
                break;
            case "ТРК":
                AddFD.SetActive(true);
                break;
            case "тип":
                AddFuel.SetActive(true);
                break;
            case "автомобиль":
                AddCar.SetActive(true);
                break;
        }
    }

    public void openChangePanel()
    {
        string type = addButton.GetComponentInChildren<TextMeshProUGUI>().text.Split(" ")[1];
        switch (type)
        {
            case "ТБ":
                AddFT.SetActive(true);
                FTName.text = prefab.GetComponentsInChildren<LayoutElement>()[0].GetComponentInChildren<TextMeshProUGUI>().text;
                FTVolume.text = prefab.GetComponentsInChildren<TextMeshProUGUI>()[3].text.Split(" ")[0];
                fuelType.value = db.Fhelp.IndexOf((prefab.GetComponentsInChildren<TextMeshProUGUI>()[4].text));
                break;          
            case "ТРК":
                AddFD.SetActive(true);
                FDName.text = prefab.GetComponentsInChildren<LayoutElement>()[0].GetComponentInChildren<TextMeshProUGUI>().text;
                FDSpeed.text = prefab.GetComponentsInChildren<TextMeshProUGUI>()[2].text.Split(" ")[0];
                break;
            case "тип":
                AddFuel.SetActive(true);
                fuelName.text = prefab.GetComponentsInChildren<LayoutElement>()[0].GetComponentInChildren<TextMeshProUGUI>().text;
                stringtochange = fuelName.text;
                fuelPrice.text = prefab.GetComponentsInChildren<TextMeshProUGUI>()[2].text.Split(" ")[0];
                break;
            case "автомобиль":
                AddCar.SetActive(true);
                carName.text = prefab.GetComponentsInChildren<LayoutElement>()[0].GetComponentInChildren<TextMeshProUGUI>().text;
                carVolume.text = prefab.GetComponentsInChildren<TextMeshProUGUI>()[3].text.Split(" ")[0];
                carFuelType.value = db.Fhelp.IndexOf((prefab.GetComponentsInChildren<TextMeshProUGUI>()[4].text));
                break;
        }
    }
    public void addTRK()
    {
        if (toChange) {
            DBManager.ExecuteQueryWithoutAnswer($"UPDATE FuelTank set ftank_name='{FTName.text}',ftank_volume={int.Parse(FTVolume.text)},ftank_ftype_id={db.FuelList[fuelType.value].id} where ftank_id={prefab.GetComponentsInChildren<ObjectPars>()[0].id}");
            prefab.GetComponentsInChildren<ObjectPars>()[0].name = FTName.text;
            prefab.GetComponentsInChildren<ObjectPars>()[0].par1 = int.Parse(FTVolume.text);
            prefab.GetComponentsInChildren<ObjectPars>()[0].fuel_id = db.FuelList[fuelType.value].id;
            prefab.GetComponentsInChildren<ObjectPars>()[0].fuel_name = db.FuelList[fuelType.value].name;
            prefab.GetComponentsInChildren<LayoutElement>()[0].GetComponentInChildren<TextMeshProUGUI>().text = FTName.text;
            prefab.GetComponentsInChildren<TextMeshProUGUI>()[3].text = FTVolume.text + " Л";
            prefab.GetComponentsInChildren<TextMeshProUGUI>()[4].text = db.FuelList[fuelType.value].name;
        } else {
            var copy = Instantiate(prefab, content.transform);
            DBManager.ExecuteQueryWithoutAnswer($"INSERT INTO FuelTank(ftank_name,ftank_volume,ftank_ftype_id) VALUES ('{FTName.text}',{int.Parse(FTVolume.text)},{db.FuelList[fuelType.value].id});");
            int ind = int.Parse(DBManager.ExecuteQueryWithAnswer("SELECT max(ftank_id) from FuelTank"));
            copy.GetComponentsInChildren<ObjectPars>()[0].id = ind;
            copy.GetComponentsInChildren<ObjectPars>()[0].name = FTName.text;
            copy.GetComponentsInChildren<ObjectPars>()[0].par1 = int.Parse(FTVolume.text);
            copy.GetComponentsInChildren<ObjectPars>()[0].fuel_id = db.FuelList[fuelType.value].id;
            copy.GetComponentsInChildren<ObjectPars>()[0].fuel_name = db.FuelList[fuelType.value].name;
            copy.GetComponentsInChildren<LayoutElement>()[0].GetComponentInChildren<TextMeshProUGUI>().text = FTName.text;
<<<<<<< Updated upstream
            copy.GetComponentsInChildren<TextMeshProUGUI>()[3].text = FTVolume.text + " Ð";
            copy.GetComponentsInChildren<TextMeshProUGUI>()[4].text = fuelTypeList[fuelType.value];

            FuelTank newFuelTank = new FuelTank(Convert.ToInt32(FTVolume.text), fuelTypeList[fuelType.value]);
            fuelTanksDB.Add(newFuelTank);

=======
            copy.GetComponentsInChildren<TextMeshProUGUI>()[3].text = FTVolume.text + " Л";
            copy.GetComponentsInChildren<TextMeshProUGUI>()[4].text = db.FuelList[fuelType.value].name;
>>>>>>> Stashed changes
            setLinks(copy);
        }
        
        FTName.text = "";
        FTVolume.text = "";
    }
    public void addFD()
    {
        if (toChange) {
            DBManager.ExecuteQueryWithoutAnswer($"UPDATE TRK set TRK_name='{FDName.text}',TRK_speed={int.Parse(FDSpeed.text)} where TRK_id={prefab.GetComponentsInChildren<ObjectPars>()[0].id}");
            prefab.GetComponentsInChildren<ObjectPars>()[0].name = FDName.text;
            prefab.GetComponentsInChildren<ObjectPars>()[0].par1 = int.Parse(FDSpeed.text);
            prefab.GetComponentsInChildren<LayoutElement>()[0].GetComponentInChildren<TextMeshProUGUI>().text = FDName.text;
            prefab.GetComponentsInChildren<TextMeshProUGUI>()[2].text = FDSpeed.text + " Ð/Ð¡";
        } else {
            var copy = Instantiate(prefab, content.transform);
            DBManager.ExecuteQueryWithoutAnswer($"INSERT INTO TRK(TRK_name,TRK_speed) VALUES ('{FDName.text}',{int.Parse(FDSpeed.text)});");
            int ind = int.Parse(DBManager.ExecuteQueryWithAnswer("SELECT max(TRK_id) from TRK"));
            copy.GetComponentsInChildren<ObjectPars>()[0].id = ind;
            copy.GetComponentsInChildren<ObjectPars>()[0].name = FDName.text;
            copy.GetComponentsInChildren<ObjectPars>()[0].par1 = int.Parse(FDSpeed.text);
            copy.GetComponentsInChildren<LayoutElement>()[0].GetComponentInChildren<TextMeshProUGUI>().text = FDName.text;

            copy.GetComponentsInChildren<TextMeshProUGUI>()[2].text = FDSpeed.text + " �/�";

            FuelDispenser newFuelDispenser = new FuelDispenser();
            fuelDispenserDB.Add(newFuelDispenser);

            setLinks(copy);
        }
        
        FDName.text = "";
        FDSpeed.text = "";
    }
    public void addFuelType()
    {
        if (toChange) {
            DBManager.ExecuteQueryWithoutAnswer($"UPDATE Ftype set Ftype_name='{fuelName.text}',Ftype_price={int.Parse(fuelPrice.text)} where Ftype_id={prefab.GetComponentsInChildren<ObjectPars>()[0].id}");
            prefab.GetComponentsInChildren<ObjectPars>()[0].name = fuelName.text;
            prefab.GetComponentsInChildren<ObjectPars>()[0].par1 = int.Parse(fuelPrice.text);
            db.Fhelp[db.Fhelp.IndexOf(stringtochange)] = fuelName.text;
            db.ReloadFuel();
            prefab.GetComponentsInChildren<LayoutElement>()[0].GetComponentInChildren<TextMeshProUGUI>().text = fuelName.text;
            prefab.GetComponentsInChildren<TextMeshProUGUI>()[2].text = fuelPrice.text + " ÑÑÐ±.";
        } else {

<<<<<<< Updated upstream
            var copy = Instantiate(prefab, content.transform); // ������ ����� ������� (��������, ������� ��� ������ �������)
            copy.GetComponentsInChildren<LayoutElement>()[0].GetComponentInChildren<TextMeshProUGUI>().text = fuelName.text; // �������� ������, �� ������� �������� ����������� � �� ���� ����� � ��������� ��� �����
            copy.GetComponentsInChildren<TextMeshProUGUI>()[2].text = fuelPrice.text + " ���.";
            fuelTypeList.Add(fuelName.text);

            Fuel newFuel = new Fuel(fuelName.text, Convert.ToInt32(fuelPrice.text));
            fuelDB.Add(newFuel);

=======
            var copy = Instantiate(prefab, content.transform);
            DBManager.ExecuteQueryWithoutAnswer($"INSERT INTO Ftype(Ftype_name,Ftype_price) VALUES ('{fuelName.text}',{int.Parse(fuelPrice.text)});");
            int ind = int.Parse(DBManager.ExecuteQueryWithAnswer("SELECT max(Ftype_id) from Ftype"));
            copy.GetComponentsInChildren<ObjectPars>()[0].id = ind;
            copy.GetComponentsInChildren<ObjectPars>()[0].name = FDName.text;
            copy.GetComponentsInChildren<ObjectPars>()[0].par1 = int.Parse(FDSpeed.text);
            copy.GetComponentsInChildren<LayoutElement>()[0].GetComponentInChildren<TextMeshProUGUI>().text = fuelName.text;
            copy.GetComponentsInChildren<TextMeshProUGUI>()[2].text = fuelPrice.text + " руб.";
            db.ReloadFuel();
>>>>>>> Stashed changes
            setLinks(copy);

            // òåñòèðóþ çàìåíó â ñïèñêå
            //{
            //    Fuel f1 = new Fuel("1", 1);
            //    Fuel f2 = new Fuel("2", 2);
            //    Fuel f3 = new Fuel("3", 3);

            //    DBInterface.Add(f1, fuelDB);
            //    DBInterface.Add(f2, fuelDB);
            //    DBInterface.Add(f3, fuelDB);

            //    Fuel f4 = new Fuel("4", 4);
            //    DBInterface.Change(f2, f4, fuelDB);

            //    DBInterface.Delete(f3, fuelDB);
            //}

        }
        
        db.setDropDown();
        fuelName.text = "";
        fuelPrice.text = "";
    }
    public void addCar()
    {
        if (toChange) {
            DBManager.ExecuteQueryWithoutAnswer($"UPDATE Car set car_name='{carName.text}',car_volume={int.Parse(carVolume.text)},car_ftype_id={db.FuelList[fuelType.value].id} where car_id={prefab.GetComponentsInChildren<ObjectPars>()[0].id}");
            prefab.GetComponentsInChildren<ObjectPars>()[0].name = carName.text;
            prefab.GetComponentsInChildren<ObjectPars>()[0].par1 = int.Parse(carVolume.text);
            prefab.GetComponentsInChildren<ObjectPars>()[0].fuel_id = db.FuelList[fuelType.value].id;
            prefab.GetComponentsInChildren<ObjectPars>()[0].fuel_name = db.FuelList[fuelType.value].name;
            prefab.GetComponentsInChildren<LayoutElement>()[0].GetComponentInChildren<TextMeshProUGUI>().text = carName.text;
            prefab.GetComponentsInChildren<TextMeshProUGUI>()[3].text = carVolume.text + " Л";
            prefab.GetComponentsInChildren<TextMeshProUGUI>()[4].text = db.FuelList[fuelType.value].name;
        } else {
            var copy = Instantiate(prefab, content.transform);
            DBManager.ExecuteQueryWithoutAnswer($"INSERT INTO Car(car_name,car_volume,car_ftype_id) VALUES ('{carName.text}',{int.Parse(carVolume.text)},{db.FuelList[fuelType.value].id});");
            int ind = int.Parse(DBManager.ExecuteQueryWithAnswer("SELECT max(ftank_id) from FuelTank"));
            copy.GetComponentsInChildren<ObjectPars>()[0].id = ind;
            copy.GetComponentsInChildren<ObjectPars>()[0].name = carName.text;
            copy.GetComponentsInChildren<ObjectPars>()[0].par1 = int.Parse(carVolume.text);
            copy.GetComponentsInChildren<ObjectPars>()[0].fuel_id = db.FuelList[fuelType.value].id;
            copy.GetComponentsInChildren<ObjectPars>()[0].fuel_name = db.FuelList[fuelType.value].name;
            copy.GetComponentsInChildren<LayoutElement>()[0].GetComponentInChildren<TextMeshProUGUI>().text = carName.text;
<<<<<<< Updated upstream
            copy.GetComponentsInChildren<TextMeshProUGUI>()[3].text = carVolume.text + " Ð";
            copy.GetComponentsInChildren<TextMeshProUGUI>()[4].text = fuelTypeList[carFuelType.value];

            Car newCar = new Car(Convert.ToInt32(carVolume.text), fuelTypeList[carFuelType.value]);
            carDB.Add(newCar);

=======
            copy.GetComponentsInChildren<TextMeshProUGUI>()[3].text = carVolume.text + " Л";
            copy.GetComponentsInChildren<TextMeshProUGUI>()[4].text = db.FuelList[fuelType.value].name;
>>>>>>> Stashed changes
            setLinks(copy);
        }
        
    }
    public void setParent(GameObject child)
    {
        content = child.transform.parent.gameObject;
        GameObject.Find("DBWorkerMain").GetComponent<DBWorker>().content = content;

    }

    public void setPrefab(GameObject prefab) {
        GameObject.Find("DBWorkerMain").GetComponent<DBWorker>().prefab = prefab;
    }
    public void setLinks(GameObject prefab) {
        prefab.GetComponentInChildren<DBWorker>().addButton = GameObject.Find("DBWorkerMain").GetComponent<DBWorker>().addButton;
        prefab.GetComponentInChildren<DBWorker>().changeButton = GameObject.Find("DBWorkerMain").GetComponent<DBWorker>().changeButton;
        prefab.GetComponentInChildren<DBWorker>().deleteButton = GameObject.Find("DBWorkerMain").GetComponent<DBWorker>().deleteButton;
        prefab.GetComponentInChildren<DBWorker>().content = GameObject.Find("DBWorkerMain").GetComponent<DBWorker>().content;
        prefab.GetComponentInChildren<DBWorker>().prefab = GameObject.Find("DBWorkerMain").GetComponent<DBWorker>().prefab;
        prefab.GetComponentInChildren<DBWorker>().fuelType = GameObject.Find("DBWorkerMain").GetComponent<DBWorker>().fuelType;
        prefab.GetComponentInChildren<DBWorker>().carFuelType = GameObject.Find("DBWorkerMain").GetComponent<DBWorker>().carFuelType;
        prefab.GetComponentInChildren<DBWorker>().FTName = GameObject.Find("DBWorkerMain").GetComponent<DBWorker>().FTName;
        prefab.GetComponentInChildren<DBWorker>().FTVolume = GameObject.Find("DBWorkerMain").GetComponent<DBWorker>().FTVolume;
    }

    public void setContent(GameObject content) {
        this.content = content;
    }

}
