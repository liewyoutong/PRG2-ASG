﻿
class Terminal
{
    public string TerminalName { get; set; }
    public Dictionary<string, Airline> Airlines { get; set; } = new Dictionary<string, Airline>();
    public Dictionary<string, Flight> Flights { get; set; } = new Dictionary<string, Flight>();
    public Dictionary<string, BoardingGate> BoardingGates { get; set; } = new Dictionary<string, BoardingGate>();
    public Dictionary<string, double> GateFees { get; set; } = new Dictionary<string, double>();

    public Terminal() { }
    public Terminal(string terminalname)
    {
        TerminalName = terminalname;
    }
    public bool AddAirline(Airline airline)
    {
        if (Airlines.ContainsKey(airline.Code))
        {
            return false;
        }
        Airlines[airline.Code] = airline;
        return true;
    }

    public bool AddBoardingGate(BoardingGate gate)
    {
        if (BoardingGates.ContainsKey(gate.GateName))
        {
            return false;
        }
        BoardingGates[gate.GateName] = gate;
        return true;
    }

    public Airline GetAirlineFromFlight(Flight flight)
    {
        string flightCode = flight.FlightNumber.Substring(0, 2);
        Airline? airline = null;

        foreach (KeyValuePair<string, Airline> kvp in Airlines)
        {
            if (kvp.Key == flightCode)
            {
                airline = kvp.Value;
            }
        }
        return airline;
    }

    //public void PrintAirlineFees()
    //{

    //}

    public override string ToString()
    {
        return "";
    }
}
