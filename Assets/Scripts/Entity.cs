using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Entity : MonoBehaviour
{
    protected int maxHp;
    [SerializeField] protected int currentHp;
    //protected int attackDamage;
    //protected Vector3 startPosition;
    protected Rigidbody rigidbody;
    protected Animator animator;

    virtual public void Move(float moveSpeed, Vector2 moveInput)
    {

    }

    virtual public void Move(float moveSpeed, Transform targetPos)
    {

    }

    abstract public void Hit(int damage);
}
