using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CatAI : Entity
{
    string skill;

    public override int Attack()
    {
        throw new System.NotImplementedException();
    }

    public override Vector3 Detect(Vector3 targetPos)
    {
        return base.Detect(targetPos);
    }

    public override void Move(float moveSpeed, Vector3 targetPos)
    {
        base.Move(moveSpeed, targetPos);
    }

    public override void SetDamage(int damage)
    {
        throw new System.NotImplementedException();
    }

    public void Skill()
    {
        
    }


}
