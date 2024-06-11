using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Entity : MonoBehaviour
{
    protected int maxHp;
    protected int currentHp;
    protected int attackDamage;
    protected Vector3 startPosition;
    protected Rigidbody rigidbody;

    abstract public void Move(float moveSpeed);

    abstract public int Attack();

    abstract public void SetDamage(int damage);

    virtual public Vector3 Detect(Vector3 targetPos)
    {
        Vector3 direction = (targetPos - transform.position).normalized;

        return direction;
    }
}
