using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Entity : MonoBehaviour
{
    protected int maxHp;
    [SerializeField] public int currentHp;
    //protected int attackDamage;
    //protected Vector3 startPosition;
    protected Rigidbody rigidbody;
    protected Animator animator;

    virtual public void Move(float moveSpeed, Vector2 moveInput)
    {

    }

    virtual public void Move(float moveSpeed, Vector3 targetPos)
    {

    }

    abstract public void Hit(int damage);
}
