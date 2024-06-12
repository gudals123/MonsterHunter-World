using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Entity : MonoBehaviour
{
    public int maxHp { get; protected set; }
    public int currentHp { get; protected set; }
    //public int attackDamage { get; protected set; }
    //public Vector3 startPosition { get; protected set; }
    public Rigidbody rigidbody { get; protected set; }
    public Animator animator { get; protected set; }


    virtual public void Move(float moveSpeed, Vector2 moveInput)
    {

    }
    virtual public void Move(float moveSpeed, Vector3 targetPos)
    {

    }
    abstract public int Attack();

    abstract public void Hit(int damage);


}