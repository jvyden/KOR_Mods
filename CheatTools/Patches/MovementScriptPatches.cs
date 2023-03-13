using System.Diagnostics.CodeAnalysis;
using System.Reflection;
using HarmonyLib;
using UnityEngine;

namespace CheatTools.Patches;

[SuppressMessage("ReSharper", "InconsistentNaming")]
public static class MovementScriptPatches
{
    private static readonly FieldInfo _movementInfo = AccessTools.Field(typeof(MovementScript), "movement");
    private static readonly FieldInfo _cosmeticSwitcherInfo = AccessTools.Field(typeof(MovementScript), "cosmeticSwitcher");
    private static readonly FieldInfo _rbInfo = AccessTools.Field(typeof(MovementScript), "rb");
    
    [HarmonyPatch(typeof(MovementScript), nameof(MovementScript.MovementFunction))]
    [HarmonyPrefix]
    public static bool BeforeMovementFunction(MovementScript __instance, float moveHorizontal)
    {
        if (!CheatToolsPlugin.ConfigBouncy.Value) return true;
        if (GameManager.Instance.GameOver) return false;

        CosmeticSwitcher cosmeticSwitcher = (CosmeticSwitcher)_cosmeticSwitcherInfo.GetValue(__instance);
        Vector3 movement = (Vector3)_movementInfo.GetValue(__instance);
        Rigidbody2D rb = (Rigidbody2D)_rbInfo.GetValue(__instance);
        
        // OriginalMovementLogic(__instance, moveHorizontal, movement, rb);
        BouncyMovementLogic(__instance, moveHorizontal, movement, rb);

        if (moveHorizontal * Mathf.Sign(moveHorizontal) > 0.2f && cosmeticSwitcher.cosmeticItem.flippable)
            __instance.transform.localScale = new Vector2(Mathf.Sign(moveHorizontal), 1f);

        return false;
    }

    private static void OriginalMovementLogic(MovementScript __instance, float moveHorizontal, Vector3 movement, Rigidbody2D rb)
    {
        rb.velocity = new Vector2(moveHorizontal * (__instance.speed * GameManager.Instance.gameSpeed), rb.velocity.y);
        _movementInfo.SetValue(__instance, movement * __instance.speed * Time.deltaTime);
        
        __instance.transform.position += movement;
    }

    private static void BouncyMovementLogic(MovementScript __instance, float moveHorizontal, Vector3 movement, Rigidbody2D rb)
    {
        rb.constraints = RigidbodyConstraints2D.None;
        
        PhysicsMaterial2D sharedMaterial = rb.sharedMaterial;
        sharedMaterial.bounciness = 1;
        sharedMaterial.friction = 1;
        
        _movementInfo.SetValue(__instance, movement * __instance.speed * Time.deltaTime);

        float force = moveHorizontal * (__instance.speed * GameManager.Instance.gameSpeed);
        
        rb.AddTorque(force / 2, ForceMode2D.Impulse);
        rb.AddForce(new Vector2(force, 0), ForceMode2D.Impulse);
    }
}