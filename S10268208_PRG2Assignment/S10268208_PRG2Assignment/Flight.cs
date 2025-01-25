//==========================================================
// Student Number : S10268015F
// Student Name : liew You Tong
// Partner Name : Gao Yu Hao
//==========================================================

abstract class Flight : IComparable<Flight>
{
    public string FlightNumber { get; set; }
    public string Origin { get; set; }
    public string Destination { get; set; }
    public DateTime? ExpectedTime { get; set; }
    public string Status { get; set; } = "Scheduled";
    public Flight(string flightnumber, string origin, string destination, DateTime expectedtime)
    {
        FlightNumber = flightnumber;
        Origin = origin;
        Destination = destination;
        ExpectedTime = expectedtime;
    }
    public abstract double CalculateFees();
    public int CompareTo(Flight other)
    {
        if (other.ExpectedTime < this.ExpectedTime) return 1;
        else if (other.ExpectedTime > this.ExpectedTime) return -1;
        return 0;
    }
    public override string ToString()
    {
        return $"Flight Number: {FlightNumber}\n"+
               $"Origin: {Origin}\n"+
               $"Destination: {Destination}\n"+
               $"Expected Time: {ExpectedTime} \n";
    }
}