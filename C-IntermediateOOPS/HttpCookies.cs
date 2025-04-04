﻿

using System;
using System.Collections.Generic;

namespace C_Intermediate
{
    internal class HttpCookies
    {
        private readonly Dictionary<string, string> _dictionary;
        public  DateTime Expiry { get; set; }
        public HttpCookies() { 
            _dictionary = new Dictionary<string, string>();
        }
        public string this[string key]
        {
            get { return _dictionary[key]; }
            set { _dictionary[key] = value;}
        }

    }
}
