//==========================================================
// Student Number : S10268208B
// Student Name : Gao Yu Hao
// Partner Name : Liew You Tong
//==========================================================
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

    public void PrintAirlineFees()
    {
        double airlineFees = 0;
        double totalDiscount = 0;
        double totalFees = 0;
        int flightCount = 0;
        bool moreThan5Flights = false;
        Console.WriteLine($"{"Airline Name",-20} {"Airline Fee ($)",15} {"Discount ($)",15} {"3% Discount",15}");
        foreach (Airline airline in Airlines.Values)
        {
            airlineFees = airline.CalculateFees();

            foreach (Flight flight in airline.Flights.Values)
            {
                flightCount++;
                BoardingGate? assignedGate = null;
                foreach (BoardingGate gate in BoardingGates.Values)
                {
                    if (gate.flight == flight)
                    {
                        assignedGate = gate;// Check if this gate is assigned to the current flight

                        airlineFees += flight.CalculateFees();
                        if (flightCount % 3 == 0 && flightCount > 0)
                        {
                            totalDiscount += 350;
                        }

                        if (flight.ExpectedTime.Hour < 11 || flight.ExpectedTime.Hour > 21)
                        {
                            totalDiscount += 110;
                        }
                        if (flight.Origin == "Dubai (DXB)" || flight.Origin == "Bangkok (BKK)" || flight.Origin == "Tokyo (NRT)")
                        {
                            totalDiscount += 25;
                        }
                        if (flight is NORMFlight)
                        {
                            totalDiscount += 50;
                        }

                        if (flightCount > 5)
                        {
                            moreThan5Flights = true;
                        }
                    }
                    else
                    {
                        Console.WriteLine("You do not have an assigned gate: ");
                    }
                }
            }
            if (moreThan5Flights)
            {
                airlineFees = airlineFees * 0.97;
            }
            totalFees += airlineFees - totalDiscount;
            Console.WriteLine($"{airline.Name,-20} {airlineFees,15:F2} {totalDiscount,15:F2} {moreThan5Flights,15}");

        }
        Console.WriteLine();
        Console.WriteLine($"Total fees after discount: ${totalFees}");
    }
    

    public override string ToString()
    {
        return "";
    }
}
