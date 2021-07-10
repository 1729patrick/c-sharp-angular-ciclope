

using System;
using System.Globalization;

namespace Ciclope.Services
{
    public class Currency : ICurrency
    {
        public Currency(){

        }
        
        public string Format(double value)
        {
            return value.ToString("C", new CultureInfo("pt-PT"));
        }

        
    }
}
