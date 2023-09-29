using DAL.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DAL.ViewModels
{
    public class SubscriberVM
    {
        public string Id { get; set; }
        public string FirstName { get; set; } 
        public string LastName { get; set; } 
        public DateTime DateOfBirth { get; set; }
        public DateTime JoinedOn { get; set; }
        public string NationalId { get; set; } 
        public string PhoneNumber { get; set; } 
        public string Email { get; set; } 
        public bool HasWhatsapp { get; set; }
        public string ProfilePhoto { get; set; }
        public string Governorate { get; set; } 
        public string Area { get; set; } 
        public string ZipCode { get; set; } 
        public string Address { get; set; } 
        public bool IsBlocked { get; set; }
    }
}
