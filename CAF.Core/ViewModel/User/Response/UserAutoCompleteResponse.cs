namespace CAF.Core.ViewModel.User.Response
{
    public class UserAutoCompleteResponse
    {
        public long Id { get; set; }
        public string UserName { get; set; }

        public UserAutoCompleteResponse()
        {

        }
        public UserAutoCompleteResponse(Entities.User entity)
        {
            if (entity == null) return;

            Id = entity.Id;
            UserName = $"{entity.FirstName} {entity.LastName}";
        }
    }
}
