using SPANTask.Entities;
using System.Data.SqlClient;

namespace SPANTask.Helpers
{
    public class DataHelper : SQLConnectionHelper
    {
        private readonly IConfiguration _config;
        private readonly ConvertHelper _convertHelper;
        public DataHelper(string connectionString, IConfiguration config) : base(connectionString)
        {
            _config = config;
            _convertHelper = new ConvertHelper();
        }

        public IEnumerable<Person> LoadData()
        {
            var persons = new List<Person>();

            string path = _config.GetSection("Files")["PodaciCSV"];

            string[] lines = File.ReadAllLines(path);
            foreach (string line in lines)
            {
                string[] columns = line.Split(';');
                var person = new Person();
                person.Name = columns[0];
                person.Surname = columns[1];
                person.ZipCode = columns[2];
                person.City = columns[3];
                person.PhoneNumber = columns[4];
                person.IsZipValid = int.TryParse(columns[2], out _);
                persons.Add(person);
            }

            return persons;
        }

        public int? SaveData(List<Person> people)
        {
            try
            {
                //Creating SqlCommand object
                string commandString = "AddPodaciItems";

                var dtValidPeople = _convertHelper
                                        .ConvertToDataTable(people.Where(x => x.IsZipValid)
                                        .Select(x =>
                                            new
                                            {
                                                x.Name,
                                                x.Surname,
                                                x.ZipCode,
                                                x.City,
                                                x.PhoneNumber
                                            }));

                SqlCommand command = new SqlCommand(commandString, Connection);
                command.CommandType = System.Data.CommandType.StoredProcedure;
                SqlParameter tvpParam = command.Parameters.AddWithValue("@PodaciItems", dtValidPeople);
                tvpParam.SqlDbType = System.Data.SqlDbType.Structured;

                // Opening Connection  
                Connection.Open();
                // Executing the SQL command  
                var retrunValue = command.ExecuteNonQuery();

                int? resultId = null;

                if (retrunValue > 0 && retrunValue < people.Count)
                {
                    resultId = 1;
                }
                else if (retrunValue == 0)
                {
                    resultId = 2;
                }
                else
                {
                    resultId = 3;
                }

                return resultId;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                Connection.Close();
            }
        }
    }
}
