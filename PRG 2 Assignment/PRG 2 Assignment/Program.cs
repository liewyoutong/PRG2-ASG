//==========================================================
// Student Name : Liew You Tong (S10268015F)
// Student Name : Gao Yu Hao ()
//==========================================================

Terminal terminal = new Terminal("Terminal 5");

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
void Spaces()
{
    Console.WriteLine();
    Console.WriteLine();
    Console.WriteLine();
    Console.WriteLine();
    Console.WriteLine();
}


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


void LoadFlightfiles() //task 2
{
    using (StreamReader sr = new StreamReader("flights.csv"))
    {
        string s = sr.ReadLine();
        while ((s = sr.ReadLine()) != null)
        {
            string[] flightData = s.Split(',');
            string flightNumber = flightData[0];
            string origin = flightData[1];
            string destination = flightData[2];
            string time = flightData[3];
            string specialRequestCode = flightData[4];
            DateTime expectedTime = DateTime.ParseExact(time, "h:mm tt", null);
            Flight flight = null;
            string airlineCode = flightNumber.Substring(0, 2);
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
                flight = new DDJBFlight(flightNumber, origin, destination, expectedTime, 300);
            }
            else if (specialRequestCode == "LWTT")
            {
                flight = new LWTTFlight(flightNumber, origin, destination, expectedTime, 500);
            }

            if (flight != null && !terminal.Flights.ContainsKey(flightNumber))
            {
                terminal.Flights.Add(flightNumber, flight);
            }
            if (terminal.Airlines.ContainsKey(airlineCode))
            {
                Airline airline = terminal.Airlines[airlineCode];
                airline.Flights[flightNumber] = flight;
            }
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
        Airline airline = terminal.GetAirlineFromFlight(flight);
        string airlineName = airline.Name;

            Console.WriteLine($"{flight.FlightNumber,-15}{airlineName,-20}{flight.Origin,-25}{flight.Destination,-25}{flight.ExpectedTime,-25}");
    }
}
void DisplayBGList() // task 4 
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

void AssignBoardingGate(Dictionary<string, Flight> flightDict) //task 5
{
    Console.Write("Enter flight number: ");
    string flightNum = Console.ReadLine().ToUpper();
    Console.Write("Enter Boarding Gate Name: ");
    string boardinggate = Console.ReadLine().ToUpper();
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
    string updateStatus = Console.ReadLine().ToUpper();
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


void CreateFlights() // task 6
{
    while (true)
    {
        Console.Write("Enter your flight: ");
        string? newFlightNum = Console.ReadLine();
        Console.Write("Enter your Origin: ");
        string? newOrigin = Console.ReadLine();
        Console.Write("Enter your Airline Name:");
        string? newAirline = Console.ReadLine();
        Console.Write("Enter your Destination: ");
        string? newDestination = Console.ReadLine();
        Console.Write("Enter your Expected Departure/Arrival Time (hh:mm AM/PM): ");
        string? time = Console.ReadLine();
        DateTime expectedTime;
        if (!DateTime.TryParseExact(time, "h:mm tt", null, System.Globalization.DateTimeStyles.None, out expectedTime))
        {
            Console.WriteLine("Invalid time format. Please enter the time in hh:mm AM/PM format.");
            return;
        }

        DateTime now = DateTime.Now;
        expectedTime = new DateTime(now.Year, now.Month, now.Day, expectedTime.Hour, expectedTime.Minute, 0);

        Console.Write("Would you like to enter any Special Requst Code? Y/N ");
        string? option = Console.ReadLine();

        string specialRequestCode = "";
        if (option == "Y")
        {
            Console.Write("Enter Special Request Code (CFFT/DDJB/LWTT): ");
            string speicalRequestCode = Console.ReadLine()?.ToUpper();
            if (speicalRequestCode == "CFFT" || speicalRequestCode == "DDJB" || speicalRequestCode == "LWTT")
            {
                specialRequestCode = speicalRequestCode;

            }
            else
            {
                Console.WriteLine("Please enter correct Special Request Code");
            }
        }
        else
        {
            specialRequestCode = "";
        }
        Flight newFlight;
        if (specialRequestCode.ToUpper() == "CFFT")
        {
            newFlight = new CFFTFlight(newFlightNum, newOrigin, newDestination, expectedTime, 150);
        }
        else if (specialRequestCode.ToUpper() == "LWTT")
        {
            newFlight = new LWTTFlight(newFlightNum, newOrigin, newDestination, expectedTime, 500);
        }
        else if (specialRequestCode.ToUpper() == "DDJB")
        {
            newFlight = new DDJBFlight(newFlightNum, newOrigin, newDestination, expectedTime, 300);
        }
        else
        {
            newFlight = new NORMFlight(newFlightNum, newOrigin, newDestination, expectedTime);
        }
        terminal.Flights[newFlight.FlightNumber] = newFlight;
        Console.WriteLine($"Flight {newFlightNum} has been successfully added to the system.");
        string filePath = "flights.csv";
        try
        {
            using (StreamWriter sw = new StreamWriter(filePath, true))
            {
                sw.WriteLine($"{newFlightNum},{newOrigin},{newDestination},{expectedTime:h:mm tt},{specialRequestCode}");
            }
            Console.WriteLine("Flight information has been saved to flights.csv.");
        }
        catch (Exception ex)
        {
            Console.WriteLine($"Unable save flight information: {ex.Message}");
        }

        Console.WriteLine($"Flight Number: {newFlightNum}");
        Console.WriteLine($"Airline Name: {newAirline}");
        Console.WriteLine($"Origin: {newOrigin}");
        Console.WriteLine($"Destination: {newDestination}");
        Console.WriteLine($"Special Request Code: {specialRequestCode}");
        Console.WriteLine($"Expected Departure/Arrival Time: {expectedTime:dd/MM/yyyy h:mm:ss tt}");

        Console.Write("Would you like to add another flight? (Y/N): ");
        string anotherFlight = Console.ReadLine().ToUpper();
        if (anotherFlight != "Y")
        {
            break;
        }
    }
}

void DisplayAirlineFlights() // task 7
{
    Console.WriteLine("=============================================");
    Console.WriteLine("List of Airlines for Changi Airport Terminal 5");
    Console.WriteLine("=============================================");
    Console.WriteLine("Airline Code  Airline Name");

    foreach (var airlines in terminal.Airlines.Values)
    {
        Console.WriteLine($"{airlines.Code,-13}{airlines.Name}");
    }

    Console.Write("Enter Airline Code: ");
    string airlineCode = Console.ReadLine().ToUpper();

    if (terminal.Airlines.ContainsKey(airlineCode))
    {
        Airline airline = terminal.Airlines[airlineCode];

        Console.WriteLine($"Number of flights for {airline.Name}: {airline.Flights.Count}");

        Console.WriteLine("=============================================");
        Console.WriteLine($"List of Flights for {airline.Name}");
        Console.WriteLine("=============================================");
        Console.WriteLine($"{"Flight Number",-16}{"Airline Name",-23}{"Origin",-23}{"Destination",-23}Expected Departure/Arrival Time");

        if (airline.Flights.Count == 0)
        {
            Console.WriteLine("No flights available for this airline.");
        }

        else
        {
            foreach (Flight flight in airline.Flights.Values)
            {
                Console.WriteLine($"{flight.FlightNumber,-16}{airline.Name,-23}{flight.Origin,-23}{flight.Destination,-23}{flight.ExpectedTime}");
            }
        }
    }
    else
    {
        Console.WriteLine("Invalid Airline Code.");
    }
}

void ModifyFlightDetails() // task 8 
{
    DisplayAirlineFlights();
    Console.Write("Choose an existing Flight to modify or delete:  ");
    string flightName = Console.ReadLine().ToUpper();
    Console.WriteLine("1. Modify Flight");
    Console.WriteLine("2. Delete Flight");
    Console.WriteLine("Choose an option: ");
    int option = Convert.ToInt32(Console.ReadLine());
    if (option == 1)
    {
        Flight flight = terminal.Flights[flightName];
        Airline airline = terminal.GetAirlineFromFlight(flight);
        Console.WriteLine("1. Modify Basic Information");
        Console.WriteLine("2. Modify Status");
        Console.WriteLine("3. Modify Special Request Code");
        Console.WriteLine("4. Modify Boarding Gate");
        Console.WriteLine("Choose an option: ");
        int option2 = Convert.ToInt32(Console.ReadLine());
        if (option == 1)
        {
            Console.Write("Enter new Origin: ");
            string newOrigin = Console.ReadLine();
            Console.Write("Enter new Destination: ");
            string newDestination = Console.ReadLine();
            Console.Write("Enter new Expected Departure/Arrival Time (dd/mm/yyyy hh:mm): ");
            string time = Console.ReadLine();
            DateTime expectedTime;
            if (!DateTime.TryParseExact(time, "dd/M/yyyy HH:mm", null, System.Globalization.DateTimeStyles.None, out expectedTime))
            {
                Console.WriteLine("Invalid time format. Please enter the time in dd/mm/yyyy hh:mm format.");
                return;
            }

            flight.Origin = newOrigin;
            flight.Destination = newDestination;
            flight.ExpectedTime = expectedTime;
        }
        Console.WriteLine("Flight updated!");
        Console.WriteLine($"Flight Number: {flight.FlightNumber}");
        Console.WriteLine($"Airline Name: {airline.Name}");
        Console.WriteLine($"Origin: {flight.Origin}");
        Console.WriteLine($"Destination: {flight.Destination}");
        Console.WriteLine($"Expected Departure/Arrival Time: {flight.ExpectedTime:dd/M/yyyy h:mm:ss tt}");
    }
    else if (option == 2)
    {
        if (terminal.Flights.ContainsKey(flightName))
        {
            Flight flight = terminal.Flights[flightName];
            Airline airline = terminal.GetAirlineFromFlight(flight);
            airline.RemoveFlight(flight);
            terminal.Flights.Remove(flightName);
            Console.WriteLine($"Flight {flightName} has been successfully deleted.");
        }
        else
        {
            Console.WriteLine("Invalid Flight Number.");
        }
    }
    else
    {
        Console.WriteLine("Invalid choice.");

    }
}


        //Main program
        DisplayLoadingMenu();
Spaces();
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
        Spaces();
    }
    else if (option == 2)
    {
        DisplayBGList();
        Spaces();
    }
    else if (option == 3)
    {
        AssignBoardingGate(terminal.Flights);
        Spaces();
    }
    else if (option == 4)
    {
        CreateFlights();
        Spaces();
    }
    else if (option == 5)
    {
        DisplayAirlineFlights();
        Spaces();
    }
    else if (option == 6)
    {
        ModifyFlightDetails();
        Spaces();
    }
    
}