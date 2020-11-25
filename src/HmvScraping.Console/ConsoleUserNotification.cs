namespace HmvScraping.Console
{
    using System;
    using System.Collections.Generic;
    using System.Text;

    public class ConsoleUserNotification : IUserNotification
    {
        public void Put(string message)
        {
            Console.WriteLine(message);
        }
    }
}
