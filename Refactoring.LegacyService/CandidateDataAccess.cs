namespace Refactoring.LegacyService
{
    using Refactoring.LegacyService.Interfaces;
    using System.Configuration;
    using System.Data;
    using System.Data.SqlClient;

    public class CandidateDataAccess : ICandidateDataAccess
    {
        private readonly string _connectionString;
        public CandidateDataAccess()
        {
            //_connectionString = ConfigurationManager.ConnectionStrings["applicationDatabase"].ConnectionString;
        }
        public void AddCandidate(Candidate candidate)
        {
            using (var connection = new SqlConnection(_connectionString))
            {
                var command = new SqlCommand
                {
                    Connection = connection,
                    CommandType = CommandType.StoredProcedure,
                    CommandText = "uspAddCandidate"
                };

                var firstNameParameter = new SqlParameter("@Firstname", SqlDbType.VarChar, 50) { Value = candidate.Firstname };
                command.Parameters.Add(firstNameParameter);
                var surnameParameter = new SqlParameter("@Surname", SqlDbType.VarChar, 50) { Value = candidate.Surname };
                command.Parameters.Add(surnameParameter);
                var dateOfBirthParameter = new SqlParameter("@DateOfBirth", SqlDbType.DateTime) { Value = candidate.DateOfBirth };
                command.Parameters.Add(dateOfBirthParameter);
                var emailAddressParameter = new SqlParameter("@EmailAddress", SqlDbType.VarChar, 50) { Value = candidate.EmailAddress };
                command.Parameters.Add(emailAddressParameter);
                var requireCreditCheckParameter = new SqlParameter("@RequireCreditCheck", SqlDbType.Bit) { Value = candidate.RequireCreditCheck };
                command.Parameters.Add(requireCreditCheckParameter);
                var creditParameter = new SqlParameter("@Credit", SqlDbType.Int) { Value = candidate.Credit };
                command.Parameters.Add(creditParameter);
                var positionIdParameter = new SqlParameter("@PositionId", SqlDbType.Int) { Value = candidate.Position.Id };
                command.Parameters.Add(positionIdParameter);

                connection.Open();
                command.ExecuteNonQuery();
            }
        }
    }
}
