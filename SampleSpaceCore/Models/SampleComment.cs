namespace SampleSpaceCore.Models;

public class SampleComment
{
    private const int MaxCommentLength = 300;

    private SampleComment(Guid sampleCommentGuid, Guid sampleGuid, Guid userGuid, DateTime date, string comment,
        User? user)
    {
        SampleCommentGuid = sampleCommentGuid;
        SampleGuid = sampleGuid;
        UserGuid = userGuid;
        Date = date;
        Comment = comment;
        User = user;
    }

    public Guid SampleCommentGuid { get; private set; }

    public Guid SampleGuid { get; private set; }

    public Guid UserGuid { get; private set; }

    public DateTime Date { get; private set; }

    public string Comment { get; private set; }

    public User? User { get; private set; }

    public static (SampleComment? SampleComment, string Error) Create(Guid sampleCommentEntityGuid, Guid sampleGuid,
        Guid userGuid, DateTime date, string comment, User? user)
    {
        if (string.IsNullOrEmpty(comment) || comment.Length > 300)
            return (null, $"Comment cannot be empty or longer then {MaxCommentLength} symbols");
        
        var sampleComment = new SampleComment(sampleCommentEntityGuid, sampleGuid, userGuid, date, comment, user);

        return (sampleComment, string.Empty);
    }

    public (SampleComment? SampleComment, string error) Edit(string newCommentValue)
    {
        if(string.IsNullOrEmpty(newCommentValue) || newCommentValue.Length > 300)
            return (null, $"Comment cannot be empty or longer then {MaxCommentLength} symbols");

        Comment = newCommentValue;
        Date = DateTime.Today;
        
        return (this, string.Empty);
    }
}