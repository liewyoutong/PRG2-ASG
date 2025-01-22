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
    //public double CalculateFees()
    //{

    //}
    public bool RemoveFlight(Flight flight)
    {
        if (Flights.ContainsKey(flight.FlightNumber))
        {
            Flights.Remove(flight.FlightNumber);
            return true;
        }
        return false;
    }
    public override string ToString()
    {
        return base.ToString(); //idk 
    }
}