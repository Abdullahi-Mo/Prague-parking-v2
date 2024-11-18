using System;
using System.Collections.Generic;
using System.IO;
using System.Text.Json;
using Spectre.Console;

class MainProgram
{
    static void Main(string[] args)
    {
        // Load configuration and pricing
        Configuration config = LoadConfiguration();
        if (config == null) return;

        Pricing pricing = LoadPricing();
        if (pricing == null) return;

        ParkingGarage parkingGarage = new ParkingGarage(config.GetNumberOfSpaces());

        bool isRunning = true;

        while (isRunning)
        {
            Console.Clear();
            AnsiConsole.Markup("[bold red]Välj följande alternativ Iron Sherrifs parkering.[/]");
            AnsiConsole.WriteLine();

            var choice = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                    .Title("[bold green]Välj ett alternativ:[/]")
                    .PageSize(10)
                    .AddChoices(new[]
                    {
                        "1. Parkera fordon",
                        "2. Flytta fordon",
                        "3. Hämta fordon",
                        "4. Sök efter fordon",
                        "5. Visa parkeringsstatus",
                        "6. Avsluta"
                    })
                    .HighlightStyle(new Style(Color.Blue, decoration: Decoration.Bold)));

            switch (choice)
            {
                case "1. Parkera fordon":
                    ParkVehicle(parkingGarage, config);
                    break;
                case "2. Flytta fordon":
                    MoveVehicle(parkingGarage);
                    break;
                case "3. Hämta fordon":
                    RetrieveVehicle(parkingGarage, pricing);
                    break;
                case "4. Sök efter fordon":
                    SearchVehicle(parkingGarage);
                    break;
                case "5. Visa parkeringsstatus":
                    parkingGarage.ShowStatus();
                    break;
                case "6. Avsluta":
                    isRunning = false;
                    AnsiConsole.MarkupLine("[yellow]Programmet avslutas...[/]");
                    break;
                default:
                    AnsiConsole.MarkupLine("[blue]Ogiltigt val, försök igen.[/]");
                    break;
            }

            if (isRunning)
            {
                AnsiConsole.MarkupLine("[grey]Tryck på valfri tangent för att fortsätta...[/]");
                Console.ReadKey();
            }
        }
    }

    private static Configuration LoadConfiguration()
    {
        const string path = "Config.json";

        try
        {
            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                return JsonSerializer.Deserialize<Configuration>(json);
            }

            AnsiConsole.MarkupLine("[red]Konfigurationsfil saknas: Config.json[/]");
            return null;
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]Fel vid inläsning av konfigurationsfilen: {ex.Message}[/]");
            return null;
        }
    }

    private static Pricing LoadPricing()
    {
        const string path = "Pricelist.Json";

        try
        {
            if (File.Exists(path))
            {
                string json = File.ReadAllText(path);
                return JsonSerializer.Deserialize<Pricing>(json);
            }

            AnsiConsole.MarkupLine("[red]Prislista saknas: Pricelist.json[/]");
            return null;
        }
        catch (Exception ex)
        {
            AnsiConsole.MarkupLine($"[red]Fel vid inläsning av prislistan: {ex.Message}[/]");
            return null;
        }
    }

    private static void ParkVehicle(ParkingGarage parkingGarage, Configuration config)
    {
        Console.Write("Ange fordonstyp (CAR/MC): ");
        string vehicleType = Console.ReadLine()?.ToUpper();

        if (string.IsNullOrWhiteSpace(vehicleType) || !config.VehicleTypes.ContainsKey(vehicleType))
        {
            Console.WriteLine("Ogiltig fordonstyp.");
            return;
        }

        Console.Write("Ange registreringsnummer: ");
        string regNr = Console.ReadLine()?.ToUpper();

        if (string.IsNullOrWhiteSpace(regNr))
        {
            Console.WriteLine("Registreringsnumret måste fyllas upp.");
            return;
        }

        Vehicle vehicle = null;
        if (vehicleType == "CAR")
            vehicle = new Car(regNr);
        else if (vehicleType == "MC")
            vehicle = new MC(regNr);

        if (vehicle == null)
        {
            Console.WriteLine("Ogiltig fordonstyp.");
            return;
        }

        if (parkingGarage.ParkVehicle(vehicle))
        {
            Console.WriteLine("Fordon parkerat på plats.");
        }
        else
        {
            Console.WriteLine("Inga lediga platser tillgängliga.");
        }
    }

    private static void MoveVehicle(ParkingGarage parkingGarage)
    {
        Console.Write("Ange registreringsnummer på fordonet som ska flyttas: ");
        string regNr = Console.ReadLine()?.ToUpper();

        if (string.IsNullOrWhiteSpace(regNr))
        {
            Console.WriteLine("Registreringsnumret måste fyllas.");
            return;
        }

        if (parkingGarage.MoveVehicle(regNr))
        {
            Console.WriteLine($"Fordon med registreringsnummer {regNr} har flyttats.");
        }
        else
        {
            Console.WriteLine("Fordonet hittas EJ eller kan EJ flyttas.");
        }
    }

    private static void RetrieveVehicle(ParkingGarage parkingGarage, Pricing pricing)
    {
        Console.Write("Ange registreringsnummer på fordonet som ska hämtas: ");
        string regNr = Console.ReadLine()?.ToUpper();

        if (string.IsNullOrWhiteSpace(regNr))
        {
            Console.WriteLine("Registreringsnumret kan inte vara tomt.");
            return;
        }

        Vehicle vehicle = parkingGarage.RetrieveVehicle(regNr);
        if (vehicle != null)
        {
            string typeName = vehicle.GetType().Name.ToUpper();
            int fee = pricing.Prices.ContainsKey(typeName) ? pricing.Prices[typeName] : 0;

            Console.WriteLine($"Fordon med registreringsnummer {regNr} har hämtats.");
            Console.WriteLine($"Parkeringsavgift: {fee * Math.Ceiling(vehicle.GetParkingDuration().TotalHours)} SEK");
        }
        else
        {
            Console.WriteLine("Fordonet hittas EJ.");
        }
    }

    private static void SearchVehicle(ParkingGarage parkingGarage)
    {
        Console.Write("Ange registreringsnummer för att söka: ");
        string regNr = Console.ReadLine()?.ToUpper();

        if (string.IsNullOrWhiteSpace(regNr))
        {
            Console.WriteLine("Registreringsnumret måste fyllas.");
            return;
        }

        Vehicle vehicle = parkingGarage.SearchVehicle(regNr);
        if (vehicle != null)
        {
            Console.WriteLine($"Fordon med registreringsnummer {regNr} finns i garaget.");
        }
        else
        {
            Console.WriteLine("Fordonet hittas EJ.");
        }
    }
}