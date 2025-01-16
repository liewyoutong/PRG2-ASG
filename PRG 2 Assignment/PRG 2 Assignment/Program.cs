
//Dictionary<string, Flight> flightDict = new Dictionary<string, Flight>();
//Dictionary<string, string> airlineDict = new Dictionary<string, string>();
Terminal terminal = new Terminal("Terminal 5");
string[] flightFile = File.ReadAllLines("flights.csv");
string[] boardingFile = File.ReadAllLines("boardinggates.csv");
string[] airlineFile = File.ReadAllLines("airlines.csv");


void DisplayLoadingMenu()
{
    Console.WriteLine("Loading Airlines..."); Console.WriteLine("8 Airlines Loaded!");
    Console.WriteLine("Loading Boarding Gates..."); Console.WriteLine("66 Boarding Gates Loaded!");
    Console.WriteLine("Loading Flights..."); Console.WriteLine("30 Flights Loaded!");
}
void DisplayMenu()
{
    Console.WriteLine("=============================================");
    Console.WriteLine("Welcome to Changi Airport Terminal 5"); Console.WriteLine("=============================================");
    Console.WriteLine("1. List All Flights"); Console.WriteLine("2. List Boarding Gates");
    Console.WriteLine("3. Assign a Boarding Gate to a Flight"); Console.WriteLine("4. Create Flight");
    Console.WriteLine("5. Display Airline Flights"); Console.WriteLine("6. Modify Flight Detailst");
    Console.WriteLine("7. Display Flight Schedule"); Console.WriteLine("0. Exit");
    Console.WriteLine(); Console.WriteLine("Please select your choice: ");
}
// start of task 1 

void LoadAirlinefiles(Terminal terminal) // task 1 
{
    using (StreamReader sr = new StreamReader("airlines.csv"))
    {
        string s = sr.ReadLine();
        while ((s = sr.ReadLine()) != null)
        {
            string[] airlineData = s.Split(',');
            string name = airlineData[0];
            string code = airlineData[1];
            Airline airline = new Airline(name, code);
            terminal.AddAirline(airline);
        }
    }
}
LoadAirlinefiles(terminal);
void LoadBoaringGatefiles(Terminal terminal)
{
    using (StreamReader sr = new StreamReader("boardinggates.csv"))
    {
        string s = sr.ReadLine();
        while ((s = sr.ReadLine()) != null)
        {
            string[] boardingData = s.Split(',');
            string gateName = boardingData[0];
            bool supportsDDJB = Convert.ToBoolean(boardingData[1]);
            bool supportsCFFT = Convert.ToBoolean(boardingData[2]);
            bool supportsLWTT = Convert.ToBoolean(boardingData[3]);
            BoardingGate gate = new BoardingGate(gateName, supportsCFFT, supportsDDJB, supportsLWTT);
            terminal.AddBoardingGate(gate);
        }
    }
}

LoadBoaringGatefiles(terminal);

// end of task 1 
void LoadFlightfiles() //task 2
{

    for (int i = 1; i < flightFile.Length; i++)
    {
        string[]flightData = flightFile[i].Split(',');
        string flightNumber = flightData[0];
        string origin = flightData[1];
        string destination = flightData[2];
        string time = flightData[3];
        string specialRequestCode = flightData[4];
        DateTime expectedTime = DateTime.ParseExact(time, "h:mm tt", null);
        Flight flight = null;

        if (string.IsNullOrWhiteSpace(specialRequestCode))
        {
            specialRequestCode = "NORM";
        }
        if (specialRequestCode == "CFFT")
        {
            flight = new CFFTFlight(flightNumber, origin, destination, expectedTime, 150);
        }
        else if (specialRequestCode == "NORM")
        {
            flight = new NORMFlight(flightNumber, origin, destination, expectedTime);
        }
        else if (specialRequestCode == "DDJB")
        {
            flight = new DDJBFlight(flightNumber, origin, destination, expectedTime,  300); 
        }
        else if (specialRequestCode == "LWTT")
        {
            flight = new LWTTFlight(flightNumber, origin, destination, expectedTime, 500);
        }

        if (flight != null && !terminal.Flights.ContainsKey(flightNumber))
        {
            terminal.Flights.Add(flightNumber, flight);
        }
    }
}
LoadFlightfiles();

void FlightInfo(Dictionary<string, Airline> airlineDict)//task 3
{
    Console.WriteLine("Flight Number  Airline Name        Origin                   Destination             Expected Departure/Arrival Time");
    Console.WriteLine("----------------------------------------------------------------------------------------------------------------");

    foreach (Flight flight in terminal.Flights.Values)
    { 
        string airlineName = "";
        string code = "";
        for (int i = 0; i < 2; i++)
        {
            code += flight.FlightNumber[i];
        }

        if (terminal.Airlines.ContainsKey(code))
        {
            Airline airline = terminal.Airlines[code];
            airlineName = airline.Name;

            Console.WriteLine($"{flight.FlightNumber,-15}{airlineName,-20}{flight.Origin,-25}{flight.Destination,-25}{flight.ExpectedTime,-25}");
        }
    }
}

FlightInfo(terminal.Airlines);

    // start of task 4  
void DisplayBDList()
{
    Console.WriteLine("=============================================");
    Console.WriteLine("List of Boarding Gates for Changi Airport Terminal 5");
    Console.WriteLine("=============================================");
    Console.WriteLine($"{"Gate Name",-16}{"DDJB",-23}{"CFFT",-23}{"LWTT"}");
    foreach (var boardinggate in terminal.BoardingGates.Values)
    {
        Console.WriteLine($"{boardinggate.GateName,-16}{boardinggate.SupportsDDJB,-23}{boardinggate.SupportsCFFT,-23}{boardinggate.SupportsLWTT}");
    }
}
DisplayBDList();
void AssignBoardingGate(Dictionary<string, Flight> flightDict) //task 5
{
    Console.Write("Enter flight number: ");
    string flightNum = Console.ReadLine();
    Console.Write("Enter Boarding Gate Name: ");
    string boardinggate = Console.ReadLine();
    Flight flight = flightDict[flightNum];
    if (flightDict.ContainsKey(flightNum))
    {
        Console.WriteLine($"\nFlight Number: {flight.FlightNumber}\n");
        Console.WriteLine($"Origin: {flight.Origin}\n");
        Console.WriteLine($"Destination: {flight.Destination}\n");
        Console.WriteLine($"Expected Time: {flight.ExpectedTime} \n");
        if (flight is DDJBFlight)
        {
            Console.WriteLine("Special Request Code: DDJB");
        }
        else if (flight is CFFTFlight)
        {
            Console.WriteLine("Special Request Code: CFFT");
        }
        else if (flight is LWTTFlight)
        {
            Console.WriteLine("Special Request Code: LWTT");
        }
        else
        {
            Console.WriteLine("Special Request Code: None");
        }
    }

    else { Console.WriteLine("Invalid flight number"); }

    

    bool gateFound = false;
    BoardingGate chosenGate = null;
    foreach (BoardingGate gate in terminal.BoardingGates.Values)
    {
        if (gate.GateName == boardinggate)
        {
            chosenGate = gate;
            Console.WriteLine($"Boarding Gate Name: {gate.GateName}");
            Console.WriteLine($"Supports DDJB: {gate.SupportsDDJB}");
            Console.WriteLine($"Supports CFFT: {gate.SupportsCFFT}");
            Console.WriteLine($"Supports LWTT: {gate.SupportsLWTT}");
            gateFound = true;
            break;
        }
    }
    if (!gateFound)
    {
        Console.WriteLine("Invalid Boarding Gate. Please try again.");
    }

    Console.Write("\nWould you like to update the status of the flight? (Y/N): ");
    string updateStatus = Console.ReadLine();
    if (updateStatus == "Y")
    {
        Console.WriteLine("\n1. Delayed");
        Console.WriteLine("2. Boarding");
        Console.WriteLine("3. On Time");

        Console.Write("Please select the new status of the flight: ");
        int option = Convert.ToInt32(Console.ReadLine());
        if (option == 1)
        {
            flight.Status = "Delayed";
        }
        else if (option == 2)
        {
            flight.Status = "Boarding";
        }
        else if (option == 3)
        {
            flight.Status = "On Time";
        }
        else
        {
            Console.WriteLine("Invalid choice.");
            flight.Status = "On Time";
        }
        Console.WriteLine($"Flight {flight.FlightNumber} has been assigned to Boarding Gate {chosenGate.GateName}!");
    }
    else
    {
        flight.Status = "On Time";
        Console.WriteLine("No changes made to the flight status.");
    }
    Console.WriteLine("Updated Flight Details: ");
    Console.WriteLine($"Flight Number: {flight.FlightNumber}");
    Console.WriteLine($"Status: {flight.Status}");
    Console.WriteLine($"Assigned Boarding Gate: {chosenGate.GateName}");

}
AssignBoardingGate(terminal.Flights);

void CreateFlights()
{
    Console.Write("Enter your flight: ");
    string newFlight = Console.ReadLine();
    Console.Write("Enter your Origin: ");
    string newOrigin = Console.ReadLine();
    Console.Write("Enter your Airline Name:");
    string newAirline = Console.ReadLine();
    Console.Write("Enter your Destination: ");
    string newDestination = Console.ReadLine();
    Console.Write("Enter your Expected Departure/Arrival Time (hh:mm AM/PM): ");
    string? time = Console.ReadLine();
    DateTime expectedTime = DateTime.ParseExact(time, "h:mm tt", null);

    Console.Write("Would you like to enter any Special Requst Code? Y/N ");
    string? option = Console.ReadLine();

    string specialRequestCode = "NORM";
    if (option == "Y")
    {
        Console.Write("Enter Special Request Code (CFFT/DDJB/LWTT): ");
        string choice = Console.ReadLine()?.ToUpper();
        if (choice == "CFFT" || choice == "DDJB" || choice == "LWTT")
        {
            specialRequestCode = choice;

        }
    }
    else
    {
        specialRequestCode = "NORM";
    }
    Console.WriteLine($"Flight Number: {newFlight}");
    Console.WriteLine($"Airline Name: {newAirline}");
    Console.WriteLine($"Origin: {newOrigin}");
    Console.WriteLine($"Destination: {newDestination}");
    Console.WriteLine($"Special Request Code: {specialRequestCode}");
}
CreateFlights();


// Main program 
DisplayLoadingMenu();
while (true)
{
    DisplayMenu();
    int option = Convert.ToInt32(Console.ReadLine());
    if (option == 0)
    {
        Console.WriteLine("Goodbye!");
    }
    else if (option == 1)
    {
        FlightInfo(terminal.Airlines);
    }
}