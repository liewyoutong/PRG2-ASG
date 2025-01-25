//==========================================================
// Student Number : 
// Student Name : Gao Yu Hao
// Partner Name : Liew You Tong
//==========================================================
class DDJBFlight : Flight
{
    public double RequestFee { get; set; }
    public DDJBFlight(string flightNumber, string origin, string destination, DateTime expectedTime, double requestFee) : base(flightNumber, origin, destination, expectedTime)
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
        return ""; 
    }
}