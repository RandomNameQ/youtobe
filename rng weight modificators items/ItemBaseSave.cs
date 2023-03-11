using System;
using System.Reflection;
using UnityEngine;



    [System.Serializable]
    public class ItemBaseSave:MonoBehaviour
    {
        public string unicId;
        public string itemType;
        public string itemPlaceName;
        public string itemPlaceState;

        // gloves
        public int decreasedXRecoil;

        public int decreasedYRecoil;
        public int firstXShotsNoRecoil;
        public int concentration;
        public int immunity;
        public int reloadSpeed;
        public int weaponChangeSpeed;
        public int critChance;
        public int autoAim;
        public int firstXShotsIncreasedDamage;

        // boots
        public int movementSpeed;

        public int powerConsumption;
        public int movingNoise;
        public int endurance;
        public int health;
        public int amountOfEnergy;
        public int staminaRecoveryRate;
        public int energyRecoveryByWalking;
        public int chanceToDodgeDamage;

    public void SetItemStats(ItemBase item, SaveManager saveManager)
    {
       /* if (saveManager == null)
        {
            print("agde");
            return;
        }

        var saveFields = this.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);
        var itemFields = item.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);

        foreach (var itemField in itemFields)
        {
            foreach (var saveField in saveFields)
            {
                if (itemField.Name == saveField.Name)
                {
                    saveField.SetValue(this, itemField.GetValue(item));
                }
            }
        }

        if (string.IsNullOrEmpty(unicId))
        {
            unicId = item.UnicId;
        }

        itemType = item.ItemType.ToString();
        itemPlaceName = item.ItemPlaceName;
        itemPlaceState = item.itemPlaceState.ToString();*/


        /*saveManager.Save(this);*/
    }

    public void GetItemStats(ItemBase item)
        {
            var saveFields = this.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);
            var itemFields = item.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);

            foreach (var saveField in saveFields)
            {
                foreach (var itemField in itemFields)
                {
                    if (itemField.Name == saveField.Name)
                    {
                        itemField.SetValue(item, saveField.GetValue(this));
                    }
                }
            }

            item.UnicId = unicId;
            item.ItemType = itemType;
            item.ItemPlaceName = itemPlaceName;

            foreach (ItemBase.ItemPlaceState state in Enum.GetValues(typeof(ItemBase.ItemPlaceState)))
            {
                if (itemPlaceState==state.ToString())
                {
                    item.itemPlaceState = state;
                    break;
                }
            }
         
        }

        public void ResetFields()
        {
            var fields = GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);

            foreach (var field in fields)
            {
                field.SetValue(this, default);
            }
        }
    }
