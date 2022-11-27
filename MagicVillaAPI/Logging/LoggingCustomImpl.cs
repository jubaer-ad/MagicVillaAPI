namespace MagicVillaAPI.Logging
{
    public class LoggingCustomImpl : ILoggingCustom
    {
        public void Log(string message, string type)
        {
            if (type.ToLower() == "err")
            {
                Console.BackgroundColor = ConsoleColor.Red;
                Console.ForegroundColor = ConsoleColor.White;
            }
            else
            {
                Console.BackgroundColor = ConsoleColor.Green;
                Console.ForegroundColor = ConsoleColor.White;
            }
            Console.WriteLine(type.ToUpper() + ": " + message);
        }
    }
}
