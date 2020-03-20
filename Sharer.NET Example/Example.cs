using Sharer;
using System;

class Example
{

    public void ExecuteSimpleTest()
    {
        // Connect to Arduino board
        var connection = new SharerConnection("COM3", 115200);
        connection.Connect();

        // Scan all functions shared
        connection.RefreshFunctions();

        // remote call function on Arduino and wait for the result
        var result = connection.Call("Sum", 10, 12);

        // Display the result
        Console.WriteLine("Status : " + result.Status);
        Console.WriteLine("Type : " + result.Type);
        Console.WriteLine("Value : " + result.Value);

        // Status : OK
        // Type : int
        // Value : 22
    }
}
