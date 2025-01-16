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
        return base.ToString(); // idk if this correct 
    }
}