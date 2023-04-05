

using Application.Specifications;
using Degano_API.Models.Entities;

namespace Models.Specifications
{
    public class UserSubscriptionSpecification : BaseSpecification<Subscription>
    {
        public UserSubscriptionSpecification(Guid userId) : base()
        {
            AddInclude(sub => sub.User);
            AddInclude(sub => sub.Offer);
            ApplyCriteria(sub => sub.User.Id == userId);
            ApplyOrderByDescending(sub => sub.OrderDate);
        }
    }
}
