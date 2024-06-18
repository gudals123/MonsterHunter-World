public class AnjanathController : AIController
{
    public AnjanathBT anjanathBT;
    private Anjanath anjanath;

    protected bool checkTarget;

    private void Awake()
    {
        anjanathBT = GetComponent<AnjanathBT>();
        anjanath = GetComponent<Anjanath>();
    }

    private void Update()
    {
        if(anjanath.leaveHere) 
        {
            anjanath.LeaveHere();
        }

        else if(anjanath.currentHp <= 0) 
        {
            anjanathBT.anjanathState = State.Dead;
        }

        else if (anjanath.getHit)
        {
            anjanathBT.anjanathState = State.GetHit;
        }

        else if (anjanath.isSturn)
        {
            anjanathBT.anjanathState = State.Sturn;
        }

        else
        {
            anjanath.IsPlayerInRange();
        }
    }
}
