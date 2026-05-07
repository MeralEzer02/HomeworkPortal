namespace HomeworkPortal.API.Models
{
    public class Badge
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty; 
        public string Description { get; set; } = string.Empty;    
        public string Icon { get; set; } = string.Empty;           
        public string ConditionType { get; set; } = string.Empty;  
        public int RequiredCount { get; set; }                     
    }
}