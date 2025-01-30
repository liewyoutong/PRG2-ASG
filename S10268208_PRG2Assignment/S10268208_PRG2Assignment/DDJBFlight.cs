﻿//==========================================================
// Student Number : S10268208B
// Student Name : Gao Yu Hao
// Partner Name : Liew You Tong
//==========================================================
class DDJBFlight : Flight
{
    public double RequestFee { get; set; }
    public DDJBFlight(string flightNumber, string origin, string destination, DateTime expectedTime) : base(flightNumber, origin, destination, expectedTime)
    {
        RequestFee = 300;
    }

    public override double CalculateFees()
    {
        return RequestFee + base.CalculateFees();
    }
    public override string ToString()
    {
        return $"Flight Number: {FlightNumber}\n" +
               $"Origin: {Origin}\n" +
               $"Destination: {Destination}\n" +
               $"Expected Time: {ExpectedTime}\n";

    }
}