using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata;
using System.Text;
using System.Threading.Tasks;

namespace SurveyShop.Utility
{
    public static class SD
    {
        public const string Role_Customer = "Customer";
        public const string Role_Company = "Company";
        public const string Role_Admin = "Admin";
        public const string Role_Employee = "Employee";

        public const string Order_Pending = "Pending";
        public const string Order_Approved = "Approved";
        public const string Order_Proccesing = "Processing";
        public const string Order_Shipped = "Shipped";
        public const string Order_Cancelled = "Cancelled";

        public const string Payment_Pending = "Pending";
        public const string Payment_Paid = "Paid";
        public const string Payment_Refunded = "Refunded";
        public const string Payment_AllowedLatePayment = "Late Payment Allowed";
    }
}
