using System.Threading.Tasks;

class Program
{
    static async Task Main(string[] args)
    {
        Console.WriteLine("Start Main");
        Task<string> taskA = Func(name:"A", delay:1200);
        Task<string> taskB = Func(name:"B", delay:1100);
        Task<string> taskC = Func(name:"C", delay:1000);
        
        string a = await taskA;
        string b = await taskB;
        string c = await taskC;

        Console.WriteLine(a);
        Console.WriteLine(b);
        Console.WriteLine(c);
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