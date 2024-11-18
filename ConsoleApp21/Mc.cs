using System;

public class MC : Vehicle
{
    private const decimal PricePerHour = 10;

    public MC(string registrationNumber) : base(registrationNumber) { }

    public override decimal CalculateParkingFee()
    {
        return PricePerHour * (decimal)Math.Ceiling(GetParkingDuration().TotalHours);
    }
}