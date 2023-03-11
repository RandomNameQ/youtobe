using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemMods 
{
    private int _maxLevel;
    private int _tierStepWeight;

    
    private ItemType itemType;
    private enum ItemType
    {
        Gloves,
        Boots
    }

    public struct Mod
    {
        public string itemType;
        public string name;
        public int tier;
        public int chanceTier;
        public int bonus;
        public int chaneMod;


        public Mod(string itemType, string name, int tier, int chanceTier, int bonus, int chaneMod)
        {
            this.itemType = itemType;
            this.name = name;
            this.tier = tier;
            this.chanceTier = chanceTier;
            this.bonus = bonus;
            this.chaneMod = chaneMod;

        }
    }

    public List<Mod> mods = new List<Mod>();
    public void Awake()
    {
        _maxLevel = 10;
        _tierStepWeight = 500;
        AddModToList();
       
    }
    public void AddModToList()
    {
        int bonus = -5;
        //тип шмотки, имя мода, лвл тира, вес спавна тира, сколько дает, вес спавнав мода
        for (int i = 0; i < _maxLevel; i++)
        {
            bonus++;
            if (bonus==0)
            {
                bonus = 1;
            }

          
    
            //boots
            mods.Add(new Mod(ItemType.Boots.ToString(), "movementSpeed", i, 5000 - (i* _tierStepWeight), bonus, 10));
            mods.Add(new Mod(ItemType.Boots.ToString(), "powerConsumption", i, 5000 - (i* _tierStepWeight), bonus, 10));
            mods.Add(new Mod(ItemType.Boots.ToString(), "movingNoise", i, 5000 - (i* _tierStepWeight), bonus, 10));
            mods.Add(new Mod(ItemType.Boots.ToString(), "endurance", i, 5000 - (i* _tierStepWeight), bonus, 10));
            mods.Add(new Mod(ItemType.Boots.ToString(), "health", i, 5000 - (i*_tierStepWeight), bonus, 10));
            mods.Add(new Mod(ItemType.Boots.ToString(), "amountOfEnergy", i, 5000 - (i* _tierStepWeight), bonus, 10));
            mods.Add(new Mod(ItemType.Boots.ToString(), "staminaRecoveryRate", i, 5000 - (i* _tierStepWeight), bonus, 10));
            mods.Add(new Mod(ItemType.Boots.ToString(), "energyRecoveryByWalking", i, 5000 - (i* _tierStepWeight), bonus, 10));
            mods.Add(new Mod(ItemType.Boots.ToString(), "chanceToDodgeDamage", i, 5000 - (i* _tierStepWeight), bonus, 10));


            //gloves
            mods.Add(new Mod(ItemType.Gloves.ToString(), "decreasedXRecoil", i, 5000 - (i * _tierStepWeight), bonus, 10));
            mods.Add(new Mod(ItemType.Gloves.ToString(), "decreasedYRecoil", i, 5000 - (i * _tierStepWeight), bonus, 10));
            mods.Add(new Mod(ItemType.Gloves.ToString(), "firstXShotsNoRecoil", i, 5000 - (i * _tierStepWeight), bonus, 10));
            mods.Add(new Mod(ItemType.Gloves.ToString(), "concentration", i, 5000 - (i * _tierStepWeight), bonus, 10));
            mods.Add(new Mod(ItemType.Gloves.ToString(), "immunity", i, 5000 - (i * _tierStepWeight), bonus, 10));
            mods.Add(new Mod(ItemType.Gloves.ToString(), "reloadSpeed", i, 5000 - (i * _tierStepWeight), bonus, 10));
            mods.Add(new Mod(ItemType.Gloves.ToString(), "weaponChangeSpeed", i, 5000 - (i * _tierStepWeight), bonus, 10));
            mods.Add(new Mod(ItemType.Gloves.ToString(), "critChance", i, 5000 - (i * _tierStepWeight), bonus, 10));
            mods.Add(new Mod(ItemType.Gloves.ToString(), "autoAim", i, 5000 - (i * _tierStepWeight), bonus, 10));
            mods.Add(new Mod(ItemType.Gloves.ToString(), "firstXShotsIncreasedDamage", i, 5000 - (i * _tierStepWeight), bonus, 10));

            




        }



    }



}
