using System;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;


    [System.Serializable]
    public class ItemBase : MonoBehaviour
    {
        [SerializeField]
        private List<GameObject> _accessibleInventoryPlace;

        [SerializeField]
        private Sprite _image;

        [SerializeField]
        private ItemBaseSave _itemBaseSave;

        [SerializeField]
        private string _itemPlaceName;

        [SerializeField]
        private ItemPlaceState _itemPlaceState;

        [SerializeField]
        private string _itemType;

    [SerializeField]
    private SaveManager _saveManager;

    [SerializeField]
        private string _unicID;
        public enum ItemPlaceState
        {
            OnPlayer,
            InInventory,
            Out,
            InLoot
        }

        public List<GameObject> AccessibleInventoryPlace
        {
            get { return _accessibleInventoryPlace; }
            set { _accessibleInventoryPlace = value; }
        }

        public Sprite Image
        {
            get { return _image; }
            set { _image = value; }
        }

        public string ItemPlaceName
        {
            get { return _itemPlaceName; }
            set { _itemPlaceName = value; }
        }

    public ItemPlaceState itemPlaceState
    {
        get { return _itemPlaceState; }
        set
        {
            bool isNewItem = false;
            if (_itemPlaceState != value)
            {
                if (_itemPlaceState == ItemPlaceState.InLoot)
                {
                    if (value != ItemPlaceState.InLoot)
                    {
                        isNewItem = true;
                    }
                }

                _itemPlaceState = value;
                /*GlobalEventManager.OnItemStateChanged.Invoke(this, isNewItem);*/
            }
        }
    }

    public string ItemType
        {
            get { return _itemType; }
            set { _itemType = value; }
        }

        public string UnicId
        {
            get { return _unicID; }
            set { _unicID = value; }
        }
        public void Awake()
        {
            Type type = GetType();
            _itemType = type.Name;

            _itemBaseSave = FindObjectOfType<ItemBaseSave>();
        _saveManager = FindObjectOfType<SaveManager>();
    }

        public void GenerateUnicID()
        {
            if (string.IsNullOrEmpty(_unicID))
            {
                _unicID = Guid.NewGuid().ToString();
            }
            else
            {
                print("fail");
            }
        }

        public void ShowComponentData()
        {
            // читаем поля для вывода инфы

            List<string> fieldsNameAndDigit = new List<string>();

            foreach (FieldInfo field in this.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance))
            {
                if (field.FieldType == typeof(int))
                {
                    int value = (int)field.GetValue(this);

                    if (value != 0)
                    {
                        fieldsNameAndDigit.Add($"{field.Name}: {value}");
                    }
                }
            }
            foreach (var item in fieldsNameAndDigit)
            {
                print(item);
            }

            fieldsNameAndDigit.Clear();
        }

        public void UpdateFieldFromGeneratedMods(List<ModifierGenerator.Mod> generatedMods)
        {
            // обновляем поля потомков на полученный аргумент

            FieldInfo[] fields = GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);

            foreach (FieldInfo field in fields)
            {
                string fieldName = field.Name;
                ModifierGenerator.Mod matchingMod = generatedMods.Find(mod => mod.name == fieldName);

                field.SetValue(this, matchingMod.bonus);
            }
            fields = null;
        }

        public void UpdateJsonData()
        {
            _itemBaseSave.SetItemStats(this, _saveManager);
            _itemBaseSave.ResetFields();
        }
    }
