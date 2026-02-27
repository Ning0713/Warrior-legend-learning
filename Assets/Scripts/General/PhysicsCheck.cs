using System.Collections;
using System.Collections.Generic;
using System.Runtime.Remoting.Messaging;
using UnityEngine;

public class PhysicsCheck : MonoBehaviour
{
    [Header("¼ì²â²ÎÊý")]
    public Vector2 bottomOffset;
    public float checkRadius;
    public LayerMask groundLayer;

    [Header("×´Ì¬")]
    public bool isGround;
    private void Update()
    {
        check();
    }
    public void check()
    {
       isGround = Physics2D.OverlapCircle((Vector2)transform.position + transform.localScale*bottomOffset, checkRadius, groundLayer);
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere((Vector2)transform.position + transform.localScale*bottomOffset, checkRadius);
    }

}
