namespace IntegracaoBC.Domain.Mappings
{
    public class NewWorkScheduleRequest
    {
        public string id { get; set; }
        public string start_date { get; set; }
        public string end_date { get; set; }
        public string start_time { get; set; }
        public string end_time { get; set; }
        public int duration { get; set; }
        public string weekdays { get; set; }
    }
}
