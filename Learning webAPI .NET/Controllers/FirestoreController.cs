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

            List<Users> values = new List<Users>();
            foreach (DocumentSnapshot document in snapshot.Documents)
            {
                string email = document.GetValue<string>("email");
                string username = document.GetValue<string>("username");
                DateTime dateRegister = document.GetValue<DateTime>("date_register");

                values.Add(new Users(dateRegister,email,username));
            }

            return new JsonResult(values);
        }



        // GET api/<DummyController>/5
        [HttpGet("{id}")]
        public async Task<string> Get(int id)
        {
            DocumentReference docRef = db.Collection("dummy").Document(id.ToString());
            DocumentSnapshot snapshot = await docRef.GetSnapshotAsync();

            string email = snapshot.GetValue<string>("email");
            string username = snapshot.GetValue<string>("username");
            DateTime dateRegister = snapshot.GetValue<DateTime>("date_register");

            return $"{email} {username} {dateRegister.ToString()}";
        }

        // POST api/<DummyController>
        [HttpPost]
        public async Task Post([FromBody] Users dummy)
        {
            DocumentReference docRef = db.Collection("dummy").Document();
            Dictionary<string, object> data = new Dictionary<string, object>
            {
                {"email", dummy.Email ?? "null"},
                {"username", dummy.Username ?? "null"},
                {"date_register", dummy.DateRegister}
            };
            await docRef.SetAsync(data);
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
