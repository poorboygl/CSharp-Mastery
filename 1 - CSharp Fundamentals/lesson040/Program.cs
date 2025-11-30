public class Program
{
    static void Main()
    {
        var simulator = new DiceSimulator();

        Console.WriteLine("=== DiceSimulator Result ===");

        string result = simulator.RollDiceAndCheckWin();

        Console.WriteLine("Dice Roll Result: " + result);

        Console.ReadKey();
    }
}

public class DiceSimulator
{
    public string RollDiceAndCheckWin()
    {
        Random random = new Random();
        int diceRoll = random.Next(1, 7); // Generates a number between 1 and 6

        if (diceRoll == 6)
        {
            return "Win";
        }
        else
        {
            return "Lose";
        }
    }
}

/*
* 1.Generate Random Dice Roll:

    random.Next(1, 7) generates a random integer from 1 to 6.

* 2.Conditional Check for Win:

    If diceRoll is 6, return "Win".

    Otherwise, return "Lose".
 
 */