using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTID.BusinessLogic.Models
{
    public class SourceFile
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public string OriginalName { get; set; }
        public Indicator Indicator { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }

        public SourceFile()
        {
            Name = RandomString(10);
        }

        private static Random random = new Random();
        private static string RandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            return new string(Enumerable.Repeat(chars, length)
              .Select(s => s[random.Next(s.Length)]).ToArray());
        }
    }
}
