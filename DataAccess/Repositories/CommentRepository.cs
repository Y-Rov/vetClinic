using Core.Entities;
using Core.Interfaces.Repositories;
using DataAccess.Context;

namespace DataAccess.Repositories;

public class CommentRepository : Repository<Comment>, ICommentRepository
{
    public CommentRepository(ClinicContext clinicContext) : base(clinicContext)
    {
    }
}