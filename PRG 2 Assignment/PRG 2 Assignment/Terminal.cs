using PRG_2_Assignment;

class Terminal
{
    string terminalName { get; set; }
    Dictionary<string, Airline> airlines { get; set; } = new Dictionary<string, Airline>();
    Dictionary<string,Flight> flights { get; set; } = new Dictionary<string, Flight>();
    Dictionary<string,BoardingGate> boardingGates { get; set; } = new Dictionary<string, BoardingGate>();
    Dictionary<string,double> gateFees { get; set; } = new Dictionary<string,double>();

    public Terminal(string terminalname) 
    { 
        terminalName=terminalname;
    }





}