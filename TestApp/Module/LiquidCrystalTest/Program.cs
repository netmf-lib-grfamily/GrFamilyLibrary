using System.Threading;
using GrFamily.MainBoard;
using GrFamily.Module;

namespace LiquidCrystalTest
{
    public class Program
    {
        public static void Main()
        {
            Thread.Sleep(1000);

            // �J�����̕W���s���A�T�C��
            var lcd = new LiquidCrystal(Pins.GPIO_PIN_D9, Pins.GPIO_PIN_D8,
                Pins.GPIO_PIN_D7, Pins.GPIO_PIN_D6, Pins.GPIO_PIN_D5, Pins.GPIO_PIN_D4);
            // �s���ύX�e�X�g
            //var lcd = new LiquidCrystal(Pins.GPIO_PIN_D9, Pins.GPIO_PIN_D8,
            //    Pins.GPIO_PIN_D3, Pins.GPIO_PIN_D2, Pins.GPIO_PIN_D1, Pins.GPIO_PIN_D0);

            // GR-PEACH�ł̓R�}���h�Ԃ̃E�F�C�g�Ȃ����삷��i�E�F�C�g����ł����Ȃ��j
            // GR -SAKURA�ł̓E�F�C�g�����Ȃ��ƌ듮�삷��
            lcd.InitDevice(5);

            while (true)
            {
                lcd.SetCursor(0, 0);
                lcd.Print("GRFamily Library");
                lcd.SetCursor(1, 0);
                lcd.Print("LiquidCrystal !!");

                Thread.Sleep(3000);

                lcd.SetCursor(0, 0);
                lcd.Print("Hello, NETMF !! ");
                lcd.SetCursor(1, 0);
                lcd.Print("Lib for GR-PEACH");

                Thread.Sleep(3000);
            }
        }
    }
}
