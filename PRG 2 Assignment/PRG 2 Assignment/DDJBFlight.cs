class DDJBFlight : Flight
{
    public double RequestFee { get; set; }
    public DDJBFlight(string flightNumber, string origin, string destination, DateTime expectedTime, string status, double requestFee) : base(flightNumber, origin, destination, expectedTime, status)
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