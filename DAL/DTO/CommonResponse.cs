namespace DAL.DTO
{
    public class CommonResponse
    {
        public bool Result { get; set; }
        public string Message { get; set; } = null!;

        public List<string> Value { get; set; }
    }
}
