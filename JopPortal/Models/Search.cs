using Microsoft.EntityFrameworkCore;

namespace JopPortal.Models
{
    public enum J_Category
    {
        Any,
        Customer_Service,
        Sales,
        Software_Development,
        Marketing,
        Instructor,
        Education,
        Media,
        Medical
    }
    public enum J_Type
    {
        Any,
        Full_Time,
        Internship,
        Freelance,
        Part_Time,
    }
    public enum J_Location
    {
        Any,
        Alexandria,
        Aswan,
        Asyut,
        Beheira,
        BeniSuef,
        Cairo,
        Dakahlia,
        Damietta,
        Faiyum,
        Gharbia,
        Giza,
        Ismailia,
        KafrElSheikh,
        Luxor,
        Matruh,
        Minya,
        Monufia,
        NewValley,
        NorthSinai,
        PortSaid,
        Qalyubia,
        Qena,
        RedSea,
        Sharqia,
        Sohag,
        SouthSinai,
        Suez
    }
    [Keyless]
    public class Search
    {

        public J_Category Category { get; set; }

        public J_Type Type { get; set; }


        public J_Location Location { get; set; }

        public int Min_Salary { get; set; }

        public int Max_Salary { get; set; }
    }
}