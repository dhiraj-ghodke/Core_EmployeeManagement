using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace EM.Models
{
    [MetadataType(typeof(TeamMetadata))]
    public partial class Team
    {
    }

    public partial class TeamMetadata
    {
        public int TeamId { get; set; }

        [DisplayName("Team Number")]
        public string TeamName { get; set; }
    }
}