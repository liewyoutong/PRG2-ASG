//==========================================================
// Student Number : S10268208B
// Student Name : Gao Yu Hao
// Partner Name : Liew You Tong
//==========================================================
class LWTTFlight : Flight
{
    public double RequestFee { get; set; }
    public LWTTFlight(string flightNumber, string origin, string destination, DateTime expectedTime, double requestFee) : base(flightNumber, origin, destination, expectedTime)
    {
        RequestFee = requestFee;
    }

    public override double CalculateFees()
    {
        double basefee = 300;
        double totalfee = basefee + RequestFee;
        return totalfee;
    }
    public override string ToString()
    {
        return $"Flight Number: {FlightNumber}\n" +
               $"Origin: {Origin}\n" +
               $"Destination: {Destination}\n" +
               $"Expected Time: {ExpectedTime}\n";

    }
}