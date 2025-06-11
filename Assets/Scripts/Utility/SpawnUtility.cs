using UnityEngine;

/// <summary>
/// Rect 기반 스폰 유틸리티
/// </summary>
public static class SpawnUtility
{
    /// <summary>
    /// Rect 영역 내에서 랜덤 위치 생성 (XZ 평면)
    /// </summary>
    public static Vector3 GetRandomPositionInRect(Rect rect, float y = 0f)
    {
        float randomX = Random.Range(rect.xMin, rect.xMax);
        float randomZ = Random.Range(rect.yMin, rect.yMax);
        return new Vector3(randomX, y, randomZ);
    }
    
    /// <summary>
    /// 중심점과 크기로 Rect 영역 내에서 랜덤 위치 생성
    /// </summary>
    public static Vector3 GetRandomPositionInArea(Vector3 center, Vector2 size, float y = 0f)
    {
        Rect rect = new Rect(center.x - size.x * 0.5f, center.z - size.y * 0.5f, size.x, size.y);
        return GetRandomPositionInRect(rect, y);
    }
    
    /// <summary>
    /// 특정 위치 주변의 원형 영역에서 랜덤 위치 생성
    /// </summary>
    public static Vector3 GetRandomPositionInCircle(Vector3 center, float radius, float y = 0f)
    {
        Vector2 randomPoint = Random.insideUnitCircle * radius;
        return new Vector3(center.x + randomPoint.x, y, center.z + randomPoint.y);
    }
}
