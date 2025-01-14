using System.Globalization;

class NORMFlight: Flight
{
    public NORMFlight(string flightNumber, string origin, string destination, DateTime expectedTime, string status) : base(flightNumber, origin, destination, expectedTime, status) { }
    public override double CalculateFees()
    {
        return 100; //im not sure return what help 
    }
    public override string ToString()
    {
        return base.ToString(); // not sure if correct
    }
}