using static App.Models.ElectricalEquipment;

namespace App.ExtendMethods
{
    public static class StateEquipToVN
    {
        public static string StateEquipToVns (this StatusEquip ToVN)
        {
            switch (ToVN)
            {
                case StatusEquip.NonActive:
                    return "<p class='text-primary'>Máy trống</p>";

                case StatusEquip.Active:
                    return "<p class='text-warning'>Máy đang hoạt động</p>";

                default: return "";
            }




        }
    }
}
