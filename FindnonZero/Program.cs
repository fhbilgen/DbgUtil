
// Usage:
// 

var fileName = args[0];
const string zeros = "00000000";
string[] splitStrings = { "  ", " " };
foreach (string line in System.IO.File.ReadLines(fileName))
{
    var parts = line.Split( splitStrings, 6, StringSplitOptions.RemoveEmptyEntries);
    if (!parts[1].Equals(zeros) || !parts[2].Equals(zeros) || !parts[3].Equals(zeros) || !parts[4].Equals(zeros))
        Console.WriteLine(line);
}

System.Console.WriteLine("Hit to exit");
System.Console.ReadLine();
