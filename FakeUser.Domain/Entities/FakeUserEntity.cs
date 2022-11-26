

namespace FakeUser.Domain.Entities
{
    public class FakeUserEntity : BaseEntity
    {
        public int UserId { get; set; }
        public string Title { get; set; }
        public string Body { get; set; }
    }
}
