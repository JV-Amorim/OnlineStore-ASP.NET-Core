using System.Linq;
using Microsoft.EntityFrameworkCore;
using OnlineStore.Models;
using OnlineStore.Repositories.Interfaces;
using OnlineStore.Database;
using X.PagedList;

namespace OnlineStore.Repositories
{
    public class CollaboratorRepository : ICollaboratorRepository
    {
        private OnlineStoreContext database;

        public CollaboratorRepository(OnlineStoreContext database) => this.database = database;

        public void Register(Collaborator collaborator)
        {
            database.Add(collaborator);
            database.SaveChanges();
        }

        public void Update(Collaborator collaborator)
        {
            database.Update(collaborator);
            database.SaveChanges();
        }

        public Collaborator GetCollaborator(int id) => database.Collaborators.Find(id);

        public IPagedList<Collaborator> GetAllCollaborators(int? page, int pageSize) =>
            database.Collaborators.ToPagedList<Collaborator>(page ?? 1, pageSize);
        
        public Collaborator GetCollaboratorByEmail(string emailAddress)
        {
            return
                database.Collaborators
                .Where(c => c.Email == emailAddress)
                .AsNoTracking()
                .FirstOrDefault();
        }

        public void Delete(int id)
        {
            Collaborator item = GetCollaborator(id);
            database.Remove(item);
            database.SaveChanges();
        }

        public Collaborator Login(string email, string password)
        {
            return 
                database.Collaborators
                .Where(item => item.Email == email && item.Password == password)
                .FirstOrDefault();
        }
    }
}