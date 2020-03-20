# Sharer.NET

# Sharer

![Sharer](https://raw.githubusercontent.com/Rufus31415/Sharer.NET/master/Sharer.png)

Sharer is an Arduino library that facilitates the communication between a PC and an Arduino board.
You chose the functions and variables you want to share with your desktop application and your arduino board. Functions can be easily remotly called.


![Sharer](https://raw.githubusercontent.com/Rufus31415/Sharer.NET/master/Resources/RemoteFunctionCall.png)

Tested on boards :
- Arduino UNO
- Arduino NANO
- Arduino MEGA
- Arduino DUE



## .NET C# code example


``` C#
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
```

## [Arduino code example](https://github.com/Rufus31415/Sharer.NET)

``` C++
#include <Sharer.h>

// A simple function that sums an integer and a byte and return an integer
int Sum(int a, byte b) {
	return a + b;
}

// A simple function that return a^2
float Square(float a) {
	return a * a;
}

// Init Sharer and declare your function to share
void setup() {
	Sharer.init(115200); // Init Serial communication with 115200 bauds

	// Expose this function to Sharer : int Sum(int a, byte b) 
	Sharer_ShareFunction(int, Sum, int, a, byte, b);

	// Expose this function to Sharer : float Square(float a)
	Sharer_ShareFunction(float, Square, float, a);
}

// Run Sharer engine in the main Loop
void loop() {
	Sharer.run();
}
```
## Usage

### Download and install the library
The library should be downloaded from GitHub and the Sharer folder copied into the \libraries folder.

### Initialisation
The communication between the .NET application and the Arduino is over the Serial protocole.
The Arduino code should initialize Sharer in the setup() function.
``` C++
#include <Sharer.h>

void setup() {
	// Initialize with Serial
	Sharer.init(115200);
	
	// Initialize with another Serial interface
	Serial2.begin(9600);
	Sharer.init(Serial2);
}
```

The loop() function should call Sharer.run().
``` C++
void loop() {
	Sharer.run();
}
```

### Share a function
 to be continued...
