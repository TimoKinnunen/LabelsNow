using SQLite;
using TableAttribute = SQLite.TableAttribute;

namespace LabelsNow.Models
{
    // SQLite class
    [Table("LabelAddresses")]
    public class LabelAddress
    {
        [PrimaryKey]
        public string Guid { get; set; }
        public string Line1 { get; set; }
        public string Line2 { get; set; }
        public string Line3 { get; set; }
        public string Line4 { get; set; }
        public string Line5 { get; set; }
        public string Line6 { get; set; }
        public string Line7 { get; set; }
        public string Line8 { get; set; }
    }
}
