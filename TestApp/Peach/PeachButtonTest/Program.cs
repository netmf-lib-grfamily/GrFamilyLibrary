using GrFamily.MainBoard;
using Microsoft.SPOT;

namespace PeachButtonTest
{
    public class Program
    {
        private readonly Peach _peach;

        public static void Main()
        {
            var program = new Program();
            program.Run();
        }

        public Program()
        {
            _peach = new Peach();

            _peach.Button.ButtonPressed += Button_ButtonPressed;
            _peach.Button.ButtonReleased += Button_ButtonReleased;
        }

        void Button_ButtonPressed(Button sender, Button.ButtonState state)
        {
            _peach.SetDebugLed(true);
        }

        void Button_ButtonReleased(Button sender, Button.ButtonState state)
        {
            _peach.SetDebugLed(false);
        }

        public void Run()
        {
            Debug.Print("Test Started");

            while (true) { }
        }
    }
}
