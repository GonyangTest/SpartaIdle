using UnityEngine;

/// <summary>
/// Inspector에서 Rect를 쉽게 설정할 수 있는 컴포넌트
/// </summary>
public class SpawnAreaRect : MonoBehaviour
{
    [Header("스폰 영역 설정")]
    public Rect spawnArea = new Rect(GameConstants.Spawn.DEFAULT_SPAWN_AREA_X, GameConstants.Spawn.DEFAULT_SPAWN_AREA_Y, GameConstants.Spawn.DEFAULT_SPAWN_AREA_WIDTH, GameConstants.Spawn.DEFAULT_SPAWN_AREA_HEIGHT);
    public float spawnHeight = 0f;
    
    [Header("시각화")]
    public Color gizmoColor = Color.green;
    public bool showGizmo = true;
    
    /// <summary>
    /// 이 영역에서 랜덤 위치 반환
    /// </summary>
    public Vector3 GetRandomSpawnPosition()
    {
        return SpawnUtility.GetRandomPositionInRect(spawnArea, spawnHeight);
    }
    
    private void OnDrawGizmos()
    {
        if (!showGizmo) return;
        
        Gizmos.color = gizmoColor;
        
        Vector3 center = new Vector3(spawnArea.center.x, spawnHeight, spawnArea.center.y);
        Vector3 size = new Vector3(spawnArea.width, 0.1f, spawnArea.height);
        
        Gizmos.DrawWireCube(center, size);
        
        // 반투명 영역 표시
        Gizmos.color = new Color(gizmoColor.r, gizmoColor.g, gizmoColor.b, 0.2f);
        Gizmos.DrawCube(center, size);
        
        #if UNITY_EDITOR
        UnityEditor.Handles.Label(center + Vector3.up, gameObject.name);
        #endif
    }
} 