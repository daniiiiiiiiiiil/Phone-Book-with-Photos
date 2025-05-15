using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Телефонный_справочник_с_фотографиями
{
    internal class PhoneItem
    {
        public int Id { get; set; }
        public string PhoneNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public byte[] Photo { get; set; }
        public PhoneItem() { }
        public PhoneItem(int id,string phoneNumber,string firstName,string lastName, byte[] photo)
        {
            Id = id;
            PhoneNumber = phoneNumber;
            FirstName = firstName;
            LastName = lastName;
            Photo = photo;
        }
        public PhoneItem(string phoneNumber, string firstName, string lastName, byte[] photo)
        {
            PhoneNumber = phoneNumber;
            FirstName = firstName;
            LastName = lastName;
            Photo = photo;
        }
    }
}
