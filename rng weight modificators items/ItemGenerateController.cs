using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


    public class ItemGenerateController : MonoBehaviour
    {
    private List<ItemBase> _mobLoot = new();
    [SerializeField]
        private List<GameObject> listIdealObject = new();

        private ModifierGenerator _modifierGenerator;

        private ItemPlaceState _itemPlace;

        public enum ItemPlaceState
        {
            Inventory,
            Out
        }

        public enum ItemType
        {
            Gloves,
            Boots,
            Ring,
            Helm,
            Belt,
            Amulet,
            Suit,
            Ear
        }

        private ItemType _itemType;

        private void Start()
        {
        
           _modifierGenerator = new ModifierGenerator();
            _modifierGenerator.Awake();
        }


    private void Update()
    {

        if (Input.GetKeyDown("r"))
        {
            var loot = new GameObject();
            loot.name = "lutovaya";
            loot.transform.SetParent(gameObject.transform);
            loot.transform.position = gameObject.transform.position;

            loot.AddComponent<BoxCollider>();
            loot.GetComponent<BoxCollider>().size = new Vector3(5, 5, 5);
            loot.GetComponent<BoxCollider>().isTrigger = true;


            //продумать босов уников и етк шоб машстрабировалось от сложности
            var countItems = Random.Range(0, 10);

            for (int i = 0; i < 20; i++)
            {
                int randomIndex = Random.Range(0, (int)ItemGenerateController.ItemType.Boots + 1);
                ItemGenerateController.ItemType randomItemType = (ItemGenerateController.ItemType)randomIndex;

                var item = FindObjectOfType<ItemGenerateController>().CreateNewItem(randomItemType, 1, 1);
               /* var item = CreateNewItem(randomItemType, 1, 1);*/
               /* print(item);*/
                _mobLoot.Add(item.GetComponent<ItemBase>());

                item.transform.SetParent(loot.transform);

                // Добавление итемов в канвас при открытие окно лута




            }

            /*loot.GetComponent<LoadItemToCanvas>().MobItem = _mobLoot;
            loot.GetComponent<LoadItemToCanvas>()._lootCanvas = _lootPlace;*/
        }

    }
    public GameObject CreateNewItem(ItemType itemType, int worth, int floor)
        {
            _itemType = itemType;

            var mods = _modifierGenerator.GenerateMods(_itemType.ToString(), worth, floor);

            var item = CreateItemFromItemType(_itemType.ToString(), mods);

            return item;
        }

        public void MoveItem(GameObject item, GameObject destination, ItemPlaceState newState)
        {
            _itemPlace = newState;
            item.transform.SetParent(destination.transform);
        }

        private GameObject CreateItemFromItemType(string itemTypeFromSource, List<ModifierGenerator.Mod> generatedMods)
        {
        foreach (var sampleItem in listIdealObject)
        {

            if (sampleItem.GetComponent<ItemBase>().ItemType == itemTypeFromSource)
            {
                var item = Instantiate(sampleItem);
                var itemComponent = item.GetComponent<ItemBase>();

                itemComponent.GenerateUnicID();
                itemComponent.UpdateFieldFromGeneratedMods(generatedMods);

                /*item.GetComponent<Image>().sprite = itemComponent.Image;*/

                return item;
            }
        }
        return null;
        }
    }
