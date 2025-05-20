
// Usage:
// 

var fileName = args[0];
const string zeros = "00000000";
const string fs = "ffffffff";
string[] splitStrings = { "  ", " " };
foreach (string line in System.IO.File.ReadLines(fileName))
{
    var parts = line.Split( splitStrings, 6, StringSplitOptions.RemoveEmptyEntries);
    var skipFirst = true;
    // This is the first change in the code
    foreach(var str in parts)
    {
        if ( skipFirst)
        {
            skipFirst = false;
            continue;
        }

        if (!str.Equals(zeros) && !str.Equals(fs))
            Console.WriteLine(str);        
    }
    // The following is the original code
    //if (!parts[1].Equals(zeros) || !parts[2].Equals(zeros) || !parts[3].Equals(zeros) || !parts[4].Equals(zeros))
    //    Console.WriteLine(line);
}

System.Console.WriteLine("Hit to exit");
System.Console.ReadLine();
