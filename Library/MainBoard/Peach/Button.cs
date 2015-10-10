using Microsoft.SPOT.Hardware;

namespace GrFamily.MainBoard
{
    public delegate void ButtonEventHandler(Button sender, Button.ButtonState state);

    public class Button
    {
        public enum ButtonState
        {
            /// <summary>‰Ÿ‚³‚ê‚Ä‚¢‚é</summary>
            Pressed,
            /// <summary>—£‚³‚ê‚Ä‚¢‚é</summary>
            Released
        }

        protected readonly InterruptPort ButtonPort;

        protected bool PrevPressed;

        public event ButtonEventHandler ButtonPressed;
        public event ButtonEventHandler ButtonReleased;

        public Button(Cpu.Pin pin) 
        {
            ButtonPort = new InterruptPort(pin, true, Port.ResistorMode.Disabled, Port.InterruptMode.InterruptEdgeBoth);
            ButtonPort.OnInterrupt += ButtonPort_OnInterrupt;
        }

        void ButtonPort_OnInterrupt(uint data1, uint data2, System.DateTime time)
        {
            var isPressed = data2 == 0;

            if (isPressed && !PrevPressed && ButtonPressed != null)
                ButtonPressed(this, ButtonState.Pressed);
            else if (!isPressed && PrevPressed && ButtonReleased != null)
                ButtonReleased(this, ButtonState.Released);

            PrevPressed = isPressed;
        }

        public bool IsPressed
        {
            get
            {
                lock (this)
                {
                    return !ButtonPort.Read();
                }
            }
        }
    }
}
