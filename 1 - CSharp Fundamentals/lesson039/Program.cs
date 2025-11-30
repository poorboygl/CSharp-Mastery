public class Program
{
    static void Main()
    {
        var originalList = new LinkedList<int>(new int[] { 1, 2, 3, 4, 5 });

        var reverser = new LinkedListReverser();

        Console.WriteLine("=== ReverseLinkedList Result ===");
        Console.WriteLine("Original Linked List: " + string.Join(", ", originalList));

        var reversedList = reverser.ReverseLinkedList(originalList);

        Console.WriteLine("Reversed Linked List: " + string.Join(", ", reversedList));

        Console.ReadKey();
    }
}


public class LinkedListReverser
{
    public LinkedList<int> ReverseLinkedList(LinkedList<int> list)
    {
        var reversedList = new LinkedList<int>();

        foreach (var item in list)
        {
            reversedList.AddFirst(item);
        }

        return reversedList;
    }
}

/*
* 1.Create New Linked List:

reversedList is initialized as an empty linked list.

* 2.Reverse Elements:

Loop through each item in list, adding it to the start of reversedList using AddFirst.

* 3.Return Reversed List:

reversedList now contains the elements in reverse order, so it is returned.
 */