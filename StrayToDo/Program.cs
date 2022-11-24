namespace StrayToDo
{
    public class Program
    {
        static void Main(string[] args)
        {
            string location = @"D:\Problem-Solving-Hub";
            new Runner().Start(new string[1] { location });
        }
    }
}
