using System.ComponentModel;

namespace HomeBankingMindHub.Models
{
    public enum CardType
    {
        [Description("DEBIT")]
        DEBIT=1,
        [Description("CREDIT")]
        CREDIT=2
    }
}
