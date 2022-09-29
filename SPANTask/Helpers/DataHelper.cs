using SPANTask.Entities;
using SPANTask.Enums;
using System.Data.SqlClient;

namespace SPANTask.Helpers
{
    public class DataHelper : SQLConnectionHelper
    {
        private readonly IConfiguration _config;
        private readonly ConvertHelper _convertHelper;
        private readonly ValidateZipHelper _validateZipHelper;
        public DataHelper(string connectionString, IConfiguration config) : base(connectionString)
        {
            _config = config;
            _convertHelper = new ConvertHelper();
            _validateZipHelper = new ValidateZipHelper();
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
                person.IsZipValid = _validateZipHelper.Validate(columns[2]);
                persons.Add(person);
            }

            return persons;
        }

        public int SaveData(List<Person> people)
        {
            try
            {
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
 
                Connection.Open(); 
                var retrunValue = command.ExecuteNonQuery();

                int resultId = 0;

                if (retrunValue > 0 && retrunValue < people.Count)
                {
                    resultId = (int)SavePeopleMessageType.NotAllSaved;
                }
                else if (retrunValue == 0)
                {
                    resultId = (int)SavePeopleMessageType.NoneSaved;
                }
                else
                {
                    resultId = (int)SavePeopleMessageType.AllSaved;
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
