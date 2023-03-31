using Google.Cloud.Firestore;
using Microsoft.AspNetCore.Mvc;

namespace Learning_webAPI_.NET.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FirestoreController : ControllerBase
    {
        private FirestoreDb db;

        public FirestoreController()
        {
            string path = Path.Combine(Environment.CurrentDirectory, "credentials.json");
            Environment.SetEnvironmentVariable("GOOGLE_APPLICATION_CREDENTIALS", path);

            FirestoreDb db = FirestoreDb.Create("shuusan-tutorial");

            this.db = db;
        }

        // GET: api/<FirestoreController>
        [HttpGet]
        public async Task<IActionResult> Get()
        {
            // Query the dummy collection and return the email, username, and date register fields
            QuerySnapshot snapshot = await db.Collection("dummy").GetSnapshotAsync();

            List<object> values = new List<object>();
            foreach (DocumentSnapshot document in snapshot.Documents)
            {
                string email = document.GetValue<string>("email");
                string username = document.GetValue<string>("username");
                DateTime dateRegister = document.GetValue<DateTime>("date_register");

                values.Add(new
                {
                    email = email,
                    username = username,
                    date_register = dateRegister.ToString()
                });
            }

            return new JsonResult(values);
        }



        // GET api/<DummyController>/shuusan
        [HttpGet("{nameOrEmail}")]
        public async Task<IActionResult> Get(string nameOrEmail)
        {
            // Query the dummy collection and filter by name and/or email
            Query query = db.Collection("dummy");
            List<Users> values = new List<Users>();

            values.AddRange(await getByUsernameEmailQuery(query.WhereEqualTo("username", nameOrEmail)));
            values.AddRange(await getByUsernameEmailQuery(query.WhereEqualTo("email", nameOrEmail)));

            return new JsonResult(values);
        }

        private async Task<List<Users>> getByUsernameEmailQuery(Query query)
        {
            List<Users> returnValues = new List<Users>();

            QuerySnapshot snapshot = await query.GetSnapshotAsync();
            foreach (DocumentSnapshot document in snapshot.Documents)
            {
                string email = document.GetValue<string>("email");
                string username = document.GetValue<string>("username");
                DateTime dateRegister = document.GetValue<DateTime>("date_register");

                returnValues.Add(new Users(dateRegister, email, username));
            }

            return returnValues;
        }


        // PUT api/<DummyController>/5
        [HttpPut("{id}")]
        public async Task Put(int id, [FromBody] Users dummy)
        {
            DocumentReference docRef = db.Collection("dummy").Document(id.ToString());
            Dictionary<string, object> data = new Dictionary<string, object>
            {
                {"email", dummy.Email ?? "null"},
                {"username", dummy.Username ?? "null"},
                {"date_register", dummy.DateRegister}
            };
            await docRef.SetAsync(data);
        }

        // DELETE api/<DummyController>/5
        [HttpDelete("{id}")]
        public async Task Delete(int id)
        {
            DocumentReference docRef = db.Collection("ASP.NET").Document(id.ToString());
            await docRef.DeleteAsync();
        }
    }
}
