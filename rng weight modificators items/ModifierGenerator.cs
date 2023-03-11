using System.Collections.Generic;
using System.Linq;
using UnityEngine;


    public class ModifierGenerator
    {
        private ItemMods _allAvailableMods;
        private int _maxModifiersPerItem = 6;

        [SerializeField]
        private int _countMods;

        private List<int> _slotModCost = new List<int>();

        private int _unluck = -99;

        [SerializeField]
        public bool lucky, isFullFillMods;

        private List<string> _modNameList = new List<string>();
        private List<int> _totalWeight = new List<int>();
        private List<Mod> _generatedMods = new List<Mod>();

        public int valueObject;

        public void Awake()
        {
            _allAvailableMods = new ItemMods();
            _allAvailableMods.Awake();
        }

        public List<Mod> GenerateMods(string itemType, int mobValue, float floor)
        {
            ClearData();
            _generatedMods.Clear();
            floor *= 0.1f;

            if (floor < 1)
            {
                floor += 1;
            }

            valueObject = Mathf.RoundToInt(mobValue * floor);
            valueObject = 999999;

            GenerateCountMod(itemType);
            return _generatedMods;
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

        // сколько модификаторов надо скрафтить
        private void GenerateCountMod(string itemType)
        {
            var tempLuck = _unluck;

            // модифицруем неудачу, чтобы она отнимала когда - и прибавляла, когда +
            if (_unluck != 0)
            {
                if (_unluck < 0)
                {
                    _unluck *= -1;
                }
                else if (_unluck > 0)
                {
                    _unluck *= -1;
                }
            }

            // неудача будет уменьшать, а удача увеличивать
            valueObject += _unluck;

            for (int i = 0; i < 6; i++)
            {
                _slotModCost.Add(Random.Range(0, _unluck) + 200 * i);

                if (valueObject >= _slotModCost[i])
                {
                    _countMods = i;
                }
            }

            if (lucky)
            {
                _countMods++;
            }

            _unluck = tempLuck;
            GenerateNameMods(itemType);
        }

        //выбираем имя мода монеткой без рнг
        private void GenerateNameMods(string itemType)
        {
            int randomDigit = 0;
            int tempCycle = 0;

            // крутим все моды и ищем в них те, которые совпадают с получаемым itemType, boots, gloves etc
            for (int i = 0; i < _allAvailableMods.mods.Count; i++)
            {
                if (_allAvailableMods.mods[i].itemType != itemType)
                {
                    continue;
                }

                // подбрасываем монетку, чтобы получить имя мода
                // может получить countMods, а можем 0
                randomDigit = Random.Range(0, 100);
                if (randomDigit < 5 && _countMods > tempCycle)
                {
                    if (!_modNameList.Contains(_allAvailableMods.mods[i].name))
                    {
                        tempCycle++;
                        _modNameList.Add(_allAvailableMods.mods[i].name);
                    }
                }

                // isFullFillMods чтобы заполнить все модификаторы, но стоимость резко возрастает от количества модов и подброса монетки
                if (_countMods != tempCycle && isFullFillMods && i >= _allAvailableMods.mods.Count - 1)
                {
                    i = 0;
                }
            }

            // генерируем столько модов сколько получили имен
            for (int i = 0; i < _modNameList.Count; i++)
            {
                GenerateTierMods(itemType, _modNameList[i]);
            }

            //чекаем спавн модов

            /*foreach (var asd in newMod)
            {
                string modInfo = asd.itemType + " - " + asd.name + " - Tier " + asd.tier + " - Chance Tier " + asd.chanceTier + " - Bonus " + asd.bonus + " - Chance Mod " + asd.chaneMod;
                Debug.Log(modInfo);
            }
            Debug.Log("__________");*/
        }

        private void GenerateTierMods(string itemType, string nameMod)
        {
            // добавляем в список индекс элемента от которого получили весы, чтобы в конце найти необходимый элемент
            List<int> indexSelectedMods = new();

            for (int i = 0; i < _allAvailableMods.mods.Count; i++)
            {
                if (nameMod == _allAvailableMods.mods[i].name && _allAvailableMods.mods[i].itemType == itemType)
                {
                    _totalWeight.Add(_allAvailableMods.mods[i].chanceTier);
                    indexSelectedMods.Add(i);
                }
            }

            //уменьшаем\увеличиваем вес качественных модов в зависимости от удачи
            for (var i = _totalWeight.Count - 1; i >= 0; i--)
            {
                _totalWeight[i] += _unluck;
                if (i == _totalWeight.Count / 2)
                {
                    break;
                }
            }
            //уменьшаем вес хуевых модов в зависимсоти от ценности добычи
            for (int i = 0; i < _totalWeight.Count; i++)
            {
                var tempValueObj = 0;
                tempValueObj = valueObject;

                if (valueObject >= _totalWeight[i])
                {
                    tempValueObj = _totalWeight[i] - 1;
                }
                _totalWeight[i] -= tempValueObj;
                if (i == _totalWeight.Count / 2)
                {
                    break;
                }
            }

            int randomWeight = Random.Range(0, _totalWeight.Sum());

            for (int i = 0; i < _totalWeight.Count; i++)
            {
                randomWeight -= _totalWeight[i];

                if (randomWeight < 0)
                {
                    SaveMod(indexSelectedMods[i]);
                    _totalWeight.Clear();
                    return;
                }
            }
        }

        private void SaveMod(int indexToFindMod)
        {
            var mod = _allAvailableMods.mods[indexToFindMod];
            Mod temp = new Mod(mod.itemType, mod.name, mod.tier, mod.chanceTier, mod.bonus, mod.chaneMod);
            _generatedMods.Add(temp);
        }

        private void ClearData()
        {
            _modNameList.Clear();
            _slotModCost.Clear();
            _countMods = 0;
        }
    }
