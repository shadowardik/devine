namespace devine.Models
{
    public class DetectionInfo
    {
        public string Description { get; set; }
        
        public DetectionInfo(string description)
        {
            Description = description;
        }
    }
}