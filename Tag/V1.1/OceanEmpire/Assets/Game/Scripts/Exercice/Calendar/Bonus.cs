[System.Serializable]
public class Bonus
{
    public float ticketMultiplier;
    public Bonus(float ticketMultiplier)
    {
        this.ticketMultiplier = ticketMultiplier;
    }

    public static Bonus Join(Bonus a, Bonus b)
    {
        return new Bonus(a.ticketMultiplier * b.ticketMultiplier);
    }
}
