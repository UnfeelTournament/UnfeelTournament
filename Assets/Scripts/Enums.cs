using UnityEngine;
using System.Collections;

/* Enumerations */
namespace Enums
{
    public enum CharacterName
    {
        Happy,
        Sad,
        Angry
    };

    //Can use a boolean instead
    public enum CharacterFaceDirection
    {
        Right,
        Left
    }

}

/* Constants */
public static class Constants
{

    public const bool TOGGLE_WALL_HANG_JUMP = false;
    /* Characters */
    public const int DEF_START_HEALTH = 100;
    public const int DEF_START_LIVES = 3;

    public const float DEF_CHAR_MOVE_SPEED = 15f;
    public const float DEF_CHAR_JUMP_FORCE = 250f;
    public const float DEF_CHAR_MOVE_SPEED_MAX = 3f;

    public const float MIN_GROUND_CHECK = 0.25f;

    public const float PROTECT_TIMER = 1.5f;
    public const float DEATH_TIMER = 1f;

    public const float SCALE_TIMER = 3f;

    public const float DEF_SCALE = 0.18f;

    public const int BULLET_DAMAGE = 5;

    //Animation Transitions
    public const string CHAR_JUMP_TRANS_NAME = "CharacterJump";
    public const string CHAR_MOVE_TRANS_NAME = "CharacterMove";
    public const string CHAR_HIT_TRANS_NAME = "CharacterHit";
    public const string CHAR_DIE_TRANS_NAME = "CharacterDie";
    public const string CHAR_PICKUP_TRANS_NAME = "CharacterPickup";
    public const string CHAR_EQUIP_TRANS_NAME = "CharacterEquip";
    public const string CHAR_DROP_TRANS_NAME = "CharacterDrop";
    public const string CHAR_ATTACK_TRANS_NAME = "CharacterAttack";

    //Default Keys
    public const string DEF_MOVE_HORIZONTAL_AXIS = "Horizontal";
    public const string DEF_JUMP_AXIS = "Jump";
    public const string DEF_EQUIP_AXIS = "Equip";
    public const string DEF_ATTACK_AXIS = "Attack";
    public const string DEF_ATTACK_SECONDARY_AXIS = "AttackSecondary";

    //Players
    public const string PLAYER_1 = "1";

}

/* TODO:
-Have some sort of Explosion prefab for when the Bullet strikes a Player, or goes out of Range
-Have a more noticable reaction to getting hit for Player
-Update the walking animation for the Player, so that both legs move (alternating)
-Update the jumping animation for the Player, so that it bends its knees, and bends its horns slightly
-Fix the baseball bat weapon
-Find sounds for walking, jumping, getting hit, and dying
-Find sounds for picking up the cake, equipping a weapon, and picking up the star
-Find sounds for firing the gun, and swinging the bat
-Apply a small knockback to bullets and the melee swing
-Implement the Manager script, to handle transitioning between scenes, loading up levels, respawning/spawning, and checking for game over
-Implement HUD script for updating the UI. The UI will be initialized based on the Main Menu settings. The UI will be updated based on actions that occur in game.
-Find background music for each Level
-Find background music for Main Menu
-Find sounds for Menu select and move
*/