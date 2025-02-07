﻿//==========================================================
// Student Number : S10268015F
// Student Name : liew You Tong
// Partner Name : Gao Yu Hao
//==========================================================
class Airline
{
    public string Name { get; set; }
    public string Code { get; set; }
    public Dictionary<string, Flight> Flights { get; set; }

    public Airline(string name, string code)
    {
        Name = name;
        Code = code;
        Flights = new Dictionary<string, Flight>();
    }

    public bool AddFlight(Flight flight)
    {
        if (Flights.ContainsKey(flight.FlightNumber))
        {
            return false;
        }
        Flights.Add(flight.FlightNumber, flight);
        return true;
    }

    public bool RemoveFlight(Flight flight)
    {
        if (Flights.ContainsKey(flight.FlightNumber))
        {
            Flights.Remove(flight.FlightNumber);
            return true;
        }
        return false;
    }
    public double CalculateFees()
    {
        double airlineFee = 0;
        foreach (Flight flight in Flights.Values)
        {
            airlineFee += flight.CalculateFees();
        }
        return airlineFee;
    }

    public override string ToString()
    {
        return $"Airline Name: {Name}\n" +
               $"Airline Code: {Code}";

    }
}