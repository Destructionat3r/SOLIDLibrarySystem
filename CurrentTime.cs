using System;

namespace SOLIDLibrarySystem
{
    public class CurrentTime : IUserInterfaceElement
    {
        private DateTime time;

        public CurrentTime()
        {
            time = System.DateTime.Now;
        }
        public void Display()
        {
            Console.WriteLine($"Time: {time.ToString("HH:mm:ss")}");
        }
        public void Update()
        {
            time = System.DateTime.Now;
        }
    }
}