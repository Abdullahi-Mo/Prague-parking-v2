using System.Collections.Generic;

public class Configuration
{
    private int numberOfSpaces = 100;

    public int GetNumberOfSpaces()
    {
        return numberOfSpaces;
    }

    public void SetNumberOfSpaces(int value)
    {
        numberOfSpaces = value;
    }

    public Dictionary<string, VehicleTypeConfig> VehicleTypes { get; set; } = new Dictionary<string, VehicleTypeConfig>
    {
        { "CAR", new VehicleTypeConfig { MaxPerSpace = 1 } },
        { "MC", new VehicleTypeConfig { MaxPerSpace = 2 } }
    };

    public class NumberOfSpaces
    {
    }
}

public class VehicleTypeConfig
{
    public int MaxPerSpace { get; set; }
}