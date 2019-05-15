using System.ComponentModel;

namespace AssinaturaDigital.Models
{
    public enum DocumentOrientation
    {
        [Description("Frente")]
        Front,
        [Description("Verso")]
        Back,
        [Description("FrenteEVerso")]
        FrontAndBack
    }
}
