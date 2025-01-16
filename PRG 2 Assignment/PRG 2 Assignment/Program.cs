
Dictionary<string, Flight> flightDict = new Dictionary<string, Flight>();
Dictionary<string, string> airlineDict = new Dictionary<string, string>();
string[] flightFile = File.ReadAllLines("flights.csv");
string[] boardingFile = File.ReadAllLines("boardinggates.csv");
string[] airlineFile = File.ReadAllLines("airlines.csv");
void DisplayLoadingMenu()
{

}


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

        if (flight != null && !flightDict.ContainsKey(flightNumber))
        {
            flightDict.Add(flightNumber, flight);
        }
    }
}
LoadFlightfiles();

void FlightInfo()//task 3
{ 
    Console.WriteLine("Flight Number Airline Name        Origin                   Destination           Expected Departure/Arrival Time");
    Console.WriteLine("----------------------------------------------------------------------------------------------------------------");

    for (int i = 1; i < airlineFile.Length; i++) 
    {
        string[] airlineData = airlineFile[i].Split(',');
        if (airlineData.Length >= 2) 
        {
            string airlineName = airlineData[0];
            string airlineCode = airlineData[1];
            if (!airlineDict.ContainsKey(airlineCode))
            {
                airlineDict.Add(airlineCode, airlineName);
            }   
        }
    }
    foreach (Flight flight in flightDict.Values)
    {
        string airlineName = "";
        string code = "";
        for (int i = 0; i < 2 && i < flight.FlightNumber.Length; i++)
        {
            code += flight.FlightNumber[i];
        }
        if (airlineDict.ContainsKey(code))
        {
            airlineName = airlineDict[code];
        }

        Console.WriteLine($"{flight.FlightNumber,-15}{airlineName,-20}{flight.Origin,-25}{flight.Destination,-25}{flight.ExpectedTime,-25}");
    }
}
FlightInfo();

void AssignBoardingGate() //task 5
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
        if (flight is DDJBFlight )
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

    List<BoardingGate> boardingGates = new List<BoardingGate>();
    for (int i = 1; i < boardingFile.Length; i++) 
    {
        string[] boardingData = boardingFile[i].Split(",");
        string gateName = boardingData[0];
        bool supportsDDJB = Convert.ToBoolean(boardingData[1]);
        bool supportsCFFT = Convert.ToBoolean(boardingData[2]);
        bool supportsLWTT = Convert.ToBoolean(boardingData[3]); 
        BoardingGate gate = new BoardingGate(gateName, supportsCFFT, supportsDDJB, supportsLWTT);
        boardingGates.Add(gate);
    }
    bool gateFound = false;
    BoardingGate chosenGate = null;
    foreach (BoardingGate gate in boardingGates)
    {
        if(gate.GateName == boardinggate)
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
AssignBoardingGate();

void CreateFlights()
{
    Console.Write("Enter your flight: ");
    string newFlight = Console.ReadLine();
    Console.Write("Enter your Origin: ");
    string newOrigin = Console.ReadLine();
    Console.Write("Enter your Airline Name:");
    string newAirline = Console.ReadLine() ;
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