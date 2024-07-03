using FakeNewsDetection.Entities;
using System.ComponentModel.DataAnnotations;

namespace FakeNewsDetection.Models
{
    public class NewsData
    {
        public string? Title { get; set; } = "";

        [Required]
        public string? Text { get; set; }

        [Required]
        public TextType Type { get; set; }

        public string? Result { get; set; }
    }
}
