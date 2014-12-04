using System;
using System.Text;

namespace NumberToString
{
    internal class Program
    {
        private static LangId glbLanguage;

        private enum Errors
        {
            Success = 0,
            InvalidParametersCount = -1,
            IllegalNumber = -2,
            TooBigNumber = -3
        }

        private enum LangId
        {
            Ru = 0,
            En
        }

        private static readonly string[] digitNamesTo20Ru =
        {
            string.Empty, "один", "два", "три", "четыре", "пять", 
            "шесть", "семь", "восемь", "девять", "десять", 
            "одиннадцать", "двенадцать", "тринадцать", "четырнадцать", "пятнадцать", 
            "шестнадцать", "семнадцать", "восемнадцать", "девятнадцать"
        };

        private static readonly string[] digitNamesTensRu =
        {
            string.Empty, "десять", "двадцать", "тридцать", "сорок", "пятьдесят", 
            "шестьдесят", "семьдесят", "восемьдесят", "девяносто"
        };

        private static readonly string[] digitNamesHundredsRu =
        {
            string.Empty, "сто", "двести", "триста", "четыреста", "пятьсот", 
            "шестьсот", "семьсот", "восемьсот", "девятьсот"
        };

        private static readonly string[] digitNamesOverRu =
        {
            "тысяч", "миллионов", "миллиардов", "триллионов", "квадриллионов", 
            "квинтиллионов", "секстиллионов", "септиллионов", "октиллионов", "нониллионов", 
            "дециллионов", "ундециллионов", "додециллионов", "тредециллионов", "кваттуордециллионов", 
            "квиндециллионов", "cедециллионов", "септдециллионов", "дуодевигинтиллионов", "ундевигинтиллионов", 
            "вигинтиллионов", "анвигинтиллионов"
        };

        private static readonly string[] digitNamesTo20En =
        {
            string.Empty, "one", "two", "three", 
            "four", "five", "six", "seven", 
            "eight", "nine", "ten", "eleven", 
            "twelve", "thirteen", "fourteen", "fifteen", 
            "sixteen", "seventeen", "eighteen", "nineteen"
        };

        private static readonly string[] digitNamesTensEn =
        {
            string.Empty, "ten", "twenty", "thirty", 
            "forty", "fifty", "sixty", "seventy", 
            "eighty", "ninety"
        };

        private static readonly string[] digitNamesOverEn =
        {
            "hundred", "thousand", "million", "billion", 
            "trillion", "quadrillion", "quintillion", "sextillion", 
            "septillion", "octillion", "nonillion", "decillion", 
            "undecillion", "duodecillion", "tredecillion", "quattuordecillion", 
            "quindecillion", "sexdecillion", 
            "septendecillion", "octodecillion", 
            "novemdecillion", "vigintillion"
        };

        private static void Usage()
        {
            if (glbLanguage == LangId.Ru)
            {
                Console.WriteLine("NumberToString (перевод строки в число) версия 0.1");
                Console.WriteLine("Использовать: numbertostring число [-язык(RU|EN)]");
            }
            else
            {
                Console.WriteLine("NumberToString version 0.1");
                Console.WriteLine("Usage: numbertostring number [-language(RU|EN)]");
            }
        }

        private static string GetLocaleString(Errors error)
        {
            switch (error)
            {
                case Errors.IllegalNumber:
                    return glbLanguage == LangId.Ru ? "Недопустимое число " : "Illegal number ";

                case Errors.TooBigNumber:
                    return glbLanguage == LangId.Ru ? "Слишком большое число, максимум " : "Number too large, maximum ";
            }

            return string.Empty;
        }

        private static void CorrectEndOfWord(int number, int length, int correctionOffset, ref string outString)
        {
            var sb = new StringBuilder(outString);

            if ((length / 3) == 1)
            {
                if (number == 1)
                {
                    sb[correctionOffset - 2] = 'н';
                    sb[correctionOffset - 1] = 'а';
                    sb.Append("а");

                    outString = sb.ToString();
                    return;
                }

                if (number == 2)
                {
                    sb[correctionOffset - 1] = 'е';
                }

                sb.Append("и");

                outString = sb.ToString();
                return;
            }

            if ((length / 3) <= 1)
            {
                return;
            }

            if (number == 1)
            {
                outString = outString.Remove(outString.Length - 2, 2);
                return;
            }

            if (number >= 5)
            {
                return;
            }

            outString = outString.Remove(outString.Length - 2, 2);
            outString += 'а';
        }

        private static int Number(int length, string sourceString, ref string outString)
        {
            int result = 0;
            int sourceStringIdx = 0;
            var sb = new StringBuilder(outString);

            switch (length)
            {
                case 3:
                    if (sourceString[sourceStringIdx] != '0')
                    {
                        if (glbLanguage == LangId.Ru)
                            sb.Append(digitNamesHundredsRu[sourceString[sourceStringIdx] - '0']);
                        else
                        {
                            sb.Append(digitNamesTo20En[sourceString[sourceStringIdx] - '0']);
                            sb.Append(" ");
                            sb.Append(digitNamesOverEn[0]);
                        }
                        result = 20;
                    }

                    sourceStringIdx++;
                    goto case 2;

                case 2:
                    int value = (sourceString[sourceStringIdx + 1] - '0')
                        + (sourceString[sourceStringIdx] - '0') * 10;

                    if (value != 0)
                    {
                        if (result != 0)
                        {
                            sb.Append(" ");
                        }

                        if (value < 20)
                        {
                            sb.Append(glbLanguage == LangId.Ru ? digitNamesTo20Ru[value] : digitNamesTo20En[value]);
                        }
                        else
                        {
                            sb.Append(glbLanguage == LangId.Ru
                                ? digitNamesTensRu[value / 10]
                                : digitNamesTensEn[value / 10]);

                            if (value % 10 != 0)
                            {
                                if (glbLanguage == LangId.Ru)
                                {
                                    sb.Append(" ");
                                    sb.Append(digitNamesTo20Ru[value % 10]);
                                }
                                else
                                {
                                    sb.Append("-");
                                    sb.Append(digitNamesTo20En[value % 10]);
                                }

                                value = value % 10;
                            }
                        }
                    }

                    outString = sb.ToString();
                    result = value;
                    break;

                case 1:
                    result = sourceString[0] - '0';

                    if (result != 0)
                    {
                        sb.Append(glbLanguage == LangId.Ru ? digitNamesTo20Ru[result] : digitNamesTo20En[result % 10]);
                    }

                    outString = sb.ToString();
                    break;
            }

            return result;
        }

        private static void Unit(int length, string sourceString, ref string outString)
        {
            int result = 0;

            var sb = new StringBuilder();
            var sourceStringStringBuilder = new StringBuilder(sourceString);

            if (length > 3)
            {
                int correctOff;
                int num;
                if (length % 3 != 0)
                {
                    int offset = length % 3;
                    length -= offset;

                    num = Number(offset, sourceStringStringBuilder.ToString(), ref outString);
                    sb.Append(outString);

                    if (num != 0)
                    {
                        if (glbLanguage == LangId.Ru)
                        {
                            correctOff = outString.Length;
                            sb.Append(" ");
                            sb.Append(digitNamesOverRu[(length / 3) - 1]);

                            outString = sb.ToString();
                            CorrectEndOfWord(num, length, correctOff, ref outString);
                        }
                        else
                        {
                            sb.Append(" ");
                            sb.Append(digitNamesOverEn[(length / 3)]);

                            outString = sb.ToString();
                        }

                        result = 1;
                    }

                    sourceStringStringBuilder.Remove(0, offset);
                }

                while (length > 3)
                {
                    length -= 3;
                    outString += " ";

                    num = Number(3, sourceStringStringBuilder.ToString(), ref outString);
                    if (num != 0)
                    {
                        if (glbLanguage == LangId.Ru)
                        {
                            correctOff = outString.Length;
                            outString += " ";
                            outString += digitNamesOverRu[(length / 3) - 1];

                            CorrectEndOfWord(num, length, correctOff, ref outString);
                        }
                        else
                        {
                            outString += " ";
                            outString += digitNamesOverEn[(length / 3)];
                        }

                        result = 1;
                    }

                    sourceStringStringBuilder.Remove(0, 3);
                }
            }

            if (result == 1)
            {
                outString += " ";
            }

            Number(length, sourceStringStringBuilder.ToString(), ref outString);
        }

        private static void NumberToString(string number, ref string convertedString)
        {
            int idx = 0;
            while (number[idx] == '0')
            {
                idx++;
            }

            number = number.Remove(0, idx);

            if (number.Length == 0)
            {
                convertedString = glbLanguage == LangId.Ru ? "ноль" : "zero";
                return;
            }

            if (number[0] == '-')
            {
                convertedString = glbLanguage == LangId.Ru ? "минус" : "minus";
                number = number.Remove(0, 1);
            }

            Unit(number.Length, number, ref convertedString);
        }

        private static string ToOctRadix(double decValue)
        {
            if (decValue > float.MaxValue)
            {
                return glbLanguage == LangId.Ru ? "Слишком большое число" : "Value too large";
            }

            double value = Math.Abs(Math.Floor(decValue));
            int depth = 0;
            var preresult = new StringBuilder();

            while (value > 8)
            {
                double remainder = value % 8.0;
                char ch = (char)((char)remainder + '0');
                value = Math.Floor(value / 8.0);
                preresult.Append(ch);
                ++depth;
            }

            preresult.Append((char)((char)value + '0'));

            var result = new StringBuilder(depth + 1);

            if (decValue < 0)
            {
                result.Append('-');
            }
            while (depth > -1)
            {
                result.Append(preresult[depth]);
                --depth;
            }

            return result.ToString();
        }

        private static void Main(string[] args)
        {
            glbLanguage = LangId.Ru;

            if (args.Length > 2)
            {
                Usage();
                return;
            }

            string number = string.Empty;
            bool needReadNumber = false;
            if (args.Length == 0)
            {
                needReadNumber = true;
            }
            else
            {
                number = args[0].ToUpper();

                string lang = args.Length == 2 ? args[1].ToUpper() : number;

                switch (lang)
                {
                    case "-EN":
                        glbLanguage = LangId.En;
                        break;
                    case "-RU":
                        glbLanguage = LangId.Ru;
                        break;
                    default:
                        lang = string.Empty;
                        break;
                }

                if ((args.Length == 1) && !string.IsNullOrEmpty(lang))
                {
                    number = string.Empty;
                }

                if (string.IsNullOrEmpty(number))
                {
                    needReadNumber = true;
                }
            }

            if (needReadNumber)
            {
                Usage();

                Console.Write(glbLanguage == LangId.Ru ? "Ввидите число: " : "Input number: ");

                number = Console.ReadLine();
            }

            double val;
            try
            {
                val = Convert.ToDouble(number);
            }
            catch (Exception)
            {
                Console.WriteLine("{0}{1}", GetLocaleString(Errors.IllegalNumber), number);
                return;
            }

            if (number != null && number.Length > 65)
            {
                Console.Write("{0}{1}", GetLocaleString(Errors.TooBigNumber), 65);
                Console.WriteLine(glbLanguage == LangId.Ru ? " цифр." : " digits.");

                return;
            }

            string str = string.Empty;

            NumberToString(number, ref str);

            Console.Write("   {0}", str);
            Console.Write(glbLanguage == LangId.Ru ? " в десятичной системе" : " in decimal");
            Console.WriteLine(";");

            str = string.Empty;

            number = ToOctRadix(val);
            NumberToString(number, ref str);

            Console.Write("   {0}", str);
            Console.Write(glbLanguage == LangId.Ru ? " в восьмеричной системе" : " in octal");
            Console.WriteLine(".");
        }
    }
}
