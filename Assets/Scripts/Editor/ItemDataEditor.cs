using UnityEngine;
using UnityEditor;
using System.Collections.Generic;
using System.Linq;
using System.IO;

public class ItemDataEditor : EditorWindow
{
    private string _itemName = "New Item";
    private ItemType _itemType = ItemType.Weapon;
    private ItemType _searchItemType = ItemType.Weapon;
    private ItemRarity _itemRarity = ItemRarity.Common;
    private Sprite _itemIcon;
    private GameObject _itemPrefab;
    private bool _isStackable = false;
    private int _maxStack = 1;
    private int _price = 100;
    private string _description = "";


    // Weapon
    private int _damage = 0;
    private int _criticalChance = 0;
    private int _criticalDamage = 0;

    // Armor
    private int _defense = 0;
    private int _health = 0;
    private int _mana = 0;

    // Consumable
    private float _coolTime = 0f;
    private List<ConsumableEffect> _effects = new List<ConsumableEffect>();

    
    private Vector2 _scrollPosition;
    private List<GenericItemDataSO> _existingItems = new List<GenericItemDataSO>();
    private GenericItemDataSO _selectedItem;
    
    private readonly string _itemFolderPath = "Assets/Resources/Item";
    
    [MenuItem("Tools/Item Data Editor")]
    public static void ShowWindow()
    {
        var window = GetWindow<ItemDataEditor>("Item Data Editor");
        window.minSize = new Vector2(600, 700);
    }
    
    private void OnEnable()
    {
        RefreshItemList();
    }
    
    private void OnGUI()
    {
        EditorGUILayout.BeginVertical();
        
        // 헤더
        GUILayout.Space(10);
        EditorGUILayout.LabelField("아이템 데이터 관리", EditorStyles.boldLabel);
        GUILayout.Space(10);
        
        EditorGUILayout.BeginHorizontal();
        
        // 왼쪽: 아이템 생성/편집
        EditorGUILayout.BeginVertical("box", GUILayout.Width(350));
        DrawItemCreationPanel();
        EditorGUILayout.EndVertical();
        
        GUILayout.Space(10);
        
        // 오른쪽: 기존 아이템 목록
        EditorGUILayout.BeginVertical("box");
        DrawItemListPanel();
        EditorGUILayout.EndVertical();
        
        EditorGUILayout.EndHorizontal();
        
        EditorGUILayout.EndVertical();
    }
    
    private void DrawItemCreationPanel()
    {
        EditorGUILayout.LabelField("Item Creation / Edit", EditorStyles.boldLabel);
        GUILayout.Space(5);
        
        // 기본 정보
        EditorGUILayout.LabelField("Basic Info", EditorStyles.label);
        _itemName = EditorGUILayout.TextField("Item Name", _itemName);
        _itemType = (ItemType)EditorGUILayout.EnumPopup("Item Type", _itemType);
        _itemRarity = (ItemRarity)EditorGUILayout.EnumPopup("Rarity", _itemRarity);
        
        DrawItemCreationPanelByType();

        GUILayout.Space(10);
        
        _itemIcon = (Sprite)EditorGUILayout.ObjectField("Icon", _itemIcon, typeof(Sprite), false);
        _itemPrefab = (GameObject)EditorGUILayout.ObjectField("Prefab", _itemPrefab, typeof(GameObject), false);
        
        GUILayout.Space(10);
        
        _isStackable = EditorGUILayout.Toggle("Stackable", _isStackable);
        
        if (_isStackable)
        {
            _maxStack = EditorGUILayout.IntField("Max Stack", _maxStack);
            _maxStack = Mathf.Max(1, _maxStack);
        }
        else
        {
            _maxStack = 1;
        }
        
        GUILayout.Space(10);
        
        _price = EditorGUILayout.IntField("Price", _price);
        _price = Mathf.Max(0, _price);
        
        EditorGUILayout.LabelField($"Sell Price: {(int)(_price * 0.5f)}");
        
        GUILayout.Space(10);
        
        // 설명
        EditorGUILayout.LabelField("Description", EditorStyles.label);
        _description = EditorGUILayout.TextArea(_description, GUILayout.Height(60));
        
        GUILayout.Space(20);
        
        // 버튼들
        EditorGUILayout.BeginHorizontal();
        
        if (GUILayout.Button("Create New Item", GUILayout.Height(30)))
        {
            CreateNewItem();
        }
        
        if (_selectedItem != null)
        {
            if (GUILayout.Button("Update Selected Item", GUILayout.Height(30)))
            {
                UpdateSelectedItem();
            }
        }
        
        EditorGUILayout.EndHorizontal();
        
        if (_selectedItem != null)
        {
            EditorGUILayout.BeginHorizontal();
            
            if (GUILayout.Button("Clear Selection"))
            {
                ClearSelection();
            }
            
            GUI.backgroundColor = Color.red;
            if (GUILayout.Button("Delete Selected Item"))
            {
                if (EditorUtility.DisplayDialog("Delete Item", 
                    $"'{_selectedItem.name}' Item을 정말 삭제하시겠습니까?", "Delete", "Cancel"))
                {
                    DeleteSelectedItem();
                }
            }
            GUI.backgroundColor = Color.white;
            
            EditorGUILayout.EndHorizontal();
        }
    }

    private void DrawItemCreationPanelByType()
    {
        if (_itemType == ItemType.Weapon)
        {
            GUILayout.Space(10);
            DrawWeaponCreationPanel();
        }
        else if (_itemType == ItemType.Armor)
        {
            GUILayout.Space(10);
            DrawArmorCreationPanel();
        }
        else if (_itemType == ItemType.Consumable)
        {
            GUILayout.Space(10);
            DrawConsumableCreationPanel();
        }
    }

    // 무기 아이템 패널
    private void DrawWeaponCreationPanel()
    {
        EditorGUILayout.LabelField("Weapon", EditorStyles.boldLabel);
        _damage = EditorGUILayout.IntField("Damage", _damage);
        _criticalChance = EditorGUILayout.IntField("Critical Chance", _criticalChance);
        _criticalDamage = EditorGUILayout.IntField("Critical Damage", _criticalDamage);
    }

    // 방어구 아이템 패널
    private void DrawArmorCreationPanel()
    {
        EditorGUILayout.LabelField("Armor", EditorStyles.boldLabel);
        _defense = EditorGUILayout.IntField("Defense", _defense);
        _health = EditorGUILayout.IntField("Health", _health);
        _mana = EditorGUILayout.IntField("Mana", _mana);
    }

    public void DrawConsumableCreationPanel()
    {
        EditorGUILayout.LabelField("Consumable", EditorStyles.boldLabel);
        _coolTime = EditorGUILayout.FloatField("Cool Time", _coolTime);
        
        GUILayout.Space(10);
        
        // 효과 목록
        EditorGUILayout.LabelField("Effects", EditorStyles.boldLabel);
        
        if (_effects == null)
            _effects = new List<ConsumableEffect>();
        
        for (int i = 0; i < _effects.Count; i++)
        {
            EditorGUILayout.BeginVertical("box");
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField($"Effect {i + 1}", EditorStyles.boldLabel);
            
            if (GUILayout.Button("Remove", GUILayout.Width(60)))
            {
                _effects.RemoveAt(i);
                break;
            }
            EditorGUILayout.EndHorizontal();
            
            if (_effects[i] == null)
                _effects[i] = new ConsumableEffect();
            

            EditorGUILayout.LabelField("Effect Info", EditorStyles.boldLabel);
            _effects[i].EffectType = (EffectType)EditorGUILayout.EnumPopup("Effect Type", _effects[i].EffectType);
            _effects[i].DurationType = (EffectDurationType)EditorGUILayout.EnumPopup("Duration Type", _effects[i].DurationType);
            _effects[i].Value = EditorGUILayout.FloatField("Value", _effects[i].Value);
            _effects[i].Duration = EditorGUILayout.FloatField("Duration", _effects[i].Duration);
            _effects[i].IsPercentage = EditorGUILayout.Toggle("Is Percentage", _effects[i].IsPercentage);

            GUILayout.Space(10);
            EditorGUILayout.LabelField("Effect Display Info", EditorStyles.boldLabel);
            _effects[i].EffectName = EditorGUILayout.TextField("Effect Name", _effects[i].EffectName);
            _effects[i].Description = EditorGUILayout.TextField("Description", _effects[i].Description);
            _effects[i].EffectIcon = (Sprite)EditorGUILayout.ObjectField("Effect Icon", _effects[i].EffectIcon, typeof(Sprite), false);
            
            EditorGUILayout.EndVertical();
            GUILayout.Space(5);
        }
        
        if (GUILayout.Button("Add Effect"))
        {
            _effects.Add(new ConsumableEffect());
        }
    }
    
    // 아이템 목록 패널
    private void DrawItemListPanel()
    {   
        EditorGUILayout.BeginHorizontal();
        EditorGUILayout.LabelField("Existing Item List", EditorStyles.boldLabel);

        _searchItemType = (ItemType)EditorGUILayout.EnumPopup("Item Type", _searchItemType);
        
        if (GUILayout.Button("Search", GUILayout.Width(80)))
        {
            RefreshItemList();
        }
        EditorGUILayout.EndHorizontal();
        
        GUILayout.Space(5);
        
        EditorGUILayout.LabelField($"Total {_existingItems.Count} items");
        
        _scrollPosition = EditorGUILayout.BeginScrollView(_scrollPosition, GUILayout.Height(500));
        
        foreach (var item in _existingItems)
        {
            if (item == null) continue;
            
            
            // 선택된 아이템 하이라이트
            if (_selectedItem == item)
            {
                GUI.backgroundColor = Color.cyan;
                EditorGUILayout.BeginVertical("box");
                GUI.backgroundColor = Color.white;
            }
            else
            {
                EditorGUILayout.BeginVertical();
            }
            
            EditorGUILayout.BeginHorizontal();
            
            // 아이콘
            if (item.Icon != null)
            {
                var iconRect = GUILayoutUtility.GetRect(32, 32, GUILayout.Width(32), GUILayout.Height(32));
                EditorGUI.DrawPreviewTexture(iconRect, item.Icon.texture);
            }
            else
            {
                EditorGUILayout.LabelField("No Image", GUILayout.Width(32), GUILayout.Height(32));
            }
            
            // 정보
            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField($"{item.ItemName} (ID: {item.ItemID})", EditorStyles.boldLabel);
            EditorGUILayout.LabelField($"{item.Type} | {item.Rarity} | {item.Price}Gold");
            if (!string.IsNullOrEmpty(item.Description))
            {
                EditorGUILayout.LabelField(item.Description, EditorStyles.miniLabel);
            }
            EditorGUILayout.EndVertical();
            
            EditorGUILayout.EndHorizontal();
            
            // 버튼
            EditorGUILayout.BeginHorizontal();
            
            if (GUILayout.Button("Edit"))
            {
                Selection.activeObject = item;
                EditorGUIUtility.PingObject(item);
                SelectItem(item);
            }
            
            if (GUILayout.Button("Duplicate"))
            {
                CopyItem(item);
            }
            
            EditorGUILayout.EndHorizontal();
            


            EditorGUILayout.EndVertical();
            
            GUILayout.Space(5);
        }
        
        EditorGUILayout.EndScrollView();
    }
    
    private void CreateNewItem()
    {
        string itemCreatePath = _itemFolderPath + "/" + _itemType.ToString();
        
        // 폴더 생성
        if (!Directory.Exists(itemCreatePath))
        {
            Directory.CreateDirectory(itemCreatePath);
            AssetDatabase.Refresh();
        }
        
        // 타입에 따른 적절한 ScriptableObject 생성
        GenericItemDataSO newItem = CreateItemByType(_itemType);
        
        // ID 생성
        int newId = CreateAvailableID();
        
        // 기본 정보 설정
        newItem.ItemID = newId;
        newItem.ItemName = _itemName;
        newItem.Type = _itemType;
        newItem.Rarity = _itemRarity;
        newItem.Icon = _itemIcon;
        newItem.Prefab = _itemPrefab;
        newItem.IsStackable = _isStackable;
        newItem.MaxStack = _maxStack;
        newItem.Price = _price;
        newItem.Description = _description;
        
        // 타입별 속성 설정
        SetItemTypeSpecificProperties(newItem);
        
        // 파일명 생성
        string fileName = $"{_itemType}_{newId}.asset";
        string assetPath = Path.Combine(itemCreatePath, fileName);
        
        // 에셋 생성
        AssetDatabase.CreateAsset(newItem, assetPath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        
        Debug.Log($"New Item Created: {_itemName} (ID: {newId}) - Type: {_itemType}");
        
        RefreshItemList(); // 목록 새로고침

        Selection.activeObject = newItem; // 생성된 아이템 선택
        EditorGUIUtility.PingObject(newItem); // Project 창에서 생성된 파일 깜빡임 효과
    }
    
    private GenericItemDataSO CreateItemByType(ItemType itemType)
    {
        return itemType switch
        {
            ItemType.Weapon => ScriptableObject.CreateInstance<WeaponItemDataSO>(),
            ItemType.Armor => ScriptableObject.CreateInstance<ArmorItemDataSO>(),
            ItemType.Consumable => ScriptableObject.CreateInstance<ConsumableItemDataSO>(),
            _ => ScriptableObject.CreateInstance<GenericItemDataSO>()
        };
    }
    
    private void SetItemTypeSpecificProperties(GenericItemDataSO item)
    {
        switch (item.Type)
        {
            case ItemType.Weapon:
                if (item is WeaponItemDataSO weaponItem)
                {
                    weaponItem.Damage = _damage;
                    weaponItem.CriticalChance = _criticalChance;
                    weaponItem.CriticalDamage = _criticalDamage;
                }
                break;
                
            case ItemType.Armor:
                if (item is ArmorItemDataSO armorItem)
                {
                    armorItem.Defense = _defense;
                    armorItem.Health = _health;
                    armorItem.Mana = _mana;
                }
                break;
                
            case ItemType.Consumable:
                if (item is ConsumableItemDataSO consumableItem)
                {
                    consumableItem.coolTime = _coolTime;

                    consumableItem.effects = new List<ConsumableEffect>();
                    foreach (var effect in _effects)
                    {
                        consumableItem.effects.Add(effect?.Clone());
                    }
                }
                break;
        }
    }
    
    private void UpdateSelectedItem()
    {
        if (_selectedItem == null) return;
        
        _selectedItem.ItemName = _itemName;
        _selectedItem.Type = _itemType;
        _selectedItem.Rarity = _itemRarity;
        _selectedItem.Icon = _itemIcon;
        _selectedItem.Prefab = _itemPrefab;
        _selectedItem.IsStackable = _isStackable;
        _selectedItem.MaxStack = _maxStack;
        _selectedItem.Price = _price;
        _selectedItem.Description = _description;
        
        // 타입별 속성 업데이트
        SetItemTypeSpecificProperties(_selectedItem);
        
        EditorUtility.SetDirty(_selectedItem);
        AssetDatabase.SaveAssets();
        
        Debug.Log($"Item Updated: {_itemName}");
        RefreshItemList();
    }
    
    private void SelectItem(GenericItemDataSO item)
    {
        _selectedItem = item;
        
        _itemName = item.ItemName;
        _itemType = item.Type;
        _itemRarity = item.Rarity;
        _itemIcon = item.Icon;
        _itemPrefab = item.Prefab;
        _isStackable = item.IsStackable;
        _maxStack = item.MaxStack;
        _price = item.Price;
        _description = item.Description;
        
        // 타입별 속성 로드
        LoadItemTypeSpecificProperties(item);
        
        Repaint();
    }
    
    private void LoadItemTypeSpecificProperties(GenericItemDataSO item)
    {
        // 먼저 모든 값 초기화
        ResetTypeSpecificProperties();
        
        switch (item.Type)
        {
            case ItemType.Weapon:
                if (item is WeaponItemDataSO weaponItem)
                {
                    _damage = weaponItem.Damage;
                    _criticalChance = weaponItem.CriticalChance;
                    _criticalDamage = weaponItem.CriticalDamage;
                }
                break;
                
            case ItemType.Armor:
                if (item is ArmorItemDataSO armorItem)
                {
                    _defense = armorItem.Defense;
                    _health = armorItem.Health;
                    _mana = armorItem.Mana;
                }
                break;
                
            case ItemType.Consumable:
                if (item is ConsumableItemDataSO consumableItem)
                {
                    _coolTime = consumableItem.coolTime;
                    
                    _effects = new List<ConsumableEffect>();
                    if (consumableItem.effects != null)
                    {
                        foreach (var effect in consumableItem.effects)
                        {
                            _effects.Add(effect?.Clone());
                        }
                    }
                }
                break;
        }
    }
    
    private void ResetTypeSpecificProperties()
    {
        // Weapon
        _damage = 0;
        _criticalChance = 0;
        _criticalDamage = 0;
        
        // Armor
        _defense = 0;
        _health = 0;
        _mana = 0;
        
        // Consumable
        _coolTime = 0f;
        _effects = new List<ConsumableEffect>();
    }
    
    private void ClearSelection()
    {
        _selectedItem = null;
        
        // UI 초기화
        _itemName = "New Item";
        _itemType = ItemType.Weapon;
        _itemRarity = ItemRarity.Common;
        _itemIcon = null;
        _itemPrefab = null;
        _isStackable = false;
        _maxStack = 1;
        _price = 100;
        _description = "";
        
        // 타입별 속성 초기화
        ResetTypeSpecificProperties();
        
        Repaint();
    }
    
    private void CopyItem(GenericItemDataSO original)
    {
        SelectItem(original);
        _itemName += " (Copy)";
        _selectedItem = null;
        CreateNewItem();
    }
    
    private void DeleteSelectedItem()
    {
        if (_selectedItem == null) return;
        
        string assetPath = AssetDatabase.GetAssetPath(_selectedItem);
        AssetDatabase.DeleteAsset(assetPath);
        AssetDatabase.Refresh();
        
        ClearSelection();
        RefreshItemList();
    }
    
    private void RefreshItemList()
    {
        _existingItems.Clear();
        
        // 모든 아이템 타입의 ScriptableObject 검색
        string[] weaponGuids = AssetDatabase.FindAssets("t:WeaponItemDataSO");
        string[] armorGuids = AssetDatabase.FindAssets("t:ArmorItemDataSO");
        string[] consumableGuids = AssetDatabase.FindAssets("t:ConsumableItemDataSO");
        string[] genericGuids = AssetDatabase.FindAssets("t:GenericItemDataSO");
        
        // 모든 GUID 합치기
        var allGuids = weaponGuids.Concat(armorGuids).Concat(consumableGuids).Concat(genericGuids);
        
        foreach (string guid in allGuids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid);
            var item = AssetDatabase.LoadAssetAtPath<GenericItemDataSO>(path);
            if (item != null && item.Type == _searchItemType && !_existingItems.Contains(item))
            {
                _existingItems.Add(item);
            }
        }
        
        // ID 순으로 정렬
        _existingItems = _existingItems.OrderBy(item => item.ItemID).ToList();
    }
    
    private int CreateAvailableID()
    {
        // 타입별 ID 범위 정의
        int baseID = GetBaseIDForType(_itemType);
        int maxID = baseID + 9999; // 각 타입마다 10000개씩 할당
        
        var usedIDs = _existingItems
            .Where(item => item.Type == _itemType)
            .Select(item => item.ItemID)
            .ToHashSet();
        
        // 해당 타입 범위에서 사용 가능한 첫 번째 ID 찾기
        for (int id = baseID; id <= maxID; id++)
        {
            if (!usedIDs.Contains(id))
            {
                return id;
            }
        }
        
        // 범위 초과 시 경고
        Debug.LogError($"{_itemType} type ID range({baseID}~{maxID}) is all used!");
        return baseID; // 기본값 반환
    }
    
    private int GetBaseIDForType(ItemType type)
    {
        return type switch
        {
            ItemType.Weapon => 10000,      // 10000~19999
            ItemType.Armor => 20000,       // 20000~29999
            ItemType.Consumable => 50000,  // 50000~59999
            ItemType.Resources => 60000,   // 60000~69999
            _ => 60000 // 새로운 타입을 위한 예비 공간
        };
    }
} 