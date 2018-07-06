using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace DTID.BusinessLogic.Models
{
    public class Attachment
    {
        public int ID { get; set; }
        public int InterpretationId { get; set; }
        public Interpretation Interpretation { get; set; }
        public string HashedName { get; set; }
        public string Filename { get; set; }
        public string Mime { get; set; }
        public string Extension { get; set; }
        public DateTime DateCreated { get; set; }
        public DateTime DateUpdated { get; set; }
        public string Newname
        {
            get
            {
                return $"{HashedName}.{Extension}";
            }
        }

        public Attachment()
        {
            HashedName = RandomString(10);
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
