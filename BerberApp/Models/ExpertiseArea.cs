using System.ComponentModel;

namespace BerberApp.Models
{
    using System.ComponentModel;

    public enum ExpertiseArea
    {
        [Description("Saç Kesimi")]
        Haircut = 1,

        [Description("Sakal Kesimi")]
        BeardTrim = 2,

        [Description("Saç Boyama")]
        HairColoring = 3,

        [Description("Cilt Bakımı")]
        FacialCare = 4,

        [Description("Çocuk Saç Kesimi")]
        ChildHaircut = 5,

        [Description("Masaj")]
        Massage = 6,

        [Description("Fön Çekmek")]
        BlowDry = 7,

        [Description("Manikür")]
        Manicure = 8,
    }


}
