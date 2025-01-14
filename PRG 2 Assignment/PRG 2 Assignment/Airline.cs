class Airline
{
    public string Name { get; set; }
    public string Code { get; set; }
    private Dictionary<string, Flight> Flights { get; } = new();

    public Airline(string name, string code)
    {
        Name = name;
        Code = code;
    }

    public bool AddFlight(Flight flight)
    {
 
    }
    public double CalculateFees()
    {

    }
    public bool RemoveFlight(Flight flight)
    {
        return Flights.Remove(flight.FlightNumber);
    }
    public override string ToString()
    {
        return base.ToString(); //idk 
    }
}