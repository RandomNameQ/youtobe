
using System.Collections.Generic;
using System.IO;
using UnityEngine;

    [System.Serializable]
    public class ItemSaveData
    {
        public List<string> savedItems;
    }

    public class SaveManager : MonoBehaviour
    {
        private string saveFolderPath = "saves/items";
        private string itemListFolderPath = "saves";
        private ItemSaveData itemSaveData;

        [SerializeField]
        private List<GameObject> _exampleItemBaseObjects = new();
        [SerializeField]
        private GameObject _canvas;

        [SerializeField]
        private ItemBaseSave _itemBaseSave;

        [SerializeField]
        private List<ItemBase> _items = new();
        [SerializeField]
        private List<string> itemIdToDelete = new();

    

        private void Update()
        {
            if (Input.GetKeyDown("x"))
            {
                _canvas.SetActive(true);
            }
            if (Input.GetKeyDown("z"))
            {
                _canvas.SetActive(false);
            }
        }

        public void Save(ItemBaseSave item)
        {
            // Serialize the item to JSON
            string json = JsonUtility.ToJson(item);

            // Write the JSON string to a file
            string filePath = Path.Combine(saveFolderPath, item.unicId + ".json");
            File.WriteAllText(filePath, json);

            // Add the item's id to the saved items list
            if (!itemSaveData.savedItems.Contains(item.unicId))
            {
               
                itemSaveData.savedItems.Add(item.unicId);
                
                if (item.itemPlaceState =="OnPlayer" || item.itemPlaceState == "InInventory")
                {
                    SaveListItems();
                }
                
            }

            // Verify that the file was created and contains the expected data
            if (File.Exists(filePath))
            {
                string fileContents = File.ReadAllText(filePath);
                Debug.Log($"Saved item {item.unicId} to file {filePath} with contents: {fileContents}");
            }
            else
            {
                Debug.LogError($"Failed to save item {item.unicId} to file {filePath}");
            }
        }

        private void SaveListItems()
        {
            if (File.Exists(itemListFolderPath))
            {
                foreach (var item in itemSaveData.savedItems)
                {
                    print(item);
                }

                string json = JsonUtility.ToJson(itemSaveData);

                File.WriteAllText(itemListFolderPath, json);
            }
            else
            {
                itemSaveData = new ItemSaveData();
                string json = JsonUtility.ToJson(itemSaveData);

                File.WriteAllText(itemListFolderPath, json);
            }
        }

        public void Load(string itemId)
        {
            string filePath = Path.Combine(saveFolderPath, itemId + ".json");

            if (File.Exists(filePath))
            {
                // Read the JSON string from the file
                string json = File.ReadAllText(filePath);
                // Deserialize the JSON string to an ItemBaseSave object
                /*_itemBaseSave = JsonUtility.FromJson<ItemBaseSave>(json);*/
                JsonUtility.FromJsonOverwrite(json, _itemBaseSave);
            }
            else
            {
                Debug.LogError($"Save file not found for item {itemId}!");
            }
        }

        private void RessurectItems()
        {
            
            if (_itemBaseSave.itemPlaceState == "Out")
            {
                itemIdToDelete.Add(_itemBaseSave.unicId);
                return;
            }


            foreach (var itemType in _exampleItemBaseObjects)
            {
                

                if (itemType.GetComponent<ItemBase>().ItemType == _itemBaseSave.itemType)
                {
                    bool isItemOnPlayer = false;
                    GameObject parentRessurectedItem = null;

                    if (_itemBaseSave.itemPlaceName == "inventory" || string.IsNullOrEmpty(_itemBaseSave.itemPlaceName))
                    {
                        parentRessurectedItem = GameObject.Find("InventoryGrid");
                    }
                    else
                    {
                        parentRessurectedItem = GameObject.Find(_itemBaseSave.itemPlaceName);
                        isItemOnPlayer = true;
                    }

                    var itemObj = Instantiate(itemType);
                    var itemComp = itemObj.GetComponent<ItemBase>();


                    if (isItemOnPlayer)
                    {
                        itemObj.transform.position = parentRessurectedItem.transform.position;
                        var mainParent = parentRessurectedItem.transform.parent;

                        itemObj.transform.SetParent(mainParent.transform);

                        parentRessurectedItem.transform.SetParent(itemObj.transform);
                        parentRessurectedItem.SetActive(false);

                        _items.Add(itemComp);
                    }
                    else
                    {
                        itemObj.transform.SetParent(parentRessurectedItem.transform);
                    }

                    _itemBaseSave.GetItemStats(itemComp);

                    break;
                }
            }
        }

        private void DeleteLostItems()
        {
            // Get a list of all JSON files in the save folder
            string[] fileNames = Directory.GetFiles(saveFolderPath, "*.json");

            // Loop through each file and check if its ID is in the saved items list
            foreach (string fileName in fileNames)
            {
                string itemId = Path.GetFileNameWithoutExtension(fileName);

                if (!itemSaveData.savedItems.Contains(itemId))
                {
                    // Delete the file
                    File.Delete(fileName);
                    /*Debug.Log($"Deleted lost save file {fileName}");*/
                }
            }




        }

        private  void ClearItemListFromOutInventory()
        {

            for (int i = 0; i < itemSaveData.savedItems.Count; i++)
            {
                foreach (var itemDelete in itemIdToDelete)
                {
                    if (itemSaveData.savedItems[i]== itemDelete)
                    {
                        itemSaveData.savedItems.RemoveAt(i);
                        break;
                    }
                }
                
            }
            SaveListItems();
        }
        
    }
