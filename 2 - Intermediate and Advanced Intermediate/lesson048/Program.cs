public class Program
{
    static void Main()
    {
        Console.WriteLine("=== TRAFFIC LIGHT SIMULATION ===\n");

        TrafficLight trafficLight = new TrafficLight();

        for (int i = 0; i < 5; i++) // chạy 5 lần mô phỏng
        {
            Console.WriteLine($"Current State: {trafficLight.CurrentState}");
            trafficLight.ChangeState();
            Console.WriteLine();
            System.Threading.Thread.Sleep(500); // delay 0.5s cho dễ nhìn
        }

        Console.WriteLine("=== END OF SIMULATION ===");
        Console.ReadKey();
    }
}

public enum TrafficLightState
{
    Red,
    Green,
    Yellow
}

public class TrafficLight
{
    public TrafficLightState CurrentState { get; private set; } = TrafficLightState.Red;

    public void ChangeState()
    {
        switch (CurrentState)
        {
            case TrafficLightState.Red:
                Console.WriteLine("Changing from Red to Green.");
                CurrentState = TrafficLightState.Green;
                break;
            case TrafficLightState.Green:
                Console.WriteLine("Changing from Green to Yellow.");
                CurrentState = TrafficLightState.Yellow;
                break;
            case TrafficLightState.Yellow:
                Console.WriteLine("Changing from Yellow to Red.");
                CurrentState = TrafficLightState.Red;
                break;
        }
    }
}
/*
 === TRAFFIC LIGHT SIMULATION ===

Current State: Red
Changing from Red to Green.

Current State: Green
Changing from Green to Yellow.

Current State: Yellow
Changing from Yellow to Red.

Current State: Red
Changing from Red to Green.

Current State: Green
Changing from Green to Yellow.

=== END OF SIMULATION ===
 
 */

/*
* 1.Initial State:

    CurrentState is initially set to Red.

* 2.Switch Case for State Transitions:

    ChangeState uses a switch to update CurrentState and print messages indicating the transition.

* 3.Cyclic State Transitions:

    After Yellow, the state transitions back to Red, completing a cycle.
 
 */