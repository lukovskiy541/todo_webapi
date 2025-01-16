namespace todo_webapi.Models
{
    public class TodoItem
    {
        public int Id { get; set; }
        public string? Name { get; set; }
        public bool IsComplete { get; set; }
        public int? CategoryId { get; set; } 
        public Category? Category { get; set; }
    }
}
