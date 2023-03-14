using System.Diagnostics.CodeAnalysis;
using UnityEngine;

namespace CheatTools.Movement;

public class MouseXMovement : MonoBehaviour
{
    [SuppressMessage("ReSharper", "Unity.InefficientPropertyAccess")]
    public void Update()
    {
        Vector3 pos = Camera.main!.ScreenToWorldPoint(Input.mousePosition);
        pos.y = transform.position.y;
        pos.z = 0;

        transform.position = pos;
    }
}