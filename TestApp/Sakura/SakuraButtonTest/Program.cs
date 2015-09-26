using GrFamily.MainBoard;
using Microsoft.SPOT;

namespace SakuraButtonTest
{
    public class Program
    {
        private readonly Sakura _sakura;

        public static void Main()
        {
            var program = new Program();
            program.Run();
        }

        public Program()
        {
            _sakura = new Sakura();

            _sakura.Button.ButtonPressed += Button_ButtonPressed;
            _sakura.Button.ButtonReleased += Button_ButtonReleased;
        }

        void Button_ButtonPressed(Button sender, Button.ButtonState state)
        {
            _sakura.SetDebugLed(true);
        }

        void Button_ButtonReleased(Button sender, Button.ButtonState state)
        {
            _sakura.SetDebugLed(false);
        }

        public void Run()
        {
            Debug.Print("Test Started");

            while (true) { }
        }
    }
}
