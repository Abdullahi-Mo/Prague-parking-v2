using System;

public class Car : Vehicle
{
    private const decimal PricePerHour = 20;

    public Car(string registrationNumber) : base(registrationNumber) { }

    public override decimal CalculateParkingFee()
    {
        return PricePerHour * (decimal)Math.Ceiling(GetParkingDuration().TotalHours);
    }
}
