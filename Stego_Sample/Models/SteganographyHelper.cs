using System.Drawing;

namespace Stego_Sample.Models
{
    public class SteganographyHelper
    {
        public static Bitmap MergeText(string text, Bitmap bmp)
        {
            var s = State.Hiding;

            var charIndex = 0;
            var charValue = 0;
            long colorUnitIndex = 0;

            var zeros = 0;

            int R = 0, G = 0, B = 0;

            for (var i = 0; i < bmp.Height; i++)
            {
                for (var j = 0; j < bmp.Width; j++)
                {
                    var pixel = bmp.GetPixel(j, i);

                    pixel = Color.FromArgb(pixel.R - pixel.R%2,
                        pixel.G - pixel.G%2, pixel.B - pixel.B%2);

                    R = pixel.R;
                    G = pixel.G;
                    B = pixel.B;

                    for (var n = 0; n < 3; n++)
                    {
                        if (colorUnitIndex%8 == 0)
                        {
                            if (zeros == 8)
                            {
                                if ((colorUnitIndex - 1)%3 < 2)
                                {
                                    bmp.SetPixel(j, i, Color.FromArgb(R, G, B));
                                }

                                return bmp;
                            }

                            if (charIndex >= text.Length)
                            {
                                s = State.FillingWithZeros;
                            }
                            else
                            {
                                charValue = text[charIndex++];
                            }
                        }

                        switch (colorUnitIndex%3)
                        {
                            case 0:
                            {
                                if (s == State.Hiding)
                                {
                                    R += charValue%2;

                                    charValue /= 2;
                                }
                            }
                                break;
                            case 1:
                            {
                                if (s == State.Hiding)
                                {
                                    G += charValue%2;

                                    charValue /= 2;
                                }
                            }
                                break;
                            case 2:
                            {
                                if (s == State.Hiding)
                                {
                                    B += charValue%2;

                                    charValue /= 2;
                                }

                                bmp.SetPixel(j, i, Color.FromArgb(R, G, B));
                            }
                                break;
                        }

                        colorUnitIndex++;

                        if (s == State.FillingWithZeros)
                        {
                            zeros++;
                        }
                    }
                }
            }

            return bmp;
        }

        public static string ExtractText(Bitmap bmp)
        {
            var colorUnitIndex = 0;
            var charValue = 0;

            var extractedText = string.Empty;

            for (var i = 0; i < bmp.Height; i++)
            {
                for (var j = 0; j < bmp.Width; j++)
                {
                    var pixel = bmp.GetPixel(j, i);

                    for (var n = 0; n < 3; n++)
                    {
                        switch (colorUnitIndex%3)
                        {
                            case 0:
                            {
                                charValue = charValue*2 + pixel.R%2;
                            }
                                break;
                            case 1:
                            {
                                charValue = charValue*2 + pixel.G%2;
                            }
                                break;
                            case 2:
                            {
                                charValue = charValue*2 + pixel.B%2;
                            }
                                break;
                        }

                        colorUnitIndex++;

                        if (colorUnitIndex%8 == 0)
                        {
                            charValue = ReverseBits(charValue);

                            if (charValue == 0)
                            {
                                return extractedText;
                            }

                            var c = (char) charValue;

                            extractedText += c.ToString();
                        }
                    }
                }
            }

            return extractedText;
        }

        public static int ReverseBits(int n)
        {
            var result = 0;

            for (var i = 0; i < 8; i++)
            {
                result = result*2 + n%2;

                n /= 2;
            }

            return result;
        }

        private enum State
        {
            Hiding,
            FillingWithZeros
        }
    }
}