
class Terminal
{
    public string TerminalName { get; set; }
    Dictionary<string, Airline> Airlines { get; set; } = new Dictionary<string, Airline>();
    Dictionary<string, Flight> Flights { get; set; } = new Dictionary<string, Flight>();
    Dictionary<string, BoardingGate> BoardingGates { get; set; } = new Dictionary<string, BoardingGate>();
    Dictionary<string, double> GateFees { get; set; } = new Dictionary<string, double>();

    public Terminal(string terminalname)
    {
        TerminalName = terminalname;
    }
    public bool AddAirline(Airline airline)
    {

    }
    public bool AddBoardingGate(BoardingGate boardingGate)
    {

    }
    public void PrintAirlineFees()
    {

    }
    public override string ToString()
    {

    }

}