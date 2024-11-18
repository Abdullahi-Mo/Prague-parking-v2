public class ParkingSpot
{
    public int SpotNumber { get; private set; }
    public Vehicle ParkedVehicle { get; set; }

    public ParkingSpot(int spotNumber)
    {
        SpotNumber = spotNumber;
    }

    public bool IsAvailable()
    {
        return ParkedVehicle == null;
    }
}
