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
        double airlineFees;
        double finalFees = 0;
        double totalDiscount;
        int flightCount;
        bool moreThan5Flights;
        double totalFees = 0;
        double grandTotalDiscount = 0;
        double totalAirlineFee = 0;
        // Display header
        Console.WriteLine($"{"Airline Name",-23}{"Airline Fee ($)",-18}{"Discount ($)",-18}{"Final Fee($)",-18}");

        foreach (Airline airline in Airlines.Values)
        {
            airlineFees = airline.CalculateFees(); // Base airline fee
            totalDiscount = 0; 
            flightCount = 0;
            moreThan5Flights = false;

            foreach (Flight flight in airline.Flights.Values)
            {
                flightCount++;

                // Find the assigned BoardingGate for this flight
                BoardingGate? assignedGate = null;
                foreach (BoardingGate gate in BoardingGates.Values)
                {
                    if (gate.Flight == flight)
                    {
                        assignedGate = gate;// Check if this gate is assigned to the current flight
                        break; 
                    }
                }

                // If the flight has an assigned gate, add its fees
                if (assignedGate != null)
                {
                    airlineFees += assignedGate.CalculateFees();
                }

                // Apply discounts
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

            // Apply 3% discount if applicable before adding to totalFees
            if (moreThan5Flights)
            {
                airlineFees *= 0.97;
            }

            finalFees = airlineFees - totalDiscount;
            totalFees += finalFees;
            grandTotalDiscount += totalDiscount;
            totalAirlineFee += airlineFees;
            Console.WriteLine($"{airline.Name,-23}{airlineFees,-18:N2}{totalDiscount,-18:N2}{finalFees,-18:N2}");
        }

        Console.WriteLine();
        Console.WriteLine($"Total Airline Fees: ${totalAirlineFee:N2}");
        Console.WriteLine($"Total Discount: ${grandTotalDiscount:N2}");
        Console.WriteLine($"Total fees after discount: ${totalFees:N2}");
    }

    public override string ToString()
    {
        return $" Terminal: {TerminalName}, Airlines: {Airlines.Count}, Flights: {Flights.Count}, Boarding Gates: {BoardingGates.Count}, Gate Fees Entries: {GateFees.Count}";
    }
}
