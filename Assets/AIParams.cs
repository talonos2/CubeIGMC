internal class AIParams
{
    internal bool robotic;
    internal float robotSpeed;
    internal bool loop;
    internal string text;

    public AIParams(string text, bool robotic, float robotSpeed, bool loop)
    {
        this.robotic = robotic;
        this.text = text;
        this.robotSpeed = robotSpeed;
        this.loop = loop;
    }
}