﻿namespace ServerSide.Models.ResponseModels
{
    public class ClaimsModelUpdate
    {
        public string roleId { get; set; }
        public List<string> claimsList { get; set; } = new List<string>();
    }
}
