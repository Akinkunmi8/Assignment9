using Assignment9.Context;
using Assignment9.Entities;
using Assignment9.ResourceParameter;

namespace Assignment9.Services
{
    public class Assignment9Repo : IAssignment9Repo
    {
        private readonly Assignment9Context _context;
        public Assignment9Repo(Assignment9Context context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public void AddCourse(Guid authorId, Course course)
        {
            if (authorId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(authorId));
            }

            if (course == null)
            {
                throw new ArgumentNullException(nameof(course));
            }
            // the AuthorId is set to the passed-in authorId
            course.AuthorId = authorId;
            _context.Courses.Add(course);

        }

        public void DeleteCourse(Course course)
        {
            _context.Courses.Remove(course);
        }

        public Course GetCourse(Guid authorId, Guid courseId)
        {
            if (authorId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(authorId));
            }

            if (courseId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(courseId));
            }

            return _context.Courses
              .Where(c => c.AuthorId == authorId && c.Id == courseId).FirstOrDefault();
        }

        public IEnumerable<Course> GetCourses(Guid authorId)
        {
            if (authorId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(authorId));
            }

            return _context.Courses
                        .Where(c => c.AuthorId == authorId)
                        .OrderBy(c => c.Title).ToList();
        }

        public void UpdateCourse(Course course)
        {
            //throw new NotImplementedException();
        }

        // for the author implementation
        public void AddAuthor(Author author)
        {
            if (author == null)
            {
                throw new ArgumentNullException(nameof(author));
            }

            // the repository fills the id (instead of using identity columns)
            author.Id = Guid.NewGuid();

            foreach (var course in author.Courses)
            {
                course.Id = Guid.NewGuid();
            }

            _context.Authors.Add(author);
        }

        public bool AuthorExists(Guid authorId)
        {
            if (authorId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(authorId));
            }

            return _context.Authors.Any(a => a.Id == authorId);
        }

        public void DeleteAuthor(Author author)
        {
            if (author == null)
            {
                throw new ArgumentNullException(nameof(author));
            }

            _context.Authors.Remove(author);
        }

        public Author GetAuthor(Guid authorId)
        {
            if (authorId == Guid.Empty)
            {
                throw new ArgumentNullException(nameof(authorId));
            }

            return _context.Authors.FirstOrDefault(a => a.Id == authorId);
        }

        public IEnumerable<Author> GetAuthors()
        {
            return _context.Authors.ToList<Author>();
        }
        public IEnumerable<Author> GetAuthors(AuthorResourcesParameter authorResourcesParameter)
        {
            if (authorResourcesParameter == null)
            {
                throw new ArgumentNullException(nameof(authorResourcesParameter));
            }

            if (string.IsNullOrWhiteSpace(authorResourcesParameter.MainCategory)
                && string.IsNullOrWhiteSpace(authorResourcesParameter.SearchQuery))
            {
                return GetAuthors();
            }
            var collection = _context.Authors as IQueryable<Author>;
            // for mainCategory
            if (!string.IsNullOrWhiteSpace(authorResourcesParameter.MainCategory))
            {
                var mainCategory = authorResourcesParameter.MainCategory.Trim();
                collection = collection.Where(a => a.MainCategory == mainCategory);

            }

            // for searchQuery
            if (!string.IsNullOrWhiteSpace(authorResourcesParameter.SearchQuery))
            {
               var searchQuery = authorResourcesParameter.SearchQuery.Trim();
                collection = collection.Where(a => a.MainCategory.Contains(searchQuery)
                    || a.FirstName.Contains(searchQuery)
                    || a.LastName.Contains(searchQuery));
            }
            return collection.ToList();
            
        }

        public IEnumerable<Author> GetAuthors(IEnumerable<Guid> authorIds)
        {
            if (authorIds == null)
            {
                throw new ArgumentNullException(nameof(authorIds));
            }

            return _context.Authors.Where(a => authorIds.Contains(a.Id))
                .OrderBy(a => a.FirstName)
                .OrderBy(a => a.LastName)
                .ToList();
        }

        public void UpdateAuthor(Author author)
        {
            //throw new NotImplementedException();
        }

        public bool Save()
        {
            return (_context.SaveChanges() >= 0);
        }
    }
}
