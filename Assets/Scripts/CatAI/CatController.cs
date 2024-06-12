using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CleverCrow.Fluid.BTs.Tasks;
using CleverCrow.Fluid.BTs.Trees;

public class CatController : AIController
{
    public enum CatState
    {
        Idle,
        Move,
        Hit,
        Dead,
        Detect,
        Tracking,
        Attack,
        Skill
    }

    private int attackPriority;
    public CatState catState;

    //private AIController catTree;
    [SerializeField] public Transform player;
    [SerializeField] public Transform boss;
    [SerializeField] public Transform target;

    public bool isPlayer = true;
    public bool isBoss;

    [Header("Range Info")]
    private float detectRange;
    private float interactionRange;

    public Vector3 Detect(Vector3 targetPos)
    {
        Vector3 direction = (transform.position - targetPos)/*.normalized*/;

        return direction;
    }

    public void Tracking(Transform target)
    {
        Vector3 dir = Detect(target.position);

        // ������ �������� ���� ���� ��
        if (target.CompareTag("Boss") && dir.magnitude <= detectRange)
        {
            Debug.Log("Boss, dir.magnitude <= detectRange");
            transform.position += dir;
        }
        // ������ ��ȣ�ۿ���� ���� ���� ��
        if (target.CompareTag("Boss") && dir.magnitude <= interactionRange)
        {
            Debug.Log("Boss, dir.magnitude <= interactionRange");
        }
        // �÷��̾ ��ȣ�ۿ���� ���� ���� ��
        if (target.CompareTag("Player") && dir.magnitude <= interactionRange)
        {
            Debug.Log("Player, dir.magnitude <= interactionRange");
        }
        // else �÷��̾� Ʈ��ŷ
        else
        {
            Debug.Log("else Player");
            target = player;
        }
    }
}