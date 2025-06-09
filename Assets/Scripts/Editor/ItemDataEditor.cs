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
        
        // 새 아이템 데이터 생성
        var newItem = ScriptableObject.CreateInstance<GenericItemDataSO>();
        
        // ID 생성
        int newId = CreateAvailableID();
        
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
        
        // 파일명 생성
        string fileName = $"{_itemType}_{newId}.asset";
        string assetPath = Path.Combine(itemCreatePath, fileName);
        
        // 에셋 생성
        AssetDatabase.CreateAsset(newItem, assetPath);
        AssetDatabase.SaveAssets();
        AssetDatabase.Refresh();
        
        Debug.Log($"New Item Created: {_itemName} (ID: {newId})");
        
        RefreshItemList(); // 목록 새로고침

        Selection.activeObject = newItem; // 생성된 아이템 선택
        EditorGUIUtility.PingObject(newItem); // Project 창에서 생성된 파일 깜빡임 효과
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
        
        Repaint();
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
        
        string[] guids = AssetDatabase.FindAssets("t:GenericItemDataSO"); // 모든 아이템 데이터 찾기
        foreach (string guid in guids)
        {
            string path = AssetDatabase.GUIDToAssetPath(guid); // 에셋 경로 가져오기
            var item = AssetDatabase.LoadAssetAtPath<GenericItemDataSO>(path); // 에셋 로드
            if (item != null && item.Type == _searchItemType)
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