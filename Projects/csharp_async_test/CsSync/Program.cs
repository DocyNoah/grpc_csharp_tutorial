using System.Threading.Tasks;

class Program
{
    static void Main(string[] args)
    {
        Console.WriteLine("Start Main");
        string a = Func(name:"A", delay:1200);
        string b = Func(name:"B", delay:1100);
        string c = Func(name:"C", delay:1000);

        Console.WriteLine(a);
        Console.WriteLine(b);
        Console.WriteLine(c);
    }

    private static string Func(string name, int delay)
    {
        for(int i=0; i<3; i++)
        {
            Task.Delay(delay).Wait();
            Console.WriteLine("Func{0} wait {1}ms", name, (i+1)*delay);    
        }

        return name;
    }
}