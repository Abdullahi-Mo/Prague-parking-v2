using System;
using System.Collections.Generic;

public class ParkingGarage
{
    private int numberOfSpaces;

    public List<ParkingSpot> Spots { get; private set; }

    public ParkingGarage(Configuration.NumberOfSpaces numberOfSpaces, int numberOfSpots)
    {
        Spots = new List<ParkingSpot>(numberOfSpots);
        for (int i = 1; i <= numberOfSpots; i++)
        {
            Spots.Add(new ParkingSpot(i));
        }
    }

    public ParkingGarage(int numberOfSpaces)
    {
        this.numberOfSpaces = numberOfSpaces;
    }

    public bool ParkVehicle(Vehicle vehicle)
    {
        foreach (var spot in Spots)
        {
            if (spot.IsAvailable())
            {
                spot.ParkedVehicle = vehicle;
                return true; // Vehicle parked
            }
        }
        return false; // No available spots
    }

    public Vehicle RetrieveVehicle(int spotNumber)
    {
        var spot = Spots[spotNumber - 1];
        var vehicle = spot.ParkedVehicle;
        spot.ParkedVehicle = null; // Clear the spot
        return vehicle; // Return the retrieved vehicle
    }

    public void ShowStatus()
    {
        foreach (var spot in Spots)
        {
            string status = spot.IsAvailable() ? "Available" : $"Parked: {spot.ParkedVehicle.RegistrationNumber}";
            Console.WriteLine($"Spot {spot.SpotNumber}: {status}");
        }
    }

    internal Vehicle RetrieveVehicle(string regNr)
    {
        foreach (var spot in Spots)
        {
            if (spot.ParkedVehicle != null && spot.ParkedVehicle.RegistrationNumber.Equals(regNr, StringComparison.OrdinalIgnoreCase))
            {
                var vehicle = spot.ParkedVehicle;
                spot.ParkedVehicle = null; // Töm platsen
                return vehicle; // Returnera fordonet
            }
        }
        return null; // Fordonet hittades inte
    }


    internal bool MoveVehicle(string regNr)
    {
        // Leta efter fordonet
        foreach (var spot in Spots)
        {
            if (spot.ParkedVehicle != null && spot.ParkedVehicle.RegistrationNumber.Equals(regNr, StringComparison.OrdinalIgnoreCase))
            {
                // Leta efter en ledig plats
                foreach (var targetSpot in Spots)
                {
                    if (targetSpot.IsAvailable())
                    {
                        targetSpot.ParkedVehicle = spot.ParkedVehicle; // Flytta fordonet
                        spot.ParkedVehicle = null; // Töm den gamla platsen
                        return true; // Fordonet har flyttats
                    }
                }
                return false; // Ingen ledig plats fanns
            }
        }
        return false; // Fordonet hittades inte
    }

    internal Vehicle SearchVehicle(string regNr)
    {
        foreach (var spot in Spots)
        {
            if (spot.ParkedVehicle != null && spot.ParkedVehicle.RegistrationNumber.Equals(regNr, StringComparison.OrdinalIgnoreCase))
            {
                return spot.ParkedVehicle; // Returnera fordonet
            }
        }
        return null; // Fordonet hittades inte
    }

}