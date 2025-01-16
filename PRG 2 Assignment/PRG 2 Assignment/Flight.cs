abstract class Flight
{
    public string FlightNumber { get; set; }
    public string Origin { get; set; }
    public string Destination { get; set; }
    public DateTime? ExpectedTime { get; set; }
    public string Status { get; set; } =  "On Time";
    public Flight(string flightnumber, string origin, string destination, DateTime expectedtime)
    {
        FlightNumber = flightnumber;
        Origin = origin;
        Destination = destination;
        ExpectedTime = expectedtime;
    }
    public abstract double CalculateFees();
    public override string ToString()
    {
        return $"{FlightNumber,-8} {Origin,-20} {Destination,-20} {ExpectedTime,-24} {Status,-12}";
    }
}