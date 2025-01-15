
Dictionary<string, Flight> flightDict = new Dictionary<string, Flight>();
string[] flightFile = File.ReadAllLines("flights.csv");
string[] boardingFile = File.ReadAllLines("boardinggates.csv");
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
        string status = flightData[4];
        DateTime expectedTime = DateTime.ParseExact(time, "h:mm tt", null);
        Flight flight = null;

        if (string.IsNullOrWhiteSpace(status))
        {
            status = "NORM";
        }
        if (status == "CFFT")
        {
            flight = new CFFTFlight(flightNumber, origin, destination, expectedTime, status, 150);
        }
        else if (status == "NORM")
        {
            flight = new NORMFlight(flightNumber, origin, destination, expectedTime, status);
        }
        else if (status == "DDJB")
        {
            flight = new DDJBFlight(flightNumber, origin, destination, expectedTime, status, 300); 
        }
        else if (status == "LWTT")
        {
            flight = new LWTTFlight(flightNumber, origin, destination, expectedTime, status, 500);
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
    foreach (KeyValuePair<string, Flight> flightDetail in flightDict)
    {
        Flight flight = flightDetail.Value;
        string airlineName = "";
        if (flight.FlightNumber.StartsWith("SQ"))
            airlineName = "Singapore Airlines";
        else if (flight.FlightNumber.StartsWith("MH"))
            airlineName = "Malaysia Airlines";
        else if (flight.FlightNumber.StartsWith("JL"))
            airlineName = "Japan Airlines";
        else if (flight.FlightNumber.StartsWith("CX"))
            airlineName = "Cathay Pacific";
        else if (flight.FlightNumber.StartsWith("QF"))
            airlineName = "Qantas Airways";
        else if (flight.FlightNumber.StartsWith("TR"))
            airlineName = "AirAsia";
        else if (flight.FlightNumber.StartsWith("EK"))
            airlineName = "Emirates";
        else if (flight.FlightNumber.StartsWith("BA"))
            airlineName = "British Airways";

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
        if (flight.Status == "DDJB")
        {
            Console.WriteLine("Special Request Code: DDJB");
        }
        else if (flight.Status == "CFFT")
        {
            Console.WriteLine("Special Request Code: CFFT");
        }
        else if (flight.Status == "LWTT")
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
        }
        chosenGate.flight = flight;
        Console.WriteLine($"Flight {flight.FlightNumber} has been assigned to Boarding Gate {chosenGate.GateName}!");
    }
    else
    {
        Console.WriteLine("No changes made to the flight status.");
    }
}
AssignBoardingGate();