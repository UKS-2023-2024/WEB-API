using Domain.Auth;
using Domain.Comments;

namespace Domain.Reactions;

public class Reaction
{
    public Guid Id { get; private set; }
    public Guid CommentId { get; private set; }
    public Comment Comment { get; private set; }
    public User Creator { get; private set; }
    public Guid CreatorId { get; private set; }
    public string EmojiCode { get; private set; }

    public Reaction()
    {
    }

    public Reaction(Guid commentId, Guid creatorId, string emojiCode)
    {
        CommentId = commentId;
        CreatorId = creatorId;
        EmojiCode = emojiCode;
    }

    public static Reaction Create(Guid commentId, Guid creatorId, string emojiCode)
    {
        return new Reaction(commentId, creatorId, emojiCode);
    }
}