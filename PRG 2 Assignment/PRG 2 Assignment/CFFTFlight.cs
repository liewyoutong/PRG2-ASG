class CFFTFlight : Flight
{
    public double RequestFee { get; set; }
    public CFFTFlight(string flightNumber, string origin, string destination, DateTime expectedTime, string status, double requestFee): base(flightNumber, origin, destination, expectedTime, status)
    {
        RequestFee = requestFee;
    }

    public override double CalculateFees()
    {
        return RequestFee; // i dont know if correct 
    }
    public override string ToString()
    {
        return base.ToString(); // idk if this correct 
    }
}