using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _EN
{
    public interface ITipoMoneda
    {
        Int64 idTipoMoneda { get; set; }
        string codigo { get; set; }
        string moneda { get; set; }
        bool favorite { get; set; }
        Nullable<DateTime> createdDate { get; set; }
        Nullable<DateTime> modifiedDate { get; set; }
        bool enable { get; set; }
    }

 
    public interface IFocusFactories
    {
        int focusFactoryId { get; set; }
        string focusFactory { get; set; }
    }

    public interface IAreas
    {
        int areaId { get; set; }
        string area { get; set; }
    }

    public interface IReelStatus
    {
        int statusReelId { get; set; }
        string statusReel { get; set; }
    }

    public interface ILanguage
    {
        int language { get; set; }
    }
}
