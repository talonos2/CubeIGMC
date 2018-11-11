public class SeededRandom
{
    private int randomSeed;

    private int position;
    private float[] randomValues = new float[65535];

    public SeededRandom(int randomSeed)
    {
        this.randomSeed = randomSeed;
        UnityEngine.Random.InitState(randomSeed);
        for (int x = 0; x < 65535; x++)
        {
            randomValues[x] = UnityEngine.Random.value;
        }
    }

    public float NextFloat()
    {
        return randomValues[position++];
    }

    public int NextInt(int min, int max)
    {
        return (int)(randomValues[position++] * (max-min)) + min;
    }
}