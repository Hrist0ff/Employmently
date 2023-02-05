namespace employmently_be.Data.Entities
{
    public class ForgotPassword
    {
        public int Id { get; set; }
        public string Token { get; set; }
        public string UserId { get; set; }
    }
}
