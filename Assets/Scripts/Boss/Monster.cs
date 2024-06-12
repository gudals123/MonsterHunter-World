using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Monster : Entity
{
    protected int grade;
    protected Vector3 targetPos;
    protected int rotationSpeed;
    protected bool canAttack;
    protected bool setHit;

    public override int Attack()
    {
        setHit = true;
        setHit = false;
        return attackDamage;
    }

    public override void Move(float moveSpeed, Vector3 targetPos)
    {
        Quaternion targetRotation = Quaternion.LookRotation(targetPos);
        Quaternion rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotationSpeed * Time.fixedDeltaTime);
        rigidbody.MoveRotation(rotation);
        rigidbody.MovePosition(transform.position + targetPos * moveSpeed * 0.1f * Time.fixedDeltaTime);
    }

    public Vector3 SetRandomPos()
    {
        float targetX = Random.Range(transform.position.x - 100, transform.position.x + 100);
        float targetZ = Random.Range(transform.position.z - 100, transform.position.z + 100);
        return new Vector3(targetX, transform.position.y, targetZ);
    }

    public override void SetDamage(int damage)
    {
        currentHp -= damage;
    }

    virtual public void DetectOpponent(Monster Opponent)
    {
        if(Opponent.grade == grade)
        {
            Debug.Log("Same Grade!!!!");
        }
    }

    public void Dead()
    {
        transform.GetChild(0).GetChild(0).gameObject.SetActive(false);  // 메쉬를 제외한 자식 오브젝트 모두 끔
        transform.position = new Vector3(transform.position.x, transform.position.y, transform.position.z);
    }

    virtual public void Idle()
    {
        animator.Play("Idle");
        targetPos = SetRandomPos();
    }

    virtual public void NomalMoving()
    {
        animator.Play("NomalWalking");
        // GroundCheck 필요
        Move(1, targetPos);
    }



}
