using System;

public static class RandomUtils
{
    private static readonly Random random = new Random();

    // 生成一个指定范围内的随机整数
    public static int GetRandomInt(int minValue, int maxValue)
    {
        return random.Next(minValue, maxValue);
    }

    // 生成一个指定范围内的随机浮点数
    public static float GetRandomFloat(float minValue, float maxValue)
    {
        return (float)(random.NextDouble() * (maxValue - minValue) + minValue);
    }

    // 生成一个指定范围内的随机布尔值
    public static bool GetRandomBool()
    {
        return random.Next(2) == 0;
    }
}
