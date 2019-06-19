using System;
using System.Collections;
using System.Text;
using System.Threading;
using Bauland.Others;

namespace Min
{
    public interface IAnimation
    {
        void Do();
    }

    public class Animation1 : IAnimation
    {
        private int delay = 10;
        private int longDelay = 500;
        private byte _brightness;
        private Color _color;
        private LedStrip _ledStrip;

        public Animation1(byte brightness, Color color, LedStrip ledStrip)
        {
            _brightness = brightness;
            _color = color;
            _ledStrip = ledStrip;
        }
        public void Do()
        {
            for (byte intensity = _brightness; intensity < LedStrip.IntensityMax + 1; intensity--)
            {
                _ledStrip.SetAll(intensity, _color);
                _ledStrip.Show();
                Thread.Sleep(delay);
            }
            Thread.Sleep(longDelay);
        }
    }
    public class AnimationNothing : IAnimation
    {
        private readonly int _delay;

        public AnimationNothing(Random random)
        {
            _delay = random.Next(2000) + 2000;
        }
        public void Do()
        {
            Thread.Sleep(_delay);
        }
    }

    public class Animation2 : IAnimation
    {
        private int delay = 10;
        private int longDelay = 500;
        private byte _brightness;
        private Color _color;
        private LedStrip _ledStrip;

        public Animation2(byte brightness, Color color, LedStrip ledStrip)
        {
            _brightness = brightness;
            _color = color;
            _ledStrip = ledStrip;
        }
        public void Do()
        {
            for (byte intensity = 0; intensity <= _brightness; intensity++)
            {
                _ledStrip.SetAll(intensity, _color);
                _ledStrip.Show();
                Thread.Sleep(delay);
            }
            for (byte intensity = _brightness; intensity < LedStrip.IntensityMax + 1; intensity--)
            {
                _ledStrip.SetAll(intensity, _color);
                _ledStrip.Show();
                Thread.Sleep(delay);
            }
            Thread.Sleep(longDelay);
        }
    }


}
