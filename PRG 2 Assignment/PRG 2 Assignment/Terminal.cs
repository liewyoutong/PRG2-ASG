
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
        string code = "";
        for (int i = 0; i < 2; i++)
        {
            code += flight.FlightNumber[i];
        }

        if (Airlines.ContainsKey(code))
             return Airlines[code];

        return null;
    }

    //public void PrintAirlineFees()
    //{

    //}

    public override string ToString()
    {
        return "";
    }
}
