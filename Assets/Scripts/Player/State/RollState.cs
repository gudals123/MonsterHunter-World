using UnityEngine;

public class RollState : StateMachineBehaviour
{
    private Player _player;
    private Animator _animator;
    private float staminaCostRoll = 30f;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        _animator = animator.GetComponent<Animator>();
        _player = _animator.GetComponentInParent<Player>();
        _player.Roll();
        _player.DrainStamina(staminaCostRoll);
        _player.isRoll = true;
    }
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("IsRoll", true);
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SetBool("IsRoll", false);
        _player.isRoll = false;
    }

}

