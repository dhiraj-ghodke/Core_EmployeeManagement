using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace EM.Models
{
    [MetadataType(typeof(BatchMetadata))]
    public partial class Batch
    {
    }

    public class BatchMetadata
    {
        public int BatchId { get; set; }

        [Display(Name = "Batch No")]
        public string BatchName { get; set; }
    }
}