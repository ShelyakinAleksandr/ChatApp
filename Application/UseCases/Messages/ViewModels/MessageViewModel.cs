using Domain;
using NodaTime;

namespace Application.UseCases.Messages.ViewModels
{
    public class MessageViewModel
    {
        public Guid Id { get; set; }
        public LocalDateTime CreatedDate { get; set; }
        public User Autor { get; set; }
        public string MessageText { get; set; }
    }
}
