//==========================================================
// Student Number : 
// Student Name : Gao Yu Hao
// Partner Name : Liew You Tong
//==========================================================
class NORMFlight : Flight
{
    public NORMFlight(string flightNumber, string origin, string destination, DateTime expectedTime)
        : base(flightNumber, origin, destination, expectedTime)
    {
        FlightNumber = flightNumber;
        Origin = origin;
        Destination = destination;
        ExpectedTime = expectedTime;
    }


    public override double CalculateFees()
    {
        double basefee = 300;
        return basefee;
    }
    public override string ToString()
    {
        return ""; 
    }
}