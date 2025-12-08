#load "../../lib/AoC.Benchmark.csx"
#load "../../lib/AoC.File.csx"

using(new Timer("Stage 1"))
{
    var inputLine = ReadInputLines("2015/1/input.txt").First();

    int floor = 0;
    foreach(var ch in inputLine)
    {
        floor += ch switch
        {
            '(' => 1,
            ')' => -1,
            _ => 0
        };
    }
    Console.WriteLine($"Final floor: {floor}");
}

using(new Timer("Stage 2"))
{
    var inputLine = ReadInputLines("2015/1/input.txt").First();

    int floor = 0;
    int position = 0;
    foreach(var ch in inputLine)
    {
        position++;
        floor += ch switch
        {
            '(' => 1,
            ')' => -1,
            _ => 0
        };

        if(floor == -1)
        {
            Console.WriteLine($"Position entering basement: {position}");
            break;
        }
    }
}