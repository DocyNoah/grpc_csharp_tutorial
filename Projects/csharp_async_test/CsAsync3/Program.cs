using System.Threading.Tasks;
using System.Collections.Generic;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Start Main");
        Task<string> taskA = Func(name:"A", delay:1200);
        Task<string> taskB = Func(name:"B", delay:1100);
        Task<string> taskC = Func(name:"C", delay:1000);

        List<Task<string>> taskList = new List<Task<string>> {taskA, taskB, taskC};

        while(taskList.Count > 0)
        {
            Task<string> finishedTask = await Task.WhenAny(taskList);
            string x = await finishedTask;
            Console.WriteLine(x);
            taskList.Remove(finishedTask);
        }
    }

    private static async Task<string> Func(string name, int delay)
    {
        for(int i=0; i<3; i++)
        {
            await Task.Delay(delay);
            Console.WriteLine("Func{0} wait {1}ms", name, (i+1)*delay);    
        }

        return name;
    }
}