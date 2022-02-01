namespace TO_DO.ENTİTY.Models
{
    public class Todo
    {
        public string TodoId { get; set; }
        public string UserName { get; set; }
        public string Content { get; set; }
        public DateTime WriteDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public bool IsActive { get; set; }
    }
}