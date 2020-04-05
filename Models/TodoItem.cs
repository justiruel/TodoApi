using System.ComponentModel.DataAnnotations;

namespace TodoApi.Models
{
    public class TodoItem
    {
        public long Id { get; set; }
        public string Title { get; set; }
        public string ExpirateDate { get; set; }
        public string Description { get; set; }
        public int PercentageComplete { get; set; }
    }

    public class createTodoDto
        {
            [Required]
            public string Title { get; set; }
            [Required]
            public string ExpirateDate { get; set; }
            [Required]
            public string Description { get; set; }
            [Required]
            public int PercentageComplete { get; set; }
        }

    public class ReturnData
        {
            public string Success { get; set; }
        }
}


