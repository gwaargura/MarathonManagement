using System;

namespace MM_api.DTOs
{
    // Read DTO — for returning checkpoint data to the client
    public class ReadCheckpointDTO
    {
        public string? Name { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public int Sequence { get; set; }
    }

    // Create DTO — for creating new checkpoints
    public class CreateCheckpointDTO
    {
        public int MarathonId { get; set; }
        public string? Name { get; set; }
        public decimal Latitude { get; set; }
        public decimal Longitude { get; set; }
        public int Sequence { get; set; }
    }

    // Update DTO — for updating existing checkpoints
    public class UpdateCheckpointDTO
    {
        public string? Name { get; set; }
        public decimal? Latitude { get; set; }
        public decimal? Longitude { get; set; }
        public int? Sequence { get; set; }
    }
}
