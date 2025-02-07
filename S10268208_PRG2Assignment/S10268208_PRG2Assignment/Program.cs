﻿//==========================================================
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
    Console.WriteLine("6. Modify Flight Details");
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


void LoadAirlinefiles(Terminal terminal) // task 1 - YouTong
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


void LoadFlightfiles() //task 2 - Yuhao
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
            DateTime expectedTime = DateTime.ParseExact(time, "h:mm tt", null); //ParseExact? It ensures that the input matches exactly with the format you specify
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

void FlightInfo(Dictionary<string, Airline> airlineDict)//task 3 - Yuhao
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
void DisplayBGList() // task 4 - YouTong
{
    Console.WriteLine("=============================================");
    Console.WriteLine("List of Boarding Gates for Changi Airport Terminal 5");
    Console.WriteLine("=============================================");
    Console.WriteLine($"{"Gate Name",-16}{"DDJB",-23}{"CFFT",-23}{"LWTT"}");
    foreach (BoardingGate boardinggate in terminal.BoardingGates.Values)
    {
        Console.WriteLine($"{boardinggate.GateName,-16}{boardinggate.SupportsDDJB,-23}{boardinggate.SupportsCFFT,-23}{boardinggate.SupportsLWTT}");
    }
}
void DisplaySpeicalCode(Flight flight) // method for speicalcode
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
void AssignBoardingGate(Dictionary<string, Flight> flightDict) //task 5 - Yuhao
{
    string flightNum;
    while (true)
    {
        Console.Write("Enter flight number: ");
        flightNum = Console.ReadLine().ToUpper();

        if (flightDict.ContainsKey(flightNum))
        {
            break;
        }

        Console.WriteLine("Invalid Flight Number. Please try again.");
    }
    Flight flight = flightDict[flightNum];
    BoardingGate chosenGate = null;
    while (true)
    {
        Console.Write("Enter Boarding Gate: ");
        string boardinggate = Console.ReadLine().ToUpper();

        foreach (BoardingGate gate in terminal.BoardingGates.Values)
        {
            if (gate.GateName == boardinggate)
            {
                chosenGate = gate;
                gate.Flight = flight;
                Console.WriteLine(gate.ToString());
                break;
            }
        }

        if (chosenGate != null)
        {
            break;
        }

        Console.WriteLine("Invalid Boarding Gate. Please try again.");
    }
    string updateStatus;
    while (true)
    {
        Console.Write("\nWould you like to update the status of the flight? (Y/N): ");
        updateStatus = Console.ReadLine().ToUpper();
        if (updateStatus == "Y" || updateStatus == "N")
        {
            break;
        }
        Console.WriteLine("Invalid choice. Please enter either Y/N.");
    }
    if (updateStatus == "Y")
    {
        Console.WriteLine("\n1. Delayed");
        Console.WriteLine("2. Boarding");
        Console.WriteLine("3. On Time");
        int option;
        while (true)
        {
            Console.Write("Please select the new status of the flight: ");
            string input = Console.ReadLine();

            try
            {
                option = Convert.ToInt32(input);

                if (option >= 1 && option <= 3)
                {
                    break; // If input is valid, break out of the loop
                }
                else
                {
                    Console.WriteLine("Invalid choice. Please enter a number between 1 and 3.");
                }
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid input. Please enter a valid number between 1 and 3.");
            }
        }
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
        Console.WriteLine($"Flight {flight.FlightNumber} has been assigned to Boarding Gate {chosenGate.GateName}!");
    }
    else if (updateStatus == "N")
    {
        flight.Status = "On Time";
        Console.WriteLine("No changes made to the flight status.");
    }
}


void CreateFlights() // task 6 - Yuhao
{
    while (true)
    {
        Console.Write("Enter your flight: ");
        string? newFlightNum = Console.ReadLine().ToUpper();
        Console.Write("Enter your Origin: ");
        string? newOrigin = Console.ReadLine();
        Console.Write("Enter your Destination: ");
        string? newDestination = Console.ReadLine();
        DateTime expectedTime;
        while (true) // allow the user to enter the time again if the input is invalid
        {
            Console.Write("\nEnter Expected Departure/Arrival Time (d/M/yyyy HH:mm): ");
            string? time = Console.ReadLine();
            if (DateTime.TryParseExact(time, "d/M/yyyy HH:mm", null, System.Globalization.DateTimeStyles.None, out expectedTime))
            {
                break; // Exit the loop if input is valid
            }
            Console.WriteLine("\nInvalid time format. Please enter the time in d/M/yyyy HH:mm format.");
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
            specialRequstCodeInFile = ""; 
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
        
        string anotherFLight;
        
        while (true)
        {
            Console.Write("Would you like to add another flight? (Y/N): ");
            anotherFLight = Console.ReadLine().ToUpper();
            if (anotherFLight != "Y" && anotherFLight != "N")
            {
                Console.WriteLine("Invalid option. Please enter either Y/N. ");
            }
            else
            {
                break;
            }
        }
        if (anotherFLight != "Y")
        {
            break;
        }
            
    }
}

void DisplayAirlineFlights() // task 7 - YouTong
{
    Console.WriteLine("=============================================");
    Console.WriteLine("List of Airlines for Changi Airport Terminal 5");
    Console.WriteLine("=============================================");
    Console.WriteLine("Airline Code  Airline Name");

    foreach (var airlines in terminal.Airlines.Values)
    {
        Console.WriteLine($"{airlines.Code,-13}{airlines.Name}");
    }
    string airlineCode;
    while (true)
    {
        Console.Write("Enter Airline Code: ");
        airlineCode = Console.ReadLine().ToUpper();

        if (terminal.Airlines.ContainsKey(airlineCode))
        {
            break;
        }
        else
        {
            Console.WriteLine("Invalid Airline Code. Enter valid airline code.");
            continue;
        }
    }
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
 
void ModifyFlightDetails() // Task 8 - YouTong
{
    DisplayAirlineFlights();

    string flightName;
    while (true)
    {
        Console.Write("Choose an existing Flight to modify or delete: ");
        flightName = Console.ReadLine().ToUpper();

        // Ensure flight format (must contain space)
        if (!flightName.Contains(" "))
        {
            Console.WriteLine("Invalid format! Ensure a space between airline code and number (e.g SQ 115).");
            continue;
        }

        // Check if flight exists
        if (!terminal.Flights.ContainsKey(flightName))
        {
            Console.WriteLine("Flight not found. Please enter a valid flight number.");
            continue;
        }
        break; // Valid input
    }

    int option;
    while (true)
    {
        try
        {
            Console.WriteLine("1. Modify Flight");
            Console.WriteLine("2. Delete Flight");
            Console.Write("Choose an option: ");
            option = Convert.ToInt32(Console.ReadLine());

            if (option == 1 || option == 2) break; 

            Console.WriteLine("Invalid option. Please enter 1 or 2.");
        }
        catch (FormatException)
        {
            Console.WriteLine("Invalid input. Please enter a number (1 or 2).");
        }
    }

    Flight flight = terminal.Flights[flightName];
    Airline airline = terminal.GetAirlineFromFlight(flight);

    if (option == 1) // Modify Flight
    {
        int modifyOption;
        while (true)
        {
            try
            {
                Console.WriteLine("Choose an option to modify:");
                Console.WriteLine("1. Modify Origin/Destination/Expected Time");
                Console.WriteLine("2. Modify Flight Status");
                Console.WriteLine("3. Modify Special Request Code");
                Console.WriteLine("4. Modify Boarding Gate");
                Console.Write("Enter your choice: ");
                modifyOption = Convert.ToInt32(Console.ReadLine());

                if (modifyOption >= 1 && modifyOption <= 4) break;

                Console.WriteLine("Invalid choice. Please enter a number from 1 to 4.");
            }
            catch (FormatException)
            {
                Console.WriteLine("Invalid input. Please enter a number from 1 to 4.");
            }
        }

        if (modifyOption == 1) // Modify Origin, Destination, Expected Time
        {
            Console.Write("Enter new Origin: ");
            string newOrigin = Console.ReadLine();
            Console.Write("Enter new Destination: ");
            string newDestination = Console.ReadLine();

            DateTime expectedTime;
            while (true)
            {
                Console.Write("Enter new Expected Departure/Arrival Time (dd/MM/yyyy HH:mm): ");
                string timeInput = Console.ReadLine();

                if (DateTime.TryParseExact(timeInput, "dd/MM/yyyy HH:mm", null, System.Globalization.DateTimeStyles.None, out expectedTime))
                {
                    break; // Valid time
                }
                Console.WriteLine("Invalid time format. Please enter in dd/MM/yyyy HH:mm format.");
            }

            flight.Origin = newOrigin;
            flight.Destination = newDestination;
            flight.ExpectedTime = expectedTime;
        }
        else if (modifyOption == 2) // Modify Flight Status
        {
            int newStatus;
            while (true)
            {
                try
                {
                    Console.WriteLine("1. Delayed");
                    Console.WriteLine("2. Boarding");
                    Console.WriteLine("3. On Time");
                    Console.Write("Choose new status: ");
                    newStatus = Convert.ToInt32(Console.ReadLine());

                    if (newStatus >= 1 && newStatus <= 3) break;

                    Console.WriteLine("Invalid choice. Please enter a number from 1 to 3.");
                }
                catch (FormatException)
                {
                    Console.WriteLine("Invalid input. Please enter a number from 1 to 3.");
                }
            }

            if (newStatus == 1)
            {
                flight.Status = "Delayed";
            }
            else if (newStatus == 2)
            {
                flight.Status = "Boarding";
            }
            else
            {
                flight.Status = "On Time";
            }
            Console.WriteLine("Flight status has been updated.\n");
        }
        else if (modifyOption == 3) // Modify Special Request Code
        {
            int newCode;
            while (true)
            {
                try
                {
                    Console.WriteLine("1. DDJB");
                    Console.WriteLine("2. CFFT");
                    Console.WriteLine("3. LWTT");
                    Console.WriteLine("4. None");
                    Console.Write("Choose new Special Request Code: ");
                    newCode = Convert.ToInt32(Console.ReadLine());

                    if (newCode >= 1 && newCode <= 4) break;

                    Console.WriteLine("Invalid choice. Please enter a number from 1 to 4.");
                }
                catch (FormatException)
                {
                    Console.WriteLine("Invalid input. Please enter a number from 1 to 4.");
                }
            }

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
                    Console.WriteLine("Special Request Code updated to None.");
                    break;
                default:
                    Console.WriteLine("Invalid choice. No changes made to Special Request Code.");
                    return;
            }

            terminal.Flights[flight.FlightNumber] = modifiedFlight; // Update the flight in the dictionary
            flight = modifiedFlight; // Reassign 'flight' to the new modifiedFlight reference
            Console.WriteLine($"Special Request Code updated.");
        }
        else if (modifyOption == 4) // Modify Boarding Gate
        {
            string newGate;

            while (true)
            {
                try
                {
                    Console.Write("Enter new Boarding Gate: ");
                    newGate = Console.ReadLine().ToUpper();

                    // Check if the gate exists in the dictionary
                    if (terminal.BoardingGates.ContainsKey(newGate))
                    {
                        terminal.BoardingGates[newGate].Flight = flight;

                        // Update the gate name for the assigned flight
                        foreach (BoardingGate gate in terminal.BoardingGates.Values)
                        {
                            if (gate.Flight == flight)
                            {
                                gate.GateName = newGate;
                            }
                        }
                        Console.WriteLine($"Boarding gate has been updated to {newGate}.");
                        break;
                    }
                    throw new KeyNotFoundException(); // If the gate does not exist, throw an exception
                }
                catch (KeyNotFoundException)
                {
                    Console.WriteLine("Invalid Boarding Gate. Please enter a valid gate.");
                }
            }
        }

        Console.WriteLine("Flight updated!");
        Console.WriteLine($"Flight Number: {flight.FlightNumber}");
        Console.WriteLine($"Airline Name: {airline.Name}");
        Console.WriteLine($"Origin: {flight.Origin}");
        Console.WriteLine($"Destination: {flight.Destination}");
        Console.WriteLine($"Expected Departure/Arrival Time: {flight.ExpectedTime:dd/MM/yyyy HH:mm}");
        Console.WriteLine($"Status: {flight.Status}");
        DisplaySpeicalCode(flight);
    }
    else if (option == 2) // Delete Flight
    {
        terminal.Flights.Remove(flightName);
        Console.WriteLine($"Flight {flightName} has been successfully deleted.");
    }
}
void DisplayScheduledFlight(Terminal terminal, List<Flight> flightList) //task 9 - Yuhao
{
    Console.WriteLine($"{"Flight Number",-15}{"Airline Name",-20}{"Origin",-20}{"Destination",-20}{"Expected Time",-25}{"Status",-15}{"Special Request",-20}{"Boarding Gate",-15}");
    if (flightList.Count == 0)
    {
        foreach (Flight flight in terminal.Flights.Values)
        {
            flightList.Add(flight);
        }
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
        else
        {
            specialRequestCode = "";
        }

        Console.Write($"{flight.FlightNumber,-15}{airlineName,-20}{flight.Origin,-20}{flight.Destination,-20}{flight.ExpectedTime,-25}{flight.Status,-15}{specialRequestCode,-20}");
        if (assignedGate == null)
        {
            Console.WriteLine("Unassigned");
        }
        else { Console.WriteLine(assignedGate); }
    }
}

void BulkUnassignedflights(Terminal terminal) // Advanced Feature A - YouTong
{
    Queue<Flight> unassignedFlights = new Queue<Flight>();
    List<BoardingGate> availableGates = new List<BoardingGate>();
    int initialAssignedFlights = 0;
    int initialAssignedGates = 0; 
    foreach (BoardingGate boardingGate in terminal.BoardingGates.Values)
    {
        if (boardingGate.Flight == null)
        {
            availableGates.Add(boardingGate);
        }
        else
        {
            initialAssignedGates++;
        }
    }
    foreach (var flight in terminal.Flights.Values)
    {
        bool isAssigned = false;
        foreach (BoardingGate boardingGate in terminal.BoardingGates.Values)
        {
            if (boardingGate.Flight == flight)
            {
                isAssigned = true;
                initialAssignedFlights++;
                break; 
            }
        }
        if (!isAssigned) 
        {
            unassignedFlights.Enqueue(flight);
        }
    }
    Console.WriteLine($"Total number of flights without a boarding gate assigned: {unassignedFlights.Count}");
    Console.WriteLine($"Total number of available boarding gates: {availableGates.Count}");
    int assigned = 0;
    while (availableGates.Count > 0 && unassignedFlights.Count > 0)
    {
        Flight flight = unassignedFlights.Dequeue();
        BoardingGate assignedGate = null;
        foreach (BoardingGate gate in availableGates)
        {
            if ((flight is CFFTFlight && gate.SupportsCFFT) || (flight is DDJBFlight && gate.SupportsDDJB) || (flight is LWTTFlight && gate.SupportsLWTT) || (!(flight is CFFTFlight || flight is DDJBFlight || flight is LWTTFlight))) // Assign a gate without restrictions
            {
                assignedGate = gate;
                break;
            }
        }
        Console.WriteLine();
        if (assignedGate != null)
        {
            assignedGate.Flight = flight;
            assigned++;
            availableGates.Remove(assignedGate);
            Console.WriteLine($"Flight {flight.FlightNumber} has been assigned to Boarding Gate {assignedGate.GateName}");
        }
        else
        {
            Console.WriteLine($"No suitable boarding gate available for Flight {flight.FlightNumber}");
        }
    }
    int totalProcessedFlights = assigned;
    int totalProcessedGates= assigned;
    int totalFlights = assigned + initialAssignedFlights;
    int totalGates = assigned + initialAssignedGates;
    double percentageAssignedFlights = (double)assigned / initialAssignedFlights * 100;
    double percentageAssignedGates = (double)assigned / initialAssignedGates * 100;
    Console.WriteLine();
    DisplayScheduledFlight(terminal, terminal.Flights.Values.ToList());
    Console.WriteLine($"Total number of flights assigned: {assigned}");
    Console.WriteLine($"Remaining unassigned flights: {unassignedFlights.Count}");
    Console.WriteLine($"Total number of flights and boarding gates processed: {totalProcessedFlights}");
    if (initialAssignedFlights == 0)
    {
        Console.WriteLine("No flights were previously assigned to boarding gates hence the percentage of the total number of flights and boarding gates that were processed automatically over those that were processed automatically over those that were already assigned cannot be calculated.");
    }
    else
    {
        Console.WriteLine($"Total number of flights and boarding gates that were processed automatically over those that were already assigned (in percentage): {percentageAssignedFlights}%");
    }
    
    
}

void Displaytotalfee()//Advanced Feature B - Yuhao
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
    terminal.PrintAirlineFees();
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
        Console.WriteLine("Invalid option. Please enter a number between 0 and 9.");
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
        BulkUnassignedflights(terminal);
        Spaces();
    }
    else if (option == 9)
    {
        Displaytotalfee();
        Spaces();
    }
    else
    {
        Console.WriteLine("Invalid option. Please enter a number between 0 and 9.");
    }
}
