using System;

namespace MovieTicketsAPI.Models
{
    public class ReservationResponse
    {
        public string Id { get; set; }
        public DateTime ReservationTime { get; set; }
        public string CustomerName { get; set; }
        public string MovieName { get; set; }
        public string MovieImageUrl { get; set; }
        public int TotalPages { get; set; }

    }
}
