using UnityEngine;

/// <summary>
/// 게임 전반에서 사용되는 상수들을 관리하는 클래스
/// </summary>
public static class GameConstants
{
    public static class Player
    {
        public const int INITIAL_LEVEL = 1;
        public const int INITIAL_EXP = 0;
        public const int INITIAL_MAX_EXP = 100;
        public const float ATTACK_RANGE = 1.0f;
        public const int MIN_DAMAGE = 0;
    }

    public static class Stage
    {
        public const float ENEMY_CHECK_INTERVAL = 0.5f;
    }

    public static class Buff
    {
        public const float UPDATE_INTERVAL = 1.0f;
    }

    public static class Health
    {
        public const float INVULNERABILITY_DURATION = 1.0f;
    }

    public static class Currency
    {
        public const int INITIAL_GOLD = 1000;
    }

    public static class UI
    {
        public const int DEFAULT_POOL_SIZE = 10;
        public const int UI_MANAGER_POOL_SIZE = 1;
    }

    public static class Animation
    {
        public const int INVALID_COMBO_INDEX = -1;
    }

    public static class Spawn
    {
        public const float DEFAULT_SPAWN_AREA_X = -10f;
        public const float DEFAULT_SPAWN_AREA_Y = -10f;
        public const float DEFAULT_SPAWN_AREA_WIDTH = 20f;
        public const float DEFAULT_SPAWN_AREA_HEIGHT = 20f;
    }

    public static class Physics
    {
        public const float DEFAULT_DRAG = 0.3f;
    }

} 