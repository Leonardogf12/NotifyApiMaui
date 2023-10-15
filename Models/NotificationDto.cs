namespace NotifyApiMaui.Models
{
    public class NotificationDto
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public string? UrlImage { get; set; }
        public DateTime DateTimeOfNotification { get; set; }
        public string? Token { get; set; }
    }
}
