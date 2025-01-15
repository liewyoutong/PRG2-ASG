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

    //public bool AddFlight(Flight flight)
    //{

    //}
    //public double CalculateFees()
    //{

    //}
    public bool RemoveFlight(Flight flight)
    {
        return Flights.Remove(flight.FlightNumber);
    }
    public override string ToString()
    {
        return base.ToString(); //idk 
    }
}