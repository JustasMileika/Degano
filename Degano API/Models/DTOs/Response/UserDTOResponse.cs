namespace Degano_API.Models.DTOs.Response
{
    public class UserDTOResponse
    {
        public UserDTOResponse(Guid id)
        {
            Id = id;
        }

        public Guid Id { get; set; }
    }
}
