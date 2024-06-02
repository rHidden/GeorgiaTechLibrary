using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Webshop.User.Application.Contracts.Persistence;
using Webshop.Data.Persistence;

namespace Webshop.User.Persistence
{
    /// <summary>
    /// The purpose of this class is to save the User to a JSON file that can be comitted to github
    /// </summary>
    public class UserRepositoryJSON : BaseRepository, IUserRepository
    {
        private readonly string foldername;
        private readonly string filename;
        private List<Domain.AggregateRoots.User> Users;
        public UserRepositoryJSON(DataContext dataContext) : base("Users.json", dataContext)
        {
            foldername = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "database");
            filename = Path.Combine(foldername, this.TableName);
            EnsureFolder();
            this.Users = new List<Domain.AggregateRoots.User>();
            LoadUsersFromFile();
        }

        public async Task CreateAsync(Domain.AggregateRoots.User entity)
        {
            entity.Id = CreateNextId();
            this.Users.Add(entity);
            await Task.Run(()=> this.SaveUsersToFile());
        }

        public async Task DeleteAsync(int id)
        {
            var existingUser = this.Users.FirstOrDefault(x=>x.Id == id);
            if(existingUser != null)
            {
                this.Users.Remove(existingUser);
            }
            await Task.Run(()=> SaveUsersToFile());
        }

        public async Task<IEnumerable<Domain.AggregateRoots.User>> GetAll()
        {
            return await Task.Run(()=>this.Users);
        }

        public async Task<Domain.AggregateRoots.User> GetById(int id)
        {
            return await Task.Run(()=>this.Users.FirstOrDefault(x=> x.Id == id));
        }

        public async Task UpdateAsync(Domain.AggregateRoots.User entity)
        {
            //updating is the same as removing old and adding new
            await DeleteAsync(entity.Id);
            //do not call create, it creates a new id, just add it to the list
            this.Users.Add(entity);            
            await Task.Run(()=>SaveUsersToFile());
        }

        #region Load and Save from JSON file
        private void LoadUsersFromFile()
        {
            if (File.Exists(this.filename))
            {
                string data = System.IO.File.ReadAllText(this.filename);
                this.Users = JsonConvert.DeserializeObject<List<Domain.AggregateRoots.User>>(data);
            }
        }

        private void SaveUsersToFile()
        {
            string data = JsonConvert.SerializeObject(this.Users, Formatting.Indented);
            System.IO.File.WriteAllText(this.filename, data);
        }

        private void EnsureFolder()
        {            
            DirectoryInfo dir = Directory.CreateDirectory(this.foldername);
            if(!dir.Exists)
            {
                dir.Create();
            }
        }

        private int CreateNextId()
        {
            LoadUsersFromFile();
            if (this.Users.Count > 0)
            {
                return this.Users.Max(x => x.Id) + 1;
            }
            else
            {
                return 1;
            }
        }
        #endregion
    }
}
