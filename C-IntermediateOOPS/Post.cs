using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace C_Intermediate
{
    class Post
    {
        // title,description,currentTime
        public string Title { get; set; }
        public string Description { get; set; }
        public DateTime CreatedAt { get; set; }

        public Post()
        {

            CreatedAt = DateTime.Today;
        }
        public int Vote { get; set; }
        public void UpVote()
        {
            Vote++;
        }
        public void DownVote()
        {
            Vote--;
        }
        public int GetVote
        {
            get
            {
                return Vote;
            }
        }
    }
}
