//==========================================================
// Student Name : Liew You Tong (S10268015F)
// Student Name : Gao Yu Hao (S10268208B)
//==========================================================

using System.Collections.Generic;
using System.Reflection.Metadata.Ecma335;

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
    Console.WriteLine("Welcome to Changi Airport Terminal 5");
    Console.WriteLine("=============================================");
    Console.WriteLine("1. List All Flights"); 
    Console.WriteLine("2. List Boarding Gates");
    Console.WriteLine("3. Assign a Boarding Gate to a Flight");
    Console.WriteLine("4. Create Flight");
    Console.WriteLine("5. Display Airline Flights"); 
    Console.WriteLine("6. Modify Flight Detailst");
    Console.WriteLine("7. Display Flight Schedule");
    Console.WriteLine("8. Process all unassigned flights to boarding gates in bulk");
    Console.WriteLine("9. Display the total fee per airline for the day");
    Console.WriteLine("0. Exit");
    Console.WriteLine();
    Console.Write("Please select your choice: ");
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
            terminal.GateFees[gate.GateName] = gate.CalculateFees(); // stores fees for all its boarding gates separately.
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
            string airlineCode = flightNumber.Substring(0, 2); // to extract the airline code from the flight number
            if (string.IsNullOrWhiteSpace(specialRequestCode))
            {
                specialRequestCode = "NORM";
            }
            if (specialRequestCode == "CFFT")
            {
                flight = new CFFTFlight(flightNumber, origin, destination, expectedTime);
            }
            else if (specialRequestCode == "NORM")
            {
                flight = new NORMFlight(flightNumber, origin, destination, expectedTime);
            }
            else if (specialRequestCode == "DDJB")
            {
                flight = new DDJBFlight(flightNumber, origin, destination, expectedTime);
            }
            else if (specialRequestCode == "LWTT")
            {
                flight = new LWTTFlight(flightNumber, origin, destination, expectedTime);
            }

            if (flight != null && !terminal.Flights.ContainsKey(flightNumber))
            {
                terminal.Flights.Add(flightNumber, flight);
            }
                                                            // If a flight with flightNumber = "SQ 115" is being processed:
            if (terminal.Airlines.ContainsKey(airlineCode))//Check if "SQ" exists in terminal.Airlines
            {
                Airline airline = terminal.Airlines[airlineCode]; // If a flight with flightNumber = "SQ 115" is being processed, Retrieve the Airline object for "SQ".
                airline.Flights[flightNumber] = flight; //Add the flight to the Flights dictionary of Singapore Airlines:
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
        if (airline != null)
        {
            string airlineName = airline.Name;
            Console.WriteLine($"{flight.FlightNumber,-15}{airlineName,-20}{flight.Origin,-25}{flight.Destination,-25}{flight.ExpectedTime,-25}");
        }

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
void DisplaySpeicalCode(Flight flight)
{
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
void AssignBoardingGate(Dictionary<string, Flight> flightDict) //task 5
{
    Console.Write("Enter flight number: ");
    string flightNum = Console.ReadLine().ToUpper();
    Console.Write("Enter Boarding Gate Name: ");
    string boardinggate = Console.ReadLine().ToUpper();
    Flight flight = flightDict[flightNum];
    if (flightDict.ContainsKey(flightNum))
    {
        Console.WriteLine(flight.ToString());
        DisplaySpeicalCode(flight);
    }

    else { Console.WriteLine("Invalid flight number"); }



    bool gateFound = false;

    BoardingGate chosenGate = null;
    foreach (BoardingGate gate in terminal.BoardingGates.Values)
    {
        if (gate.GateName == boardinggate)
        {
            chosenGate = gate;
            gate.Flight = flight;
            Console.WriteLine(gate.ToString());
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
}


void CreateFlights() // task 6
{
    while (true)
    {
        Console.Write("Enter your flight: ");
        string? newFlightNum = Console.ReadLine();
        Console.Write("Enter your Origin: ");
        string? newOrigin = Console.ReadLine();
        Console.Write("Enter your Destination: ");
        string? newDestination = Console.ReadLine();
        DateTime expectedTime;
        while (true) // allow the user to enter the time again if the input is invalid
        {
            Console.Write("\nEnter Expected Departure/Arrival Time (dd/M/yyyy HH:mm): ");
            string? time = Console.ReadLine();
            if (DateTime.TryParseExact(time, "dd/M/yyyy HH:mm", null, System.Globalization.DateTimeStyles.None, out expectedTime))
            {
                break; // Exit the loop if input is valid
            }
            Console.WriteLine("\nInvalid time format. Please enter the time in dd/M/yyyy HH:mm format.");
        }


        Console.Write("Enter Special Request Code (CFFT/DDJB/LWTT/None): ");
        string specialRequestCode = Console.ReadLine().ToUpper();

        if (specialRequestCode != "CFFT" && specialRequestCode != "DDJB" && specialRequestCode != "LWTT" && specialRequestCode != "NONE")
        {
            Console.WriteLine("Invalid code entered. Defaulting to 'None'.");
            specialRequestCode = "NONE";
        }
        string specialRequstCodeInFile;
        if (specialRequestCode == "NONE")
        {
            specialRequstCodeInFile = ""; // Assign an empty string if specialRequestCode is "NORM"
        }
        else
        {
            specialRequstCodeInFile = specialRequestCode;
        }
            Flight newFlight;
            if (specialRequestCode.ToUpper() == "CFFT")
            {
                newFlight = new CFFTFlight(newFlightNum, newOrigin, newDestination, expectedTime);
            }
            else if (specialRequestCode.ToUpper() == "LWTT")
            {
                newFlight = new LWTTFlight(newFlightNum, newOrigin, newDestination, expectedTime);
            }
            else if (specialRequestCode.ToUpper() == "DDJB")
            {
                newFlight = new DDJBFlight(newFlightNum, newOrigin, newDestination, expectedTime);
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
                    sw.WriteLine($"{newFlightNum},{newOrigin},{newDestination},{expectedTime:h:mm tt},{specialRequstCodeInFile}");
                }
                Console.WriteLine("Flight information has been saved to flights.csv.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Unable save flight information: {ex.Message}");
            }

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
        string speicalRequest = "";
        Console.Write("Choose an existing Flight to modify or delete:  ");
        string flightName = Console.ReadLine().ToUpper();
        Console.WriteLine("1. Modify Flight");
        Console.WriteLine("2. Delete Flight");
        Console.WriteLine("Choose an option: ");
        int option;
        try
        {
        option = Convert.ToInt32(Console.ReadLine());
        }
        catch (FormatException)
        {
            Console.WriteLine("Invalid option. Please enter a number between 1 and 2.");
            return;
        }
    if (option == 1)
        {
            Flight flight = terminal.Flights[flightName];
            Airline airline = terminal.GetAirlineFromFlight(flight);
            Console.WriteLine("Choose an option to modify:");
            Console.WriteLine("1. Modify Origin/Destination/Expected Time");
            Console.WriteLine("2. Modify Flight Status");
            Console.WriteLine("3. Modify Special Request Code");
            Console.WriteLine("4. Modify Boarding Gate");
            Console.Write("Enter your choice: ");
            int ModifyOption = Convert.ToInt32(Console.ReadLine());
        if (ModifyOption == 1)
        {
            Console.Write("Enter new Origin: ");
            string newOrigin = Console.ReadLine();
            Console.Write("Enter new Destination: ");
            string newDestination = Console.ReadLine();
            Console.Write("Enter new Expected Departure/Arrival Time (dd/MM/yyyy hh:mm): ");
            string time = Console.ReadLine();
            if (DateTime.TryParseExact(time, "dd/M/yyyy HH:mm", null, System.Globalization.DateTimeStyles.None, out DateTime expectedTime))
            {
                flight.Origin = newOrigin;
                flight.Destination = newDestination;
                flight.ExpectedTime = expectedTime;
            }
            else
            {
                Console.WriteLine("Invalid time format. Please enter the time in dd/mm/yyyy hh:mm format.");
            }
        }
        else if (ModifyOption == 2)
        {
            Console.WriteLine("1. Delayed");
            Console.WriteLine("2. Boarding");
            Console.WriteLine("3. On Time");
            Console.Write("Choose new status: ");
            int newStatus = Convert.ToInt32(Console.ReadLine());
            flight.Status = newStatus == 1 ? "Delayed" : newStatus == 2 ? "Boarding" : "On Time";
            Console.WriteLine("Flight status has been updated.\n");
        }
        else if (ModifyOption == 3)
        {
            {
                Console.WriteLine("1. DDJB");
                Console.WriteLine("2. CFFT");
                Console.WriteLine("3. LWTT");
                Console.WriteLine("4. None");
                Console.Write("Choose new Special Request Code: ");
                int newCode = Convert.ToInt32(Console.ReadLine());
                DateTime expectedTime = flight.ExpectedTime;
                Flight modifiedFlight;
                switch (newCode)
                {
                    case 1:
                        modifiedFlight = new DDJBFlight(flight.FlightNumber, flight.Origin, flight.Destination, expectedTime);
                        Console.WriteLine("Special Request Code updated to DDJB.");
             
                        break;
                    case 2:
                        modifiedFlight = new CFFTFlight(flight.FlightNumber, flight.Origin, flight.Destination, expectedTime);
                        Console.WriteLine("Special Request Code updated to CFFT.");
                        break;
                    case 3:
                        modifiedFlight = new LWTTFlight(flight.FlightNumber, flight.Origin, flight.Destination, expectedTime);
                        Console.WriteLine("Special Request Code updated to LWTT.");
                        break;
                    case 4:
                        modifiedFlight = new NORMFlight(flight.FlightNumber, flight.Origin, flight.Destination, expectedTime);
                        Console.WriteLine("Special Request Code updated to NORM.");
                        break;
                    default:
                        Console.WriteLine("Invalid choice. No changes made to Special Request Code.");
                        return; 
                }

                terminal.Flights[flight.FlightNumber] = modifiedFlight;
                Airline associatedAirline = terminal.GetAirlineFromFlight(flight);
            }
 
        }




        else if (ModifyOption == 4)
        {
            Console.Write("Enter new Boarding Gate: ");
            string newGate = Console.ReadLine().ToUpper();
            // Ensure that the gate exists in the BoardingGates collection
            if (terminal.BoardingGates.ContainsKey(newGate))
            {
                terminal.BoardingGates[newGate].Flight = flight;

                // Update the gate name for the assigned flight
                foreach (BoardingGate gate in terminal.BoardingGates.Values)
                {
                    if (flight == gate.Flight)
                    {
                        gate.GateName = newGate;
                    }
                }
            }
        }
        else
        {
            Console.WriteLine("Invalid option. Please restart and enter a valid number between 1 and 4.");
        }

            Console.WriteLine("Flight updated!");
            Console.WriteLine($"Flight Number: {flight.FlightNumber}");
            Console.WriteLine($"Airline Name: {airline.Name}");
            Console.WriteLine($"Origin: {flight.Origin}");
            Console.WriteLine($"Destination: {flight.Destination}");
            Console.WriteLine($"Expected Departure/Arrival Time: {flight.ExpectedTime:dd/M/yyyy h:mm:ss tt}");
            Console.WriteLine($"Status: {flight.Status}");
            DisplaySpeicalCode(flight);
            bool gateFound = false;
            foreach (BoardingGate gate in terminal.BoardingGates.Values)
            {
                if (gate.Flight == flight)
                {
                    Console.WriteLine($"Boarding Gate: {gate.GateName}");
                    gateFound = true;
                    break; // Exit the loop once the gate is found
                }
            }

            if (!gateFound)
            {
                Console.WriteLine("Boarding Gate: Unassigned");
            }
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

void DisplayScheduledFlight(Terminal terminal, List<Flight> flightList) //task 9
{
    Console.WriteLine($"{"Flight Number",-15}{"Airline Name",-20}{"Origin",-20}{"Destination",-20}{"Expected Time",-25}{"Status",-15}{"Special Request",-20}{"Boarding Gate",-15}");
    foreach (Flight flight in terminal.Flights.Values)
    {
        flightList.Add(flight);
    }
    flightList.Sort();
    foreach (Flight flight in flightList)
    {
        string assignedGate = null;
        string airlineName = "";
        Airline airline = terminal.GetAirlineFromFlight(flight);
        if (airline != null)
        {
            airlineName = airline.Name;
        }

        else { continue; }

        foreach (BoardingGate gate in terminal.BoardingGates.Values)
        {
            if (flight == gate.Flight)
            {
                assignedGate = gate.GateName;

            }
        }
        string specialRequestCode = null;
        if (flight is DDJBFlight)
        {
            specialRequestCode = "DDJB";
        }
        else if (flight is CFFTFlight)
        {
            specialRequestCode = "CFFT";
        }
        else if (flight is LWTTFlight)
        {
            specialRequestCode = "LWTT";
        }

        Console.Write($"{flight.FlightNumber,-15}{airlineName,-20}{flight.Origin,-20}{flight.Destination,-20}{flight.ExpectedTime,-25}{flight.Status,-15}{specialRequestCode,-20}");
        if (assignedGate == null)
        {
            Console.WriteLine("Unassigned");
        }
        else { Console.WriteLine(assignedGate); }
    }
}

bool AssignAllFlights()//ADvance feature A
{
    return true;
}

void BulkUnassignedflights() //Advanced feature A
{
    Queue<Flight> unassignedFlights = new Queue<Flight>();
    List<BoardingGate> availableGates = new List<BoardingGate>();
    int initialAssignedFlights = 0;
    int intialAssignedGates = 0;
    foreach (var boardingGate in terminal.BoardingGates.Values)
    {
        if (boardingGate.Flight == null)
        {
            availableGates.Add(boardingGate);
        }
        else
        {
            intialAssignedGates++;
        }
    }
    foreach (var flights in terminal.Flights.Values)
    {
        bool isAssigned = false;
        foreach (var boardinggate in terminal.BoardingGates.Values)
        {
            if (boardinggate.Flight == flights)
            {
                isAssigned = true;
                initialAssignedFlights++;
                break;
            }
            if (isAssigned)
            {
                unassignedFlights.Enqueue(flights);
            }
        }
    }
    Console.WriteLine($"Total number of flights that does not have any boarding gate assigned: {unassignedFlights.Count}");
    Console.WriteLine($"Total number of boarding gates that does not have a flight number assigned yet: {availableGates.Count}");
    int assigned = 0;
    while (availableGates.Count > 0 && unassignedFlights.Count > 0)
    {
        Flight flight = unassignedFlights.Dequeue();
        BoardingGate assignedgate = null;
        foreach (var gate in availableGates)
        {
            bool isSupportedFlight =
                (flight is CFFTFlight && gate.SupportsCFFT) ||
                (flight is DDJBFlight && gate.SupportsDDJB) ||
                (flight is LWTTFlight && gate.SupportsLWTT);

            bool isOtherFlight = !(flight is CFFTFlight || flight is DDJBFlight || flight is LWTTFlight);

            if (isSupportedFlight || isOtherFlight)
            {
                assignedgate = gate;
                break;
            }

            if (assignedgate != null)
            {
                assignedgate.Flight = flight;
                availableGates.Remove(assignedgate);
                assigned++;
                Console.WriteLine($"{flight.FlightNumber} has been assigned to gate {assignedgate.GateName}");
            }
            else
            {
                Console.WriteLine($"{flight.FlightNumber} could not be assigned to any gate.");
            }
        }
    }
    int totalFlights = assigned + initialAssignedFlights;
    int totalGates = assigned + intialAssignedGates;
    double percentageAssigned = (double)assigned / totalFlights * 100;
    double percentageGates = (double)assigned / totalGates * 100;

    DisplayScheduledFlight(terminal, new List<Flight>());
    Console.WriteLine($"Total number of flights assigned: {assigned}");
    Console.WriteLine($"Remaining Unassigned Flights: {unassignedFlights.Count}");
    Console.WriteLine($"Total number of flights and boarding gates processed: {totalFlights}, {totalGates}");
    Console.WriteLine($"Percentage of flights and gates processed automatically: {percentageAssigned:F2}% for flights, {percentageGates:F2}% for gates");
}

void Displaytotalfee()
{
    // Check that all flights have been assigned a boarding gate
    bool flightsAllAssigned = true;
    List<Flight> unassignedFlights = new List<Flight>();

    foreach (Flight flight in terminal.Flights.Values)
    {
        bool isAssigned = false;

        // Check if this flight has been assigned to any gate
        foreach (BoardingGate gate in terminal.BoardingGates.Values)
        {
            if (gate.Flight == flight)
            {
                isAssigned = true;
                break; 
            }
        }

        if (!isAssigned)
        {
            unassignedFlights.Add(flight);
        }
    }

    // If not all flights have been assigned a boarding gate, show a message
    if (unassignedFlights.Count > 0)
    {
        Console.WriteLine("Please ensure that all unassigned flights have their boarding gates assigned before running this feature again.");
        foreach (Flight flight in unassignedFlights)
        {
            Console.WriteLine($"Flight Number: {flight.FlightNumber} is not assigned with a Boarding Gate");
        }
       
        return;
    }
}


    //Main program
DisplayLoadingMenu();
Spaces();
while (true)
{
    List<Flight> flightList = new List<Flight>();
    DisplayMenu();
    int option;
    try
    {
        option = Convert.ToInt32(Console.ReadLine());
    }
    catch (FormatException)
    {
        Console.WriteLine("Invalid option. Please enter a number between 0 and 7.");
        continue;
    }
    if (option == 0)
    {
        Console.WriteLine("Goodbye!");
        break;
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
    else if (option == 7)
    {
        DisplayScheduledFlight(terminal, flightList);
        Spaces();
        flightList.Sort();

    }
    else if (option == 8)
    {
        BulkUnassignedflights();
        Spaces();
    }
    else if (option == 9)
    {
        Displaytotalfee();
        Spaces();
    }
    else
    {
        Console.WriteLine("Invalid option. Please enter a number between 0 and 7.");
    }
}
