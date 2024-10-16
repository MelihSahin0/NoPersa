﻿using System.ComponentModel.DataAnnotations;
using Website.Client.Models;

namespace Website.Client.FormModels
{
    public class CustomerSeq
    {
        [Required]
        public required List<SequenceDetail> SequenceDetails { get; set; }

        [Required]
        public required int[] SelectedRouteDetailsId { get; set; }

        [Required]
        public required List<RouteOverview> RouteOverview { get; set; }
    }
}
