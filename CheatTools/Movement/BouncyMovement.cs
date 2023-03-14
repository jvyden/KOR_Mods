using UnityEngine;

namespace CheatTools.Movement;

public class BouncyMovement : MonoBehaviour
{
    public static BouncyMovement Instance; // BAD BAD BAD THIS SUCKS

    private Rigidbody2D _rb;
    private MovementScript _movementScript;
    
    public void Awake()
    {
        Instance = this;
        
        _rb = GetComponent<Rigidbody2D>();
        _movementScript = GetComponent<MovementScript>();
        
        _rb.constraints = RigidbodyConstraints2D.None;
        
        PhysicsMaterial2D sharedMaterial = _rb.sharedMaterial;
        sharedMaterial.bounciness = 0.5f;
        sharedMaterial.friction = 0.1f;
        _rb.sharedMaterial = sharedMaterial;

        // 5 wipes, 5hp at start, because this shit is difficult
        GameManager.Instance.wipes = 5;
        GameManager.Instance.HealthSystem.AddHP(2);
    }

    public void RunLogic(float moveHorizontal)
    {
        float force = moveHorizontal * _movementScript.speed * (GameManager.Instance.gameSpeed / 2);

        _rb.AddTorque(-force / 6, ForceMode2D.Impulse);
        _rb.AddForce(new Vector2(force / 4, 0), ForceMode2D.Impulse);
    }
}