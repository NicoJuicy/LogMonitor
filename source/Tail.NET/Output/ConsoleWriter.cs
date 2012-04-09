﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Tail.Processors
{
    internal class ConsoleWriter : IProcessor
    {
        private string lastName = null;

        public void OnContentAdded(object sender, ContentEventArgs e)
        {
            if (lastName != e.FullName)
            {
                var color = Console.ForegroundColor;
                Console.ForegroundColor = ConsoleColor.White;

                Console.WriteLine();
                Console.WriteLine();

                Console.WriteLine(new String('-', 40));
                
                Console.WriteLine(e.FullName);

                Console.ForegroundColor = color;

                lastName = e.FullName;
            }

            Console.Write(e.AddedContent);
        }
    }
}
