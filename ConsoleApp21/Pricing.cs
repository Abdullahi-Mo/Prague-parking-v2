using System.Collections.Generic;

public class Pricing
{
    public Dictionary<string, int> Prices { get; set; } = new Dictionary<string, int>
    {
        { "CAR", 20 },
        { "MC", 10 }
    };
}