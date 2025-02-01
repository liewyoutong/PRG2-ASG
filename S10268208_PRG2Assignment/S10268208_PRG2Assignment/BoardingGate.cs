//==========================================================
// Student Number : S10268015F
// Student Name : liew You Tong
// Partner Name : Gao Yu Hao
//==========================================================
using static System.Runtime.InteropServices.JavaScript.JSType;

class BoardingGate
{
    public string GateName { get; set; }
    public bool SupportsCFFT { get; set; }
    public bool SupportsDDJB { get; set; }
    public bool SupportsLWTT{ get; set; }
    public Flight Flight { get; set; }
    public BoardingGate(string gateName, bool supportsCFFT, bool supportsDDJB, bool supportsLWTT)
    {
        GateName = gateName;
        SupportsCFFT = supportsCFFT;
        SupportsDDJB = supportsDDJB;
        SupportsLWTT = supportsLWTT;
    }
    public double CalculateFees()
    {
        return 300;
    }

    public override string ToString()
    {
        return $"Gate Name: {GateName}\n" +
               $"Supports DDJB: {SupportsDDJB}\n"+
               $"Supports CFFT: {SupportsCFFT}\n" +
               $"Supports LWTT: {SupportsLWTT}\n";
    }


}