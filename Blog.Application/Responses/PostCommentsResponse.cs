using Blog.Application.ViewModels;

namespace Blog.Application.Responses
{
    public class PostCommentsResponse
    {
        public int PostId { get; set; }
        public List<PostCommentViewModel> Comments { get; set; } = new List<PostCommentViewModel>();
        public int TotalComments { get; set; }
    }
}
