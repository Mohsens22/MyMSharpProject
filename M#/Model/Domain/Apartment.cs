using System;
using System.Collections.Generic;
using System.Text;
using MSharp;

namespace Model
{
    public class Apartment : EntityType
    {
        public Apartment()
        {
            String("Name").Mandatory();
            DateTime("BuildTime");
        }
    }
}