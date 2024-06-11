using System.Collections;
using System.Collections.Generic;
using UnityEngine;

abstract public class Entity : MonoBehaviour
{
    enum State
    {
        Idle,
        Move,
        SetDamage,
        Dead,
        Attack,
        Roll,
        Fall,
        Walk,
        Run,
        WeaponSheath,
        Tracking,
        Sturn,
        Roar
    }

    protected int HP;
    //private State state;
    protected Vector3 startPosition;

    abstract public void Move();

    abstract public void Attack();

    abstract public void SetDamage();

    virtual public void Detect()
    {

    }
}
