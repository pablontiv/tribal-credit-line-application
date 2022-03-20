using tribal_credit_line_application.Model;

namespace tribal_credit_line_application.Repository
{
    public class ApplicationRepository
    {
        private readonly Dictionary<int, ApplicationResult> applications = new Dictionary<int, ApplicationResult>();

        public ApplicationResult? GetById(int id)
        {
            if (!applications.ContainsKey(id))
                return null;

            return applications[id];
        }

        public void Add(int id, ApplicationResult result)
        {
            if (applications.ContainsKey(id))
                return;

            applications.Add(id, result);
        }
    }
}
