using KSH;
using NTJ;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public Inventory inventory;
    public PlayerStatManager statManager; // HP, Hunger, etc 관리

    private int currentDay = 1;

    void Start()
    {
        if (SaveManager.HasSavedData())
        {
            var data = SaveManager.LoadData();
            if (data != null)
            {
                LoadGameData(data);
                Debug.Log("게임 데이터 로드 완료");
            }
        }
    }

    public void SaveGame()
    {
        GameData data = new GameData();
        data.day = currentDay;

        data.hp = statManager.HP.CurrentHP;
        data.hunger = statManager.Hunger.HungerCur;
        data.thirst = statManager.Thirsty.ThirstyCur;
        data.fatigue = statManager.Fatigue.FatigueCur;

        data.inventoryItems = inventory.GetItemSaveDataList();
        data.quickSlotItemIDs = inventory.GetQuickSlotItemIDs();

        SaveManager.SaveData(data);
    }

    public void LoadGameData(GameData data)
    {
        currentDay = data.day;

        statManager.HP.CurrentHP = data.hp;
        statManager.Hunger.SetHunger(data.hunger);
        statManager.Thirsty.SetThirst(data.thirst);
        statManager.Fatigue.SetFatigue(data.fatigue);

        inventory.LoadFromItemSaveDataList(data.inventoryItems);
        inventory.LoadQuickSlotItems(data.quickSlotItemIDs);
    }
}
