using NodaTime;

namespace Application.Servises
{
    public class DateTimeService
    {
        private readonly DateTimeZone _timeZone;

        public DateTimeService()
        {
            _timeZone = NodaTime.TimeZones.BclDateTimeZone.ForSystemDefault();
        }

        public LocalDateTime GetCurrentLocalDateTime()
        {
            Instant now = SystemClock.Instance.GetCurrentInstant();
            var zonedDateTime = now.InZone(_timeZone);
            return zonedDateTime.LocalDateTime;
        }
    }
}
