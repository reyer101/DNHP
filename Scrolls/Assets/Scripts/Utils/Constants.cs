using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Constants
public class Constants : MonoBehaviour {
    // Audio
    public static string FireSpellAudio = "Audio/FireSpell";
    public static string EnemySpellAudio = "Audio/EnemySpell";

    // General Animation
    private static string AnimationPrefix = "AnimationControllers/";  

    // Player Animations 
    public static string GirlPrefix = AnimationPrefix + "Girl/";
    public static string BoyPrefix = AnimationPrefix + "Boy/";
    public static string Walk = "main_walk";
    public static string Crouch = "main_crouch";
    public static string Jump = "main_jump";

    // Boss Animations    
    public static string BossPrefix = AnimationPrefix + "Kitty/";
    public static string Transform = BossPrefix + "kitty_transform";
    public static string Idle = BossPrefix + "kitty_idle";
}
