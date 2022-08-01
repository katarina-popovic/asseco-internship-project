namespace TransactionAPI.Database.Entities
{
    public class SplitEntity
    {
        public int Id { get; set; }
        public double Amount { get; set; }
        public string TransactionId { get; set; }
        public string CategoryCode { get; set; }
        public virtual TransactionEntity Transaction { get; set; }
        public virtual CategoryEntity Category { get; set; }
    }
}
